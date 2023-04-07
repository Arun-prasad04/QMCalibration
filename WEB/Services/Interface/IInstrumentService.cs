using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface; 
public interface IInstrumentService
{
 ResponseViewModel<InstrumentViewModel> GetAllInstrumentList(int userId, int userRoleId);
 ResponseViewModel<InstrumentViewModel> GetInstrumentById(int instrumentId);
 ResponseViewModel<InstrumentViewModel> InsertInstrument(InstrumentViewModel instrument);
 ResponseViewModel<InstrumentViewModel> UpdateInstrument(InstrumentViewModel  instrument);
 ResponseViewModel<InstrumentViewModel> DeleteInstrument(int instrumentId);
 ResponseViewModel<InstrumentViewModel> CreateNewInstrument();
 ResponseViewModel<InstrumentViewModel> GetAllInstrumentQuarantineList(int userId, int userRoleId);
 ResponseViewModel<InstrumentViewModel> InstrumentQuarantine(int instrumentId, string reason,int userId,int statusId);
 ResponseViewModel<InstrumentViewModel> InstrumentRemoveQuarantine(int instrumentId,int statusId,int userId);
 ResponseViewModel<InstrumentViewModel> GetInstrumentListByName(string instrumentName);
 ResponseViewModel<InstrumentViewModel> GetInstrumentListByIdNo(string idNo);
 ResponseViewModel<InstrumentViewModel> GetCurrentMonthDueList();
}