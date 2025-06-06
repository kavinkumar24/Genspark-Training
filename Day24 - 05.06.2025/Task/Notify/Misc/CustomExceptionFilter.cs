using Microsoft.AspNetCore.Mvc;
using Notify.Models.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
namespace Notify.Misc;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = new BadRequestObjectResult(new ErrorObject
        {
            ErrorNumber = 500,
            ErrorMessage = context.Exception.Message
        });
    }
}