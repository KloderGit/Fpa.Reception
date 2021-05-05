using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

namespace reception.fitnesspro.ru.Misc
{
    public class ScoppedSerilogMiddleware
    {
        static string ApplicationName = Assembly.GetExecutingAssembly().GetName().Name;

        private readonly RequestDelegate _next;

        public ScoppedSerilogMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (LogContext.PushProperty("Service-Name", ApplicationName, false))
            {
                if (context.Request.Headers.Any(x => x.Key == "Aggregate-Request-Id"))
                {
                    var id = context.Request.Headers.FirstOrDefault(x => x.Key == "Aggregate-Request-Id").Value.FirstOrDefault().ToString();

                    using (LogContext.PushProperty("Aggregate-Request-Id", id, false))
                    {
                        await _next.Invoke(context);
                    }
                }
                else
                {
                    await _next.Invoke(context);
                }
            }
        }
    }
}
