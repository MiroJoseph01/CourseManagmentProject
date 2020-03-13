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
    /// Class DepartmentDTO
    /// is used to process and transfer information about Department
    /// between DAL and WEB 
    /// </summary>
    public class DepartmentDTO
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The name of the department must be more then 10 letters"), MaxLength(60)]
        public string DepartmentName { get; set; }

        public virtual IList<Lecturer> Lecturers { get; set; }
    }
}
