namespace CMT.DATAMODELS
{
    public class ObsTemplateThreadGauges
    {
        public int Id { get; set; }
        public int ObservationId { get; set; }
        public string? EnvironmentCondition { get; set; }
        public string? Uncertainity { get; set; }
        
        public string? CalibrationResult { get; set; }
        
        public string? Remarks{ get; set; }
        public int CalibrationReviewedBy { get; set; }
        public DateTime CalibrationReviewedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Min1 { get; set; }
        public string? Max1 { get; set; }
        public string? WearLimit1 { get; set; }
        public string? Min2 { get; set; }
        public string? Max2 { get; set; }
        public string? WearLimit2 { get; set; }
        public string? Plane1 { get; set; }
        public string? Plane2 { get; set; }
        public string? Plane3 { get; set; }
        public string? Plane4 { get; set; }
        public string? Plane5 { get; set; }
        public string? Repeatability1 { get; set; }
        public string? Repeatability2 { get; set; }
        public string? Repeatability3 { get; set; }
        public string? Repeatability4 { get; set; }
        public string? Repeatability5 { get; set; }
        public string? CalibratedBy { get; set; }
        public string? CalibDate { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewDate { get; set; }
        public virtual TemplateObservation Observation{get;set;}
    }
}