using CMT.DATAMODELS;

public class TemplateObservation
{
    public int Id { get; set; }
    public int RequestId { get; set; }
    public int InstrumentId { get; set; }
    public string? TempStart { get; set; }
    public string? TempEnd { get; set; }
    public string? Humidity { get; set; }
    public string? InstrumentCondition { get; set; }
    public string? RefWi { get; set; }
    public string? Allvalues { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public int? ReviewStatus { get; set; }
    public int? ULRNumber { get; set; }
    public int? CertificateNumber { get; set; }
    public DateTime CalibrationReviewedDate { get; set; }
    public int CalibrationReviewedBy { get; set; }
    public virtual List<ObsTemplateLeverTypeDial> LeverTypeDialModel { get; set; }
    public virtual List<ObsTemplateMicrometer> MicromerterModel { get; set; }
    public virtual List<ObsTemplatePlungerDial> PlungerDialModel { get; set; }
    public virtual List<ObsTemplateThreadGauges> ThreadGaugesDialModel { get; set; }
    public virtual List<ObsTemplateTWobs> TwosModel { get; set; }
    public virtual List<ObsTemplateGeneral> GeneralModel { get; set; }
    public virtual List<ObsTemplateVernierCaliper> VerniercaliperModel { get; set; }
    public virtual List<ObsTemplateGeneralNew> GeneralNewModel { get; set; }
    public virtual List<ObsTemplateMetalRules> MetalRulesModel { get; set; }
    public virtual User CalibrationCreatedModel { get; set; }
    public virtual User CalibrationReviewedModel { get; set; }

    public int? ExternalObsStatus { get; set; }

    public string? CalibrationResult { get; set; }
    public string? Remarks { get; set; }
    public string? Units { get; set; }
   // public string? PermissibleLimit { get; set; }
    

}