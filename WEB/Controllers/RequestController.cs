using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;

namespace WEB.Controllers;

public class RequestController : BaseController
{
     public RequestController(ILogger<BaseController>logger,IHttpContextAccessor contextAccessor):base(logger,contextAccessor)
    {
 
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Edit() 
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    } 


} 
