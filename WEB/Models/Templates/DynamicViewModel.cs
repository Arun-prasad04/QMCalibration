namespace WEB.Models
{
	public class DynamicViewModel
	{
		//Instrument
		public int Id { get; set; }
		public int RequestId { get; set; }

		public int InstrumentId { get; set; }
		public string? FormatNo { get; set; }
		public int RevNo { get; set; }
		public string? Date { get; set; }
		public string? Name { get; set; }
		public string? Range { get; set; }
		public string? Make { get; set; }
		public string? SlNo { get; set; }
		public string? IdNo { get; set; }
		public string? RefStd { get; set; }
		public string? TempStart { get; set; }
		public string? TempEnd { get; set; }
		public string? Humidity { get; set; }
		public string? Condition { get; set; }
		public string? Units { get; set; }
		public string? RefWi { get; set; }
		public string? ReviewedBy { get; set; }
		public int? ReviewStatus { get; set; }
		public DateTime? ReveiwedByDate { get; set; }
		public string? CalibrationReviewedBy { get; set; }
		public DateTime? CalibrationReviewedDate { get; set; }
		public DateTime? CalibrationPerformedDate { get; set; }
		public string? CalibrationPerformedBy { get; set; }
		public int CreatedBy { get; set; }
		public string Grade { get; set; }
		public string Title { get; set; }
		public string SubTitle { get; set; }

		//Content
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
		public List<ObservationContentViewModel> ObservationContentList { get; set; }
		public List<MasterViewModel> MasterEqiupmentList { get; set; }
		public List<ObservationContentValuesViewModel> ObservationContentValuesList { get; set; }
		public List<ObservationContentMappingViewModel> ObservationContentMappingList { get; set; }
		public string? FormatNoTitle { get; set; }
		public int? TemplateObservationId { get; set; }
		public List<UserContentMappingView> UserContentMappingViewModel { get; set; }
	}
	public class UserContentMappingView
	{
		//		Id ContentId   ObservationId InstrumentId    CreatedBy CreatedOn   IsActive

		public int? Id { get; set; }
		public int? ContentId { get; set; }
		public int? ObservationId { get; set; }
		public int? InstrumentId { get; set; }

	}
}
