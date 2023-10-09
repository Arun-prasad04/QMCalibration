namespace CMT.DATAMODELS
{
	public class ObservationContentMapping
	{
		public int? Id { get; set; }
		public int? ContentId { get; set; }
		public int? ObservationId { get; set; }
		public int? InstrumentId { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
	}
}
