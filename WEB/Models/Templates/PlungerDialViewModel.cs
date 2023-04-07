namespace WEB.Models;
public class PlungerDialViewModel
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string Name { get; set; }
    public string RangeLC { get; set; }
    public string Make { get; set; }
    public string SerialNumber { get; set; }
    public string IdNumber { get; set; }
    public string ReferenceStandard { get; set; }
    public string TempStart { get; set; }
    public string TempEnd { get; set; }
    public string Humidity { get; set; }
    public string ConditionAndObservation { get; set; }
    public string RefWi { get; set; }
    public string Allvalues { get; set; }
    public string Spec1 { get; set; }
    public string Spec2 { get; set; }
    public string Spec3 { get; set; }
    public string Spec4 { get; set; }
    public string Spec5 { get; set; }
    public string Spec6 { get; set; }
    public string Actual1 { get; set; }
    public string Actual2 { get; set; }
    public string Actual3 { get; set; }
    public string Actual4 { get; set; }
    public string Actual5 { get; set; }
    public string Actual6 { get; set; }
    public string Interval1 { get; set; }
    public string Interval2 { get; set; }
    public string Interval3 { get; set; }
    public string Interval4 { get; set; }
    public string Interval5 { get; set; }

    public string CalibrationPerformedBy { get; set; }
    public DateTime? CalibrationPerformedDate { get; set; }
    public int CalibrationReviewedBy { get; set; }
    public string ReviewedBy { get; set; }

    public int? ULRNumber {get;set;}
    public string? ULRFormat {get;set;}
    public string CertificateFormat {get;set;}
    public string PerformedByDesignation {get;set;}
    public string ReviewedByDesignation {get;set;}
    public string PerformedBySign {get;set;}
    public string ReviewedBySign {get;set;}
    
    public DateTime? CalibrationReviewedDate { get; set; }
    public int? ReviewStatus {get;set;}

    public int InstrumentId { get; set; }
    public int RequestId { get; set; }

    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public string EnvironmentCondition { get; set; }
    public string Uncertainity { get; set; }
    public string CalibrationResult { get; set; }
    public string IsDisabled { get; set; }
    public string Remarks { get; set; }
    public int TemplateObservationId { get; set; }

    public int? ObsSubType{get;set;}
    public string? Review_Date { get; set; }

    public string PdfUncertainity{get;set;}
    public string PdfCalibrationResult{get;set;}
    public string PdfRemarks{get;set;}
    public int? CertificateNumber {get;set;}
}


