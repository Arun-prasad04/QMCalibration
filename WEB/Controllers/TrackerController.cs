
using System.Diagnostics;
using CMT.DATAMODELS;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services;
using WEB.Services.Interface;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

namespace WEB.Controllers;

public class TrackerController : BaseController
{
	private IExternalRequestService _externalRequestService { get; set; }
	private IRequestService _requestService { get; set; }
	private IMasterService _masterService { get; set; }
	private IConfiguration _configuration;
	public TrackerController(ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IExternalRequestService externalRequestService, IRequestService requestService, IMasterService masterService, IConfiguration Configuration) : base(logger, contextAccessor)
	{
		_externalRequestService = externalRequestService;
		_requestService = requestService;
		_masterService = masterService;
		_configuration = Configuration;		
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
		// ViewBag.PageTitle = "Request";
		ViewBag.ReqType = reqType;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		string SessionLang = base.SessionGetString("Language");
		//ResponseViewModel<MasterViewModel> masterResponse = _masterService.GetAllMasterList(SessionLang);
		//ViewBag.MasterData = masterResponse.ResponseDataList;
		//ResponseViewModel<RequestViewModel> response = _requestService.GetAllRequestList(userRoleId, userId);

		//if (reqType == 1 && response.ResponseDataList != null)
		//{
		//	return View(response.ResponseDataList.Where(W => W.TypeOfRequest == 1).ToList());
		//}
		//else if (reqType == 2 && response.ResponseDataList != null)
		//{
		//	return View(response.ResponseDataList.Where(W => W.TypeOfRequest == 2).ToList());
		//}
		//else if (reqType == 3 && response.ResponseDataList != null)
		//{
		//	return View(response.ResponseDataList.Where(W => W.TypeOfRequest == 3).ToList());
		//}
		//else
		//{
		//	return View(response.ResponseDataList);
		//}
		return View();
    }
    public IActionResult GetAllRequestList(int reqType)
	{
		//int reqType = 4;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		//int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<RequestViewModel> response = _requestService.GetAllRequestList(userRoleId, userId);

        if (reqType == 1 && response.ResponseDataList != null)
        {
            return Json(response.ResponseDataList.Where(W => W.TypeOfRequest == 1).ToList());
        }
        else if (reqType == 2 && response.ResponseDataList != null)
        {
            return Json(response.ResponseDataList.Where(W => W.TypeOfRequest == 2).ToList());
        }
        else if (reqType == 3 && response.ResponseDataList != null)
        {
            return Json(response.ResponseDataList.Where(W => W.TypeOfRequest == 3).ToList());
        }
        else
        {
            return Json(response.ResponseDataList);
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
        ViewBag.SessionLang = base.SessionGetString("Language");
		ResponseViewModel<MasterViewModel> masterResponse = _masterService.GetAllMasterList(SessionLang);
		ViewBag.MasterData = masterResponse.ResponseDataList;
		ResponseViewModel<RequestViewModel> response = _requestService.GetRequestById(requestId);
		//ViewBag.ObservationType = response.ResponseData.ObservationType;
		//ViewBag.ObservationTypeList = response.ResponseData.LovsList;
		ViewBag.CalibFreqExternal = response.ResponseData.CalibFreq;

		return View(response.ResponseData);
	}

	public IActionResult AcceptRequest(int requestId, string InstrumentCondition, int DueDate
    , string Scope, DateTime TentativeCompletionDate, int newObservation, int CalibFreq, string ToolInventory
    , int newObservationType, int newMU, int newCertification, string standardReffered, bool newNABL, int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4)
	{		
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.AcceptRequest(requestId, userId, InstrumentCondition, Scope, TentativeCompletionDate, CalibFreq, ToolInventory, newObservation, newObservationType, newMU, newCertification, standardReffered, newNABL, MasterInstrument1, MasterInstrument2, MasterInstrument3, MasterInstrument4, DueDate);
		return Json(response.ResponseData);
	}
	public IActionResult AcceptRequestReCalibration(int requestId, int AcceptValue, int departmentId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.AcceptRequestRecalibration(requestId, userId, AcceptValue, departmentId);
		return Json(response.ResponseData);
	}
	public IActionResult RejectRequest(int requestId, string rejectReason, string InstrumentCondition, string Scope,string ToolInventory, DateTime TentativeCompletionDate, string standardReffered)
	{
		//return View();
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.RejectRequest(requestId, rejectReason, userId, InstrumentCondition, Scope, ToolInventory, TentativeCompletionDate, standardReffered);
		return Json(response.ResponseData);
	}

	public IActionResult SubmitDepartmentRequestVisual(int requestId, string Result, string CollectedBy, string InstrumentIdNo, int CalibFreq)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.SubmitDepartmentRequestVisual(requestId, Result, userId, CollectedBy, InstrumentIdNo, CalibFreq);
		//return RedirectToAction("Index", "Home"); 
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
	public IActionResult LoadObservationType(string attrType, string attrsubType, string LangType)
	{
		ResponseViewModel<LovsViewModel> response = _requestService.GetLovs(attrType, attrsubType, LangType);
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

    public IActionResult ExternalRejectRequest(int requestId, string rejectReason, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string standardReffered)
    {
        //return View();
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response = _requestService.ExternalRejectRequest(requestId, rejectReason, userId, InstrumentCondition, Feasiblity, TentativeCompletionDate, standardReffered);
        return Json(response.ResponseData);
    }

	public IActionResult ExternalAcceptRequest(int requestId, string acceptReason, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string InstrumentIdNo, string ReceivedBy, IFormFile httpPostedFileBase, string StandardReffered, int CalibFreq, int DueDate)	
	{
		//return Json(true);
        int UserId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response = _requestService.ExternalAcceptRequest(requestId, UserId, InstrumentCondition, Feasiblity, TentativeCompletionDate, InstrumentIdNo, acceptReason, ReceivedBy, httpPostedFileBase, StandardReffered, CalibFreq, DueDate);
        return Json(response.ResponseData);
    }

	public IActionResult DueInstrument(int month)
	{
		ViewBag.DueMonth = month;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		CMTDL _cmtdl = new CMTDL(_configuration);
		List<DueInstrument> response = _cmtdl.GetAllDueInstrumentList(month);
		return View(response);
	}

	//public IActionResult GetAllDueInstrumentList()
	//{
	//	int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
	//	int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
	//	CMTDL _cmtdl = new CMTDL(_configuration);
	//	List<DueInstrument> response = _cmtdl.GetAllDueInstrumentList();
	//	return View(response);
	//}

	public IActionResult DueInstrumentAdminApprove(List<DueInstrument> DueList) 
	{
        //return Json(true);
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response;
        response = _requestService.DueInstrumentAdminApprove(DueList, userId);
        return Json(true); 
	}

    public IActionResult DueTracker()
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<DueInstrument> response = _requestService.GetAdminApproveInstrumentList();
        return View(response.ResponseDataList);
    }

    public IActionResult DueInstrumentManagerApprove(List<DueInstrument> DueList)
    {
        //return Json(true);
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response;
        response = _requestService.DueInstrumentManagerApprove(DueList, userId);
        return Json(true);
    }

    public IActionResult ExternalUserSubmit(int requestId, IFormFile httpPostedFileBase)
    {
       // return Json(true);
        int UserId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response = _requestService.ExternalUserSubmit(requestId, UserId, httpPostedFileBase);
        return Json(response.ResponseData);
    }

	public IActionResult SaveExternalObs(int requestId,int InstrumentID, string InstrumentIdNo, int CalibFreq)
	{
        //return Json(true);
        int UserId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response = _requestService.SaveExternalObs(requestId, InstrumentID, UserId, InstrumentIdNo, CalibFreq);
        return Json(true);
	}

    public IActionResult ExternalCalibrationReject(int requestId, string rejectReason)
    {
        //return View();
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<RequestViewModel> response = _requestService.ExternalCalibrationReject(requestId, rejectReason, userId);
        return Json(response.ResponseData);
    }
}
