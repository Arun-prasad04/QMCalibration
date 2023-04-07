namespace CMT.DATAMODELS
{
    public partial class InstrumentQuarantine
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public string? Reason { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int StatusId { get; set; }
        public virtual Instrument Instruments{get;set;}
    }
}