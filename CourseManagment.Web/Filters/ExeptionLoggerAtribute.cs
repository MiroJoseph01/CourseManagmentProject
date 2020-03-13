using CourseManagment.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagment.Web.Filters
{
    /// <summary>
    /// Class ExceptionLoggerAttribute 
    /// extends class FilterAttribute and 
    /// implements interface IExceptionFilter 
    /// to create custom Filter to process Exceptions
    /// </summary>
    public class ExceptionLoggerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            ExceptionDetail exceptionDetail = new ExceptionDetail()
            {
                ExceptionMessage = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                ControllerName = filterContext.RouteData.Values["controller"].ToString(),
                ActionName = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.Now
            };

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.ExceptionDetails.Add(exceptionDetail);
                db.SaveChanges();
            }
            filterContext.Result = new RedirectResult("/Error/NotFound");

            filterContext.ExceptionHandled = true;
        }
    }
}