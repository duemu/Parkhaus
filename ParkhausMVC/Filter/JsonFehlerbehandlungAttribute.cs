using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParkhausMVC.Filter
{
    //Filter Attribut zur Fehlerbehandlung von ajax-Requests
    public class JsonFehlerbehandlungAttribute : FilterAttribute, IExceptionFilter
    {
        //Wird nur benutzt, wenn eine Exception geworfen wird
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            //Gibt ein Json mit dem Status und der Fehlermeldung zürück 
            filterContext.Result = new JsonResult
            {
                Data = new { success = false, error = filterContext.Exception.Message.ToString() },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }

}

