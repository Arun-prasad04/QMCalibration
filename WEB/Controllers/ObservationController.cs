using System.Diagnostics;
using CMT.DAL;
using CMT.DATAMODELS;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using AutoMapper;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Diagnostics.Metrics;
using WEB.Models.Templates;
using Nancy;

namespace WEB.Controllers;
public class ObservationController : BaseController
{
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork { get; set; }
    private IRequestService _requestService { get; set; }
    private IObservationTemplateService _ObservationTemplateService { get; set; }
    private IInstrumentService _instrumentService { get; set; }
    private IMasterService _iMasterService { get; set; }
    public ObservationController(IObservationTemplateService observationTemplateService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IRequestService requestService, IInstrumentService instrumentService, IUnitOfWork unitOfWork) : base(logger, contextAccessor)
    {
        _ObservationTemplateService = observationTemplateService;
        _requestService = requestService;
        _instrumentService = instrumentService;
        _unitOfWork = unitOfWork;
    }
    public IActionResult ViewObservation(int requestId, int instrumentId)
    {
        //ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentById(instrumentId);
        int ObservationTemplateId = _instrumentService.GetObservationTemplateId(instrumentId, "Observation");


        string templateName = "Gentral";
        if (ObservationTemplateId != null)
        {
            if (ObservationTemplateId == 71)
            {
                templateName = "General";
            }

            else if (ObservationTemplateId == 72)
            {
                templateName = "LeverDial";
            }
            else if (ObservationTemplateId == 73)
            {
                templateName = "Micrometer";
            }
            else if (ObservationTemplateId == 74)
            {
                templateName = "PlungerDial";
            }
            else if (ObservationTemplateId == 75)
            {
                templateName = "ThreadGauges";
            }
            else if (ObservationTemplateId == 76)
            {
                templateName = "TWObs";
            }
            else if (ObservationTemplateId == 77)
            {
                templateName = "VernierCaliper";
            }
            if (ObservationTemplateId == 155)
            {
                templateName = "GeneralNew";
            }
            if(ObservationTemplateId == 01)
            {
                templateName = "ExternalObs";
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
            if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
            else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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

        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
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
		//if (ViewBag.ObservationTypeMicro == "Depth micrometer")//For Depth micrometer
		//{
		//	micrometer.Flatness1 = "1";
		//	micrometer.InstrumentErrValue = "1";
		//}

		response = _ObservationTemplateService.InsertMicrometer(micrometer);


		return Json(response.ResponseData);

	}

    public IActionResult InsertVernierCaliper(VernierCaliperViewModel verniercaliper)
    {
        ResponseViewModel<VernierCaliperViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        verniercaliper.CreatedBy = userId;

        response = _ObservationTemplateService.InsertVernierCaliper(verniercaliper);


        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
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


        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
        return Json(response.ResponseData);
        //if (response.ResponseMessage == "Success")
        //{
        //	return RedirectToAction("ViewObservation", new { requestId = verniercaliper.RequestId, instrumentId = verniercaliper.InstrumentId });
        //	// return RedirectToAction("Index", "Department");
        //}
        //else
        //{
        //	return View(response.ResponseData);
        //}

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


        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
        return Json(response.ResponseData);

    }

    public IActionResult InsertThreadGuages(ThreadGaugesViewModel threadGauges)
    {
        ResponseViewModel<ThreadGaugesViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        threadGauges.CreatedBy = userId;

        response = _ObservationTemplateService.InsertThreadGuages(threadGauges);


        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
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

        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;
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


        TempData["ResponseCode"] = response.ResponseCode;
        TempData["ResponseMessage"] = response.ResponseMessage;

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

                if (response.ResponseData.GeneralAddResultViewModelList == null
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

                if (response.ResponseData.GeneralManualAddResultViewModelList == null
                   || response.ResponseData.GeneralManualAddResultViewModelList.Count() == 0)
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
            else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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

            if (instrumentresponse.ResponseData != null)
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
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);

        ResponseViewModel<MasterViewModel> MasterEqiupmentList = _ObservationTemplateService.GetEquipmentListByInstrumentId(Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument1), Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument2), Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument3), Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument4));
        ViewBag.ObservationTypeMicro = "";
        var objtype = instrumentresponse.ResponseData.ObservationType;

        if (!(objtype.Equals(0) || objtype.Equals(null)))
        {

            Lovs objlovsModel = _unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(objtype)).SingleOrDefault();

            if (objlovsModel != null)
            {
                if (objlovsModel.Id != 0)
                {
                    ViewBag.ObservationTypeMicro = objlovsModel.AttrValue;
                }
            }
            //HttpContext.Session.SetString("ObservationMicro", objlovsModel.AttrValue);
        }
        ResponseViewModel<MicrometerViewModel> response = _ObservationTemplateService.GetMicrometerById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
                response.ResponseData.RefWi = Constants.MICROMETER_REFERENCE_WITH_INDICATOR;

            //List<MasterViewModel> EqiupmentListMaster = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == instrumentresponse.ResponseData.MasterInstrument1 || Q.Id == instrumentresponse.ResponseData.MasterInstrument2 || Q.Id == instrumentresponse.ResponseData.MasterInstrument3 || Q.Id == instrumentresponse.ResponseData.MasterInstrument4).ToList());

            response.ResponseData.MasterEqiupmentList = MasterEqiupmentList.ResponseDataList;
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            response.ResponseData.Grade = instrumentresponse.ResponseData.Grade;
            // instrumentresponse.ResponseData.ObservationType = 1159)



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
            else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }

            if (response.ResponseData.MicrometerAddResultViewModelList == null
                 || response.ResponseData.MicrometerAddResultViewModelList.Count() == 0)
            {
                MicrometerResultViewModel MicroResultViewModel = new MicrometerResultViewModel()
                {
                    SNO = 4,
                    MeasuedValue = "0",
                    ActualsT1 = "0",
                    Diff1 = "0",
                };

                List<MicrometerResultViewModel> MicroResultList = new List<MicrometerResultViewModel>();
                MicroResultList.Add(MicroResultViewModel);
                response.ResponseData.MicrometerAddResultViewModelList = MicroResultList;
            }
            //if (response.ResponseData.MicrometerAddResultViewModelTwoList == null
            //	 || response.ResponseData.MicrometerAddResultViewModelTwoList.Count() == 0)
            //{
            //	MicrometerResultViewModel MicroResultViewModel = new MicrometerResultViewModel()
            //	{
            //		SNO = 5,
            //		MeasuedValue = "0",
            //		ActualsT1 = "0",
            //		Diff1 = "0",
            //	};

            //	List<MicrometerResultViewModel> MicroResultList = new List<MicrometerResultViewModel>();
            //	MicroResultList.Add(MicroResultViewModel);
            //	response.ResponseData.MicrometerAddResultViewModelTwoList = MicroResultList;
            //}
        }
        else
        {
            ResponseViewModel<MicrometerViewModel> responseempty = new ResponseViewModel<MicrometerViewModel>();

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
            micrometer.Grade = instrumentresponse.ResponseData.Grade;
            micrometer.MasterEqiupmentList = MasterEqiupmentList.ResponseDataList;
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

            MicrometerResultViewModel MicroResultViewModel = new MicrometerResultViewModel()
            {
                SNO = 4,
                MeasuedValue = "0",
                ActualsT1 = "0",
                Diff1 = "0",
            };

            List<MicrometerResultViewModel> MicroResultList = new List<MicrometerResultViewModel>();
            MicroResultList.Add(MicroResultViewModel);
            micrometer.MicrometerAddResultViewModelList = MicroResultList;
            //For Parallelism Check Add New Row Option
            //MicrometerResultViewModel MicroResultViewModelTwo = new MicrometerResultViewModel()
            //{
            //	SNO = 5,
            //	MeasuedValue = "0",
            //	ActualsT1 = "0",
            //	Diff1 = "0",
            //};

            //List<MicrometerResultViewModel> MicroResultListTwo = new List<MicrometerResultViewModel>();
            //MicroResultListTwo.Add(MicroResultViewModelTwo);
            //response.ResponseData.MicrometerAddResultViewModelTwoList = MicroResultListTwo;
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
            if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
            else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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
                micrometer.ReviewedBy = string.Concat(firstName, " ", lastName);
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
            if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
			else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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

			if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
			else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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
			if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
			else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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
		ResponseViewModel<LeverTypeDialViewModel> response = _ObservationTemplateService.SubmitReview(observationId, reviewDate, reviewStatus, userId);
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
			if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
			else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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

	public IActionResult MetalRules(int requestId, int instrumentId)
	{

		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		string firstName = base.SessionGetString("FirstName");
		string lastName = base.SessionGetString("LastName");
		ResponseViewModel<MetalRulesViewModel> response = _ObservationTemplateService.GetMetalRulesId(requestId, instrumentId);
		ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
		ResponseViewModel<MasterViewModel> MasterEqiupmentList = _ObservationTemplateService.GetEquipmentListByInstrumentId(Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument1), Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument2), Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument3), Convert.ToInt32(instrumentresponse.ResponseData.MasterInstrument4));


		if (instrumentresponse.ResponseData != null)
		{
			var observationyype = instrumentresponse.ResponseData.ObservationType;
			if (!(observationyype.Equals(0) || observationyype.Equals(null)))
			{

				Lovs objlovsModel = _unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(observationyype)).SingleOrDefault();

				if (objlovsModel != null)
				{
					if (objlovsModel.Id != 0)
					{
						ViewBag.ObservationTypeMetal = objlovsModel.AttrValue;
						ViewBag.ObservationTypeMetalId = objlovsModel.Id;
					}
				}
			}
		}

		if (response.ResponseData != null)
		{

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
				response.ResponseData.Grade = instrumentresponse.ResponseData.Grade;
				response.ResponseData.Unit1 = instrumentresponse.ResponseData.Unit1;
				response.ResponseData.MasterEqiupmentList = MasterEqiupmentList.ResponseDataList;
				response.ResponseData.TemplateObservationId = response.ResponseData.Id;

				//if (response.ResponseData.GeneralAddResultViewModelList == null
				//   || response.ResponseData.GeneralAddResultViewModelList.Count() == 0)
				//{
				//	GeneralResultViewModel generalResultViewModel = new GeneralResultViewModel()
				//	{
				//		MeasuedValue = 0,
				//		Trial1 = 0,
				//		Trial2 = 0,
				//		Trial3 = 0,
				//		Average = 0
				//	};
				//	List<GeneralResultViewModel> generalResultList = new List<GeneralResultViewModel>();
				//	generalResultList.Add(generalResultViewModel);
				//	response.ResponseData.GeneralAddResultViewModelList = generalResultList;
				//}

				//if (response.ResponseData.GeneralManualAddResultViewModelList == null
				//   || response.ResponseData.GeneralManualAddResultViewModelList.Count() == 0)
				//{
				//	GeneralManualResultViewModel generalManualResultViewModel = new GeneralManualResultViewModel()
				//	{
				//		Column1 = "0",
				//		Column2 = "0",
				//		Column3 = "0",
				//		Column4 = "0",
				//		Column5 = "0",
				//		Column6 = "0"
				//	};
				//	List<GeneralManualResultViewModel> generalManualResultList = new List<GeneralManualResultViewModel>();
				//	generalManualResultList.Add(generalManualResultViewModel);
				//	response.ResponseData.GeneralManualAddResultViewModelList = generalManualResultList;
				//}

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
			else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
			{
				response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
			}
		}
		else
		{
			ResponseViewModel<MetalRulesViewModel> responseempty = new ResponseViewModel<MetalRulesViewModel>();
			//ResponseViewModel<InstrumentViewModel> instrumentresponse1 = _instrumentService.GetInstrumentById(instrumentId);
			MetalRulesViewModel metalrule = new MetalRulesViewModel();
			metalrule.InstrumentId = instrumentId;
			metalrule.RequestId = requestId;

			if (instrumentresponse.ResponseData != null)
			{
				metalrule.Name = instrumentresponse.ResponseData.InstrumentName;
				metalrule.SerialNo = instrumentresponse.ResponseData.SlNo;
				metalrule.Range = instrumentresponse.ResponseData.Range;
				metalrule.IdNo = instrumentresponse.ResponseData.IdNo;
				metalrule.Make = instrumentresponse.ResponseData.Make;
				metalrule.RefStd = instrumentresponse.ResponseData.StandardReffered;
				metalrule.Grade = instrumentresponse.ResponseData.Grade;
				metalrule.MasterEqiupmentList = MasterEqiupmentList.ResponseDataList;
			}

			metalrule.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
			metalrule.CalibrationPerformedDate = DateTime.Now;
			metalrule.CalibrationReviewedDate = DateTime.Now;

			if (userRoleId == 4)
			{
				metalrule.ReviewedBy = string.Concat(firstName, " ", lastName);
				metalrule.CalibrationReviewedDate = DateTime.Now;
			}
			else
			{
				metalrule.ReviewedBy = string.Empty;
				metalrule.Review_Date = string.Empty;
			}


			MetalRuleResultViewModel MicroResultViewModel = new MetalRuleResultViewModel()
			{
				SNO = 1,
				MeasuedValue = "0",
				Actuals = "0",
				InstrumentError = "0",
				MasterView1 = 1
			};

			List<MetalRuleResultViewModel> MicroResultList = new List<MetalRuleResultViewModel>();
			MicroResultList.Add(MicroResultViewModel);
			metalrule.MetalRuleAddResultViewModelList1 = MicroResultList;

			List<MetalRuleResultViewModel> items = new List<MetalRuleResultViewModel>()
			{
				new MetalRuleResultViewModel{ SNO=1, MeasuedValue="0", Actuals = "0",InstrumentError = "0",MasterView1 = 1 }
			};

			metalrule.MetalRuleAddResultViewModelList2 = items;


			if (userRoleId == 1)
			{
				metalrule.IsDisabled = "disabled";
			}
			else
			{
				metalrule.IsDisabled = "";
			}
			responseempty.ResponseData = metalrule;

			return View(responseempty.ResponseData);
		}
		return View(response.ResponseData);
	}



	public IActionResult InsertMetalRule(MetalRulesViewModel metalrule)
	{
		//return View();
		ResponseViewModel<MetalRulesViewModel> response;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		metalrule.CreatedBy = userId;
		//if (ViewBag.ObservationTypeMicro == "Depth micrometer")//For Depth micrometer
		//{
		//    micrometer.Flatness1 = "1";
		//}
		response = _ObservationTemplateService.InsertMetalRule(metalrule);


		TempData["ResponseCode"] = response.ResponseCode;
		TempData["ResponseMessage"] = response.ResponseMessage;
		//return Json(response.ResponseData);

		if (response.ResponseMessage == "Success")
		{
			return RedirectToAction("Request", "Tracker", new { reqType = 4 });

		}

		else
		{
			return View(response.ResponseData);
		}

	}

	public IActionResult VernierCaliperDepth(int requestId, int instrumentId)
	{
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		string firstName = base.SessionGetString("FirstName");
		string lastName = base.SessionGetString("LastName");
		ResponseViewModel<VernierCaliperViewModel> response = _ObservationTemplateService.GetVernierCaliperById(requestId, instrumentId);
		if (response.ResponseData != null)
		{
			if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
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
			response.ResponseData.Grade = instrumentresponse.ResponseData.Grade;

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
			else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
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
	#region "Dynamic Observation"
	public IActionResult InternalObservation(int instrumentId, int requestId)
	{
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		string firstName = base.SessionGetString("FirstName");
		string lastName = base.SessionGetString("LastName");


		ResponseViewModel<DynamicViewModel> response = _ObservationTemplateService.GetObservationInstrumentById(instrumentId, requestId);
		if ((response.ResponseData.TemplateObservationId == 0) || (response.ResponseData.TemplateObservationId == null))
		 { 
		response.ResponseData.CalibrationPerformedBy = firstName + " " + lastName;
		response.ResponseData.CalibrationPerformedDate = DateTime.Now;
        response.ResponseData.CalibrationReviewedDate = DateTime.Now;
         
        }
		if (Convert.ToInt32(base.SessionGetString("UserRoleId")) == 4 )
		{
            response.ResponseData.CalibrationReviewedBy = firstName + " " + lastName;

        }
        return View(response.ResponseData);


    }
	public JsonResult GetObservationById(int InstrumentId, int RequestId)
	{
		ResponseViewModel<ObservationContentViewModel> response = _ObservationTemplateService.GetObservationById(InstrumentId,RequestId);
		return Json(response.ResponseDataList);

	}
	public JsonResult GetSelectedObservationContentById(int ContentId,int InstrumentId, int RequestId)
	{
		ResponseViewModel<ObservationContentViewModel> response = _ObservationTemplateService.GetSelectedObservationContentById(ContentId,InstrumentId,RequestId);
		return Json(response.ResponseDataList);

	}
	public IActionResult InsertDynamicObservationContent(DynamicViewModel Dynamic)
	{
		ResponseViewModel<DynamicViewModel> response;
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));

		Dynamic.CreatedBy = userId;

		response = _ObservationTemplateService.InsertObservation(Dynamic);

		return Json(response.ResponseData);
	}
	public IActionResult GetObservationContentValuesById(int InstrumentId, int RequestId)
	{
		ResponseViewModel<ObservationContentValuesViewModel> response = _ObservationTemplateService.GetObservationContentValuesById(InstrumentId, RequestId);

		return Json(response.ResponseDataList);
	}
	public JsonResult GetObservationContentSelectedList(List<Contentids> Contents)
	{
		ResponseViewModel<ObservationContentViewModel> response = _ObservationTemplateService.GetObservationContentSelectedList(Contents);

		return Json(response.ResponseDataList);

	}
	#endregion

	public IActionResult ExternalObs(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        string firstName = base.SessionGetString("FirstName");
        string lastName = base.SessionGetString("LastName");
        ResponseViewModel<ExternalObsViewModel> response = _ObservationTemplateService.GetExternalObsById(requestId, instrumentId);
        if (response.ResponseData != null)
        {
            //if (response.ResponseData.RefWi == null || response.ResponseData.RefWi == string.Empty)
            //response.ResponseData.RefWi = Constants.VERNIER_CALIPER_REFERENCE_WITH_INDICATOR;

            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            response.ResponseData.InstrumentId = instrumentId;
            response.ResponseData.RequestId = requestId;
            response.ResponseData.Name = instrumentresponse.ResponseData.InstrumentName;
            response.ResponseData.Range = instrumentresponse.ResponseData.Range;
            response.ResponseData.Make = instrumentresponse.ResponseData.Make;
            response.ResponseData.SerialNo = instrumentresponse.ResponseData.SlNo;
            response.ResponseData.IdNo = instrumentresponse.ResponseData.IdNo;
            response.ResponseData.RefStd = instrumentresponse.ResponseData.StandardReffered;
            //response.ResponseData.ObsSubType = instrumentresponse.ResponseData.ObservationType;
            //response.ResponseData.Grade = instrumentresponse.ResponseData.Grade;

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
            else if (userRoleId != 4 && response.ResponseData.ReviewStatus != null)
            {
                response.ResponseData.Review_Date = response.ResponseData.CalibrationReviewedDate.ToString();
            }

            List<Uploads> UploadList = _unitOfWork.Repository<Uploads>().GetQueryAsNoTracking(g => g.RequestId == requestId).ToList();
            if (UploadList.Count > 0)
            {
                // RequestById.MUTemplateFileName = UploadList.Where(w => w.RequestId == requestId).Select(q => q.FileName).Take(1).SingleOrDefault();
                response.ResponseData.SignImageName = UploadList.Where(w => w.RequestId == requestId && w.TemplateType == "EX-AP").Select(q => q.FileName).Take(1).SingleOrDefault();
            }

        }
        else
        {
            ResponseViewModel<ExternalObsViewModel> responseempty = new ResponseViewModel<ExternalObsViewModel>();
            ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
            ExternalObsViewModel extObs = new ExternalObsViewModel();
            extObs.InstrumentId = instrumentId;
            extObs.RequestId = requestId;
            extObs.Name = instrumentresponse.ResponseData.InstrumentName;
            extObs.SerialNo = instrumentresponse.ResponseData.SlNo;
            extObs.IdNo = instrumentresponse.ResponseData.IdNo;
            extObs.Range = instrumentresponse.ResponseData.Range;
            extObs.RefStd = instrumentresponse.ResponseData.StandardReffered;
            extObs.Make = instrumentresponse.ResponseData.Make;
            extObs.CalibrationPerformedBy = string.Concat(firstName, " ", lastName);
            extObs.CalibrationPerformedDate = DateTime.Now;
            extObs.CalibrationReviewedDate = DateTime.Now;
            //extObs.RefWi = Constants.THREAD_GAUGE_REFERENCE_WITH_INDICATOR;
            if (userRoleId == 4)
            {
                extObs.ReviewedBy = string.Concat(firstName, " ", lastName);
                extObs.CalibrationReviewedDate = DateTime.Now;
            }
            else
            {
                extObs.ReviewedBy = string.Empty;
                // extObs.Review_Date = string.Empty;
            }

            if (userRoleId == 1)
            {
                extObs.IsDisabled = "disabled";
            }
            else
            {
                extObs.IsDisabled = "";
            }

            //List<Uploads> UploadList = _unitOfWork.Repository<Uploads>().GetQueryAsNoTracking(g => g.RequestId == requestId).ToList();
            //if (UploadList.Count > 0)
            //{
            //   // RequestById.MUTemplateFileName = UploadList.Where(w => w.RequestId == requestId).Select(q => q.FileName).Take(1).SingleOrDefault();
            //    extObs.SignImageName = UploadList.Where(w => w.RequestId == requestId && w.TemplateType == "EX-AP").Select(q => q.FileName).Take(1).SingleOrDefault();
            //}

            responseempty.ResponseData = extObs;
            return View(responseempty.ResponseData);
        }
        return View(response.ResponseData);
    }

    public IActionResult InsertExternalObs(ExternalObsViewModel exObs)
    {
        //return Json(true);
        ResponseViewModel<ExternalObsViewModel> response;
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        exObs.CreatedBy = userId;

        response = _ObservationTemplateService.InsertExternalObs(exObs);


        //TempData["ResponseCode"] = response.ResponseCode;
        //TempData["ResponseMessage"] = response.ResponseMessage;

        return Json(response.ResponseData);

    }
}


