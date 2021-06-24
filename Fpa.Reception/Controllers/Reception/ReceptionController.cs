using Domain.Interface;
using Microsoft.AspNetCore.Mvc;
using reception.fitnesspro.ru.Controllers.Reception.Converter;
using reception.fitnesspro.ru.Controllers.Reception.ViewModel;
using reception.fitnesspro.ru.Misc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Application.Extensions;

namespace reception.fitnesspro.ru.Controllers.Reception
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class ReceptionController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public ReceptionController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var result = context.Reception.Get();

                if (result == default) return NoContent();

                var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }

        [HttpGet]
        [Route("GetByKey")]
        public async Task<ActionResult> GetByKey(Guid receptionKey)
        {
            if (receptionKey == default)
            {
                ModelState.AddModelError(nameof(receptionKey), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Reception.GetByKey(receptionKey);

                if (result == default) return NoContent();

                var viewmodel = ReceptionViewModelConverter.ConvertDomainViewModel(result);

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateReception(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            try
            {
                var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

                var result = await context.Reception.Create(item);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("MakePositionFree")]
        public async Task<ActionResult> MakePositionFree(Guid positionKey)
        {
            if (positionKey == default) return BadRequest(nameof(positionKey));

            try
            {
                var reception = await context.Reception.GetByPosition(positionKey);

                if (reception == default) return NoContent();

                var position = reception?.PositionManager.Positions.FirstOrDefault(x => x.Key == positionKey);

                if (position == default || position.Record == default) return NoContent();

                var disciplineKey = position.Record.DisciplineKey;
                var programKey = position.Record.ProgramKey;
                var studentKey = position.Record.StudentKey;

                position.Record = null;

                await context.Reception.Update(reception);


                var studentSetting = await context.Setting.GetStudentSetting(studentKey);

                if (studentSetting == default)
                {
                    var constraints = context.Setting.Find(programKey, disciplineKey).FirstOrDefault();

                    if (constraints != default)
                    {
                        studentSetting = new Domain.Model.StudentSetting(studentKey);
                        studentSetting.AddDiscipline(constraints.DisciplineKey, constraints.SignUpBeforeMinutes, constraints.SignOutBeforeMinutes, null);

                        studentSetting.SubtractSignOutAttempt(disciplineKey);

                        await context.Setting.AddStudentSetting(studentSetting);
                    }
                }
                else
                {
                    if (studentSetting.IsDisciplineSettingExists(disciplineKey) == false)
                    {
                        var constraints = context.Setting.Find(programKey, disciplineKey).FirstOrDefault();

                        if (constraints != default)
                        {
                            studentSetting.AddDiscipline(constraints.DisciplineKey, constraints.SignUpBeforeMinutes, constraints.SignOutBeforeMinutes, null);
                        }
                        else
                        {
                            studentSetting.AddDiscipline(disciplineKey, 5, 5, null);
                        }
                    }

                    studentSetting.SubtractSignOutAttempt(disciplineKey);
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
        [Route("GetReceptionsForPeriod")]
        public async Task<ActionResult<IEnumerable<Domain.Reception>>> GetReceptionsForPeriod(Guid? employeeKey,
            Guid? disciplineKey, DateTime fromDate, DateTime toDate)
        {
            try
            {
                var currentYear = DateTime.Now.Year;
                var currentMonth = DateTime.Now.Month;

                if (fromDate == default) fromDate = new DateTime(currentYear, currentMonth, 1);
                if (toDate == default) toDate = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));
                if (toDate < fromDate)
                {
                    var temp = toDate;
                    toDate = fromDate;
                    fromDate = temp;
                }

                var receptions = await context.Reception.GetForPeriod(employeeKey, disciplineKey, fromDate, toDate);

                if (receptions.IsNullOrEmpty()) return NoContent();

                return receptions.ToList();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }

        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteReception(Guid receptionKey)
        {
            try
            {
                var reception = context.Reception.GetByKey(receptionKey);

                if (reception == default) return NoContent();

                context.Reception.Delete(receptionKey);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<ActionResult> UpdateReception(Domain.Reception model)
        {
            if (model == default || model.Key == default) return BadRequest(nameof(model));

            try
            {
                await context.Reception.Update(model);

                return Ok();
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("ChangeDate")]
        public async Task<ActionResult> ChangeReceptionDate(ChangeDateViewModel viewModel)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var reception = context.Reception.GetByKey(viewModel.ReceptionKey);
                if (reception == default) return NoContent();

                reception.ChangeData(viewModel.Date);

                await context.Reception.Update(reception);

                return Ok();
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        #region OLD implementation

        [HttpPost]
        [Obsolete]
        public async Task<ActionResult> Post(CreateReceptionViewModel model)
        {
            if (ModelState.IsValid == false) return BadRequest(model);

            try
            {
                var item = new Domain.Reception().ConvertFromType(ReceptionViewModelConverter.ConvertViewModelToDomain, model);

                var result = await context.Reception.Create(item);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }

        [HttpGet]
        [Route("FindByDiscipline")]
        [Obsolete]
        public async Task<ActionResult> FindByDiscipline(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var result = await context.Reception.GetByDisciplineKey(key);

                if (result == default) return NoContent();

                var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }

        }

        [HttpGet]
        [Obsolete]
        public ActionResult Get()
        {
            try
            {
                var result = context.Reception.Get();

                if (result == default) return NoContent();

                var viewmodel = result.Select(x => ReceptionViewModelConverter.ConvertDomainViewModel(x));

                return Ok(viewmodel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        #endregion
    }

}
