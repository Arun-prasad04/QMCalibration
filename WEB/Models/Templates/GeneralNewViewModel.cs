namespace WEB.Models
{
    public class GeneralNewViewModel
    {
        public int Id { get; set; }
        public string? FormatNo { get; set; }
        public string? RevNo { get; set; }
        public string? Name { get; set; }
        public string? Range { get; set; }
        public string? Make { get; set; }
        public string? SerialNo { get; set; }
        public string? IDNo { get; set; }
        public string? RefStd { get; set; }
        public string? TempStart { get; set; }
        public string? TempEnd { get; set; }
        public string? Humidity { get; set; }
        public string? RefWi { get; set; }
        public string? IdNo { get; set; }
        public string ConditionOfVernierCaliper { get; set; }
        public string CalibrationPerformedBy { get; set; }
        public int CalibrationReviewedBy { get; set; }
        public string ReviewedBy { get; set; }

        public DateTime CalibrationPerformedDate { get; set; }
        public DateTime CalibrationReviewedDate { get; set; }
        public string Interval { get; set; }        
        public int InstrumentId { get; set; }
        public int RequestId { get; set; }
        public int CreatedBy { get; set; }
        public int? ReviewStatus {get;set;}
        public string EnvironmentCondition { get; set; }
        public string Uncertainity { get; set; }
        public string CalibrationResult { get; set; }
        public string IsDisabled { get; set; }
        public string Remarks { get; set; }

        public string DialIndicatiorCondition { get; set; }
        public string? Allvalues { get; set; }
        public string? Review_Date { get; set; }
        public int TemplateObservationId { get; set; }
        public int? ObsSubType { get; set; }
        public int? ULRNumber {get;set;}
        public string ULRFormat {get;set;}
        public string CertificateFormat {get;set;}
        public string PerformedByDesignation {get;set;}
        public string ReviewedByDesignation {get;set;}
        public string PerformedBySign {get;set;}
        public string ReviewedBySign {get;set;}
        public string PdfUncertainity{get;set;}
        public string PdfCalibrationResult{get;set;}
        public string PdfRemarks{get;set;}
        public int? CertificateNumber {get;set;}

    public string ErrorinDMS1_1 { get; set; }
    public string ErrorinDMS1_2 { get; set; }
    public string ErrorinDMS1_3 { get; set; }
    public string ErrorinDMS1_4 { get; set; }
    public string ErrorinDMS1_5 { get; set; }
    public string ErrorinDMS2_1 { get; set; }
    public string ErrorinDMS2_2 { get; set; }
    public string ErrorinDMS2_3 { get; set; }
    public string ErrorinDMS2_4 { get; set; }
    public string ErrorinDMS3_1 { get; set; }
    public string ErrorinDMS3_2 { get; set; }
    public string ErrorinDMS3_3 { get; set; }
    public string ErrorinDMS3_4 { get; set; }
    public string ErrorinDMS4_1 { get; set; }
    public string ErrorinDMS4_2 { get; set; }
    public string ErrorinDMS4_3 { get; set; }
    public string Straightness_spec {get;set;}
    public string Straightness_Actual{get;set;}

    public string Straightness_DevfromNom{get;set;}
    public string Parallelism_Spec{get;set;}
    
    public string Parallelism_Actual{get;set;}
    public string Parallelism_DevfromNom{get;set;}
    public string FlatnessofBlade_spec_1{get;set;}
    public string FlatnessofBlade_Actual_1{get;set;}
    public string FlatnessofBlade_DevfromNom_1 { get; set; }
    public string FlatnessofBlade_spec_2 {get;set;}

    public string FlatnessofBlade_Actual_2 {get;set;}
    public string FlatnessofBlade_DevfromNom_2 {get;set;}

 }

}