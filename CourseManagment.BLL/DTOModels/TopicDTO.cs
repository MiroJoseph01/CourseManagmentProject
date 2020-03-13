using CourseManagment.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.DTOModels
{
    /// <summary>
    /// Class TopicDTO
    /// is used to process and transfer information about Topic
    /// between DAL and WEB 
    /// </summary>
    public class TopicDTO
    {
        [Key]
        public int TopicId { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "The name of the Topic must be more then 2 letters"), MaxLength(30)]
        public string TopicName { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public TopicDTO()
        {
            Courses = new List<Course>();
        }
    }
}
