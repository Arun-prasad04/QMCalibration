using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;

namespace WEB.Controllers;

public class BaseController : Controller
{
    private readonly ILogger<BaseController> _logger;
    private IHttpContextAccessor _contextAccessor { get; set; }
    public BaseController(ILogger<BaseController> logger,IHttpContextAccessor contextAccessor)
    {
        _logger=logger;
        _contextAccessor=contextAccessor;
        
    }
    public string SessionGetString(string key)
        {
            return _contextAccessor.HttpContext.Session.GetString(key);
        }

        public void SessionSetString(string key, string value)
        {
            _contextAccessor.HttpContext.Session.SetString(key, value);
        }

        public void SessionRemoveString(string key)
        {
            _contextAccessor.HttpContext.Session.Remove(key);
        }
}