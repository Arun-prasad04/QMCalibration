using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface;
public interface IDynamicObservationService
{
	ResponseViewModel<DynamicViewModel> GetObservationById(int ObservationTemplate, int ObservationType);
}
