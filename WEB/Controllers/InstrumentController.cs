
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Net.NetworkInformation;
using CMT.DAL;
using CMT.DATAMODELS;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using NPOI.SS.Formula.Functions;
using WEB.Models;
using WEB.Services;
using WEB.Services.Interface;
using Microsoft.AspNetCore.Http;
using static iTextSharp.text.pdf.PdfDocument;

namespace WEB.Controllers;

public class InstrumentController : BaseController
{
	private IInstrumentService _instrumentService { get; set; }
	private IRequestService _requestService { get; set; }
	private IUnitOfWork _unitOfWork { get; set; }
    private IHttpContextAccessor _contextAccessor { get; set; }
    private IQRCodeGeneratorService _qrCodeGeneratorService { get; set; }
	private IConfiguration _configuration;
	public InstrumentController(IInstrumentService instrumentService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IRequestService requestService, IUnitOfWork unitOfWork, IQRCodeGeneratorService qrCodeGeneratorService, IConfiguration configuration) : base(logger, contextAccessor)
	{
		_instrumentService = instrumentService;
		_requestService = requestService;
		_unitOfWork = unitOfWork;
		_qrCodeGeneratorService = qrCodeGeneratorService;
		_configuration = configuration;
	}

	public IActionResult Index()
	{
		
		ViewBag.ResponseCode = TempData["ResponseCode"];
		ViewBag.ResponseMessage = TempData["ResponseMessage"];
		//int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		//int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		//ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList(userId, userRoleId);
		//return View(response.ResponseDataList);
		return View();

	}

