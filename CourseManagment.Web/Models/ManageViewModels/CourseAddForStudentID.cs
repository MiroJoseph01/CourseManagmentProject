using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    public class CourseAddForStudentID
    {
        public int Id { get; set; }
        public virtual ICollection<CourseAddForStudent> Courses { get; set; }
    }
}