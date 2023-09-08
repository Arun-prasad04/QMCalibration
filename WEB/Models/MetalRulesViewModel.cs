namespace WEB.Models
{

	public class MetalRulesViewModel
	{
		public int Id { get; set; }
		public string? FormatNo { get; set; }
		public int RevNo { get; set; }
		public string? Date { get; set; }
		public string? Name { get; set; }
		public string? Range { get; set; }
		public string? Make { get; set; }
		public string? SerialNo { get; set; }
		public string? IdNo { get; set; }
		public string? RefStd { get; set; }
		public string? TempStart { get; set; }
		public string? TempEnd { get; set; }
		public string? Humidity { get; set; }
		public string? MetalRulesCondition { get; set; }
		public string? Allvalues { get; set; }
		public string? RefWi { get; set; }
		public int LinearAccuracy { get; set; }
		public string? IntervalMain { get; set; }
		public int FlatnessSpec { get; set; }
		public string? Flatness1 { get; set; }
		public string? Flatness2 { get; set; }
		public string? ParallelismSpec { get; set; }
		public string? Actuals { get; set; }
		public string? Unit1 { get; set; }

		public string? ReviewedBy { get; set; }
		public int? ReviewStatus { get; set; }
		public DateTime? ReveiwedByDate { get; set; }
		public int? CalibrationReviewedBy { get; set; }
		public DateTime? CalibrationReviewedDate { get; set; }
		public DateTime? CalibrationPerformedDate { get; set; }
		public string? CalibrationPerformedBy { get; set; }

		public int? ULRNumber { get; set; }
		public string ULRFormat { get; set; }
		public string CertificateFormat { get; set; }
		public string PerformedByDesignation { get; set; }
		public string ReviewedByDesignation { get; set; }
		public string PerformedBySign { get; set; }
		public string ReviewedBySign { get; set; }


		public int InstrumentId { get; set; }
		public int RequestId { get; set; }
		public string IsDisabled { get; set; }

		public int CreatedBy { get; set; }
		
		public string CalibrationResult { get; set; }
		public string Remarks { get; set; }
		public string MURemarks { get; set; }
		public int TemplateObservationId { get; set; }
		public string? Review_Date { get; set; }
		public string? Uncertainity { get; set; }
		public string PdfCalibrationResult { get; set; }
		public string PdfRemarks { get; set; }

		public int? CertificateNumber { get; set; }
		public List<MasterViewModel> MasterEqiupmentList { get; set; }
		public string? Grade { get; set; }

        public List<MetalRuleResultViewModel> MetalRuleAddResultViewModelList { get; set; }
        public List<MetalRuleResultViewModel> MetalRuleAddResultViewModelList1 { get; set; }
		public List<MetalRuleResultViewModel> MetalRuleAddResultViewModelList2 { get; set; }
		public int? ObservationTypeId { get; set; }		
		public string InstrumentErrValue { get; set; }
        

    }
}
