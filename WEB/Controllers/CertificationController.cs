using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using WEB.Models;
using WEB.Services.Interface;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.text.html.simpleparser;
using HtmlAgilityPack;
using iTextSharp.text.pdf.parser;

namespace WEB.Controllers;

public class CertificationController : BaseController
{
    private IRequestService _requestService { get; set; }
    private IObservationTemplateService _ObservationTemplateService { get; set; }
    private IInstrumentService _instrumentService { get; set; }
    private IConfiguration _configuration;

    private IQRCodeGeneratorService _qrCodeGeneratorService { get; set; }
    public CertificationController(IObservationTemplateService observationTemplateService, ILogger<BaseController> logger,
                                   IHttpContextAccessor contextAccessor, IRequestService requestService, IInstrumentService instrumentService,
                                   IQRCodeGeneratorService qrCodeGeneratorService,
                                   IConfiguration configuration)
                                   : base(logger, contextAccessor)
    {
        _ObservationTemplateService = observationTemplateService;
        _requestService = requestService;
        _instrumentService = instrumentService;
        _qrCodeGeneratorService = qrCodeGeneratorService;
        _configuration = configuration;
    }
    public IActionResult ViewCertification_old(int requestId, int instrumentId)
    {
        //ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetInstrumentById(instrumentId);
        int CertificationTemplateId = _instrumentService.GetObservationTemplateId(instrumentId, "Certification");
        string templateName = "Gentral";
        if (CertificationTemplateId != null)
        {
            if (CertificationTemplateId == 99)
            {
                templateName = "General";
            }
            else if (CertificationTemplateId == 156)
            {
                templateName = "GeneralNew";
            }
            else if (CertificationTemplateId == 88)
            {
                templateName = "LeverDial";
            }
            else if (CertificationTemplateId == 89)
            {
                templateName = "Micrometer";
            }
            else if (CertificationTemplateId == 98)
            {
                templateName = "PlungerDial";
            }
            else if (CertificationTemplateId == 100)
            {
                templateName = "ThreadGauges";
            }
            else if (CertificationTemplateId == 101)
            {
                templateName = "TWObs";
            }
            else if (CertificationTemplateId == 102)
            {
                templateName = "VernierCaliper";
            }
        }
        return RedirectToAction(templateName, new { requestId = requestId, instrumentId = instrumentId });
    }

