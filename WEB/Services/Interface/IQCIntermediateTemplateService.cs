 using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IQCIntermediateTemplateService
{
   ResponseViewModel<QCIntermediateTemplateViewModel> GetAllIntermediteList();

   ResponseViewModel<QCIntermediateTemplateViewModel> InsertData(QCIntermediateTemplateViewModel qcIntermediateTemplateViewModel);
   ResponseViewModel<QCIntermediateTemplateViewModel> UpdateData(QCIntermediateTemplateViewModel qcIntermediateTemplateViewModel);

   ResponseViewModel<QCIntermediateTemplateViewModel> GetIntermediateById(int IntermediateId); 

} 