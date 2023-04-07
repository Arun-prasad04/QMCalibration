namespace CMT.DATAMODELS
{
    public class ObsTemplateTWobs
    {
        public int Id { get; set; }
        public int ObservationId { get; set; }
        public string? EnvironmentCondition { get; set; }
        public string? Uncertainity { get; set; }
        public string? CalibrationResult{get;set;}
        public string? Remarks{get;set;}
        public int CalibrationReviewedBy { get; set; }
        public DateTime CalibrationReviewedDate { get; set; }
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
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }
        public string? Value3 { get; set; }
        public string? Value4 { get; set; }
        public string? Value5 { get; set; }
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
        public virtual TemplateObservation Observation{get;set;}
    }
}