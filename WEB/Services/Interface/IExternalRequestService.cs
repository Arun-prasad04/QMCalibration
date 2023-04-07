using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface; 
public interface IExternalRequestService
{
 ResponseViewModel<ExternalRequestViewModel> GetAllExternalRequestList();
 ResponseViewModel<ExternalRequestViewModel> GetExternalRequestById(int externalRequestId);
 ResponseViewModel<ExternalRequestViewModel> InsertExternalRequest(int masterId,int userId);
 ResponseViewModel<ExternalRequestViewModel> UpdateExternalRequest(ExternalRequestViewModel  externalRequest);
 ResponseViewModel<ExternalRequestViewModel> DeleteExternalRequest(int externalRequestId);
 ResponseViewModel<ExternalRequestViewModel> AcceptExternalRequest(int externalRequestId,int userId);
 ResponseViewModel<ExternalRequestViewModel> RejectExternalRequest(int externalRequestId,string RejectReason,int userId);
 ResponseViewModel<ExternalRequestViewModel> SubmitFMExternalRequest(int externalRequestId,string Result,int userId);
 public ResponseViewModel<ExternalRequestViewModel> SubmitLABExternalRequest(int externalRequestId,string Result,int userId);
}