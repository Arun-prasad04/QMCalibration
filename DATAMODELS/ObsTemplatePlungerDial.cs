namespace CMT.DATAMODELS
{
    public class ObsTemplatePlungerDial
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int ObservationId { get; set; }
        public string? EnvironmentCondition { get; set; }
        public string? Uncertainity { get; set; }
        public string? CalibrationResult { get; set; }
        public string? Spec1 { get; set; }
        public string? Spec2 { get; set; }
        public string? Spec3 { get; set; }
        public string? Spec4 { get; set; }
        public string? Spec5 { get; set; }
        public string? Spec6 { get; set; }
        public string? Actual1 { get; set; }
        public string? Actual2 { get; set; }
        public string? Actual3 { get; set; }
        public string? Actual4 { get; set; }
        public string? Actual5 { get; set; }
        public string? Actual6 { get; set; }
        public string? Interval1 { get; set; }
        public string? Interval2 { get; set; }
        public string? Interval3 { get; set; }
        public string? Interval4 { get; set; }
        public string? Interval5 { get; set; }
        public string? Remarks { get; set; }
        public int? CalibrationReviewedBy { get; set; }
        public DateTime CalibrationReviewedDate { get; set; }
        public virtual TemplateObservation Observation{get;set;}
    }
}