namespace WEB.Models
{

    public class TorqueWrenchesViewModel
    {
        public int Id { get; set; }
        public string? FormatNo { get; set; }
        public string? RevNoAndDate { get; set; }
        public string? Name { get; set; }
        public string? Range { get; set; }
        public string? Make { get; set; }
        public string? SerialNo { get; set; }
        public string? IdNo { get; set; }
        public string? RefStd { get; set; }
        public string? RefWi { get; set; }
        public string? TempEnd { get; set; }
        public string? TempStart { get; set; }
        public string? Humidity { get; set; }
        public string ConditionOfTW { get; set; }
        public string WIno { get; set; }
        public string MasterUsed { get; set; }
        public string? Allvalues { get; set; }
        public string? SpecMax { get; set; }
        public string? SpecMin { get; set; }
        public string? ActualInOne { get; set; }
        public string? ActualInTwo { get; set; }
        public string? ActualInThree { get; set; }
        public string? ActualInFour { get; set; }
        public string? ActualInFive { get; set; }
        public string? ActualInSix { get; set; }
        public string? ActualInSeven { get; set; }
        public string? ActualInEight { get; set; }
        public string? ActualInNine { get; set; }
        public string? ActualInTen { get; set; }
        public string? Nominal20 { get; set; }
        public string? Nominal60 { get; set; }
        public string? Nominal100 { get; set; }
        public string? Spec20 { get; set; }
        public string? Spec60 { get; set; }
        public string? Spec100 { get; set; }
        public string? Comments { get; set; }
        public string? CalibBy { get; set; }
        public string? Calib_Date { get; set; }
        public string? Reviewed_By { get; set; }
        public string? Review_Date { get; set; }
        public int? ReviewStatus { get; set; }
        public int InstrumentId { get; set; }
        public int RequestId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }
        public string? Value3 { get; set; }
        public string? Value4 { get; set; }
        public string Value5 { get; set; }
        public string? Value6 { get; set; }
        public string? Value7 { get; set; }
        public string? Value8 { get; set; }
        public string? Value9 { get; set; }
        public string? Value10 { get; set; }
        public string? Value11 { get; set; }
        public string? Value12 { get; set; }
        public string? Value13 { get; set; }
        public string? Value14 { get; set; }
        public string? Value15 { get; set; }
        public string? SetValue { get; set; }
        public int? AWSTransducers { get; set; }
        public int? NorbarTransducers { get; set; }


        public string? EnvironmentCondition { get; set; }
        public string? Uncertainity { get; set; }
        public string? CalibrationResult { get; set; }
        public string? Remarks { get; set; }

        public int? ULRNumber { get; set; }
        public string ULRFormat { get; set; }
        public string CertificateFormat { get; set; }
        public string PerformedByDesignation { get; set; }
        public string ReviewedByDesignation { get; set; }
        public string PerformedBySign { get; set; }
        public string ReviewedBySign { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? CalibrationReviewedDate { get; set; }
        public DateTime? CalibrationPerformedDate { get; set; }
        public string? CalibrationPerformedBy { get; set; }
         public int? CalibrationReviewedBy { get; set; }
        public int TemplateObservationId { get; set; }
        public string? IsDisabled { get; set; }
        public int? ObsSubType { get; set; }
        public string PdfUncertainity{get;set;}
        public string PdfCalibrationResult{get;set;}
        public string PdfRemarks{get;set;}
        public int? CertificateNumber {get;set;}
    }
}