    #region "Lever Dial Type"
    public IActionResult LeverDial(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<LeverTypeDialViewModel> response = _ObservationTemplateService.GetLeverDialById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.LeverDialData = response.ResponseData;
                instrumentresponse.ResponseData.LeverDialData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.LeverDialData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.LeverDialData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.LeverDialData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
            }
            else
            {
                instrumentresponse.ResponseData.LeverDialData = new LeverTypeDialViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
            ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
            if (requestResponse.ResponseData != null)
            {
                instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
            }
            if (requestResponse.ResponseData != null)
            {
                instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);

    }
    public IActionResult Micrometer(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<MicrometerViewModel> response = _ObservationTemplateService.GetMicrometerById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.MicrometerData = response.ResponseData;
                decimal flatness1 = Convert.ToDecimal(response.ResponseData.Flatness1);
                decimal flatness2 = Convert.ToDecimal(response.ResponseData.Flatness2);
                decimal flatness = flatness1 + flatness2;
                instrumentresponse.ResponseData.MicrometerData.Flatness1 = flatness.ToString();
                instrumentresponse.ResponseData.MicrometerData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.MicrometerData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.MicrometerData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.MicrometerData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
            }
            else
            {
                instrumentresponse.ResponseData.MicrometerData = new MicrometerViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
            ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
            if (requestResponse.ResponseData != null)
            {
                instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
            }
            if (requestResponse.ResponseData != null)
            {
                instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }

    public IActionResult General(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<GeneralViewModel> response = _ObservationTemplateService.GetGeneralById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.GeneralData = response.ResponseData;
                instrumentresponse.ResponseData.GeneralData.GeneralAddResultViewModelList = response.ResponseData.GeneralAddResultViewModelList;
                instrumentresponse.ResponseData.GeneralData.GeneralManualAddResultViewModelList = response.ResponseData.GeneralManualAddResultViewModelList;

                instrumentresponse.ResponseData.GeneralData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.GeneralData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.GeneralData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.GeneralData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
            }
            else
            {
                instrumentresponse.ResponseData.isExportCertificate = false;
                instrumentresponse.ResponseData.GeneralData = new GeneralViewModel();
                instrumentresponse.ResponseData.GeneralData.GeneralAddResultViewModelList = new List<GeneralResultViewModel>();
                instrumentresponse.ResponseData.GeneralData.GeneralManualAddResultViewModelList = new List<GeneralManualResultViewModel>();
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }


    public IActionResult PlungerDial(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<PlungerDialViewModel> response = _ObservationTemplateService.GetPlungerDialById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.PlungerDialData = response.ResponseData;
                instrumentresponse.ResponseData.PlungerDialData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.PlungerDialData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.PlungerDialData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.PlungerDialData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
            }
            else
            {
                instrumentresponse.ResponseData.PlungerDialData = new PlungerDialViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }
    public IActionResult ThreadGauges(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<ThreadGaugesViewModel> response = _ObservationTemplateService.GetThreadGaugesById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.ThreadGaugeData = response.ResponseData;
                instrumentresponse.ResponseData.ThreadGaugeData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.ThreadGaugeData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.ThreadGaugeData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.ThreadGaugeData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
            }
            else
            {
                instrumentresponse.ResponseData.ThreadGaugeData = new ThreadGaugesViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }
    public IActionResult TWObs(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<TorqueWrenchesViewModel> response = _ObservationTemplateService.GetTorqueWrenchesById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.TorqueData = response.ResponseData;
                instrumentresponse.ResponseData.TorqueData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.TorqueData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.TorqueData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.TorqueData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
            }
            else
            {
                instrumentresponse.ResponseData.TorqueData = new TorqueWrenchesViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }
    public IActionResult VernierCaliper(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
                                                                                   instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<VernierCaliperViewModel> response = _ObservationTemplateService.GetVernierCaliperById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.VernierData = response.ResponseData;
                instrumentresponse.ResponseData.VernierData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.VernierData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.VernierData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.VernierData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
            }
            else
            {
                instrumentresponse.ResponseData.VernierData = new VernierCaliperViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }

    public IActionResult GeneralNew(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            // instrumentresponse.ResponseData.RuleConfirmityStatement = string.Format(Constants.DECISION_RULE_CONFIRMITY,
            //                                                                        instrumentresponse.ResponseData.Rule_Confirmity);
            ResponseViewModel<GeneralNewViewModel> response = _ObservationTemplateService.GetGeneralNewById(requestId, instrumentId);
            if (response.ResponseData != null)
            {
                instrumentresponse.ResponseData.GeneralNewData = response.ResponseData;
                instrumentresponse.ResponseData.GeneralNewData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.GeneralNewData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                instrumentresponse.ResponseData.GeneralNewData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;

                if (string.IsNullOrEmpty(instrumentresponse.ResponseData.GeneralNewData.CalibrationResult))
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
            }
            else
            {
                instrumentresponse.ResponseData.GeneralNewData = new GeneralNewViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }

    public IActionResult SaveLeverDialCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity,
        string CalibrationResult, string Remarks, string ExportData, string TempltateName)

    {

        ExportData = ExportData.Replace(Constants.PDF_CERTIFICATE_RESULTS, CalibrationResult);
        ExportData = ExportData.Replace(Constants.PDF_CERTIFICATE_REMARKS, Remarks);
        ExportData = ExportData.Replace(Constants.PDF_CERTIFICATE_UNCERTAINTY, Uncertainity);

        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        if (TempltateName == "LeverDial")
        {
            ResponseViewModel<LeverTypeDialViewModel> response = new ResponseViewModel<LeverTypeDialViewModel>();
            response = _ObservationTemplateService.SaveLeverDialCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "Micrometer")
        {
            ResponseViewModel<MicrometerViewModel> response = new ResponseViewModel<MicrometerViewModel>();
            response = _ObservationTemplateService.SaveMicrometerCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "PlungerDail")
        {
            ResponseViewModel<PlungerDialViewModel> response = new ResponseViewModel<PlungerDialViewModel>();
            response = _ObservationTemplateService.SavePlungerDialCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "ThreadGauges")
        {
            ResponseViewModel<ThreadGaugesViewModel> response = new ResponseViewModel<ThreadGaugesViewModel>();
            response = _ObservationTemplateService.SaveThreadGaugesCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "TWObs")
        {
            ResponseViewModel<TorqueWrenchesViewModel> response = new ResponseViewModel<TorqueWrenchesViewModel>();
            response = _ObservationTemplateService.SaveTorqueWrenchesCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "VernierCaliper")
        {
            ResponseViewModel<VernierCaliperViewModel> response = new ResponseViewModel<VernierCaliperViewModel>();
            response = _ObservationTemplateService.SaveVernierCaliperCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "GeneralNew")
        {
            ResponseViewModel<GeneralNewViewModel> response = new ResponseViewModel<GeneralNewViewModel>();
            response = _ObservationTemplateService.SaveGeneralNewCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }
        if (TempltateName == "General")
        {
            ResponseViewModel<GeneralViewModel> response = new ResponseViewModel<GeneralViewModel>();
            response = _ObservationTemplateService.SaveGeneralCertificate(requestId, instrumentId,
            EnvironmentCondition, Uncertainity, CalibrationResult, Remarks, userId, ExportData);
            return Json(response);
        }

        return View();
    }

    public IActionResult SaveAsPdf(string ExportData)
    {
        HtmlNode.ElementsFlags["img"] = HtmlElementFlag.Closed;
        HtmlNode.ElementsFlags["input"] = HtmlElementFlag.Closed;
        HtmlDocument doc = new HtmlDocument();
        doc.OptionFixNestedTags = true;
        doc.LoadHtml(ExportData);
        ExportData = doc.DocumentNode.OuterHtml;

        using (MemoryStream sourceStream = new System.IO.MemoryStream())
        {

            StringReader reader = new StringReader(ExportData);


            Document PdfFile = new Document(PageSize.A4, 20, 20, 30, 30);
            PdfWriter writer = PdfWriter.GetInstance(PdfFile, sourceStream);

            PdfFile.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);

            PdfFile.Close();


            //System.IO.FileStream fs = new FileStream(Server.MapPath("PDFDIR") + "\\" + otd.NumeroOTD.ToString() + ".pdf", FileMode.Create);



            // StringReader stringReader = new StringReader(ExportData);         
            //     Document PDFdoc = new Document(PageSize.A4, 10, 10, 10, 20);
            //     HTMLWorker htmlparser =   new HTMLWorker(PDFdoc);
            //     PdfWriter.GetInstance(PDFdoc, sourceStream);

            //     PDFdoc.Open();
            //     htmlparser.Parse(stringReader);
            //     //XMLWorkerHelper.GetInstance().ParseXHtml(writer, PdfFile, reader);
            //     PDFdoc.Close();
            //return File(Fs, "application/pdf", sTemp);
            return File(sourceStream.ToArray(), "application/pdf", "ExportData.pdf");

        }
    }

