namespace WEB.Models
{

    public class QCIntermediateTemplateResultViewModel{
        public int? Id {get; set;}
        public int? ParentId { get; set; }
        public decimal? CalibrationResult { get; set; }
        public decimal? CurrentInternalCheck	{ get; set; }
        public decimal? DifferenceResult { get; set; }
        public string? AcceptanceCriteria { get; set; }
        public string? Decision { get; set; }
        
        
    }
}