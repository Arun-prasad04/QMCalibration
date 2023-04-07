using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Service;
using WEB.Services.Interface;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System;
using System.Collections.Generic;

namespace WEB.Controllers;

public class AccountController : BaseController
{
    private IUserService _userService { get; set; }
    private IMasterService _masterService { get; set; }
    public AccountController(IMasterService masterService, IUserService userService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
    {
        _userService = userService;
        _masterService = masterService;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult UpdatePassword()
    {
        return View();
    }
    public IActionResult PasswordUpdate()
    {
        return View();
    }



    public IActionResult ForgetPassword(string Email)
    {
        ResponseViewModel<UserViewModel> response;
        ResponseViewModel<string> UserResponse = _userService.CheckEmailAddress(Email);
        if (UserResponse.ResponseData == "True")
        {
            if (Email != "" || Email != null)
            {

                response = _userService.ForgotUserPassword(Email);
                TempData["ResponseCode"] = response.ResponseData;
                TempData["ResponseMessage"] = response.ResponseMessage;
                ViewBag.ResponseCode = response.ResponseCode;
                TempData["check"] = response.ResponseCode;


                return RedirectToAction("Login", "Account");

            }
        }
        TempData["EmailValididty"] = 500;

        return RedirectToAction("Login", "Account");


    }
    public IActionResult Login()

    {
        string returnUrl = "";
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.PasswordUpdateResponseCode = TempData["PasswordUpdate"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        ViewBag.ForgerPasswordResponse = TempData["check"];
        ViewBag.IncorrectEmailAddress = TempData["EmailValididty"];

        LoginViewModel loginViewModel = new LoginViewModel()
        {
            ReturnUrl = returnUrl
        };

        return View(loginViewModel);
    }
}
