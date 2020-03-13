using CourseManagment.BLL.DTOModels;
using CourseManagment.DAL.Entities.Lecturers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class CreateLec  
    /// declares properties to show needed information 
    /// about Lecturer to create one
    /// </summary>
    public class CreateLec
    {
        public int LecturerId { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters")]
        [MinLength(3, ErrorMessage = "The first name of the lecturer must be more then 3 letters"), MaxLength(30)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Second Name")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only letters")]
        [MinLength(5, ErrorMessage = "The second name of the lecturer must be more then 5 letters"), MaxLength(40)]
        public string SecondName { get; set; }

        
        public string ImageName { get; set; }
        [DataType(DataType.Upload)]
        public byte[] Image { get; set; }

        [Required]

        [MinLength(50, ErrorMessage = "The information must be more then 50 letters"), MaxLength(1000)]
        public string Information { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string LecturerEmail { get; set; }

        public List<DepartmentDTO> departments;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Department: ")]
        [Required]
        [Range(0,1000, ErrorMessage ="Choose Department")]
        public int SelectedDepId { get; set; }

        public IEnumerable<SelectListItem> departmentItems
        {
            get
            {

                var allDep = departments.Select(f => new SelectListItem
                {
                    Value = f.DepartmentId.ToString(),
                    Text = f.DepartmentName
                });


                return DefaultDepItem.Concat(allDep);
            }
        }

        public IEnumerable<SelectListItem> DefaultDepItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "Select a Dep"
                }, count: 1);
            }
        }
    }
}

