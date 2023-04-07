namespace CMT.DATAMODELS
{
    public partial class RequestStatus
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int StatusId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? Comment { get; set; }
        public virtual Request RequestModel { get; set; }
        public virtual User UserModel { get; set; }
    }
}
