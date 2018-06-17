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

        public ActionResult Forms()
        {
            var idUser = User.Identity.GetUserId();
            ViewBag.Id = idUser;
            ViewBag.TASK_NAMES = new List<string> { "а", "б", "в", "г", "д", "е" };//added ж

            var currentUser = UserManager.FindById(User.Identity.GetUserId());

            return View(currentUser);
        }

        public ActionResult AddNewTask(string name)
        {

            SetlSityTest.Models.Task task = new Models.Task { Name = name };

            task.ApplicationUsers = new List<ApplicationUser>
            {
                UserManager.FindById(User.Identity.GetUserId())
            };

            ApplicationUserManager.Context.Tasks.Add(task);

            var id = User.Identity.GetUserId();
                var user = ApplicationUserManager.Context.Users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    user.Tasks.Add(task);
                }
            ApplicationUserManager.Context.SaveChanges();

            return RedirectToAction("Forms");
            //return "задача добавлена";
        }

        [Authorize(Roles = "admin")]
        public int Add(int one)
        {
            return one + 100;
        }

        public int NewTaskDo(int one)
        {
            return one + 1000;
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
                    user.LastLogIn = DateTime.Now;
                    UserManager.Update(user);

                    return RedirectToAction("Index", "Home");
                }
            }

            return View(model);
        }

        //[AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AllowAnonymous]
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


        public ActionResult History()
        {
            var users = UserManager.Users.ToList();
            return View(users);
        }
    }
}