using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class CourseAddForStudent 
    /// declares properties to show needed information 
    /// about Course for Student
    /// </summary>
    public class CourseAddForStudent
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public string CourseStart { get; set; }
        public string CourseEnd { get; set; }
        public bool Agree { get; set; }
    }
}