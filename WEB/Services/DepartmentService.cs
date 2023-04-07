using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using WEB.Models;
using AutoMapper;
namespace WEB.Services;


public class DepartmentService:IDepartmentService
{
    
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork{get;set;}
    public DepartmentService(IUnitOfWork unitOfWork,IMapper mapper){
            _unitOfWork=unitOfWork;
             _mapper = mapper;  
    }
    public ResponseViewModel<DepartmentViewModel> GetAllDepartmentList(){
        try
       {
            List<DepartmentViewModel> departmentList= _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
            return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=departmentList
            };
        }catch(Exception e){
             return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseServiceMethod="Department",
                ResponseService="GetAllDepartmentList"
            };
        }
    }
    public ResponseViewModel<DepartmentViewModel> GetDepartmentById(int departmentId){  
        try
        {
           DepartmentViewModel departmentData = _mapper.Map<DepartmentViewModel>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().Where(x => x.Id == departmentId).SingleOrDefault());
           return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=departmentData,
                ResponseDataList=null
            };
        }catch(Exception e){
             return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="Department",
                ResponseServiceMethod="GetDepartmentById"
            };
        }

    }
    public ResponseViewModel<DepartmentViewModel> InsertDepartment(DepartmentViewModel department){
        try{
            _unitOfWork.BeginTransaction();
            _unitOfWork.Repository<Department>().Insert(_mapper.Map<Department>(department));
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=null
            };
        }catch(Exception e){
                _unitOfWork.RollBack();
                return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=department,
                ResponseDataList=null,
                ResponseService="Department",
                ResponseServiceMethod="InsertDepartment"
                };
        }
    }
    public ResponseViewModel<DepartmentViewModel> UpdateDepartment(DepartmentViewModel department){

        try{

            Department departmentById=_unitOfWork.Repository<Department>().GetQueryAsNoTracking(Q=>Q.Id==department.Id).SingleOrDefault();
            departmentById.Name=department.Name;
            departmentById.Description=department.Description;
            departmentById.ModifiedBy=1;
            departmentById.ModifiedOn=DateTime.Now;
            _unitOfWork.BeginTransaction();
            _unitOfWork.Repository<Department>().Update(departmentById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=null
            };
            
        }catch(Exception e){
            _unitOfWork.RollBack();
             return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=department,
                ResponseDataList=null,
                ResponseService="Department",
                ResponseServiceMethod="UpdateDepartment"
            };
        }
    }
    public ResponseViewModel<DepartmentViewModel> DeleteDepartment(int departmentId){
        try{
            Department departmentById=_unitOfWork.Repository<Department>().GetQueryAsNoTracking(Q=>Q.Id==departmentId).SingleOrDefault();
            _unitOfWork.BeginTransaction();
            _unitOfWork.Repository<Department>().Delete(departmentById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=null
            };
        }catch(Exception e){
            _unitOfWork.RollBack();
            return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="Department",
                ResponseServiceMethod="DeleteDepartment"
            };
        }
    }
}
