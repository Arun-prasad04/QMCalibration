using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using System.Net;
using System.Net.Mail;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace WEB.Services;


public class InstrumentService : IInstrumentService
{
  private readonly IMapper _mapper;

  private IHttpContextAccessor _contextAccessor { get; set; }
  private IUnitOfWork _unitOfWork { get; set; }
  private IEmailService _emailService;
  private IConfiguration _configuration;
  private IUtilityService _utilityService;
  public InstrumentService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService, IHttpContextAccessor contextAccessor, IEmailService emailService, IConfiguration Configuration)
  {
    _unitOfWork = unitOfWork;
    _mapper = mapper;
    _utilityService = utilityService;
    _contextAccessor = contextAccessor;
    _emailService = emailService;
    _configuration = Configuration;

  }

  public ResponseViewModel<InstrumentViewModel> GetAllInstrumentList(int userId, int userRoleId)
  {
    try
    {
      UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
      List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
      //if (userRoleId == 2 || userRoleId == 4)
        if (userRoleId == 2 || labUserById.DepartmentId == 66)
      {

         instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && (Q.IdNo != "" && Q.IdNo != null)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
        {
          Id = s.Id,
          InstrumentName = s.InstrumentName,
          SlNo = s.SlNo,
          IdNo = s.IdNo,
          Range = s.Range,
          LC = s.LC,
          CalibFreq = s.CalibFreq,
          CalibDate = s.CalibDate,
          DueDate = s.DueDate,
          Make = s.Make,
          CalibSource = s.CalibSource,
          StandardReffered = s.StandardReffered,
          Remarks = s.Remarks,
          Status = s.Status,
          FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
          CalibrationStatus = s.CalibrationStatus,
          InstrumentStatus = s.InstrumentStatus,
          DateOfReceipt = s.DateOfReceipt,
          DepartmentName = s.DepartmenttModel.Name,
          NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault(),
          RequestStatus = s.RequestModel.Where(x => x.InstrumentId == s.Id).OrderByDescending(U => U.Id).Select(D => D.StatusId).FirstOrDefault(),
		  Grade=s.Grade
		 }
        ).ToList();
      }
      else if (userRoleId == 1)
      {
         
        instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && (Q.IdNo != "" && Q.IdNo != null) && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
        {
          Id = s.Id,
          InstrumentName = s.InstrumentName,
          SlNo = s.SlNo,
          IdNo = s.IdNo,
          Range = s.Range,
          LC = s.LC,
          CalibFreq = s.CalibFreq,
          CalibDate = s.CalibDate,
          DueDate = s.DueDate,
          Make = s.Make,
          CalibSource = s.CalibSource,
          StandardReffered = s.StandardReffered,
          Remarks = s.Remarks,
          Status = s.Status,
          FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
          CalibrationStatus = s.CalibrationStatus,
          InstrumentStatus = s.InstrumentStatus,
          DateOfReceipt = s.DateOfReceipt,
          DepartmentName = s.DepartmenttModel.Name,
          NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault(),
          RequestStatus = s.RequestModel.Where(x => x.InstrumentId == s.Id).OrderByDescending(U => U.Id).Select(D => D.StatusId).FirstOrDefault(),
        }
        ).ToList();

      }
            else if (userRoleId == 4)
            {
                instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
                {
                    Id = s.Id,
                    InstrumentName = s.InstrumentName,
                    SlNo = s.SlNo,
                    IdNo = s.IdNo,
                    Range = s.Range,
                    LC = s.LC,
                    CalibFreq = s.CalibFreq,
                    CalibDate = s.CalibDate,
                    DueDate = s.DueDate,
                    Make = s.Make,
                    CalibSource = s.CalibSource,
                    StandardReffered = s.StandardReffered,
                    Remarks = s.Remarks,
                    Status = s.Status,
                    FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
                    CalibrationStatus = s.CalibrationStatus,
                    InstrumentStatus = s.InstrumentStatus,
                    DateOfReceipt = s.DateOfReceipt,
                    DepartmentName = s.DepartmenttModel.Name,
                    NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault(),
                    RequestStatus = s.RequestModel.Where(x => x.InstrumentId == s.Id).OrderByDescending(U => U.Id).Select(D => D.StatusId).FirstOrDefault(),
                }
                ).ToList();

            }

            return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = instrumentList
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseServiceMethod = "Instrument",
        ResponseService = "GetAllInstrumentList"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> GetInstrumentById(int instrumentId)
  {
    try
    {
      var DepartmentData = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());


      InstrumentViewModel instrumentById = _unitOfWork.Repository<Instrument>()
      .GetQueryAsNoTracking(Q => Q.QuarantineModel
      .Select(s => s.InstrumentId)
      .SingleOrDefault()== instrumentId)
      .Include(I => I.QuarantineModel)
      .Include(I => I.FileUploadModel)
      .Include(I => I.UserModel)
      .Include(I => I.DepartmenttModel)
      .Select(s => new InstrumentViewModel()
      {
        Id = s.Id,
        InstrumentName = s.InstrumentName,
        SlNo = s.SlNo,
        IdNo = s.IdNo,
        Range = s.Range,
        LC = s.LC,
        CalibFreq = s.CalibFreq,
        CalibDate = s.CalibDate,
        DueDate = s.DueDate,
        Make = s.Make,
        CalibSource = s.CalibSource,
        StandardReffered = s.StandardReffered,
        Remarks = s.Remarks,
        Status = s.Status,
        FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
        CalibrationStatus = s.CalibrationStatus,
        InstrumentStatus = s.InstrumentStatus,
        DateOfReceipt = s.DateOfReceipt,
        CertificationTemplate = s.CertificationTemplate,
        ObservationTemplate = s.ObservationTemplate,
        MasterInstrument1 = s.MasterInstrument1,
        MasterInstrument2 = s.MasterInstrument2,
        MasterInstrument3 = s.MasterInstrument3,
        MasterInstrument4 = s.MasterInstrument4,
        CustomerName = s.UserModel.FirstName + " " + s.UserModel.LastName,
        DepartmentName = s.DepartmenttModel.Name,
        ObservationType = s.ObservationType,
	   MUTemplate=s.MUTemplate,
		Unit1 = s.Unit1,
        Unit2 = s.Unit2,
        Unit3 = s.Unit3,
        UserDept = s.UserDept,
        TW_Type = s.TW_Type,
        Instrument_Type = s.Instrument_Type,
        Drawing_Attached = s.Drawing_Attached,
        Rule_Confirmity = s.Rule_Confirmity,
        StandardReffered1 = s.StandardReffered1,
        IsNABL = s.IsNABL == null ? false : s.IsNABL,
        Grade=s.Grade
      }
          ).SingleOrDefault();
      List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>()
                                                                          //.GetQueryAsNoTracking(Q => Q.Attrform == "Instrument").ToList());
                                                                          .GetQueryAsNoTracking().ToList());
      if (instrumentById != null)
      {        
        instrumentById.InstrumentStatusList = lovsList.Where(W => W.AttrName == "InstrumentStatus").ToList();
        instrumentById.StatusList = lovsList.Where(W => W.AttrName == "Status").ToList();
        instrumentById.TemplateNameList = lovsList.Where(W => W.AttrName == "TemplateName").ToList();
        instrumentById.CalibFreqList = lovsList.Where(W => W.AttrName == "CalibrationFreq").ToList();
        instrumentById.CalibrationStatusList = lovsList.Where(W => W.AttrName == "CalibrationStatus").ToList();
        instrumentById.ObservationTemplateList = lovsList.Where(W => W.AttrName == "ObservationTemplate").ToList();
        instrumentById.MUTemplateList = lovsList.Where(W => W.AttrName == "MUTemplate").ToList();
        instrumentById.CertificationTemplateList = lovsList.Where(W => W.AttrName == "CerTemplate").ToList();
        instrumentById.MasterEqiupmentList = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == instrumentById.MasterInstrument1 || Q.Id == instrumentById.MasterInstrument2 || Q.Id == instrumentById.MasterInstrument3 || Q.Id == instrumentById.MasterInstrument4).ToList());
        instrumentById.MasterData = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking().ToList());
        instrumentById.Departments = DepartmentData;

      }
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = instrumentById,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "GetInstrumentByID"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> InsertInstrument(InstrumentViewModel instrument)
  {
    try
    {
      _unitOfWork.BeginTransaction();
      instrument.IsQuarantine = false;
      Instrument instrumentdata = _mapper.Map<Instrument>(instrument);
      _unitOfWork.Repository<Instrument>().Insert(instrumentdata);
      _unitOfWork.SaveChanges();

      Request newRequest = new Request();
      Request getMaxId = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id > 0).OrderByDescending(O => O.Id).FirstOrDefault();
      long maxId = 1;
      if (getMaxId != null)
      {
        maxId = getMaxId.Id + 1;
      }
      string requestNumberFormat = maxId.ToString().PadLeft(4, '0');
      newRequest.ReqestNo = "CR" + DateTime.Now.Year + requestNumberFormat;
      newRequest.InstrumentId = instrumentdata.Id;
      newRequest.RequestDate = DateTime.Now;
      newRequest.TypeOfReqest = 1;
      newRequest.StatusId = (Int32)EnumRequestStatus.Requested;
      newRequest.CreatedOn = DateTime.Now;
      newRequest.CreatedBy = instrument.CreatedBy;
      _unitOfWork.Repository<Request>().Insert(newRequest);
      _unitOfWork.SaveChanges();

      RequestStatus ReqestStatus = new RequestStatus();
      ReqestStatus.RequestId = newRequest.Id;
      ReqestStatus.StatusId = (Int32)EnumRequestStatus.Requested;
      ReqestStatus.CreatedOn = DateTime.Now;
      ReqestStatus.CreatedBy = instrument.CreatedBy;
      _unitOfWork.Repository<RequestStatus>().Insert(ReqestStatus);
      _unitOfWork.SaveChanges();

      InstrumentQuarantine instrumentQuarantine = new InstrumentQuarantine()
      {
        InstrumentId = instrumentdata.Id,
        Reason = "",
        UserId = instrumentdata.CreatedBy,
        CreatedOn = DateTime.Now,
        StatusId = 2
      };
      _unitOfWork.Repository<InstrumentQuarantine>().Insert(instrumentQuarantine);
      _unitOfWork.SaveChanges();
      if (instrument.ImageUpload != null && instrument.ImageUpload.Count > 0)
      {
        foreach (IFormFile fileObj in instrument.ImageUpload)
        {
          string filePath = _utilityService.UploadImage(fileObj, "Instrument");
          Uploads upload = new Uploads()
          {
            FileName = fileObj.FileName,
            FileGuid = Guid.NewGuid(),
            CreatedOn = DateTime.Now,
            ModifiedOn = DateTime.Now,
            FilePath = filePath
          };
          _unitOfWork.Repository<Uploads>().Insert(upload);
          _unitOfWork.SaveChanges();
          InstrumentFileUpload instrumentFileUpload = new InstrumentFileUpload();
          instrumentFileUpload.InstrumentId = instrumentdata.Id;
          instrumentFileUpload.UploadId = upload.Id;
          _unitOfWork.Repository<InstrumentFileUpload>().Insert(instrumentFileUpload);
          _unitOfWork.SaveChanges();
        }
      }
      _unitOfWork.Commit();
      string UserId = _contextAccessor.HttpContext.Session.GetString("LoggedId");
      UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
      List<UserViewModel> fmUserById = _mapper.Map<List<UserViewModel>>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2 && Q.Level != "L4").ToList());
      List<string> emailList = new List<string>();
      foreach (var item in fmUserById)
      {
        emailList.Add(item.Email.Trim());
        string RequestType = string.Empty;
        if (newRequest.TypeOfReqest == 1)
        {
          RequestType = "New";
        }
        else if (newRequest.TypeOfReqest == 2)
        {
          RequestType = "Regular";
        }
        else if (newRequest.TypeOfReqest == 3)
        {
          RequestType = "Recalibration";
        }
        string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Created by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
        mailbody = mailbody.Replace("$NAME$", item.FirstName + " " + item.LastName).Replace("$USERNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$", newRequest.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentdata.InstrumentName).Replace("$INSTRUMENTID$", instrumentdata.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(newRequest.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName);

        MailMessage message = new MailMessage();
        SmtpClient smtp = new SmtpClient();
        SmtpSettings smtpvalue = new SmtpSettings();
        message.From = new MailAddress(smtpvalue.FromAddress);
        message.To.Add(new MailAddress(item.Email.Trim()));
        //message.To.Add("gurushev.p@daimlertruck.com");
        //message.Bcc.Add("mohammedashik.s@intelizign.com");  
        message.Subject = "New Instrument Calibration Request- " + newRequest.ReqestNo + "";
        message.IsBodyHtml = true; //to make message body as html  
        message.Body = mailbody;
        smtp.Port = int.Parse(smtpvalue.Port);
        smtp.Host = smtpvalue.Server; //for gmail host  
        smtp.EnableSsl = false;
        smtp.UseDefaultCredentials = false;
        smtp.Credentials = new NetworkCredential(smtpvalue.UserId, smtpvalue.Pwd);
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //smtp.Send(message);


      }
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "InsertInstrument"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> UpdateInstrument(InstrumentViewModel instrument)
  {

    try
    {
      _unitOfWork.BeginTransaction();
      Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrument.Id).SingleOrDefault();
      if (instrument.InstrumentName != null)
      {
        instrumentById.InstrumentName = instrument.InstrumentName;
      }
      if (instrument.SlNo != null)
      {
        instrumentById.SlNo = instrument.SlNo;
      }
      if (instrument.IdNo != null)
      {
        instrumentById.IdNo = instrument.IdNo;
      }
      if (instrument.Range != null)
      {
        instrumentById.Range = instrument.Range;
      }
      if (instrument.InstrumentStatus != null)
      {
        instrumentById.InstrumentStatus = instrument.InstrumentStatus;
      }
      if (instrument.LC != null)
      {
        instrumentById.LC = instrument.LC;
      }
      if (instrument.CalibFreq != null)
      {
        instrumentById.CalibFreq = instrument.CalibFreq;
      }
      if (instrument.UserRoleId == 2)
      {
        if (instrument.UserDept != null)
        {
          instrumentById.UserDept = instrument.UserDept;
        }
      }
      if (instrument.Make != null)
      {
        instrumentById.Make = instrument.Make;
      }
      if (instrument.CalibSource != null)
      {
        instrumentById.CalibSource = instrument.CalibSource;
      }
      if (instrument.StandardReffered != null)
      {
        instrumentById.StandardReffered = instrument.StandardReffered;
      }
      if (instrument.Remarks != null)
      {
        instrumentById.Remarks = instrument.Remarks;
      }
      if (instrument.DueDate != null)
      {
        instrumentById.DueDate = instrument.DueDate;
      }
      if (instrument.CalibDate != null)
      {
        instrumentById.CalibDate = instrument.CalibDate;
      }
      if (instrument.DateOfReceipt != null)
      {
        instrumentById.DateOfReceipt = instrument.DateOfReceipt;
      }
      if (instrument.ObservationTemplate != null && instrument.ObservationTemplate > 0)
      {
        instrumentById.ObservationTemplate = instrument.ObservationTemplate;
      }
      if (instrument.ObservationType != null && instrument.ObservationType > 0)
      {
        instrumentById.ObservationType = instrument.ObservationType;
      }
      if (instrument.MUTemplate != null && instrument.MUTemplate > 0)
      {
        instrumentById.MUTemplate = instrument.MUTemplate;
      }

      if (instrument.MasterInstrument1 != null && instrument.MasterInstrument1 > 0)
      {
        instrumentById.MasterInstrument1 = instrument.MasterInstrument1;
      }
      else
      {
        instrumentById.MasterInstrument1 = 0;
      }

      if (instrument.MasterInstrument2 != null && instrument.MasterInstrument2 > 0)
      {
        instrumentById.MasterInstrument2 = instrument.MasterInstrument2;
      }
      else
      {
        instrumentById.MasterInstrument2 = 0;
      }

      if (instrument.MasterInstrument3 != null && instrument.MasterInstrument3 > 0)
      {
        instrumentById.MasterInstrument3 = instrument.MasterInstrument3;
      }
      else
      {
        instrumentById.MasterInstrument3 = 0;
      }

      if (instrument.MasterInstrument4 != null && instrument.MasterInstrument4 > 0)
      {
        instrumentById.MasterInstrument4 = instrument.MasterInstrument4;
      }
      else
      {
        instrumentById.MasterInstrument4 = 0;
      }

      instrumentById.IsNABL = instrument.IsNABL;
            //Newly Added 
			if (instrument.CertificationTemplate != null && instrument.CertificationTemplate > 0)
			{
				instrumentById.CertificationTemplate = instrument.CertificationTemplate;
			}
			if (instrument.CalibrationStatus != null && instrument.CalibrationStatus > 0)
			{
				instrumentById.CalibrationStatus = instrument.CalibrationStatus;
			}
			if (instrument.Rule_Confirmity != null)
			{
				instrumentById.Rule_Confirmity = instrument.Rule_Confirmity;
			}
			if (instrument.Drawing_Attached != null)
			{
				instrumentById.Drawing_Attached = instrument.Drawing_Attached;
			}
			if (instrument.Instrument_Type != null && instrument.Instrument_Type > 0)
			{
				instrumentById.Instrument_Type = instrument.Instrument_Type;
			}
			

			_unitOfWork.Repository<Instrument>().Update(instrumentById);
      _unitOfWork.SaveChanges();
      _unitOfWork.Commit();
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "UpdateInstrument"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> DeleteInstrument(int instrumentId)
  {
    try
    {
      Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrumentId).SingleOrDefault();
      _unitOfWork.BeginTransaction();
      _unitOfWork.Repository<Instrument>().Delete(instrumentById);
      _unitOfWork.SaveChanges();
      _unitOfWork.Commit();
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      _unitOfWork.RollBack();
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "DeleteInstrument"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> CreateNewInstrument()
  {
    try
    {
      var DepartmentData = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());


      InstrumentViewModel instrumentEmptyViewModel = new InstrumentViewModel();
      List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Instrument").ToList());
      List<LovsViewModel> lovsListFrquency = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Master").ToList());
      instrumentEmptyViewModel.InstrumentStatusList = lovsList.Where(W => W.AttrName == "InstrumentStatus").ToList();
      instrumentEmptyViewModel.StatusList = lovsList.Where(W => W.AttrName == "Status").ToList();
      instrumentEmptyViewModel.TemplateNameList = lovsList.Where(W => W.AttrName == "TemplateName").ToList();
      instrumentEmptyViewModel.CalibFreqList = lovsListFrquency.Where(W => W.AttrName == "CalibrationFreq").ToList();
      instrumentEmptyViewModel.CalibrationStatusList = lovsList.Where(W => W.AttrName == "CalibrationStatus").ToList();
      instrumentEmptyViewModel.ObservationTemplateList = lovsList.Where(W => W.AttrName == "ObservationTemplate").ToList();
      instrumentEmptyViewModel.MUTemplateList = lovsList.Where(W => W.AttrName == "MUTemplate").ToList();
      instrumentEmptyViewModel.CertificationTemplateList = lovsList.Where(W => W.AttrName == "CerTemplate").ToList();
      instrumentEmptyViewModel.MasterData = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking().ToList());
      instrumentEmptyViewModel.Departments = DepartmentData;
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = instrumentEmptyViewModel,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "CreateNewInstrument"
      };
    }
  }

  public ResponseViewModel<InstrumentViewModel> GetAllInstrumentQuarantineList(int userId, int userRoleId)
  {

    try
    {
      UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
      List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
      if (userRoleId == 2 || labUserById.DepartmentId == 66)
      {
        instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Include(I => I.DepartmenttModel).Select(s => new InstrumentViewModel()
        {
          Id = s.Id,
          InstrumentName = s.InstrumentName,
          SlNo = s.SlNo,
          IdNo = s.IdNo,
          Range = s.Range,
          LC = s.LC,
          CalibFreq = s.CalibFreq,
          CalibDate = s.CalibDate,
          DueDate = s.DueDate,
          Make = s.Make,
          CalibSource = s.CalibSource,
          StandardReffered = s.StandardReffered,
          Remarks = s.Remarks,
          Status = s.Status,
          FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
          CalibrationStatus = s.CalibrationStatus,
          InstrumentStatus = s.InstrumentStatus,
          DateOfReceipt = s.DateOfReceipt,
          DepartmentName = s.DepartmenttModel.Name,
          NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault(),
          QuaraantineOn = s.QuarantineModel.Select(s => s.CreatedOn).FirstOrDefault(),
          QuarantineReason = s.QuarantineModel.Select(s => s.Reason).FirstOrDefault()
        }
        ).ToList();
      }
      else if (userRoleId == 1)//And 
      {
        instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1 && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Select(s => new InstrumentViewModel()
        {
          Id = s.Id,
          InstrumentName = s.InstrumentName,
          SlNo = s.SlNo,
          IdNo = s.IdNo,
          Range = s.Range,
          LC = s.LC,
          CalibFreq = s.CalibFreq,
          CalibDate = s.CalibDate,
          DueDate = s.DueDate,
          Make = s.Make,
          CalibSource = s.CalibSource,
          StandardReffered = s.StandardReffered,
          Remarks = s.Remarks,
          Status = s.Status,
          FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
          CalibrationStatus = s.CalibrationStatus,
          InstrumentStatus = s.InstrumentStatus,
          DateOfReceipt = s.DateOfReceipt,
          DepartmentName = s.DepartmenttModel.Name,
          NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault(),
          QuaraantineOn = s.QuarantineModel.Select(s => s.CreatedOn).FirstOrDefault(),
          QuarantineReason = s.QuarantineModel.Select(s => s.Reason).FirstOrDefault()
        }
        ).ToList();
         }
            if (userRoleId == 4)
            {
                instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1 && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Include(I => I.DepartmenttModel).Select(s => new InstrumentViewModel()
                {
                    Id = s.Id,
                    InstrumentName = s.InstrumentName,
                    SlNo = s.SlNo,
                    IdNo = s.IdNo,
                    Range = s.Range,
                    LC = s.LC,
                    CalibFreq = s.CalibFreq,
                    CalibDate = s.CalibDate,
                    DueDate = s.DueDate,
                    Make = s.Make,
                    CalibSource = s.CalibSource,
                    StandardReffered = s.StandardReffered,
                    Remarks = s.Remarks,
                    Status = s.Status,
                    FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
                    CalibrationStatus = s.CalibrationStatus,
                    InstrumentStatus = s.InstrumentStatus,
                    DateOfReceipt = s.DateOfReceipt,
                    DepartmentName = s.DepartmenttModel.Name,
                    NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault(),
                    QuaraantineOn = s.QuarantineModel.Select(s => s.CreatedOn).FirstOrDefault(),
                    QuarantineReason = s.QuarantineModel.Select(s => s.Reason).FirstOrDefault()
                }
                ).ToList();
            }
            return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = instrumentList
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseServiceMethod = "Instrument",
        ResponseService = "GetAllInstrumentList"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> InstrumentQuarantine(int instrumentId, string reason, int userId, int statusId)
  {

    try
    {
      _unitOfWork.BeginTransaction();
      InstrumentQuarantine instumentQuarantineById = _unitOfWork.Repository<InstrumentQuarantine>().GetQueryAsNoTracking(Q => Q.InstrumentId == instrumentId).SingleOrDefault();
      if (instumentQuarantineById != null)
      {
        instumentQuarantineById.StatusId = 1;
        instumentQuarantineById.CreatedOn = DateTime.Now;
        instumentQuarantineById.Reason = reason;
        _unitOfWork.Repository<InstrumentQuarantine>().Update(instumentQuarantineById);
      }
      else
      {
        InstrumentQuarantine instrumentQuarantine = new InstrumentQuarantine()
        {
          InstrumentId = instrumentId,
          Reason = reason,
          UserId = userId,
          CreatedOn = DateTime.Now,
          StatusId = statusId
        };
        _unitOfWork.Repository<InstrumentQuarantine>().Insert(instrumentQuarantine);
      }

      _unitOfWork.SaveChanges();
      _unitOfWork.Commit();

      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      _unitOfWork.RollBack();
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "InstrumentQuarantine"
      };
    }
  }

  public ResponseViewModel<InstrumentViewModel> InstrumentRemoveQuarantine(int instrumentId, int statusId, int userId)
  {
    try
    {
      _unitOfWork.BeginTransaction();
      User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
      User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.ForemanShortId).SingleOrDefault();
      User LabuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Level == "L4" && Q.DepartmentId == 66).SingleOrDefault();
      Instrument instrument = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrumentId).FirstOrDefault();
      InstrumentQuarantine instrumentQuarantineById = _unitOfWork.Repository<InstrumentQuarantine>().GetQueryAsNoTracking(Q => Q.InstrumentId == instrumentId && Q.StatusId == 1).FirstOrDefault();
      instrumentQuarantineById.StatusId = statusId;
      _unitOfWork.Repository<InstrumentQuarantine>().Update(instrumentQuarantineById);
      Request newRequest = new Request();
      Request getMaxId = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id > 0).OrderByDescending(O => O.Id).FirstOrDefault();
      long maxId = 1;
      if (getMaxId != null)
      {
        maxId = getMaxId.Id + 1;
      }
      string requestNumberFormat = maxId.ToString().PadLeft(4, '0');
      newRequest.ReqestNo = "CR" + DateTime.Now.Year + requestNumberFormat;
      newRequest.InstrumentId = instrumentId;
      newRequest.RequestDate = DateTime.Now;
      newRequest.TypeOfReqest = 3;
      newRequest.StatusId = (Int32)EnumRequestStatus.Requested;
      newRequest.CreatedBy = userId;
      newRequest.CreatedOn = DateTime.Now;
      newRequest.UserL4 = DeptuserByL4Id.Id;
      newRequest.LabL4 = LabuserByL4Id.Id;
      _unitOfWork.Repository<Request>().Insert(newRequest);
      _unitOfWork.SaveChanges();

      RequestStatus ReqestStatus = new RequestStatus();
      ReqestStatus.RequestId = newRequest.Id;
      ReqestStatus.StatusId = (Int32)EnumRequestStatus.Requested;
      ReqestStatus.CreatedOn = DateTime.Now;
      ReqestStatus.CreatedBy = userId;
      _unitOfWork.Repository<RequestStatus>().Insert(ReqestStatus);
      _unitOfWork.SaveChanges();
      _unitOfWork.Commit();
      List<string> emailList = new List<string>();
      string RequestType = string.Empty;
      if (newRequest.TypeOfReqest == 1)
      {
        RequestType = "New";
      }
      else if (newRequest.TypeOfReqest == 2)
      {
        RequestType = "Regular";
      }
      else if (newRequest.TypeOfReqest == 3)
      {
        RequestType = "Recalibration";
      }
      string UserId = _contextAccessor.HttpContext.Session.GetString("LoggedId");
      UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
      //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2).SingleOrDefault());
      string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Created by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";

      emailList.Add(DeptuserByL4Id.Email.Trim());
      emailList.Add(LabuserByL4Id.Email.Trim());
      mailbody = mailbody.Replace("$NAME$", DeptuserByL4Id.FirstName + "/" + LabuserByL4Id.LastName).Replace("$USERNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$", newRequest.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrument.InstrumentName).Replace("$INSTRUMENTID$", instrument.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(newRequest.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName);
      EmailViewModel emailViewModel = new EmailViewModel()
      {
        ToList = emailList,
        Subject = "New Instrument Request Recalibration- " + newRequest.ReqestNo + "",
        Body = mailbody,//"Hi " + fmUserById.FirstName + " " + fmUserById.LastName + ",<br/> New Calibration Request created by " + labUserById.FirstName + " " + labUserById.LastName + ". Please login to your CMT account and Approve / Reject request.",
        IsHtml = true
      };
      _emailService.SendEmailAsync(emailViewModel, true);


      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = null
      };
    }
    catch (Exception e)
    {
      _unitOfWork.RollBack();
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "InstrumentQuarantine"
      };
    }
  }
  public ResponseViewModel<InstrumentViewModel> GetInstrumentListByName(string instrumentName)
  {
    try
    {
      List<InstrumentViewModel> instrumentByNameList = _unitOfWork.Repository<Instrument>()
      .GetQueryAsNoTracking(Q => Q.InstrumentName.StartsWith(instrumentName))
      .Include(I => I.QuarantineModel)
      .Include(I => I.FileUploadModel)
      .Select(s => new InstrumentViewModel()
      {
        Id = s.Id,
        InstrumentName = s.InstrumentName,
        SlNo = s.SlNo,
        IdNo = s.IdNo,
        Range = s.Range,
        LC = s.LC,
      }).ToList();

      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = instrumentByNameList
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "GetInstrumentListByName"
      };
    }
  }

  public ResponseViewModel<InstrumentViewModel> GetInstrumentListByIdNo(string idNo)
  {
    try
    {
      List<InstrumentViewModel> instrumentViewModelList = _unitOfWork.Repository<Instrument>()
     .GetQueryAsNoTracking(Q => Q.IdNo.ToUpper().StartsWith(idNo.ToUpper()))
     .Include(I => I.QuarantineModel)
     .Include(I => I.FileUploadModel)
     .Select(s => new InstrumentViewModel()
     {
       Id = s.Id,
       InstrumentName = s.InstrumentName,
       SlNo = s.SlNo,
       IdNo = s.IdNo,
       Range = s.Range,
       LC = s.LC,
     }).ToList();

      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = instrumentViewModelList
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseService = "Instrument",
        ResponseServiceMethod = "GetInstrumentListByIdNo"
      };
    }
  }
  public class SmtpSettings
  {
    public string Server = "53.151.100.102";
    public string Port = "25";
    public string FromAddress = "DICV-CAL@DAIMLER.COM";
    public string UserId = "DICV-EBOM@DAIMLER.COM";
    public string Pwd = "Dicv@123";
    public bool IsDevelopmentMode = true;
  }

  public ResponseViewModel<InstrumentViewModel> GetCurrentMonthDueList()
  {
    try
    {
      List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();

      int currentYear = DateTime.Now.Year;
      int currentMonth = DateTime.Now.Month;
      instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => (Q.IdNo != "" && Q.IdNo != null)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Where(Q => Q.DueDate.Month == DateTime.Now.Month).Select(s => new InstrumentViewModel()
      {
        Id = s.Id,
        InstrumentName = s.InstrumentName,
        SlNo = s.SlNo,
        IdNo = s.IdNo,
        Range = s.Range,
        LC = s.LC,
        CalibFreq = s.CalibFreq,
        CalibDate = s.CalibDate,
        DueDate = s.DueDate,
        Make = s.Make,
        CalibSource = s.CalibSource,
        StandardReffered = s.StandardReffered,
        UserDept = s.UserDept,
        CreatedBy = s.CreatedBy,
        Remarks = s.Remarks,
        Status = s.Status,
        FileList = s.FileUploadModel.Select(s => s.Upload.FileName.ToString()).ToList(),
        CalibrationStatus = s.CalibrationStatus,
        InstrumentStatus = s.InstrumentStatus,
        DateOfReceipt = s.DateOfReceipt,
        NewReqAcceptStatus = s.RequestModel.Where(W => W.TypeOfReqest == 1).Select(S => S.StatusId).FirstOrDefault()
      }
      ).ToList();
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 200,
        ResponseMessage = "Success",
        ResponseData = null,
        ResponseDataList = instrumentList.Where(W => W.DueDate.Month == DateTime.Now.Month && W.
        DueDate.Year == DateTime.Now.Year).ToList()
      };
    }
    catch (Exception e)
    {
      return new ResponseViewModel<InstrumentViewModel>
      {
        ResponseCode = 500,
        ResponseMessage = "Failure",
        ErrorMessage = e.Message,
        ResponseData = null,
        ResponseDataList = null,
        ResponseServiceMethod = "Instrument",
        ResponseService = "GetAllInstrumentList"
      };
    }
  }
}
