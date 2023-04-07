using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System.Text;

namespace WEB.Controllers;

public class QCIntermediateTemplateController : BaseController
{
    private readonly ILogger<BaseController> _logger;
    private IQCIntermediateTemplateService _qcintermediateTemplateService { get; set; }
    private IInstrumentService _instrumentService { get; set; }
    private IMasterService _masterService { get; set; }
    private IUserService _userService { get; set; }

    public QCIntermediateTemplateController(IQCIntermediateTemplateService qcintermediateTemplateService,
                                            IMasterService masterService, IInstrumentService instrumentService,
                                            IUserService userService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor)
                                            : base(logger, contextAccessor)
    {
        _qcintermediateTemplateService = qcintermediateTemplateService;
        _logger = logger;
        _instrumentService = instrumentService;
        _masterService = masterService;
        _userService = userService;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult QCIntermediateTemplate(int IntermediateId)
    {
        ViewBag.PageTitle = "Intermediate Template";

        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string userName = string.Concat(firstName.Trim(), " ", lastName.Trim());

        if (IntermediateId > 0)
        {

            ResponseViewModel<QCIntermediateTemplateViewModel> response = _qcintermediateTemplateService.GetIntermediateById(IntermediateId);

            if (response.ResponseData.DocumentStatus == 1)
            {
                response.ResponseData.TMSignDate = DateTime.Now;
                response.ResponseData.TechnicalManagerName = userName;
                response.ResponseData.TechnicalManager = userId;

            }
            
            response.ResponseData.RevisionNoAndDate = string.Concat(response.ResponseData.RevisionNo,
                                                        "/", (response.ResponseData.RevisionDate.ToString("dd.MM.yyyy")));

            return View("QCIntermediateTemplate", response.ResponseData);
        }

        QCIntermediateTemplateResultViewModel QCIntermediateResultViewModel = new QCIntermediateTemplateResultViewModel()
        {
            CalibrationResult = 0,
            CurrentInternalCheck= 0,
            AcceptanceCriteria= string.Empty,
            DifferenceResult = 0,
            Decision = string.Empty
        };
        List<QCIntermediateTemplateResultViewModel> qcIntermediateResultViewModelList = new List<QCIntermediateTemplateResultViewModel>();
        qcIntermediateResultViewModelList.Add(QCIntermediateResultViewModel);


        QCIntermediateTemplateViewModel qCIntermediateTemplateViewModel = new QCIntermediateTemplateViewModel()
        {
            Id = 0,
            DocumentStatus = 0,
            FormatNo = Constants.INTERMEDIATE_FORMATNUMER,
            RevisionNo = Constants.INTERMEDIATE_REVISIONNO,
            RevisionDate = Constants.INTERMEDIATE_REVISIONDATE,
            RevisionNoAndDate = Constants.INTERMEDIATE_REVISIONNO_DATE,
            SignDate = DateTime.Now,
            StudyPerformedBy = userId,
            StudyPerformedByName = userName,
            QCIntermediateResultViewModelList = qcIntermediateResultViewModelList
        };
        return View(qCIntermediateTemplateViewModel);
    }

    public IActionResult IntermediateGridPage()
    {
        ViewBag.PageTitle = "Intermedite Data List";
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        ResponseViewModel<QCIntermediateTemplateViewModel> response = _qcintermediateTemplateService.GetAllIntermediteList();
        if (response.ResponseDataList == null)
        {
            QCIntermediateTemplateViewModel emptyIntermediateTemplate = new QCIntermediateTemplateViewModel();
            response.ResponseDataList = new List<QCIntermediateTemplateViewModel>();
            response.ResponseDataList.Add(emptyIntermediateTemplate);
        }
        else
        {
            response.ResponseDataList.Where(x => x.DocumentStatus == (int)(DocumentStatus.Submitted)).ToList().ForEach(i => i.Status = "Submited");
            response.ResponseDataList.Where(x => x.DocumentStatus == (int)(DocumentStatus.Approved)).ToList().ForEach(i => i.Status = "Approved");
            response.ResponseDataList.Where(x => x.DocumentStatus == (int)(DocumentStatus.Rejected)).ToList().ForEach(i => i.Status = "Rejected");
        }

        return View(response.ResponseDataList);
    }

    public IActionResult GetMasterListByName(string equipmentName)
    {
        var masterList = new List<MasterListViewModel>();
        ResponseViewModel<MasterViewModel> dbMasterListByName = _masterService.GetEquipmentListByName(equipmentName);

        if (!(dbMasterListByName.ResponseDataList.Any()))
            return Json(masterList);

        foreach (var master in dbMasterListByName.ResponseDataList)
        {
            var masterInfo = new MasterListViewModel()
            {
                MasterId = master.Id,
                EquipmentName = master.EquipName,
                CalibDate = master.CalibDate,
                Range = master.Range
            };

            masterList.Add(masterInfo);
        }
        return Json(masterList);
    }

    public IActionResult GetMasterListByLabId(string labId)
    {
        var masterList = new List<MasterListViewModel>();
        ResponseViewModel<MasterViewModel> dbMasterListByName = _masterService.GetEquipmentListByLabId(labId);

        if (!(dbMasterListByName.ResponseDataList.Any()))
            return Json(masterList);

        foreach (var master in dbMasterListByName.ResponseDataList)
        {
            var masterInfo = new MasterListViewModel()
            {
                MasterId = master.Id,
                LabId = master.LabId,
                EquipmentName = master.EquipName,
                CalibDate = master.CalibDate,
                Range = master.Range
            };

            masterList.Add(masterInfo);
        }
        return Json(masterList);
    }

    [HttpPost]
    public IActionResult SaveData(QCIntermediateTemplateViewModel qcIntermediateTemplateViewModel)
    {
        ResponseViewModel<QCIntermediateTemplateViewModel> response;
        if (qcIntermediateTemplateViewModel.Id > 0)
        {
            response = _qcintermediateTemplateService.UpdateData(qcIntermediateTemplateViewModel);
        }
        else
        {        
            qcIntermediateTemplateViewModel.DocumentStatus = (int)QualityCheckDocumentStatus.Submitted;
            response = _qcintermediateTemplateService.InsertData(qcIntermediateTemplateViewModel);
       
             return Json(response.ResponseData);
        }

        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
        return RedirectToAction("IntermediateGridPage", "QCIntermediateTemplate");
    }



}


