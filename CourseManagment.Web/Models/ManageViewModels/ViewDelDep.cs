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
    /// Class ViewDelDep  
    /// declares properties to show needed information 
    /// about Departments to delete it
    /// </summary>
    public class ViewDelDep
    {
        public List<DepartmentDTO> departments;

        [Display(Name = "Department: ")]
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