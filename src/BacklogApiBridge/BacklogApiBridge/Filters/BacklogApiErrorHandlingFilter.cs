using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BacklogApiBridge.Filters
{
    public class BacklogApiErrorHandlingFilter: IActionFilter, IOrderedFilter
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is Refit.ApiException exception)
            {
                context.Result = new ObjectResult(new
                {
                    exception.Message
                })
                {
                    StatusCode = (int)exception.StatusCode
                };
                context.ExceptionHandled = true;
            }
        }

    }
}
