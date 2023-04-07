namespace WEB.Models;

public class ExternalRequestViewModel
{
        public int Id{get;set;}
        public string ReqNo{get; set;}
        public DateTime? RequestDate{get; set;}
        public string? MasterName{get;set;}
        public string MasterSerialNo{get;set;}
        public string MasterIdNo{get;set;}
        public DateTime? CalibrationDate{get; set;}
        public DateTime? NextDue{get; set;}
        public string CertificateNo {get;set;}
        public string? Traceability{get; set;}
        public int AcceptOrReject{get; set;}
        public DateTime? SubmittedOn{get;set;}
        public string? VisualCheckFm {get;set;}
        public string? RecordBy {get;set;}
        public string? ResultFM{get;set;}
        public string? ResultLAB{get;set;}
        public DateTime? ClosedDate{get;set;}
        public string? ReturnToLab{get;set;}
        public DateTime? ReturnDate{get;set;}
        public string? VisualCheckLab{get;set;}
        public string? RecodedByLAB {get;set;}
        public int Status{get;set;}
        public int CreatedBy{get;set;}
        public int ModifiedBy{get;set;}
        public DateTime? CreatedOn{get;set;}
        public DateTime? ModifiedOn{get;set;}
        public string? RejectReason{get;set;}        
}
