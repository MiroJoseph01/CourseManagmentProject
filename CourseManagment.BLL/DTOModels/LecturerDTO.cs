using CourseManagment.DAL.Entities;
using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.DTOModels
{
    /// <summary>
    /// Class LecturerDTO
    /// is used to process and transfer information about Lecturer
    /// between DAL and WEB 
    /// </summary>
    public class LecturerDTO
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

        public string ImageName { get; set; }
        public byte[] Image { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "The information must be more then 50 letters"), MaxLength(1000)]
        public string Information { get; set; }

        public string LecturerEmail { get; set; }

        public virtual Department Department { get; set; }

        public virtual IList<Course> Courses { get; set; }
    }
}
