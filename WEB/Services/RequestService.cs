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
using System.Linq;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.ComponentModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;

namespace WEB.Services;

public class RequestService : IRequestService
{
	private readonly IMapper _mapper;
	private IHttpContextAccessor _contextAccessor { get; set; }
	private IUnitOfWork _unitOfWork { get; set; }
	private IEmailService _emailService;
	private IConfiguration _configuration;
	private IInstrumentService _instrumentService { get; set; }
	private IUtilityService _utilityService;
	public RequestService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IEmailService emailService, IConfiguration Configuration, IInstrumentService instrumentService, IUtilityService utilityService)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_contextAccessor = contextAccessor;
		_emailService = emailService;
		_configuration = Configuration;
		_instrumentService = instrumentService;
		_utilityService = utilityService;
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
			//UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
			List<RequestViewModel> RequestList = new List<RequestViewModel>();
            CMTDL _cmtdl = new CMTDL(_configuration);
            DataSet ds = _cmtdl.GetRequestList(userId, userRoleId);
            //List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					RequestViewModel REQlist = new RequestViewModel
					{
						Id = Convert.ToInt32(dr["RequestId"]),
						ReqestNo = dr["ReqestNo"].ToString(),
						InstrumentName = dr["InstrumentName"].ToString(),
						InstrumentId = Convert.ToInt32(dr["InstruementId"]),
						InstrumentIdNo = dr["IdNo"].ToString(),
						Range = dr["Range"].ToString(),
						InstrumentSerialNumber = dr["SlNo"].ToString(),
						RequestDate = Convert.ToDateTime(dr["RequestDate"]),
						TypeOfRequest = Convert.ToInt32(dr["TypeOfReqest"]),
						Status = Convert.ToInt16(dr["StatusId"]),
						UserDept = Convert.ToInt16(dr["UserDept"]),
						CertificationTemplate = Convert.ToInt16(dr["CertificationTemplate"]),
						UserRoleId = userRoleId,
						SubSectionCode = dr["SubSectionCode"].ToString(),
						TypeOfEquipment = dr["TypeOfEquipment"].ToString(),


					};
					RequestList.Add(REQlist);

				}
			}



			#region MyRegion
			/*
			
			if (userRoleId == 2)
			{
				RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking()
				.Include(I => I.InstrumentModel).Where(t => t.InstrumentModel.ActiveStatus == Convert.ToBoolean(1)).Include(I => I.RequestStatusModel)
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
					//CalibDate = s.InstrumentModel.CalibDate,
					//DueDate = s.InstrumentModel.DueDate,
					UserDept = s.InstrumentModel.UserDept,
					//SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
					//RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
					Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
					//ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
					//ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
					//RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
					//Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
					Status = s.StatusId,
					//ReceivedBy = s.ReceivedBy,
					//InstrumentCondition = s.InstrumentCondition,
					//Feasiblity = s.Feasiblity,
					//TentativeCompletionDate = s.TentativeCompletionDate,
					//ReceivedDate = s.ReceivedDate,
					//IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
					//ObservationTemplate = s.InstrumentModel.ObservationTemplate,
					//ObservationType = s.InstrumentModel.ObservationType,
					//MUTemplate = s.InstrumentModel.MUTemplate,
					CertificationTemplate = s.InstrumentModel.CertificationTemplate,
					UserRoleId = userRoleId,
					LabResult = s.Result
				}).ToList();
			}
            else if (userRoleId == 4)
			{
				RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(x => x.StatusId== (int)EnumRequestStatus.Approved)
				.Include(I => I.InstrumentModel).Where(t => t.InstrumentModel.ActiveStatus == Convert.ToBoolean(1))
				.Include(I => I.RequestStatusModel).Where(t => t.StatusId == (int)EnumRequestStatus.Approved)
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
                    //CalibDate = s.InstrumentModel.CalibDate,
                    //DueDate = s.InstrumentModel.DueDate,
                    UserDept = s.InstrumentModel.UserDept,
                    //SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
                    //RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
                    Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.Comment).FirstOrDefault(),
      //              ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.CreatedOn).FirstOrDefault(),
      //              ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.CreatedOn).FirstOrDefault(),
      //              RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
					 //Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId ).FirstOrDefault(),
                    //Status = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.StatusId).FirstOrDefault(),
                    Status = s.StatusId,
                    //ReceivedBy = s.ReceivedBy,
                    //InstrumentCondition = s.InstrumentCondition,
                    //Feasiblity = s.Feasiblity,
                    //TentativeCompletionDate = s.TentativeCompletionDate,
                    //ReceivedDate = s.ReceivedDate,
                    //IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
                    //ObservationTemplate = s.InstrumentModel.ObservationTemplate,
                    //ObservationType = s.InstrumentModel.ObservationType,
                    //MUTemplate = s.InstrumentModel.MUTemplate,
                    CertificationTemplate = s.InstrumentModel.CertificationTemplate,
                    UserRoleId = userRoleId,
                    LabResult = s.Result
                }).ToList();
            }
            else
			{
				RequestList = _unitOfWork.Repository<Request>()
									   .GetQueryAsNoTracking(x => x.CreatedBy == userId || x.LabL4 == userId || x.UserL4 == userId)
									   .Include(I => I.InstrumentModel).Where(t => t.InstrumentModel.ActiveStatus == Convert.ToBoolean(1))
									   .Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
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
										   //CalibDate = s.InstrumentModel.CalibDate,
										   //DueDate = s.InstrumentModel.DueDate,
										   UserDept = s.InstrumentModel.UserDept,
										   //SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
										   //RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
										   Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
										   //ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
										   //ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
										   //RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
										   //Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
                                           Status = s.StatusId,
             //                              ReceivedBy = s.ReceivedBy,
										   //InstrumentCondition = s.InstrumentCondition,
										   //Feasiblity = s.Feasiblity,
										   //TentativeCompletionDate = s.TentativeCompletionDate,
										   //ReceivedDate = s.ReceivedDate,
										   //IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
										   //ObservationTemplate = s.InstrumentModel.ObservationTemplate,
										   //ObservationType = s.InstrumentModel.ObservationType,
										   //MUTemplate = s.InstrumentModel.MUTemplate,
										   CertificationTemplate = s.InstrumentModel.CertificationTemplate,
										   UserRoleId = userRoleId,
										   LabResult = s.Result
									   }).ToList();
			}
			*/
			#endregion
			if (RequestList.Any())
			{
				var templateObservationList = GetTemplateObservations();
				int? reviewStatus = 0;
				int? ExObsreviewStatus = 0;
				foreach (var data in RequestList)
				{
					var observationData = templateObservationList.Where(x => x.InstrumentId == data.InstrumentId
																	  && x.RequestId == data.Id)
														   .FirstOrDefault();
					if (observationData != null)
                    {
                        reviewStatus = observationData.ReviewStatus;
                        ExObsreviewStatus = observationData.ExternalObsStatus;
                    }
                    else
                    {
                        reviewStatus = 0;
                        ExObsreviewStatus = 0;
                    }
						

					data.TemplateReviewStatus = reviewStatus == null ? 0 : reviewStatus;
                    data.ExObsTemplateReviewStatus = ExObsreviewStatus == null ? 0 : ExObsreviewStatus;

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
			ErrorViewModelTest.Log("RequestService - GetAllRequestList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
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
				//IdNo = s.InstrumentModel.IdNo,
				InstrumentName = s.InstrumentModel.InstrumentName,
				InstrumentIdNo = s.InstrumentModel.IdNo,
				InstrumentId = s.InstrumentId,
				RequestDate = s.RequestDate,
				TypeOfRequest = s.TypeOfReqest,
				Range = s.InstrumentModel.Range,
				LC = s.InstrumentModel.LC,
				Unit1 = s.InstrumentModel.Unit1,
				Unit2 = s.InstrumentModel.Unit2,
				AmountJPY = s.InstrumentModel.AmountJPY,
				Instrument_Type = s.InstrumentModel.Instrument_Type,
				Rule_Confirmity = s.InstrumentModel.Rule_Confirmity,
				EquipmentStation = s.InstrumentModel.EquipmentStation,
				Capacity = s.InstrumentModel.Capacity,
				Make = s.InstrumentModel.Make,
				CalibFreq = s.InstrumentModel.CalibFreq,
				Comment = s.InstrumentModel.Comment,
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
                AcceptReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved).Select(S => S.Comment).FirstOrDefault(),
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
				IsUserL4Accepted = s.IsUserL4Accepted,
                TypeOfEquipment = s.InstrumentModel.TypeOfEquipment,
                ToolInventory = s.InstrumentModel.ToolInventory
            }).SingleOrDefault();

			if (RequestById.ReceivedBy != null)
			{
				UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
				RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
			}
			if (RequestById.Status == 27 || RequestById.Status == 30)
			{
				List<TemplateObservation> TemplateObservationList = _unitOfWork.Repository<TemplateObservation>().GetQueryAsNoTracking(g => g.RequestId == RequestById.Id).ToList();
				RequestById.ReviewedStatus = TemplateObservationList.Where(w => w.RequestId == RequestById.Id).Select(q => q.ReviewStatus).FirstOrDefault();
				RequestById.AdminReviewStatus = TemplateObservationList.Where(w => w.RequestId == RequestById.Id).Select(q => q.ExternalObsStatus).FirstOrDefault();
			}
			//List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
			//RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
			//RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
			//RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
			//RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

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
            RequestById.LovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "ObservationType").ToList());
            instrumentEmptyViewModel.MasterData = _mapper.Map<List<MasterViewModel>>(_unitOfWork.Repository<Master>().GetQueryAsNoTracking().ToList());
            var CFreq = RequestById.CalibFreq;
            RequestById.CalibFrequency = lovsListFrquency.Where(W => W.AttrName == "CalibrationFreq" && W.Id == CFreq).Select(x => x.AttrValue).SingleOrDefault();
            List<Uploads> UploadList = _unitOfWork.Repository<Uploads>().GetQueryAsNoTracking(g => g.RequestId == RequestId).ToList();
            if (UploadList.Count > 0)
            {
                RequestById.MUTemplateFileName = UploadList.Where(w => w.RequestId == RequestId).Select(q => q.FileName).Take(1).SingleOrDefault();
                RequestById.SignImageName = UploadList.Where(w => w.RequestId == RequestId && w.TemplateType == "EX-AP").Select(q => q.FileName).Take(1).SingleOrDefault();
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
            CMTDL _cmtdl = new CMTDL(_configuration);
            User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
            User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.DeptCordShortId).FirstOrDefault();
            //User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.ForemanShortId && Q.DepartmentId == userById.DepartmentId).FirstOrDefault();
            //User LabuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Level == "L4" && Q.DepartmentId == 66).FirstOrDefault();
            User LabuserByL4Id = _cmtdl.GetCalibrationLabUsers();
            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrumentId).SingleOrDefault();

            if (instrumentById.ToolInventory == "Yes" && instrumentById.ToolInventoryStatus == 1)
            {
                instrumentById.ToolInventoryStatus = 2;

            }
            _unitOfWork.Repository<Instrument>().Update(instrumentById);
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
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
            //List<UserViewModel> fmUserById = _mapper.Map<List<UserViewModel>>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2 && Q.Level != "L4").ToList());
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            List<UserViewModel> fmUserById = _cmtdl.GetLadAdminUsers();
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
			ErrorViewModelTest.Log("RequestService - InsertRequest Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			_unitOfWork.RollBack();
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
			ErrorViewModelTest.Log("RequestService - UpdateRequest Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
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
			ErrorViewModelTest.Log("RequestService - DeleteRequest Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
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
	public ResponseViewModel<RequestViewModel> AcceptRequest(int requestId, int userId, string InstrumentCondition, string Scope, DateTime TentativeCompletionDate, int CalibFreq, string ToolInventory, int newObservation, int newObservationType, int newMU, int newCertification, string standardReffered, bool newNABL, int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4, DateTime DueDate)
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
            requestById.Feasiblity = "";// Feasiblity;

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
            instrumentById.CalibFreq = CalibFreq;
            instrumentById.ToolInventory = ToolInventory;
            instrumentById.ObservationTemplate = newObservation;
            instrumentById.ObservationType = newObservationType;
            instrumentById.MUTemplate = newMU;
            instrumentById.CertificationTemplate = newCertification;
            instrumentById.StandardReffered = standardReffered;
            instrumentById.MasterInstrument1 = MasterInstrument1;
            instrumentById.MasterInstrument2 = MasterInstrument2;
            instrumentById.MasterInstrument3 = MasterInstrument3;
            instrumentById.MasterInstrument4 = MasterInstrument4;
            instrumentById.DueDate = DueDate;
            if (instrumentById.ToolInventory != null && instrumentById.ToolInventory == "Yes")
            {
                instrumentById.ToolInventoryStatus = (Int32)ToolInventoryStatus.AcceptTool;
            }
            _unitOfWork.Repository<Instrument>().Update(instrumentById);
			//To Update ToolInventory Status
			if (instrumentById.ToolInventory != null && instrumentById.ToolInventory == "Yes")
			{
				instrumentById.ToolInventoryStatus = (Int32)ToolInventoryStatus.AcceptTool;
			}
			
			_unitOfWork.Repository<Instrument>().Update(instrumentById);
            _unitOfWork.SaveChanges();


            _unitOfWork.Commit();
            long UserId = requestById.CreatedBy;
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
            //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            //List<string> emailList = new List<string>();
            //emailList.Add(fmUserById.Email);
            CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
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





            //RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
            //{
            //	Id = s.Id,
            //	ReqestNo = s.ReqestNo,
            //	InstrumentName = s.InstrumentModel.InstrumentName,
            //	InstrumentIdNo = s.InstrumentModel.IdNo,
            //	InstrumentId = s.InstrumentId,
            //	RequestDate = s.RequestDate,
            //	TypeOfRequest = s.TypeOfReqest,
            //	Range = s.InstrumentModel.Range,
            //	InstrumentSerialNumber = s.InstrumentModel.SlNo,
            //	CalibDate = s.InstrumentModel.CalibDate,
            //	DueDate = s.InstrumentModel.DueDate,
            //	UserDept = s.InstrumentModel.UserDept,
            //	SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
            //	RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
            //	Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
            //	ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
            //	ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
            //	RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
            //	ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
            //	Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
            //	DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
            //	RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
            //	ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
            //	ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
            //	ReceivedBy = s.ReceivedBy,
            //	InstrumentCondition = s.InstrumentCondition,
            //	Feasiblity = s.Feasiblity,
            //	TentativeCompletionDate = s.TentativeCompletionDate,
            //	ReceivedDate = s.ReceivedDate,
            //	MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
            //	MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
            //	MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
            //	MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
            //	CalibSource = s.InstrumentModel.CalibSource,
            //	StandardReffered = s.InstrumentModel.StandardReffered,
            //	DateOfReceipt = s.InstrumentModel.DateOfReceipt,
            //	IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
            //	ObservationTemplate = s.InstrumentModel.ObservationTemplate,
            //	ObservationType = s.InstrumentModel.ObservationType,
            //	MUTemplate = s.InstrumentModel.MUTemplate,
            //	CertificationTemplate = s.InstrumentModel.CertificationTemplate,



            //}).SingleOrDefault();
            //if (RequestById.ReceivedBy != null)
            //{
            //	UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
            //	RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
            //}
            //List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
            //RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
            //RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
            //RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
            //RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

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
            ErrorViewModelTest.Log("RequestService - AcceptRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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
            ErrorViewModelTest.Log("RequestService - AcceptRequestRecalibration Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
    public ResponseViewModel<RequestViewModel> RejectRequest(int requestId, string RejectReason, int userId, string InstrumentCondition, string Scope, string ToolInventory, DateTime TentativeCompletionDate, string standardReffered)
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
            //requestById.Feasiblity = Feasiblity;
            requestById.TentativeCompletionDate = TentativeCompletionDate;
            requestById.ReceivedDate = DateTime.Now;
            _unitOfWork.Repository<Request>().Update(requestById);
            _unitOfWork.SaveChanges();

            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
            instrumentById.StandardReffered = standardReffered;
            instrumentById.ToolInventory = ToolInventory;
			//To Update ToolInventory Status
			if (instrumentById.ToolInventory != null && instrumentById.ToolInventory == "Yes")
			{
				instrumentById.ToolInventoryStatus = (Int32)ToolInventoryStatus.RejectedTool;
			}

			_unitOfWork.Repository<Instrument>().Update(instrumentById);
            _unitOfWork.SaveChanges();

            _unitOfWork.Commit();
            long UserId = requestById.CreatedBy;
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
            //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
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

            //RequestViewModel RequestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).Select(s => new RequestViewModel()
            //{
            //	Id = s.Id,
            //	ReqestNo = s.ReqestNo,
            //	InstrumentName = s.InstrumentModel.InstrumentName,
            //	InstrumentIdNo = s.InstrumentModel.IdNo,
            //	InstrumentId = s.InstrumentId,
            //	RequestDate = s.RequestDate,
            //	TypeOfRequest = s.TypeOfReqest,
            //	Range = s.InstrumentModel.Range,
            //	InstrumentSerialNumber = s.InstrumentModel.SlNo,
            //	CalibDate = s.InstrumentModel.CalibDate,
            //	DueDate = s.InstrumentModel.DueDate,
            //	UserDept = s.InstrumentModel.UserDept,
            //	SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved || W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
            //	RecordBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
            //	Result = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
            //	ClosedDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.CreatedOn).FirstOrDefault(),
            //	ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.CreatedOn).FirstOrDefault(),
            //	RecodedByLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
            //	ReqestBy = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Requested).Select(S => S.UserModel.FirstName + " " + S.UserModel.LastName).FirstOrDefault(),
            //	Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn).Select(S => S.StatusId).FirstOrDefault(),
            //	DepartmentName = s.InstrumentModel.DepartmenttModel.Name,
            //	RejectReason = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Rejected).Select(S => S.Comment).FirstOrDefault(),
            //	ResultLAB = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent).Select(S => S.Comment).FirstOrDefault(),
            //	ResultDEP = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Closed).Select(S => S.Comment).FirstOrDefault(),
            //	ReceivedBy = s.ReceivedBy,
            //	InstrumentCondition = s.InstrumentCondition,
            //	Feasiblity = s.Feasiblity,
            //	TentativeCompletionDate = s.TentativeCompletionDate,
            //	ReceivedDate = s.ReceivedDate,
            //	MasterInstrument1 = s.InstrumentModel.MasterInstrument1,
            //	MasterInstrument2 = s.InstrumentModel.MasterInstrument2,
            //	MasterInstrument3 = s.InstrumentModel.MasterInstrument3,
            //	MasterInstrument4 = s.InstrumentModel.MasterInstrument4,
            //	CalibSource = s.InstrumentModel.CalibSource,
            //	StandardReffered = s.InstrumentModel.StandardReffered,
            //	DateOfReceipt = s.InstrumentModel.DateOfReceipt,
            //	IsNABL = s.InstrumentModel.IsNABL == null ? false : s.InstrumentModel.IsNABL,
            //	ObservationTemplate = s.InstrumentModel.ObservationTemplate,
            //	ObservationType = s.InstrumentModel.ObservationType,
            //	MUTemplate = s.InstrumentModel.MUTemplate,
            //	CertificationTemplate = s.InstrumentModel.CertificationTemplate,

            //}).SingleOrDefault();
            //if (RequestById.ReceivedBy != null)
            //{
            //	UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
            //	RequestById.ReceivedByName = ReceivedUserById.FirstName + " " + ReceivedUserById.LastName;
            //}
            //List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.Id == RequestById.MasterInstrument1 || g.Id == RequestById.MasterInstrument2 || g.Id == RequestById.MasterInstrument3 || g.Id == RequestById.MasterInstrument4).ToList();
            //RequestById.MasterInstrumentName1 = MasterList.Where(w => w.Id == RequestById.MasterInstrument1).Select(q => q.EquipName).SingleOrDefault();
            //RequestById.MasterInstrumentName2 = MasterList.Where(w => w.Id == RequestById.MasterInstrument2).Select(q => q.EquipName).SingleOrDefault();
            //RequestById.MasterInstrumentName3 = MasterList.Where(w => w.Id == RequestById.MasterInstrument3).Select(q => q.EquipName).SingleOrDefault();
            //RequestById.MasterInstrumentName4 = MasterList.Where(w => w.Id == RequestById.MasterInstrument4).Select(q => q.EquipName).SingleOrDefault();

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
            ErrorViewModelTest.Log("RequestService - RejectRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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
    public ResponseViewModel<RequestViewModel> SubmitDepartmentRequestVisual(int requestId, string Result, int userId, string CollectedBy, string InstrumentIdNo, DateTime DueDate)
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

            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
            instrumentById.IdNo = InstrumentIdNo;
            instrumentById.DueDate = DueDate;
			//To Update ToolInventory Status
			if (instrumentById.ToolInventory != null && instrumentById.ToolInventory == "Yes")
			{
				instrumentById.ToolInventoryStatus = (Int32)ToolInventoryStatus.ClosedTool;
				instrumentById.ToolRoomStatus = (Int32)ToolRoomStatus.Pending;
				instrumentById.ReplacementLabID = null;
			}


			_unitOfWork.Repository<Instrument>().Update(instrumentById);
            _unitOfWork.SaveChanges();
           
            _unitOfWork.Commit();
            //Email Service
            long UserId = requestById.CreatedBy;
           
            CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
            string RequestType = string.Empty;
            string mailbody = string.Empty;
            string mailSubject = string.Empty;
            if (requestById.TypeOfReqest == 1)
            {
                RequestType = "New";
                mailSubject = "New calibration request / 新規計量器登録完了の件" + requestById.ReqestNo + "-closed";
                mailbody = "\r\n<!DOCTYPE html>  \r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>     \r\n\t<meta charset=\"utf-8\">  \r\n\t<meta name=\"viewport\" content=\"width=device-width\">     \r\n\t<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">     \r\n\t<meta name=\"x-apple-disable-message-reformatting\">   \r\n\t<title></title> <!--[if mso]><style>*{font-family:sans-serif !important}</style><![endif]-->    \r\n\t<style>         \r\n\t\thtml, body {              \r\n\t\t\tmargin: 0 auto !important;             \r\n\t\t\tpadding: 0 !important;              \r\n\t\t\theight: 100% !important;              \r\n\t\t\twidth: 100% !important          }          \r\n\t\t\tp {              font-size: 11px          }     \r\n\t\t\t* {              -ms-text-size-adjust: 100%;              -webkit-text-size-adjust: 100%          }\r\n\t\t\tdiv[style*=\"margin: 16px 0\"] {              margin: 0 !important          }        \r\n\t\t\ttable, td {              mso-table-lspace: 0pt !important;              mso-table-rspace: 0pt !important          }  \r\n\t\t\ttable {              border-spacing: 0 !important;              border-collapse: collapse !important;              table-layout: fixed !important;              margin: 0 auto !important          }     \r\n\t\t\ttable table table {                  table-layout: auto              }            a {              text-decoration: none          } \r\n\t\t\timg {              -ms-interpolation-mode: bicubic          }            *[x-apple-data-detectors], .unstyle-auto-detected-links *, .aBn {              border-bottom: 0 !important;              cursor: default !important;              color: inherit !important;              text-decoration: none !important;              font-size: inherit !important;              font-family: inherit !important;              font-weight: inherit !important;              line-height: inherit !important          }            .a6S {              display: none !important;              opacity: 0.01 !important          }         \r\n\t\t\timg.g-img + div {              display: none !important          }           \r\n\t\t\t@media only screen and (min-device-width: 320px) and (max-device-width: 374px) {         \r\n\t\t\t\t.email-container {                  min-width: 320px !important              }          }  \r\n\t\t\t\t@media only screen and (min-device-width: 375px) and (max-device-width: 413px) {             \r\n\t\t\t\t\t.email-container {                  min-width: 375px !important              }          }       \r\n\t\t\t\t\t@media only screen and (min-device-width: 414px) {              \r\n\t\t\t\t\t\t.email-container {                  min-width: 414px !important              }          }      \r\n\t\t\t\t\t\t</style>    \r\n\t\t\t\t\t\t<style>        \r\n\t\t\t\t\t\t\t.button-td, .button-a {              transition: all 100ms ease-in          }     \r\n\t\t\t\t\t\t\t.button-td-primary:hover, \r\n\t\t\t\t\t\t\t.button-a-primary:hover {              background: #555 !important;              border-color: #555 !important          }   \r\n\t\t\t\t\t\t\t@media screen and (max-width: 600px) {             \r\n\t\t\t\t\t\t\t\t.email-container {                  width: 100% !important;                  margin: auto !important              }           \r\n\t\t\t\t\t\t\t\t.fluid {                  max-width: 100% !important;                  height: auto !important;                  margin-left: auto !important;                  margin-right: auto !important              }              \r\n\t\t\t\t\t\t\t\t.stack-column, .stack-column-center {                  display: block !important;                  width: 100% !important;                  max-width: 100% !important;                  direction: ltr !important              }                .stack-column-center {                  text-align: center !important              }                .center-on-narrow {                  text-align: center !important;                  display: block !important;                  margin-left: auto !important;                  margin-right: auto !important;                  float: none !important              }                table.center-on-narrow {                  display: inline-block !important              }                .email-container p {                  font-size: 20px !important              }          }      \r\n\t\t</style><!--[if gte mso 9]>\r\n\r\n\t\t<xml> <o:OfficeDocumentSettings> <o:AllowPNG/> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> <![endif]--> \r\n\t\t</head>  \r\n\t\t<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;\">      \r\n\t\t\t<center style=\"width: 100%; background-color: #f1f1f1;\">          <!--[if mso | IE]><table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color: #f1f1f1;\"><tr><td> <![endif]-->          <span style=\"font-size: 18px\">英語版は日本語に続きます。</span>     \r\n<table align=\"center\" role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"600\" style=\"margin: 0 auto;\" class=\"email-container\">\r\n\t\t\t\t\t\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">Dear User,\r\n</h1></table></td></tr>\r\n\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">New calibration request has been closed by lab\r\n</h1></table></td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Request no:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNO$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Request type:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>New</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Instrument name:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$INSTRUMENTNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Requested by:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Date:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$DATE$</span></p></td></tr></table>\r\n</td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Instrument return on:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$DUEDATE$</span></p></td></tr></table>\r\n</td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">User</p></td></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">$REQNAME$</p></td></table></td></tr>\r\n\r\n \r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p></p></table></td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p></p></table></td></tr>\r\n\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n\r\n\r\n\r\n<br />  \r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">計量管理部門の皆様\r\n\r\n</h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">新規測定器登録を作成しました。<span>\r\n</span></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">依頼№:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNO$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">種類　:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>新規</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">計量器名:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$INSTRUMENTNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">に要求された:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">日付:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$DATE$</span></p></td></tr></table>\r\n</td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">機器のリターンオン:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$DUEDATE$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">使用部門:</p></td></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">$REQNAME$</p></td></table></td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n\r\n</table>\r\n</td></tr>\r\n</table>\r\n</table>  \r\n <!--[if mso | IE]></td></tr></table> <![endif]-->      </center>      <br /> \r\n\t\t\r\n\t\t\r\n\t\t</body>  </html>";
                mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestById.ReqestNo).Replace("$TYPEREQUEST$", Convert.ToString(requestById.TypeOfReqest)).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestById.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$DUEDATE$", Convert.ToString(requestById.InstrumentReturnedOn));

            }
			_emailService.EmailSendingFunction(labUserById.Email, mailbody, mailSubject);

			
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
            ErrorViewModelTest.Log("RequestService - SubmitDepartmentRequestVisual Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
            ErrorViewModelTest.Log("RequestService - SaveFiles Method");
            ErrorViewModelTest.Log("exception - " + ex.Message);
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
            ErrorViewModelTest.Log("RequestService - SaveQRFile Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return false;
            //throw e;
        }

    }
    public ResponseViewModel<RequestViewModel> SubmitLABRequestVisual(int requestId, string Result, int userId, string RecordBy, DateTime ClosedDate, string Remarks, DateTime InstrumentReturnedDate, string CollectedBy, string ReasonforRejection, string IsFeasibleService, string IsFeasibleYes, string ServiceResponsibility, int RequestType, int InstrumentId, DateTime CalibDate, DateTime DueDate, bool newNABL, List<UploadFile> FileData, string IdNo)
    {
        try
        {
            CMTDL _cmtdl = new CMTDL(_configuration);
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
            //To Update ToolInventory Status
            if (instrumentById.ToolInventory != null && instrumentById.ToolInventory == "Yes")
            {
                instrumentById.ToolInventoryStatus = (Int32)ToolInventoryStatus.SentTool;
            }
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
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
            //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            //CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
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
                UserViewModel ReceivedUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(RequestById.ReceivedBy));
                //UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
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
            ErrorViewModelTest.Log("RequestService - SubmitLABRequestVisual Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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
            ErrorViewModelTest.Log("RequestService - SubmitLABAdminUpdates Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
            //To Update ToolInventory Status
            if (instrumentById.ToolInventory != null && instrumentById.ToolInventory == "Yes")
            {
                instrumentById.ToolInventoryStatus = (Int32)ToolInventoryStatus.AcceptTool;
            }

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
                CMTDL _cmtdl = new CMTDL(_configuration);
                UserViewModel ReceivedUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(RequestById.ReceivedBy));
                //UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
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
            ErrorViewModelTest.Log("RequestService - SubmitNewRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
                CMTDL _cmtdl = new CMTDL(_configuration);
                UserViewModel ReceivedUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(RequestById.ReceivedBy));
                //UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
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
            ErrorViewModelTest.Log("RequestService - SubmitQuarantineRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
    public ResponseViewModel<LovsViewModel> GetLovs(string attrType, string attrsubType, string LangType)
    {
        try
        {
            List<LovsViewModel> lovsList = new List<LovsViewModel>();
            if (attrsubType != null && attrsubType != "")
            {
                if (LangType == "en")
                    lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == attrsubType && Q.IsActive == true).ToList());
                else
                    lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrformJp == attrsubType && Q.IsActive == true).ToList());
            }
            else
            {
                if (LangType == "en")
                    lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == attrType && Q.IsActive == true).ToList());
                else
                    lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrNameJp == attrType && Q.IsActive == true).ToList());
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
            ErrorViewModelTest.Log("RequestService - GetLovs Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
                CMTDL _cmtdl = new CMTDL(_configuration);
                UserViewModel ReceivedUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(RequestById.ReceivedBy));
                //UserViewModel ReceivedUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(RequestById.ReceivedBy)).SingleOrDefault());
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
            ErrorViewModelTest.Log("RequestService - SaveInstrumentData Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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


    public ResponseViewModel<RequestViewModel> ExternalRejectRequest(int requestId, string RejectReason, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string standardReffered)
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
            //requestById.InstrumentCondition = InstrumentCondition;
            requestById.ReceivedBy = userId;
           // requestById.Feasiblity = Feasiblity;
            requestById.TentativeCompletionDate = TentativeCompletionDate;
            requestById.ReceivedDate = DateTime.Now;
            _unitOfWork.Repository<Request>().Update(requestById);
            _unitOfWork.SaveChanges();

            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
            //instrumentById.StandardReffered = standardReffered;
            //_unitOfWork.Repository<Instrument>().Update(instrumentById);
            //_unitOfWork.SaveChanges();



            _unitOfWork.Commit();
            long UserId = requestById.CreatedBy;
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
            //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
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
            string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            External Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr><tr><td align='left'>Reason of Rejection</td><td>:</td>                <td>                    $Reason$                </td>            </tr></table>  <p><a href='http://s365id1qdg044' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestById.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestById.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$Reason$", RejectReason);
            var mailSubject = "External Calibration Request Reject Notification-" + requestById.ReqestNo + "";
            _emailService.EmailSendingFunction(fmUserById.Email, mailbody, mailSubject);

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
            ErrorViewModelTest.Log("RequestService - ExternalRejectRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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

    public ResponseViewModel<RequestViewModel> ExternalAcceptRequest(int requestId, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string InstrumentIdNo, string acceptReason, string ReceivedBy, IFormFile httpPostedFileBase, string StandardReffered, DateTime DueDate)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            RequestStatus reqestStatus = new RequestStatus();
            reqestStatus.RequestId = requestId;
            reqestStatus.StatusId = (Int32)EnumRequestStatus.Approved;
            reqestStatus.CreatedOn = DateTime.Now;
            reqestStatus.CreatedBy = userId;
            reqestStatus.Comment = acceptReason;
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
            instrumentById.DueDate = DueDate;
            instrumentById.StandardReffered = StandardReffered;
            _unitOfWork.Repository<Instrument>().Update(instrumentById);
            _unitOfWork.SaveChanges();


            if (httpPostedFileBase != null)
            {
                string filePath = _utilityService.UploadImage(httpPostedFileBase, "Instrument");
                Uploads upload = new Uploads()
                {
                    FileName = httpPostedFileBase.FileName,
                    FileGuid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    RequestId = requestId,
                    TemplateType = "EX-AP",
                    FilePath = filePath
                };
                _unitOfWork.Repository<Uploads>().Insert(upload);
                _unitOfWork.SaveChanges();
                InstrumentFileUpload instrumentFileUpload = new InstrumentFileUpload();
                instrumentFileUpload.InstrumentId = instrumentById.Id;
                instrumentFileUpload.UploadId = upload.Id;
                _unitOfWork.Repository<InstrumentFileUpload>().Insert(instrumentFileUpload);
                _unitOfWork.SaveChanges();
            }

            _unitOfWork.Commit();
            long UserId = requestById.CreatedBy;
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
            //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            //List<string> emailList = new List<string>();
            //emailList.Add(fmUserById.Email);
            CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
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

            string mailSubject = "New Calibration Request Accept Notification-" + requestById.ReqestNo + "";

            _emailService.EmailSendingFunction(fmUserById.Email, mailbody, mailSubject);

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
            ErrorViewModelTest.Log("RequestService - AcceptRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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


    public ResponseViewModel<RequestViewModel> InsertDueRequest(List<RequestAllView> reqlist, int userId)
    {
        DataTable dt = new DataTable();

        StringBuilder data = new StringBuilder("");
        data.Append("<Root>");
        foreach (var sd in reqlist)
        {
            data.Append("<RequestList>");
            data.Append(string.Format("<instrumentId>{0}</instrumentId>", sd.instrumentId));
            data.Append(string.Format("<TypeValue>{0}</TypeValue>", sd.TypeValue));
            data.Append("</RequestList>");
        }
        data.Append("</Root>");


        CMTDL _cmtdl = new CMTDL(_configuration);
        var status = _cmtdl.InsertDueRequest(data.ToString(), userId);


        //List<RequestAllView> recalibrationlist = reqlist.Where(r => r.TypeValue == 2).ToList();
        //dt = ListToDataTable(recalibrationlist);
        foreach (var rql in reqlist)
        {
            RequestMailList getrqlist = _cmtdl.GetDataForInstrumentRequest(rql.instrumentId, rql.TypeValue);
            if (getrqlist.EquipmentType == "Regular")
            {
                SendEmailRegular(getrqlist);
            }
            else if (getrqlist.EquipmentType == "Recalibration")
            {
                SendEmailRecalibration(getrqlist);
            }

        }

        return new ResponseViewModel<RequestViewModel>
        {
            ResponseCode = 500,
            ResponseMessage = "Failure",
            ErrorMessage = "",
            ResponseData = null,
            ResponseDataList = null,
            ResponseService = "CalibrationRequest",
            ResponseServiceMethod = "AcceptCalibrationRequest"
        };
    }

    public DataTable ListToDataTable(List<RequestAllView> data)
    {
        PropertyDescriptorCollection props =
            TypeDescriptor.GetProperties(typeof(RequestAllView));
        DataTable table = new DataTable();
        for (int i = 0; i < props.Count; i++)
        {
            PropertyDescriptor prop = props[i];
            table.Columns.Add(prop.Name, prop.PropertyType);
        }
        object[] values = new object[props.Count];
        foreach (RequestAllView item in data)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = props[i].GetValue(item);
            }
            table.Rows.Add(values);
        }
        return table;
    }


    public static void GenerateExcel(DataTable listdata, string path)
    {
        try
        {
            var memoryStream = new MemoryStream();

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet();

                List<String> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                int columnIndex = 0;

                foreach (System.Data.DataColumn column in listdata.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.CreateCell(columnIndex).SetCellValue(column.ColumnName);
                    columnIndex++;
                }

                int rowIndex = 1;
                foreach (DataRow dsrow in listdata.Rows)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    int cellIndex = 0;
                    foreach (String col in columns)
                    {
                        row.CreateCell(cellIndex).SetCellValue(dsrow[col].ToString());
                        cellIndex++;
                    }

                    rowIndex++;
                }
                workbook.Write(fs);
            }
        }
        catch (Exception ex)
        {
            ErrorViewModelTest.Log("CMT Email Service - GenerateExcel Method");
            ErrorViewModelTest.Log("exception - " + ex.Message);
        }
    }

    public ResponseViewModel<RequestViewModel> InsertDueRequestss(List<RequestAllView> reqlist, int userId)
    {
        try
        {

            _unitOfWork.BeginTransaction();
            foreach (var user in reqlist)
            {
                CMTDL _cmtdl = new CMTDL(_configuration);
                User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
                User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.DeptCordShortId).FirstOrDefault();
                //User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.ForemanShortId && Q.DepartmentId == userById.DepartmentId).FirstOrDefault();
                //User LabuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Level == "L4" && Q.DepartmentId == 66).FirstOrDefault();
                User LabuserByL4Id = _cmtdl.GetCalibrationLabUsers();
                Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == user.instrumentId).SingleOrDefault();

                if (instrumentById.ToolInventory == "Yes" && instrumentById.ToolInventoryStatus == 1)
                {
                    instrumentById.ToolInventoryStatus = 2;

                }
                _unitOfWork.Repository<Instrument>().Update(instrumentById);
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
                newRequest.InstrumentId = instrumentById.Id;
                newRequest.RequestDate = DateTime.Now;
                newRequest.TypeOfReqest = user.TypeValue;
                newRequest.CreatedBy = userId;
                if (user.TypeValue == 3)
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
            }
            _unitOfWork.Commit();

            #region commend in mail part
            /*
			string UserId = _contextAccessor.HttpContext.Session.GetString("LoggedId");
			//UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
			//List<UserViewModel> fmUserById = _mapper.Map<List<UserViewModel>>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2 && Q.Level != "L4").ToList());
			UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
			List<UserViewModel> fmUserById = _cmtdl.GetLadAdminUsers();
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
					//smtp.Send(message);
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
				//smtp.Send(message);
			}
			*/
            #endregion
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
            ErrorViewModelTest.Log("RequestService - InsertRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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
    public bool SendEmailRegular(RequestMailList reqlist) //, List<Attachments> att)
    {
        bool result = false;
        try
        {

            MailMessage mail = new MailMessage();
            //SmtpSettings smtpvalue = new SmtpSettings();
            //System.Net.Mail.Attachment attachment;


            //if (!string.IsNullOrEmpty(tomail))
            //{
            //    string[] toid = tomail.Split(',');
            //    for (int i = 0; i < toid.Length; i++)
            //    {
            //        //if (!toid[i].Contains("daimler"))
            //        //{
            //        mail.To.Add(toid[i]);
            //        //}

            //    }
            //}                

            //         string to = "karthicksm2096@gmail.com";
            //         mail.To.Add(new MailAddress(to));
            //         mail.Subject = "Next Month DueDate Case for User -" + reqlist.RequestNo;
            //         mail.IsBodyHtml = true;
            //         string mailbody  = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear lab Team,</p>   <p>Regular instrument calibration request has been created by <b>$USERNAME$</b></p>          <table>       <tr>         <th>Sr.No</th>         <th>Request No.</th>         <th>Lab Id</th>         <th>Equipment Type</th>         <th>Equipment Name</th>         <th>Sub Section Code</th>         <th>Calibration Type</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>    <p>User</p>   <p><b>$USERNAME$</b></p>         <p>計量管理部門の皆様</p>   <p>計量器定期検査依頼がありました。<b>$USERNAME$</b></p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>      <p>使用部門</p>   <p><b>$USERNAME$</b></p>   </body>  </html>";
            //         mailbody = mailbody.Replace("$USERNAME$", reqlist.CreaterFirstName + " " + reqlist.CreaterLastName).Replace("$S.No$", reqlist.SNo.ToString()).Replace("$RequestNo$", reqlist.RequestNo).Replace("$LabId$", reqlist.LabId).Replace("$EquType$", reqlist.EquipmentType).Replace("$EquName$", reqlist.EquipmentName).Replace("$Subcode$", reqlist.SubsectionCode).Replace("$CalibType$", reqlist.CalibrationType);
            //mail.Body = mailbody;
            //         //mail.Attachments.Add(new Attachment(path));
            //         mail.From = new MailAddress(FromAddress);
            //         mail.Priority = MailPriority.Normal;

            //         SmtpClient SMTPServer = new SmtpClient(Server, Convert.ToInt32(Port));
            //         SMTPServer.Credentials = new System.Net.NetworkCredential("karthicksm2096@gmail.com", "naclimdvhapbxazm");
            //         SMTPServer.EnableSsl = true;
            //         SMTPServer.UseDefaultCredentials = false;
            //         SMTPServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            //         SMTPServer.Send(mail);
            //         result = true;


            
            List<string> emailList = new List<string>();
			//string mailbody = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear lab Team,</p>   <p>Regular instrument calibration request has been created by <b>$USERNAME$</b></p> <table>"+content + "</table>    <p>User</p>   <p><b>$USERNAME$</b></p>         <p>計量管理部門の皆様</p>   <p>計量器定期検査依頼がありました。<b>$USERNAME$</b></p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>      <p>使用部門</p>   <p><b>$USERNAME$</b></p>   </body>  </html>";
			string mailbody = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear lab Team,</p>   <p>Regular instrument calibration request has been created by <b>$USERNAME$</b></p>          <table>       <tr>         <th>Sr.No</th>         <th>Request No.</th>         <th>Lab Id</th>         <th>Equipment Type</th>         <th>Equipment Name</th>         <th>Sub Section Code</th>         <th>Calibration Type</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>    <p>User</p>   <p><b>$USERNAME$</b></p>         <p>計量管理部門の皆様</p>   <p>計量器定期検査依頼がありました。<b>$USERNAME$</b></p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>      <p>使用部門</p>   <p><b>$USERNAME$</b></p>   </body>  </html>";
            mailbody = mailbody.Replace("$USERNAME$", reqlist.CreaterFirstName + " " + reqlist.CreaterLastName).Replace("$S.No$", reqlist.SNo.ToString()).Replace("$RequestNo$", reqlist.RequestNo).Replace("$LabId$", reqlist.LabId).Replace("$EquType$", reqlist.EquipmentType).Replace("$EquName$", reqlist.EquipmentName).Replace("$Subcode$", reqlist.SubsectionCode).Replace("$CalibType$", reqlist.CalibrationType);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            SmtpSettings smtpvalue = new SmtpSettings();
            message.From = new MailAddress(smtpvalue.FromAddress);
            message.To.Add(new MailAddress(reqlist.CreaterEmail.Trim()));
            //message.To.Add(new MailAddress(item.Email.Trim()));
            //message.To.Add("gurushev.p@daimlertruck.com");
            //message.Bcc.Add("mohammedashik.s@intelizign.com");  
            message.Subject = "Regular instrument calibration request";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = mailbody;
            smtp.Port = int.Parse(smtpvalue.Port);
            smtp.Host = smtpvalue.Server; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(smtpvalue.UserId, smtpvalue.Pwd);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
        catch (Exception ex)
        {
            ErrorViewModelTest.Log("Request Service - SendEmailRegular Method");
            ErrorViewModelTest.Log("exception - " + ex.Message);
            result = false;
        }
        return result;
    }
    public bool SendEmailRecalibration(RequestMailList reqlist) //, List<Attachments> att)
    {
        bool result = false;
        try
        {

            MailMessage mail = new MailMessage();
            //SmtpSettings smtpvalue = new SmtpSettings();
            //System.Net.Mail.Attachment attachment;


            //if (!string.IsNullOrEmpty(tomail))
            //{
            //    string[] toid = tomail.Split(',');
            //    for (int i = 0; i < toid.Length; i++)
            //    {
            //        //if (!toid[i].Contains("daimler"))
            //        //{
            //        mail.To.Add(toid[i]);
            //        //}

            //    }
            //}                

            //         string to = "karthicksm2096@gmail.com";
            //         mail.To.Add(new MailAddress(to));
            //         mail.Subject = "Next Month DueDate Case for User -" + reqlist.RequestNo;
            //         mail.IsBodyHtml = true;
            //         string mailbody  = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear lab Team,</p>   <p>Regular instrument calibration request has been created by <b>$USERNAME$</b></p>          <table>       <tr>         <th>Sr.No</th>         <th>Request No.</th>         <th>Lab Id</th>         <th>Equipment Type</th>         <th>Equipment Name</th>         <th>Sub Section Code</th>         <th>Calibration Type</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>    <p>User</p>   <p><b>$USERNAME$</b></p>         <p>計量管理部門の皆様</p>   <p>計量器定期検査依頼がありました。<b>$USERNAME$</b></p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>      <p>使用部門</p>   <p><b>$USERNAME$</b></p>   </body>  </html>";
            //         mailbody = mailbody.Replace("$USERNAME$", reqlist.CreaterFirstName + " " + reqlist.CreaterLastName).Replace("$S.No$", reqlist.SNo.ToString()).Replace("$RequestNo$", reqlist.RequestNo).Replace("$LabId$", reqlist.LabId).Replace("$EquType$", reqlist.EquipmentType).Replace("$EquName$", reqlist.EquipmentName).Replace("$Subcode$", reqlist.SubsectionCode).Replace("$CalibType$", reqlist.CalibrationType);
            //mail.Body = mailbody;
            //         //mail.Attachments.Add(new Attachment(path));
            //         mail.From = new MailAddress(FromAddress);
            //         mail.Priority = MailPriority.Normal;

            //         SmtpClient SMTPServer = new SmtpClient(Server, Convert.ToInt32(Port));
            //         SMTPServer.Credentials = new System.Net.NetworkCredential("karthicksm2096@gmail.com", "naclimdvhapbxazm");
            //         SMTPServer.EnableSsl = true;
            //         SMTPServer.UseDefaultCredentials = false;
            //         SMTPServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            //         SMTPServer.Send(mail);
            //         result = true;


            List<string> emailList = new List<string>();
            string mailbody = "<!DOCTYPE html> <html lang=\"en\">  <head>   <meta charset=\"UTF-8\" />   <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />   <title></title>   <style>     table,     th,     td {         border: 1px solid black;         border-collapse: collapse;     }     </style> </head>  <body>      <p>Dear lab Team,</p>   <p>Re calibration request has been created by <b>$USERNAME$</b></p>          <table>       <tr>         <th>Sr.No</th>         <th>Request No.</th>         <th>Lab Id</th>         <th>Equipment Type</th>         <th>Equipment Name</th>         <th>Sub Section Code</th>         <th>Calibration Type</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>    <p>User</p>   <p><b>$USERNAME$</b></p>         <p>計量管理部門の皆様</p>   <p>計量器臨時検査の依頼がありました。<b>$USERNAME$</b></p>      <table>       <tr>         <th>シリアル№</th>         <th>依頼№</th>         <th>計量器№</th>         <th>計量器タイプ</th>         <th>計量器名</th>         <th>部門コード</th>         <th>内部校正or外部校正</th>       </tr>       <tr>         <td>$S.No$</td>         <td>$RequestNo$</td>         <td>$LabId$</td>         <td>$EquType$</td>         <td>$EquName$</td>         <td>$Subcode$</td>         <td>$CalibType$</td>       </tr>     </table>      <p>使用部門</p>   <p><b>$USERNAME$</b></p>   </body>  </html>";
            mailbody = mailbody.Replace("$USERNAME$", reqlist.CreaterFirstName + " " + reqlist.CreaterLastName).Replace("$S.No$", reqlist.SNo.ToString()).Replace("$RequestNo$", reqlist.RequestNo).Replace("$LabId$", reqlist.LabId).Replace("$EquType$", reqlist.EquipmentType).Replace("$EquName$", reqlist.EquipmentName).Replace("$Subcode$", reqlist.SubsectionCode).Replace("$CalibType$", reqlist.CalibrationType);
            MailMessage message = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            SmtpSettings smtpvalue = new SmtpSettings();
            message.From = new MailAddress(smtpvalue.FromAddress);
            message.To.Add(new MailAddress(reqlist.CreaterEmail.Trim()));
            //message.To.Add(new MailAddress(item.Email.Trim()));
            //message.To.Add("gurushev.p@daimlertruck.com");
            //message.Bcc.Add("mohammedashik.s@intelizign.com");  
            message.Subject = "Re- calibration calibration request";
            message.IsBodyHtml = true; //to make message body as html  
            message.Body = mailbody;
            smtp.Port = int.Parse(smtpvalue.Port);
            smtp.Host = smtpvalue.Server; //for gmail host  
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(smtpvalue.UserId, smtpvalue.Pwd);
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        }
        catch (Exception ex)
        {
            ErrorViewModelTest.Log("Request Service - SendEmailRecalibration Method");
            ErrorViewModelTest.Log("exception - " + ex.Message);
            result = false;
        }
        return result;
    }

    public ResponseViewModel<RequestViewModel> DueInstrumentAdminApprove(List<DueInstrument> DueList, int userId)
    {
        DataTable dt = new DataTable();

        StringBuilder data = new StringBuilder("");
        data.Append("<Root>");
        foreach (var sd in DueList)
        {
            data.Append("<DueList>");
            data.Append(string.Format("<instrumentId>{0}</instrumentId>", sd.InstrumentId));
            data.Append(string.Format("<InstrumentName>{0}</InstrumentName>", sd.InstrumentName));
            data.Append(string.Format("<IdNo>{0}</IdNo>", sd.IdNo));
            data.Append(string.Format("<SubSectionCode>{0}</SubSectionCode>", sd.SubSectionCode));
            data.Append(string.Format("<TypeofScope>{0}</TypeofScope>", sd.TypeofScope));
            data.Append(string.Format("<DueDate>{0}</DueDate>", sd.DueDate));
            data.Append(string.Format("<DeptId>{0}</DeptId>", sd.DeptId));
            data.Append(string.Format("<InstrumentCreatedBy>{0}</InstrumentCreatedBy>", sd.InstrumentCreatedBy));
            data.Append("</DueList>");
        }
        data.Append("</Root>");

        CMTDL _cmtdl = new CMTDL(_configuration);
        var status = _cmtdl.InsertDueInstrumentList(data.ToString(), userId);

        return new ResponseViewModel<RequestViewModel>
        {
            ResponseCode = 200,
            ResponseMessage = "Success",
            ResponseData = null,
            ResponseDataList = null
        };
    }

    public ResponseViewModel<DueInstrument> GetAdminApproveInstrumentList()
    {
        try
        {
            List<DueInstrument> DuelList = new List<DueInstrument>();

            CMTDL _cmtdl = new CMTDL(_configuration);
            DataSet ds = _cmtdl.GetAdminApproveInstrumentList();
            //List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DueInstrument Instlist = new DueInstrument
                    {
                        IdNo = dr["IdNo"].ToString(),
                        InstrumentId = Convert.ToInt32(dr["InstrumentId"]),
                        EquipmentType = dr["EquipmentType"].ToString(),
                        InstrumentName = dr["EquipmentName"].ToString(),
                        Class = dr["Class"].ToString(),
                        Location = dr["Class"].ToString(),
                        SubSectionCode = dr["SubSectionCode"].ToString(),
                        SectionName = dr["SectionName"].ToString(),
                        TypeofScope = dr["CalibrationType"].ToString(),
                        DueDate = Convert.ToDateTime(dr["DueDate"]),
                        //ToolRoom = dr["ToolRoom"].ToString(),
                        DeptId = Convert.ToInt32(dr["DeptId"]),
                    };
                    DuelList.Add(Instlist);

                }
            }

            return new ResponseViewModel<DueInstrument>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = DuelList
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("CMTDL - GetAllDueInstrumentList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return null;
        }


    }

    public ResponseViewModel<RequestViewModel> DueInstrumentManagerApprove(List<DueInstrument> DueList, int userId)
    {
        DataTable dt = new DataTable();

        StringBuilder data = new StringBuilder("");
        data.Append("<Root>");
        foreach (var sd in DueList)
        {
            data.Append("<DueList>");
            data.Append(string.Format("<instrumentId>{0}</instrumentId>", sd.InstrumentId));
            //data.Append(string.Format("<InstrumentName>{0}</InstrumentName>", sd.InstrumentName));
            //data.Append(string.Format("<IdNo>{0}</IdNo>", sd.IdNo));
            //data.Append(string.Format("<SubSectionCode>{0}</SubSectionCode>", sd.SubSectionCode));
            //data.Append(string.Format("<TypeofScope>{0}</TypeofScope>", sd.TypeofScope));
            //data.Append(string.Format("<DueDate>{0}</DueDate>", sd.DueDate));
            //data.Append(string.Format("<DeptId>{0}</DeptId>", sd.DeptId));
            data.Append("</DueList>");
        }
        data.Append("</Root>");

        CMTDL _cmtdl = new CMTDL(_configuration);
        var status = _cmtdl.InsertMgApproveDueInstrumentList(data.ToString(), userId);

        return new ResponseViewModel<RequestViewModel>
        {
            ResponseCode = 200,
            ResponseMessage = "Success",
            ResponseData = null,
            ResponseDataList = null
        };
    }

    public ResponseViewModel<RequestViewModel> ExternalUserSubmit(int requestId, int userId,  IFormFile httpPostedFileBase)
    {
        try
        {
            _unitOfWork.BeginTransaction();

            RequestStatus reqestStatus = new RequestStatus();
            reqestStatus.RequestId = requestId;
            reqestStatus.StatusId = (Int32)EnumRequestStatus.Closed;
            reqestStatus.CreatedOn = DateTime.Now;
            reqestStatus.CreatedBy = userId;
            //reqestStatus.Comment = acceptReason;
            _unitOfWork.Repository<RequestStatus>().Insert(reqestStatus);

            Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.Id == requestId).SingleOrDefault();
            requestById.StatusId = (Int32)EnumRequestStatus.Closed;           
            _unitOfWork.Repository<Request>().Update(requestById);
            _unitOfWork.SaveChanges();

            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();
            //instrumentById.IdNo = InstrumentIdNo;
            //_unitOfWork.Repository<Instrument>().Update(instrumentById);
            _unitOfWork.SaveChanges();


            if (httpPostedFileBase != null)
            {
                string filePath = _utilityService.UploadImage(httpPostedFileBase, "Instrument");
                Uploads upload = new Uploads()
                {
                    FileName = httpPostedFileBase.FileName,
                    FileGuid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    RequestId = requestId,
                    TemplateType = "EX-AP",
                    FilePath = filePath
                };
                _unitOfWork.Repository<Uploads>().Insert(upload);
                _unitOfWork.SaveChanges();
                InstrumentFileUpload instrumentFileUpload = new InstrumentFileUpload();
                instrumentFileUpload.InstrumentId = instrumentById.Id;
                instrumentFileUpload.UploadId = upload.Id;
                _unitOfWork.Repository<InstrumentFileUpload>().Insert(instrumentFileUpload);
                _unitOfWork.SaveChanges();
            }

            _unitOfWork.Commit();
            //long UserId = requestById.CreatedBy;
            ////UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
            ////UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            ////List<string> emailList = new List<string>();
            ////emailList.Add(fmUserById.Email);
            //CMTDL _cmtdl = new CMTDL(_configuration);
            //UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            //UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
            //string RequestType = string.Empty;
            //if (requestById.TypeOfReqest == 1)
            //{
            //    RequestType = "New";
            //}
            //else if (requestById.TypeOfReqest == 2)
            //{
            //    RequestType = "Regular";
            //}
            //else if (requestById.TypeOfReqest == 3)
            //{
            //    RequestType = "Recalibration";
            //}
            //string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Accepted by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            //mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestById.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestById.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName);

            //string mailSubject = "New Calibration Request Accept Notification-" + requestById.ReqestNo + "";

            //_emailService.EmailSendingFunction(fmUserById.Email, mailbody, mailSubject);

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
            ErrorViewModelTest.Log("RequestService - AcceptRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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

    public ResponseViewModel<RequestViewModel> SaveExternalObs(int requestId, int InstrumentID, int userId,string IdNo)
    {
        try
        {
            _unitOfWork.BeginTransaction();
           
            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == InstrumentID).SingleOrDefault();
            instrumentById.IdNo = IdNo;
            _unitOfWork.Repository<Instrument>().Update(instrumentById);
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
            ErrorViewModelTest.Log("RequestService - AcceptRequest Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
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

    public ResponseViewModel<RequestViewModel> ExternalCalibrationReject(int requestId, string RejectReason, int userId)
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
            requestById.ReceivedBy = userId;            
            requestById.ReceivedDate = DateTime.Now;
            _unitOfWork.Repository<Request>().Update(requestById);
            _unitOfWork.SaveChanges();

            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == requestById.InstrumentId).SingleOrDefault();

            _unitOfWork.Commit();
            long UserId = requestById.CreatedBy;
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
            //UserViewModel fmUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 4 && Q.Level != "L4").SingleOrDefault());
            CMTDL _cmtdl = new CMTDL(_configuration);
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            UserViewModel fmUserById = _cmtdl.GetLadTechnicalManagerUsers();
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
            string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            External Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr><tr><td align='left'>Reason of Rejection</td><td>:</td>                <td>                    $Reason$                </td>            </tr></table>  <p><a href='http://s365id1qdg044' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            mailbody = mailbody.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$", requestById.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentById.InstrumentName).Replace("$INSTRUMENTID$", instrumentById.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(requestById.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$Reason$", RejectReason);
            var mailSubject = "External Calibration Request Reject Notification-" + requestById.ReqestNo + "";
            _emailService.EmailSendingFunction(fmUserById.Email, mailbody, mailSubject);

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
            ErrorViewModelTest.Log("RequestService - ExternalCalibrationReject Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
            return new ResponseViewModel<RequestViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "RequestService",
                ResponseServiceMethod = "ExternalCalibrationReject"
            };
        }
    }

}