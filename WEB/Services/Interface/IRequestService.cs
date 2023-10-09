using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface; 
public interface IRequestService
{
 ResponseViewModel<RequestViewModel> GetAllRequestList(int userRoleId,int userId);
 ResponseViewModel<RequestViewModel> GetRequestById(int RequestId);
 ResponseViewModel<RequestViewModel> InsertRequest(int instrumentId,int userId,int typeId);
 ResponseViewModel<RequestViewModel> UpdateRequest(RequestViewModel Request);
 ResponseViewModel<RequestViewModel> DeleteRequest(RequestViewModel Request);
 ResponseViewModel<RequestViewModel> AcceptRequest(int requestId, int userId,string InstrumentCondition,string Scope, DateTime TentativeCompletionDate, int CalibFreq, string ToolInventory, int newObservation,int newObservationType,int newMU,int newCertification,string standardReffered,bool newNABL,int MasterInstrument1,int MasterInstrument2,int MasterInstrument3,int MasterInstrument4, DateTime DueDate);
 ResponseViewModel<RequestViewModel> AcceptRequestRecalibration(int requestId, int userId,int AcceptValue,int departmentId);
 ResponseViewModel<RequestViewModel> RejectRequest(int requestId, string RejectReason, int userId,string InstrumentCondition,string Scope, string ToolInventory, DateTime TentativeCompletionDate,string standardReffered);
 ResponseViewModel<RequestViewModel> SubmitDepartmentRequestVisual(int requestId, string Result, int userId,string CollectedBy, string InstrumentIdNo, DateTime DueDate);
 ResponseViewModel<RequestViewModel> SubmitLABRequestVisual(int requestId, string Result, int userId,string RecordBy,DateTime ClosedDate,string Remarks,DateTime InstrumentReturnedDate,string CollectedBy,string ReasonforRejection,string IsFeasibleService,string IsFeasibleYes,string ServiceResponsibility,int RequestType,int InstrumentId,DateTime CalibDate,DateTime DueDate, bool newNABL, List<UploadFile> FileData,string IdNo);
ResponseViewModel<InstrumentViewModel> SubmitLABAdminUpdates(int requestId, int ObservationTemplate, int ObservationTemplateType, int MUTemplate, int CertificationTemplate);
 ResponseViewModel<RequestViewModel> SubmitNewRequest(int requestId,int userId,string InstrumentCondition,string Feasiblity,DateTime TentativeCompletionDate);
 ResponseViewModel<RequestViewModel> SubmitQuarantineRequest(int requestId,int userId);
 ResponseViewModel<LovsViewModel> GetLovs(string attrType,string attrsubType, string LangType);
 bool SaveQRFile(QRCodeFilesViewModel qrCodeFilesViewModel, string instrumentId);
 ResponseViewModel<RequestViewModel> SaveInstrumentData(int requestId,string newLabId,
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
            DateTime dateOfReceipt);

    ResponseViewModel<RequestViewModel> ExternalRejectRequest(int requestId, string RejectReason, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string standardReffered);
    ResponseViewModel<RequestViewModel> ExternalAcceptRequest(int requestId, int userId, string InstrumentCondition, string Feasiblity, DateTime TentativeCompletionDate, string InstrumentIdNo, string acceptReason, string ReceivedBy, IFormFile httpPostedFileBase, DateTime DueDate);

    ResponseViewModel<RequestViewModel> InsertDueRequest(List<RequestAllView> reqlist, int userId);
    ResponseViewModel<RequestViewModel> DueInstrumentAdminApprove(List<DueInstrument> DueList, int userId);
    ResponseViewModel<DueInstrument> GetAdminApproveInstrumentList();

    ResponseViewModel<RequestViewModel> DueInstrumentManagerApprove(List<DueInstrument> DueList, int userId);

    ResponseViewModel<RequestViewModel> ExternalUserSubmit(int requestId, int userId, IFormFile httpPostedFileBase);
    ResponseViewModel<RequestViewModel> SaveExternalObs(int requestId,int InstrumentID, int userId, string InstrumentIdNo);

    ResponseViewModel<RequestViewModel> ExternalCalibrationReject(int requestId, string RejectReason, int userId);
}