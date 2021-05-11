using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace reception.fitnesspro.ru.Misc
{

    public class ResourseLoggingFilter : Attribute, IActionFilter
    {
        ILogger _logger;
        public ResourseLoggingFilter(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ControllerLogger");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult)
            {
                var result = ((ObjectResult)context.Result).Value;

                _logger.LogDebug("Получен результат запроса {@Result}", result);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var arguments = context.ActionArguments;

            _logger.LogInformation("Получен запрос с аргументами - {@Argument}", arguments);
        }
    }
}
