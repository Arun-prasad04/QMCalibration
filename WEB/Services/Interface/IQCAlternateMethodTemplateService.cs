 using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IQCAlternateMethodTemplateService
{
   ResponseViewModel<QCAlternateMethodTemplateViewModel> GetById(int Id);
   ResponseViewModel<QCAlternateMethodTemplateViewModel> InsertData(QCAlternateMethodTemplateViewModel qcalternatemethodtemplateViewModel);
   ResponseViewModel<QCAlternateMethodTemplateViewModel> UpdateData(QCAlternateMethodTemplateViewModel qcalternatemethodtemplateViewModel); 
   ResponseViewModel<QCAlternateMethodTemplateViewModel>GetAllAlternateMethodList();
} 