using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface
{
public interface IObservationTemplateService
{
    ResponseViewModel<LeverTypeDialViewModel> InsertLeverDial(LeverTypeDialViewModel levertypedial);
    ResponseViewModel<MicrometerViewModel> InsertMicrometer(MicrometerViewModel micrometer);
    ResponseViewModel<VernierCaliperViewModel> InsertVernierCaliper(VernierCaliperViewModel verniercaliper);

    ResponseViewModel<GeneralNewViewModel> InsertGeneralnewobs(GeneralNewViewModel GeneralNew);
    ResponseViewModel<PlungerDialViewModel> InsertPlungerDial(PlungerDialViewModel plungerDial);
    ResponseViewModel<ThreadGaugesViewModel> InsertThreadGuages(ThreadGaugesViewModel threadGauges);
    ResponseViewModel<TorqueWrenchesViewModel> InsertTWobs(TorqueWrenchesViewModel torquewrenches);
    ResponseViewModel<GeneralViewModel> InsertGeneral(GeneralViewModel general);
    ResponseViewModel<LeverTypeDialViewModel> GetLeverDialById(int requestId,int instrumentId);
    
    ResponseViewModel<MicrometerViewModel> GetMicrometerById(int requestId, int instrumentId);
    
    ResponseViewModel<GeneralViewModel> GetGeneralById(int requestId, int instrumentId);
    
    ResponseViewModel<PlungerDialViewModel> GetPlungerDialById(int requestId, int instrumentId);
    
    ResponseViewModel<ThreadGaugesViewModel> GetThreadGaugesById(int requestId, int instrumentId);
    
    ResponseViewModel<TorqueWrenchesViewModel> GetTorqueWrenchesById(int requestId, int instrumentId);
    
    ResponseViewModel<VernierCaliperViewModel> GetVernierCaliperById(int requestId, int instrumentId);
    ResponseViewModel<GeneralNewViewModel> GetGeneralNewById(int requestId, int instrumentId);
    
    ResponseViewModel<LeverTypeDialViewModel>SaveLeverDialCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);

    ResponseViewModel<MicrometerViewModel>SaveMicrometerCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);

    ResponseViewModel<PlungerDialViewModel>SavePlungerDialCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);
    ResponseViewModel<ThreadGaugesViewModel>SaveThreadGaugesCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);
    ResponseViewModel<TorqueWrenchesViewModel>SaveTorqueWrenchesCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);
    ResponseViewModel<VernierCaliperViewModel>SaveVernierCaliperCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);
    
    ResponseViewModel<GeneralNewViewModel>SaveGeneralNewCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);
   
    ResponseViewModel<GeneralViewModel>SaveGeneralCertificate(int requestId, int instrumentId,string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks,int loginBy,string ExportData);
    ResponseViewModel<LeverTypeDialViewModel> SubmitReview(int observationId,DateTime reviewDate, int reviewStatus, int reviewedBy);
  } 

}