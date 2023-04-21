using DataConCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DataConApi.Filters;

public class GlobalExceptionsFilter : ExceptionFilterAttribute
{
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<GlobalExceptionsFilter> _logger;

    public GlobalExceptionsFilter(IWebHostEnvironment env, ILogger<GlobalExceptionsFilter> logger)
    {
        _env = env;
        _logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        var status = context.HttpContext.Response.StatusCode;
        if (status >= 500)
        {
            var exception = context.Exception;
            var requestId = System.Diagnostics.Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;
            var eventId = new EventId(exception.HResult, requestId);
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            //string className = descriptor.ControllerName;
            //string method = descriptor.ActionName;
            var hostAndPort = context.HttpContext.Request.Host.HasValue ? context.HttpContext.Request.Host.Value : string.Empty;
            var requestUrl = string.Concat(hostAndPort, context.HttpContext.Request.Path);
            var type = string.Concat("https://httpstatuses.com/", status);

            string title;
            string detial;
            if (exception is IParamsException)
            {
                title = "Parameter error";
                detial = exception.Message;
            }
            else
            {
                title = _env.IsDevelopment() ? exception.Message : $"System error";
                detial = _env.IsDevelopment() ? exception.GetExceptionDetail() : $"System error, please contact the administrator({eventId})";
                _logger.LogError(eventId, exception, exception.Message, requestUrl);
            }

            var problemDetails = new ProblemDetails
            {
                Title = title
                ,
                Detail = detial
                ,
                Type = type
                ,
                Status = status
                ,
                Instance = requestUrl
            };

            context.Result = new ObjectResult(problemDetails) { StatusCode = status };
        }

        context.ExceptionHandled = true;
    }

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }
}
