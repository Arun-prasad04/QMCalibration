using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;

public class FeedbackDataService : IFeedbackDataService
{
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork { get; set; }
    private IEmailService _emailService;
    IUtilityService _utilityService;
    public FeedbackDataService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IUtilityService utilityService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _utilityService = utilityService;
    }

    public ResponseViewModel<FeedbackViewModel> GetFeedbackData(int feedbackInviteId)
    {
        try
        {
            FeedbackViewModel? feedbackViewModel = _unitOfWork.Repository<FeedbackData>()
           .GetQueryAsNoTracking(Q => Q.FeedbackInviteId == feedbackInviteId)
           .Include(I => I.UserModel)
           .Include(I => I.FeedbackInviteModel.UserModel.Department)
           .Include(I => I.FeedbackInviteModel)
           .Select(s => new FeedbackViewModel()
           {
               Id = s.Id,
               FeedbackInviteId = s.FeedbackInviteId,
               DesignationId = s.DesignationId == null ? 0 : s.DesignationId,
               DepartmentId = s.DepartmentId,
               LabId = s.FeedbackInviteModel.LabId,
               QualityOfService = s.QualityOfService,
               ReliabilityOfTest = s.ReliabilityOfTest,
               PersonnelCompetency = s.PersonnelCompetency,
               TechnicalCapability = s.TechnicalCapability,
               TestCalibration = s.TestCalibration,
               SafeguardingData = s.SafeguardingData,
               OnTimeDeliveryTest = s.OnTimeDeliveryTest,
               ResponseToCustomer = s.ResponseToCustomer,
               ComplaintResolution = s.ComplaintResolution,
               EmergencySupport = s.EmergencySupport,
               HandlingOfTest = s.HandlingOfTest,
               Commitment = s.Commitment,
               CommunicationAccess = s.CommunicationAccess,
               DocReportingSystem = s.DocReportingSystem,
               FacilitiesAndEnvironment = s.FacilitiesAndEnvironment,
               FiveSManagement = s.FiveSManagement,
               Confidentiality = s.Confidentiality,
               UserComments = s.UserComments,
               UserSubmitedOn = s.UserSubmitedOn,
               OverallScore = s.OverallScore,
               OverallPercentage = s.OverallPercentage,
               ReviewerRemarks = s.ReviewerRemarks,
               ReviewedBy = s.FeedbackStatus == 2 ? s.ReviewedBy : s.FeedbackInviteModel.InvitedBy,
               ReviewedOn = s.FeedbackStatus == 2 ? s.ReviewedOn : DateTime.Now.Date,
               ReviewedByName = s.FeedbackStatus == 2 ? s.UserModel.FirstName + " " + s.UserModel.LastName
                                                             : s.FeedbackInviteModel.InviteUserModel.FirstName + " "
                                                             + s.FeedbackInviteModel.InviteUserModel.LastName,
               FeedbackStatus = s.FeedbackStatus,
               DepartmentName = s.FeedbackInviteModel.UserModel.Department.Name,
               CustomerName = s.FeedbackInviteModel.UserModel.FirstName + " " + s.FeedbackInviteModel.UserModel.LastName,
               ActionRequired = s.ActionRequired == null ? 0 : s.ActionRequired,
               ProposedActions = s.ProposedActions,
               TargetDate = s.TargetDate == null ? DateTime.Now : s.TargetDate,
               Responsibility = s.Responsibility,
               UpdateStatus = s.UpdateStatus,
               ClosedDate = s.ClosedDate == null ? DateTime.Now : s.ClosedDate,
               ReviewFileName = s.ReviewFileName
           
           }).SingleOrDefault();

            if (feedbackViewModel != null)
            {
                List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork
                                                      .Repository<Lovs>()
                                                      .GetQueryAsNoTracking(Q => Q.Attrform == "Master")
                                                      .ToList());

                feedbackViewModel.LabName = lovsList.Where(x => x.AttrName.ToUpper().Equals("Lab", StringComparison.OrdinalIgnoreCase) && x.Id == feedbackViewModel.LabId)
                                                    .First().AttrValue;

                if (feedbackViewModel.DesignationId > 0)
                {
                    feedbackViewModel.Designation = lovsList.Where(x => x.AttrName.ToUpper().Equals("Designation", StringComparison.OrdinalIgnoreCase) && x.Id == feedbackViewModel.DesignationId)
                                                        .First().AttrValue;
                }
            }

            return new ResponseViewModel<FeedbackViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = feedbackViewModel,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<FeedbackViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "FeedbackDataService",
                ResponseServiceMethod = "GetFeedbackDataById"
            };
        }
    }

    public ResponseViewModel<FeedbackViewModel> InsertFeedbackData(FeedbackViewModel feedbackViewModel)
    {
        try
        {
            var labData = _unitOfWork.Repository<Lovs>()
                                    .GetQueryAsNoTracking(Q => Q.Attrform == "Master"
                                                                && Q.AttrName == "Lab"
                                                                && Q.AttrValue == feedbackViewModel.LabName)
                                    .ToList();
            if (!labData.Any())
            {
                return new ResponseViewModel<FeedbackViewModel>
                {
                    ResponseCode = 500,
                    ResponseMessage = "Failure",
                    ErrorMessage = "Lab data is not available",
                    ResponseData = null,
                    ResponseDataList = null,
                    ResponseService = "FeedbackData",
                    ResponseServiceMethod = "InsertFeedbackData"
                };
            }

            //Insert into FeedbackInvite table
            _unitOfWork.BeginTransaction();

            FeedbackData feedbackData = new FeedbackData()
            {
                FeedbackInviteId = feedbackViewModel.FeedbackInviteId,
                DesignationId = feedbackViewModel.DesignationId,
                DepartmentId = feedbackViewModel.DepartmentId,
                QualityOfService = feedbackViewModel.QualityOfService,
                ReliabilityOfTest = feedbackViewModel.ReliabilityOfTest,
                PersonnelCompetency = feedbackViewModel.PersonnelCompetency,
                TechnicalCapability = feedbackViewModel.TechnicalCapability,
                TestCalibration = feedbackViewModel.TestCalibration,
                SafeguardingData = feedbackViewModel.SafeguardingData,
                OnTimeDeliveryTest = feedbackViewModel.OnTimeDeliveryTest,
                ResponseToCustomer = feedbackViewModel.ResponseToCustomer,
                ComplaintResolution = feedbackViewModel.ComplaintResolution,
                EmergencySupport = feedbackViewModel.EmergencySupport,
                HandlingOfTest = feedbackViewModel.HandlingOfTest,
                Commitment = feedbackViewModel.Commitment,
                CommunicationAccess = feedbackViewModel.CommunicationAccess,
                DocReportingSystem = feedbackViewModel.DocReportingSystem,
                FacilitiesAndEnvironment = feedbackViewModel.FacilitiesAndEnvironment,
                FiveSManagement = feedbackViewModel.FiveSManagement,
                Confidentiality = feedbackViewModel.Confidentiality,
                UserComments = feedbackViewModel.UserComments,
                UserSubmitedOn = feedbackViewModel.UserSubmitedOn,
                OverallScore = feedbackViewModel.OverallScore,
                OverallPercentage = feedbackViewModel.OverallPercentage,
                FeedbackStatus = feedbackViewModel.FeedbackStatus
            };

            _unitOfWork.Repository<FeedbackData>().Insert(feedbackData);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return new ResponseViewModel<FeedbackViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<FeedbackViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "FeedbackData",
                ResponseServiceMethod = "InsertFeedbackData"
            };
        }
    }

    public ResponseViewModel<FeedbackViewModel> UpdateFeedbackData(FeedbackViewModel feedbackViewModel)
    {
        try
        {
            FeedbackData? feedbackData = _unitOfWork.Repository<FeedbackData>()
                                                    .GetQueryAsNoTracking(Q => Q.Id == feedbackViewModel.Id)
                                                    .SingleOrDefault();

            feedbackData.ReviewerRemarks = feedbackViewModel.ReviewerRemarks;
            feedbackData.ReviewedOn = feedbackViewModel.ReviewedOn;
            feedbackData.ReviewedBy = feedbackViewModel.ReviewedBy;
            feedbackData.FeedbackStatus = feedbackViewModel.FeedbackStatus;
            feedbackData.ActionRequired = feedbackViewModel.ActionRequired;

            if (feedbackViewModel.ActionRequired == (int)FeedbackDocumentStatus.Submitted)
            {
                feedbackData.ProposedActions = feedbackViewModel.ProposedActions;
                feedbackData.TargetDate = feedbackViewModel.TargetDate;
                feedbackData.Responsibility = feedbackViewModel.Responsibility;
                feedbackData.UpdateStatus = feedbackViewModel.UpdateStatus;
                feedbackData.ClosedDate = feedbackViewModel.ClosedDate;

                if (feedbackViewModel.ActionRequired == (int)FeedbackDocumentStatus.Submitted
                    && feedbackViewModel.FileUpload != null )
                {
                    string filePath = _utilityService.UploadImage(feedbackViewModel.FileUpload, Constants.CustomerFeedback_FolderName);
                    IFormFile fileobj = feedbackViewModel.FileUpload;
                    feedbackData.ReviewFileName = fileobj.FileName;
                }
            }

            _unitOfWork.BeginTransaction();
            _unitOfWork.Repository<FeedbackData>().Update(feedbackData);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return new ResponseViewModel<FeedbackViewModel>
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
            return new ResponseViewModel<FeedbackViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = feedbackViewModel,
                ResponseDataList = null,
                ResponseService = "FeedbackData",
                ResponseServiceMethod = "UpdateFeedbackData"
            };
        }
    }
}
