using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class StudentMark  
    /// declares properties to show needed information 
    /// about Student and his Mark to rate
    /// </summary>
    public class StudentMark
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        [Required]
        [Range(0,100, ErrorMessage ="Invalid mark!")]
        public int? Mark { get; set; }
        
    }
}