using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Api.Filters
{
    public class LogAttribute : ActionFilterAttribute
    {
        private readonly IActivityLogger _activityLogger;
        private readonly ILogger<LogAttribute> _logger;

        public LogAttribute(IActivityLogger activityLogger, ILogger<LogAttribute> logger)
        {
            _activityLogger = activityLogger;
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userName = context.HttpContext.User.Identity.Name ?? "Anonymous";
            var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName ?? context.ActionDescriptor.DisplayName;
            _activityLogger.Log(_logger, userName, actionName, $"User {userName} executing action {actionName}");
        }
    }
}
