using WEB.Models;
namespace WEB.Services.Interface;

public interface IQRCodeGeneratorService
{
    public QRCodeFilesViewModel QRCodeGeneration(QRCodeFilesViewModel qrCodeFilesViewModels);
    public ResponseViewModel<QRCodeFilesViewModel> InsertQRCodeFiles(List<QRCodeFilesViewModel> qrCodeFilesViewModels);
    public QRCodeFilesViewModel GetQRCodeDetails(QRCodeFilesViewModel qrCodeFilesViewModel);
    public QRCodeFilesViewModel GetQRCodeByGuid(Guid guid);
    public RequestViewModel GetRequestData(int requestId);
    public QRCodeFilesViewModel GetQRCodeDetailsForCertificate(int requestId, int instrumentId);
	public QRCodeFilesViewModel QRCodeGenerationForInstrument(QRCodeFilesViewModel qrCodeGenInputViewModel, int instrumentId, string IdNo);
}