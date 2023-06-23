using AutoMapper;
using WEB.Models;
using CMT.DATAMODELS;
namespace WEB.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserViewModel>();
        CreateMap<Department, DepartmentViewModel>();
        CreateMap<UserViewModel, User>();
        CreateMap<DepartmentViewModel, Department>();
        CreateMap<ContactViewModel, Contact>();
        CreateMap<Contact, ContactViewModel>();
        CreateMap<InstrumentViewModel, Instrument>();
        CreateMap<Instrument, InstrumentViewModel>();
        CreateMap<FeedbackInvite, FeedbackInviteViewModel>().ReverseMap();
        CreateMap<FeedbackData, FeedbackViewModel>();
        CreateMap<FeedbackViewModel, FeedbackData>();
        CreateMap<MasterViewModel, Master>();
        CreateMap<Master, MasterViewModel>();
        CreateMap<RequestViewModel, Request>();
        CreateMap<Request, RequestViewModel>();
        CreateMap<ExternalRequestViewModel, ExternalRequest>();
        CreateMap<ExternalRequest, ExternalRequestViewModel>();
        CreateMap<LovsViewModel, Lovs>();
        CreateMap<Lovs, LovsViewModel>();
		CreateMap<Location, LocationViewModel>();
		CreateMap<LocationViewModel, Location>();
		CreateMap<QCAlternateMethodTemplateViewModel, QCAlternateMethodTemplate>();
        CreateMap<QCAlternateMethodTemplate, QCAlternateMethodTemplateViewModel>();

        CreateMap<QCAlternateMethodTemplateDataViewModel, QCAlternateMethodTemplateData>();
        CreateMap<QCAlternateMethodTemplateData, QCAlternateMethodTemplateDataViewModel>();

        CreateMap<ObsTemplateLeverTypeDial, LeverTypeDialViewModel>();
        CreateMap<LeverTypeDialViewModel, ObsTemplateLeverTypeDial>();

        CreateMap<ObsTemplateVernierCaliper, VernierCaliperViewModel>();
        CreateMap<VernierCaliperViewModel, ObsTemplateVernierCaliper>();

        CreateMap<PlungerDialViewModel, ObsTemplatePlungerDial>();
        CreateMap<ObsTemplatePlungerDial, PlungerDialViewModel>();

        CreateMap<ThreadGaugesViewModel, ObsTemplateThreadGauges>();
        CreateMap<ObsTemplateThreadGauges, ThreadGaugesViewModel>();

        CreateMap<TorqueWrenchesViewModel, ObsTemplateTWobs>();
        CreateMap<ObsTemplateTWobs, TorqueWrenchesViewModel>();

        CreateMap<MicrometerViewModel, ObsTemplateMicrometer>();
        CreateMap<ObsTemplateMicrometer, MicrometerViewModel>();

        CreateMap<GeneralViewModel, ObsTemplateGeneral>();
        CreateMap<ObsTemplateGeneral, GeneralViewModel>();

        CreateMap<QCIntermediateTemplateViewModel, QCIntermediateTemplate>();
        CreateMap<QCIntermediateTemplate, QCIntermediateTemplateViewModel>();

        CreateMap<QCIntermediateTemplateResultViewModel, QCIntermediateTemplateResult>();
        CreateMap<QCIntermediateTemplateResult, QCIntermediateTemplateResultViewModel>();

        CreateMap<GeneralResultViewModel, ObsGeneralMeasuredValues>();
        CreateMap<ObsGeneralMeasuredValues, GeneralResultViewModel>();

        CreateMap<GeneralManualResultViewModel, ObsGeneralDynamicValues>();
        CreateMap<ObsGeneralDynamicValues, GeneralManualResultViewModel>();

        CreateMap<ReplicateTestViewModel, QCReplicateTestTemplate>();
        CreateMap<QCReplicateTestTemplate, ReplicateTestViewModel>();

        CreateMap<ReplicateTestDataViewModel, QCReplicateTestTemplateData>();
        CreateMap<QCReplicateTestTemplateData, ReplicateTestDataViewModel>();


        CreateMap<ReTestViewModel, QCReTestTemplate>();
        CreateMap<QCReTestTemplate, ReTestViewModel>();

        CreateMap<ReTestDataViewModel, QCReTestTemplateData>();
        CreateMap<QCReTestTemplateData, ReTestDataViewModel>();

        
        CreateMap<MasterHistoryViewModel, MasterEquipmentHistory>();
        CreateMap<MasterEquipmentHistory, MasterHistoryViewModel>();

        CreateMap<QRCodeFilesViewModel, QRCodeFiles>();
        CreateMap<QRCodeFiles, QRCodeFilesViewModel>();

        //This comment added For Git merge conflict issue   
    }
}