namespace CMT.DATAMODELS
{
    public class ObsTemplateGeneralNew
    {
        public int Id { get; set; }
        public int? ObservationId { get; set; }
        public string? EnvironmentCondition { get; set; }
        public string? Uncertainity { get; set; }

        public string? CalibrationResult{get;set;}
        public string? Remarks{get;set;}

        public int? CalibrationReviewedBy { get; set; }
        public DateTime CalibrationReviewedDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string? CalibrationPerformedBy { get; set; }
        public string? ReviewedBy { get; set; }

        public string? CalibrationPerformedDate { get; set; }
        public string? ReviewedDate { get; set; }
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

        public virtual TemplateObservation Observation{get;set;}
    }
}