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
        string inputData, nameForm, userName;
        History history = null;

        public FormActionAttribute(string nameForm)
        {
            this.nameForm = nameForm;
            //this.userName = userName;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                userName = HttpContext.Current.User.Identity.Name;
            }
            history = new History();
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            history.InputData = filterContext.ActionParameters["one"].ToString();
            base.OnActionExecuting(filterContext);
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    var objectContent = filterContext.Response.Content as ObjectContent;
        //    if (objectContent != null)
        //    {
        //        var type = objectContent.ObjectType; //type of the returned object
        //        var value = objectContent.Value; //holding the returned value
        //    }

        //    history.OutputData = filterContext.HttpContext.Response.co
        //    history.NameTask = nameForm;
        //    history.UserName = userName;

        //    ApplicationUserManager.Context.Histories.Add(history);
        //    ApplicationUserManager.Context.SaveChanges();

        //    base.OnActionExecuted(filterContext);
        //}

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            history.OutputData = HttpContext.Current.Items["rezult"].ToString();
            history.NameTask = nameForm;
            history.UserName = userName;

            ApplicationUserManager.Context.Histories.Add(history);
            ApplicationUserManager.Context.SaveChanges();

            base.OnResultExecuted(filterContext);
        }
    }
}