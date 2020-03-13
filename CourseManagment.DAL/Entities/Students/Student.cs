using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Gradebooks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities.Students
{
    /// <summary>
    /// Class Student
    /// Describes entity "Student"
    /// Contain information about Student and his courses
    /// </summary>
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLength(3, ErrorMessage = "The first name of the student must be more then 3 letters"), MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLength(5, ErrorMessage = "The second name of the student must be more then 3 letters"), MaxLength(40)]
        public string SecondName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Uncorrect Email")]
        public string StudentEmail { get; set; }

        [Required]
        [Range(1, 6, ErrorMessage = "Uncorrect term")]
        public int StudentTerm { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public Student()
        {
            Courses = new List<Course>();
        }

        public bool IsBanned { get; set; } = false;

        public virtual IList<Progress> Progresses { get; set; }
    }
}
