using System.Web.Mvc;
using System.Web.Routing;

namespace Fenit.Toolbox.UserManager.Attributes
{
    public class OnlyAuthenticatedAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        {"controller", "Account"},
                        {"action", "Login"}
                    });
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}