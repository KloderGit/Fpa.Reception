using System;
using System.Diagnostics;
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
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var arguments = context.ActionArguments;

            foreach (var argument in context.ActionArguments)
            {
                _logger.LogInformation("Получен запрос с аргументами - {Name} - {@Argument}", argument.Key, argument.Value);
            }
        }
    }
}
