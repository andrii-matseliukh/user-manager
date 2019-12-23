using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserManagerInfrastructure.Filters
{
    public class ExceptionLoggingFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionLoggingFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName;
            string message = context.Exception.Message;

            _logger.LogError("Error happened in {actionName}", actionName);
            _logger.LogError("Details: {details}", message);
        }
    }
}
