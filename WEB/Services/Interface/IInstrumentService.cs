
using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface; 
public interface IInstrumentService
{
 ResponseViewModel<InstrumentViewModel> GetAllInstrumentList(int userId, int userRoleId);
 ResponseViewModel<InstrumentViewModel> GetInstrumentById(int instrumentId);
 int GetObservationTemplateId(int instrumentId, string Type);
 ResponseViewModel<InstrumentViewModel> InsertInstrument(InstrumentViewModel instrument);
 ResponseViewModel<InstrumentViewModel> UpdateInstrument(InstrumentViewModel  instrument);
 ResponseViewModel<InstrumentViewModel> DeleteInstrument(int instrumentId);
 ResponseViewModel<InstrumentViewModel> CreateNewInstrument(int userId, int userRoleId);
 ResponseViewModel<InstrumentViewModel> GetAllInstrumentQuarantineList(int userId, int userRoleId);
 ResponseViewModel<InstrumentViewModel> InstrumentQuarantine(int instrumentId, string reason,int userId,int statusId);
 ResponseViewModel<InstrumentViewModel> InstrumentRemoveQuarantine(int instrumentId,int statusId,int userId);
 ResponseViewModel<InstrumentViewModel> GetInstrumentListByName(string instrumentName);
 ResponseViewModel<InstrumentViewModel> GetInstrumentListByIdNo(string idNo);
 ResponseViewModel<InstrumentViewModel> GetCurrentMonthDueList();
	ResponseViewModel<InstrumentViewModel> GetAllToolInventoryInstrumentList(int UserDept);
	ResponseViewModel<InstrumentViewModel> SaveInventoryCalibration(List<Instrumentids> Instrumentid, int userId);
	ResponseViewModel<InstrumentViewModel> GetAllToolRoomDepartmentwiseInstrument();
	ResponseViewModel<InstrumentViewModel> PopUpList(string InstrumentName, int InstrumentId);
	
}