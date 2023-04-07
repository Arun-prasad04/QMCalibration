namespace CMT.DATAMODELS
{
    public partial class ExternalRequestStatus
    {
        public int Id { get; set; }
        public int ExternalRequestId { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? Comment { get; set; }


        public virtual ExternalRequest ExternalRequestModel { get; set; }
        public virtual User UserModel { get; set; }
    }
}