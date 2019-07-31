using DtoLayer.Dto;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Authors.Extensions
{
    public class LoginControlAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SessionData<AuthorDto>.SessionValue == null || SessionData<AuthorDto>.SessionValue.Id <= 0)
            {
                filterContext.HttpContext.Response.Redirect("/information");
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
