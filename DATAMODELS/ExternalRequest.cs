namespace CMT.DATAMODELS
{
    public partial class ExternalRequest
    {
        public int Id{get;set;}
        public string ReqNo{get; set;}
        public DateTime? RequestDate{get; set;}
        public int MasterId{get;set;}
        public int StatusId{get;set;}
        public int CreatedBy{get;set;}
        public int ModifiedBy{get;set;}
        public DateTime? CreatedOn{get;set;}
        public DateTime? ModifiedOn{get;set;}

        public virtual Master MasterModel{get;set;}
        public virtual List<ExternalRequestStatus> ExternalRequestStatusModal{get;set;}
    }
}