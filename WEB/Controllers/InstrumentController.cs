
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
namespace WEB.Controllers;

public class InstrumentController : BaseController
{
    private IInstrumentService _instrumentService { get; set; }
    private IRequestService _requestService{get;set;}
    public InstrumentController(IInstrumentService instrumentService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor,IRequestService requestService) : base(logger, contextAccessor)
    {
        _instrumentService = instrumentService;
        _requestService=requestService;
    }

    public IActionResult Index() 
    {
        ViewBag.PageTitle="Instrument List";
        ViewBag.ResponseCode = TempData["ResponseCode"];  
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        //int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        //int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        //ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentList(userId, userRoleId);
        //return View(response.ResponseDataList);
        return View();
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
        ViewBag.PageTitle="Instrument Create";
        ResponseViewModel<InstrumentViewModel> response = _instrumentService.CreateNewInstrument();
		//ViewBag.ObservationType = response.ResponseData.ObservationType;
		//ViewBag.ObservationTypeList = response.ResponseData.LovsList;
		return View(response.ResponseData);
    }

    public IActionResult InsertInstrument(InstrumentViewModel instrument)
    {		
		int userId=Convert.ToInt32(base.SessionGetString("LoggedId"));
        int UserDeptId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userRoleId=Convert.ToInt32(base.SessionGetString("UserRoleId"));
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
            instrument.CalibDate=DateTime.Now;
            //instrument.UserDept=Convert.ToInt32(base.SessionGetString("DepartmentId"));
            if(userRoleId != 2)
            {
                instrument.UserDept = UserDeptId;
            }
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
        ViewBag.PageTitle="Instrument Edit";
        int userRoleId=Convert.ToInt32(base.SessionGetString("UserRoleId"));
        
        ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentById(instrumentId);
        if(userRoleId==1 || userRoleId==3){
            response.ResponseData.IsDisabled="readonly";
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
        ViewBag.PageTitle="Instrument QuratineList";
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
      int userId=Convert.ToInt32(base.SessionGetString("LoggedId"));
      int userRoleId=Convert.ToInt32(base.SessionGetString("UserRoleId"));

        ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetAllInstrumentQuarantineList(userId,userRoleId);
        return View(response.ResponseDataList);
    }

    public IActionResult InstrumentQuarantine(int instrumentId, string reason)
    {
      int statusId=1;
      int userId=Convert.ToInt32(base.SessionGetString("LoggedId"));
      ResponseViewModel<InstrumentViewModel> response = _instrumentService.InstrumentQuarantine(instrumentId, reason,userId,statusId);
      return Json(response.ResponseData);
    }
    public IActionResult InstrumentRemoveQuarantine(int instrumentId)
    {
        int userRoleId=Convert.ToInt32(base.SessionGetString("UserRoleId"));
        int userId=Convert.ToInt32(base.SessionGetString("LoggedId"));
        ResponseViewModel<InstrumentViewModel> response=new ResponseViewModel<InstrumentViewModel>();
        if(userRoleId==1){
            response= _instrumentService.InstrumentRemoveQuarantine(instrumentId,3,userId);
        }else if(userRoleId==2 || userRoleId==4){
            response= _instrumentService.InstrumentRemoveQuarantine(instrumentId,2,userId);
        }
        return Json(response.ResponseData);
    }

    public ActionResult Request(int instumentId, int typeId)
    {
     int userId=Convert.ToInt32(base.SessionGetString("LoggedId"));
      ResponseViewModel<RequestViewModel>response=_requestService.InsertRequest(instumentId,userId,typeId);
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return RedirectToAction("Index","Instrument");
    }
}
