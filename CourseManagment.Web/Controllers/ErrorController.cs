using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagment.Web.Controllers
{
    /// <summary>
    /// Class ErrorController 
    /// contains controllers to redirect to views 
    /// when errors occur
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Method NotFound
        /// Returns Error View
        /// </summary>
        /// <returns></returns>
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

    }
}