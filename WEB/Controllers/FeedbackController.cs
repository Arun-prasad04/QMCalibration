using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System.Text;

namespace WEB.Controllers;

public class FeedbackController : BaseController
{
    private readonly ILogger<BaseController> _logger;
    private IFeedbackDataService _feedbackDataService { get; set; }
    private IFeedbackInviteService _feedbackInviteService { get; set; }
    private IUserService _userService { get; set; }

    public FeedbackController(IFeedbackDataService feedbackDataService, IFeedbackInviteService feedbackInviteService, IUserService userService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
    {
        _feedbackDataService = feedbackDataService;
        _feedbackInviteService = feedbackInviteService;
        _userService = userService;
        _logger = logger;
    }

    public ActionResult Index(int FeedbackInviteId)
    {
        ViewBag.PageTitle = "FeedBack Form";

        //Get Session data
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string designation = Constants.DEFAULT_DESIGNATION;

        if (FeedbackInviteId > 0)
        {
            ResponseViewModel<FeedbackViewModel> feedbackExistingData = _feedbackDataService.GetFeedbackData(FeedbackInviteId);

            TempData["ResponseCode"] = feedbackExistingData.ResponseCode;
            TempData["ResponseMessage"] = feedbackExistingData.ResponseMessage;

            if (feedbackExistingData.ResponseData == null)
            {
                feedbackExistingData.ResponseData = new FeedbackViewModel()
                {
                    Id = 0,
                    LoginUserRoleId = userRoleId,
                    CustomerName = string.Concat(firstName.Trim(), " ", lastName.Trim()),
                    DepartmentName = departmentName,
                    DepartmentId = departmentId,
                    UserSubmitedOn = DateTime.Now.Date,
                    ReviewedOn = DateTime.Now.Date,
                    Designation = designation,
                    //For default document status 0
                    FeedbackStatus = 0
                };
                ViewBag.ResponseCode = TempData["ResponseCode"];
                ViewBag.ResponseMessage = TempData["ResponseMessage"];
                feedbackExistingData.ResponseData.FormatNo = Constants.FORMATNUMER;
                feedbackExistingData.ResponseData.RevNo = Constants.REVISION_AND_DATE;
                return View(feedbackExistingData.ResponseData);
            }

            //To Assign Feedback format number , revision date and Login user Role Id
            feedbackExistingData.ResponseData.FormatNo = Constants.FORMATNUMER;
            feedbackExistingData.ResponseData.RevNo = Constants.REVISION_AND_DATE;
            feedbackExistingData.ResponseData.LoginUserRoleId = userRoleId;
            feedbackExistingData.ResponseData.Designation = designation;
            return View(feedbackExistingData.ResponseData);
        }

        //Get Feedback invite data
        ResponseViewModel<FeedbackInviteViewModel> feedbackInviteResponse = _feedbackInviteService.GetFeedbackInviteUserData(userId);

        if (feedbackInviteResponse.ResponseData != null)
        {
            //Get Feedback data baed on the Feedback invites...
            FeedbackInviteId = feedbackInviteResponse.ResponseData.Id;
            ResponseViewModel<FeedbackViewModel> feedbackNewFormData = _feedbackDataService.GetFeedbackData(FeedbackInviteId);

            if (feedbackNewFormData.ResponseData == null)
            {
                feedbackNewFormData.ResponseData = new FeedbackViewModel()
                {
                    Id = 0,
                    LoginUserRoleId = userRoleId,
                    CustomerName = string.Concat(firstName.Trim(), " ", lastName.Trim()),
                    DepartmentName = departmentName,
                    DepartmentId = departmentId,
                    FeedbackInviteId = feedbackInviteResponse.ResponseData.Id,
                    UserSubmitedOn = DateTime.Now.Date,
                    ReviewedByName = feedbackInviteResponse.ResponseData.InvitedByName,
                    ReviewedOn = DateTime.Now.Date,
                    Designation = "Product Executive",
                    LabName=feedbackInviteResponse.ResponseData.LabName,
                    //For default document status 0
                    FeedbackStatus = 0
                };
            }

            TempData["ResponseCode"] = feedbackNewFormData.ResponseCode;
            TempData["ResponseMessage"] = feedbackNewFormData.ResponseMessage;

            //To Assign Feedback format number , revision date and Login user Role Id
            feedbackNewFormData.ResponseData.FormatNo = Constants.FORMATNUMER;
            feedbackNewFormData.ResponseData.RevNo = Constants.REVISION_AND_DATE;
            feedbackNewFormData.ResponseData.LoginUserRoleId = userRoleId;
            return View(feedbackNewFormData.ResponseData);
        }
        else
        {
            //If Login user does not received Feedback form details, its redirect to No Access...
            TempData["ResponseCode"] = feedbackInviteResponse.ResponseCode;
            TempData["ResponseMessage"] = feedbackInviteResponse.ResponseMessage;
            return RedirectToAction("NoAccess", "Feedback");
        }
    }
    public IActionResult FeedbackInvite()
    {
        //get Feedback invite details
        ResponseViewModel<UserViewModel> response = _userService.GetAllUserList();
        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;

        return View(response.ResponseDataList);
    }
     public IActionResult InsertFeedback(FeedbackViewModel feedbackViewModel)
    {
        ResponseViewModel<FeedbackViewModel> response;
        if (feedbackViewModel.Id > 0)
        {

            int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
            feedbackViewModel.ReviewedBy = userId;
            feedbackViewModel.ReviewedOn = DateTime.Now.Date;
            feedbackViewModel.FeedbackStatus = (int)FeedbackDocumentStatus.Reviewed;
            response = _feedbackDataService.UpdateFeedbackData(feedbackViewModel);
        }
        else
        {
            feedbackViewModel.UserSubmitedOn = DateTime.Now.Date;
            feedbackViewModel.FeedbackStatus = (int)FeedbackDocumentStatus.Submitted;
            response = _feedbackDataService.InsertFeedbackData(feedbackViewModel);
        }

        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;

        return RedirectToAction("Index", "Feedback", new { feedbackInviteId = feedbackViewModel.FeedbackInviteId });
    }

    public IActionResult SendInvite(List<UserView> userViewModelList)
    {
        ResponseViewModel<FeedbackInviteViewModel> response;

        List<FeedbackInviteViewModel> feedbackInviteViewModelList = new List<FeedbackInviteViewModel>();

        foreach (var user in userViewModelList)
        {
            int userId = 0;
            var result = Int32.TryParse(user.id, out userId);
            if (result)
            {
                FeedbackInviteViewModel feedbackInviteViewModel = new FeedbackInviteViewModel()
                {
                    UserId = userId,
                    InvitedOn = DateTime.Now,
                    InvitedBy = Convert.ToInt32(base.SessionGetString("LoggedId")),
                    Email = user.email,
                    LabName=user.LabName,
                    FirstName=user.firstName,
                    LastName=user.lastName
                };

                feedbackInviteViewModelList.Add(feedbackInviteViewModel);
            }
        }

        response = _feedbackInviteService.InsertFeedbackInvite(feedbackInviteViewModelList);

        return Json(response.ResponseMessage);

    }
    public IActionResult SubmittedList()
    {
        int submittedStstus = (int)FeedbackDocumentStatus.Submitted;
        ResponseViewModel<FeedbackInviteViewModel> response = _feedbackInviteService.GetFeedbackInviteList(submittedStstus);
        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;

        if (response.ResponseDataList.Any())
        {
            return View(response.ResponseDataList);
        }
        else
        {
            List<FeedbackInviteViewModel> feedbackInviteViewModelList = new List<FeedbackInviteViewModel>();
            return View(feedbackInviteViewModelList);
        }

    }
    public IActionResult ReviewedList()
    {
        int reviewdStstus = (int)FeedbackDocumentStatus.Reviewed;
        ResponseViewModel<FeedbackInviteViewModel> response = _feedbackInviteService.GetFeedbackInviteList(reviewdStstus);
        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;

        if (response.ResponseDataList.Any())
        {
            return View(response.ResponseDataList);
        }
        else
        {
            List<FeedbackInviteViewModel> feedbackInviteViewModelList = new List<FeedbackInviteViewModel>();
            return View(feedbackInviteViewModelList);
        }
    }

    public IActionResult NoAccess()
    {
        return View();
    }
}