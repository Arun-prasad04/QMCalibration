using System.Diagnostics.Metrics;

namespace CMT.DATAMODELS
{
	internal partial class ObservationContent
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
		public string? TypeOfContent { get; set; }
      //  Id ObservationTemplate ObservationType ContentName ContentValue ContentCount    ContentTitle1 ContentTitle2   ContentSubTitle1 ContentSubTitle2    ContentSubTitle3 IsActive    FormatNoTitle FormatNoValue   ContentSubTitle4 ContentSubTitle5    TypeOfContent ContentSubTitle6
	//73	162	Flatness 基準面の平面度／Flatness	1	NULL NULL    測定値／  Measured value    実測／Actual value 器差／Instrumental error	1	NULL NULL    NULL NULL    IN NULL


    }
}
