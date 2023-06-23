using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;

public class QCIntermediateTemplateService : IQCIntermediateTemplateService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	IUtilityService _utilityService;
	public QCIntermediateTemplateService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_utilityService = utilityService;
	}
	public ResponseViewModel<QCIntermediateTemplateViewModel> GetAllIntermediteList()
	{
		try
		{
			List<QCIntermediateTemplateViewModel> intermediateList = new List<QCIntermediateTemplateViewModel>();

			IEnumerable<QCIntermediateTemplate>? intermediateData = _unitOfWork.Repository<QCIntermediateTemplate>()
																					 .GetQueryAsNoTracking()
																					 .Where(x => x.DocumentStatus != (int)(DocumentStatus.Rejected));
			if (intermediateData == null)
			{
				QCIntermediateTemplateViewModel qcIntermediateTemplateViewModel = new QCIntermediateTemplateViewModel();
				intermediateList.Add(qcIntermediateTemplateViewModel);
			}
			else
			{
				intermediateList.AddRange(_mapper.Map<List<QCIntermediateTemplateViewModel>>(intermediateData.ToList()));

				foreach (var data in intermediateList)
				{
					data.StudyPerformedByName = GetUserName(data.StudyPerformedBy);
				}
			}

			return new ResponseViewModel<QCIntermediateTemplateViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = intermediateList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCIntermediateTemplateService - GetAllIntermediteList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QCIntermediateTemplateViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCIntermediateTemplateService",
				ResponseServiceMethod = "GetAllIntermediateList"
			};
		}
	}
	public ResponseViewModel<QCIntermediateTemplateViewModel> GetIntermediateById(int IntermediateId)
	{
		var returnObject = new ResponseViewModel<QCIntermediateTemplateViewModel>();

		try
		{
			var parentData = _unitOfWork.Repository<QCIntermediateTemplate>()
											.GetQueryAsNoTracking(Q => Q.Id == IntermediateId)
											.SingleOrDefault();
			var parentVM = _mapper.Map<QCIntermediateTemplateViewModel>(parentData);

			var childData = _unitOfWork.Repository<QCIntermediateTemplateResult>()
										.GetQueryAsNoTracking(x => x.ParentId == IntermediateId);
			var childListVM = _mapper.Map<List<QCIntermediateTemplateResultViewModel>>(childData);

			parentVM.QCIntermediateResultViewModelList = childListVM;

			if (parentVM.StudyPerformedBy != null)
				parentVM.StudyPerformedByName = GetUserName(parentVM.StudyPerformedBy);

			if (parentVM.StudyPerformedBy != null)
				parentVM.TechnicalManagerName = GetUserName(parentVM.TechnicalManager);

			returnObject.ResponseCode = 200;
			returnObject.ResponseMessage = "Success";
			returnObject.ResponseData = parentVM;
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCIntermediateTemplateService - GetIntermediateById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			returnObject.ResponseCode = 500;
			returnObject.ResponseMessage = "Failure";
			returnObject.ErrorMessage = e.Message;
			returnObject.ResponseService = "QCIntermediateTemplate";
			returnObject.ResponseServiceMethod = "GetIntermediateById";
		}

		return returnObject;

	}
	public string GetUserName(int? Id)
	{
		var userName = string.Empty;

		var user = _unitOfWork.Repository<User>().GetQueryAsNoTracking(x => x.Id == Id);
		if (user.Any())
			userName = user.First().FirstName + " " + user.First().LastName;

		return userName;
	}

	public ResponseViewModel<QCIntermediateTemplateViewModel> InsertData(QCIntermediateTemplateViewModel qcIntermediateTemplateViewModel)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			var parentData = _mapper.Map<QCIntermediateTemplate>(qcIntermediateTemplateViewModel);

			_unitOfWork.Repository<QCIntermediateTemplate>().Insert(parentData);
			_unitOfWork.SaveChanges();

			foreach (var row in qcIntermediateTemplateViewModel.QCIntermediateResultViewModelList)
			{
				row.ParentId = parentData.Id;
			}

			var detailData = _mapper.Map<QCIntermediateTemplateResult[]>(qcIntermediateTemplateViewModel.QCIntermediateResultViewModelList);
			_unitOfWork.Repository<QCIntermediateTemplateResult>().InsertRange(detailData);
			_unitOfWork.SaveChanges();

			_unitOfWork.Commit();

			return new ResponseViewModel<QCIntermediateTemplateViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success"
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCIntermediateTemplateService - InsertData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QCIntermediateTemplateViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCIntermediateTemplateService",
				ResponseServiceMethod = "InsertData"
			};
		}

	}
	public ResponseViewModel<QCIntermediateTemplateViewModel> UpdateData(QCIntermediateTemplateViewModel qcIntermediateTemplateViewModel)
	{
		try
		{
			QCIntermediateTemplate? qcIntermediateTemplate = _unitOfWork.Repository<QCIntermediateTemplate>()
																	 .GetQueryAsNoTracking(Q => Q.Id == qcIntermediateTemplateViewModel.Id)
																	 .SingleOrDefault();

			qcIntermediateTemplate.TechnicalManager = qcIntermediateTemplateViewModel.TechnicalManager;
			qcIntermediateTemplate.TMSignDate = qcIntermediateTemplateViewModel.TMSignDate;
			qcIntermediateTemplate.DocumentStatus = qcIntermediateTemplateViewModel.DocumentStatus;
			qcIntermediateTemplate.TMRemarks = qcIntermediateTemplateViewModel.TMRemarks;

			_unitOfWork.BeginTransaction();
			_unitOfWork.Repository<QCIntermediateTemplate>().Update(qcIntermediateTemplate);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<QCIntermediateTemplateViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("QCIntermediateTemplateService - UpdateData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<QCIntermediateTemplateViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "QCIntermediateTemplateService",
				ResponseServiceMethod = "Update"
			};
		}

	}
}