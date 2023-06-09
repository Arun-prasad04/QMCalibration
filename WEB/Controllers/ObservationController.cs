using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
namespace WEB.Controllers;
public class ObservationController : BaseController
{
    private IRequestService _requestService { get; set; }
    private IObservationTemplateService _ObservationTemplateService { get; set; }
    private IInstrumentService _instrumentService { get; set; }
    public ObservationController(IObservationTemplateService observationTemplateService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IRequestService requestService, IInstrumentService instrumentService) : base(logger, contextAccessor)
    {
        _ObservationTemplateService = observationTemplateService;
        _requestService = requestService;
        _instrumentService = instrumentService;
    }
    public IActionResult ViewObservation(int requestId, int instrumentId)
    {
        ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentById(instrumentId);
        string templateName = "Gentral";
        if (response.ResponseData != null)
        {
            if (response.ResponseData.ObservationTemplate == 71)
            {
                templateName = "General";
            }

            else if (response.ResponseData.ObservationTemplate == 72)
            {  
                templateName = "LeverDial";
            }
            else if (response.ResponseData.ObservationTemplate == 73)
            {
                templateName = "Micrometer";
            }
            else if (response.ResponseData.ObservationTemplate == 74)
            {
                templateName = "PlungerDial";
            }
            else if (response.ResponseData.ObservationTemplate == 75)
            {
                templateName = "ThreadGauges";
            }
            else if (response.ResponseData.ObservationTemplate == 76)
            {
                templateName = "TWObs";
            }
            else if (response.ResponseData.ObservationTemplate == 77)
            {
                templateName = "VernierCaliper";
            }
            if (response.ResponseData.ObservationTemplate == 155)
            {
                templateName = "GeneralNew";
            }
        }
        return RedirectToAction(templateName, new { requestId = requestId, instrumentId = instrumentId });
    }

