using CourseManagment.BLL.DTOModels;
using CourseManagment.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services.Interfaces
{
    /// <summary>
    /// Interface ICourseService
    /// declares functionality related to class CourseDTO
    /// </summary>
    public interface ICourseService
    {
        IEnumerable<CourseDTO> GetCourses();
        CourseDTO GetCourse(int? id);
        IEnumerable<CourseDTO> GetCoursesOfLecturer(int? id);
        IEnumerable<CourseDTO> GetCoursesByTopic(int? id);
        void AddCourse(CourseDTO courseDTO, int lecId, string topics);
        void EditCourse(CourseDTO courseDTO, int lecId, string topics);
        void Dispose();
    }
}
