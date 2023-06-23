using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace WEB.Services;


public class MasterHistoryService : IMasterHistoryService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	private IUtilityService _utilityService;
	public MasterHistoryService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_utilityService = utilityService;
	}
	public ResponseViewModel<MasterHistoryViewModel> GetMasterHistoryListbyId(int MasterId)
	{
		try
		{
			List<MasterHistoryViewModel> masterHistoryViewModelList = _unitOfWork.Repository<MasterEquipmentHistory>().GetQueryAsNoTracking(Q => Q.MasterId == MasterId).Select(s => new MasterHistoryViewModel()
			{
				Id = s.Id,
				MasterId = s.MasterId,
				Date = s.Date,
				Category = s.Category,
				Description = s.Description,
				Actions_Taken = s.Actions_Taken,
				FromDate = s.FromDate,
				ToDate = s.ToDate,
				Date_of_Completion = s.Date_of_Completion,
				Breakdown_hrs_days = s.Breakdown_hrs_days,
				Status = s.Status,
				Maintainence = s.Maintainence,
				CreatedOn = s.CreatedOn,
				CreatedBy = s.CreatedBy,
				StatusId = s.StatusId
			}).ToList();
			return new ResponseViewModel<MasterHistoryViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = masterHistoryViewModelList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterHistoryService - GetMasterHistoryListbyId Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterHistoryViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "MasterHistory",
				ResponseService = "GetMasterHistoryListbyId"
			};
		}
	}

	public ResponseViewModel<MasterHistoryViewModel> CreateNewMasterHistory()
	{
		try
		{
			MasterHistoryViewModel masterHistoryViewModel = new MasterHistoryViewModel();
			masterHistoryViewModel.Date = DateTime.Now;
			return new ResponseViewModel<MasterHistoryViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = masterHistoryViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterHistoryService - CreateNewMasterHistory Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterHistoryViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "MasterHistory",
				ResponseServiceMethod = "CreateNewMasterHistory"
			};
		}
	}
	public ResponseViewModel<MasterHistoryViewModel> InsertMaster(MasterHistoryViewModel master)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			MasterEquipmentHistory newMaster = new MasterEquipmentHistory();
			newMaster = _mapper.Map<MasterEquipmentHistory>(master);
			_unitOfWork.Repository<MasterEquipmentHistory>().Insert(newMaster);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<MasterHistoryViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("MasterHistoryService - InsertMaster Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<MasterHistoryViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "MasterHistory",
				ResponseServiceMethod = "InsertMaster"
			};
		}
	}


}

