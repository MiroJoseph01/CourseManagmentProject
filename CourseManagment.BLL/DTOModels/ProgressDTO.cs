using CourseManagment.DAL.Entities.Courses;
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
    /// Class ProgressDTO
    /// is used to process and transfer information about Progress
    /// between DAL and WEB 
    /// </summary>
    public class ProgressDTO
    {
        [Key]
        public int ProgresslId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Student Student { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Uncorrect Mark")]
        public int GradeBookMark { get; set; }
    }
}
