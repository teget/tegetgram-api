using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Controllers;
namespace Tegetgram.Api.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var exceptionMessage = exception.Message;

            var userName = context.HttpContext.User.Identity.Name ?? "Anonymous";
            var actionName = (context.ActionDescriptor as ControllerActionDescriptor)?.ActionName ?? context.ActionDescriptor.DisplayName;
            _logger.LogError($"user: {userName} | Action: {actionName} | Message: {exceptionMessage}");

            if (exception is ApplicationException)
                context.Result = new BadRequestObjectResult(exceptionMessage);
            else
                context.Result = new JsonResult(new
                {
                    message = "An unexpected error has occured."
                })
                { StatusCode = 500 };

            base.OnException(context);
        }
    }
}
