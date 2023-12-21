namespace WEB.Models
{
	public class ObservationContentValuesViewModel
	{

		public int? Id { get; set; }
		public int? ParentId { get; set; }
		public int? Sno { get; set; }
		public string? MeasuedValue { get; set; }
		public string? ActualValue { get; set; }
		public string? InstrumentError { get; set; }
		public string? Diff { get; set; }
		public string? MeasuedValue1 { get; set; }
		public string? MeasuedValue2 { get; set; }
		public string? MeasuedValue3 { get; set; }
		public string? Average { get; set; }
		public string? Percent { get; set; }
		public int? ContentId { get; set; }
		public int? ContentMappingId { get; set; }

		public string? PermissibleLimit { get; set; }



	}
}
