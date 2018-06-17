using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SetlSityTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SetlSityTest.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        // GET: Task
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                AuthenticationManager.SignOut();
                return RedirectToAction("Login", "Task");
            }
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.UserName, model.Pass);

                if (user == null)
                {
                    ModelState.AddModelError("", "неверный логин или пароль");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager
                        .CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(
                        new AuthenticationProperties
                        {
                            IsPersistent = true,
                        }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser { UserName = model.UserName };
                IdentityResult rezult = await UserManager.CreateAsync(user, model.Pass);
                if (rezult.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, "OneTask");

                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in rezult.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            return View(model);
        }
    }
}