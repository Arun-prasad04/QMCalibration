public class QRCodeFiles
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
}