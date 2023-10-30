namespace CMT.DATAMODELS
{
	public class EmailServiceStatus
	{//Id	InstrumentId	EmailServiceNo	TypeOfRequest	Status	CreatedBy	CreatedOn	RequestId
		public int Id { get; set; }
		public int? InstrumentId { get; set; }
		public string? EmailServiceNo { get; set; }
		public string? TypeOfRequest { get; set; }
		public int? Status { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? RequestId { get; set; }
		
	}
}
