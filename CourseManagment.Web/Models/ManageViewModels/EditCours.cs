using CourseManagment.BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class EditCours  
    /// declares properties to show needed information 
    /// about Course to edit one
    /// </summary>
    public class EditCours
    {

        public int CourseId { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [MinLength(5, ErrorMessage = "The name of the course must be more then 5 letters"), MaxLength(60)]
        public string CourseName { get; set; }

        [Required]
        [MinLength(50, ErrorMessage = "The description must be more then 50 letters"), MaxLength(1000)]
        public string Description { get; set; }

        [Display(Name = "Lecturer: ")]
        [Required]
        [Range(0, 1000, ErrorMessage = "Choose lecturer")]
        public int SelectedLecId { get; set; }

        public List<LecturerDTO> lecturers;

        public IEnumerable<SelectListItem> lecturerItems
        {
            get
            {

                var allLec = lecturers.Select(f => new SelectListItem
                {
                    Value = f.LecturerId.ToString(),
                    Text = f.FirstName + " " + f.SecondName
                });


                return DefaultLecItem.Concat(allLec);
            }
        }

        public IEnumerable<SelectListItem> DefaultLecItem
        {
            get
            {
                return Enumerable.Repeat(new SelectListItem
                {
                    Value = "-1",
                    Text = "Select a Lecturer"
                }, count: 1);
            }
        }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CourseStart { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime CourseEnd { get; set; }

        [Required]
        [Range(1, 6, ErrorMessage = "Uncorrect term")]
        public int ForTerm { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required]
        public string Topics { get; set; }

        public string ImageName { get; set; }
        public byte[] Image { get; set; }
    }

}