using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SetlSityTest.Filters;
using SetlSityTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
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

        private AlgorithmsSetlSityTest.Tasks taskAlg = new AlgorithmsSetlSityTest.Tasks();        



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

        [FormAction("Add")]
        [Authorize(Roles = "admin")]
        public int Add(int inputData)
        {
            HttpContext.Items["rezult"] = 100;
            return inputData + 100;
        }

        [FormAction("AddNewUser")]
        public string AddNewUser(string inputData)
        {            
            try
            {
                string[] sLogPass = inputData.Split('/');
                string login = sLogPass[0];
                string pass = sLogPass[1];

                string patternPass = "^(?=.*?[a - z]).{6,}$";//needa check pass and check loging from existing in db

                if (login.Length > 3)
                {
                    ApplicationUser user = new ApplicationUser { UserName = login };
                    IdentityResult rezult = UserManager.Create(user, pass);

                    if (rezult.Succeeded)
                    {
                        UserManager.AddToRoles(user.Id, "OneTask");

                        HttpContext.Items["rezult"] = pass.Length.ToString();
                        return pass.Length.ToString();
                    }
                }
            }
            catch (Exception) { }

            return "Неверный логин или пароль";
        }

        [FormAction("CountUserTask")]
        public string CountUserTask(string inputData)
        {
            try
            {
                string[] sLogTask = inputData.Split('/');
                string userName = sLogTask[0];
                string[] tasks = sLogTask[1].Split(';');

                var user = UserManager.Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    HttpContext.Items["rezult"] = tasks
                        .Intersect(user.Tasks.Select(i => i.Name)
                        .ToArray())
                        .Count()
                        .ToString();

                    return HttpContext.Items["rezult"].ToString();
                }
            }
            catch (Exception) { }

            return "Неверно введенные данные";
        }

        [FormAction("ConvertToBin")]
        public string ConvertToBin(int inputData)
        {
            var rezult = taskAlg.TaskOne(inputData);

            HttpContext.Items["rezult"] = rezult;

            return rezult;
        }

        [FormAction("ConvertToHex")]
        public string ConvertToHex(int inputData)
        {
            var rezult = taskAlg.TaskTwo(inputData);

            HttpContext.Items["rezult"] = rezult;

            return rezult;
        }

        [FormAction("Sorting")]
        public string Sorting(int inputData)
        {
            var rezult = taskAlg.TaskThree(inputData);

            HttpContext.Items["rezult"] = rezult;

            return rezult.ToString();
        }

        [FormAction("Fibo")]
        public string Fibo(int inputData)
        {
            var rezult = taskAlg.TaskFour(inputData);

            HttpContext.Items["rezult"] = rezult;

            return rezult.ToString();
        }

        [FormAction("Reverse")]
        public string Reverse(string inputData)
        {
            var rezult = taskAlg.TaskSeven(inputData);

            HttpContext.Items["rezult"] = rezult;

            return rezult.ToString();
        }

        public string NewTaskDo(int inputData)
        {
            return "новая задача"; 
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