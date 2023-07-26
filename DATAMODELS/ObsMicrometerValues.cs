namespace DATAMODELS
{
	public class ObsMicrometerValues
	{
		public int? Id { get; set; }
		public int? ParentId { get; set; }
		public string? MeasuedValue { get; set; }
		public string? ActualsT1 { get; set; }
		public string? Diff1 { get; set; }
		public int? InstrumentError { get; set; }
		public int? Flatness { get; set; }
		public int? Parallelism { get; set; }
		public int? CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public int? SNO { get; set; }
		
	}
}
