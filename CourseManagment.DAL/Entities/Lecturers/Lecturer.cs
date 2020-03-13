using CourseManagment.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities.Lecturers
{
    /// <summary>
    /// Class Lecurer
    /// Describes entity "Lecturer"
    /// Gives some information about Lecturer that teaches Courses
    /// </summary>
    public class Lecturer
    {
        [Key]
        public int LecturerId { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLength(3, ErrorMessage = "The first name of the lecturer must be more then 3 letters"), MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z]+$")]
        [MinLength(5, ErrorMessage = "The second name of the lecturer must be more then 5 letters"), MaxLength(40)]
        public string SecondName { get; set; }


        [Required]
        [MinLength(50, ErrorMessage = "The information must be more then 50 letters"), MaxLength(1000)]
        public string Information { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Uncorrect Email")]
        public string LecturerEmail { get; set; }

        public virtual Department Department { get; set; }

        public virtual IList<Course> Courses { get; set; }

        public string ImageName { get; set; }
        public byte[] Image { get; set; }
    }
}
