using CourseManagment.BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services.Interfaces
{
    /// <summary>
    /// Interface IStudentService
    /// declares functionality related to class StudentDTO
    /// </summary>
    public interface IStudentService
    {
        void AddStudent(StudentDTO studentDTO);
        IEnumerable<StudentDTO> GetStudents();
        StudentDTO GetStudent(int? id);
        StudentDTO GetStudentByEmail(string email);
        void AddCourse(int studentId, int courseId);
        void BlockUnblock(int studentId, bool pos);
        void Dispose();
    }
}
