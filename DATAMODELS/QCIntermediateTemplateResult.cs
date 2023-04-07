namespace CMT.DATAMODELS
{

    public partial class QCIntermediateTemplateResult
    {
        public int? Id {get; set;}
        public int? ParentId { get; set; }
        public decimal? CalibrationResult { get; set; }
        public decimal? CurrentInternalCheck	{ get; set; }
        public string? AcceptanceCriteria { get; set; }
        public decimal? DifferenceResult { get; set; }
        public string? Decision { get; set; }

        


    }
}