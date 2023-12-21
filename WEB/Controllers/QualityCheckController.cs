using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System.Text;

namespace WEB.Controllers;

public class QualityCheckController : BaseController
{
    private readonly ILogger<BaseController> _logger;

    private IQCReplicateTestTemplateService _qcreplicatetestTemplateService { get; set; }
    private IQCReTestTemplateService _qcretestTemplateService { get; set; }
    private IQCAlternateMethodTemplateService _qcalternateMethodTemplateService { get; set; }
    private IInstrumentService _instrumentService { get; set; }
    private IMasterService _masterService { get; set; }
    private IUserService _userService { get; set; }

    public QualityCheckController(IQCAlternateMethodTemplateService qcalternateMethodTemplateService, IQCReplicateTestTemplateService qcreplicatetestTemplateService, IQCReTestTemplateService qcretestTemplateService, IMasterService masterService, IInstrumentService instrumentService, IUserService userService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor) : base(logger, contextAccessor)
    {
        _qcreplicatetestTemplateService = qcreplicatetestTemplateService;
        _qcretestTemplateService = qcretestTemplateService;
        _qcalternateMethodTemplateService = qcalternateMethodTemplateService;
        _logger = logger;
        _instrumentService = instrumentService;
        _masterService = masterService;
        _userService = userService;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    public IActionResult AlternateTemplateGrid()
    {
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        ResponseViewModel<QCAlternateMethodTemplateViewModel> response = _qcalternateMethodTemplateService.GetAllAlternateMethodList();
        if (response.ResponseDataList == null)
        {
            return View(new List<QCAlternateMethodTemplateViewModel>());
        }
        else
        {
            response.ResponseDataList.Where(x => x.TemplateStatus == (int)(DocumentStatus.Submitted)).ToList().ForEach(i => i.Status = "Submited");
            response.ResponseDataList.Where(x => x.TemplateStatus == (int)(DocumentStatus.Approved)).ToList().ForEach(i => i.Status = "Approved");
            response.ResponseDataList.Where(x => x.TemplateStatus == (int)(DocumentStatus.Rejected)).ToList().ForEach(i => i.Status = "Rejected");
        }
        return View(response.ResponseDataList);
    }

    //AlternativeeMethodTemplate Starts 
    public IActionResult QCAlternateMethodTemplate(int Id)
    {
        ViewBag.PageTitle = "Alternative Template";

        //Get Session data
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));

        ResponseViewModel<QCAlternateMethodTemplateViewModel> alternateExistingData = new ResponseViewModel<QCAlternateMethodTemplateViewModel>();
        if (Id > 0)
        {
            alternateExistingData = _qcalternateMethodTemplateService.GetById(Id);
            if (alternateExistingData.ResponseData != null)
            {
                alternateExistingData.ResponseData.ReviewedByName = alternateExistingData.ResponseData.ReviewedByName == null
                                                                  ? string.Concat(firstName.Trim(), " ", lastName.Trim())
                                                                  : alternateExistingData.ResponseData.ReviewedByName;

                alternateExistingData.ResponseData.ReviewedBy = alternateExistingData.ResponseData.ReviewedBy == null
                                                                   ? userId
                                                                   : alternateExistingData.ResponseData.ReviewedBy;

                alternateExistingData.ResponseData.ReviewedOn = alternateExistingData.ResponseData.ReviewedOn == null
                                                                    ? DateTime.Now
                                                                    : alternateExistingData.ResponseData.ReviewedOn;

                alternateExistingData.ResponseData.LoginUserRoleId = alternateExistingData.ResponseData.LoginUserRoleId == null
                                                                      || alternateExistingData.ResponseData.LoginUserRoleId == 0
                                                                      ? userRoleId
                                                                      : alternateExistingData.ResponseData.LoginUserRoleId;
                alternateExistingData.ResponseData.RevisionNoAndDate = string.Concat(alternateExistingData.ResponseData.RevisionNo,
                                                                       "/", (alternateExistingData.ResponseData.RevisionDate.ToString("dd.MM.yyyy")));
            }
        }

