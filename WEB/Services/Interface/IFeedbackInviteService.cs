using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IFeedbackInviteService
{
     ResponseViewModel<FeedbackInviteViewModel> InsertFeedbackInvite(List<FeedbackInviteViewModel> feedbackInviteViewModelList);
     ResponseViewModel<FeedbackInviteViewModel> GetFeedbackInviteList(int feedbackStatus);
     ResponseViewModel<FeedbackInviteViewModel> GetFeedbackInviteUserData(int userId);
}