namespace WEB.Models
{
	public class ObservationContentViewModel
	{

		public int Id { get; set; }
		public int? ObservationTemplate { get; set; }
		public int? ObservationType { get; set; }
		public string? ContentName { get; set; }
		public string? ContentValue { get; set; }
		public string? ContentCount { get; set; }
		public string? ContentTitle1 { get; set; }
		public string? ContentTitle2 { get; set; }
		public string? ContentSubTitle1 { get; set; }
		public string? ContentSubTitle2 { get; set; }
		public string? ContentSubTitle3 { get; set; }
		public string? ContentSubTitle4 { get; set; }
		public string? ContentSubTitle5 { get; set; }
		public string? TypeOfContent { get; set; }
		/////////////////test		///
		public int? ObsContentValueId { get; set; }
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

	}
	public class Contentids
	{
		public int ContentId { get; set; }
		public int InstrumentId { get; set; }
		public int RequestId { get; set; }


	}

}
