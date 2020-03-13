using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities.Courses
{
    /// <summary>
    /// Class Topic
    /// Describes entity "Topic" 
    /// Connected with "Course"
    /// </summary>
    public class Topic
    {
        [Key]
        public int TopicId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "The name of the Topic must be more then 2 letters"), MaxLength(30)]
        public string TopicName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public Topic()
        {
            Courses = new List<Course>();
        }

    }
}
