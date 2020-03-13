using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class CoursesForLecturer  
    /// declares properties to show needed information 
    /// about Courses for Lecturer
    /// </summary>
    public class CoursesForLecturer
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Position { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        public int NumberOfStudents { get; set; }
    }
}