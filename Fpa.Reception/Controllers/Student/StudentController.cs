using Domain;
using Domain.Interface;
using Domain.Model.Education;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Student.ViewModel;
using reception.fitnesspro.ru.Misc;
using reception.fitnesspro.ru.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace reception.fitnesspro.ru.Controllers.Student
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public StudentController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        [HttpGet]
        [Route("GetEducation")]
        public async Task<ActionResult<Domain.Education.Program>> GetEducation(Guid programKey)
        {
            if (programKey == default)
            {
                ModelState.AddModelError(nameof(programKey), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var program = await context.Student.GetStudentEducation(programKey);

                if (program == default) return NoContent();

                return program;
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("GetHistory")]
        public async Task<ActionResult<dynamic>> GetHistory(Guid studentKey)
        {
            if (studentKey == default)
            {
                ModelState.AddModelError(nameof(studentKey), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var receptions = await context.Student.GetReceptionsWithSignedUpStudent(studentKey);

                var positions = receptions.SelectMany(x=>x.PositionManager.GetSignedUpStudentPosition(studentKey));

                var programKeys = positions.Where(x=>x.Record != default).Select(x=>x.Record.ProgramKey).Where(x=>x != default);
                var programs = await context.Education.GetProgramsByKeys(programKeys);

                var disciplineKeys = positions.Where(x=>x.Record != default).Select(x=>x.Record.DisciplineKey).Where(x=>x != default);
                var disciplines = await context.Education.GetDisciplinesByKeys(disciplineKeys);

                var teacherKeys = positions.Where(x=>x.Record != default).Where(x=>x.Record.Result != default).Select(x=>x.Record.Result.TeacherKey).Where(x=>x != default);
                var teachers = await context.Education.GetTeachers(teacherKeys);

                var rateKeys = positions.Where(x=>x.Record != default).Where(x=>x.Record.Result != default).Select(x=>x.Record.Result.RateKey).Where(x=>x != default);
                var rates = await context.Education.GetRates();

                var viewModel = positions
                    .Select(y => new StudentHistoryViewModel
                    {
                        DateTime = y.Time,
                        Program = FindByKey(y?.Record?.ProgramKey, programs),
                        Discipline = FindByKey(y?.Record?.DisciplineKey, disciplines),
                        Teacher = FindByKey(y?.Record?.Result?.TeacherKey, teachers),
                        Rate = FindByKey(y?.Record?.Result?.RateKey, rates),
                        Comment = y?.Record?.Result?.Comment
                    });

                return viewModel.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

            BaseInfoViewModel FindByKey(Guid? key, IEnumerable<BaseInfo> array)
            { 
                if(array == default) return null;
                if(key == default || key.HasValue == false) return null;
                var item = array.FirstOrDefault(x=>x.Key == key.Value);
                var vm = new BaseInfoViewModel{ Key = item.Key, Title = item.Title };
                return vm;
            }
        }


        [HttpGet]
        [Route("GetSchedule")]
        public async Task<ActionResult<IEnumerable<DisciplineReceptionViewModel>>> GetProgramReceptions(Guid studentKey, Guid disciplineKey)
        {
            if (studentKey == default) ModelState.AddModelError(nameof(studentKey), "Ключ студента не указан");
            if (studentKey == default) ModelState.AddModelError(nameof(studentKey), "Ключ дисциплины не указан");

            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var receptions = context.Student.GetReceptionsForSignUpStudent(studentKey,disciplineKey);

                var contract = await GetStudentContract();
            
                var disciplineReceptions = await context.Student.GetReceptionsForSignUpStudent(studentKey, disciplineKey);

                var filtered = disciplineReceptions
                    .Where(x => x.IsForProgram(contract.EducationProgram.Key))
                    .Where(x => x.IsForGroup(contract.Group.Key))
                    .Where(x => x.IsForSubGroup(contract.SubGroup.Key));

                var viewModel = filtered.Select(x => new DisciplineReceptionViewModel(x)).ToList();

                viewModel.ForEach(x => x.CheckContractExpired(contract));
                viewModel.ForEach(x => x.CheckEmptyPlaces());
                viewModel.ForEach(x => x.CheckIsNotInPast());
                viewModel.ForEach(x => x.CheckAllowedDisciplinePeriod(contract));
                viewModel.ForEach(x => x.CheckAttemptsCount(disciplineKey, studentKey, contract, context.Student));
                viewModel.ForEach(x => x.CheckDependencies(disciplineKey, studentKey, context.Reception));
                viewModel.ForEach(x => x.CheckSignUpBefore());
                viewModel.ForEach(x => x.CheckSignUpDoubles(disciplineKey, studentKey, context.Student));

                if (viewModel == default) return NoContent();

                return viewModel;
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
            
            async Task<Contract> GetStudentContract()
            {
                var allStudentContract = await context.Student.GetContracts(studentKey);
                var contract = allStudentContract//.Where(x => x.ExpiredDate > DateTime.Now.Date)
                    .FirstOrDefault(x => x.ExpiredDate == allStudentContract.Max(d => d.ExpiredDate));

                return contract;
            }
        }

        [HttpPost]
        [Route("Result")]
        public async Task<ActionResult> StudentResult(StudentResultViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);
            
            try
            {
                var reception = await context.Reception.GetByPosition(model.PositionKey);
                if(reception == default) return NotFound("Рецепция с такой позицией не найдена");

                var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);
                if (position == default) return NotFound("Позиция не найдена");

                var result = new Domain.Result(){ TeacherKey = model.TeacherKey, RateKey = model.RateKey, Comment = model.Comment };

                position.Record.Result = result;

                await context.Reception.Update(reception);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            try
            {
                var reception = await context.Reception.GetByPosition(model.PositionKey);

                if(reception == default) return NoContent();

                var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);

                if (position == default) return NotFound(nameof(model.PositionKey));

                if (position.Record != default) return BadRequest("Выбранное время занято");

                position.Record = new Domain.Record { DisciplineKey = model.DisciplineKey, ProgramKey = model.ProgramKey, StudentKey = model.StudentKey };

                await context.Reception.Update(reception);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
           
        }

        #region OLD

        [HttpGet]
        [Route("GetReceptions")]
        [Obsolete]
        public async Task<ActionResult<IEnumerable<DisciplineReceptionViewModel>>> GetProgramReceptionsOld(Guid studentKey, Guid disciplineKey)
        {
            if (studentKey == default) ModelState.AddModelError(nameof(studentKey), "Ключ студента не указан");
            if (studentKey == default) ModelState.AddModelError(nameof(studentKey), "Ключ дисциплины не указан");

            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var contract = await GetStudentContract();
            
                var disciplineReceptions = await context.Reception.GetByDisciplineKey(disciplineKey);

                var filtered = disciplineReceptions
                    .Where(x => x.IsForProgram(contract.EducationProgram.Key))
                    .Where(x => x.IsForGroup(contract.Group.Key))
                    .Where(x => x.IsForSubGroup(contract.SubGroup.Key));

                var viewModel = filtered.Select(x => new DisciplineReceptionViewModel(x)).ToList();

                viewModel.ForEach(x => x.CheckContractExpired(contract));
                viewModel.ForEach(x => x.CheckEmptyPlaces());
                viewModel.ForEach(x => x.CheckIsNotInPast());
                viewModel.ForEach(x => x.CheckAllowedDisciplinePeriod(contract));
                viewModel.ForEach(x => x.CheckAttemptsCount(disciplineKey, studentKey, contract, context.Student));
                viewModel.ForEach(x => x.CheckDependencies(disciplineKey, studentKey, context.Reception));
                viewModel.ForEach(x => x.CheckSignUpBefore());
                viewModel.ForEach(x => x.CheckSignUpDoubles(disciplineKey, studentKey, context.Student));

                if (viewModel == default) return NoContent();

                return viewModel;
            }
            catch (Exception e)
            {
                logger.LogWarning(e,"При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

            async Task<Contract> GetStudentContract()
            {
                var allStudentContract = await context.Student.GetContracts(studentKey);
                var contract = allStudentContract//.Where(x => x.ExpiredDate > DateTime.Now.Date)
                    .FirstOrDefault(x => x.ExpiredDate == allStudentContract.Max(d => d.ExpiredDate));

                return contract;
            }
        }

        #endregion
    }
}
