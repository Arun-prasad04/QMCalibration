namespace CMT.DATAMODELS
{
    public partial class ObsTemplateMicrometer
    {
        public int Id { get; set; }
        public int ObservationId { get; set; }
        public string? EnvironmentCondition { get; set; }
        public string? Uncertainity { get; set; }
        public string? Remarks { get; set; }
        public string? CalibrationResult { get; set; }
        public int CalibrationReviewedBy { get; set; }
        public DateTime CalibrationReviewedDate { get; set; }
        public string? Flatness1 { get; set; }
        public string? Flatness2 { get; set; }
        public string? ParallelismSpec { get; set; }
        public string? Actuals { get; set; }
        public string? CalibrationPerformedBy { get; set; }
        public string? ReviewedBy { get; set; }
        public DateTime? CalibrationPerformedDate { get; set; }
        public string? ReveiwedByDate { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public string? ActualsT11 { get; set; }
        public string? ActualsT21 { get; set; }
        public string? ActualsT31 { get; set; }
        public string? Avg1 { get; set; }
        public string? MuInterval1 { get; set; }
        public string? ActualsT12 { get; set; }
        public string? ActualsT22 { get; set; }
        public string? ActualsT32 { get; set; }
        public string? Avg2 { get; set; }
        public string? MuInterval2 { get; set; }
        public string? ActualsT13 { get; set; }
        public string? ActualsT23 { get; set; }
        public string? ActualsT33 { get; set; }
        public string? Avg3 { get; set; }
        public string? MuInterval3 { get; set; }
        public string? ActualsT14 { get; set; }
        public string? ActualsT24 { get; set; }
        public string? ActualsT34 { get; set; }
        public string? Avg4 { get; set; }
        public string? MuInterval4 { get; set; }
        public string? ActualsT15 { get; set; }
        public string? ActualsT25 { get; set; }
        public string? ActualsT35 { get; set; }
        public string? Avg5 { get; set; }
        public string? MuInterval5 { get; set; }
        public string? ActualsT16 { get; set; }
        public string? ActualsT26 { get; set; }
        public string? ActualsT36 { get; set; }
        public string? Avg6 { get; set; }
        public string? ActualsT17 { get; set; }
        public string? ActualsT27 { get; set; }
        public string? ActualsT37 { get; set; }
        public string? Avg7 { get; set; }
        public string? ActualsT18 { get; set; }
        public string? ActualsT28 { get; set; }
        public string? ActualsT38 { get; set; }
        public string? Avg8 { get; set; }
        public string? ActualsT19 { get; set; }
        public string? ActualsT29 { get; set; }
        public string? ActualsT39 { get; set; }
        public string? Avg9 { get; set; }
        public string? ActualsT110 { get; set; }
        public string? ActualsT210 { get; set; }
        public string? ActualsT310 { get; set; }
        public string? Avg10 { get; set; }
        public string? ActualsT111 { get; set; }
        public string? ActualsT211 { get; set; }
        public string? ActualsT311 { get; set; }
        public string? Avg11 { get; set; }
        public string? Measurement1 { get; set; }
        public string? Measurement2 { get; set; }
        public string? Measurement3 { get; set; }
        public string? Measurement4 { get; set; }
        public string? Measurement5 { get; set; }
        public string? Measurement6 { get; set; }
        public string? Measurement7 { get; set; }
        public string? Measurement8 { get; set; }
        public string? Measurement9 { get; set; }
        public string? Measurement10 { get; set; }
        public string? Measurement11 { get; set; }
        public string? MURemarks { get; set; }
       public virtual TemplateObservation Observation{get;set;}
		public string? FlatnessMeasure { get; set; }
		public string? FlatnessInserr { get; set; }
		public string? FlatnessActual { get; set; }
		public string? InstrumentErrValue { get; set; }





	}
}