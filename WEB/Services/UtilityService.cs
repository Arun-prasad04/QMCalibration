using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using System.Security.Cryptography;
using System.Text;
using NPOI.HPSF;


namespace WEB.Services;


public class UtilityService : IUtilityService
{

	private IConfiguration _configuration;

	private Microsoft.AspNetCore.Hosting.IHostingEnvironment Environment;

    public UtilityService(Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment, IConfiguration configuration)
    {
        Environment = _environment;
		_configuration = configuration;

	}

    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    public string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }
	public string SaveFiles(string FileData, string FileName, string Refno,int TemplateId,int ContentId)
	{
		string wwwPath = this.Environment.WebRootPath;
		string contentPath = this.Environment.ContentRootPath;

		string filePath = Path.Combine(this.Environment.WebRootPath, "Observation");

		//string filePath = Path.Combine(this.Environment.WebRootPath, "Observation");
		//var filePath = "C:\\Users\\prasaar\\Desktop\\datas\\observation";// _configuration.GetSection("AppSettings")["UploadFile"];
		//var filePath = _configuration.GetSection("AppSettings")["UploadFile"];
		
		var bytes = Convert.FromBase64String(FileData.Split(',')[1]);
       
		if (!Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
         //filePath = filePath + "\\" + Refno + "\\" + FileName;
        using (var imageFile = new FileStream(Path.Combine(filePath, FileName), FileMode.Create))
        {

            imageFile.Write(bytes, 0, bytes.Length);
            imageFile.Flush();
        }
        //using (FileStream stream = new FileStream(Path.Combine(filePath, FileName), FileMode.Create))
        //{
        //    postedFile.CopyTo(stream);
        //}
        return Path.Combine(filePath, FileName);// filePath + FileName;
	}
	public string UploadImage(IFormFile postedFile, string UploadFolder)
    {
        string wwwPath = this.Environment.WebRootPath;
        string contentPath = this.Environment.ContentRootPath;

        string path = Path.Combine(this.Environment.WebRootPath, UploadFolder);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);

        }

        
            string fileName = postedFile.FileName;
            //string fileName = Path.GetFileName(postedFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }
        
        return path+fileName;
    }
}




