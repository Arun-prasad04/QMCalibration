using WEB.Models;

namespace WEB.Services.Interface
{
	public interface ITemplateObservationService
	{
		ResponseViewModel<MicrometerViewModel> GetMicrometerById(int requestId, int instrumentId);
	}
}
