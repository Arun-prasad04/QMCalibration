namespace WEB.Models;
public class LeverTypeDialViewModel
{
    public int Id{get; set;}
    public string? FormatNo {get;set;}
    public string? RevNo{get;set;}
    public string? Name{get;set;}
    public string? Range{get;set;}
    public string? Make{get;set;}
    public string? SerialNo{get;set;}
    public string? IDNo{get;set;}
    public string? RefStd{get;set;}
    public string? TempStart{get;set;}
    public string? TempEnd{get;set;}
    public string? Humidity{get;set;}
    public string? RefWi{get;set;}
    public string? Allvalues{get;set;}
    public string? MeasuringRangeSpec{get;set;}
    public string? MeasuringRangeDirectionA1{get;set;}
    public string? MeasuringRangeDirectionB1{get;set;}
    public string? ScaleDivisionSpec{get;set;}
    public string? ScaleDivisionDirectionA2{get;set;}
    public string? ScaleDivisionDirectionB2{get;set;}
    public string? HysteresisSpec{get;set;}
    public string? HysteresisDirectionA3{get;set;}
    public string? HysteresisDirectionB3{get;set;}
    public string? RepeatabilitySpec{get;set;}
    public string? RepeatabilityDirectionA4{get;set;}
    public string? RepeatabilityDirectionB4{get;set;}
    public string? CalibrationPerformedBy {get;set;}
    public int? CalibrationReviewedBy {get;set;}
    public string? ReviewedBy {get;set;}
    public DateTime CalibrationPerformedDate{get;set;}
    public DateTime CalibrationReviewedDate{get;set;}
    public int InstrumentId{get;set;}
    public int RequestId{get;set;}
    public string? DialIndicatiorCondition{get;set;}
    public int CreatedBy {get;set;}
    public DateTime CreatedOn{get;set;}

    public string IsDisabled {get;set;}

    public string EnvironmentCondition{get;set;}
    public string Uncertainity{get;set;}
    public string CalibrationResult{get;set;}
    public string Remarks{get;set;}
    
    public int? ReviewStatus {get;set;}

    public int? ULRNumber {get;set;}

    public string ULRFormat {get;set;}
    public string CertificateFormat {get;set;}
    public string PerformedByDesignation {get;set;}
    public string ReviewedByDesignation {get;set;}
    public string PerformedBySign {get;set;}
    public string ReviewedBySign {get;set;}

    public int TemplateObservationId { get; set; }
    public string? Review_Date { get; set; }
    public string PdfUncertainity{get;set;}
    public string PdfCalibrationResult{get;set;}
    public string PdfRemarks{get;set;}
    public int? CertificateNumber {get;set;}

}
