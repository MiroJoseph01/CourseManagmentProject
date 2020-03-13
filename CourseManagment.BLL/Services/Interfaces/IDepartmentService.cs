using CourseManagment.BLL.DTOModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.BLL.Services.Interfaces
{
    /// <summary>
    /// Interface IDepartmentService
    /// declares functionality related to class DepartmentDTO
    /// </summary>
    public interface IDepartmentService
    {
        IEnumerable<DepartmentDTO> GetDepartments();
        DepartmentDTO GetDepartment(int? id);
        void CreateDepartment(DepartmentDTO departmentDTO);
        void DeleteDepartment(int id);
        void Dispose();
    }
}
