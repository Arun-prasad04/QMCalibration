using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IQCReTestTemplateService
{
   ResponseViewModel<ReTestViewModel> GetByTemplateData(int Id);
   ResponseViewModel<ReTestViewModel> InsertData(ReTestViewModel qcReTestTemplateViewModel);
   ResponseViewModel<ReTestViewModel> UpdateData(ReTestViewModel qcReTestTemplateViewModel); 

   ResponseViewModel<ReTestViewModel> GetReTestGridData();
} 