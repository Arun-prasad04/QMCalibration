
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services;
using WEB.Services.Interface;
namespace WEB.Controllers;

public class TrackerController : BaseController
{
	private IExternalRequestService _externalRequestService { get; set; }
	private IRequestService _requestService { get; set; }
	private IMasterService _masterService { get; set; }
	public TrackerController(ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IExternalRequestService externalRequestService, IRequestService requestService, IMasterService masterService) : base(logger, contextAccessor)
	{
		_externalRequestService = externalRequestService;
		_requestService = requestService;
		_masterService = masterService;
	}

	public IActionResult ExternalRequest()
	{
		ViewBag.PageTitle = "ExternalRequest";
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.GetAllExternalRequestList();
		return View(response.ResponseDataList);
	}
	public IActionResult ExternalRequestGetById(int externalRequestId)
	{
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.GetExternalRequestById(externalRequestId);
		return Json(response.ResponseData);
	}

	public IActionResult ExternalRequestNew()
	{
		int externalRequestId = int.Parse(HttpContext.Request.Query["Id"].ToString());
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.GetExternalRequestById(externalRequestId);
		return View(response.ResponseData);
	}


	public IActionResult AcceptExternalRequest(int externalRequestId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.AcceptExternalRequest(externalRequestId, userId);
		return Json(response.ResponseData);
	}
	public IActionResult RejectExternalRequest(int externalRequestId, string rejectReason)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.RejectExternalRequest(externalRequestId, rejectReason, userId);
		return Json(response.ResponseData);
	}

	public IActionResult SubmitFMExternalRequest(int externalRequestId, string Result)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.SubmitFMExternalRequest(externalRequestId, Result, userId);
		return Json(response.ResponseData);
	}

	public IActionResult SubmitLABExternalRequest(int externalRequestId, string Result)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.SubmitLABExternalRequest(externalRequestId, Result, userId);
		return Json(response.ResponseData);
	}
	public IActionResult Request(int? reqType)
	{
		 ViewBag.PageTitle = "Request";
		ViewBag.ReqType = reqType;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		string SessionLang = base.SessionGetString("Language");
		ResponseViewModel<MasterViewModel> masterResponse = _masterService.GetAllMasterList(SessionLang);
		ViewBag.MasterData = masterResponse.ResponseDataList;
		ResponseViewModel<RequestViewModel> response = _requestService.GetAllRequestList(userRoleId, userId);

		if (reqType == 1 && response.ResponseDataList != null)
		{
			return View(response.ResponseDataList.Where(W => W.TypeOfRequest == 1).ToList());
		}
		else if (reqType == 2 && response.ResponseDataList != null)
		{
			return View(response.ResponseDataList.Where(W => W.TypeOfRequest == 2).ToList());
		}
		else if (reqType == 3 && response.ResponseDataList != null)
		{
			return View(response.ResponseDataList.Where(W => W.TypeOfRequest == 3).ToList());
		}
		else
		{
			return View(response.ResponseDataList);
		}
	}
	public IActionResult GetRequestById(int requestId)
	{
		ResponseViewModel<RequestViewModel> response = _requestService.GetRequestById(requestId);
		return Json(response.ResponseData);
	}

	public IActionResult RequestDetails()
	{
		//ResponseViewModel<RequestViewModel> response=_requestService.GetRequestById(requestId);
		return View();
	}
	public IActionResult RequestDetailsNew()
	{
		int requestId = int.Parse(HttpContext.Request.Query["ID"].ToString());
		string SessionLang = base.SessionGetString("Language");
		ResponseViewModel<MasterViewModel> masterResponse = _masterService.GetAllMasterList(SessionLang);
		ViewBag.MasterData = masterResponse.ResponseDataList;
		ResponseViewModel<RequestViewModel> response = _requestService.GetRequestById(requestId);
		return View(response.ResponseData);
	}

	public IActionResult AcceptRequest(int requestId, string InstrumentCondition
	, string Feasiblity, DateTime TentativeCompletionDate, int newObservation
	, int newObservationType, int newMU, int newCertification, string standardReffered, bool newNABL, int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.AcceptRequest(requestId, userId, InstrumentCondition, Feasiblity, TentativeCompletionDate, newObservation, newObservationType, newMU, newCertification, standardReffered, newNABL, MasterInstrument1, MasterInstrument2, MasterInstrument3, MasterInstrument4);
		return Json(response.ResponseData);
	}
	public IActionResult AcceptRequestReCalibration(int requestId, int AcceptValue, int departmentId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.AcceptRequestRecalibration(requestId, userId, AcceptValue, departmentId);
		return Json(response.ResponseData);
	}
	public IActionResult RejectRequest(int requestId, string rejectReason, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string standardReffered)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.RejectRequest(requestId, rejectReason, userId, InstrumentCondition, Feasiblity, TentativeCompletionDate, standardReffered);
		return Json(response.ResponseData);
	}

	public IActionResult SubmitDepartmentRequestVisual(int requestId, string Result, string CollectedBy)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.SubmitDepartmentRequestVisual(requestId, Result, userId, CollectedBy);
		return Json(response.ResponseData);
	}

	public IActionResult SubmitLABRequestVisual(int requestId, string Result, string RecordBy, DateTime ClosedDate, string Remarks, DateTime InstrumentReturnedDate, string CollectedBy, string ReasonforRejection, string IsFeasibleService, string IsFeasibleYes, string ServiceResponsibility, int RequestType, int InstrumentId, DateTime CalibDate, DateTime DueDate, bool newNABL, List<UploadFile> FileData, string IdNo)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.SubmitLABRequestVisual(requestId, Result, userId, RecordBy, ClosedDate, Remarks, InstrumentReturnedDate, CollectedBy, ReasonforRejection, IsFeasibleService, IsFeasibleYes, ServiceResponsibility, RequestType, InstrumentId, CalibDate, DueDate, newNABL, FileData, IdNo);
		return Json(response.ResponseData);
	}
	public IActionResult SubmitLABAdminUpdates(int requestId, int ObservationTemplate, int ObservationTemplateType, int MUTemplate, int CertificationTemplate)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<InstrumentViewModel> response = _requestService.SubmitLABAdminUpdates(requestId, ObservationTemplate, ObservationTemplateType, MUTemplate, CertificationTemplate);
		return Json(response.ResponseData);
	}

	public IActionResult ApproveNewRequest(int requestId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.SubmitNewRequest(requestId, userId, InstrumentCondition, Feasiblity, TentativeCompletionDate);
		return Json(response.ResponseData);
	}

	public IActionResult ApproveQuarRequest(int requestId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.SubmitQuarantineRequest(requestId, userId);
		return Json(response.ResponseData);
	}
	public IActionResult LoadObservationType(string attrType, string attrsubType)
	{
		ResponseViewModel<LovsViewModel> response = _requestService.GetLovs(attrType, attrsubType);
		return Json(response.ResponseDataList);
	}
	public IActionResult SaveInstrumentFromRequest(int requestId,
			string newLabId,
			bool newNABL,
			int newObservation,
			int newObservationType,
			int newMU,
			int newCertification,
			int masterInstrument1,
			int masterInstrument2,
			int masterInstrument3,
			int masterInstrument4,
			string calibSource,
			string standardReffered,
			DateTime calibDate,
			DateTime dueDate,
			DateTime dateOfReceipt)
	{
		ResponseViewModel<RequestViewModel> response = _requestService.SaveInstrumentData(requestId,
		newLabId,
		newNABL,
		newObservation,
		newObservationType,
		newMU,
		newCertification,
		masterInstrument1,
		masterInstrument2,
		masterInstrument3,
		masterInstrument4,
	   calibSource,
		standardReffered,
	   calibDate,
	   dueDate,
		dateOfReceipt);
		return Json(response.ResponseData);
	}
}
