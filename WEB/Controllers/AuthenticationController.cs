using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;

namespace WEB.Controllers;

public class AuthenticationController : BaseController
{
    private IUserService _userService{get;set;}
    public AuthenticationController(ILogger<BaseController>logger, IUserService userService,IHttpContextAccessor contextAccessor):base(logger,contextAccessor)
    {
        _userService=userService;
    }
    public IActionResult AuthenticateUser(Guid activationKey)
    {
        ViewBag.ActivationKey=activationKey;
        return View();
    }
    public IActionResult ActivateUser(ActivationUserViewModel userActivation){
       _userService.ActivateUser(userActivation); 
     return RedirectToAction("Login","Account");
    }

}
