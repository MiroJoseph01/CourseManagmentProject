using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Entities.Lecturers
{
    /// <summary>
    /// Class Department
    /// Describes entity "Department"
    /// Shows what department does the Lecturer belong to
    /// </summary>
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "The name of the department must be more then 10 letters"), MaxLength(60)]
        public string DepartmentName { get; set; }

        public virtual IList<Lecturer> Lecturers { get; set; }

    }
}
