using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System;
using System.Collections.Generic;
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
        ViewBag.PageTitle="Department List";
        ViewBag.ResponseCode=TempData["ResponseCode"];
        ViewBag.ResponseMessage=TempData["ResponseMessage"];
        ResponseViewModel<DepartmentViewModel>response=_departmentService.GetAllDepartmentList();
        return View(response.ResponseDataList);
    }
    public IActionResult Create()
    {
        ViewBag.PageTitle="Department Create";
        return View(new DepartmentViewModel());  
    } 
    public IActionResult InsertDepartment(DepartmentViewModel department)
    {
         ResponseViewModel<DepartmentViewModel>response;
        if(department.Id !=null && department.Id>0){ 
            department.ModifiedBy=1;
            department.ModifiedOn=DateTime.Now;
            response=_departmentService.UpdateDepartment(department); 
        }else{
            department.ActiveStatus=true; 
            department.CreatedBy=1;
            department.ModifiedBy=1;
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
