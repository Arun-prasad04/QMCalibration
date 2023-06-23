using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using WEB.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1.Ocsp;

namespace WEB.Services;


public class DepartmentService : IDepartmentService
{
    
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork{get;set;}
	private IUtilityService _utilityService;
	public DepartmentService(IUnitOfWork unitOfWork,IMapper mapper,IUtilityService utilityService)
	{
        _unitOfWork=unitOfWork;
		_utilityService = utilityService;
		_mapper = mapper;  
    }
    public ResponseViewModel<DepartmentViewModel> GetAllDepartmentList(){
        try
       {
//			List<UserViewModel> userViewModelList = _unitOfWork.Repository<User>().
//GetQueryAsNoTracking(s => s.ActiveStatus == true).Include(I => I.Department).Select(S => new UserViewModel()
//{

	        List <DepartmentViewModel> departmentList = _unitOfWork.Repository<Department>().GetQueryAsNoTracking(s => s.ActiveStatus == true).Include(I => I.Location).Select(S => new DepartmentViewModel()
			{
					
							 Id = S.Id,
				             PlantLocation = S.Location.PlantLocation,
				             PlantCode = S.Location.PlantCode,
				             Name = S.Name,
				             Description = S.Description,
							 DescriptionJP = S.DescriptionJP,
				             DeptCode=S.DeptCode,
                             Section = S.Section,
                             SubSection = S.SubSection,
				             NameJP = S.NameJP,
				             SectionJP = S.SectionJP,
				             SubSectionJP = S.SubSectionJP,
                             PlantId =S.PlantId,
                             CreatedOn=S.CreatedOn
			 }).ToList();

							
			// List <DepartmentViewModel> departmentList= _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
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
    public ResponseViewModel<DepartmentViewModel> CreateNewDepartment()
    {
        try
        {

			List<LocationViewModel> LocationData = _mapper.Map<List<LocationViewModel>>(_unitOfWork.Repository<Location>().GetQueryAsNoTracking().ToList());
			DepartmentViewModel departmenttEmptyViewModel = new DepartmentViewModel();
            departmenttEmptyViewModel.locationList = LocationData;
			
			return new ResponseViewModel<DepartmentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = departmenttEmptyViewModel

			};
		
       } catch(Exception e){
            return new ResponseViewModel<DepartmentViewModel> {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "Department",
                ResponseServiceMethod = "GetDepartmentById"
            };
        }
    }
			public ResponseViewModel<DepartmentViewModel> GetDepartmentById(int departmentId)
{  
        try
        {
			// DepartmentViewModel departmenttEmptyViewModel = _mapper.Map<DepartmentViewModel>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().Where(x => x.Id == departmentId).SingleOrDefault());

            DepartmentViewModel departmenttEmptyViewModel = _mapper.Map<DepartmentViewModel>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking(Q => Q.Id == departmentId)
				.Include(I => I.Location).Select(S => new DepartmentViewModel()
				{
					Id = S.Id,
					PlantLocation = S.Location.PlantLocation,
					PlantCode = S.Location.PlantCode,
					Name = S.Name,
					Description = S.Description,
					DescriptionJP = S.DescriptionJP,
					DeptCode = S.DeptCode,
					Section = S.Section,
					SubSection = S.SubSection,
					NameJP = S.NameJP,
					SectionJP = S.SectionJP,
					SubSectionJP = S.SubSectionJP,
					PlantId = S.PlantId,
					CreatedOn = S.CreatedOn

					//Id = S.Id,
					//PlantLocation = S.Location.PlantLocation,
					//PlantCode = S.Location.PlantCode,
					//Name = S.Name,
					//Description=S.Description,
					//DeptCode = S.DeptCode,
					//Section = S.Section,
					//SubSection = S.SubSection,
					//PlantId = S.PlantId,
					//CreatedOn = S.CreatedOn

				}).SingleOrDefault());


			List<LocationViewModel> LocationData = _mapper.Map<List<LocationViewModel>>(_unitOfWork.Repository<Location>().GetQueryAsNoTracking().ToList());
			departmenttEmptyViewModel.locationList = LocationData;
			return new ResponseViewModel<DepartmentViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData= departmenttEmptyViewModel,
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
