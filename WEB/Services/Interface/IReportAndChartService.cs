using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IReportAndChartService{
ResponseViewModel<InstrumentReportchartViewModel> CreateNewInstrumentReport();
ResponseViewModel<InstrumentViewModel>GetAllInstrumentQuarantineList(int departmentId);
ResponseViewModel<InstrumentViewModel>GetAllInstrumentList(int departmentId);
ResponseViewModel<ChartDataViewModel> GetChartData(int Year, int DepartmentId);
ResponseViewModel<RequestKPIViewModel> GetCalibrationlab(int Year, int Month, int DepartmentId);

}