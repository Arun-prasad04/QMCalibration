using CMT.DATAMODELS;
using WEB.Models;
using WEB.Models.Templates;

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
		ResponseViewModel<LeverTypeDialViewModel> GetLeverDialById(int requestId, int instrumentId);

		ResponseViewModel<MicrometerViewModel> GetMicrometerById(int requestId, int instrumentId);

		ResponseViewModel<GeneralViewModel> GetGeneralById(int requestId, int instrumentId);

		ResponseViewModel<PlungerDialViewModel> GetPlungerDialById(int requestId, int instrumentId);

		ResponseViewModel<ThreadGaugesViewModel> GetThreadGaugesById(int requestId, int instrumentId);

		ResponseViewModel<TorqueWrenchesViewModel> GetTorqueWrenchesById(int requestId, int instrumentId);

		ResponseViewModel<VernierCaliperViewModel> GetVernierCaliperById(int requestId, int instrumentId);
		ResponseViewModel<GeneralNewViewModel> GetGeneralNewById(int requestId, int instrumentId);

		ResponseViewModel<LeverTypeDialViewModel> SaveLeverDialCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);

		ResponseViewModel<MicrometerViewModel> SaveMicrometerCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);

		ResponseViewModel<PlungerDialViewModel> SavePlungerDialCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);
		ResponseViewModel<ThreadGaugesViewModel> SaveThreadGaugesCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);
		ResponseViewModel<TorqueWrenchesViewModel> SaveTorqueWrenchesCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);
		ResponseViewModel<VernierCaliperViewModel> SaveVernierCaliperCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);

		ResponseViewModel<GeneralNewViewModel> SaveGeneralNewCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);

		ResponseViewModel<GeneralViewModel> SaveGeneralCertificate(int requestId, int instrumentId, string EnvironmentCondition, string Uncertainity, string CalibrationResult, string Remarks, int loginBy, string ExportData);
		ResponseViewModel<LeverTypeDialViewModel> SubmitReview(int observationId, DateTime reviewDate, int reviewStatus, int reviewedBy, string Remarks);

		//ResponseViewModel<MasterViewModel> GetEquipmentListByInstrumentId(InstrumentViewModel instrument);
		ResponseViewModel<MasterViewModel> GetEquipmentListByInstrumentId(int MasterInstrument1, int MasterInstrument2, int MasterInstrument3, int MasterInstrument4);

		ResponseViewModel<MetalRulesViewModel> GetMetalRulesId(int requestId, int instrumentId);

        ResponseViewModel<MetalRulesViewModel> InsertMetalRule(MetalRulesViewModel micrometer);
		#region "Dynamic Observation"
		ResponseViewModel<ObservationContentViewModel> GetObservationById(int InstrumentId, int RequestId);
	
		ResponseViewModel<DynamicViewModel> GetObservationInstrumentById(int InstrumentId, int RequestId);
		ResponseViewModel<ObservationContentViewModel> GetSelectedObservationContentById(int ContentId, int InstrumentId,int RequestId);

		ResponseViewModel<DynamicViewModel> InsertObservation(DynamicViewModel Dynamic);
		ResponseViewModel<ObservationContentValuesViewModel> GetObservationContentValuesById(int InstrumentId, int RequestId);
		ResponseViewModel<ObservationContentViewModel> GetObservationContentSelectedList(List<Contentids> Contents);
		#endregion
		ResponseViewModel<ExternalObsViewModel> GetExternalObsById(int requestId, int instrumentId);
		ResponseViewModel<ExternalObsViewModel> InsertExternalObs(ExternalObsViewModel exObs);
        ResponseViewModel<CertificateViewModel> GetTemplateObservationById(int requestId, int instrumentId);
        ResponseViewModel<CertificateViewModel> SaveCertificateTemp(int requestId, int instrumentId, string EnvironmentCondition, int loginBy, string ExportData);


	}

}