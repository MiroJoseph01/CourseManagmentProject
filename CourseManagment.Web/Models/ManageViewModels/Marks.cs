using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class Marks  
    /// declares properties to show needed information 
    /// about Courses and Students to rate
    /// </summary>
    public class Marks
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual IList<StudentMark> Students { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        public int Position { get; set; }
    }
}