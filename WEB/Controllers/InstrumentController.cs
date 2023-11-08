
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using CMT.DAL;
using CMT.DATAMODELS;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;
using WEB.Models;
using WEB.Services;
using WEB.Services.Interface;
using static iTextSharp.text.pdf.PdfDocument;

namespace WEB.Controllers;

public class InstrumentController : BaseController
{
	private IInstrumentService _instrumentService { get; set; }
	private IRequestService _requestService { get; set; }
	private IUnitOfWork _unitOfWork { get; set; }
	private IQRCodeGeneratorService _qrCodeGeneratorService { get; set; }
	public InstrumentController(IInstrumentService instrumentService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IRequestService requestService, IUnitOfWork unitOfWork, IQRCodeGeneratorService qrCodeGeneratorService) : base(logger, contextAccessor)
	{
		_instrumentService = instrumentService;
		_requestService = requestService;
		_unitOfWork = unitOfWork;
		_qrCodeGeneratorService = qrCodeGeneratorService;
	}

	public IActionResult Index()
	{
		//ViewBag.PageTitle="Instrument List";
		ViewBag.ResponseCode = TempData["ResponseCode"];
		ViewBag.ResponseMessage = TempData["ResponseMessage"];
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList(userId, userRoleId);
		return View(response.ResponseDataList);
		//return View();

	}

