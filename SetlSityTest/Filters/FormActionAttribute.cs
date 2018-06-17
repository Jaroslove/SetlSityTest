using SetlSityTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SetlSityTest.Filters
{
    public class FormActionAttribute : ActionFilterAttribute
    {
        string nameForm, userName;
        History history = null;

        public FormActionAttribute(string nameForm)
        {
            this.nameForm = nameForm;

            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            history = new History();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                history.InputData = filterContext.ActionParameters["inputData"].ToString();
            }
            catch (Exception)
            {                
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            try
            {
                history.OutputData = HttpContext.Current.Items["rezult"].ToString();
                history.NameTask = nameForm;
                history.UserName = userName;

                ApplicationUserManager.Context.Histories.Add(history);
                ApplicationUserManager.Context.SaveChanges();
            }
            catch (Exception)
            {                
            }


            base.OnResultExecuted(filterContext);
        }
    }
}