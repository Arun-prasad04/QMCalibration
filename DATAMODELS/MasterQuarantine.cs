namespace CMT.DATAMODELS
{
    public partial class MasterQuarantine
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public string? Reason { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int StatusId { get; set; }

        public virtual Master Masters{get;set;}
    }
}