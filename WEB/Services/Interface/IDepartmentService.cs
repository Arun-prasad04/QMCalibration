using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IDepartmentService{
ResponseViewModel<DepartmentViewModel> GetAllDepartmentList();
ResponseViewModel<DepartmentViewModel> GetDepartmentById(int departmentId);
ResponseViewModel<DepartmentViewModel> InsertDepartment(DepartmentViewModel department);
ResponseViewModel<DepartmentViewModel> UpdateDepartment(DepartmentViewModel department);
ResponseViewModel<DepartmentViewModel> CreateNewDepartment();
	ResponseViewModel<DepartmentViewModel> DeleteDepartment(int departmentId);
}