	public JsonResult GetAllInstrumentList()
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList(userId, userRoleId);
		return Json(response.ResponseDataList);
	}

	public IActionResult Create()
	{
		ViewBag.PageTitle = "Instrument Create";
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.CreateNewInstrument(userId, userRoleId);
		//ViewBag.ObservationType = response.ResponseData.ObservationType;
		ViewBag.UserBaseDepartment = response.ResponseData.Departments;
		ViewBag.ShowDetails = true;
		return View(response.ResponseData);
	}

	public IActionResult InsertInstrument(InstrumentViewModel instrument)
	{
		//return Json(true);
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int UserDeptId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response;
		if (instrument.Id != null && instrument.Id > 0)
		{
			instrument.ModifiedBy = userId;
			instrument.ModifiedOn = DateTime.Now;
			instrument.CreatedOn = DateTime.Now;
			//instrument.UserDept=Convert.ToInt32(base.SessionGetString("DepartmentId"));
			instrument.UserRoleId = userRoleId;
			response = _instrumentService.UpdateInstrument(instrument);
		}
		else
		{
			instrument.ActiveStatus = true;
			instrument.CreatedBy = userId;
			instrument.ModifiedBy = userId;
			instrument.CreatedOn = DateTime.Now;
			instrument.ModifiedOn = DateTime.Now;
			instrument.DateOfReceipt = DateTime.Now;
			instrument.DueDate = DateTime.Now;
			instrument.CalibDate = DateTime.Now;
			//instrument.UserDept=Convert.ToInt32(base.SessionGetString("DepartmentId"));
			//if(userRoleId != 2)
			//{
			//    instrument.UserDept = UserDeptId;
			//}
			response = _instrumentService.InsertInstrument(instrument);
		}
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		if (response.ResponseMessage == "Success")
		{
			return RedirectToAction("Index", "Instrument");
		}
		else
		{
			return Json(response.ResponseData);
		}
	}

	public ActionResult InstrumentEdit(int instrumentId)
	{
		ViewBag.PageTitle = "Instrument Edit";
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));

		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentById(instrumentId);

		ViewBag.ObservationType = response.ResponseData.ObservationType;
		ViewBag.UserDept = response.ResponseData.UserDept;
		ViewBag.CertificationTemplate = response.ResponseData.CertificationTemplate;
		ViewBag.CalibFreq = response.ResponseData.CalibFreq;
		ViewBag.MUTemplates = response.ResponseData.MUTemplate;
		ViewBag.Observation = response.ResponseData.ObservationTemplate;

		if (userRoleId == 1 || userRoleId == 3)
		{
			response.ResponseData.IsDisabled = "readonly";
		}


		Request NewRequest = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.InstrumentId == instrumentId).OrderByDescending(O => O.Id).FirstOrDefault();
		if (NewRequest.TypeOfReqest == 2 || NewRequest.TypeOfReqest == 3 || NewRequest.StatusId == 30)
		{
			ViewBag.ShowDetails = false;
			response.ResponseData.IsDisabled = "readonly";
		}
		else
		{
			ViewBag.ShowDetails = true;
		}
		return View("Create", response.ResponseData);
	}

	public ActionResult InstrumentDelete(int instrumentId)
	{
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.DeleteInstrument(instrumentId);
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		return RedirectToAction("Index", "Instrument");
	}
	public IActionResult QuratineList()
	{
		ViewBag.PageTitle = "Instrument QuratineList";
		ViewBag.ResponseCode = TempData["ResponseCode"];
		ViewBag.ResponseMessage = TempData["ResponseMessage"];
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));

		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentQuarantineList(userId, userRoleId);
		return View(response.ResponseDataList);
	}

	public IActionResult InstrumentQuarantine(int instrumentId, string reason)
	{
		int statusId = 1;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.InstrumentQuarantine(instrumentId, reason, userId, statusId);
		return Json(response.ResponseData);
	}
	public IActionResult InstrumentRemoveQuarantine(int instrumentId)
	{
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<InstrumentViewModel> response = new ResponseViewModel<InstrumentViewModel>();
		if (userRoleId == 1)
		{
			response = _instrumentService.InstrumentRemoveQuarantine(instrumentId, 3, userId);
		}
		else if (userRoleId == 2 || userRoleId == 4)
		{
			response = _instrumentService.InstrumentRemoveQuarantine(instrumentId, 2, userId);
		}
		return Json(response.ResponseData);
	}

	public IActionResult Request(int instumentId, int typeId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response = _requestService.InsertRequest(instumentId, userId, typeId);
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		return RedirectToAction("Index", "Instrument");
	}
	//Due For Calibration 

	public JsonResult PopUpInstrumentList(string InstrumentName, int InstrumentId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.PopUpList(InstrumentName, InstrumentId);

		return Json(response.ResponseDataList);
	}


	public ActionResult RegularRecaliRequest(List<RequestAllView> userViewModelList)
	{
		//return Json(true);
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response;
		response = _requestService.InsertDueRequest(userViewModelList, userId);

		//response = _requestService.InsertDueRequest(userViewModelList, userId);

		//foreach (var user in userViewModelList)
		//      {
		//	
		//}
		//ResponseViewModel<RequestViewModel> response = _requestService.InsertDueRequest(Request, userId); 
		return Json(true);
	}
	//For Tool Inventory Manager
	public IActionResult ToolInventory(int UserDept)
	{

		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllToolInventoryInstrumentList(UserDept);

		return View(response.ResponseDataList);
	}
	public JsonResult SaveInventoryCalibration(List<Instrumentids> InstrumentList)
	{

		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));

		ResponseViewModel<InstrumentViewModel> response = _instrumentService.SaveInventoryCalibration(InstrumentList, userId);

		return Json(response.ResponseData);

	}
	public IActionResult ToolRoomDepartment()
	{
		ViewBag.PageTitle = "Replacement Due List Details";
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllToolRoomDepartmentwiseInstrument();

		return View(response.ResponseDataList);
	}
	public IActionResult ToolRoomInstrumentListing()
	{

		ResponseViewModel<InstrumentViewModel> response  = _instrumentService.GetAllToolRoomInstrument();
		return View(response.ResponseDataList);
		//return View();

	}
	//For Tool Inventory Manager
	public IActionResult SaveInstrumenDetails(DateTime DueDate)
	{
		return Json(true);
		//int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		//ResponseViewModel<RequestViewModel> response = _requestService.ExternalCalibrationReject(requestId, rejectReason, userId);
		//return Json(response.ResponseData);
	}

	#region Control Card
	public IActionResult ControlCard(int instrumentId)
	{
		ViewBag.InstrumentId = instrumentId;
		QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImageForInstru(instrumentId);
		ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentDetailById(instrumentId);

		return View(response.ResponseData);

	}

	public JsonResult RequestListForInstrument(int instrumentId)
	{
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetRequestListForInstrument(instrumentId);
		return Json(response.ResponseDataList);
	}
	private QRCodeFilesViewModel GetQRCodeImageForInstru(int instrumentId)
	{
	#region Control Card
	public IActionResult ControlCard(int instrumentId)
	{
		ViewBag.InstrumentId = instrumentId;
		QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImageForInstru(instrumentId);
		ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentDetailById(instrumentId);

		return View(response.ResponseData);
		
	}

	public JsonResult RequestListForInstrument(int instrumentId)
	{
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetRequestListForInstrument(instrumentId);
		return Json(response.ResponseDataList);
	}
	private QRCodeFilesViewModel GetQRCodeImageForInstru(int instrumentId)
	{
	
		QRCodeFilesViewModel qrCodeGenInputViewModel = new QRCodeFilesViewModel()
		{
			TemplateName = Constants.INSCONTROLLERNAME,
			InstrumentId = instrumentId
		};

		QRCodeFilesViewModel qrCodeGenOutputViewModel = _qrCodeGeneratorService.QRCodeGenerationForInstrument(qrCodeGenInputViewModel, instrumentId);


		if (qrCodeGenOutputViewModel == null)
		{ 
			return qrCodeGenInputViewModel;
		}

		return qrCodeGenOutputViewModel;
	}

	public JsonResult updateRequestforInstrument(List<RequestAllData> reqlist, int InstrumentId,string IssueNo)
	{

		ResponseViewModel<InstrumentViewModel> response = _instrumentService.UpdateControlCardRequestList(reqlist, InstrumentId, IssueNo);

		return Json(response.ResponseDataList);
	}

	#endregion
}