using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using QuickCode.Turuncu.Common.Nswag;
using QuickCode.Turuncu.Common.Model;
using QuickCode.Turuncu.Common.Nswag.Clients.UserManagerModuleApi.Contracts;
using QuickCode.Turuncu.Portal.Models;

namespace QuickCode.Turuncu.Portal.Controllers
{
     public class LoginController : BaseController
    {
        private readonly IAuthenticationsClient authenticationsClient;
        private readonly IAspNetUsersClient usersClient;

        public LoginController(IAuthenticationsClient authenticationsClient,
            IAspNetUsersClient usersClient, ITableComboboxSettingsClient tableComboboxSettingsClient,
            IHttpContextAccessor httpContextAccessor, IMapper mapper, IMemoryCache cache) : base(tableComboboxSettingsClient,
            httpContextAccessor, mapper, cache)
        {
            this.authenticationsClient = authenticationsClient;
            this.usersClient = usersClient;
        }

        public  IActionResult Index(string returnUrl)
        {
            var model = GetModel<LoginData>();
            model.ReturnUrl = returnUrl;
            SetModelBinder(ref model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginData model)
        {
            ModelBinder(ref model);
            try
            {
                var response = await authenticationsClient.LoginAsync(new LoginRequest()
                    { Email = model.Username, Password = model.Password });
                (authenticationsClient as ClientBase)!.SetBearerToken(response.AccessToken);
                (usersClient as ClientBase)!.SetBearerToken(response.AccessToken);
                var userData = await usersClient.GetUserAsync(model.Username);
                httpContextAccessor.HttpContext!.Session.Clear();

                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, $"{userData.Id}"),
                    new Claim(ClaimTypes.Name, $"{userData.FirstName} {userData.LastName}"),
                    new Claim(ClaimTypes.Email, userData.Email),
                    new Claim(ClaimTypes.GroupSid, $"{userData.PermissionGroupId}"),
                    new Claim("QuickCodeApiToken", response.AccessToken),
                    new Claim("QuickCodeApiTokenExpiresIn", response.ExpiresIn.ToString()),
                    new Claim("RefreshToken", response.RefreshToken)

                }, CookieAuthenticationDefaults.AuthenticationScheme);

                var userPrincipal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = false,
                        AllowRefresh = false,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    });
                

                if (String.IsNullOrEmpty(model.ReturnUrl))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return Redirect(model.ReturnUrl);
                }
            }
            catch (QuickCodeSwaggerException ex)
            {
                if (ex.StatusCode == 401)
                {
                    model = new LoginData
                    {
                        ErrorMessage = "The email or password you entered is incorrect. Please check and try again."
                    };
                    return await Task.FromResult<IActionResult>(View(model));
                }
            }
            catch (Exception ex)
            {
                model = new LoginData
                {
                    ErrorMessage = "Server Error !!!"
                };
                return await Task.FromResult<IActionResult>(View(model));
            }
            
            return await Task.FromResult<IActionResult>(View(model));
        }

        public async Task<IActionResult> Logout()
        {
            httpContextAccessor.HttpContext!.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
          
            return RedirectToAction("Index", "Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Error()
        {
            return await Task.FromResult(View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }));
        }


    }
}