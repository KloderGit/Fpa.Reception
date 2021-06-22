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
using Domain.Model;
using System.Collections;
using Domain.Infrastructure;

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
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
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

                var positions = receptions.SelectMany(x => x.PositionManager.GetSignedUpStudentPosition(studentKey));

                var programKeys = positions.Where(x => x.Record != default).Select(x => x.Record.ProgramKey).Where(x => x != default);
                var programs = await context.Education.GetProgramsByKeys(programKeys);

                var disciplineKeys = positions.Where(x => x.Record != default).Select(x => x.Record.DisciplineKey).Where(x => x != default);
                var disciplines = await context.Education.GetDisciplinesByKeys(disciplineKeys);

                var teacherKeys = positions.Where(x => x.Record != default).Where(x => x.Record.Result != default).Select(x => x.Record.Result.TeacherKey).Where(x => x != default);
                var teachers = await context.Education.GetTeachers(teacherKeys);

                var rateKeys = positions.Where(x => x.Record != default).Where(x => x.Record.Result != default).Select(x => x.Record.Result.RateKey).Where(x => x != default);
                var rates = await context.Education.GetRates();

                var viewModel = positions
                    .Select(y => new StudentHistoryViewModel
                    {
                        PositionKey = y.Key,
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
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

            BaseInfoViewModel FindByKey(Guid? key, IEnumerable<BaseInfo> array)
            {
                if (array == default) return null;
                if (key == default || key.HasValue == false) return null;
                var item = array.FirstOrDefault(x => x.Key == key.Value);
                var vm = new BaseInfoViewModel { Key = item.Key, Title = item.Title };
                return vm;
            }
        }


        [HttpGet]
        [Route("GetSchedule")]
        public async Task<ActionResult<IEnumerable<DisciplineReceptionViewModel>>> GetProgramReceptions(Guid studentKey, Guid disciplineKey)
        {
            if (studentKey == default) ModelState.AddModelError(nameof(studentKey), "Ключ студента не указан");
            if (disciplineKey == default) ModelState.AddModelError(nameof(disciplineKey), "Ключ дисциплины не указан");

            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var contract = await context.Student.GetContract(studentKey);
                var studentSetting = await context.Setting.GetStudentSetting(studentKey);
                var commonDisciplineSettings = context.Setting.FindCommonSettingsByDiscipline(disciplineKey);

                var allDisciplineReceptions = await context.Student.GetReceptionsForSignUpStudent(studentKey, disciplineKey);
                var filteredReceptionsForContractEducation = allDisciplineReceptions
                    .Where(x => x.IsForProgram(contract.EducationProgram.Key))
                    .Where(x => x.IsForGroup(contract.Group.Key))
                    .Where(x => x.IsForSubGroup(contract.SubGroup.Key));

                var viewModel = filteredReceptionsForContractEducation.Select(x => new DisciplineReceptionViewModel(x)).ToList();

                viewModel.ForEach(x => x.CheckContractExpired(contract, commonDisciplineSettings));
                viewModel.ForEach(x => x.CheckEmptyPlaces());
                viewModel.ForEach(x => x.CheckIsNotInPast());
                viewModel.ForEach(x => x.CheckAllowedDisciplinePeriod(contract)); // ToDo
                viewModel.ForEach(x => x.CheckAttemptsCount(disciplineKey, studentKey, contract, context.Student)); //Todo
                viewModel.ForEach(x => x.CheckDependencies(disciplineKey, studentKey, context.Reception)); // ToDo
                viewModel.ForEach(x => x.CheckSignUpBefore());
                viewModel.ForEach(x => x.CheckSignUpDoubles(disciplineKey, studentKey, context.Student));

                if (viewModel == default) return NoContent();

                return viewModel;
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
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
                if (reception == default) return NotFound("Рецепция с такой позицией не найдена");

                var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);
                if (position == default) return NotFound("Позиция не найдена");

                var result = new Domain.Result() { TeacherKey = model.TeacherKey, RateKey = model.RateKey, Comment = model.Comment };
                if (model.RateKey == default) result = null;

                position.Record.Result = result;

                await context.Reception.Update(reception);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
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

                if (reception == default) return NoContent();

                var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == model.PositionKey);

                if (position == default) return NotFound(nameof(model.PositionKey));

                if (position.Record != default) return BadRequest("Выбранное время занято");

                position.Record = new Domain.Record { DisciplineKey = model.DisciplineKey, ProgramKey = model.ProgramKey, StudentKey = model.StudentKey };

                await context.Reception.Update(reception);

                var studentSetting = await context.Setting.GetStudentSetting(model.StudentKey);

                if (studentSetting == default)
                {
                    var constraints = context.Setting.Find(model.ProgramKey, model.DisciplineKey).FirstOrDefault();

                    if (constraints != default)
                    {
                        studentSetting = new Domain.Model.StudentSetting(model.StudentKey);
                        studentSetting.AddDiscipline(constraints.DisciplineKey, constraints.SignUpBeforeMinutes, constraints.SignOutBeforeMinutes, null);

                        studentSetting.SubtractSignUpAttempt(model.DisciplineKey);

                        await context.Setting.AddStudentSetting(studentSetting);
                    }
                }
                else
                {
                    studentSetting.SubtractSignUpAttempt(model.DisciplineKey);
                    await context.Setting.UpdateStudentSetting(studentSetting);
                }

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        [HttpGet]
        [Route("GetSignedUpPositions")]
        public async Task<ActionResult> StudentsSignedUpPositions(Guid studentKey, Guid disciplineKey, Guid programKey)
        {
            if (studentKey == default) ModelState.AddModelError(nameof(studentKey), "Ключ студента не указан");
            if (disciplineKey == default) ModelState.AddModelError(nameof(disciplineKey), "Ключ дисциплины не указан");
            if (programKey == default) ModelState.AddModelError(nameof(programKey), "Ключ программы не указан");

            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var studentReceptions = await context.Student.GetReceptionsWithSignedUpStudent(studentKey);

                var receptions = studentReceptions.Where(x => x.PositionManager.GetSignedUpStudentPosition(studentKey)
                                                                .Any(p => p.Record.StudentKey == studentKey
                                                                    && p.Record.ProgramKey == programKey
                                                                    && p.Record.DisciplineKey == disciplineKey
                                                                    && p.Record.Result == default));

                if (receptions == default) return NoContent();

                var result = receptions.Select(x =>
                    new
                    {
                        Date = x.Date,
                        Positions = GetPositionWithOutResult(x.PositionManager.GetSignedUpStudentPosition(studentKey))
                    });

                return Ok(result);

                IEnumerable<Position> GetPositionWithOutResult(IEnumerable<Position> positions)
                {
                    return positions.Where(p => p.Record.StudentKey == studentKey
                                              && p.Record.ProgramKey == programKey
                                              && p.Record.DisciplineKey == disciplineKey
                                              && p.Record.Result == default);
                }

            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        [HttpPost]
        [Route("StudentInfo")]
        public async Task<ActionResult> StudentInfo(IEnumerable<Guid> studentsKeys)
        {
            studentsKeys = studentsKeys.Distinct();

            if (studentsKeys == default) ModelState.AddModelError(nameof(studentsKeys), "Ключи студента не указаны");
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            var studentTask = context.Student.GetStudents(studentsKeys);
            var receptionTask = GetReceptions(studentsKeys);
            var contractTask = GetContract(studentsKeys);
            var studentSettingTask = GetStudentSettings(studentsKeys);
            await Task.WhenAll(studentTask, receptionTask, contractTask, studentSettingTask);

            var students = await studentTask;
            var receptions = (await receptionTask).ToList();
            var contracts = await contractTask;
            var studentSettings = await studentSettingTask;

            var programTask = GetPrograms(receptions, studentsKeys);
            var disciplineTask = GetDisciplines(receptions, studentsKeys);
            var groupTask = GetGroups(contracts);
            await Task.WhenAll(programTask, disciplineTask,groupTask);

            var programs = (await programTask).ToList();
            var disciplines = await disciplineTask;
            var groups = await groupTask;
            var controlTypes = await GetControlTypes(receptions, studentsKeys, programs);
            contracts.ToList().ForEach(x => x.Group = groups.FirstOrDefault(g => g.Key == x.Group.Key));

            var viewModel = new StudentRecordInfoViewModel(students, receptions, studentSettings);
            viewModel.FillDisciplines(disciplines);
            viewModel.FillPrograms(programs);
            viewModel.FillContract(contracts);
            viewModel.FillControlType(programs, controlTypes);

            // -- Получить Фл | Не нужно - есть на пред шаге
            // -- Получить студентов  | Не нужно - есть на пред шаге
            //  + Получить договоры
            //  + Получить рецепции для договора\студента. Несколько паралельно
            //  + Получить настройки студента
            // Для каждого договора создать вьюмодель из рецепций
            // Добавить системы контроля

            return Ok(viewModel);


            async Task<IEnumerable<Domain.Education.Program>> GetPrograms(IEnumerable<Domain.Reception> receptions, IEnumerable<Guid> studentsKeys)
            {
                List<Guid> programsKeys = new List<Guid>();

                studentsKeys.ToList().ForEach(x =>
                        {
                            var studentPositions = receptions.SelectMany(p => p.PositionManager.GetSignedUpStudentPosition(x));
                            var keys = studentPositions.Select(k => k.Record.ProgramKey);
                            programsKeys.AddRange(keys);
                        }
                    );

                var programs = await context.Education.GetProgramsByKeys(programsKeys.Distinct());

                return programs;
            }

            async Task<IEnumerable<BaseInfo>> GetGroups(IEnumerable<Domain.Model.Education.Contract> contracts)
            {
                var groupKeys = contracts.Select(x => x.Group.Key).Distinct();

                var groups = await context.Education.GetGroupsByKeys(groupKeys);

                return groups;
            }

            async Task<IEnumerable<Domain.Model.Education.ControlType>> GetControlTypes(IEnumerable<Domain.Reception> receptions, IEnumerable<Guid> studentsKeys, IEnumerable<Domain.Education.Program> programs)
            {
                var controlKeysTuples = new List<Tuple<Guid, Guid>>();

                studentsKeys.ToList().ForEach(x =>
                        {
                            var studentPositions = receptions.SelectMany(p => p.PositionManager.GetSignedUpStudentPosition(x));
                            var keys = studentPositions.Select(k => new Tuple<Guid, Guid>(k.Record.DisciplineKey, k.Record.ProgramKey));
                            controlKeysTuples.AddRange(keys);
                        }
                    );

                var controlKeys = new List<Guid>();

                foreach (var pair in controlKeysTuples)
                {
                    var program = programs?.FirstOrDefault(x=>x.Key == pair.Item2);
                    var controlTypeKey = program.FindControlTypeKey(pair.Item1);

                    if(controlTypeKey.HasValue && controlTypeKey.Value != default) controlKeys.Add(controlTypeKey.Value);
                }

                var controlTypes = await context.Education.GetControlTypesByKeys(controlKeys.Distinct());

                return controlTypes;
            }

            async Task<IEnumerable<BaseInfo>> GetDisciplines(IEnumerable<Domain.Reception> receptions, IEnumerable<Guid> studentsKeys)
            {
                List<Guid> disciplinesKeys = new List<Guid>();

                studentsKeys.ToList().ForEach(x =>
                        {
                            var studentPositions = receptions.SelectMany(p => p.PositionManager.GetSignedUpStudentPosition(x));
                            var keys = studentPositions.Select(k => k.Record.DisciplineKey);
                            disciplinesKeys.AddRange(keys);
                        }
                    );

                var disciplines = await context.Education.GetDisciplinesByKeys(disciplinesKeys.Distinct());

                return disciplines;
            }

            async Task<IEnumerable<Contract>> GetContract(IEnumerable<Guid> studentsKeys)
            {
                var contractTasks = new List<Task<Contract>>();
                studentsKeys.ToList().ForEach(x => contractTasks.Add(context.Student.GetContract(x)));
                await Task.WhenAll(contractTasks);

                var contracts = new List<Contract>();
                foreach (var task in contractTasks)
                {
                    var contract = await task;
                    contracts.Add(contract);
                }

                var result = contracts.Distinct(new KeyEqualityComparer<Contract>());

                return contracts;
            }

            async Task<IEnumerable<Domain.Reception>> GetReceptions(IEnumerable<Guid> studentsKeys)
            {
                var receptionTasks = new List<Task<IEnumerable<Domain.Reception>>>();

                studentsKeys.ToList().ForEach(x => receptionTasks.Add(context.Student.GetReceptionsWithSignedUpStudent(x)));
                await Task.WhenAll(receptionTasks);

                var receptionTasksResult = new List<IEnumerable<Domain.Reception>>();
                foreach (var task in receptionTasks)
                {
                    var reception = await task;
                    receptionTasksResult.Add(reception);
                }

                var receptions = receptionTasksResult.SelectMany(x => x.Select(y => y)).ToList().Distinct(new KeyEqualityComparer<Domain.Reception>());

                return receptions;
            }

            async Task<IEnumerable<StudentSetting>> GetStudentSettings(IEnumerable<Guid> studentsKeys)
            {
                var studentSettingTasks = new List<Task<StudentSetting>>();
                studentsKeys.ToList().ForEach(x => context.Setting.GetStudentSetting(x));
                await Task.WhenAll(studentSettingTasks);

                var studentSettings = new List<StudentSetting>();
                foreach (var task in studentSettingTasks)
                {
                    var studentSetting = await task;
                    studentSettings.Add(studentSetting);
                }

                return studentSettings;
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

                viewModel.ForEach(x => x.CheckContractExpired(contract, null));
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
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

            async Task<Contract> GetStudentContract()
            {
                var contract = await context.Student.GetContract(studentKey);

                return contract;
            }
        }

        #endregion
    }
}
