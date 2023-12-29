using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System;
using System.Collections.Generic;
using WEB.Services;
using Org.BouncyCastle.Bcpg.Sig;

namespace WEB.Controllers;

public class DepartmentController : BaseController
{
    private IDepartmentService _departmentService{get;set;}
    public DepartmentController(IDepartmentService departmentService,ILogger<BaseController>logger,IHttpContextAccessor contextAccessor):base(logger,contextAccessor)
    {
        _departmentService=departmentService;
    }
    public IActionResult Index()
    {
		ViewBag.Shared = "Department";
		ViewBag.PageTitle="Department List";
        ViewBag.ResponseCode=TempData["ResponseCode"];
        ViewBag.ResponseMessage=TempData["ResponseMessage"];
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		ResponseViewModel<DepartmentViewModel>response=_departmentService.GetAllDepartmentList();
        return View(response.ResponseDataList);
    }
    public IActionResult Create()
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ViewBag.PageTitle="Department Create";
        ResponseViewModel<DepartmentViewModel> response = _departmentService.CreateNewDepartment();
		ViewBag.Location = response.ResponseData.locationList;
		return View(response.ResponseData);

		//return View(new DepartmentViewModel());  
	} 
    public IActionResult InsertDepartment(DepartmentViewModel department)
    {
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		ResponseViewModel<DepartmentViewModel>response;
        if(department.Id != null && department.Id>0){ 
            department.ModifiedBy= userId;
            department.ModifiedOn=DateTime.Now;
            response=_departmentService.UpdateDepartment(department); 
        }else{
            department.ActiveStatus=true; 
            department.CreatedBy= userId;
            department.ModifiedBy= userId;
            department.CreatedOn=DateTime.Now;
            department.ModifiedOn=DateTime.Now;
            response=_departmentService.InsertDepartment(department);
        }
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
       if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }
    }
    public ActionResult DepartmentEdit(int departmentId)
    {
        ViewBag.PageTitle="Department Edit";

      ResponseViewModel<DepartmentViewModel>response= _departmentService.GetDepartmentById(departmentId);
        ViewBag.Location = response.ResponseData.locationList;
		return View("Create",response.ResponseData);

    }
    public ActionResult DepartmentDelete(int departmentId)
    {
        ResponseViewModel<DepartmentViewModel>response= _departmentService.DeleteDepartment(departmentId);
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return RedirectToAction("Index","Department");
    }
}
