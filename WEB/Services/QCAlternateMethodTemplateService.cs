using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;

public class QCAlternateMethodTemplateService : IQCAlternateMethodTemplateService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	IUtilityService _utilityService;
	public QCAlternateMethodTemplateService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_utilityService = utilityService;
	}

	public ResponseViewModel<QCAlternateMethodTemplateViewModel> GetAllAlternateMethodList()
	{
		try
		{
			IEnumerable<QCAlternateMethodTemplate>? alternateMethodData = _unitOfWork.Repository<QCAlternateMethodTemplate>()
																					 .GetAll()
																					 .Where(x => x.TemplateStatus != (int)(DocumentStatus.Rejected));

			var alternateMethodList = new List<QCAlternateMethodTemplateViewModel>();
			if (alternateMethodData == null)
			{
				QCAlternateMethodTemplateViewModel qCAlternateMethodTemplateViewModel = new QCAlternateMethodTemplateViewModel();
				alternateMethodList.Add(qCAlternateMethodTemplateViewModel);
			}
			else
			{
				alternateMethodList.AddRange(_mapper.Map<List<QCAlternateMethodTemplateViewModel>>(alternateMethodData.ToList()));
			}
			return new ResponseViewModel<QCAlternateMethodTemplateViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = alternateMethodList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCAlternateMethodTemplateService - GetAllAlternateMethodList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QCAlternateMethodTemplateViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "QCAlternateMethodTemplate",
				ResponseService = "GetAllAlternateMethodList"
			};
		}
	}
	public ResponseViewModel<QCAlternateMethodTemplateViewModel> GetById(int Id)
	{
		var returnObject = new ResponseViewModel<QCAlternateMethodTemplateViewModel>();

		try
		{

			var parentData = _unitOfWork.Repository<QCAlternateMethodTemplate>().GetQueryAsNoTracking(Q => Q.Id == Id).SingleOrDefault();
			var parentVM = _mapper.Map<QCAlternateMethodTemplateViewModel>(parentData);

			var data = _unitOfWork.Repository<QCAlternateMethodTemplateData>().GetQueryAsNoTracking(x => x.ParentId == Id && x.SlNo == 1).SingleOrDefault();
			var dataListVM = _mapper.Map<QCAlternateMethodTemplateDataViewModel>(data);
			parentVM.QCEquipmentOneMeasuredValues = dataListVM;

			data = _unitOfWork.Repository<QCAlternateMethodTemplateData>().GetQueryAsNoTracking(x => x.ParentId == Id && x.SlNo == 2).SingleOrDefault();
			dataListVM = _mapper.Map<QCAlternateMethodTemplateDataViewModel>(data);
			parentVM.QCEquipmentTwoMeasuredValues = dataListVM;
			if (parentVM.ReviewedBy != null)
				parentVM.ReviewedByName = GetUserName(parentVM.ReviewedBy.Value);

			if (parentVM.EvaluatedBy != null)
				parentVM.EvaluatedByName = GetUserName(parentVM.EvaluatedBy.Value);

			returnObject.ResponseCode = 200;
			returnObject.ResponseMessage = "Success";
			returnObject.ResponseData = parentVM;
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCAlternateMethodTemplateService - GetById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			returnObject.ResponseCode = 500;
			returnObject.ResponseMessage = "Failure";
			returnObject.ErrorMessage = e.Message;
			returnObject.ResponseService = "QCAlternateMethodTemplateService";
			returnObject.ResponseServiceMethod = "GetByTemplateData";

		}
		return returnObject;
	}

	public string GetUserName(int Id)
	{
		var userName = string.Empty;

		var user = _unitOfWork.Repository<User>().GetQueryAsNoTracking(x => x.Id == Id);
		if (user.Any())
			userName = user.First().FirstName + " " + user.First().LastName;

		return userName;
	}


	public ResponseViewModel<QCAlternateMethodTemplateViewModel> InsertData(QCAlternateMethodTemplateViewModel qcalternatemethodtemplateViewModel)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			QCAlternateMethodTemplate qcalternateMethodTemplate = new QCAlternateMethodTemplate()
			{
				FormatNo = qcalternatemethodtemplateViewModel.FormatNo,
				RevisionNo = qcalternatemethodtemplateViewModel.RevisionNo,
				RevisionDate = qcalternatemethodtemplateViewModel.RevisionDate,
				InstrumentId = qcalternatemethodtemplateViewModel.InstrumentId,
				RangeOrSize = qcalternatemethodtemplateViewModel.RangeOrSize,
				LC = qcalternatemethodtemplateViewModel.LC,
				InstrumentName = qcalternatemethodtemplateViewModel.InstrumentName,
				MasterId1 = qcalternatemethodtemplateViewModel.MasterId1,
				Master1Name = qcalternatemethodtemplateViewModel.Master1Name,
				Master1DateOfCalibration = qcalternatemethodtemplateViewModel.Master1DateOfCalibration,
				Mux1 = qcalternatemethodtemplateViewModel.Mux1,
				MasterId2 = qcalternatemethodtemplateViewModel.MasterId2,
				Master2Name = qcalternatemethodtemplateViewModel.Master2Name,
				Master2DateOfCalibration = qcalternatemethodtemplateViewModel.Master2DateOfCalibration,
				Mux2 = qcalternatemethodtemplateViewModel.Mux2,
				Mux1AvgValue = qcalternatemethodtemplateViewModel.Mux1AvgValue,
				Mux2AvgValue = qcalternatemethodtemplateViewModel.Mux2AvgValue,
				MesuredValuexOne = qcalternatemethodtemplateViewModel.MesuredValuexOne,
				MesuredValueXTwo = qcalternatemethodtemplateViewModel.MesuredValueXTwo,
				Mux1SqrValue = qcalternatemethodtemplateViewModel.Mux1SqrValue,
				Mux2SqrValue = qcalternatemethodtemplateViewModel.Mux2SqrValue,
				EnValue = qcalternatemethodtemplateViewModel.EnValue,
				Conclusion = qcalternatemethodtemplateViewModel.Conclusion,
				EvaluatedBy = qcalternatemethodtemplateViewModel.EvaluatedBy,
				EvaluationOn = qcalternatemethodtemplateViewModel.EvaluationOn,
				TemplateStatus = qcalternatemethodtemplateViewModel.TemplateStatus,
				DataUnit = qcalternatemethodtemplateViewModel.DataUnit,
				InstrumentLabId = qcalternatemethodtemplateViewModel.InstrumentLabId,
				Master1LabId = qcalternatemethodtemplateViewModel.Master1LabId,
				Master2LabId = qcalternatemethodtemplateViewModel.Master2LabId
			};

			if (qcalternatemethodtemplateViewModel.ImageUpload1 != null)
			{
				string filePath = _utilityService.UploadImage(qcalternatemethodtemplateViewModel.ImageUpload1, Constants.QCAlternative_FolderName);
				IFormFile fileobj1 = qcalternatemethodtemplateViewModel.ImageUpload1;
				qcalternateMethodTemplate.MUx1FileName = fileobj1.FileName;
			}

			if (qcalternatemethodtemplateViewModel.ImageUpload2 != null)
			{
				string filePath = _utilityService.UploadImage(qcalternatemethodtemplateViewModel.ImageUpload2, Constants.QCAlternative_FolderName);
				IFormFile fileobj2 = qcalternatemethodtemplateViewModel.ImageUpload2;
				qcalternateMethodTemplate.MUx2FileName = fileobj2.FileName;
			}

			_unitOfWork.Repository<QCAlternateMethodTemplate>().Insert(qcalternateMethodTemplate);
			_unitOfWork.SaveChanges();


			qcalternatemethodtemplateViewModel.QCEquipmentOneMeasuredValues.ParentId = qcalternateMethodTemplate.Id;
			qcalternatemethodtemplateViewModel.QCEquipmentTwoMeasuredValues.ParentId = qcalternateMethodTemplate.Id;

			List<QCAlternateMethodTemplateDataViewModel> qcAlternateChildDataList = new List<QCAlternateMethodTemplateDataViewModel>();
			qcAlternateChildDataList.Add(qcalternatemethodtemplateViewModel.QCEquipmentOneMeasuredValues);
			qcAlternateChildDataList.Add(qcalternatemethodtemplateViewModel.QCEquipmentTwoMeasuredValues);

			_unitOfWork.Repository<QCAlternateMethodTemplateData>().InsertRange(_mapper.Map<QCAlternateMethodTemplateData[]>(qcAlternateChildDataList));
			_unitOfWork.SaveChanges();


			_unitOfWork.Commit();

			return new ResponseViewModel<QCAlternateMethodTemplateViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};

		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCAlternateMethodTemplateService - InsertData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QCAlternateMethodTemplateViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCAlternateMethodTemplate",
				ResponseServiceMethod = "InsertData"
			};
		}

	}
	public ResponseViewModel<QCAlternateMethodTemplateViewModel> UpdateData(QCAlternateMethodTemplateViewModel qcalternatemethodtemplateViewModel)
	{
		try
		{
			QCAlternateMethodTemplate? qcalternateMethodTemplate = _unitOfWork.Repository<QCAlternateMethodTemplate>()
												   .GetQueryAsNoTracking(Q => Q.Id == qcalternatemethodtemplateViewModel.Id)
												   .SingleOrDefault();

			qcalternateMethodTemplate.ReviewerRemarks = qcalternatemethodtemplateViewModel.ReviewerRemarks;
			qcalternateMethodTemplate.ReviewedOn = qcalternatemethodtemplateViewModel.ReviewedOn;
			qcalternateMethodTemplate.ReviewedBy = qcalternatemethodtemplateViewModel.ReviewedBy;
			qcalternateMethodTemplate.TemplateStatus = qcalternatemethodtemplateViewModel.TemplateStatus;

			_unitOfWork.BeginTransaction();
			_unitOfWork.Repository<QCAlternateMethodTemplate>().Update(qcalternateMethodTemplate);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<QCAlternateMethodTemplateViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCAlternateMethodTemplateService - UpdateData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QCAlternateMethodTemplateViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCAlternateMethodTemplateService",
				ResponseServiceMethod = "Update"
			};
		}
	}
}