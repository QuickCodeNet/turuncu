using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QuickCode.Turuncu.Portal.Models;

namespace QuickCode.Turuncu.Portal.ViewComponents
{
    public class UserOperation : ViewComponent
    {

        public UserOperation()
        {

        }

        public IViewComponentResult Invoke()
        {
            var userDetail = new UserDetail();
            if (HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.Name).Count() > 0)
            {
                userDetail.NameSurname = HttpContext.User.Claims.Where(i => i.Type == ClaimTypes.Name).FirstOrDefault().Value;
                userDetail.ImageUrl = "/images/no_image.png";
            }
            return View(userDetail);
        }

    }


}
