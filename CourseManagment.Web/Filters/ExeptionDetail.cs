using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Filters
{
    /// <summary>
    /// Class ExceptionDetail 
    /// Describes Entity "ExceptionDetail" 
    /// contains all needed information about Exceptions
    /// </summary>
    public class ExceptionDetail
    {
        public int Id { get; set; }
        public string ExceptionMessage { get; set; } 
        public string ControllerName { get; set; } 
        public string ActionName { get; set; } 
        public string StackTrace { get; set; }
        public DateTime Date { get; set; } 
    }

}