using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;

public class QCReplicateTestTemplateService : IQCReplicateTestTemplateService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	IUtilityService _utilityService;
	public QCReplicateTestTemplateService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_utilityService = utilityService;
	}
	public ResponseViewModel<ReplicateTestViewModel> GetByTemplateData(int ReplicateId)
	{
		try
		{
			QCReplicateTestTemplate qcReplicateTestTemplateModel = _unitOfWork.Repository<QCReplicateTestTemplate>().GetQueryAsNoTracking(x => x.Id == ReplicateId).SingleOrDefault();
			ReplicateTestViewModel qcReplicateTestTemplateviewModel = _mapper.Map<ReplicateTestViewModel>(qcReplicateTestTemplateModel);

			if (qcReplicateTestTemplateviewModel.ReviewedBy != null)
				qcReplicateTestTemplateviewModel.ReviewedByName = GetUserName(qcReplicateTestTemplateviewModel.ReviewedBy);

			QCReplicateTestTemplateData data = _unitOfWork.Repository<QCReplicateTestTemplateData>().GetQueryAsNoTracking(x => x.ParentId == ReplicateId && x.SINo == 1).SingleOrDefault();
			ReplicateTestDataViewModel ReplicateTemplateDetailVMList = _mapper.Map<ReplicateTestDataViewModel>(data);
			qcReplicateTestTemplateviewModel.Obs1 = ReplicateTemplateDetailVMList;

			if (qcReplicateTestTemplateviewModel.Obs1 != null && qcReplicateTestTemplateviewModel.Obs1.AppraiserName != null)
				qcReplicateTestTemplateviewModel.Obs1.AppraiserFullName = GetUserName(qcReplicateTestTemplateviewModel.Obs1.AppraiserName);

			data = _unitOfWork.Repository<QCReplicateTestTemplateData>().GetQueryAsNoTracking(x => x.ParentId == ReplicateId && x.SINo == 2).SingleOrDefault();
			ReplicateTemplateDetailVMList = _mapper.Map<ReplicateTestDataViewModel>(data);
			qcReplicateTestTemplateviewModel.Obs2 = ReplicateTemplateDetailVMList;

			if (qcReplicateTestTemplateviewModel.Obs2 != null && qcReplicateTestTemplateviewModel.Obs2.AppraiserName != null)
				qcReplicateTestTemplateviewModel.Obs2.AppraiserFullName = GetUserName(qcReplicateTestTemplateviewModel.Obs2.AppraiserName);

			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = qcReplicateTestTemplateviewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCReplicateTestTemplateService - GetByTemplateData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCReplicateTestTemplateService",
				ResponseServiceMethod = "GetByTemplateData"
			};
		}

	}

	public ResponseViewModel<ReplicateTestViewModel> GetReplicateTestGridData()
	{
		try
		{
			var data = _unitOfWork.Repository<QCReplicateTestTemplate>().GetAll()
																		.Where(x => x.DocumentStatus != (int)(ReplicateTestStatus.Rejected))
																		.OrderByDescending(x => x.CreatedOn);

			var ReplicateTestList = _mapper.Map<List<ReplicateTestViewModel>>(data);
			foreach (var replicateData in ReplicateTestList)
			{
				if (replicateData.DocumentStatus == (int)ReplicateTestStatus.ResultOneSubmitted)
				{
					replicateData.DocumentStatusName = Constants.RESULT_ONE_SUBMITTED;
				}
				else if (replicateData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
				{
					replicateData.DocumentStatusName = Constants.RESULT_TWO_SUBMITTED;
				}
				else if (replicateData.DocumentStatus == (int)ReplicateTestStatus.Approved)
				{
					replicateData.DocumentStatusName = Constants.APPROVED;
				}
				else if (replicateData.DocumentStatus == (int)ReplicateTestStatus.Rejected)
				{
					replicateData.DocumentStatusName = Constants.REJECTED;
				}
			}

			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ReplicateTestList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCReplicateTestTemplateService - GetReplicateTestGridData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "QCReplicateTestTemplate",
				ResponseService = "GetReplicateTestGridData"
			};
		}
	}
	public ResponseViewModel<ReplicateTestViewModel> InsertData(ReplicateTestViewModel qcReplicateTestTemplateViewModel)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			QCReplicateTestTemplate qcReplicateTestTemplate = new QCReplicateTestTemplate()
			{
				Id = qcReplicateTestTemplateViewModel.Id,
				FormatNo = qcReplicateTestTemplateViewModel.FormatNo,
				RevisionNo = qcReplicateTestTemplateViewModel.RevisionNo,
				RevisionDate = qcReplicateTestTemplateViewModel.RevisionDate,
				DateConducted = qcReplicateTestTemplateViewModel.DateConducted,
				InstrumentId = 0,
				RangeOrSize = qcReplicateTestTemplateViewModel.RangeOrSize,
				LC = qcReplicateTestTemplateViewModel.LC,
				InstrumentName = qcReplicateTestTemplateViewModel.InstrumentName,
				MasterEquipmentId = 0,
				MasterEquipmentName = qcReplicateTestTemplateViewModel.MasterEquipmentName,
				Temperature = qcReplicateTestTemplateViewModel.Temperature,
				Humidity = qcReplicateTestTemplateViewModel.Humidity,
				EnValue = qcReplicateTestTemplateViewModel.EnValue,
				Conclusion = qcReplicateTestTemplateViewModel.Conclusion,
				DocumentStatus = Convert.ToInt16(qcReplicateTestTemplateViewModel.DocumentStatus),
				CreatedBy = qcReplicateTestTemplateViewModel.CreatedBy,
				CreatedOn = qcReplicateTestTemplateViewModel.CreatedOn,
				DataUnit = qcReplicateTestTemplateViewModel.DataUnit,
				MasterLabId = qcReplicateTestTemplateViewModel.MasterLabId,
				InstrumentLabId = qcReplicateTestTemplateViewModel.InstrumentLabId
			};

			if (qcReplicateTestTemplateViewModel.ImageUpload1 != null)
			{
				string filePath = _utilityService.UploadImage(qcReplicateTestTemplateViewModel.ImageUpload1,
															  Constants.QCReplicateTest_FolderName);
				IFormFile fileobj1 = qcReplicateTestTemplateViewModel.ImageUpload1;
				qcReplicateTestTemplate.MUx1FileName = fileobj1.FileName;
			}

			_unitOfWork.Repository<QCReplicateTestTemplate>().Insert(qcReplicateTestTemplate);
			_unitOfWork.SaveChanges();

			qcReplicateTestTemplateViewModel.Obs1.ParentId = qcReplicateTestTemplate.Id;
			qcReplicateTestTemplateViewModel.Obs1.SINo = 1;

			List<ReplicateTestDataViewModel> qcReplicateTestChildListData = new List<ReplicateTestDataViewModel>();
			qcReplicateTestChildListData.Add(qcReplicateTestTemplateViewModel.Obs1);
			_unitOfWork.Repository<QCReplicateTestTemplateData>().InsertRange(_mapper.Map<QCReplicateTestTemplateData[]>(qcReplicateTestChildListData));
			_unitOfWork.SaveChanges();

			_unitOfWork.Commit();

			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCReplicateTestTemplateService - InsertData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCReplicateTestTemplateService",
				ResponseServiceMethod = "InsertData"
			};
		}

	}
	public ResponseViewModel<ReplicateTestViewModel> UpdateData(ReplicateTestViewModel qcReplicateTestUpdateData)
	{
		try
		{
			QCReplicateTestTemplate qcReplicateTestTemplate = _unitOfWork.Repository<QCReplicateTestTemplate>()
																		 .GetQueryAsNoTracking(Q => Q.Id == qcReplicateTestUpdateData.Id)
																		 .SingleOrDefault();
			if (qcReplicateTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
			{
				qcReplicateTestTemplate.EnValue = qcReplicateTestUpdateData.EnValue;
				qcReplicateTestTemplate.DocumentStatus = Convert.ToInt16(qcReplicateTestUpdateData.DocumentStatus);
				qcReplicateTestTemplate.ModifiedBy = qcReplicateTestUpdateData.ModifiedBy;
				qcReplicateTestTemplate.ModifiedOn = qcReplicateTestUpdateData.ModifiedOn;
				qcReplicateTestTemplate.Conclusion = qcReplicateTestUpdateData.Conclusion;

				if (qcReplicateTestUpdateData.ImageUpload2 != null)
				{
					string filePath = _utilityService.UploadImage(qcReplicateTestUpdateData.ImageUpload2,
																Constants.QCReplicateTest_FolderName);
					IFormFile fileobj2 = qcReplicateTestUpdateData.ImageUpload2;
					qcReplicateTestTemplate.MUx2FileName = fileobj2.FileName;
				}

			}
			if (qcReplicateTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.Approved
					|| qcReplicateTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.Rejected)
			{
				qcReplicateTestTemplate.DocumentStatus = Convert.ToInt16(qcReplicateTestUpdateData.DocumentStatus);
				qcReplicateTestTemplate.ReviewedBy = qcReplicateTestUpdateData.ReviewedBy;
				qcReplicateTestTemplate.ReviewedOn = qcReplicateTestUpdateData.ReviewedOn;
				qcReplicateTestTemplate.FinalStatus = qcReplicateTestUpdateData.FinalStatus;
			}
			_unitOfWork.BeginTransaction();
			_unitOfWork.Repository<QCReplicateTestTemplate>().Update(qcReplicateTestTemplate);
			_unitOfWork.SaveChanges();
			if (qcReplicateTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
			{
				List<ReplicateTestDataViewModel> qcReplicateTestChildListData = new List<ReplicateTestDataViewModel>();
				qcReplicateTestUpdateData.Obs2.SINo = 2;
				qcReplicateTestUpdateData.Obs2.ParentId = qcReplicateTestTemplate.Id;
				qcReplicateTestChildListData.Add(qcReplicateTestUpdateData.Obs2);
				_unitOfWork.Repository<QCReplicateTestTemplateData>().InsertRange(_mapper.Map<QCReplicateTestTemplateData[]>
																					(qcReplicateTestChildListData));
				_unitOfWork.SaveChanges();

			}
			_unitOfWork.Commit();
			return new ResponseViewModel<ReplicateTestViewModel>
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
			ErrorViewModelTest.Log("QCReplicateTestTemplateService - UpdateData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ReplicateTestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = qcReplicateTestUpdateData,
				ResponseDataList = null,
				ResponseService = "QCReplicateTestTemplateService",
				ResponseServiceMethod = "UpdateData"
			};
		}
	}

	public string GetUserName(string shortId)
	{
		var userName = string.Empty;

		var user = _unitOfWork.Repository<User>().GetQueryAsNoTracking(x => x.ShortId == shortId);
		if (user.Any())
			userName = user.First().FirstName + " " + user.First().LastName;

		return userName;
	}
}