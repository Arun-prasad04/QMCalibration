using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System;
using System.Collections.Generic;

namespace WEB.Controllers;

public class ContactController : BaseController
{
    private IContactService _contactService{get;set;}
    public ContactController(IContactService contactService,ILogger<BaseController>logger,IHttpContextAccessor contextAccessor):base(logger,contextAccessor)
    {
      _contactService=contactService;
    }

        public IActionResult Index()
    {
        ViewBag.PageTitle="Contact List";
        ViewBag.ResponseCode=TempData["ResponseCode"];
        ViewBag.ResponseMessage=TempData["ResponseMessage"];
        ResponseViewModel<ContactViewModel> response=_contactService.GetAllContactList();
        return View(response.ResponseDataList);
    }
    public IActionResult Create()
    {
        ViewBag.PageTitle="Contact Create";
        ResponseViewModel<ContactViewModel> response=_contactService.CreateNewContact();
        return View(response.ResponseData);
    }
    public IActionResult InsertContact(ContactViewModel contact)
    {
        ResponseViewModel<ContactViewModel> response;
        if(contact.Id !=null && contact.Id>0){
            contact.ModifiedBy=1;
            contact.ModifiedOn=DateTime.Now;
             response=_contactService.UpdateContact(contact);
        }else{
            contact.CreatedBy=1;
            contact.ModifiedBy=1;
            contact.CreatedOn=DateTime.Now; 
            contact.ModifiedOn=DateTime.Now;  
              response=_contactService.InsertContact(contact); 
        }
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
       if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Contact");
       }else{ 
           return View("Create",contact); 
       }
    }
    public ActionResult ContactEdit(int contactId){
        ViewBag.PageTitle="Contact Edit";
       ResponseViewModel<ContactViewModel> response= _contactService.GetContactById(contactId);
      return View("Create",response.ResponseData);
    }
        public ActionResult ContactDelete(int contactId){
        ResponseViewModel<ContactViewModel> response= _contactService.DeleteContact(contactId);
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return RedirectToAction("Index","Contact");
    }

}