    #endregion


    public IActionResult GenerateQRCode(string templateName, int requestId, int instrumentId)
    {
        return RedirectToAction(templateName, new { requestId = requestId, instrumentId = instrumentId });
    }

    public IActionResult QRCodePrint(string requestId, string instrumentId)
    {
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(int.Parse(requestId), int.Parse(instrumentId));
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);

        return View();
    }

    public IActionResult ViewPdfFiles(string guid)
    {
        //var loggedId = base.SessionGetString("LoggedId");
        // if (loggedId != null)
        // {
        QRCodeFilesViewModel qrCodeExistingData = _qrCodeGeneratorService.GetQRCodeByGuid(Guid.Parse(guid));

        if (qrCodeExistingData != null)
        {
            string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", guid);
            applicationUrl = string.Concat(applicationUrl, "/", qrCodeExistingData.FileName);

            qrCodeExistingData.FileName = applicationUrl;
            return View(qrCodeExistingData);
        }
        else
        {
            return Redirect("/Home/Login");
        }
        //}
        // else
        // {
        // string currentURL = HttpUtility.UrlEncode(HttpContext.Request.GetEncodedUrl());
        // string url = string.Format("/Account/Login?ReturnUrl={0}", currentURL);
        // return Redirect(url);
        //}
    }
	
	private QRCodeFilesViewModel GetQRCodeImage(int requestId, int instrumentId)
    {
        RequestViewModel requestData = _qrCodeGeneratorService.GetRequestData(requestId);
        if (requestData == null)
        {
            QRCodeFilesViewModel emptyQRCodeData = new QRCodeFilesViewModel();
            return emptyQRCodeData;
        }

        QRCodeFilesViewModel qrCodeGenInputViewModel = new QRCodeFilesViewModel()
        {
            TemplateName = Constants.CONTROLLERNAME,
            RequestId = requestId,
            InstrumentId = instrumentId,
            RequestNo = requestData.ReqestNo
        };

        QRCodeFilesViewModel qrCodeGenOutputViewModel = _qrCodeGeneratorService.QRCodeGeneration(qrCodeGenInputViewModel);


        if (qrCodeGenOutputViewModel == null)
            return qrCodeGenInputViewModel;
        else
        {
            using (FileStream stream = new FileStream(_configuration["TempQRCodePath"], FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                stream.Write(qrCodeGenOutputViewModel.DecodeText, 0, qrCodeGenOutputViewModel.DecodeText.Length);
            }
        }
        //System.IO.File.WriteAllBytes(_configuration["TempQRCodePath"],qrCodeGenOutputViewModel.DecodeText);

        return qrCodeGenOutputViewModel;
    }

    public IActionResult ViewCertification(int requestId, int instrumentId)
    {
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
        int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
        ResponseViewModel<InstrumentViewModel> instrumentresponse = _instrumentService.GetInstrumentById(instrumentId);
        if (instrumentresponse.ResponseData != null)
        {
            instrumentresponse.ResponseData.Range = string.Concat(instrumentresponse.ResponseData.Range, instrumentresponse.ResponseData.Unit1);
            instrumentresponse.ResponseData.LC = string.Concat(instrumentresponse.ResponseData.LC, instrumentresponse.ResponseData.Unit2);
            instrumentresponse.ResponseData.RuleConfirmityStatement = "";// instrumentresponse.ResponseData.Rule_Confirmity == null ? string.Empty:string.Format(Constants.DECISION_RULE_CONFIRMITY,                                                                                  instrumentresponse.ResponseData.Rule_Confirmity);//  instrumentresponse.ResponseData.Rule_Confirmity == null ? string.Empty 
            instrumentresponse.ResponseData.RequestId = requestId;

            DateTime calibrationClosedate = _instrumentService.GetcalibrationClosedate(requestId);
            instrumentresponse.ResponseData.CalibrationCloseDate = calibrationClosedate;
            //TemplateObservation observationById = _unitOfWork.Repository<TemplateObservation>()
            //                                             .GetQueryAsNoTracking(Q => Q.RequestId == levertypedial.RequestId
            //                                                                        && Q.InstrumentId == levertypedial.InstrumentId)
            //                                             .SingleOrDefault();
            
			ResponseViewModel<CertificateViewModel> response = _ObservationTemplateService.GetTemplateObservationById(requestId, instrumentId);

            if (response.ResponseData != null)
            {
                int TemplateObservationId = 0;
				instrumentresponse.ResponseData.TempObservationData = response.ResponseData;
                instrumentresponse.ResponseData.TempObservationData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
                instrumentresponse.ResponseData.TempObservationData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
                if (response !=null)
                {
                     TemplateObservationId = response.ResponseData.Id;

				}
				ResponseViewModel<ObservationContentViewModel> responseObs = _ObservationTemplateService.GetObservationById(instrumentId, requestId,TemplateObservationId);
                if (responseObs != null)
                {
                    instrumentresponse.ResponseData.obsContent = responseObs.ResponseDataList;
                }

                QRCodeFilesViewModel existingData = _qrCodeGeneratorService.GetQRCodeDetailsForCertificate(requestId, instrumentId);

                if (existingData != null)
                {
                    instrumentresponse.ResponseData.isExportCertificate = true;
                }
                else
                {
                    instrumentresponse.ResponseData.isExportCertificate = false;
                }                

                ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
                }
                if (requestResponse.ResponseData != null)
                {
                    instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
                }
                //instrumentresponse.ResponseData.GeneralData = response.ResponseData.InstrumentCondition;
            }
            else
            {
                instrumentresponse.ResponseData.GeneralNewData = new GeneralNewViewModel();
                instrumentresponse.ResponseData.isExportCertificate = false;
            }
            //if (response.ResponseData != null)
            //{
            //    instrumentresponse.ResponseData.PlungerDialData = response.ResponseData;
            //    instrumentresponse.ResponseData.PlungerDialData.PdfCalibrationResult = Constants.PDF_CERTIFICATE_RESULTS;
            //    instrumentresponse.ResponseData.PlungerDialData.PdfRemarks = Constants.PDF_CERTIFICATE_REMARKS;
            //    instrumentresponse.ResponseData.PlungerDialData.PdfUncertainity = Constants.PDF_CERTIFICATE_UNCERTAINTY;
            //    if (string.IsNullOrEmpty(instrumentresponse.ResponseData.PlungerDialData.CalibrationResult))
            //    {
            //        instrumentresponse.ResponseData.isExportCertificate = false;
            //    }
            //    else
            //    {
            //        instrumentresponse.ResponseData.isExportCertificate = true;
            //    }
            //    ResponseViewModel<RequestViewModel> requestResponse = _requestService.GetRequestById(requestId);
            //    if (requestResponse.ResponseData != null)
            //    {
            //        instrumentresponse.ResponseData.DateOfReceipt = requestResponse.ResponseData.ReceivedDate;
            //    }
            //    if (requestResponse.ResponseData != null)
            //    {
            //        instrumentresponse.ResponseData.Result = requestResponse.ResponseData.LabResult;
            //    }
            //}
            //else
            //{
            //    instrumentresponse.ResponseData.PlungerDialData = new PlungerDialViewModel();
            //    instrumentresponse.ResponseData.isExportCertificate = false;
            //}
        }
        ViewBag.RequestId = requestId;
        ViewBag.InstrumentId = instrumentId;
        QRCodeFilesViewModel qrCodeFilesViewModel = GetQRCodeImage(requestId, instrumentId);
        ViewBag.QRCodeImage = qrCodeFilesViewModel.QRImageUrl;
        ViewBag.UrlGuid = qrCodeFilesViewModel.UrlGuid;
        ViewBag.QRDecodeText = "data:image/png;base64," +
                                Convert.ToBase64String(qrCodeFilesViewModel.DecodeText, 0, qrCodeFilesViewModel.DecodeText.Length);
        string applicationUrl = string.Concat(_configuration["AppUrl"], "/", Constants.FOLDERNAME, "/", qrCodeFilesViewModel.UrlGuid);
        ViewBag.FileName = string.Concat(applicationUrl, "/", qrCodeFilesViewModel.FileName);
        return View(instrumentresponse.ResponseData);
    }

    public IActionResult SaveCertificateTemp(int requestId, int instrumentId, string EnvironmentCondition,  string ExportData, string TempltateName)
    {
        
        //ExportData = ExportData.Replace(Constants.PDF_CERTIFICATE_RESULTS, CalibrationResult);
        //ExportData = ExportData.Replace(Constants.PDF_CERTIFICATE_REMARKS, Remarks);
        //return Json(true);
        int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));        
        ResponseViewModel<CertificateViewModel> response = new ResponseViewModel<CertificateViewModel>();
        response = _ObservationTemplateService.SaveCertificateTemp(requestId, instrumentId,EnvironmentCondition, userId, ExportData);
        return Json(response);
    }    
}


