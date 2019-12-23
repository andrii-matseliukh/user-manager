using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManagerInfrastructure.Logger;

namespace UserManagerInfrastructure.Filters
{
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private IDisposable _userLoggerScope;
        private readonly ILogger _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userScopeDetails = LoggerScopeManager.GetUserScope(context.HttpContext);

            _userLoggerScope = _logger.BeginScope(userScopeDetails);

            await next(); // the actual action

            _userLoggerScope.Dispose();
        }
    }
}