    #region "Lever Dial Type"
    public IActionResult LeverDial(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");

        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        LeverTypeDialViewModel leverTypeDialViewModel = new LeverTypeDialViewModel();

        leverTypeDialViewModel.InstrumentId = instrumentId;
        leverTypeDialViewModel.RequestId = requestId;
        leverTypeDialViewModel.Name = instrumentresponse.ResponseData.InstrumentName;
        leverTypeDialViewModel.Range = instrumentresponse.ResponseData.Range;
        leverTypeDialViewModel.Make = instrumentresponse.ResponseData.Make;
        leverTypeDialViewModel.SerialNo = instrumentresponse.ResponseData.SlNo;
        leverTypeDialViewModel.IDNo = instrumentresponse.ResponseData.IdNo;
        leverTypeDialViewModel.RefStd = instrumentresponse.ResponseData.StandardReffered;
        
        ResponseViewModel<LeverTypeDialViewModel> response = _ObservationTemplateService.GetLeverDialById(requestId, instrumentId);

        if (response.ResponseData != null)
        {
            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
                leverTypeDialViewModel.RefWi = Constants.LEVER_DIAL_REFERENCE_WITH_INDICATOR;

            leverTypeDialViewModel.Id = response.ResponseData.Id;
            leverTypeDialViewModel.TemplateObservationId = response.ResponseData.TemplateObservationId;
            leverTypeDialViewModel.TempStart = response.ResponseData.TempStart;
            leverTypeDialViewModel.TempEnd = response.ResponseData.TempEnd;
            leverTypeDialViewModel.Humidity = response.ResponseData.Humidity;
            leverTypeDialViewModel.RefWi = response.ResponseData.RefWi;
            leverTypeDialViewModel.Allvalues = response.ResponseData.Allvalues;
            leverTypeDialViewModel.ReviewStatus = response.ResponseData.ReviewStatus;
            leverTypeDialViewModel.DialIndicatiorCondition = response.ResponseData.DialIndicatiorCondition;
            leverTypeDialViewModel.MeasuringRangeSpec = response.ResponseData.MeasuringRangeSpec;
            leverTypeDialViewModel.MeasuringRangeDirectionA1 = response.ResponseData.MeasuringRangeDirectionA1;
            leverTypeDialViewModel.MeasuringRangeDirectionB1 = response.ResponseData.MeasuringRangeDirectionB1;
            leverTypeDialViewModel.ScaleDivisionSpec = response.ResponseData.ScaleDivisionSpec;
            leverTypeDialViewModel.ScaleDivisionDirectionA2 = response.ResponseData.ScaleDivisionDirectionA2;
            leverTypeDialViewModel.ScaleDivisionDirectionB2 = response.ResponseData.ScaleDivisionDirectionB2;
            leverTypeDialViewModel.HysteresisDirectionA3 = response.ResponseData.HysteresisDirectionA3;
            leverTypeDialViewModel.HysteresisDirectionB3 = response.ResponseData.HysteresisDirectionB3;
            leverTypeDialViewModel.HysteresisSpec = response.ResponseData.HysteresisSpec;
            leverTypeDialViewModel.RepeatabilityDirectionA4 = response.ResponseData.RepeatabilityDirectionA4;
            leverTypeDialViewModel.RepeatabilityDirectionB4 = response.ResponseData.RepeatabilityDirectionB4;
            leverTypeDialViewModel.RepeatabilitySpec = response.ResponseData.RepeatabilitySpec;
            leverTypeDialViewModel.CalibrationPerformedBy = response.ResponseData.CalibrationPerformedBy;
            leverTypeDialViewModel.CalibrationReviewedBy = response.ResponseData.CalibrationReviewedBy;
            leverTypeDialViewModel.ReviewedBy = response.ResponseData.ReviewedBy;
            leverTypeDialViewModel.CalibrationPerformedDate = response.ResponseData.CalibrationPerformedDate;
            leverTypeDialViewModel.CalibrationReviewedDate = response.ResponseData.CalibrationReviewedDate;

            if (userRoleId == 1)
                leverTypeDialViewModel.IsDisabled = "disabled";

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                leverTypeDialViewModel.ReviewedBy = string.Concat(firstName, " ", lastName);
                leverTypeDialViewModel.CalibrationReviewedDate = DateTime.Now;
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                leverTypeDialViewModel.ReviewedBy = string.Concat(firstName, " ", lastName);
                leverTypeDialViewModel.CalibrationReviewedBy = userId;
                leverTypeDialViewModel.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                leverTypeDialViewModel.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }
        }
        else
        {
            leverTypeDialViewModel.IsDisabled = "";
            leverTypeDialViewModel.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            leverTypeDialViewModel.CalibrationPerformedDate = DateTime.Now;
            leverTypeDialViewModel.CalibrationReviewedDate = DateTime.Now;
            leverTypeDialViewModel.RefWi = Constants.LEVER_DIAL_REFERENCE_WITH_INDICATOR;

            if (userRoleId == 4)
            {
                leverTypeDialViewModel.ReviewedBy = string.Concat(firstName, " ", lastName);
                leverTypeDialViewModel.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                leverTypeDialViewModel.ReviewedBy = string.Empty;
                leverTypeDialViewModel.Review_Date = string.Empty;
            }
        }
        return View(leverTypeDialViewModel);
    }
    public IActionResult InsertLeverDial(LeverTypeDialViewModel levertypedial)
    {
        ResponseViewModel<LeverTypeDialViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        levertypedial.CreatedBy = userId;

        response = _ObservationTemplateService.InsertLeverDial(levertypedial);
        using (StreamWriter writer = System.IO.File.AppendText("logfile.txt"))
        {
            writer.WriteLine(response.ErrorMessage + response.ResponseMessage);
        }

        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);

       // return RedirectToAction("ViewObservation", new { requestId = levertypedial.RequestId, instrumentId = levertypedial.InstrumentId });
    }
    #endregion
    public IActionResult InsertMicrometer(MicrometerViewModel micrometer)
    {
        ResponseViewModel<MicrometerViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        micrometer.CreatedBy = userId;

        response = _ObservationTemplateService.InsertMicrometer(micrometer);

        
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);
       /*if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }*/
      // return RedirectToAction("ViewObservation", new { requestId = micrometer.RequestId, instrumentId = micrometer.InstrumentId });
    }

    public IActionResult InsertVernierCaliper(VernierCaliperViewModel verniercaliper)
    {
        ResponseViewModel<VernierCaliperViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        verniercaliper.CreatedBy = userId;

        response = _ObservationTemplateService.InsertVernierCaliper(verniercaliper);

        
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);
      /*if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }*/
        //return RedirectToAction("ViewObservation", new { requestId = verniercaliper.RequestId, instrumentId = verniercaliper.InstrumentId });
    }

    public IActionResult InsertGeneralnewobs(GeneralNewViewModel GeneralNew)
    {
        ResponseViewModel<GeneralNewViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        GeneralNew.CreatedBy = userId;

        response = _ObservationTemplateService.InsertGeneralnewobs(GeneralNew);

        
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);
      /*if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }*/
        //return RedirectToAction("ViewObservation", new { requestId = verniercaliper.RequestId, instrumentId = verniercaliper.InstrumentId });
    }

    

    public IActionResult InsertPlungerDial(PlungerDialViewModel plungerDial)
    {
        ResponseViewModel<PlungerDialViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        plungerDial.CreatedBy = userId;

        response = _ObservationTemplateService.InsertPlungerDial(plungerDial);

        
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);
       /*if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }*/
        //return RedirectToAction("ViewObservation", new { requestId = plungerDial.RequestId, instrumentId = plungerDial.InstrumentId });
    }

    public IActionResult InsertThreadGuages(ThreadGaugesViewModel threadGauges)
    {
        ResponseViewModel<ThreadGaugesViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        threadGauges.CreatedBy = userId;

        response = _ObservationTemplateService.InsertThreadGuages(threadGauges);

        
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);
      /* if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }*/
        //return RedirectToAction("ViewObservation", new { requestId = threadGauges.RequestId, instrumentId = threadGauges.InstrumentId });
    }

    public IActionResult InsertTWobs(TorqueWrenchesViewModel torquewrenches)
    {
        ResponseViewModel<TorqueWrenchesViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        torquewrenches.CreatedBy = userId;

        response = _ObservationTemplateService.InsertTWobs(torquewrenches);
      
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;
        return Json(response.ResponseData);

        //return RedirectToAction("ViewObservation", new { requestId = torquewrenches.RequestId, instrumentId = torquewrenches.InstrumentId });
    }

    public IActionResult InsertGeneral(GeneralViewModel general)
    {
        ResponseViewModel<GeneralViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        general.CreatedBy = userId;

        response = _ObservationTemplateService.InsertGeneral(general);

        
        TempData["ResponseCode"]=response.ResponseCode;
        TempData["ResponseMessage"]=response.ResponseMessage;

        return Json(response.ResponseData);
      /* if(response.ResponseMessage=="Success"){
        return RedirectToAction("Index","Department");
       }else{
           return View(response.ResponseData);
       }*/
     // return RedirectToAction("ViewObservation", new { requestId = general.RequestId, instrumentId = general.InstrumentId });
    }
    public IActionResult General(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");

        ResponseViewModel<GeneralViewModel> response = _ObservationTemplateService.GetGeneralById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;

            if (instrumentresponse.ResponseData != null)
            {
                response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
                response.ResponseData.Range = instrumentresponse.ResponseData.Range;
                response.ResponseData.Make = instrumentresponse.ResponseData.Make;
                response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
                response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
                response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;

               if(response.ResponseData.GeneralAddResultViewModelList == null
                  || response.ResponseData.GeneralAddResultViewModelList.Count() == 0)
               {
                   GeneralResultViewModel generalResultViewModel = new GeneralResultViewModel()
                   {
                        MeasuedValue = 0,
                        Trial1 = 0,
                        Trial2 = 0,
                        Trial3 = 0,
                        Average = 0
                   };
                    List<GeneralResultViewModel> generalResultList = new List<GeneralResultViewModel>();
                    generalResultList.Add(generalResultViewModel);
                    response.ResponseData.GeneralAddResultViewModelList = generalResultList;
               }

               if(response.ResponseData.GeneralManualAddResultViewModelList == null 
                  || response.ResponseData.GeneralManualAddResultViewModelList.Count() == 0 )
                {
                    GeneralManualResultViewModel generalManualResultViewModel = new GeneralManualResultViewModel()
                    {
                        Column1 = "0",
                        Column2 = "0",
                        Column3 = "0",
                        Column4 = "0",
                        Column5 = "0",
                        Column6 = "0"
                    };
                    List<GeneralManualResultViewModel> generalManualResultList = new List<GeneralManualResultViewModel>();
                    generalManualResultList.Add(generalManualResultViewModel);
                    response.ResponseData.GeneralManualAddResultViewModelList = generalManualResultList;
                }

            }

            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }
        }
        else
        {
            ResponseViewModel<GeneralViewModel> responseempty = new ResponseViewModel<GeneralViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            GeneralViewModel micrometer = new GeneralViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;

            if(instrumentresponse.ResponseData != null)
            {
                micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
                micrometer.SerialNo = instrumentresponse.ResponseData.SlNo;
                micrometer.Range = instrumentresponse.ResponseData.Range;
                micrometer.IdNo = instrumentresponse.ResponseData.IdNo;
                micrometer.Make = instrumentresponse.ResponseData.Make;
                micrometer.RefStd = instrumentresponse.ResponseData.StandardReffered;
            }

            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CalibrationPerformedDate = DateTime.Now;
            micrometer.CalibrationReviewedDate = DateTime.Now;

            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }
          
            GeneralResultViewModel generalResultViewModel = new GeneralResultViewModel()
            {
                MeasuedValue = 0,
                Trial1 = 0,
                Trial2 = 0,
                Trial3 = 0,
                Average = 0
            };

            List<GeneralResultViewModel> generalResultList = new List<GeneralResultViewModel>();
            generalResultList.Add(generalResultViewModel);
            micrometer.GeneralAddResultViewModelList = generalResultList;

            GeneralManualResultViewModel generalManualResultViewModel = new GeneralManualResultViewModel()
            {
                Column1 = "0",
                Column2 = "0",
                Column3 = "0",
                Column4 = "0",
                Column5 = "0",
                Column6 = "0"
            };
            List<GeneralManualResultViewModel> generalManualResultList = new List<GeneralManualResultViewModel>();
            generalManualResultList.Add(generalManualResultViewModel);
            micrometer.GeneralManualAddResultViewModelList = generalManualResultList;
            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;

            return View(responseempty.ResponseData);
        }

        return View(response.ResponseData);
    }


    public IActionResult Micrometer(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ResponseViewModel<MicrometerViewModel> response = _ObservationTemplateService.GetMicrometerById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
                response.ResponseData.RefWi = Constants.MICROMETER_REFERENCE_WITH_INDICATOR;

            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);

            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }


        }
        else
        {
            ResponseViewModel<MicrometerViewModel> responseempty = new ResponseViewModel<MicrometerViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            MicrometerViewModel micrometer = new MicrometerViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;
            micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
            micrometer.Range = instrumentresponse.ResponseData.Range;
            micrometer.Make = instrumentresponse.ResponseData.Make;
            micrometer.SerialNo = instrumentresponse.ResponseData.SlNo;
            micrometer.IdNo = instrumentresponse.ResponseData.IdNo;
            micrometer.RefStd = instrumentresponse.ResponseData.StandardReffered;
            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CalibrationPerformedDate = DateTime.Now;
            micrometer.CalibrationReviewedDate = DateTime.Now;
            micrometer.RefWi = Constants.MICROMETER_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }
            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;
            return View(responseempty.ResponseData);
        }
        return View(response.ResponseData);
    }
    public IActionResult PlungerDial(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ResponseViewModel<PlungerDialViewModel> response = _ObservationTemplateService.GetPlungerDialById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
            response.ResponseData.RefWi = Constants.PLUNGER_DIAL_REFERENCE_WITH_INDICATOR;
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.RangeLC = instrumentresponse.ResponseData.Range;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.SerialNumber = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNumber = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.ReferenceStandard = instrumentresponse.ResponseData.StandardReffered;
            response.ResponseData.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }
        }
        else
        {
            ResponseViewModel<PlungerDialViewModel> responseempty = new ResponseViewModel<PlungerDialViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            PlungerDialViewModel micrometer = new PlungerDialViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;
            micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
            micrometer.RangeLC = instrumentresponse.ResponseData.Range;
            micrometer.Make = instrumentresponse.ResponseData.Make;
            micrometer.SerialNumber = instrumentresponse.ResponseData.SlNo;
            micrometer.IdNumber = instrumentresponse.ResponseData.IdNo;
            micrometer.ReferenceStandard = instrumentresponse.ResponseData.StandardReffered;
            micrometer.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CalibrationPerformedDate = DateTime.Now;
            micrometer.RefWi = Constants.PLUNGER_DIAL_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName," ",lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }

            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
            }
            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;
            return View(responseempty.ResponseData);
        }

        return View(response.ResponseData);
    }
    public IActionResult ThreadGauges(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ResponseViewModel<ThreadGaugesViewModel> response = _ObservationTemplateService.GetThreadGaugesById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
                response.ResponseData.RefWi = Constants.THREAD_GAUGE_REFERENCE_WITH_INDICATOR;

            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);

            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.ObsSubType = instrumentresponse.ResponseData.ObservationType;

            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }

        }
        else
        {
            ResponseViewModel<ThreadGaugesViewModel> responseempty = new ResponseViewModel<ThreadGaugesViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            ThreadGaugesViewModel micrometer = new ThreadGaugesViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;
            micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
            micrometer.SerialNo = instrumentresponse.ResponseData.SlNo;
            micrometer.IdNo = instrumentresponse.ResponseData.IdNo;
            micrometer.Range = instrumentresponse.ResponseData.Range;
            micrometer.RefStd = instrumentresponse.ResponseData.StandardReffered;
            micrometer.Make = instrumentresponse.ResponseData.Make;
            micrometer.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CalibrationPerformedDate = DateTime.Now;
            micrometer.CalibrationReviewedDate = DateTime.Now;
            micrometer.RefWi = Constants.THREAD_GAUGE_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }

            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;
            return View(responseempty.ResponseData);
        }
        return View(response.ResponseData);
    }
    public IActionResult TWObs(int requestId, int instrumentId)
    {

        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ViewBag.ResponseCode = TempData["ResponseCode"];  
        ViewBag.ResponseMessage = TempData["ResponseMessage"];
        ResponseViewModel<TorqueWrenchesViewModel> response = _ObservationTemplateService.GetTorqueWrenchesById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            response.ResponseData.ObsSubType = instrumentresponse.ResponseData.ObservationType;

            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
                response.ResponseData.RefWi = Constants.TORQUE_WRENCH_REFERENCE_WITH_INDICATOR;

            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }
        }
        else
        {
            ResponseViewModel<TorqueWrenchesViewModel> responseempty = new ResponseViewModel<TorqueWrenchesViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            TorqueWrenchesViewModel micrometer = new TorqueWrenchesViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;
            micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
            micrometer.SerialNo = instrumentresponse.ResponseData.SlNo;
            micrometer.Range = instrumentresponse.ResponseData.Range;
            micrometer.IdNo = instrumentresponse.ResponseData.IdNo;
            micrometer.Make = instrumentresponse.ResponseData.Make;
            micrometer.RefStd = instrumentresponse.ResponseData.StandardReffered;
            micrometer.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CreatedOn = DateTime.Now;
            micrometer.RefWi = Constants.TORQUE_WRENCH_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }

            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;
            return View(responseempty.ResponseData);
        }
        return View(response.ResponseData);
    }
    public IActionResult VernierCaliper(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ResponseViewModel<VernierCaliperViewModel> response = _ObservationTemplateService.GetVernierCaliperById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
                response.ResponseData.RefWi = Constants.VERNIER_CALIPER_REFERENCE_WITH_INDICATOR;

            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            response.ResponseData.ObsSubType = instrumentresponse.ResponseData.ObservationType;

            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }

        }
        else
        {
            ResponseViewModel<VernierCaliperViewModel> responseempty = new ResponseViewModel<VernierCaliperViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            VernierCaliperViewModel micrometer = new VernierCaliperViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;
            micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
            micrometer.SerialNo = instrumentresponse.ResponseData.SlNo;
            micrometer.Range = instrumentresponse.ResponseData.Range;
            micrometer.IdNo = instrumentresponse.ResponseData.IdNo;
            micrometer.Make = instrumentresponse.ResponseData.Make;
            micrometer.RefStd = instrumentresponse.ResponseData.StandardReffered;
            micrometer.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CalibrationPerformedDate = DateTime.Now;
            micrometer.CalibrationReviewedDate = DateTime.Now;
            micrometer.RefWi = Constants.VERNIER_CALIPER_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }

           
            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;
            return View(responseempty.ResponseData);
        }
        return View(response.ResponseData);
    }

    public IActionResult SubmitReview(int observationId, DateTime reviewDate, int reviewStatus)
    {
         int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
         ResponseViewModel<LeverTypeDialViewModel> response =  _ObservationTemplateService.SubmitReview(observationId, reviewDate, reviewStatus,userId);
         return Json(response.ResponseData);
    }
    public IActionResult GeneralNew(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ResponseViewModel<GeneralNewViewModel> response = _ObservationTemplateService.GetGeneralNewById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            if( response.ResponseData.RefWi == null ||  response.ResponseData.RefWi == string.Empty )
                response.ResponseData.RefWi = Constants.VERNIER_CALIPER_REFERENCE_WITH_INDICATOR;

            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            response.ResponseData.ObsSubType = instrumentresponse.ResponseData.ObservationType;

            if (userRoleId == 1)
            {
                response.ResponseData.IsDisabled = "disabled";
            }
            else
            {
                response.ResponseData.IsDisabled = "";
            }

            if (userRoleId == 4 && response.ResponseData.ReviewStatus == null)
            {
                response.ResponseData.ReviewedBy = string.Concat(firstName, " ", lastName);
                response.ResponseData.CalibrationReviewedBy = userId;
                response.ResponseData.CalibrationReviewedDate = DateTime.Now;
            }
            else  if (userRoleId != 4 && response.ResponseData.ReviewStatus != null )
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }

        }
        else
        {
            ResponseViewModel<GeneralNewViewModel> responseempty = new ResponseViewModel<GeneralNewViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            GeneralNewViewModel micrometer = new GeneralNewViewModel();
            micrometer.InstrumentId = instrumentId;
            micrometer.RequestId = requestId;
            micrometer.Name = instrumentresponse.ResponseData.InstrumentName;
            micrometer.SerialNo = instrumentresponse.ResponseData.SlNo;
            micrometer.Range = instrumentresponse.ResponseData.Range;
            micrometer.IdNo = instrumentresponse.ResponseData.IdNo;
            micrometer.Make = instrumentresponse.ResponseData.Make;
            micrometer.RefStd = instrumentresponse.ResponseData.StandardReffered;
            micrometer.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            micrometer.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            micrometer.CalibrationPerformedDate = DateTime.Now;
            micrometer.CalibrationReviewedDate = DateTime.Now;
           // micrometer.RefWi = Constants.VERNIER_CALIPER_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
                micrometer.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                micrometer.ReviewedBy = string.Empty;
                micrometer.Review_Date = string.Empty;
            }

           
            if (userRoleId == 1)
            {
                micrometer.IsDisabled = "disabled";
            }
            else
            {
                micrometer.IsDisabled = "";
            }
            responseempty.ResponseData = micrometer;
            return View(responseempty.ResponseData);
        }
        return View(response.ResponseData);
    }
}


