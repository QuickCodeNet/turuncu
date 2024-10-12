using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace QuickCode.Turuncu.Portal.Helpers.Authorization
{
  public abstract class AttributeAuthorizationHandler<TRequirement, TAttribute> : AuthorizationHandler<TRequirement> where TRequirement : IAuthorizationRequirement where TAttribute : Attribute
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement)
        {
            var attributes = new List<TAttribute>();

            var action = (context.Resource as AuthorizationFilterContext)?.ActionDescriptor as ControllerActionDescriptor;
            if (action != null)
            {
                attributes.AddRange(GetAttributes(action.ControllerTypeInfo.UnderlyingSystemType));
                attributes.AddRange(GetAttributes(action.MethodInfo));
            }

            return HandleRequirementAsync(context, requirement, attributes);
        }

        protected abstract Task HandleRequirementAsync(AuthorizationHandlerContext context, TRequirement requirement, IEnumerable<TAttribute> attributes);

        private static IEnumerable<TAttribute> GetAttributes(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute : AuthorizeAttribute
    {
        public string Name { get; }

        public PermissionAttribute(string name) : base("Permission")
        {
            Name = name;
        }
    }

    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        //Add any custom requirement properties if you have them
    }

    public class PermissionAuthorizationHandler : AttributeAuthorizationHandler<PermissionAuthorizationRequirement, PermissionAttribute>
    {
        public IPortalPermissionManager portalPermissionManager { get; set; }
        public PermissionAuthorizationHandler(IPortalPermissionManager portalPermissionManager)
        {
            this.portalPermissionManager = portalPermissionManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement, IEnumerable<PermissionAttribute> attributes)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                //(context.Resource as AuthorizationFilterContext).Result = new RedirectToActionResult("Index", "Login", null);
                //(context.Resource as PortalPermissionManager).GetHttpContextAccessor().HttpContext.Response.Headers["Location"] = "/Login/Index";
                //(context.Resource as PortalPermissionManager).GetHttpContextAccessor().HttpContext.Response.StatusCode = 200;
                //context.Succeed(requirement);
                //context.Succeed(requirement);
                return ;
            }


            foreach (var permissionAttribute in attributes)
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    //(context.Resource as AuthorizationFilterContext).Result = new RedirectToActionResult("Index", "Login", null);
                    //context.Succeed(requirement);
                    return ;
                }
                else if (!(await AuthorizeAsync(context, context.User, permissionAttribute.Name)))
                {
                    //(context.Resource as AuthorizationFilterContext).Result = new RedirectToActionResult("Index", "Login", null);
                    //context.Succeed(requirement);
                    return ;
                }
            }

            context.Succeed(requirement);
            return ;
        }

        private async Task<bool> AuthorizeAsync(AuthorizationHandlerContext context, ClaimsPrincipal user, string permission)
        {
            var actionName = (context.Resource as AuthorizationFilterContext).ActionDescriptor.RouteValues["action"];
            var controllerName = (context.Resource as AuthorizationFilterContext).ActionDescriptor.RouteValues["controller"];

            if (controllerName.IsIn("Home"))
            {
                return true;
            }

            var result = await portalPermissionManager.GetPagePermission(controllerName, actionName);
            return result.IsPageAvailable;
        }
    }
}
