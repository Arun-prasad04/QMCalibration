using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface; 
public interface IMasterHistoryService
{
 ResponseViewModel<MasterHistoryViewModel> GetMasterHistoryListbyId(int MasterId);

 ResponseViewModel<MasterHistoryViewModel> CreateNewMasterHistory();
 ResponseViewModel<MasterHistoryViewModel> InsertMaster(MasterHistoryViewModel master);
 
}



