using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class CourseForEdit  
    /// declares properties to show needed information 
    /// about Courses to choose for editing
    /// </summary>
    public class LecturerForEdit
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public virtual Lecturer Lecturer { get; set; }
    }
}