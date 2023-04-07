using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IFeedbackDataService
{
     ResponseViewModel<FeedbackViewModel> InsertFeedbackData(FeedbackViewModel feedbackViewModel);
     ResponseViewModel<FeedbackViewModel> UpdateFeedbackData(FeedbackViewModel feedbackViewModel);
     ResponseViewModel<FeedbackViewModel> GetFeedbackData(int feedbackInviteId);
}