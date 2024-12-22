using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MVC.Attributes
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToActionResult("Login", "Users", null);
                return;
            }

            var isAdmin = context.HttpContext.User.IsInRole("Admin");
            if (!isAdmin)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}