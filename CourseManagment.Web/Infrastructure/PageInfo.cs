using CourseManagment.Web.Models.HomeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CourseManagment.Web.Infrastructure
{
    /// <summary>
    /// Class PageInfo 
    /// contains information about current page of general pagging
    /// </summary>
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / PageSize); }
        }
    }

    /// <summary>
    /// Class IndexViewModel 
    /// declares View Model which is passed to the view
    /// </summary>
    public class IndexViewModel
    {
        public IEnumerable<CourseViewModel> Courses { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}