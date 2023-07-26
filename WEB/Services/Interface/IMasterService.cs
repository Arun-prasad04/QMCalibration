using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface;
public interface IMasterService
{
	ResponseViewModel<MasterViewModel> GetAllMasterList(string SessionLang);
	ResponseViewModel<MasterViewModel> GetMasterById(int masterId);
	ResponseViewModel<MasterViewModel> InsertMaster(MasterViewModel master);
	ResponseViewModel<MasterViewModel> UpdateMaster(MasterViewModel master);
	ResponseViewModel<MasterViewModel> DeleteMaster(int masterId);
	ResponseViewModel<MasterViewModel> CreateNewMaster();
	ResponseViewModel<MasterViewModel> GetAllMasterQuarantineList();
	ResponseViewModel<MasterViewModel> MasterQuarantine(int masterId, string reason, int statusId, int userId);
	ResponseViewModel<MasterViewModel> MasterRemoveQuarantine(int masterId);
	ResponseViewModel<MasterViewModel> GetEquipmentListByName(string equipmentName);
	ResponseViewModel<MasterViewModel> GetEquipmentListByLabId(string labId);
	//ResponseViewModel<MasterViewModel> GetEquipmentListByInstrumentId(int MasterInstrument1, int MasterInstrument2,int MasterInstrument3,int MasterInstrument4);


}