        if (alternateExistingData.ResponseData == null)
        {
            alternateExistingData.ResponseData = new QCAlternateMethodTemplateViewModel()
            {
                Id = 0,
                TemplateStatus = 0,
                EvaluatedBy = userId,
                LoginUserRoleId = userRoleId,
                EvaluatedByName = string.Concat(firstName.Trim(), " ", lastName.Trim()),
              
                FormatNo = Constants.ALTERNATIVE_FORMATNUMER,
                RevisionNo = Constants.ALTERNATIVE_REVISIONNO,
                RevisionDate = Constants.ALTERNATIVE_REVISIONDATE,
                RevisionNoAndDate = Constants.ALTERNATIVE_REVISIONNO_DATE,             
                EvaluationOn = DateTime.Now,
                QCEquipmentOneMeasuredValues = new QCAlternateMethodTemplateDataViewModel(),
                QCEquipmentTwoMeasuredValues = new QCAlternateMethodTemplateDataViewModel()
            };
        }
        return View(alternateExistingData.ResponseData);
    }

    public IActionResult GetInstrumentsListByName(string instrumentName)
    {
        var instrumentsList = new List<InstrumentListViewModel>();
        ResponseViewModel<InstrumentViewModel> dbInstrumentListByName = _instrumentService.GetInstrumentListByName(instrumentName);

        if (!(dbInstrumentListByName.ResponseDataList.Any()))
            return Json(instrumentsList);

        foreach (var instrument in dbInstrumentListByName.ResponseDataList)
        {
            var instrumentInfo = new InstrumentListViewModel()
            {
                InstrumentId = instrument.Id,
                RangeOrSize = instrument.Range,
                InstrumentName = instrument.InstrumentName,
                LeastCount = instrument.LC
            };

            instrumentsList.Add(instrumentInfo);
        }
        return Json(instrumentsList);
    }

     public IActionResult GetInstrumentsListByIdNo(string idNo)
    {
        var instrumentsList = new List<InstrumentListViewModel>();
        ResponseViewModel<InstrumentViewModel> dbInstrumentListByName = _instrumentService.GetInstrumentListByIdNo(idNo);

        if (!(dbInstrumentListByName.ResponseDataList.Any()))
            return Json(instrumentsList);

        foreach (var instrument in dbInstrumentListByName.ResponseDataList)
        {
            var instrumentInfo = new InstrumentListViewModel()
            {
                InstrumentId = instrument.Id,
                IdNo = instrument.IdNo,
                RangeOrSize = instrument.Range,
                InstrumentName = instrument.InstrumentName,
                LeastCount = instrument.LC
            };

            instrumentsList.Add(instrumentInfo);
        }
        return Json(instrumentsList);
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
                LabId = master.EquipmentMasterId,
                EquipmentName = master.EquipName,
                CalibDate = master.CalibDate
            };

            masterList.Add(masterInfo);
        }
        return Json(masterList);
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
                CalibDate = master.CalibDate
            };

            masterList.Add(masterInfo);
        }
        return Json(masterList);
    }

    [HttpPost]
    public IActionResult SaveData(QCAlternateMethodTemplateViewModel qcalternatemethodtemplateViewModel)
    {
        ResponseViewModel<QCAlternateMethodTemplateViewModel> response;
        if (qcalternatemethodtemplateViewModel.Id > 0)
        {
            response = _qcalternateMethodTemplateService.UpdateData(qcalternatemethodtemplateViewModel);
        }
        else
        {
            qcalternatemethodtemplateViewModel.TemplateStatus = (int)QualityCheckDocumentStatus.Submitted;
            qcalternatemethodtemplateViewModel.QCEquipmentOneMeasuredValues.SlNo = (int)QualityCheckSerialNo.SerialNoOne;
            qcalternatemethodtemplateViewModel.QCEquipmentTwoMeasuredValues.SlNo = (int)QualityCheckSerialNo.SerialNoTwo;
            response = _qcalternateMethodTemplateService.InsertData(qcalternatemethodtemplateViewModel);
        }

        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;

        return RedirectToAction("AlternateTemplateGrid", "QualityCheck");
    }
    // AlternateMethodTemplate Ends 

    //ReplicateTestTemplate Starts
    public IActionResult QCReplicateTest()
    {
        return View();
    }
    public IActionResult QCReTest()
    {
        return View();
    }
    public IActionResult ReplicateTestGrid()
    { 
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];

        ResponseViewModel<ReplicateTestViewModel> response = _qcreplicatetestTemplateService.GetReplicateTestGridData();
        return View(response.ResponseDataList);
    }

    public ActionResult ReplicateTest(int ReplicateId)
    {
        ViewBag.PageTitle = "Replicate Test";
        ViewBag.UserName = base.SessionGetString("FirstName");
        ViewBag.UserRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        //Get Session data
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string ShortId = base.SessionGetString("ShortId");

        ResponseViewModel<ReplicateTestViewModel> ReplicateExistingData = new ResponseViewModel<ReplicateTestViewModel>();
        if (ReplicateId > 0)
        {
            ReplicateExistingData = _qcreplicatetestTemplateService.GetByTemplateData(ReplicateId);
            if (ReplicateExistingData.ResponseData != null &&
               ReplicateExistingData.ResponseData.DocumentStatus == (int)ReplicateTestStatus.ResultOneSubmitted)
            {
                if (ReplicateExistingData.ResponseData.Obs2 == null)
                {
                    ReplicateExistingData.ResponseData.Obs2 = new ReplicateTestDataViewModel()
                    {
                        AppraiserFullName = string.Concat(firstName.Trim(), " ", lastName.Trim()),
                        AppraiserDate = DateTime.Now
                    };
                }
                else
                {
                    ReplicateExistingData.ResponseData.Obs2.AppraiserFullName = string.Concat(firstName.Trim(), " ", lastName.Trim());
                    ReplicateExistingData.ResponseData.Obs2.AppraiserDate = DateTime.Now;
                }

               
            }
            else if (ReplicateExistingData.ResponseData != null &&
               ReplicateExistingData.ResponseData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
            {
                ReplicateExistingData.ResponseData.ReviewedByName = string.Concat(firstName.Trim(), " ", lastName.Trim());

            }

             ReplicateExistingData.ResponseData.RevisionNoAndDate = string.Concat(ReplicateExistingData.ResponseData.RevisionNo,
                                                                        "/", ReplicateExistingData.ResponseData.RevisionDate
                                                                                                   .Value.ToString("dd.MM.yyyy"));
        }
        if (ReplicateExistingData.ResponseData == null)
        {
            ReplicateExistingData.ResponseData = new ReplicateTestViewModel()
            {
                Id = 0,
                FormatNo = Constants.REPLICATE_FORMATNUMER,
                RevisionNo = Constants.REPLICATE_REVISIONNO,
                RevisionDate = Constants.REPLICATE_REVISIONDATE,
                RevisionNoAndDate = Constants.REPLICATE_REVISIONNO_DATE,
                Obs1 = new ReplicateTestDataViewModel()
                {
                    AppraiserDate = DateTime.Now,
                    AppraiserFullName = string.Concat(firstName.Trim(), " ", lastName.Trim())
                },
                Obs2 = new ReplicateTestDataViewModel()
                {
                    AppraiserDate = DateTime.Now
                }
            };
        }
        return View(ReplicateExistingData.ResponseData);
    }


    // ReplicateTest Insert and Update Starts
    [HttpPost]
    public IActionResult SaveDataReplicate(ReplicateTestViewModel qcReplicateTestTemplateViewModel)
    {
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string ShortId = base.SessionGetString("ShortId");

        ResponseViewModel<ReplicateTestViewModel> response;
        if (qcReplicateTestTemplateViewModel.Id > 0)
        {
            if (qcReplicateTestTemplateViewModel.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
            {
                qcReplicateTestTemplateViewModel.ReviewedBy = ShortId;
                qcReplicateTestTemplateViewModel.ReviewedOn = DateTime.Now.Date;
                if (qcReplicateTestTemplateViewModel.IsApproved == 1)
                {
                    qcReplicateTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.Approved;
                }
                else
                {
                    qcReplicateTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.Rejected;
                }
            }
            if (qcReplicateTestTemplateViewModel.DocumentStatus == (int)ReplicateTestStatus.ResultOneSubmitted)
            {
                qcReplicateTestTemplateViewModel.ModifiedOn = DateTime.Now.Date;
                qcReplicateTestTemplateViewModel.ModifiedBy = ShortId;
                qcReplicateTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.ResultTwoSubmitted;
                qcReplicateTestTemplateViewModel.Obs2.AppraiserName = ShortId;
                qcReplicateTestTemplateViewModel.Obs2.AppraiserDate = DateTime.Now.Date;
                qcReplicateTestTemplateViewModel.Obs2.AppraiserNo = userId;
            }
            response = _qcreplicatetestTemplateService.UpdateData(qcReplicateTestTemplateViewModel);
        }
        else
        {
            qcReplicateTestTemplateViewModel.DateConducted = DateTime.Now.Date;
            qcReplicateTestTemplateViewModel.CreatedOn = DateTime.Now.Date;
            qcReplicateTestTemplateViewModel.CreatedBy = ShortId;
            qcReplicateTestTemplateViewModel.ModifiedOn = DateTime.Now.Date;
            qcReplicateTestTemplateViewModel.ModifiedBy = ShortId;
            qcReplicateTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.ResultOneSubmitted;
            qcReplicateTestTemplateViewModel.Obs1.AppraiserDate = DateTime.Now.Date;
            qcReplicateTestTemplateViewModel.Obs1.AppraiserNo = userId;
            qcReplicateTestTemplateViewModel.Obs1.AppraiserName = ShortId;
            response = _qcreplicatetestTemplateService.InsertData(qcReplicateTestTemplateViewModel);
        }
        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
        return RedirectToAction("ReplicateTestGrid", "QualityCheck");
    }
    //ReTest Starts
    public IActionResult ReTestGrid()
    {
        ViewBag.ResponseCode = TempData["ResponseCode"];
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        ResponseViewModel<ReTestViewModel> response = _qcretestTemplateService.GetReTestGridData();
        return View(response.ResponseDataList);
    }

    public ActionResult ReTest(int ReTestId)
    {
        ViewBag.PageTitle = "Re-Test";
        ViewBag.UserName = base.SessionGetString("FirstName");
        ViewBag.UserRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        //Get Session data
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));

        ResponseViewModel<ReTestViewModel> ReTestExistingData = new ResponseViewModel<ReTestViewModel>();
        if (ReTestId > 0)
        {
            ReTestExistingData = _qcretestTemplateService.GetByTemplateData(ReTestId);

            if (ReTestExistingData.ResponseData != null &&
                ReTestExistingData.ResponseData.DocumentStatus == (int)ReplicateTestStatus.ResultOneSubmitted)
            {
                if (ReTestExistingData.ResponseData.Obs2 == null)
                {
                    ReTestExistingData.ResponseData.Obs2 = new ReTestDataViewModel()
                    {
                        AppraiserFullName = string.Concat(firstName.Trim(), " ", lastName.Trim()),
                        AppraiserDate = DateTime.Now
                    };
                }
                else
                {
                    ReTestExistingData.ResponseData.Obs2.AppraiserFullName = string.Concat(firstName.Trim(), " ", lastName.Trim());
                    ReTestExistingData.ResponseData.Obs2.AppraiserDate = DateTime.Now;
                }
            }
            else if (ReTestExistingData.ResponseData != null &&
                     ReTestExistingData.ResponseData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
            {
                ReTestExistingData.ResponseData.ReviewedByName = string.Concat(firstName.Trim(), " ", lastName.Trim());

            }

            ReTestExistingData.ResponseData.RevisionNoAndDate = string.Concat(ReTestExistingData.ResponseData.RevisionNo,
                                                        "/", (ReTestExistingData.ResponseData.RevisionDate
                                                                                    .Value.ToString("dd.MM.yyyy")));
        }
        if (ReTestExistingData.ResponseData == null)
        {
            ReTestExistingData.ResponseData = new ReTestViewModel()
            {
                Id = 0,
                ReviewedBy = string.Concat(firstName.Trim(), " ", lastName.Trim()),
                FormatNo = Constants.RESTEST_FORMATNUMER,
                RevisionNo = Constants.RESTEST_REVISIONNO,
                RevisionDate = Constants.RETEST_REVISIONDATE,
                RevisionNoAndDate = Constants.RESTEST_REVISIONNO_DATE,
                Obs1 = new ReTestDataViewModel()
                {
                    AppraiserDate = DateTime.Now,
                    AppraiserFullName = string.Concat(firstName.Trim(), " ", lastName.Trim())
                },
                Obs2 = new ReTestDataViewModel()
                {
                    AppraiserDate = DateTime.Now
                }
            };
        }
        return View(ReTestExistingData.ResponseData);
    }
    //ReTest Insert and Update Ends
    [HttpPost]
    public IActionResult SaveDataReTest(ReTestViewModel qcReTestTemplateViewModel)
    {
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        string departmentName = base.SessionGetString("DepartmentName");
        int departmentId = Convert.ToInt32(base.SessionGetString("DepartmentId"));
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string ShortId = base.SessionGetString("ShortId");
        ResponseViewModel<ReTestViewModel> response;
        if (qcReTestTemplateViewModel.Id > 0)
        {
            if (qcReTestTemplateViewModel.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
            {
                qcReTestTemplateViewModel.ReviewedBy = ShortId;
                qcReTestTemplateViewModel.ReviewedOn = DateTime.Now.Date;
                if (qcReTestTemplateViewModel.IsApproved == 1)
                {
                    qcReTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.Approved;
                }
                else
                {
                    qcReTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.Rejected;
                }
            }
            if (qcReTestTemplateViewModel.DocumentStatus == (int)ReplicateTestStatus.ResultOneSubmitted)
            {
                qcReTestTemplateViewModel.ModifiedBy = ShortId;
                qcReTestTemplateViewModel.ModifiedOn = DateTime.Now.Date;
                qcReTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.ResultTwoSubmitted;
                qcReTestTemplateViewModel.Obs2.AppraiserDate = DateTime.Now.Date;
                qcReTestTemplateViewModel.Obs2.AppraiserName = ShortId;
                qcReTestTemplateViewModel.Obs2.AppraiserNo = userId;
            }
            response = _qcretestTemplateService.UpdateData(qcReTestTemplateViewModel);
        }
        else
        {
            qcReTestTemplateViewModel.DateConducted = DateTime.Now.Date;
            qcReTestTemplateViewModel.CreatedOn = DateTime.Now.Date;
            qcReTestTemplateViewModel.CreatedBy = ShortId;
            qcReTestTemplateViewModel.ModifiedOn = DateTime.Now.Date;
            qcReTestTemplateViewModel.ModifiedBy = ShortId;
            qcReTestTemplateViewModel.DocumentStatus = (int)ReplicateTestStatus.ResultOneSubmitted;
            qcReTestTemplateViewModel.Obs1.AppraiserDate = DateTime.Now.Date;
            qcReTestTemplateViewModel.Obs1.AppraiserNo = userId;
            qcReTestTemplateViewModel.Obs1.AppraiserName = ShortId;  
            response = _qcretestTemplateService.InsertData(qcReTestTemplateViewModel);
        }
        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
        return RedirectToAction("ReTestGrid", "QualityCheck");
    }
}


