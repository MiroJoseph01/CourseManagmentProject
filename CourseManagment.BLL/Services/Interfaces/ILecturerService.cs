using CourseManagment.BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services.Interfaces
{
    /// <summary>
    /// Interface ILecturerService
    /// declares functionality related to class LecturerDTO
    /// </summary>
    public interface ILecturerService
    {
        IEnumerable<LecturerDTO> GetLecturers();
        LecturerDTO GetLecturer(int? id);
        void AddLecturer(LecturerDTO lecturerDTO, int depId);
        void EditLecturer(LecturerDTO lecturerDTO, int depId);
        void Dispose();
    }
}
