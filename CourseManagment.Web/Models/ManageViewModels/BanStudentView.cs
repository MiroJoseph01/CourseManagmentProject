using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class BanStudentView 
    /// declares properties to show needed information 
    /// about Student
    /// </summary>
    public class BanStudentView
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int Term { get; set; }
        public bool isBanned{ get; set; }
    }
}