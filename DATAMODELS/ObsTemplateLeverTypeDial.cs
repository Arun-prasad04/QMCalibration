namespace CMT.DATAMODELS
{
    public class ObsTemplateLeverTypeDial
    {
        public int Id{get;set;}
        public int ObservationId{get;set;}
        public string? DirectionA1 { get; set; }
        public string? DirectionA2 { get; set; }
        public string? DirectionA3 { get; set; }
        public string? DirectionA4 { get; set; }
        public string? DirectionB1 { get; set; }
        public string? DirectionB2 { get; set; }
        public string? DirectionB3 { get; set; }
        public string? DirectionB4 { get; set; }
        public string? Remarks { get; set; }
        public string? Uncertainity { get; set; }
        public string? CalibrationResult { get; set; }
        public string? EnvironmentCondition { get; set; }
        public string? Specification1 { get; set; }
        public string? Specification2 { get; set; }
        public string? Specification3 { get; set; }
        public string? Specification4 { get; set; }

        public virtual TemplateObservation Observation{get;set;}
 
    }
}