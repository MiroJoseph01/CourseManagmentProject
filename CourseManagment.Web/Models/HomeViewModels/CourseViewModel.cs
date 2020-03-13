using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.HomeViewModels
{
    /// <summary>
    /// Class CourseViewModel 
    /// declares needed properties to show full information 
    /// about Course
    /// </summary>
    public class CourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }

        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public string CourseStart { get; set; }
        public string CourseEnd { get; set; }
        public int ForTerm { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public CourseViewModel()
        {
            Topics = new List<Topic>();
        }
        public int NumberOfStudents { get; set; }
    }
}