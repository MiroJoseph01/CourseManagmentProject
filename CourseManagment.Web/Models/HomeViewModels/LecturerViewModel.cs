using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.HomeViewModels
{

    /// <summary>
    /// Class LecturerViewModel 
    /// declares needed properties to show full information 
    /// about Lecturer
    /// </summary>
    public class LecturerViewModel
    {
        public int LecturerId { get; set; }
        public string FirstName { get; set; } 
        public string SecondName { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public string Information { get; set; }
        public string LecturerEmail { get; set; }
        public virtual Department Department { get; set; }
        public virtual IList<Course> Courses { get; set; }
    }
}