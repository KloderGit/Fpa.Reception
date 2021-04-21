using Domain.Interface;
using Domain.Model.Education;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Student.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace reception.fitnesspro.ru.Controllers.Student
{
    [Route("[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IAppContext context;

        public StudentController(IAppContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetEducation")]
        public async Task<ActionResult<Domain.Education.Program>> GetEducation(Guid programKey)
        {
            var program = await context.Student.GetStudentEducation(programKey);

            if (program == default) return NoContent();

            return program;
        }

        [HttpGet]
        [Route("GetHistory")]
        public async Task<ActionResult<dynamic>> GetHistory(Guid studentKey)
        { 
                var receptions = await context.Student.GetReceptionsWithSignedUpStudent(studentKey);

            var positions = receptions.Select(x=> new { 
                    Date = x.Date, 
                    Time = x.PositionManager.GetSignedUpStudentPosition(studentKey)?.Time,
                    Program =x.PositionManager.GetSignedUpStudentPosition(studentKey)?.Record?.ProgramKey,
                    Discipline =x.PositionManager.GetSignedUpStudentPosition(studentKey)?.Record?.DisciplineKey,
                    Result = x.PositionManager.GetSignedUpStudentPosition(studentKey)?.Record?.Result.Comment
                });

            return positions.ToList();
        }


        [HttpGet]
        [Route("GetSchedule")]
        public async Task<ActionResult<IEnumerable<DisciplineReceptionViewModel>>> GetProgramReceptions(Guid studentKey, Guid disciplineKey)
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

            return viewModel;

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

            var reception = await context.Reception.GetByPosition(model.PositionKey);
            if(reception == default) return NotFound("Рецепция с такой позицией не найдена");

            var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);
            if (position == default) return NotFound("Позиция не найдена");

            var result = new Domain.Result(){ TeacherKey = model.TeacherKey, RateKey = model.RateKey, Comment = model.Comment };

            position.Record.Result = result;

            await context.Reception.Update(reception);

            return Ok();
        }

        [HttpPost]
        [Route("SignUp")]
        public async Task<ActionResult> SignUp([FromBody] SignUpViewModel model)
        {
            var reception = await context.Reception.GetByPosition(model.PositionKey);

            if(reception == default) return NoContent();

            var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);

            if (position == default) return NotFound(nameof(model.PositionKey));

            position.Record = new Domain.Record { DisciplineKey = model.DisciplineKey, ProgramKey = model.ProgramKey, StudentKey = model.StudentKey };

            await context.Reception.Update(reception);

            return Ok();
        }

        #region OLD

        [HttpGet]
        [Route("GetReceptions")]
        [Obsolete]
        public async Task<ActionResult<IEnumerable<DisciplineReceptionViewModel>>> GetProgramReceptionsOld(Guid studentKey, Guid disciplineKey)
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

            return viewModel;

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
