using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MusicSubscriptionApp.Models;

namespace A1T1s3655612.Security
{
    public class RequireSession : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userEmail = filterContext.HttpContext.Session.GetString(nameof(AppUser.Email));

            if (userEmail == null)
            {
                filterContext.Result
                    = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Home",
                        action = "Index"
                    }));
            }

            base.OnActionExecuting(filterContext);
        }
    }
}