using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using System.DirectoryServices;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using WEB.Services;

namespace WEB.Controllers;

public class UserController : BaseController
{
    private IUserService _userService { get; set; }
    private IUnitOfWork _unitOfWork { get; set; }
    private readonly IMapper _mapper;
    public UserController(IUserService userService, IMapper mapper, IUnitOfWork unitOfWork, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        ViewBag.PageTitle = "User List";
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        ViewBag.UserDeleteResponse = TempData["UserDeleteResponseData"];
        ViewBag.UserDeleteFinalResponse = TempData["UserDeleteFinalResponseCode"];
        ViewBag.DeletingUserIdLast = TempData["DeletingUserId"];
        ViewBag.DeletingError = TempData["DeletingError"];
        ViewBag.FinalUserDeleteResponseData = TempData["FinalUserDeleteResponseData"];

        ResponseViewModel<UserViewModel> response = _userService.GetAllUserList();
        List<UserViewModel> userById = _mapper.Map<List<UserViewModel>>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2).ToList());
        response.ResponseDataList.First().Labadminuserlist = userById;
        return View(response.ResponseDataList);
    }

    public IActionResult UserEdit(int userId)
    {
        ViewBag.PageTitle = "User Edit";
        ResponseViewModel<UserViewModel> response = _userService.GetUserById(userId);
        return View("Create", response.ResponseData);
    }
    public class CurrentUserInfo
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailId { get; set; }
        public string Landline { get; set; }
        public string Mobile { get; set; }
        public int UserCount { get; set; }
    }
    public IActionResult GetUserInfoFromLDAP(string ShortId)
    {
        var instrumentsList = new List<CurrentUserInfo>();
        if (!string.IsNullOrEmpty(ShortId) && ShortId.Length > 6)
        {
            string domainName = "APAC";
            DirectorySearcher dssearch = new DirectorySearcher(domainName);

            dssearch.Filter = "(sAMAccountName=" + ShortId + ")";

            System.DirectoryServices.SearchResult sresult = dssearch.FindOne();
            if (sresult != null)
            {
                DirectoryEntry dsresult = sresult.GetDirectoryEntry();
                var currentUserInfo = new CurrentUserInfo
                {
                    UserName = dsresult.Properties["SAMAccountName"][0].ToString(),
                    FirstName = dsresult.Properties["givenName"][0].ToString(),
                    LastName = dsresult.Properties["sn"][0].ToString(),
                    EmailId = dsresult.Properties["mail"][0].ToString(),
                    Landline = dsresult.Properties["telephoneNumber"].Value == null ? "" : dsresult.Properties["telephoneNumber"].Value.ToString(),
                    Mobile = dsresult.Properties["mobile"].Value == null ? "" : dsresult.Properties["mobile"].Value.ToString()
                };
                return Json(currentUserInfo);
            }
        }
        return Json(instrumentsList);
    }
    public IActionResult GetUserInfoFromLDAPData(string ShortId, string Level)
    {
        User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == ShortId && Q.Level == Level).SingleOrDefault();
        return Json(userById);
    }
    public IActionResult Create()
    {
        ViewBag.PageTitle = "User Create";
        ResponseViewModel<UserViewModel> response = _userService.CreateNewUser();
        return View(response.ResponseData);
    }
    public IActionResult ValidateLogin(string userName, string userPassword, string ReturnUrl, string language)
    {
        ResponseViewModel<UserViewModel> response = _userService.ValidateUser(userName, userPassword);
        if (response.ResponseMessage == "Success")
        {
            if (response.ResponseData != null)
            {
                HttpContext.Session.SetString("UserRoleId", response.ResponseData.UserRoleId.ToString());
                HttpContext.Session.SetString("FirstName", response.ResponseData.FirstName.ToString());
                HttpContext.Session.SetString("LastName", response.ResponseData.LastName.ToString());
                HttpContext.Session.SetString("ShortId", response.ResponseData.ShortId.ToString());
                HttpContext.Session.SetString("LoggedId", response.ResponseData.Id.ToString());
                HttpContext.Session.SetString("DepartmentName", response.ResponseData.DepartmentName.ToString());
                HttpContext.Session.SetString("DepartmentId", response.ResponseData.DepartmentId.ToString());
				HttpContext.Session.SetString("Language", language.ToString());
			}

            if (!string.IsNullOrEmpty(ReturnUrl))
                return Redirect(ReturnUrl);

            return RedirectToAction("Index", "Home");

        }

        else
        {
            TempData["ResponseCode"] = response.ResponseCode;
            TempData["ResponseMessage"] = response.ResponseMessage;

            return RedirectToAction("Login", "Account");
        }
    }

    public IActionResult SetLanguage(string lang)
    {
        HttpContext.Session.SetString("Language", lang);
        return Json("Success");
    }


	public IActionResult PasswordUpdate(UserViewModel user)
    {
        ResponseViewModel<UserViewModel> response;

        response = _userService.PasswordUpdate(user);


        TempData["PasswordUpdate"] = response.ResponseCode;
        if (response.ResponseMessage == "Success")
        {
            return RedirectToAction("Login", "Account");
        }
        return RedirectToAction("Login", "Account");

    }
    public IActionResult InsertUser(UserViewModel user)
    {
        ResponseViewModel<UserViewModel> response;
        if (user.Id != null && user.Id > 0)
        {
            user.ModifiedBy = 1;
            user.ModifiedOn = DateTime.Now;
            response = _userService.UpdateUser(user);
        }
        else
        {
            user.ActiveStatus = true;
            user.CreatedBy = 1;
            user.ModifiedBy = 1;
            user.CreatedOn = DateTime.Now;
            user.ModifiedOn = DateTime.Now;
            user.ActivationGuid = Guid.NewGuid();
            response = _userService.InsertUser(user);
        }
        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
        if (response.ResponseMessage == "Success")
        {
            return RedirectToAction("Index", "User");
        }
        else
        {
            return View("Create", response.ResponseData);
        }
    }
    public IActionResult ProfileUpdate()
    {
        if (SessionGetString("Id") != null)
        {
            int loggedinUserId = Convert.ToInt16(SessionGetString("Id"));
            ResponseViewModel<UserViewModel> response = _userService.GetUserById(loggedinUserId);
            return View("ProfileUpdate", response.ResponseData);
        }
        else
        {
            return RedirectToAction("Login", "Account");
        }
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult PasswordReset(string UserEmail)
    {
        return RedirectToAction("Login", "Account");
    }
    public IActionResult UserDelete(int userId)
    {

        ResponseViewModel<string> response = _userService.DeleteUser(userId);

        TempData["UserDeleteResponseData"] = response.ResponseData;
        TempData["DeletingUserId"] = userId;
        return RedirectToAction("Index");
    }
    public IActionResult AssignUser(UserViewModel user)
    {
        ResponseViewModel<UserViewModel> response;

        if (user.DeletingID != user.Id)
        {
            response = _userService.AssignUser(user);

            if (response.ResponseMessage == "Success")
            {

                ResponseViewModel<string> response1 = _userService.DeleteUser(user.DeletingID);
                if (response1.ResponseMessage == "Success")
                {
                    TempData["FinalUserDeleteResponseData"] = response.ResponseMessage;
                    TempData["UserDeleteResponseCode"] = response.ResponseCode;
                    TempData["ResponseMessage"] = response.ResponseMessage;
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    TempData["FinalUserDeleteResponseData"] = response.ResponseMessage;
                    return RedirectToAction("Index", "User");
                }


            }
            else
            {
                return RedirectToAction("Index", "User");
            }
        }
        TempData["DeletingError"] = 500;
        return RedirectToAction("Index", "User");

    }
}
