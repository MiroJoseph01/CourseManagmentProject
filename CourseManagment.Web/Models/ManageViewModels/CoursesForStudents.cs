using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class CoursesForStudents  
    /// declares properties to show needed information 
    /// about Courses for Student
    /// </summary>
    public class CoursesForStudents
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public string CourseStart { get; set; }
        public string CourseEnd { get; set; }
        public int ForTerm { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int? Mark { get; set; }
        public int Position { get; set; }
    }
}