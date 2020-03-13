using CourseManagment.DAL.Entities;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.DTOModels
{
    /// <summary>
    /// Class CourseDTO
    /// is used to process and transfer information about Course
    /// between DAL and WEB 
    /// </summary>
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public virtual Lecturer Lecturer { get; set; }
        public DateTime CourseStart { get; set; }
        public DateTime CourseEnd { get; set; }
        public int ForTerm { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Student> Students { get; set; }

        public CourseDTO()
        {
            Topics = new List<Topic>();
        }
        public int NumberOfStudents { get; set; }
        public int Position { get; set; }
        public int? Mark { get; set; }
    }
}
