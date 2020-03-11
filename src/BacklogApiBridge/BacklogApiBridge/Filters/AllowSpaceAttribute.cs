using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

namespace BacklogApiBridge.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AllowSpaceAttribute: ActionFilterAttribute
    {
        public const string DefaultSpaceParametername = "spaceKey";
        public string SpaceParameterName { get; set; } = DefaultSpaceParametername;

        /// <summary>
        /// On Action Executing
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.HttpContext.RequestServices.GetService(typeof(IConfiguration)) is IConfiguration configuration))
            {
                context.Result = new BadRequestResult();
                return;
            }

            var space = context.HttpContext.Request.Query[SpaceParameterName];
            var allowSpaces = configuration.GetSection("AllowBacklogSpaces").Get<string[]>();
            if (allowSpaces == null || allowSpaces.All(s => s != space))
            {
                context.ModelState.AddModelError("invalid space", $"Backlog space:{space} is not allowd");
                context.Result = new BadRequestObjectResult(context.ModelState);
                return;
            }
        }
    }
}
