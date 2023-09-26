using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Task9University.Middleware;

public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger _logger;
    public OperationCancelledExceptionFilter(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<OperationCancelledExceptionFilter>();
    }

    public override void OnException(ExceptionContext context)
    {
        if(context.Exception is OperationCanceledException)
        {
            _logger.LogError("Request was cancelled");
            context.ExceptionHandled = true;
            context.Result = new StatusCodeResult(400);
        }
    }
}
