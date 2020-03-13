using CourseManagment.DAL.Entities.Courses;
using CourseManagment.DAL.Entities.Gradebooks;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Models.ManageViewModels
{
    /// <summary>
    /// Class UserViewModel  
    /// declares properties to show needed information 
    /// about User for different roles
    /// </summary>
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public int StudentTerm { get; set; }
        public string Description { get; set; }
        public string Role { get; set; }

    }
}