using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Gradebooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.DTOModels
{
    /// <summary>
    /// Class StudentDTO
    /// is used to process and transfer information about Student
    /// between DAL and WEB 
    /// </summary>
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string StudentEmail { get; set; }
        public int StudentTerm { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public StudentDTO()
        {
            Courses = new List<Course>();
        }
        public bool IsBanned { get; set; } = false;
        public virtual IList<Progress> Progresses { get; set; }
    }
}