	public JsonResult GetAllInstrumentList1()
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList1(userId, userRoleId);
        return Json(new { data = response.ResponseDataList });
        //return Json(response.ResponseDataList);
    }
	public JsonResult GetAllInstrumentList(DataTableParameters InsDparam, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate,string chkDue)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		var TotalCount = 0;
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList(userId, userRoleId, InsDparam.iDisplayStart, InsDparam.iDisplayLength, InsDparam.sSearch, sscode, instrumentname, labid, typeOfEquipment, serialno, range, department, calibrationdate, duedate,chkDue);

		if (response.ResponseDataList.Count > 0)
		{
			TotalCount = (Int32)response.ResponseDataList[0].TotalCount;
		}
		//      }
		int pageNo = 1;
		if (InsDparam.iDisplayStart >= InsDparam.iDisplayLength)
		{

			pageNo = (InsDparam.iDisplayStart / InsDparam.iDisplayLength) + 1;
		}

		return Json(new
		{
			InsDparam.sEcho,
			iTotalRecords = response.ResponseDataList.Count,
			iTotalDisplayRecords = TotalCount,
			aaData = response.ResponseDataList
		});
		//return Json(new { data = response.ResponseDataList });
		//return Json(response.ResponseDataList);
	}
	public JsonResult GetAllInstrumentList1(DataTableParameters InsDparam,string sscode, string instrumentname, string labid, string typeOfEquipment,string serialno,string range,string department,string calibrationdate,string duedate)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        var TotalCount = 0;
        ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList(userId, userRoleId, InsDparam.iDisplayStart, InsDparam.iDisplayLength,InsDparam.sSearch,  sscode,  instrumentname,  labid,  typeOfEquipment,  serialno,  range,  department,  calibrationdate, duedate,"");

        if (response.ResponseDataList.Count > 0)
        {
            TotalCount = (Int32)response.ResponseDataList[0].TotalCount;
        }
        //      }
        int pageNo = 1;
        if (InsDparam.iDisplayStart >= InsDparam.iDisplayLength)
        {

            pageNo = (InsDparam.iDisplayStart / InsDparam.iDisplayLength) + 1;
        }

        return Json(new
        {
            InsDparam.sEcho,
            iTotalRecords = response.ResponseDataList.Count,
            iTotalDisplayRecords = TotalCount,
            aaData = response.ResponseDataList
        });
        //return Json(new { data = response.ResponseDataList });
        //return Json(response.ResponseDataList);
    }
    public IActionResult Create()
	{
        
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.CreateNewInstrument(userId, userRoleId);
		
		//ViewBag.UserBaseDepartment = response.ResponseData.Departments;
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
			
			response = _instrumentService.InsertInstrument(instrument);
         
        }
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		          
   //         if (response.ResponseMessage == "Success")
			//{
            return RedirectToAction("Index", "Instrument");
			//}
			//else
			//{
			//return Json(response.ResponseMessage);
			//}
       	}

	public ActionResult InstrumentEdit(int instrumentId)
	{
		ViewBag.PageTitle = "Instrument Edit";
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentsForId(instrumentId);
		//ViewBag.ObservationType = response.ResponseData.ObservationType;
		ViewBag.UserDept = response.ResponseData.UserDept;
		ViewBag.CalibFreq = response.ResponseData.CalibFreq;
		
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

	public JsonResult PopUpInstrumentList(string InstrumentName, int InstrumentId,int SubsectionCode)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.PopUpList(InstrumentName, InstrumentId, SubsectionCode);

		return Json(response.ResponseDataList);
	}


	public ActionResult RegularRecaliRequest(List<RequestAllView> userViewModelList)
	{
		//return Json(true);
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<RequestViewModel> response;
		response = _requestService.InsertDueRequest(userViewModelList, userId);

		return Json(true);
	}


	//For Tool Inventory Manager
	public IActionResult ToolInventory(int UserDept, int DueMonth)
	{
		ViewBag.DueMonth = DueMonth;
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllToolInventoryInstrumentList(UserDept, DueMonth);

		return View(response.ResponseDataList);
	}
	public JsonResult SaveInventoryCalibration(List<Instrumentids> InstrumentList)
	{

		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));

		ResponseViewModel<InstrumentViewModel> response = _instrumentService.SaveInventoryCalibration(InstrumentList, userId);

		return Json(response.ResponseData);

	}
	public IActionResult ToolRoomDepartment(int DueMonth)
	{
		//ViewBag.PageTitle = "Replacement Due List Details";
		ViewBag.DueMonth = DueMonth;
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllToolRoomDepartmentwiseInstrument(DueMonth);

		return View(response.ResponseDataList);
	}
	public IActionResult ToolRoomInstrumentListing()//
	{

		ResponseViewModel<InstrumentViewModel> response  = _instrumentService.GetAllToolRoomInstrument();
		//return View(response.ResponseDataList);
		return View();

	}


	public JsonResult ToolRoomDepartmentList(DataTableParameters dparam, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate)
	{
		var TotalCount = 0;
		string Reqtype = string.Empty;

		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<InstrumentViewModel> response;
		List<InstrumentViewModel> ins = new List<InstrumentViewModel>();
		response = _instrumentService.ToolRoomDepartmentList(userRoleId, userId, dparam.iDisplayStart, dparam.iDisplayLength, dparam.sSearch,  sscode,  instrumentname,  labid,  typeOfEquipment,  serialno,  range,  department,  calibrationdate,  duedate);

		if (response.ResponseDataList.Count > 0)
		{
			TotalCount = (Int32)response.ResponseDataList[0].TotalCount;
			
		}
		//      }
		int pageNo = 1;
		if (dparam.iDisplayStart >= dparam.iDisplayLength)
		{

			pageNo = (dparam.iDisplayStart / dparam.iDisplayLength) + 1;
		}

		return Json(new
		{
			dparam.sEcho,
			iTotalRecords = response.ResponseDataList.Count,
			iTotalDisplayRecords = TotalCount,
			aaData = response.ResponseDataList
		});

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
        ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentDetailById(instrumentId);
        ViewBag.InstrumentId = instrumentId;
		QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImageForInstru(instrumentId,response.ResponseData.IdNo);
		ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        
        ViewBag.IdNo = qrCodeFilesViewModel.InstrumentIdNo;

        //ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentDetailById(instrumentId);

        return View(response.ResponseData);
		
	}

	public JsonResult RequestListForInstrument(int instrumentId)
	{
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetRequestListForInstrument(instrumentId);
		return Json(response.ResponseDataList);
	}
	private QRCodeFilesViewModel GetQRCodeImageForInstru(int instrumentId,string IdNo)
	{
	
		QRCodeFilesViewModel qrCodeGenInputViewModel = new QRCodeFilesViewModel()
		{
			TemplateName = Constants.INSCONTROLLERNAME,
			InstrumentId = instrumentId,
             InstrumentIdNo  = IdNo
        };

		QRCodeFilesViewModel qrCodeGenOutputViewModel = _qrCodeGeneratorService.QRCodeGenerationForInstrument(qrCodeGenInputViewModel, instrumentId, IdNo);


		if (qrCodeGenOutputViewModel == null)
		{ 
			return qrCodeGenInputViewModel;
		}

		return qrCodeGenOutputViewModel;
	}

	public JsonResult updateRequestforInstrument(List<RequestAllData> reqlist, int InstrumentId,string IssueNo)
	{
		//return Json(true);
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.UpdateControlCardRequestList(reqlist, InstrumentId, IssueNo);

		return Json(response.ResponseDataList);
	}

    //public IActionResult ControlCardQRCodePrint(string instrumentId,string Idno)
    public IActionResult ControlCardQRCodePrint(string instrumentId,string IdNo)
    {
        //QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImageForInstru(int.Parse(instrumentId), Idno);
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImageForInstru(int.Parse(instrumentId), IdNo);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);

        return View();
    }

    #endregion
    public JsonResult InActiveQuarantineInstrument(int instrumentId)
    {
        ResponseViewModel<InstrumentViewModel> response = _instrumentService.InActiveQuarantineInstrument(instrumentId);
        
        return Json(response.ResponseDataList);
	}
	
	public JsonResult GetToolRoomSubSectionList()
	{
		List<ToolRoomMasterViewModel> DepartmentList;
		CMTDL _cmtdl = new CMTDL(_configuration);
		//ResponseViewModel<ToolRoomMasterViewModel> toolroom = _cmtdl.GetToolRoomsSubSectionMasterList();
		DepartmentList = _cmtdl.GetToolRoomsSubSectionMasterList(); //toolroom.ResponseDataList.ToList();
		var DepartTranslaterMaster = new List<ToolRoomDepartmentlist>();
		foreach (var item in DepartmentList)
		{
			DepartTranslaterMaster.Add(new ToolRoomDepartmentlist
			{
				id = item.Id,
				Name = item.Name,
				NameJp = item.NameJP,
				SubSectionCode = item.SubSectionCode,
				DepartmentId =item.DepartmentId

			});
		}
		return Json(DepartTranslaterMaster);

	}
	public class ToolRoomDepartmentlist
	{
		public int id { get; set; }
		public string? Name { get; set; }
		public string? NameJp { get; set; }
		public string? SubSectionCode { get; set; }
		public int? DepartmentId { get; set; }


	}
    public async Task<IActionResult> MailInsertDueRequest(List<RequestAllView> userViewModelList)
    {
        //return Json(true);
        //try
        //{ 
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        await _requestService.MailInsertDueRequest(userViewModelList, userId);
        //}
        //catch (Exception ex)
        //      {
        //	return Json(true);
        //}
        //return true;
        return Json(true);
    }

    public async Task<IActionResult> MailInsertInstrument(InstrumentViewModel instrument,string ReqestNo)
    {
    
    //instrument: InstrumentName, ReqestNo: ReqestNo, CreatedOn: CreatedOn, IdNo: IdNo
    var result = await _instrumentService.MailInsertInstrument();
	
    return Json(result);
    }

    public async Task<IActionResult> EmailForTracker()
    {
      
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        await _requestService.EmailForTracker();
       
       return Json("");
    }

	public JsonResult removeRequestRowFromControlCard(int RequestId)
	{
		ResponseViewModel<InstrumentViewModel> response = _instrumentService.removeRequestRowFromControlCard(RequestId);

		return Json(response.ResponseMessage);
	}

}
