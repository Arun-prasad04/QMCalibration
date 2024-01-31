
using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface;
public interface IInstrumentService
{
    ResponseViewModel<InstrumentViewModel> GetAllInstrumentList1(int userId, int userRoleId);

    ResponseViewModel<InstrumentViewModel> GetAllInstrumentList(int userId, int userRoleId, int Startingrow, int Endingrow, string Search, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate);
    ResponseViewModel<InstrumentViewModel> GetInstrumentById(int instrumentId);
    int GetObservationTemplateId(int instrumentId, string Type);
    ResponseViewModel<InstrumentViewModel> InsertInstrument(InstrumentViewModel instrument);
    ResponseViewModel<InstrumentViewModel> UpdateInstrument(InstrumentViewModel instrument);
    ResponseViewModel<InstrumentViewModel> DeleteInstrument(int instrumentId);
    ResponseViewModel<InstrumentViewModel> CreateNewInstrument(int userId, int userRoleId);
    ResponseViewModel<InstrumentViewModel> GetAllInstrumentQuarantineList(int userId, int userRoleId);
    ResponseViewModel<InstrumentViewModel> InstrumentQuarantine(int instrumentId, string reason, int userId, int statusId);
    ResponseViewModel<InstrumentViewModel> InstrumentRemoveQuarantine(int instrumentId, int statusId, int userId);
    ResponseViewModel<InstrumentViewModel> GetInstrumentListByName(string instrumentName);
    ResponseViewModel<InstrumentViewModel> GetInstrumentListByIdNo(string idNo);
    ResponseViewModel<InstrumentViewModel> GetCurrentMonthDueList();
    ResponseViewModel<InstrumentViewModel> GetAllToolInventoryInstrumentList(int UserDept, int DueMonth);
    ResponseViewModel<InstrumentViewModel> SaveInventoryCalibration(List<Instrumentids> Instrumentid, int userId);
    ResponseViewModel<InstrumentViewModel> GetAllToolRoomDepartmentwiseInstrument(int DueMonth);
    ResponseViewModel<InstrumentViewModel> PopUpList(string InstrumentName, int InstrumentId, int SubsectionCode);

    DateTime GetcalibrationClosedate(int requid);
    ResponseViewModel<InstrumentViewModel> GetAllToolRoomInstrument();
    ResponseViewModel<InstrumentViewModel> GetInstrumentDetailById(int InstrumentId);
    ResponseViewModel<InstrumentViewModel> GetRequestListForInstrument(int InstrumentId);
    ResponseViewModel<InstrumentViewModel> UpdateControlCardRequestList(List<RequestAllData> reqlist, int InstrumentId, string IssueNo);
    ResponseViewModel<InstrumentViewModel> InActiveQuarantineInstrument(int instrumentId);
	//ResponseViewModel<IdNoModel> IfIdNoExist();

	ResponseViewModel<InstrumentViewModel> ToolRoomDepartmentList(int userId, int userRoleId, int Startingrow, int Endingrow, string Search, string sscode, string instrumentname, string labid, string typeOfEquipment, string serialno, string range, string department, string calibrationdate, string duedate);


}