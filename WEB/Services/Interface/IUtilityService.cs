using CMT.DATAMODELS;
using WEB.Models;
using System.Security.Cryptography;
namespace WEB.Services.Interface; 
public interface IUtilityService{
    string Decrypt(string cipherText);
    string Encrypt(string clearText);
    string UploadImage(IFormFile postedFile, string UploadFolder);
   string SaveFiles(string FileData, string FileName, string Refno, int TemplateId, int ContentId);
}