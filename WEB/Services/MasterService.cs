using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Collections.Generic;
using System.Data;
using NPOI.SS.Formula.Functions;

namespace WEB.Services;


public class MasterService : IMasterService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	private IUtilityService _utilityService;
	private IConfiguration _configuration;
	public MasterService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService, IConfiguration configuration)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_utilityService = utilityService;
		_configuration = configuration;
	}
	public ResponseViewModel<MasterViewModel> GetAllMasterList(string SessionLang)
	{
		try
		{
			List<MasterViewModel> masterViewModelList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2).Include(I => I.SupplierModel).Include(I => I.QuarantineModel).Include(L => L.Lovs).Include(I => I.FileUploadModel).Select(s => new MasterViewModel()
			{
				Id = s.Id,
				EquipName = s.EquipName,
				LocationId = s.LocationId,
				PONo = s.PONo,
				PODate = s.PODate,
				CommissionedOn = s.CommissionedOn,
				Make = s.Make,
				Cost = s.Cost,
				CurrencyId = s.CurrencyId,
				CalibrationSourceId = s.CalibrationSourceId,
				Supplier = s.SupplierModel.Name,
				CalibFreqId = s.CalibFreqId,
				CalibrationFrequency = s.Lovs.AttrValue,
				CalibDate = s.CalibDate.Equals(DBNull.Value) ? null :s.CalibDate,
				DueDate = s.DueDate.Equals(DBNull.Value) ? null : s.DueDate,
				Range = s.Range,
				SerialNo = s.SerialNo,
				LabId = s.EquipmentMasterId,
				CertNo = s.CertNo,
				Traceability = s.Traceability,
				Name = s.SupplierModel.Name,
				PhoneNo = s.SupplierModel.PhoneNo,
				EmailId = s.SupplierModel.EmailId,
				MobileNo = s.SupplierModel.MobileNo,
				EquipmentMasterId= s.EquipmentMasterId,
				FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList()
			}).ToList();
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = masterViewModelList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - GetAllMasterList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Master",
				ResponseService = "GetAllMasterList"
			};
		}
	}

	public ResponseViewModel<MasterViewModel> GetMasterById(int masterId)
	{
		try
		{
			MasterViewModel masterById = _unitOfWork.Repository<Master>()
			.GetQueryAsNoTracking(Q => Q.Id == masterId)
			.Include(I => I.SupplierModel)
			.Include(I => I.QuarantineModel)
			.Select(s => new MasterViewModel()
			{
				Id = s.Id,
				EquipName = s.EquipName,
				EquipNameJP = s.EquipNameJP,
				LocationId = s.LocationId,
				PONo = s.PONo,
				PODate = s.PODate,
				CommissionedOn = s.CommissionedOn,
				Make = s.Make,
				Cost = s.Cost,
				CurrencyId = s.CurrencyId,
				CalibrationSourceId = s.CalibrationSourceId,
				Supplier = s.SupplierModel.Name,
				CalibFreqId = s.CalibFreqId,
				CalibDate = s.CalibDate,
				DueDate = s.DueDate,
				Range = s.Range,
				SerialNo = s.SerialNo,
				//LabId = s.LabId,
				CertNo = s.CertNo,
				Traceability = s.Traceability,
				Name = s.SupplierModel.Name,
				PhoneNo = s.SupplierModel.PhoneNo,
				EmailId = s.SupplierModel.EmailId,
				MobileNo = s.SupplierModel.MobileNo,
				EquipmentMasterId = s.EquipmentMasterId,
				FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
                //	TypeOfEquipment=s.TypeOfEquipment,
                DepartId = s.DepartmentModel.Id
				
				}).FirstOrDefault();

			List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Master").ToList());
			masterById.LocationList = lovsList.Where(W => W.AttrName == "Location").ToList();
			masterById.CurrencyList = lovsList.Where(W => W.AttrName == "Currency").ToList();
			masterById.CalibrationSourceList = lovsList.Where(W => W.AttrName == "CalibrationSource").ToList();
			masterById.CalibrationFreq = lovsList.Where(W => W.AttrName == "CalibrationFreq").ToList();
			//List<DepartmentViewModel> DepartmentData =_mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());			
           // var DepartmentData = GetMasterDepartmentSubSection();
			CMTDL _cmtdl = new CMTDL(_configuration);
			var DepartmentData = _cmtdl.GetMasterDepartmentSubSection();
			masterById.Departments = DepartmentData;
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = masterById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - GetMasterById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "GetMasterByID"
			};
		}

	}
	public ResponseViewModel<MasterViewModel> InsertMaster(MasterViewModel master)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			Supplier supplierById = _unitOfWork.Repository<Supplier>().GetQueryAsNoTracking(Q => Q.Name == master.Supplier).SingleOrDefault();
			if (supplierById != null)
			{
				master.SupplierId = supplierById.Id;
			}
			else
			{
				Supplier supplierData = new Supplier()
				{
					Name = master.Supplier,
					ContactName = master.Name,
					PhoneNo = master.PhoneNo,
					EmailId = master.EmailId,
					MobileNo = master.MobileNo,
					CreatedOn = DateTime.Now,
					ModifiedOn = DateTime.Now
				};
				_unitOfWork.Repository<Supplier>().Insert(supplierData);
				_unitOfWork.SaveChanges();
				master.SupplierId = supplierData.Id;
			}

			Master newMaster = new Master();
			newMaster = _mapper.Map<Master>(master);
			_unitOfWork.Repository<Master>().Insert(newMaster);
			_unitOfWork.SaveChanges();
			MasterQuarantine masterQuarantine = new MasterQuarantine()
			{
				MasterId = newMaster.Id,
				Reason = "",
				UserId = master.CreatedBy,
				CreatedOn = DateTime.Now,
				StatusId = 2
			};
			_unitOfWork.Repository<MasterQuarantine>().Insert(masterQuarantine);
			_unitOfWork.SaveChanges();
			if (master.ImageUpload != null && master.ImageUpload.Count() > 0)
			{
				foreach (IFormFile fileObj in master.ImageUpload)
				{
					string filePath = _utilityService.UploadImage(fileObj, "Master");
					Uploads upload = new Uploads()
					{
						FileName = fileObj.FileName,
						FileGuid = Guid.NewGuid(),
						CreatedOn = DateTime.Now,
						ModifiedOn = DateTime.Now,
						FilePath = filePath
					};
					_unitOfWork.Repository<Uploads>().Insert(upload);
					_unitOfWork.SaveChanges();
					MasterFileUpload masterFileUpload = new MasterFileUpload();
					masterFileUpload.MasterId = newMaster.Id;
					masterFileUpload.UploadId = upload.Id;
					_unitOfWork.Repository<MasterFileUpload>().Insert(masterFileUpload);
					_unitOfWork.SaveChanges();
				}
			}
			_unitOfWork.Commit();
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - InsertMaster Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "InsertMaster"
			};
		}
	}
	public ResponseViewModel<MasterViewModel> UpdateMaster(MasterViewModel master)
	{

		try
		{
			_unitOfWork.BeginTransaction();
			Master masterById = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == master.Id).SingleOrDefault();
			if (master.EquipName != null)
			{
				masterById.EquipName = master.EquipName;
			}
			if (master.EquipNameJP != null)
			{
				masterById.EquipNameJP = master.EquipNameJP;
			}
			if (master.LocationId != null)
			{
				masterById.LocationId = master.LocationId;
			}
			if (master.PONo != null)
			{
				masterById.PONo = master.PONo;
			}
			if (master.PODate != null)
			{
				masterById.PODate = master.PODate;
			}
			if (master.CommissionedOn != null)
			{
				masterById.CommissionedOn = master.CommissionedOn;
			}
			if (master.Make != null)
			{
				masterById.Make = master.Make;
			}
			if (master.Cost != null)
			{
				masterById.Cost = master.Cost;
			}
			if (master.CurrencyId != null)
			{
				masterById.CurrencyId = master.CurrencyId;
			}
			if (master.CalibrationSourceId != null)
			{
				masterById.CalibrationSourceId = master.CalibrationSourceId;
			}
			if (master.CalibFreqId != null)
			{
				masterById.CalibFreqId = master.CalibFreqId;
			}
			if (master.CalibDate != null)
			{
				masterById.CalibDate = master.CalibDate;
			}
			if (master.DueDate != null)
			{
				masterById.DueDate = master.DueDate;
			}
			if (master.Range != null)
			{
				masterById.Range = master.Range;
			}
			if (master.SerialNo != null)
			{
				masterById.SerialNo = master.SerialNo;
			}
			//if (master.LabId != null)
			//{
			//	masterById.LabId = master.LabId;
			//}
			if (master.CertNo != null)
			{
				masterById.CertNo = master.CertNo;
			}
			if (master.Traceability != null)
			{
				masterById.Traceability = master.Traceability;
			}
			if (master.EquipmentMasterId != null)
			{
				masterById.EquipmentMasterId = master.EquipmentMasterId;
			}
			//if (master.TypeOfEquipment != null)
			//{
			//	masterById.TypeOfEquipment = master.TypeOfEquipment;
			//}
			if (master.DepartId != null)
			{
				masterById.DepartmentId = master.DepartId;
			}

			Supplier supplierById = _unitOfWork.Repository<Supplier>().GetQueryAsNoTracking(Q => Q.Name == master.Supplier).SingleOrDefault();
			if (supplierById != null)
			{
				supplierById.Name = master.Supplier;
				supplierById.ContactName = master.Name;
				supplierById.PhoneNo = master.PhoneNo;
				supplierById.EmailId = master.EmailId;
				supplierById.MobileNo = master.MobileNo;
				supplierById.CreatedOn = DateTime.Now;
				supplierById.ModifiedOn = DateTime.Now;
				_unitOfWork.Repository<Supplier>().Update(supplierById);
			}
			else
			{
				Supplier supplierData = new Supplier()
				{
					Name = master.Supplier,
					ContactName = master.Name,
					PhoneNo = master.PhoneNo,
					EmailId = master.EmailId,
					MobileNo = master.MobileNo,
					CreatedOn = DateTime.Now,
					ModifiedOn = DateTime.Now
				};
				_unitOfWork.Repository<Supplier>().Insert(supplierData);
				_unitOfWork.SaveChanges();
				masterById.SupplierId = supplierData.Id;
			}
			_unitOfWork.Repository<Master>().Update(masterById);

			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("MasterService - UpdateMaster Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "UpdateMaster"
			};
		}
	}
	public ResponseViewModel<MasterViewModel> DeleteMaster(int masterId)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			Master masterById = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == masterId).SingleOrDefault();
			_unitOfWork.Repository<Master>().Delete(masterById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("MasterService - DeleteMaster Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "DeleteMaster"
			};
		}
	}
	public ResponseViewModel<MasterViewModel> CreateNewMaster()
	{
		try
		{
			MasterViewModel masterViewModel = new MasterViewModel();
			//var Department = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
			CMTDL _cmtdl = new CMTDL(_configuration);
			var Department = _cmtdl.GetMasterDepartmentSubSection();
			List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Master").ToList());
			masterViewModel.LocationList = lovsList.Where(W => W.AttrName == "Location").ToList();
			masterViewModel.CurrencyList = lovsList.Where(W => W.AttrName == "Currency").ToList();
			masterViewModel.CalibrationSourceList = lovsList.Where(W => W.AttrName == "CalibrationSource").ToList();
			masterViewModel.CalibrationFreq = lovsList.Where(W => W.AttrName == "CalibrationFreq").ToList();
			masterViewModel.Departments = Department;
			masterViewModel.CommissionedOn = DateTime.Now;
			masterViewModel.PODate = DateTime.Now;
			masterViewModel.CalibDate = DateTime.Now;
			masterViewModel.DueDate = DateTime.Now;
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = masterViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - CreateNewMaster Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "CreateNewMaster"
			};
		}
	}
	public ResponseViewModel<MasterViewModel> GetAllMasterQuarantineList()
	{
		try
		{
			List<MasterViewModel> masterViewModelList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1).Include(I => I.SupplierModel).Include(I => I.QuarantineModel).Select(s => new MasterViewModel()
			{
				Id = s.Id,
				EquipName = s.EquipName,
				LocationId = s.LocationId,
				PONo = s.PONo,
				PODate = s.PODate,
				CommissionedOn = s.CommissionedOn,
				Make = s.Make,
				Cost = s.Cost,
				CurrencyId = s.CurrencyId,
				CalibrationSourceId = s.CalibrationSourceId,
				Supplier = s.SupplierModel.Name,
				CalibFreqId = s.CalibFreqId,
				CalibDate = s.CalibDate,
				DueDate = s.DueDate,
				Range = s.Range,
				SerialNo = s.SerialNo,
				LabId = s.EquipmentMasterId,
				CertNo = s.CertNo,
				Traceability = s.Traceability,
				Name = s.SupplierModel.Name,
				PhoneNo = s.SupplierModel.PhoneNo,
				EmailId = s.SupplierModel.EmailId,
				MobileNo = s.SupplierModel.MobileNo,
				QuaraantineOn = s.QuarantineModel.Select(s => s.CreatedOn).FirstOrDefault(),
				QuarantineReason = s.QuarantineModel.Select(s => s.Reason).FirstOrDefault()
			}).ToList();
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = masterViewModelList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - GetAllMasterQuarantineList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Master",
				ResponseService = "GetAllMasterList"
			};
		}
	}

	public ResponseViewModel<MasterViewModel> MasterQuarantine(int masterId, string reason, int statusId, int userId)
	{

		try
		{
			_unitOfWork.BeginTransaction();
			MasterQuarantine masterQuarantineById = _unitOfWork.Repository<MasterQuarantine>().GetQueryAsNoTracking(Q => Q.MasterId == masterId).SingleOrDefault();
			if (masterQuarantineById != null)
			{
				masterQuarantineById.StatusId = 1;
				masterQuarantineById.CreatedOn = DateTime.Now;
				masterQuarantineById.Reason = reason;
				_unitOfWork.Repository<MasterQuarantine>().Update(masterQuarantineById);
			}
			else
			{
				MasterQuarantine masterQuarantine = new MasterQuarantine()
				{
					MasterId = masterId,
					Reason = reason,
					UserId = userId,
					CreatedOn = DateTime.Now,
					StatusId = statusId
				};
				_unitOfWork.Repository<MasterQuarantine>().Insert(masterQuarantine);
			}

			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("MasterService - MasterQuarantine Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "MasterQuarantine"
			};
		}
	}

	public ResponseViewModel<MasterViewModel> MasterRemoveQuarantine(int masterId)
	{

		try
		{
			_unitOfWork.BeginTransaction();

			MasterQuarantine masterQuarantineById = _unitOfWork.Repository<MasterQuarantine>().GetQueryAsNoTracking(Q => Q.MasterId == masterId && Q.StatusId == 1).FirstOrDefault();
			masterQuarantineById.StatusId = 2;
			_unitOfWork.Repository<MasterQuarantine>().Update(masterQuarantineById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("MasterService - MasterRemoveQuarantine Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "MasterQuarantine"
			};
		}
	}

	public ResponseViewModel<MasterViewModel> GetEquipmentListByName(string equipmentName)
	{
		try
		{
			List<MasterViewModel> masterByEquipName = _unitOfWork.Repository<Master>()
			.GetQueryAsNoTracking(Q => Q.EquipName.StartsWith(equipmentName))
			.Include(I => I.SupplierModel)
			.Include(I => I.QuarantineModel)
			.Select(s => new MasterViewModel()
			{
				Id = s.Id,
				EquipName = s.EquipName,
				CalibDate = s.CalibDate,
				Range = s.Range
			}).ToList();

			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = masterByEquipName
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - GetEquipmentListByName Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "GetEquipmentListByName"
			};
		}

	}
	
	//	public ResponseViewModel<MasterViewModel> GetEquipmentListByInstrumentId(int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4)
	//{
	//	try
	//	{
	//		List<MasterViewModel> masterViewModelList = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>()
	//			.GetQueryAsNoTracking(Q => Q.Id == MasterInstrument1 || Q.Id ==MasterInstrument2 || Q.Id ==MasterInstrument3 || Q.Id == MasterInstrument4).ToList());
			

	//		return new ResponseViewModel<MasterViewModel>
	//		{
	//			ResponseCode = 200,
	//			ResponseMessage = "Success",
	//			ResponseData = null,
	//			ResponseDataList = masterViewModelList
	//		};
	//	}
	//	catch (Exception e)
	//	{
	//		ErrorViewModelTest.Log("MasterService - GetEquipmentListByLabId Method");
	//		ErrorViewModelTest.Log("exception - " + e.Message);
	//		return new ResponseViewModel<MasterViewModel>
	//		{
	//			ResponseCode = 500,
	//			ResponseMessage = "Failure",
	//			ErrorMessage = e.Message,
	//			ResponseData = null,
	//			ResponseDataList = null,
	//			ResponseService = "Master",
	//			ResponseServiceMethod = "GetEquipmentListByLabId"
	//		};
	//	}

	//}
	public ResponseViewModel<MasterViewModel> GetEquipmentListByLabId(string labId)
	{
		try
		{
			List<MasterViewModel> masterViewModelList = _unitOfWork.Repository<Master>()
			.GetQueryAsNoTracking(Q => Q.LabId.ToUpper().StartsWith(labId.ToUpper()))
			.Include(I => I.SupplierModel)
			.Include(I => I.QuarantineModel)
			.Select(s => new MasterViewModel()
			{
				Id = s.Id,
				LabId = s.EquipmentMasterId,
				EquipName = s.EquipName,
				CalibDate = s.CalibDate,
				Range = s.Range
			}).ToList();

			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = masterViewModelList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterService - GetEquipmentListByLabId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Master",
				ResponseServiceMethod = "GetEquipmentListByLabId"
			};
		}

	}
	//public List<DepartmentViewModel> GetMasterDepartmentSubSection()
	//{
	//	try
	//	{// List<UserViewModel> userViewModelList = new List<UserViewModel>();

			
	//		CMTDL _cmtdl = new CMTDL(_configuration);

	//		List<DepartmentViewModel> uv = new List<DepartmentViewModel>();

	//		DataSet ds = _cmtdl.GetMasterEquipmentSubSection();

	//		if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
	//		{
	//			foreach (DataRow dr in ds.Tables[0].Rows)
	//			{
	//				DepartmentViewModel Dept = new DepartmentViewModel
	//				{
	//					Id = Convert.ToInt32(dr["Id"]),
	//					Name = Convert.ToString(dr["Name"]),
	//					SubSectionCode = Convert.ToString(dr["SubSectionCode"]),
	//					SubSection = Convert.ToString(dr["Subsection"]),
	//					Section = Convert.ToString(dr["Section"])
	//				};
	//				uv.Add(Dept);

	//			}
				
	//		}
	//		return uv;
			
			
	//	}
	//	catch (Exception e)
	//	{
	//		ErrorViewModelTest.Log("MasterService - GetMasterDepartmentSubSection Method");
	//		ErrorViewModelTest.Log("exception - " + e.Message);
	//		return null;
	//	}
		

	//}

}

