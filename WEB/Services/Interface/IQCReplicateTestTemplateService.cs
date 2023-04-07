using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IQCReplicateTestTemplateService
{
   ResponseViewModel<ReplicateTestViewModel> GetByTemplateData(int Id);
   ResponseViewModel<ReplicateTestViewModel> InsertData(ReplicateTestViewModel qcReplicateTestTemplateViewModel);
   ResponseViewModel<ReplicateTestViewModel> UpdateData(ReplicateTestViewModel qcReplicateTestTemplateViewModel); 

   ResponseViewModel<ReplicateTestViewModel> GetReplicateTestGridData();
} 