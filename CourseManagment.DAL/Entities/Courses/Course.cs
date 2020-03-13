using CourseManagment.DAL.Entities.Gradebooks;
using CourseManagment.DAL.Entities.Lecturers;
using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities.Courses
{
/// <summary>
/// Class Course
/// Describes entity "Course"
/// </summary>
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "The name of the course must be more then 5 letters"), MaxLength(60)]
        public string CourseName { get; set; }
  
        [Required]
        [MinLength(50, ErrorMessage = "The description must be more then 50 letters"), MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public virtual Lecturer Lecturer { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CourseStart { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CourseEnd { get; set; }

        [Required]
        [Range(1, 6, ErrorMessage = "Uncorrect term")]
        public int ForTerm { get; set; }

        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Topic> Topics { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        

        public Course()
        {
            Topics = new List<Topic>();
        }

        public virtual IList<Progress> Progresses { get; set; }

        public string ImageName { get; set; }
        public byte[] Image { get; set; }
    }
}
