using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System.Configuration;
using System.DirectoryServices;
using System.IO;
using System.Web;
namespace WEB.Controllers;

public class MasterController : BaseController
{
	private IMasterService _masterService { get; set; }
	private IMasterHistoryService _masterHistoryService { get; set; }
	private IExternalRequestService _externalRequestService { get; set; }
	public MasterController(IMasterService masterService, IMasterHistoryService masterHistoryService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IExternalRequestService externalRequestService) : base(logger, contextAccessor)
	{
		_masterService = masterService;
		_masterHistoryService = masterHistoryService;
		_externalRequestService = externalRequestService;
	}

	public IActionResult Index()
	{
		ViewBag.PageTitle = "Master List";
		ViewBag.ResponseCode = TempData["ResponseCode"];
		ViewBag.ResponseMessage = TempData["ResponseMessage"];
		string SessionLang = base.SessionGetString("Language");
		ResponseViewModel<MasterViewModel> response = _masterService.GetAllMasterList(SessionLang);
		return View(response.ResponseDataList);
	}

	public IActionResult Edit()
	{

		return View();
	}

	public IActionResult Create()
	{
		ViewBag.PageTitle = "Master Create";
		ResponseViewModel<MasterViewModel> response = _masterService.CreateNewMaster();
		return View(response.ResponseData);

	}
	public IActionResult MasterInsert(MasterViewModel master)
	{
		//return Json(true);
		string SessionLang = base.SessionGetString("Language");
		//return Json(true);
		ResponseViewModel<MasterViewModel> response;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		if (master.Id != null && master.Id > 0)
		{
			master.ModifiedOn = DateTime.Now;
			master.ModifiedBy = userId;
			response = _masterService.UpdateMaster(master);
		}
		else
		{

			master.CreatedOn = DateTime.Now;
			master.ModifiedOn = DateTime.Now;
			master.CreatedBy = userId;
			master.ModifiedBy = userId;
			master.IsActive = true;
			response = _masterService.InsertMaster(master);
		}
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		if (response.ResponseMessage == "Success")
		{
			return RedirectToAction("Index", "Master");
		}
		else
		{
			return View("Create", master);
		}
	}

	public ActionResult MasterEdit(int masterId)
	{
		ViewBag.PageTitle = "Master Edit";
		
		ResponseViewModel<MasterViewModel> response = _masterService.GetMasterById(masterId);
		ViewBag.CalibFreqMaster = response.ResponseData.CalibFreqId;
		return View("Create", response.ResponseData);
	}
	public ActionResult MasterDelete(int masterId)
	{
		ResponseViewModel<MasterViewModel> response = _masterService.DeleteMaster(masterId);
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		return RedirectToAction("Index", "Master");
	}

	public ActionResult ExternalRequest(int masterId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<ExternalRequestViewModel> response = _externalRequestService.InsertExternalRequest(masterId, userId);
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		return RedirectToAction("Index", "Master");
	}

	public IActionResult GetQuarantineList()
	{
		ViewBag.PageTitle = "QuarantineList";
		ViewBag.ResponseCode = TempData["ResponseCode"];
		ViewBag.ResponseMessage = TempData["ResponseMessage"];
		ResponseViewModel<MasterViewModel> response = _masterService.GetAllMasterQuarantineList();
		return View(response.ResponseDataList);
	}
	public IActionResult MasterQuarantine(int masterId, string reason)
	{
		int statusId = 1;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<MasterViewModel> response = _masterService.MasterQuarantine(masterId, reason, statusId, userId);
		return Json(response.ResponseData);
	}

	public IActionResult MasterRemoveQuarantine(int masterId)
	{
		ResponseViewModel<MasterViewModel> response = _masterService.MasterRemoveQuarantine(masterId);
		return Json(response.ResponseData);
	}
	public IActionResult MasterHistory(int MasterId)
	{
		int statusId = 1;
		ViewBag.PageTitle = "Master History";
		ViewBag.MasterId = MasterId;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<MasterHistoryViewModel> response = _masterHistoryService.GetMasterHistoryListbyId(MasterId);
		return View(response.ResponseDataList);
	}

	public IActionResult MasterHistoryCreate(int MasterId)
	{
		ViewBag.PageTitle = "Master History Create";
		string firstName = base.SessionGetString("FirstName");
		string lastName = base.SessionGetString("LastName");
		ResponseViewModel<MasterHistoryViewModel> response = new ResponseViewModel<MasterHistoryViewModel>();
		if (response.ResponseData == null)
		{
			response.ResponseData = new MasterHistoryViewModel()
			{
				Id = 0,
				CreatedBy = string.Concat(firstName.Trim(), " ", lastName.Trim()),
				Date = DateTime.Now,
				MasterId = MasterId
			};
		}
		return View(response.ResponseData);
	}
	public IActionResult MasterHistoryInsert(MasterHistoryViewModel master)
	{
		ResponseViewModel<MasterHistoryViewModel> response;
		string firstName = base.SessionGetString("FirstName");
		string lastName = base.SessionGetString("LastName");
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		master.CreatedOn = DateTime.Now;
		master.CreatedBy = firstName + " " + lastName;
		master.StatusId = 1;
		master.Status = "Closed";
		response = _masterHistoryService.InsertMaster(master);
		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		if (response.ResponseMessage == "Success")
		{
			return RedirectToAction("MasterHistory", "Master", new { MasterId = master.MasterId });
		}
		else
		{
			return View("Create", master);
		}
	}


}
