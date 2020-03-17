using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    public class LecturersForEdit
    {
        public int LecturerId { get; set; }
        public string LecturerName { get; set; }
        public virtual Department Department { get; set; }
    }
}