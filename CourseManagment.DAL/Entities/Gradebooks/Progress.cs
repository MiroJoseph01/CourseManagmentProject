using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Students;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities.Gradebooks
{
    /// <summary>
    /// Class Progress
    /// Describes entity "Progress"
    /// Allows to rate Students
    /// </summary>
    public class Progress
    {
        [Key]
        public int ProgresslId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student{ get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Uncorrect Mark")]
        public int GradeBookMark { get; set; }
    }
}
