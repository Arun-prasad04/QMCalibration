namespace WEB.Models;
public class GeneralViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Range { get; set; }
    public string Size { get; set; }
    public string Make { get; set; }
    public string? SerialNo  { get; set; }
    public string? IdNo  { get; set; }
    public string RefStd { get; set; }
    public string TempStart { get; set; }
    public string TempEnd { get; set; }
    public string Humidity { get; set; }
    public string RefWi { get; set; }
    public string ConditionAndObservation { get; set; }
    public string Allvalues { get; set; }
    public string CalibrationPerformedBy { get; set; }
    public DateTime CalibrationPerformedDate { get; set; }
    public int? CalibrationReviewedBy { get; set; }
    public DateTime CalibrationReviewedDate { get; set; }
    public List<Repeatability> Repeat { get; set; }

    public List<LMeasurement> LeftMeasurement { get; set; }
    public List<RMeasurement> RightMeasurement { get; set; }

    public int InstrumentId { get; set; }
    public int RequestId { get; set; }
    public int CreatedBy { get; set; }
    
    public string? ReviewedBy { get; set; }
    public string? ReviewedDate { get; set; }
    public int? ReviewStatus {get;set;}
    public int TemplateObservationId { get; set; }

    public string EnvironmentCondition{get;set;}
    public string Uncertainity{get;set;}
    public string CalibrationResult{get;set;}
    public string IsDisabled{get;set;}
    public string Remarks { get; set; }
    public string DialIndicatiorCondition {get;set;}
    public List<GeneralResultViewModel> GeneralAddResultViewModelList { get; set; }
    public List<GeneralManualResultViewModel> GeneralManualAddResultViewModelList { get; set; }

    public int? ULRNumber {get;set;}
    public string ULRFormat {get;set;}
    public string CertificateFormat {get;set;}
    public string PerformedByDesignation {get;set;}
    public string ReviewedByDesignation {get;set;}
    public string PerformedBySign {get;set;}
    public string ReviewedBySign {get;set;}
    public string? Review_Date { get; set; }
    public string PdfUncertainity{get;set;}
    public string PdfCalibrationResult{get;set;}
    public string PdfRemarks{get;set;}
    public int? CertificateNumber {get;set;}

}   

public class Repeatability
{

    public int Repeatability1 { get; set; }
    public int Repeatability2 { get; set; }
    public int Repeatability3 { get; set; }
    public int Repeatability4 { get; set; }
    public int Repeatability5 { get; set; }



}
public class RMeasurement
{


    public int RMeasured { get; set; }
    public int RTrial1 { get; set; }
    public int RTrial2 { get; set; }
    public int RTrial3 { get; set; }
}
public class LMeasurement
{
    public int LMeasured { get; set; }
    public int LTrial1 { get; set; }
    public int LTrial2 { get; set; }
    public int LTrial3 { get; set; }
}