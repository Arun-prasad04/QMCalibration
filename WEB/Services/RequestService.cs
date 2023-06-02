using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Drawing;
namespace WEB.Services;

public class RequestService : IRequestService
{
	private readonly IMapper _mapper;
	private IHttpContextAccessor _contextAccessor { get; set; }
	private IUnitOfWork _unitOfWork { get; set; }
	private IEmailService _emailService;
	private IConfiguration _configuration;
	private IInstrumentService _instrumentService { get; set; }
	public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IEmailService emailService, IConfiguration Configuration, IInstrumentService instrumentService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_contextAccessor = contextAccessor;
		_emailService = emailService;
		_configuration = Configuration;
		_instrumentService = instrumentService;
	}

	public List<TemplateObservation> GetTemplateObservations()
	{

		var templateObservation = _unitOfWork.Repository<TemplateObservation>()
									  .GetQueryAsNoTracking()
									  .ToList();
		return templateObservation;
	}

	public ResponseViewModel<RequestViewModel> GetAllRequestList(int userRoleId, int userId)
	{
		try
		{
			List<RequestViewModel> RequestList = new List<RequestViewModel>();
			if (userRoleId == 2 || userRoleId == 4)
			{
				RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking()
				.Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel)
				.Select(s => new RequestViewModel()
				{
					Id = s.Id,
					ReqestNo = s.ReqestNo,
					InstrumentName = s.InstrumentModel.InstrumentName,
					InstrumentIdNo = s.InstrumentModel.IdNo,
					InstrumentId = s.InstrumentId,
					RequestDate = s.RequestDate,
					TypeOfRequest = s.TypeOfReqest,
					Range = s.InstrumentModel.Range,
					InstrumentSerialNumber = s.InstrumentModel.SlNo,
					CalibDate = s.InstrumentModel.CalibDate,
					DueDate = s.InstrumentModel.DueDate,
					UserDept = s.InstrumentModel.UserDept,
					SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
					RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
					Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
					ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
					ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
					RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
					Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
					ReceivedBy = s.ReceivedBy,
					InstrumentCondition = s.InstrumentCondition,
					Feasiblity = s.Feasiblity,
					TentativeCompletionDate = s.TentativeCompletionDate,
					ReceivedDate = s.ReceivedDate,
					IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
					ObservationTemplate = s.InstrumentModel.ObservationTemplate,
					ObservationType = s.InstrumentModel.ObservationType,
					MUTemplate = s.InstrumentModel.MUTemplate,
					CertificationTemplate = s.InstrumentModel.CertificationTemplate,
					UserRoleId = userRoleId,
					LabResult = s.Result
				}).ToList();
			}
			else
			{
				RequestList = _unitOfWork.Repository<Request>()
									   .GetQueryAsNoTracking(x => x.CreatedBy == userId || x.LabL4 == userId || x.UserL4 == userId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
									   {
										   Id = s.Id,
										   ReqestNo = s.ReqestNo,
										   InstrumentName = s.InstrumentModel.InstrumentName,
										   InstrumentIdNo = s.InstrumentModel.IdNo,
										   InstrumentId = s.InstrumentId,
										   RequestDate = s.RequestDate,
										   TypeOfRequest = s.TypeOfReqest,
										   Range = s.InstrumentModel.Range,
										   InstrumentSerialNumber = s.InstrumentModel.SlNo,
										   CalibDate = s.InstrumentModel.CalibDate,
										   DueDate = s.InstrumentModel.DueDate,
										   UserDept = s.InstrumentModel.UserDept,
										   SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
										   RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
										   Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
										   ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
										   ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
										   RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
										   Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
										   ReceivedBy = s.ReceivedBy,
										   InstrumentCondition = s.InstrumentCondition,
										   Feasiblity = s.Feasiblity,
										   TentativeCompletionDate = s.TentativeCompletionDate,
										   ReceivedDate = s.ReceivedDate,
										   IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
										   ObservationTemplate = s.InstrumentModel.ObservationTemplate,
										   ObservationType = s.InstrumentModel.ObservationType,
										   MUTemplate = s.InstrumentModel.MUTemplate,
										   CertificationTemplate = s.InstrumentModel.CertificationTemplate,
										   UserRoleId = userRoleId,
										   LabResult = s.Result
									   }).ToList();
			}

			if (RequestList.Any())
			{
				var templateObservationList = GetTemplateObservations();
				int? reviewStatus = 0;
				foreach (var data in RequestList)
				{
					var observationData = templateObservationList.Where(x => x.InstrumentId == data.InstrumentId
																	  && x.RequestId == data.Id)
														   .FirstOrDefault();
					if (observationData != null)
						reviewStatus = observationData.ReviewStatus;
					else
						reviewStatus = 0;

					data.TemplateReviewStatus = reviewStatus == null ? 0 : reviewStatus;
				}
			}

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = RequestList
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Request",
				ResponseService = "GetAllRequestList"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> GetRequestById(int RequestId)
	{
		try
		{
			RequestViewModel RequestById = _unitOfWork.Repository<Request>()
									.GetQueryAsNoTracking(Q => Q.Id == RequestId)
									.Include(I => I.InstrumentModel)
									.Include(I => I.RequestStatusModel)
			.Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				LC = s.InstrumentModel.LC,
				Unit1 = s.InstrumentModel.Unit1,
				Unit2 = s.InstrumentModel.Unit2,
				Unit3 = s.InstrumentModel.Unit3,
				Instrument_Type = s.InstrumentModel.Instrument_Type,
				Rule_Confirmity = s.InstrumentModel.Rule_Confirmity,
				Drawing_Attached = s.InstrumentModel.Drawing_Attached,
				TW = s.InstrumentModel.TW_Type,
				Make = s.InstrumentModel.Make,
				CalibFreq = s.InstrumentModel.CalibFreq,
				StandardReffered1 = s.InstrumentModel.StandardReffered1,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,
				LabResult = s.Result,
				InstrumentReturnedOn = s.InstrumentReturnedOn,
				CollectedBy = s.CollectedBy,
				ReasonforRejection = s.ReasonforRejection,
				IsFeasibleService = s.IsFeasibleService,
				IsFeasibleYes = s.IsFeasibleYes,
				ServiceResponsibility = s.ServiceResponsibility,
				LabL4 = s.LabL4,
				IsLabL4Accepted = s.IsLabL4Accepted,
				UserL4 = s.UserL4,
				IsUserL4Accepted = s.IsUserL4Accepted
			}).SingleOrDefault();

			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			if (RequestById.Status == 27)
			{
				List<TemplateObservation> TemplateObservationList = _unitOfWork.Repository<TemplateObservation>().GetQueryAsNoTracking(g => g.RequestId == RequestById.Id).ToList();
				RequestById.ReviewedStatus = TemplateObservationList.Where(w => w.RequestId == RequestById.Id).Select(q => q.ReviewStatus).FirstOrDefault();
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			InstrumentViewModel instrumentEmptyViewModel = new InstrumentViewModel();
			List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Instrument").ToList());
			List<LovsViewModel> lovsListFrquency = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Master").ToList());
			instrumentEmptyViewModel.InstrumentStatusList = lovsList.Where(W => W.AttrName == "InstrumentStatus").ToList();
			instrumentEmptyViewModel.StatusList = lovsList.Where(W => W.AttrName == "Status").ToList();
			instrumentEmptyViewModel.TemplateNameList = lovsList.Where(W => W.AttrName == "TemplateName").ToList();
			RequestById.CalibFreqList = lovsListFrquency.Where(W => W.AttrName == "CalibrationFreq").ToList();
			instrumentEmptyViewModel.CalibrationStatusList = lovsList.Where(W => W.AttrName == "CalibrationStatus").ToList();
			RequestById.ObservationTemplateList = lovsList.Where(W => W.AttrName == "ObservationTemplate").ToList();
			RequestById.MUTemplateList = lovsList.Where(W => W.AttrName == "MUTemplate").ToList();
			RequestById.CertificationTemplateList = lovsList.Where(W => W.AttrName == "CerTemplate").ToList();
			instrumentEmptyViewModel.MasterData = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking().ToList());
			var CFreq = RequestById.CalibFreq;
			RequestById.CalibFrequency = lovsListFrquency.Where(W => W.AttrName == "CalibrationFreq" && W.Id == CFreq).Select(x => x.AttrValue).SingleOrDefault();
			List<Uploads> UploadList = _unitOfWork.Repository<Uploads>().GetQueryAsNoTracking(g => g.RequestId == RequestId).ToList();
			if (UploadList.Count > 0)
			{
				RequestById.MUTemplateFileName = UploadList.Where(w => w.RequestId == RequestId).Select(q => q.FileName).Take(1).SingleOrDefault();
			}

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("RequestService - GetRequestById Method Exception");
			ErrorViewModelTest.Log(e.Message);
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Request",
				ResponseServiceMethod = "GetRequestByID"
			};
		}

	}
	public ResponseViewModel<RequestViewModel> InsertRequest(int instrumentId, int userId, int typeId)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
			User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.ForemanShortId && Q.DepartmentId == userById.DepartmentId).FirstOrDefault();
			User LabuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Level == "L4" && Q.DepartmentId == 66).FirstOrDefault();
			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrumentId).SingleOrDefault();
			Request newRequest = new Request();
			Request getMaxId = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id > 0).OrderByDescending(O => O.Id).FirstOrDefault();
			long maxId = 1;
			if (getMaxId != null)
			{
				maxId = getMaxId.Id + 1;
			}
			string requestNumberFormat = maxId.ToString().PadLeft(4, '0');
			newRequest.ReqestNo = "CR" + DateTime.Now.Year + requestNumberFormat;
			newRequest.InstrumentId = instrumentById.Id;
			newRequest.RequestDate = DateTime.Now;
			newRequest.TypeOfReqest = typeId;
			newRequest.CreatedBy = userId;
			if (typeId == 3)
			{
				newRequest.UserL4 = DeptuserByL4Id.Id;
				newRequest.LabL4 = LabuserByL4Id.Id;
			}
			newRequest.CreatedOn = DateTime.Now;
			newRequest.StatusId = (Int32)EnumRequestStatus.Requested;
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
			string UserId = _contextAccessor.HttpContext.Session.GetString("LoggedId");
			UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
			List<UserViewModel> fmUserById = _mapper.Map<List<UserViewModel>>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2 && Q.Level != "L4").ToList());
			List<string> emailList = new List<string>();
			string RequestType = string.Empty;
			if (typeId == 1 || typeId == 2)
			{
				foreach (var item in fmUserById)
				{
					//semailList.Add(item.Email.Trim());

					if (typeId == 1)
					{
						RequestType = "New";
					}
					else if (typeId == 2)
					{
						RequestType = "Regular";
					}
					else if (typeId == 3)
					{
						RequestType = "Recalibration";
					}
					string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Created by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
					mailbody = mailbody.Replace("$NAME$", item.FirstName + " " + item.LastName).Replace("$REQNO$", newRequest.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(newRequest.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$USERNAME$", labUserById.FirstName + " " + labUserById.LastName);
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
					smtp.Send(message);
				}
			}
			else
			{
				RequestType = "Recalibration";
				string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Created by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
				mailbody = mailbody.Replace("$NAME$", DeptuserByL4Id.FirstName + "/" + LabuserByL4Id.FirstName).Replace("$USERNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$", newRequest.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(newRequest.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName);
				MailMessage message = new MailMessage();
				SmtpClient smtp = new SmtpClient();
				SmtpSettings smtpvalue = new SmtpSettings();
				message.From = new MailAddress(smtpvalue.FromAddress);
				emailList.Add(DeptuserByL4Id.Email.Trim());
				emailList.Add(LabuserByL4Id.Email.Trim());
				//message.To.Add(new MailAddress(item.Email.Trim()));
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
				smtp.Send(message);
			}
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Request",
				ResponseServiceMethod = "InsertRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> UpdateRequest(RequestViewModel Request)
	{

		try
		{
			_unitOfWork.BeginTransaction();
			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking().SingleOrDefault();
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Request",
				ResponseServiceMethod = "UpdateRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> DeleteRequest(RequestViewModel requestId)
	{
		try
		{
			Request RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking().SingleOrDefault();
			_unitOfWork.Repository<Request>().Delete(_mapper.Map<Request>(requestId));
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "Request",
				ResponseServiceMethod = "DeleteRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> AcceptRequest(int requestId, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, int newObservation, int newObservationType, int newMU, int newCertification, string standardReffered, bool newNABL, int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = requestId;
			reqestStatus.StatusId = (Int32)EnumRequestStatus.Approved;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = userId;
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);

			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			requestById.StatusId = (Int32)EnumRequestStatus.Approved;
			requestById.InstrumentCondition = InstrumentCondition;
			requestById.ReceivedBy = userId;
			requestById.Feasiblity = Feasiblity;

			//---------start of converting Default date to null to fix out of range issue--------

			
			int Tyear = TentativeCompletionDate.Year;
			//int? Tyear = TentativeCompletionDate.Year == 1 ? null :;
            
			if (Tyear.Equals(1))
            {
                requestById.TentativeCompletionDate = null;
            }
            else
            {
                requestById.TentativeCompletionDate = TentativeCompletionDate;
            }
            //---------end of converting date---------------------

            requestById.TentativeCompletionDate = TentativeCompletionDate;
			requestById.ReceivedDate = DateTime.Now;
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();

			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
			instrumentById.IsNABL = newNABL;
			instrumentById.ObservationTemplate = newObservation;
			instrumentById.ObservationType = newObservationType;
			instrumentById.MUTemplate = newMU;
			instrumentById.CertificationTemplate = newCertification;
			instrumentById.StandardReffered = standardReffered;
			instrumentById.MasterInstrument1 = MasterInstrument1;
			instrumentById.MasterInstrument2 = MasterInstrument2;
			instrumentById.MasterInstrument3 = MasterInstrument3;
			instrumentById.MasterInstrument4 = MasterInstrument4;
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();


			_unitOfWork.Commit();
			long UserId = requestById.CreatedBy;
			UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
			UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
			//List<string> emailList = new List<string>();
			//emailList.Add(fmUserById.Email);
			string RequestType = string.Empty;
			if (requestById.TypeOfReqest == 1)
			{
				RequestType = "New";
			}
			else if (requestById.TypeOfReqest == 2)
			{
				RequestType = "Regular";
			}
			else if (requestById.TypeOfReqest == 3)
			{
				RequestType = "Recalibration";
			}
			string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Accepted by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
			mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestById.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestById.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName);

			//   EmailViewModel emailViewModel = new EmailViewModel()
			//   {
			//     ToList = emailList,
			//     Subject = "New Calibration Request Accept Notification-" + requestById.ReqestNo + "",
			//     Body = mailbody,//"Hi " + labUserById.FirstName + " " + labUserById.LastName + ",<br/> Calibration Request( " + requestById.ReqestNo + ") accepted by " + fmUserById.FirstName + " " + fmUserById.LastName + ". Please login to your CMT account for more details about request.",
			//     IsHtml = true
			//   };
			//   _emailService.SendEmailAsync(emailViewModel, true);
			string mailSubject = "New Calibration Request Accept Notification-" + requestById.ReqestNo + "";

			_emailService.EmailSendingFunction(fmUserById.Email, mailbody, mailSubject);





			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,



			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "InsertExternalCalibrationRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> AcceptRequestRecalibration(int requestId, int userId, int AcceptValue, int departmentId)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			if (departmentId == 66)
			{
				requestById.IsLabL4Accepted = AcceptValue;
			}
			else
			{
				requestById.IsUserL4Accepted = AcceptValue;
			}
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,
				LabL4 = s.LabL4,
				IsLabL4Accepted = s.IsLabL4Accepted,
				UserL4 = s.UserL4,
				IsUserL4Accepted = s.IsUserL4Accepted


			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "InsertExternalCalibrationRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> RejectRequest(int requestId, string RejectReason, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string standardReffered)
	{
		try
		{
			_unitOfWork.BeginTransaction();

			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = requestId;
			reqestStatus.StatusId = (Int32)EnumRequestStatus.Rejected;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = userId;
			reqestStatus.Comment = RejectReason;
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);


			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			requestById.StatusId = (Int32)EnumRequestStatus.Rejected;
			requestById.InstrumentCondition = InstrumentCondition;
			requestById.ReceivedBy = userId;
			requestById.Feasiblity = Feasiblity;
			requestById.TentativeCompletionDate = TentativeCompletionDate;
			requestById.ReceivedDate = DateTime.Now;
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();

			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
			instrumentById.StandardReffered = standardReffered;
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();



			_unitOfWork.Commit();
			long UserId = requestById.CreatedBy;
			UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
			UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
			List<string> emailList = new List<string>();
			emailList.Add(fmUserById.Email);
			string RequestType = string.Empty;
			if (requestById.TypeOfReqest == 1)
			{
				RequestType = "New";
			}
			else if (requestById.TypeOfReqest == 2)
			{
				RequestType = "Regular";
			}
			else if (requestById.TypeOfReqest == 3)
			{
				RequestType = "Recalibration";
			}
			string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr><tr><td align='left'>Reason of Rejection</td><td>:</td>                <td>                    $Reason$                </td>            </tr></table>  <p><a href='http://s365id1qdg044' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
			mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestById.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestById.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$Reason$", RejectReason);
			//   EmailViewModel emailViewModel = new EmailViewModel()
			//   {
			//     ToList = emailList,
			//     Subject = "New Calibration Request Reject Notification-" + requestById.ReqestNo + "",
			//     Body = mailbody,//"Hi " + labUserById.FirstName + " " + labUserById.LastName + ",<br/> Calibration Request( " + requestById.ReqestNo + ") rejected by " + fmUserById.FirstName + " " + fmUserById.LastName + ". Please login to your CMT account for more details about request. <br /> <b>Reason:</b>  " + RejectReason,
			//     IsHtml = true
			//   };
			var mailSubject = "New Calibration Request Reject Notification-" + requestById.ReqestNo + "";
			//_emailService.SendEmailAsync(emailViewModel, true);
			_emailService.EmailSendingFunction(fmUserById.Email, mailbody, mailSubject);

			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,

			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "CalibrationRequest",
				ResponseServiceMethod = "AcceptCalibrationRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> SubmitDepartmentRequestVisual(int requestId, string Result, int userId, string CollectedBy)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = requestId;
			reqestStatus.StatusId = (Int32)EnumRequestStatus.Closed;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = userId;
			reqestStatus.Comment = Result;
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);


			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			requestById.StatusId = (Int32)EnumRequestStatus.Closed;
			requestById.CollectedBy = CollectedBy;
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,
			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "CalibrationRequest",
				ResponseServiceMethod = "AcceptCalibrationRequest"
			};
		}
	}
	public void SaveFiles(string FileData, string FileName, string RequestId)
	{
		string filePath = string.Empty;
		try
		{
			var bytes = Convert.FromBase64String(FileData.Split(',')[1]);
			var Path = _configuration["MUTemplateUrl"] + RequestId;
			if (!Directory.Exists(Path))
			{
				Directory.CreateDirectory(Path);
			}
			filePath = _configuration["MUTemplateUrl"] + "\\" + RequestId + "\\" + FileName;
			if (File.Exists(filePath))
			{
				File.Delete(filePath);
			}
			using (var imageFile = new FileStream(filePath, FileMode.Create))
			{
				imageFile.Write(bytes, 0, bytes.Length);
				imageFile.Flush();
			}
		}
		catch (Exception ex)
		{

		}

	}
	public bool SaveQRFile(QRCodeFilesViewModel qrCodeFilesViewModel, string instrumentId)
	{
		try
		{
			iTextSharp.text.Rectangle pageSize = null;


			int InstrumentId = Convert.ToInt32(instrumentId);
			var InstrumentData = _instrumentService.GetInstrumentById(InstrumentId);
			var filePath = _configuration["temp"] + InstrumentData.ResponseData.IdNo;
			FileStream fs = File.Create(filePath);


			using (var srcImage = new Bitmap(qrCodeFilesViewModel.QRImageUrl))
			{
				pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
			}
			using (var ms = new MemoryStream())
			{
				var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
				iTextSharp.text.pdf.PdfWriter.GetInstance(document, ms).SetFullCompression();
				document.Open();
				var image = iTextSharp.text.Image.GetInstance(qrCodeFilesViewModel.QRImageUrl);
				document.Add(image);
				document.Close();
				File.WriteAllBytes(filePath, ms.ToArray());
			}
			return true;

		}
		catch (Exception e)
		{
			return false;
			//throw e;
		}

	}
	public ResponseViewModel<RequestViewModel> SubmitLABRequestVisual(int requestId, string Result, int userId, string RecordBy, DateTime ClosedDate, string Remarks, DateTime InstrumentReturnedDate, string CollectedBy, string ReasonforRejection, string IsFeasibleService, string IsFeasibleYes, string ServiceResponsibility, int RequestType, int InstrumentId, DateTime CalibDate, DateTime DueDate, bool newNABL, List<UploadFile> FileData, string IdNo)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = requestId;
			reqestStatus.StatusId = (Int32)EnumRequestStatus.Sent;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = userId;
			reqestStatus.Comment = Remarks;
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);

			if (FileData.Count > 0)
			{
				SaveFiles(FileData[0].Data, FileData[0].Name, Convert.ToString(requestId));
				var filePath = _configuration["MUTemplateUrl"] + "\\" + requestId + "\\" + FileData[0].Name;
				Uploads upload = new Uploads()
				{
					FileName = FileData[0].Name,
					FileGuid = Guid.NewGuid(),
					CreatedOn = DateTime.Now,
					ModifiedOn = DateTime.Now,
					FilePath = filePath,
					RequestId = requestId,
					TemplateType = "MU"
				};
				_unitOfWork.Repository<Uploads>().Insert(upload);
				_unitOfWork.SaveChanges();
			}
			string MailSubject = string.Empty;
			string mailbody = string.Empty;
			Request requestbyIdd = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestbyIdd.InstrumentId).SingleOrDefault();

			instrumentById.CalibDate = CalibDate;
			// instrumentById.ObservationTemplate = ObservationTemplate;
			// instrumentById.ObservationType=ObservationTemplateType;
			// instrumentById.MUTemplate= MUTemplate;
			// instrumentById.CertificationTemplate=CertificationTemplate;

			if (Result != "Rejected")
			{
				instrumentById.DueDate = DueDate;
			}
			instrumentById.IdNo = IdNo;
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();

			long UserId = requestbyIdd.CreatedBy;
			UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
			UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
			List<string> emailList = new List<string>();
			emailList.Add(fmUserById.Email);
			string Type = string.Empty;
			if (requestbyIdd.TypeOfReqest == 1)
			{
				Type = "New";
			}
			else if (requestbyIdd.TypeOfReqest == 2)
			{
				Type = "Regular";
			}
			else if (requestbyIdd.TypeOfReqest == 3)
			{
				Type = "Recalibration";
			}

			if (RequestType == 1)
			{
				Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
				requestById.StatusId = (Int32)EnumRequestStatus.Sent;
				requestById.InstrumentReturnedOn = InstrumentReturnedDate;
				requestById.CollectedBy = CollectedBy;
				requestById.Result = Result;
				requestById.ReasonforRejection = ReasonforRejection;
				_unitOfWork.Repository<Request>().Update(requestById);
				_unitOfWork.SaveChanges();

				if (Result == "Rejected")
				{
					MailSubject = "New Calibration Request(" + requestById.ReqestNo + ") -" + Result;
					mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>Requested By</td>     <td>:</td><td>$REQNAME$</td></tr><tr><td align='left'>Date</td><td>:</td><td>$DATE$</td></tr><tr><td align='left'>Instrument Returned On</td><td>:</td><td>$RETURNEDDATE$</td></tr><tr><td align='left'>Collected By</td><td>:</td><td>$COLLECTED$</td></tr><tr><td align='left'>Reason for Rejection</td><td>:</td><td>$REASON$</td></tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
				}
				else
				{
					MailSubject = "New Calibration Request(" + requestById.ReqestNo + ") -" + Result;
					mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>Requested By</td>     <td>:</td><td>$REQNAME$</td></tr><tr><td align='left'>Date</td><td>:</td><td>$DATE$</td></tr><tr><td align='left'>Instrument Returned On</td><td>:</td><td>$RETURNEDDATE$</td></tr><tr><td align='left'>Collected By</td><td>:</td><td>$COLLECTED$</td></tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
				}

			}
			else if (RequestType == 2 || RequestType == 3)
			{
				Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
				requestById.StatusId = (Int32)EnumRequestStatus.Sent;
				requestById.InstrumentReturnedOn = InstrumentReturnedDate;
				requestById.CollectedBy = CollectedBy;
				requestById.Result = Result;
				requestById.ReasonforRejection = ReasonforRejection;
				requestById.IsFeasibleService = IsFeasibleService;
				requestById.IsFeasibleYes = IsFeasibleYes;
				requestById.ServiceResponsibility = ServiceResponsibility;
				_unitOfWork.Repository<Request>().Update(requestById);
				_unitOfWork.SaveChanges();

				if (IsFeasibleService == "No")
				{
					InstrumentQuarantine instumentQuarantineById = _unitOfWork.Repository<InstrumentQuarantine>().GetQueryAsNoTracking(Q => Q.InstrumentId == InstrumentId).SingleOrDefault();
					if (instumentQuarantineById != null)
					{
						instumentQuarantineById.StatusId = 1;
						instumentQuarantineById.CreatedOn = DateTime.Now;
						instumentQuarantineById.Reason = ReasonforRejection;
						_unitOfWork.Repository<InstrumentQuarantine>().Update(instumentQuarantineById);
					}
					else
					{
						InstrumentQuarantine instrumentQuarantine = new InstrumentQuarantine()
						{
							InstrumentId = InstrumentId,
							Reason = ReasonforRejection,
							UserId = userId,
							CreatedOn = DateTime.Now,
							StatusId = 1
						};
						_unitOfWork.Repository<InstrumentQuarantine>().Insert(instrumentQuarantine);
					}
				}
				_unitOfWork.SaveChanges();


				if (Result == "Rejected")
				{
					if (IsFeasibleService == "No")
					{
						MailSubject = "New Calibration Request Rejected(" + requestById.ReqestNo + ")- Moved to Quarantine";
						mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>Requested By</td>     <td>:</td><td>$REQNAME$</td></tr><tr><td align='left'>Date</td><td>:</td><td>$DATE$</td></tr><tr><td align='left'>Instrument Returned On</td><td>:</td><td>$RETURNEDDATE$</td></tr><tr><td align='left'>Collected By</td><td>:</td><td>$COLLECTED$</td></tr><tr><td align='left'>Reason for Rejection</td><td>:</td><td>$REASON$</td></tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
					}
					else
					{
						MailSubject = "New Calibration Request (" + requestById.ReqestNo + ") -" + Result;
						mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>Requested By</td>     <td>:</td><td>$REQNAME$</td></tr><tr><td align='left'>Date</td><td>:</td><td>$DATE$</td></tr><tr><td align='left'>Instrument Returned On</td><td>:</td><td>$RETURNEDDATE$</td></tr><tr><td align='left'>Collected By</td><td>:</td><td>$COLLECTED$</td></tr><tr><td align='left'>Reason for Rejection</td><td>:</td><td>$REASON$</td></tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
					}
				}
				else
				{
					MailSubject = "New Calibration Request(" + requestById.ReqestNo + ") -" + Result;
					mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>Requested By</td>     <td>:</td><td>$REQNAME$</td></tr><tr><td align='left'>Date</td><td>:</td><td>$DATE$</td></tr><tr><td align='left'>Instrument Returned On</td><td>:</td><td>$RETURNEDDATE$</td></tr><tr><td align='left'>Collected By</td><td>:</td><td>$COLLECTED$</td></tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
				}

			}
			_unitOfWork.Commit();

			mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestbyIdd.ReqestNo).Replace("$TYPEREQUEST$", Type).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestbyIdd.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$RETURNEDDATE$", DateTime.Now.ToShortDateString()).Replace("$COLLECTED$", CollectedBy).Replace("$REASON$", ReasonforRejection);
			//   EmailViewModel emailViewModel = new EmailViewModel()
			//   {
			//     ToList = emailList,
			//     Subject = MailSubject,// "New Calibration Request Closed Notification",
			//     Body = mailbody,//"Hi " + labUserById.FirstName + " " + labUserById.LastName + ",<br/> Calibration Request( " + requestById.ReqestNo + ") rejected by " + fmUserById.FirstName + " " + fmUserById.LastName + ". Please login to your CMT account for more details about request. <br /> <b>Reason:</b>  " + RejectReason,
			//     IsHtml = true
			//   };
			//_emailService.SendEmailAsync(emailViewModel, true);
			//_emailService.EmailSendingFunction(fmUserById.Email,mailbody,MailSubject);

			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,
				LabResult = s.Result,
				InstrumentReturnedOn = s.InstrumentReturnedOn,
				CollectedBy = s.CollectedBy
			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			List<Uploads> UploadList = _unitOfWork.Repository<Uploads>().GetQueryAsNoTracking(g => g.RequestId == requestId).ToList();
			if (UploadList.Count > 0)
			{
				RequestById.MUTemplateFileName = UploadList.Where(w => w.RequestId == requestId).Select(q => q.FileName).SingleOrDefault();
			}
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "AcceptExternalCalibrationRequest"
			};
		}
	}

	// ----------------new-------------------------------------
	public ResponseViewModel<InstrumentViewModel> SubmitLABAdminUpdates(int requestId, int ObservationTemplate, int ObservationTemplateType, int MUTemplate, int CertificationTemplate)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			Request requestbyIdd = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestbyIdd.InstrumentId).SingleOrDefault();
			// InstrumentViewModel instrumentmodel = new InstrumentViewModel();
			// instrumentmodel.ObservationTemplate = ObservationTemplate;
			// instrumentmodel.ObservationType = ObservationTemplateType;
			// instrumentmodel.MUTemplate = MUTemplate;
			// instrumentmodel.CertificationTemplate = CertificationTemplate;

			instrumentById.ObservationTemplate = ObservationTemplate;
			instrumentById.ObservationType = ObservationTemplateType;
			instrumentById.MUTemplate = MUTemplate;
			instrumentById.CertificationTemplate = CertificationTemplate;
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			InstrumentViewModel fff = _mapper.Map<InstrumentViewModel>(instrumentById);
			// var ab = new InstrumentViewModel();
			// ab.ObservationTemplate =instrumentById.ObservationTemplate;
			// ab.ObservationType = instrumentById.ObservationType;
			// ab.MUTemplate = instrumentById.MUTemplate;
			// ab.CertificationTemplate  = instrumentById.CertificationTemplate;

			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = fff,
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
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "AcceptExternalCalibrationRequest"
			};

		}
	}
	public ResponseViewModel<RequestViewModel> SubmitNewRequest(int requestId, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = requestId;
			reqestStatus.StatusId = (Int32)EnumRequestStatus.Approved;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = userId;
			reqestStatus.Comment = "";
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);


			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			requestById.InstrumentCondition = InstrumentCondition;
			requestById.ReceivedBy = userId;
			requestById.Feasiblity = Feasiblity;
			requestById.TentativeCompletionDate = TentativeCompletionDate;
			requestById.ReceivedDate = DateTime.Now;
			requestById.StatusId = (Int32)EnumRequestStatus.Approved;
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();

			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
			instrumentById.ModifiedBy = userId;
			instrumentById.ModifiedOn = DateTime.Now;
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();


			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,


			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "AcceptExternalCalibrationRequest"
			};
		}
	}
	public ResponseViewModel<RequestViewModel> SubmitQuarantineRequest(int requestId, int userId)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			RequestStatus reqestStatus = new RequestStatus();
			reqestStatus.RequestId = requestId;
			reqestStatus.StatusId = (Int32)EnumRequestStatus.Approved;
			reqestStatus.CreatedOn = DateTime.Now;
			reqestStatus.CreatedBy = userId;
			reqestStatus.Comment = "";
			_unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);


			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			requestById.StatusId = (Int32)EnumRequestStatus.Approved;
			_unitOfWork.Repository<Request>().Update(requestById);
			_unitOfWork.SaveChanges();

			InstrumentQuarantine instrumentQuarantineById = _unitOfWork.Repository<InstrumentQuarantine>().GetQueryAsNoTracking(Q => Q.InstrumentId == requestById.InstrumentId).SingleOrDefault();
			instrumentQuarantineById.StatusId = 2;
			_unitOfWork.Repository<InstrumentQuarantine>().Update(instrumentQuarantineById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();


			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,


			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "AcceptExternalCalibrationRequest"
			};
		}
	}
	public ResponseViewModel<LovsViewModel> GetLovs(string attrType, string attrsubType)
	{
		try
		{
			List<LovsViewModel> lovsList = new List<LovsViewModel>();
			if (attrsubType != null && attrsubType != "")
			{
				lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == attrsubType).ToList());
			}
			else
			{
				lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == attrType).ToList());
			}


			return new ResponseViewModel<LovsViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = lovsList
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<LovsViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "AcceptExternalCalibrationRequest"
			};
		}

	}
	public class SmtpSettings
	{
		public string Server = "53.151.100.102";
		public string Port = "25";
		public string FromAddress = "DICV-CMT@DAIMLER.COM";
		public string UserId = "DICV-EBOM@DAIMLER.COM";
		public string Pwd = "Dicv@123";
		public bool IsDevelopmentMode = true;
	}


	public ResponseViewModel<RequestViewModel> SaveInstrumentData(int requestId, string newLabId,
				bool newNABL,
				int newObservation,
				int newObservationType,
				int newMU,
				int newCertification,
				int masterInstrument1,
				int masterInstrument2,
				int masterInstrument3,
				int masterInstrument4,
				string calibSource,
				string standardReffered,
				DateTime calibDate,
				DateTime dueDate,
				DateTime dateOfReceipt)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
			Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
			instrumentById.IsNABL = newNABL;
			instrumentById.IdNo = newLabId;
			instrumentById.ObservationTemplate = newObservation;
			instrumentById.ObservationType = newObservationType;
			instrumentById.MUTemplate = newMU;
			instrumentById.CertificationTemplate = newCertification;
			instrumentById.MasterInstrument1 = masterInstrument1;
			instrumentById.MasterInstrument2 = masterInstrument2;
			instrumentById.MasterInstrument3 = masterInstrument3;
			instrumentById.MasterInstrument4 = masterInstrument4;
			instrumentById.CalibSource = calibSource;
			instrumentById.StandardReffered = standardReffered;
			instrumentById.CalibDate = calibDate;
			instrumentById.DueDate = dueDate;
			instrumentById.DateOfReceipt = dateOfReceipt;
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();


			RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
			{
				Id = s.Id,
				ReqestNo = s.ReqestNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				InstrumentSerialNumber = s.InstrumentModel.SlNo,
				CalibDate = s.InstrumentModel.CalibDate,
				DueDate = s.InstrumentModel.DueDate,
				UserDept = s.InstrumentModel.UserDept,
				SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
				RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
				ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
				RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
				Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
				DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
				RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
				ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
				ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
				ReceivedBy = s.ReceivedBy,
				InstrumentCondition = s.InstrumentCondition,
				Feasiblity = s.Feasiblity,
				TentativeCompletionDate = s.TentativeCompletionDate,
				ReceivedDate = s.ReceivedDate,
				MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
				MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
				MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
				MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
				CalibSource = s.InstrumentModel.CalibSource,
				StandardReffered = s.InstrumentModel.StandardReffered,
				DateOfReceipt = s.InstrumentModel.DateOfReceipt,
				IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
				ObservationTemplate = s.InstrumentModel.ObservationTemplate,
				ObservationType = s.InstrumentModel.ObservationType,
				MUTemplate = s.InstrumentModel.MUTemplate,
				CertificationTemplate = s.InstrumentModel.CertificationTemplate,


			}).SingleOrDefault();
			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = RequestById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			return new ResponseViewModel<RequestViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ExternalCalibrationRequest",
				ResponseServiceMethod = "AcceptExternalCalibrationRequest"
			};
		}
	}
}