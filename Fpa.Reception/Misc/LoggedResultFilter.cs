using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace reception.fitnesspro.ru.Misc
{
    public class LoggedResultFilterAttribute : Attribute, IResultFilter
    {
        ILogger _logger;

        public LoggedResultFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("ControllerLogger");
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {


        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.Result is BadRequestObjectResult)
            {
                var result = context.Result as BadRequestObjectResult;
                
                _logger.LogWarning("Ошибка. Запрос не выполнен. Сообщение причины - {Message}", result.Value);
                return;
            }

            if (context.Result is ObjectResult)
            {
                var result = ((ObjectResult)context.Result).Value;

                if (result == default)
                {
                    _logger.LogInformation("Нулевой результат запроса");
                }

                _logger.LogInformation("Получен результат запроса");
                _logger.LogDebug("Получен результат запроса {@Result}", result);
            }
        }
    }
}
