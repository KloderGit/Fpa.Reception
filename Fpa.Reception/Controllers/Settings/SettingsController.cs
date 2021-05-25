using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.Interface;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using reception.fitnesspro.ru.Misc;

namespace reception.fitnesspro.ru.Controllers.Settings
{
    [Route("[controller]")]
    [TypeFilter(typeof(ResourseLoggingFilter))]
    [TypeFilter(typeof(LoggedResultFilterAttribute))]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly IAppContext context;
        private readonly ILogger logger;

        public SettingsController(IAppContext context, ILoggerFactory loggerFactory)
        {
            this.context = context;
            this.logger = loggerFactory.CreateLogger(this.ToString());
        }

        [HttpPost]
        [Route("Disciplines")]
        public async Task<ActionResult> AddDisciplineSettings(BaseConstraint model)
        {
            if (model.Validate() != true) return BadRequest("Для ограничения не указана дисциплина");

            try
            {
                var result = context.Setting.Store(model);

                return Ok(result.ToString());
            }
            catch (ArgumentException e)
            { 
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("Disciplines/Update")]
        public async Task<ActionResult> EditDisciplineSettings(BaseConstraint model)
        {
            if (model.Key == default) return BadRequest("Не указан ключ");

            try
            {
                context.Setting.Update(model);
                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("Disciplines")]
        public async Task<ActionResult<IEnumerable<Domain.BaseConstraint>>> GetAllDisciplineSettings()
        {
            try
            {
                var result = context.Setting.GetAll();

                if (result == default) return NoContent();

                return Ok(result.ToList());
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("Disciplines/GetByKeys")]
        public async Task<ActionResult<IEnumerable<Domain.BaseConstraint>>> GetDisciplineSettingsByKeys(IEnumerable<Guid> constraintKeys)
        {
            if (constraintKeys == default)
            {
                ModelState.AddModelError(nameof(constraintKeys), "Ключи запроса не указаны");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Setting.Get(constraintKeys);

                if (result == default) return NoContent();

                return Ok(result.ToList());
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("Disciplines/GetByKey")]
        public async Task<ActionResult<Domain.BaseConstraint>> GetDisciplineSettingsByKey(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указаны");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Setting.GetByKey(key);

                if (result == default) return NoContent();

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("Disciplines/Find")]
        public async Task<ActionResult<Domain.BaseConstraint>> FindDisciplineSettings(Guid? programKey, Guid disciplineKey)
        {
            if (disciplineKey == default)
            {
                ModelState.AddModelError(nameof(disciplineKey), "Ключ дисциплины не указан");
                return BadRequest(ModelState);
            }

            try
            {
                var result = context.Setting.Find(programKey, disciplineKey);

                if (result == default) return NoContent();

                return Ok(result.First());
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }



        [HttpGet]
        [Route("Teachers/GetAllFromSchedule")]
        public async Task<ActionResult> GetAllScheduleTeachers()
        {
            try
            {
                var result = await context.Setting.GetAllScheduleTeachers();

                var viewModel = result.Select(x => new GetAllTeacherSettingViewModel
                {
                    Id = x.Item1,
                    Title = x.Item2
                });

                return Ok(viewModel);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("Teachers/GetAllFromService")]
        public async Task<ActionResult> GetAllServiceTeachers()
        {
            try
            {
                var result = await context.Setting.GetAllServiceTeachers();

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        [HttpGet]
        [Route("Teachers")]
        public async Task<ActionResult> GetAllTeacherSettings()
        {
            try
            {
                var result = await context.Setting.GetAllTeacherSettings();

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("Teachers")]
        public async Task<ActionResult> AddTeacherSettings(AddTeacherSettingViewModel viewModel)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var model = viewModel.Adapt<TeacherSetting>();

                var result = await context.Setting.AddTeacherSettings(model);

                return Ok(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }

        [HttpPost]
        [Route("Teachers/Update")]
        public async Task<ActionResult> UpdateTeacherSettings(UpdateTeacherSettingViewModel viewModel)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            try
            {
                var model = viewModel.Adapt<TeacherSetting>();

                await context.Setting.UpdateTeacherSettings(model);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }


        [HttpGet]
        [Route("Teachers/Delete")]
        public async Task<ActionResult> DeleteTeacherSettings(Guid key)
        {
            if (key == default)
            {
                ModelState.AddModelError(nameof(key), "Ключ запроса не указаны");
                return BadRequest(ModelState);
            }

            try
            {
                await context.Setting.DeleteTeacherSettings(key);

                return Ok();
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "При выполнении запроса произошла ошибка - {@Error}", e.Message, e);
                return new StatusCodeResult(500);
            }
        }
    }

}
