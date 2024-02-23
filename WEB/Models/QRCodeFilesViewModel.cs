namespace WEB.Models;
public class QRCodeFilesViewModel
{
        public int Id {get;set;}
        public int InstrumentId { get; set; }
        public int RequestId { get; set; }
        public string? RequestNo {get;set;}
        public Guid UrlGuid {get;set;}
        public string? FileName {get;set;}
        public string? AmendmentNo {get;set;}
        public int CreatedBy {get;set;}
        public DateTime CreatedOn {get;set;}
        public string TemplateName { get; set; }
        public string EncodeInputText { get; set; }
        public string QRFilepath { get; set; }
        public string QRFilename { get; set; }
        public string QRImageUrl { get; set; }
        public string DrawText { get; set; }
     public string InstrumentIdNo { get; set; }

    public Byte[] DecodeText { get; set; } 
}