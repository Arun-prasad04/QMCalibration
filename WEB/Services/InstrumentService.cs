using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using System.Net;
using System.Net.Mail;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.Data.SqlTypes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Text;
using static iTextSharp.text.pdf.AcroFields;
using NPOI.SS.Formula.Functions;



namespace WEB.Services;
public class InstrumentService : IInstrumentService
{
    private readonly IMapper _mapper;

    private IHttpContextAccessor _contextAccessor { get; set; }
    private IUnitOfWork _unitOfWork { get; set; }
    private IEmailService _emailService;
    private IConfiguration _configuration;
    private IUtilityService _utilityService;
    //private CMTDL _cmtdl { get; set; }
    public InstrumentService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService, IHttpContextAccessor contextAccessor, IEmailService emailService, IConfiguration Configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilityService = utilityService;
        _contextAccessor = contextAccessor;
        _emailService = emailService;
        _configuration = Configuration;
        //_cmtdl = cmtdl;
    }

	public ResponseViewModel<InstrumentViewModel> GetAllInstrumentList1(int userId, int userRoleId)
	{
		try
		{
			//UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
			List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
			CMTDL _cmtdl = new CMTDL(_configuration);
			DataSet ds = _cmtdl.GetInstruentList1(userId, userRoleId);
			//List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					InstrumentViewModel inst = new InstrumentViewModel
					{
						Id = Convert.ToInt32(dr["Id"]),
						InstrumentName = dr["InstrumentName"].Equals(DBNull.Value) ? null : dr["InstrumentName"].ToString(),
						SlNo = dr["SlNo"].Equals(DBNull.Value) ? null : dr["SlNo"].ToString(),
                        IdNo = dr["IdNo"].Equals(DBNull.Value) ? null : dr["IdNo"].ToString(),
						Range = dr["Range"].Equals(DBNull.Value) ? null : dr["Range"].ToString(),
                        //LC = dr["LC"].ToString(),
                        //CalibFreq = Convert.ToInt16(dr["CalibFreq"]),
                        CalibDate = dr["CalibDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["CalibDate"]),
						DueDate = dr["DueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["DueDate"]),
						//Make = dr["Make"].ToString(),
						//CalibSource = dr["CalibSource"].ToString(),
						//StandardReffered = dr["StandardReffered"].ToString(),
						//Remarks = dr["Remarks"].ToString(),
						Status = Convert.ToInt16(dr["Status"]),
                        RequestId = Convert.ToInt32(dr["RequestId"]),
                        //FileList = 
                        //CalibrationStatus = Convert.ToInt16(dr["CalibrationStatus"]),
                        //DateOfReceipt = Convert.ToDateTime(dr["DateOfReceipt"]),
                        //NewReqAcceptStatus = Convert.ToInt32(dr["NewReqAcceptStatus"]),
                        DepartmentName = dr["deptName"].ToString(),						
						RequestStatus = dr["RequestStatus"].Equals(DBNull.Value) ? null : Convert.ToInt32(dr["RequestStatus"]),
						UserRoleId = userRoleId,
						TypeOfEquipment = dr["TypeOfEquipment"].Equals(DBNull.Value) ? null : dr["TypeOfEquipment"].ToString(),
                        
						ToolInventoryStatus = dr["ToolInventoryStatus"].Equals(DBNull.Value) ? null : Convert.ToInt32(dr["ToolInventoryStatus"]),
						SubSecCode = dr["SubSectionCode"].ToString(),
                        ToolInventory = dr["ToolInventory"].Equals(DBNull.Value) ? null : dr["ToolInventory"].ToString(),
						//ReplacementStartDate = dr["ReplacementStartDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["ReplacementStartDate"]),// (DateTime?)Convert.ToDateTime(dr["ReplacementStartDate"]),
						//backcolor = dr["backcolor"].ToString(),
						ReqDueDate = dr["ReqDueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["ReqDueDate"]),
					};
					instrumentList.Add(inst);

                }
            }

            #region
            /*
			//if (userRoleId == 2 || userRoleId == 4)
			if (userRoleId == 2 || labUserById.DepartmentId == 66)
			{

				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.IdNo != "" && Q.IdNo != null)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel)
					.Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
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
						RequestStatus = s.RequestModel.Where(x => x.InstrumentId == s.Id).OrderByDescending(U => U.Id).Select(D => D.StatusId).FirstOrDefault()

					}
					).ToList();
				
			}
			else if (userRoleId == 1)
			{

				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.IdNo != "" && Q.IdNo != null) && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
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
				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.IdNo != "" && Q.IdNo != null) && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
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
			*/
            #endregion
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
            ErrorViewModelTest.Log("InstrumentService - GetAllInstrumentList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
    //public static ConnectionStringSettings sql_cs = ConfigurationManager.ConnectionStrings["dbConnectionString"];
    public ResponseViewModel<InstrumentViewModel> GetAllInstrumentList(int userId, int userRoleId, int Startingrow, int Endingrow, string Search, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate, string chkDue)
    {
        try
        {
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
            List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
            CMTDL _cmtdl = new CMTDL(_configuration);
            if (Search == null)
            { Search = string.Empty; }
            //if (Startingrow == 0)
            //{ Startingrow = 1; }
            DataSet ds = _cmtdl.GetInstruentList(userId, userRoleId, Startingrow, Endingrow, Search,  sscode,  instrumentname,  labid,  typeOfEquipment,  serialno,  range,  department,  calibrationdate,  duedate, chkDue);
            //List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
            var TotalCount = 0;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InstrumentViewModel inst = new InstrumentViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        InstrumentName = dr["InstrumentName"].ToString(),
                        SlNo = dr["SlNo"].ToString(),
                        IdNo = dr["IdNo"].ToString(),
                        Range = dr["Range"].ToString(),
                //LC = dr["LC"].ToString(),
                //CalibFreq = Convert.ToInt16(dr["CalibFreq"]),
                //CalibDate = Convert.ToDateTime(dr["CalibDate"]),
                        DueDate = dr["DueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["DueDate"]),
                        sCalibDate = dr["CalibDate"].Equals(DBNull.Value) ? null : String.Format("{0:dd/MM/yyyy}" , (dr["CalibDate"])),//Convert.ToDateTime(dr["CalibDate"]).ToShortDateString(),
						sDueDate = dr["DueDate"].Equals(DBNull.Value) ? null : String.Format("{0:dd/MM/yyyy}", (dr["DueDate"])),  //Convert.ToDateTime(dr["DueDate"]).ToShortDateString(),
                //Make = dr["Make"].ToString(),
                //CalibSource = dr["CalibSource"].ToString(),
                //StandardReffered = dr["StandardReffered"].ToString(),
                //Remarks = dr["Remarks"].ToString(),
                        Status = Convert.ToInt16(dr["Status"]),
                        RequestId = Convert.ToInt32(dr["RequestId"]),
                    //FileList = 
                    //CalibrationStatus = Convert.ToInt16(dr["CalibrationStatus"]),
                    //DateOfReceipt = Convert.ToDateTime(dr["DateOfReceipt"]),
                    //NewReqAcceptStatus = Convert.ToInt32(dr["NewReqAcceptStatus"]),
                        DepartmentName = dr["deptName"].ToString(),
                        RequestStatus = Convert.ToInt32(dr["RequestStatus"]),
                        UserRoleId = userRoleId,
                        TypeOfEquipment = dr["TypeOfEquipment"].ToString(),
                        ToolInventoryStatus = Convert.ToInt32(dr["ToolInventoryStatus"]),
                        SubSecCode = dr["SubSectionCode"].ToString(),
                        ToolInventory = dr["ToolInventory"].Equals(DBNull.Value) ? null : dr["ToolInventory"].ToString(),
                        //ReplacementStartDate = dr["ReplacementStartDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["ReplacementStartDate"]),// (DateTime?)Convert.ToDateTime(dr["ReplacementStartDate"]),
                        //backcolor = dr["backcolor"].ToString(),
                        ReqDueDate = dr["ReqDueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["ReqDueDate"]),
                    };
                    instrumentList.Add(inst);

                }
            }
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                TotalCount = Convert.ToInt32(ds.Tables[1].Rows[0][0]);
                instrumentList.ForEach(x => x.TotalCount = Convert.ToInt32(TotalCount));
            }
            #region
            /*
			//if (userRoleId == 2 || userRoleId == 4)
			if (userRoleId == 2 || labUserById.DepartmentId == 66)
			{

				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.IdNo != "" && Q.IdNo != null)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel)
					.Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
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
						RequestStatus = s.RequestModel.Where(x => x.InstrumentId == s.Id).OrderByDescending(U => U.Id).Select(D => D.StatusId).FirstOrDefault()

					}
					).ToList();
				
			}
			else if (userRoleId == 1)
			{

				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.IdNo != "" && Q.IdNo != null) && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
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
				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.IdNo != "" && Q.IdNo != null) && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel).Select(s => new InstrumentViewModel()
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
			*/
            #endregion
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
            ErrorViewModelTest.Log("InstrumentService - GetAllInstrumentList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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
            .SingleOrDefault() == instrumentId)
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
                MUTemplate = s.MUTemplate,
                Unit1 = s.Unit1,
                Unit2 = s.Unit2,
                AmountJPY = s.AmountJPY,
                UserDept = s.UserDept,
                Capacity = s.Capacity,
                Instrument_Type = s.Instrument_Type,
                EquipmentStation = s.EquipmentStation,
                Rule_Confirmity = s.Rule_Confirmity,
                Comment = s.Comment,
                IsNABL = s.IsNABL == null ? false : s.IsNABL,
                Grade = s.Grade,
                TypeOfEquipment = s.TypeOfEquipment,
                ToolInventory = s.ToolInventory,
                SubSecCode = s.DepartmenttModel.SubSectionCode
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

            //if (instrumentdata.ToolInventory == "Yes" && (newRequest.TypeOfReqest == 2 || newRequest.TypeOfReqest == 3))
            //{
            //    ToolRoomHistory ToolRoomHistoryById = new ToolRoomHistory();
            //    ToolRoomHistoryById.StatusId = (Int32)ToolInventoryStatus.AcceptTool;
            //    ToolRoomHistoryById.ReplacementId = instrumentdata.ReplacementLabID;
            //    ToolRoomHistoryById.Comment = "Approved";
            //    ToolRoomHistoryById.CreatedBy = 0;
            //    ToolRoomHistoryById.CreatedOn = DateTime.Now;
            //    ToolRoomHistoryById.LabId = instrumentdata.IdNo;
            //    ToolRoomHistoryById.InstrumentId = instrumentdata.Id;
            //    _unitOfWork.Repository<ToolRoomHistory>().Insert(ToolRoomHistoryById);
            //    _unitOfWork.SaveChanges();

            //}
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
			string objToolInventory = instrument.ToolInventory;

			_unitOfWork.Commit();
            CMTDL _cmtdl = new CMTDL(_configuration);
            string UserId = _contextAccessor.HttpContext.Session.GetString("LoggedId");
            UserViewModel labUserById = _cmtdl.GetUserMasterById(Convert.ToInt32(UserId));
            //UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == Convert.ToInt32(UserId)).SingleOrDefault());
            //List<UserViewModel> fmUserById = _mapper.Map<List<UserViewModel>>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.UserRoleId == 2 && Q.Level != "L4").ToList());
            List<UserViewModel> fmUserById = _cmtdl.GetLadAdminUsers();
            List<string> emailList = new List<string>();
            string mailSubject = "New Instrument Calibration Request /  新規計量器校正依頼の件 - " + newRequest.ReqestNo + "";
            string RequestType = string.Empty;
            int LabDeptUser = 0;
            string SubSectionEng = string.Empty;
            string SubSectionJP = string.Empty;
            
            Department Deptdata = _unitOfWork.Repository<Department>().GetQueryAsNoTracking(d => d.Id == instrumentdata.UserDept && d.ActiveStatus == true).FirstOrDefault();
            if (Deptdata != null)
            { 
             SubSectionEng = Deptdata.SubSection;
             SubSectionJP = Deptdata.SubSectionJP;
            }

            if (objToolInventory != "Yes")
            {
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
                foreach (var item in fmUserById)
                {
                    if (item.Id == labUserById.Id)
                    {
                        LabDeptUser += 1;
                    }
                    emailList.Add(item.Email.Trim());
                    
                   
                    string mailbodyLab = "\r\n\r\n<!DOCTYPE html>  \r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>     \r\n\t<meta charset=\"utf-8\">  \r\n\t<meta name=\"viewport\" content=\"width=device-width\">     \r\n\t<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">     \r\n\t<meta name=\"x-apple-disable-message-reformatting\">   \r\n\t<title></title> <!--[if mso]><style>*{font-family:sans-serif !important}</style><![endif]-->    \r\n\t<style>         \r\n\t\thtml, body {              \r\n\t\t\tmargin: 0 auto !important;             \r\n\t\t\tpadding: 0 !important;              \r\n\t\t\theight: 100% !important;              \r\n\t\t\twidth: 100% !important          }          \r\n\t\t\tp {              font-size: 11px          }     \r\n\t\t\t* {              -ms-text-size-adjust: 100%;              -webkit-text-size-adjust: 100%          }\r\n\t\t\tdiv[style*=\"margin: 16px 0\"] {              margin: 0 !important          }        \r\n\t\t\ttable, td {              mso-table-lspace: 0pt !important;              mso-table-rspace: 0pt !important          }  \r\n\t\t\ttable {              border-spacing: 0 !important;              border-collapse: collapse !important;              table-layout: fixed !important;              margin: 0 auto !important          }     \r\n\t\t\ttable table table {                  table-layout: auto              }            a {              text-decoration: none          } \r\n\t\t\timg {              -ms-interpolation-mode: bicubic          }            *[x-apple-data-detectors], .unstyle-auto-detected-links *, .aBn {              border-bottom: 0 !important;              cursor: default !important;              color: inherit !important;              text-decoration: none !important;              font-size: inherit !important;              font-family: inherit !important;              font-weight: inherit !important;              line-height: inherit !important          }            .a6S {              display: none !important;              opacity: 0.01 !important          }         \r\n\t\t\timg.g-img + div {              display: none !important          }           \r\n\t\t\t@media only screen and (min-device-width: 320px) and (max-device-width: 374px) {         \r\n\t\t\t\t.email-container {                  min-width: 320px !important              }          }  \r\n\t\t\t\t@media only screen and (min-device-width: 375px) and (max-device-width: 413px) {             \r\n\t\t\t\t\t.email-container {                  min-width: 375px !important              }          }       \r\n\t\t\t\t\t@media only screen and (min-device-width: 414px) {              \r\n\t\t\t\t\t\t.email-container {                  min-width: 414px !important              }          }      \r\n\t\t\t\t\t\t</style>    \r\n\t\t\t\t\t\t<style>        \r\n\t\t\t\t\t\t\t.button-td, .button-a {              transition: all 100ms ease-in          }     \r\n\t\t\t\t\t\t\t.button-td-primary:hover, \r\n\t\t\t\t\t\t\t.button-a-primary:hover {              background: #555 !important;              border-color: #555 !important          }   \r\n\t\t\t\t\t\t\t@media screen and (max-width: 600px) {             \r\n\t\t\t\t\t\t\t\t.email-container {                  width: 100% !important;                  margin: auto !important              }           \r\n\t\t\t\t\t\t\t\t.fluid {                  max-width: 100% !important;                  height: auto !important;                  margin-left: auto !important;                  margin-right: auto !important              }              \r\n\t\t\t\t\t\t\t\t.stack-column, .stack-column-center {                  display: block !important;                  width: 100% !important;                  max-width: 100% !important;                  direction: ltr !important              }                .stack-column-center {                  text-align: center !important              }                .center-on-narrow {                  text-align: center !important;                  display: block !important;                  margin-left: auto !important;                  margin-right: auto !important;                  float: none !important              }                table.center-on-narrow {                  display: inline-block !important              }                .email-container p {                  font-size: 20px !important              }          }      \r\n\t\t</style><!--[if gte mso 9]>\r\n\r\n\t\t<xml> <o:OfficeDocumentSettings> <o:AllowPNG/> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> <![endif]--> \r\n\t\t</head>  \r\n\t\t<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;\">      \r\n\t\t\t<center style=\"width: 100%; background-color: #f1f1f1;\">          <!--[if mso | IE]><table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color: #f1f1f1;\"><tr><td> <![endif]-->          <span style=\"font-size: 18px\">英語版は日本語に続きます。</span>     \r\n<table align=\"center\" role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"600\" style=\"margin: 0 auto;\" class=\"email-container\">\r\n\t\t\t\t\t\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">Dear lab Team,\r\n</h1></table></td></tr>\r\n\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">New instrument calibration request has been created by <span>\r\n<b>$USERNAME$</b></span></h1></table></td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Request no:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNO$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Request type:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>New</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Instrument name:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$INSTRUMENTNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Requested sub section:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$SUBSECTION$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Requested by:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p><a href='http://s365id1qf042.in365.corpintra.net/DTAQMPortalUAT/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">User</p></td></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">$REQNAME$</p></td></table></td></tr>\r\n\r\n \r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p></p></table></td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p></p></table></td></tr>\r\n\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n\r\n\r\n\r\n<br />  ---\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">計量管理部門の皆様\r\n\r\n</h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">新規計量器の校正依頼がありました。<span>\r\n</span></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">依頼№:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNO$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">種類　:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>新規</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">計量器名:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$INSTRUMENTNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">要求されたサブセクション:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$SUBSECTIONJP$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">に要求された:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">使用部門:</p></td></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">$REQNAME$</p></td></table></td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n \r\n</table>\r\n</td></tr>\r\n</table>\r\n</table>  \r\n <!--[if mso | IE]></td></tr></table> <![endif]-->      </center>      <br /> \r\n\t\t\r\n\t\t\r\n\t\t</body>  </html>";
                    mailbodyLab = mailbodyLab.Replace("$NAME$", item.FirstName + " " + item.LastName).Replace("$USERNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$", newRequest.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentdata.InstrumentName).Replace("$INSTRUMENTID$", instrumentdata.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(newRequest.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$SUBSECTION$", SubSectionEng).Replace("$SUBSECTIONJP$", SubSectionJP);

                    _emailService.EmailSendingFunction(item.Email.Trim(), mailbodyLab, mailSubject);
                }    
                //Mail For Instrument Created User-Start  
                if(LabDeptUser == 0) { 
                string mailbodyUser = "\r\n\r\n<!DOCTYPE html>  \r\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\">\r\n<head>     \r\n\t<meta charset=\"utf-8\">  \r\n\t<meta name=\"viewport\" content=\"width=device-width\">     \r\n\t<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">     \r\n\t<meta name=\"x-apple-disable-message-reformatting\">   \r\n\t<title></title> <!--[if mso]><style>*{font-family:sans-serif !important}</style><![endif]-->    \r\n\t<style>         \r\n\t\thtml, body {              \r\n\t\t\tmargin: 0 auto !important;             \r\n\t\t\tpadding: 0 !important;              \r\n\t\t\theight: 100% !important;              \r\n\t\t\twidth: 100% !important          }          \r\n\t\t\tp {              font-size: 11px          }     \r\n\t\t\t* {              -ms-text-size-adjust: 100%;              -webkit-text-size-adjust: 100%          }\r\n\t\t\tdiv[style*=\"margin: 16px 0\"] {              margin: 0 !important          }        \r\n\t\t\ttable, td {              mso-table-lspace: 0pt !important;              mso-table-rspace: 0pt !important          }  \r\n\t\t\ttable {              border-spacing: 0 !important;              border-collapse: collapse !important;              table-layout: fixed !important;              margin: 0 auto !important          }     \r\n\t\t\ttable table table {                  table-layout: auto              }            a {              text-decoration: none          } \r\n\t\t\timg {              -ms-interpolation-mode: bicubic          }            *[x-apple-data-detectors], .unstyle-auto-detected-links *, .aBn {              border-bottom: 0 !important;              cursor: default !important;              color: inherit !important;              text-decoration: none !important;              font-size: inherit !important;              font-family: inherit !important;              font-weight: inherit !important;              line-height: inherit !important          }            .a6S {              display: none !important;              opacity: 0.01 !important          }         \r\n\t\t\timg.g-img + div {              display: none !important          }           \r\n\t\t\t@media only screen and (min-device-width: 320px) and (max-device-width: 374px) {         \r\n\t\t\t\t.email-container {                  min-width: 320px !important              }          }  \r\n\t\t\t\t@media only screen and (min-device-width: 375px) and (max-device-width: 413px) {             \r\n\t\t\t\t\t.email-container {                  min-width: 375px !important              }          }       \r\n\t\t\t\t\t@media only screen and (min-device-width: 414px) {              \r\n\t\t\t\t\t\t.email-container {                  min-width: 414px !important              }          }      \r\n\t\t\t\t\t\t</style>    \r\n\t\t\t\t\t\t<style>        \r\n\t\t\t\t\t\t\t.button-td, .button-a {              transition: all 100ms ease-in          }     \r\n\t\t\t\t\t\t\t.button-td-primary:hover, \r\n\t\t\t\t\t\t\t.button-a-primary:hover {              background: #555 !important;              border-color: #555 !important          }   \r\n\t\t\t\t\t\t\t@media screen and (max-width: 600px) {             \r\n\t\t\t\t\t\t\t\t.email-container {                  width: 100% !important;                  margin: auto !important              }           \r\n\t\t\t\t\t\t\t\t.fluid {                  max-width: 100% !important;                  height: auto !important;                  margin-left: auto !important;                  margin-right: auto !important              }              \r\n\t\t\t\t\t\t\t\t.stack-column, .stack-column-center {                  display: block !important;                  width: 100% !important;                  max-width: 100% !important;                  direction: ltr !important              }                .stack-column-center {                  text-align: center !important              }                .center-on-narrow {                  text-align: center !important;                  display: block !important;                  margin-left: auto !important;                  margin-right: auto !important;                  float: none !important              }                table.center-on-narrow {                  display: inline-block !important              }                .email-container p {                  font-size: 20px !important              }          }      \r\n\t\t</style><!--[if gte mso 9]>\r\n\r\n\t\t<xml> <o:OfficeDocumentSettings> <o:AllowPNG/> <o:PixelsPerInch>96</o:PixelsPerInch> </o:OfficeDocumentSettings> </xml> <![endif]--> \r\n\t\t</head>  \r\n\t\t<body width=\"100%\" style=\"margin: 0; padding: 0 !important; mso-line-height-rule: exactly; background-color: #f1f1f1;\">      \r\n\t\t\t<center style=\"width: 100%; background-color: #f1f1f1;\">          <!--[if mso | IE]><table role=\"presentation\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"background-color: #f1f1f1;\"><tr><td> <![endif]-->          <span style=\"font-size: 18px\">英語版は日本語に続きます。</span>     \r\n<table align=\"center\" role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"600\" style=\"margin: 0 auto;\" class=\"email-container\">\r\n\t\t\t\t\t\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">Dear User,\r\n</h1></table></td></tr>\r\n\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">New instrument calibration request has been created. <span>\r\n</span></h1></table></td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Request no:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNO$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Request type:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>New</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Instrument name:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$INSTRUMENTNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Requested sub section:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$SUBSECTION$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">Requested by:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p><a href='http://s365id1qf042.in365.corpintra.net/DTAQMPortalUAT/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">Regards</p></td></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\"></p></td></table></td></tr>\r\n\r\n \r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p></p></table></td></tr>\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p></p></table></td></tr>\r\n\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n\r\n\r\n\r\n<br />  \r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">計量管理部門の皆様\r\n\r\n</h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\">新規測定器登録を作成しました。<span>\r\n</span></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">依頼№:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNO$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">種類　:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>新規</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">計量器名:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$INSTRUMENTNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">要求されたサブセクション:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$SUBSECTIONJP$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"font-size: 15px;\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:600;\">に要求された:</p></td>\r\n<td align=\"left\" style=\"width:65%;text-align:left;padding-top: 1px;padding-left:3px;padding-right:0px;padding-bottom:0px; font-family: sans-serif; font-size: 30px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;\"><span>$REQNAME$</span></p></td></tr></table>\r\n</td></tr>\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td align=\"left\" style=\"width:25%;text-align:left;padding-top: 1px;padding-left:20px;padding-right:0px;padding-bottom:2px; font-family: sans-serif; font-size: 15px; line-height: 20px; color: #555555;\"><p style=\"margin: 0 0 8px;text-align:left;font-weight:normal;\">計量管理部門</p></td></table></td></tr>\r\n\r\n\r\n\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"background-color: #ffffff;\">\r\n<table role=\"presentation\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\"><tr><td style=\"padding-top: 5px;padding-left:20px;padding-right:10px;padding-bottom:1px; font-family: sans-serif; font-size: 12px; line-height: 20px; color: #555555;\"><h1 style=\"margin: 0 0 15px;text-align:left;font-size: 13px; line-height: 30px; color: #333333; font-weight: normal;\"></h1></table></td></tr>\r\n\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n<tr><td style=\"padding: 0 20px 20px;\"></td></tr>\r\n \r\n</table>\r\n</td></tr>\r\n</table>\r\n</table>  \r\n     </center>      <br /> \r\n\t\t\r\n\t\t\r\n\t\t</body>  </html>";
                mailbodyUser = mailbodyUser.Replace("$NAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$USERNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$", newRequest.ReqestNo).Replace("$TYPEREQUEST$", RequestType).Replace("$INSTRUMENTNAME$", instrumentdata.InstrumentName).Replace("$INSTRUMENTID$", instrumentdata.IdNo).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$", Convert.ToString(newRequest.CreatedOn)).Replace("$REQNAME$", labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$", labUserById.DepartmentName).Replace("$SUBSECTION$", SubSectionEng).Replace("$SUBSECTIONJP$", SubSectionJP);

				_emailService.EmailSendingFunction(labUserById.Email.Trim(), mailbodyUser, mailSubject);
                    //Mail For Instrument Created User-End  
                }
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
			ErrorViewModelTest.Log("InstrumentService - InsertInstrument Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			_unitOfWork.RollBack();
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
            
            if (instrument.UserRoleId == 2)
            {
                if (instrument.UserDept != null)
                {
                    instrumentById.UserDept = instrument.UserDept;
                }
                if (instrument.CalibFreq != null)
                {
                    instrumentById.CalibFreq = instrument.CalibFreq;
                }
                if (instrument.StandardReffered != null)
				{
					instrumentById.StandardReffered = instrument.StandardReffered;
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
            
            if (instrument.Remarks != null)
            {
                instrumentById.Remarks = instrument.Remarks;
            }
            if (instrument.DueDate != null && instrument.DueDate != DateTime.MinValue)
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
			if (instrument.EquipmentStation != null)
			{
				instrumentById.EquipmentStation = instrument.EquipmentStation;
			}
			if (instrument.Instrument_Type != null && instrument.Instrument_Type > 0)
			{
				instrumentById.Instrument_Type = instrument.Instrument_Type;
			}
			if (instrument.TypeOfEquipment != null)
			{
				instrumentById.TypeOfEquipment = instrument.TypeOfEquipment;
			}
			if (instrument.ToolInventory != null)
			{
				instrumentById.ToolInventory = instrument.ToolInventory;
			}

			_unitOfWork.Repository<Instrument>().Update(instrumentById);
			_unitOfWork.SaveChanges();

            Request requestById = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(Q => Q.InstrumentId == instrumentById.Id).OrderByDescending(O => O.Id).FirstOrDefault();//.SingleOrDefault();
            if (requestById.ReqDueDate != null) {
                requestById.ReqDueDate = instrumentById.DueDate;

			}
			_unitOfWork.Repository<Request>().Update(requestById);
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
            ErrorViewModelTest.Log("InstrumentService - UpdateInstrument Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
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

	public ResponseViewModel<InstrumentViewModel> CreateNewInstrument(int userId, int userRoleId)
	{
		try
		{
			CMTDL _cmtdl = new CMTDL(_configuration);
			//var DepartmentData = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
			var DepartmentData = _cmtdl.GetUserDepartment(userId, userRoleId);



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

            List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
            CMTDL _cmtdl = new CMTDL(_configuration);
            DataSet ds = _cmtdl.GetInstruentQuartineList(userId, userRoleId);
            //List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    InstrumentViewModel inst = new InstrumentViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        InstrumentName = dr["InstrumentName"].ToString(),
                        SlNo = dr["SlNo"].ToString(),
                        IdNo = dr["IdNo"].ToString(),
                        Range = dr["Range"].ToString(),
                        LC = dr["LC"].ToString(),
                        CalibFreq = Convert.ToInt16(dr["CalibFreq"]),
                        CalibDate = Convert.ToDateTime(dr["CalibDate"]),
                        DueDate = Convert.ToDateTime(dr["DueDate"]),
                        Make = dr["Make"].ToString(),
                        CalibSource = dr["CalibSource"].ToString(),
                        StandardReffered = dr["StandardReffered"].ToString(),
                        Remarks = dr["Remarks"].ToString(),
                        Status = Convert.ToInt16(dr["Status"]),
                        //FileList = 
                        //CalibrationStatus = Convert.ToInt16(dr["CalibrationStatus"]),
                        //InstrumentStatus = Convert.ToInt16(dr["InstrumentStatus"]),
                        //DateOfReceipt = Convert.ToDateTime(dr["DateOfReceipt"]),
                        //NewReqAcceptStatus = Convert.ToInt32(dr["NewReqAcceptStatus"]),
                        DepartmentName = dr["deptName"].ToString(),
                        RequestStatus = Convert.ToInt32(dr["RequestStatus"]),
                        UserRoleId = userRoleId,
                        TypeOfEquipment = dr["TypeOfEquipment"].ToString(),
                        QuaraantineOn = Convert.ToDateTime(dr["QuarantineCreatedOn"]),
                        QuarantineReason = dr["Reason"].ToString(),

                    };
                    instrumentList.Add(inst);

                }
            }
            #region Command
            /*
            UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
			List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
			if (userRoleId == 2 || labUserById.DepartmentId == 66)
			{
				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1 && Convert.ToInt16(Q.ActiveStatus) == 1).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Include(I => I.DepartmenttModel).Select(s => new InstrumentViewModel()
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
				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Select(s => new InstrumentViewModel()
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
				instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1 && Convert.ToInt16(Q.ActiveStatus) == 1 && (Q.CreatedBy == userId && Q.UserDept == labUserById.DepartmentId)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Include(I => I.DepartmenttModel).Select(s => new InstrumentViewModel()
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
			*/
            #endregion
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
            ErrorViewModelTest.Log("InstrumentService - GetAllInstrumentQuarantineList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "Instrument",
                ResponseService = "GetAllInstrumentQuarantineList"
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
	  CMTDL _cmtdl = new CMTDL(_configuration);
	  User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
      //User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.ForemanShortId).SingleOrDefault();
	  //User LabuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Level == "L4" && Q.DepartmentId == 66).SingleOrDefault();
	  User DeptuserByL4Id = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ShortId == userById.DeptCordShortId).FirstOrDefault();
	  User LabuserByL4Id = _cmtdl.GetCalibrationLabUsers();
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
            if (DeptuserByL4Id != null) {
			 newRequest.UserL4 = DeptuserByL4Id.Id;
			}
            if (LabuserByL4Id != null)
            {
             newRequest.LabL4 = LabuserByL4Id.Id;
			}

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
            string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New Calibration Request has been Created by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>            <tr>                <td align='left'>                    Type of Request                </td><td>:</td>                <td>                    $TYPEREQUEST$                </td>            </tr>            <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qf042.in365.corpintra.net/DTAQMPortalUAT/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            if ((DeptuserByL4Id != null) && (labUserById != null))
			{
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
            instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => (Q.IdNo != "" && Q.IdNo != null)).Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Where(Q => Q.DueDate.Value.Month == DateTime.Now.Month).Select(s => new InstrumentViewModel()
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
                ResponseDataList = instrumentList.Where(W => W.DueDate.Value.Month == DateTime.Now.Month && W.
                DueDate.Value.Year == DateTime.Now.Year).ToList()
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

	int IInstrumentService.GetObservationTemplateId(int instrumentId, string Type)
	{
		Instrument row = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrumentId).SingleOrDefault();
		int? ObservationTemplateId = 0;
		if (Type == "Certification")
		{
			ObservationTemplateId = row != null ? row.CertificationTemplate : 0;
		}
		else if (Type == "Observation" && (row.TypeOfEquipment == "Internal" || row.TypeOfEquipment == "内部"))
		{
			ObservationTemplateId = row.ObservationTemplate != null ? row.ObservationTemplate : 0;
		}
		else if (Type == "Observation" && (row.TypeOfEquipment == "External" || row.TypeOfEquipment == "外部の"))
		{
			ObservationTemplateId = row.ObservationTemplate != null ? row.ObservationTemplate : 01;
		}

        return (int)ObservationTemplateId;
    }

    //for Tool Inventory 

    public ResponseViewModel<InstrumentViewModel> PopUpList(string InstrumentName, int InstrumentId,int SubsectionCode)
    {
        try
        {

            List<InstrumentViewModel> ToolInventoryList = new List<InstrumentViewModel>();

            DataSet dsToolInventory = GetPopupInstrument(InstrumentName, InstrumentId, SubsectionCode);
            if (dsToolInventory != null && dsToolInventory.Tables.Count > 0 && dsToolInventory.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsToolInventory.Tables[0].Rows)
                {
                    InstrumentViewModel ObjinstView = new InstrumentViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        InstrumentName = dr["InstrumentName"].ToString(),
                        IdNo = dr["IdNo"].ToString(),
                        DueDate = dr["DueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["DueDate"]),
                        DepartmentName = dr["deptName"].ToString(),
						RequestId = Convert.ToInt32(dr["RequestId"]),
						ReplacementDeptId = dr["ReplacementDeptId"].Equals(DBNull.Value) ? null : Convert.ToInt32(dr["ReplacementDeptId"]),
					};
                    ToolInventoryList.Add(ObjinstView);

                }
            }

            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = ToolInventoryList
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("InstrumentService - GetAllToolInventoryInstrumentList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "Instrument",
                ResponseService = "GetAllToolInventoryInstrumentList"
            };
        }
    }
    public ResponseViewModel<InstrumentViewModel> GetAllToolInventoryInstrumentList(int UserDept, int DueMonth)
    {
        try
        {

            List<InstrumentViewModel> ToolInventoryList = new List<InstrumentViewModel>();

            DataSet dsToolInventory = GetToolInventoryList(UserDept,DueMonth);
            if (dsToolInventory != null && dsToolInventory.Tables.Count > 0 && dsToolInventory.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsToolInventory.Tables[0].Rows)
                {



                    InstrumentViewModel ObjinstView = new InstrumentViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        InstrumentName = dr["InstrumentName"].ToString(),
                        SlNo = dr["SlNo"].ToString(),
                        IdNo = dr["IdNo"].ToString(),
                        Range = dr["Range"].ToString(),
                        //LC = dr["LC"].ToString(),
                        CalibFreq = Convert.ToInt16(dr["CalibFreq"]),
                        CalibDate = Convert.ToDateTime(dr["CalibDate"]),
						DueDate = dr["DueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["DueDate"]),						
                        CalibSource = dr["CalibSource"].ToString(),
                        //StandardReffered = dr["StandardReffered"].ToString(),
                        Remarks = dr["Remarks"].ToString(),
                        Status = Convert.ToInt16(dr["Status"]),
                        DepartmentName = dr["deptName"].ToString(),
                        CreatedBy = Convert.ToInt16(dr["CreatedBy"]),
                        ReplacementLabID = dr["ReplacementLabID"].ToString(),
                        ToolRoomStatus = Convert.ToInt32(dr["ToolRoomStatus"]),
                        ToolInventoryStatus = Convert.ToInt32(dr["ToolInventoryStatus"]),
						SubSectionCode = dr["SubSectionCode"].ToString(),
						UserDept = Convert.ToInt32(dr["UserDept"]),						
					};
                    ToolInventoryList.Add(ObjinstView);

                }
            }

            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = ToolInventoryList
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("InstrumentService - GetAllToolInventoryInstrumentList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "Instrument",
                ResponseService = "GetAllToolInventoryInstrumentList"
            };
        }
    }
    public ResponseViewModel<InstrumentViewModel> GetAllToolRoomDepartmentwiseInstrument(int DueMonth)
    {
        try
        {

            List<InstrumentViewModel> ToolInventoryList = new List<InstrumentViewModel>();

            DataSet dsToolInventory = GetToolRoomDepartmentwiseInstrumentCount(DueMonth);
            if (dsToolInventory != null && dsToolInventory.Tables.Count > 0 && dsToolInventory.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in dsToolInventory.Tables[0].Rows)
                {
                    InstrumentViewModel ObjinstView = new InstrumentViewModel
                    {

                        UserDept = Convert.ToInt32(dr["UserDept"]),
                        DepartmentName = dr["DepartmentName"].ToString(),
						SubSectionCode = dr["SubSectionCode"].ToString(),
						InstrumentCount = Convert.ToInt32(dr["InstrumentCount"]),
                        ToolRoomStatus = Convert.ToInt32(dr["Status"]),
                        DueDate = Convert.ToDateTime(dr["DueForCalibration"]),
                        Inscount =Convert.ToInt32(dr["Inscount"]),
     };
                    ToolInventoryList.Add(ObjinstView);

                }
            }

            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = ToolInventoryList
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("InstrumentService - GetAllToolInventoryInstrumentList Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "Instrument",
                ResponseService = "GetAllToolInventoryInstrumentList"
            };
        }
    }
    public DataSet GetToolInventoryList(int UserDept, int DueMonth)//, int deptid)
    {
        var connectionString = _configuration.GetConnectionString("CMTDatabase");
        SqlCommand cmd = new SqlCommand("GetToolInventryInstrumentList");
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@UserDept", UserDept);
		cmd.Parameters.AddWithValue("@DueMonth", DueMonth);  
		SqlConnection sqlConn = new SqlConnection(connectionString);
        DataSet dsResults = new DataSet();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        cmd.Connection = sqlConn;
        cmd.CommandTimeout = 2000;
        sqlAdapter.SelectCommand = cmd;
        sqlAdapter.Fill(dsResults);
        return dsResults;
    }
    public DataSet GetPopupInstrument(string InstrumentName, int CreatedBy,int SubsectionCode)//, int deptid)
    {
        var connectionString = _configuration.GetConnectionString("CMTDatabase");
        SqlCommand cmd = new SqlCommand("PopUpInstrumentList");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@InstrumentName", InstrumentName);
        cmd.Parameters.AddWithValue("@InstrumentId", CreatedBy);
        cmd.Parameters.AddWithValue("@SubsectionCode", SubsectionCode);
        SqlConnection sqlConn = new SqlConnection(connectionString);
        DataSet dsResults = new DataSet();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        cmd.Connection = sqlConn;
        cmd.CommandTimeout = 2000;
        sqlAdapter.SelectCommand = cmd;
        sqlAdapter.Fill(dsResults);
        return dsResults;
    }
    public DataSet GetToolRoomDepartmentwiseInstrumentCount(int DueMonth)
    {
        var connectionString = _configuration.GetConnectionString("CMTDatabase");
        SqlCommand cmd = new SqlCommand("GetToolRoomDepartmentwiseInstrumentCount");
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DueMonth", DueMonth);
		SqlConnection sqlConn = new SqlConnection(connectionString);
        DataSet dsResults = new DataSet();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        cmd.Connection = sqlConn;
        cmd.CommandTimeout = 2000;
        sqlAdapter.SelectCommand = cmd;
        sqlAdapter.Fill(dsResults);
        return dsResults;
    }
    public DataSet SaveInventoryList(string ToolInventoryList,int userid)//, int deptid)
    {
        var connectionString = _configuration.GetConnectionString("CMTDatabase");
        SqlCommand cmd = new SqlCommand("SaveToolInventryInstrumentList");
        cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.AddWithValue("@ToolInventoryList", ToolInventoryList);
        cmd.Parameters.AddWithValue("@userid", userid); 
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}

    public ResponseViewModel<InstrumentViewModel> SaveInventoryCalibration(List<Instrumentids> Instrumentid, int UserId)
      {
        try
        {

            StringBuilder Intrumentdata = new StringBuilder();
            Intrumentdata.Append("<Root>");
            foreach (var Instrument in Instrumentid)
            {
                Intrumentdata.Append("<InstrumentList>");
                Intrumentdata.Append(string.Format("<instrumentId>{0}</instrumentId>", Instrument.InstrumentId));
                Intrumentdata.Append(string.Format("<ReplacementLabId>{0}</ReplacementLabId>", Instrument.ReplacementLabId));
                Intrumentdata.Append(string.Format("<ToolInventoryStatus>{0}</ToolInventoryStatus>", (Int32)ToolInventoryStatus.UserTool));
                Intrumentdata.Append(string.Format("<ToolRoomStatus>{0}</ToolRoomStatus>", (Int32)ToolRoomStatus.Completed));
				Intrumentdata.Append(string.Format("<DueMonth>{0}</DueMonth>", Instrument.DueMonth));
				Intrumentdata.Append(string.Format("<CalibFrequency>{0}</CalibFrequency>", Instrument.CalibFrequency));
				Intrumentdata.Append(string.Format("<UserDept>{0}</UserDept>", (Int32)Instrument.UserDept)); 
				Intrumentdata.Append(string.Format("<RequestId>{0}</RequestId>", (Int32)Instrument.RequestId));
	            Intrumentdata.Append(string.Format("<IdNo>{0}</IdNo>", Instrument.IdNo));
				Intrumentdata.Append(string.Format("<DueDate>{0}</DueDate>", Instrument.DueDate));
				Intrumentdata.Append(string.Format("<ReplacementDeptId>{0}</ReplacementDeptId>", Instrument.ReplacementDeptId));
				Intrumentdata.Append("</InstrumentList>");
            }
			

			Intrumentdata.Append("</Root>");

            var status = SaveInventoryList(Intrumentdata.ToString(), UserId);

            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = status.ToString(),// "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("InstrumentService - SaveInventoryCalibration Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            _unitOfWork.RollBack();
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "ToolInventoryCalibration",
                ResponseServiceMethod = "SaveInventoryCalibration"
            };
        }
    } 

    public DateTime GetcalibrationClosedate(int RequestId)
    {
        DateTime closedate = DateTime.MinValue;
		RequestStatus reById = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(Q => Q.RequestId == RequestId && Q.StatusId == 30).SingleOrDefault();
        if (reById != null)
        {
            closedate = (DateTime)reById.CreatedOn;
        }		
        return closedate;

	}
	public ResponseViewModel<InstrumentViewModel> GetAllToolRoomInstrument()
	{
		try
			{
				//UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
				List<InstrumentViewModel> ToolRoomInstrumentListing = new List<InstrumentViewModel>();
				DataSet dsToolInventory = GetToolRoomInstrumentList();
				if (dsToolInventory != null && dsToolInventory.Tables.Count > 0 && dsToolInventory.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in dsToolInventory.Tables[0].Rows)
					{
						InstrumentViewModel inst = new InstrumentViewModel
						{
							Id = Convert.ToInt32(dr["Id"]),
							InstrumentName = dr["InstrumentName"].ToString(),
							SlNo = dr["SlNo"].ToString(),
							IdNo = dr["IdNo"].ToString(),
							Range = dr["Range"].ToString(),
						    sCalibDate = dr["CalibDate"].Equals(DBNull.Value) ? null : String.Format("{0:dd/MM/yyyy}" , (dr["CalibDate"])),//Convert.ToDateTime(dr["CalibDate"]).ToShortDateString(),
						    sDueDate = dr["DueDate"].Equals(DBNull.Value) ? null : String.Format("{0:dd/MM/yyyy}", (dr["DueDate"])),  //C
							//CalibDate = dr["CalibDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["CalibDate"]),
							//DueDate = dr["DueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["DueDate"]),
							
							DepartmentName = dr["deptName"].ToString(),
							//RequestStatus = Convert.ToInt32(dr["RequestStatus"]),
							//UserRoleId = userRoleId,
							TypeOfEquipment = dr["TypeOfEquipment"].ToString(),
							//ToolInventoryStatus = Convert.ToInt32(dr["ToolInventoryStatus"]),
							SubSectionCode = dr["SubSectionCode"].ToString(),
						};
						ToolRoomInstrumentListing.Add(inst);

					}
				}

			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ToolRoomInstrumentListing
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("InstrumentService - GetAllToolInventoryInstrumentList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Instrument",
				ResponseService = "GetAllToolInventoryInstrumentList"
			};
		}
	}
    public DataSet GetToolRoomInstrumentList()
    {
        var connectionString = _configuration.GetConnectionString("CMTDatabase");
        SqlCommand cmd = new SqlCommand("[GetToolRoomInstrumentList]");
        cmd.CommandType = CommandType.StoredProcedure;

        SqlConnection sqlConn = new SqlConnection(connectionString);
        DataSet dsResults = new DataSet();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        cmd.Connection = sqlConn;
        cmd.CommandTimeout = 2000;
        sqlAdapter.SelectCommand = cmd;
        sqlAdapter.Fill(dsResults);
        return dsResults;
    }

    #region ControlCard
    public ResponseViewModel<InstrumentViewModel> GetInstrumentDetailById(int InstrumentId)
	{
		try
		{

			InstrumentViewModel ObservationInstrument = new InstrumentViewModel();
			DataSet dsInstrumentContent = GetInstrumentCardDetailsById(InstrumentId);
			if (dsInstrumentContent != null && dsInstrumentContent.Tables.Count > 0 && dsInstrumentContent.Tables[0].Rows.Count > 0)
			{
				DataRow dr = dsInstrumentContent.Tables[0].Rows[0];
				ObservationInstrument.Id = Convert.ToInt32(dr["Id"]);
				ObservationInstrument.IdNo = dr["IdNo"].ToString();
				ObservationInstrument.InstrumentName = dr["InstrumentName"].ToString();
				ObservationInstrument.Make = dr["Make"].ToString();
				ObservationInstrument.AmountJPY = dr["AmountJPY"].ToString();
				ObservationInstrument.Grade = dr["Grade"].ToString();
				ObservationInstrument.CreatedOn = Convert.ToDateTime(dr["CreatedOn"]);
				ObservationInstrument.IssueNo = dr["IssueNo"].ToString();
			}
			if (dsInstrumentContent != null && dsInstrumentContent.Tables.Count > 0 && dsInstrumentContent.Tables[1].Rows.Count > 0)
			{

				var EqiupmentList = new List<MasterViewModel>();
				foreach (DataRow dr in dsInstrumentContent.Tables[1].Rows)
				{
					EqiupmentList.Add(new MasterViewModel
					{
						Id = Convert.ToInt32(dr["Id"]),
						EquipName = Convert.ToString(dr["EquipName"]),
						EquipmentMasterId = Convert.ToString(dr["EquipmentMasterId"])

					});

				}

				ObservationInstrument.MasterEqiupmentList = EqiupmentList;
			}

			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = ObservationInstrument,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("InstrumentService - GetInstrumentDetailById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Instrument",
				ResponseService = "GetInstrumentDetailById"
			};
		}
	}

	public DataSet GetInstrumentCardDetailsById(int InstrumentId)
	{

		var connectionstring = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand command = new SqlCommand("GetInstrumentDetailById");
		command.CommandType = CommandType.StoredProcedure;
		command.Parameters.AddWithValue("@InstrumentId", InstrumentId);
		SqlConnection sqlcon = new SqlConnection(connectionstring);
		DataSet dsResult = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		command.Connection = sqlcon;
		command.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = command;
		sqlAdapter.Fill(dsResult);
		return dsResult;


	}
	public ResponseViewModel<InstrumentViewModel> GetRequestListForInstrument(int InstrumentId)
	{
		try
		{
			List<InstrumentViewModel> InstrumentRequest = new List<InstrumentViewModel>();
			DataSet dsInstrumentContent = GetRequestListInstrument(InstrumentId);
			if (dsInstrumentContent != null && dsInstrumentContent.Tables.Count > 0 && dsInstrumentContent.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsInstrumentContent.Tables[0].Rows)
				{

					InstrumentViewModel requestListInstrument = new InstrumentViewModel
					{
						Id = Convert.ToInt32(dr["Id"]),
						IdNo = dr["IdNo"].ToString(),
						InstrumentName = dr["InstrumentName"].ToString(),
						Result = dr["Result"].ToString(),
						Grade = dr["Grade"].ToString(),
						CalibrationMonth = dr["CalibrationMonth"].ToString(),
						CalibrationRequestDate = dr["Duedate"].ToString(),
						Inspectiondetails = dr["Inspectiondetails"].ToString(),
						SectionCode = dr["SubSectionCode"].ToString(),
						InstrumentType = dr["InstrumentType"].ToString(),
						RequestId = Convert.ToInt32(dr["RequestId"]),

					};
					InstrumentRequest.Add(requestListInstrument);
				}
			}
			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = InstrumentRequest
			};


		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("InstrumentService - GetInstrumentDetailById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Instrument",
				ResponseService = "GetInstrumentDetailById"
			};
		}
	}

	public DataSet GetRequestListInstrument(int InstrumentId)
	{

		var connectionstring = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand command = new SqlCommand("GetRequestListForInstrument");
		command.CommandType = CommandType.StoredProcedure;
		command.Parameters.AddWithValue("@InstrumentId", InstrumentId);
		SqlConnection sqlcon = new SqlConnection(connectionstring);
		DataSet dsResult = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		command.Connection = sqlcon;
		command.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = command;
		sqlAdapter.Fill(dsResult);
		return dsResult;


	}

	public ResponseViewModel<InstrumentViewModel> UpdateControlCardRequestList(List<RequestAllData> reqlist, int InstrumentId, string IssueNo)
	{
		StringBuilder data = new StringBuilder("");
		data.Append("<Root>");
		foreach (var sd in reqlist)
		{

			data.Append("<RequestList>");
			data.Append(string.Format("<Inspectiondetails>{0}</Inspectiondetails>", sd.Inspectiondetails));
			data.Append(string.Format("<requestId>{0}</requestId>", sd.requestId));
			data.Append("</RequestList>");
		}
		data.Append("</Root>");

		var status = UpdateControlCard(data.ToString(), InstrumentId, IssueNo);

		return new ResponseViewModel<InstrumentViewModel>
		{
			ResponseCode = 500,
			ResponseMessage = "Failure",
			ErrorMessage = "",
			ResponseData = null,
			ResponseDataList = null,
			ResponseService = "InstrumentService",
			ResponseServiceMethod = "UpdateControlCardRequestList"
		};
	}
	public DataSet UpdateControlCard(string reqist, int InstrumentId, string IssueNo)
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand cmd = new SqlCommand("UpdateControlCardRequestList");
		cmd.CommandType = CommandType.StoredProcedure;

		cmd.Parameters.AddWithValue("@instrumentid", InstrumentId);
		cmd.Parameters.AddWithValue("@IssueNo", IssueNo);
		cmd.Parameters.AddWithValue("@requestdata ", reqist);
		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
		cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}
    #endregion
    public ResponseViewModel<InstrumentViewModel> InActiveQuarantineInstrument(int instrumentId)
    {
        try
        {
            Instrument instrumentById = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.Id == instrumentId).SingleOrDefault();
            _unitOfWork.BeginTransaction();
           
            if (instrumentById != null)
            {
                instrumentById.ActiveStatus = false;
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



	public ResponseViewModel<InstrumentViewModel> ToolRoomDepartmentList(int userId, int userRoleId, int Startingrow, int Endingrow, string Search, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate)
	{
		try
		{
		//UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
			List<InstrumentViewModel> ToolRoomInstrumentListing = new List<InstrumentViewModel>();
			DataSet dsToolInventory = DsToolRoomDepartmentList(userId, userRoleId, Startingrow, Endingrow, Search,  sscode,  instrumentname,  labid,  typeOfEquipment,  serialno,  range,  department,  calibrationdate,  duedate);
			if (dsToolInventory != null && dsToolInventory.Tables.Count > 0 && dsToolInventory.Tables[0].Rows.Count > 0)
			{
				foreach (DataRow dr in dsToolInventory.Tables[0].Rows)
				{
					InstrumentViewModel inst = new InstrumentViewModel
					{
						Id = Convert.ToInt32(dr["Id"]),
						InstrumentName = dr["InstrumentName"].ToString(),
						SlNo = dr["SlNo"].ToString(),
						IdNo = dr["IdNo"].ToString(),
						Range = dr["Range"].ToString(),
						sCalibDate = dr["CalibDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["CalibDate"]).ToShortDateString(),//String.Format("{0:dd/MM/yyyy}", (dr["CalibDate"])),
						sDueDate = dr["DueDate"].Equals(DBNull.Value) ? null :   Convert.ToDateTime(dr["DueDate"]).ToShortDateString(),//String.Format("{0:dd/MM/yyyy}", (dr["DueDate"])),
																																	   //CalibDate = dr["CalibDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["CalibDate"]),String.Format("{0:dd/MM/yyyy}", (dr["CalibDate"])), String.Format("{0:dd/MM/yyyy}", (dr["CalibDate"]))
																																	   //DueDate = dr["DueDate"].Equals(DBNull.Value) ? null : Convert.ToDateTime(dr["DueDate"]),

						DepartmentName = dr["deptName"].ToString(),
						
						TypeOfEquipment = dr["TypeOfEquipment"].ToString(),
						SubSectionCode = dr["SubSectionCode"].ToString(),
					};
					ToolRoomInstrumentListing.Add(inst);

				}
			}
            var TotalCount = 0;
            if (dsToolInventory != null && dsToolInventory.Tables.Count > 0 && dsToolInventory.Tables[1].Rows.Count > 0)
            {
                TotalCount = Convert.ToInt32(dsToolInventory.Tables[1].Rows[0][0]);
                ToolRoomInstrumentListing.ForEach(x => x.TotalCount = Convert.ToInt32(TotalCount));
            }

            return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = ToolRoomInstrumentListing
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("InstrumentService - GetAllToolInventoryInstrumentList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<InstrumentViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseServiceMethod = "Instrument",
				ResponseService = "GetAllToolInventoryInstrumentList"
			};
		}
	}

	public DataSet DsToolRoomDepartmentList(int userid, int userroleid, int Startingrow, int Endingrow, string Search, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate)
	{
		var connectionString = _configuration.GetConnectionString("CMTDatabase");
		SqlCommand cmd = new SqlCommand("[GetToolRoomInstrumentList]");
		cmd.CommandType = CommandType.StoredProcedure;

		SqlConnection sqlConn = new SqlConnection(connectionString);
		DataSet dsResults = new DataSet();
		SqlDataAdapter sqlAdapter = new SqlDataAdapter();
        cmd.Parameters.AddWithValue("@userid", userid);
        cmd.Parameters.AddWithValue("@userroleid", userroleid);
        cmd.Parameters.AddWithValue("@Startingrow", Startingrow);
        cmd.Parameters.AddWithValue("@Endingrow", Endingrow);
        cmd.Parameters.AddWithValue("@Search", Search);

        cmd.Parameters.AddWithValue("@sscode", sscode);
        cmd.Parameters.AddWithValue("@instrumentname", instrumentname);
        cmd.Parameters.AddWithValue("@labid", labid);
        cmd.Parameters.AddWithValue("@typeOfEquipment", typeOfEquipment);


        cmd.Parameters.AddWithValue("@serialno", serialno);
        cmd.Parameters.AddWithValue("@range", range);
        cmd.Parameters.AddWithValue("@department", department);
        cmd.Parameters.AddWithValue("@calibrationdate", calibrationdate);
        cmd.Parameters.AddWithValue("@duedate", duedate);
        cmd.Connection = sqlConn;
		cmd.CommandTimeout = 2000;
		sqlAdapter.SelectCommand = cmd;
		sqlAdapter.Fill(dsResults);
		return dsResults;
	}



	#region Comment
	/*
    public ResponseViewModel<IdNoModel> IfIdNoExist()
    {
        try
        {
            List<IdNoModel> instrumentList = new List<IdNoModel>();           
            instrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => (Q.IdNo != "" && Q.IdNo != null) && Convert.ToInt16(Q.ActiveStatus) == 1).Select(s => new IdNoModel()
            {               
                IdNo = s.IdNo
            }).ToList();

            return new ResponseViewModel<IdNoModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = instrumentList
            };
        }
        catch (Exception e)
        {
            ErrorViewModelTest.Log("InstrumentService - IfIdNoExist Method");
            ErrorViewModelTest.Log("exception - " + e.Message);
            return new ResponseViewModel<IdNoModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "Instrument",
                ResponseService = "IfIdNoExist"
            };
        }
    }
    */
    #endregion
}