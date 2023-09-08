namespace WEB.Models
{
    public class MetalRuleResultViewModel
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public string? MeasuedValue { get; set; }
        public string? Actuals { get; set; }
        public string? Diff { get; set; }
        public string? InstrumentError { get; set; }

        public int? MasterView1 { get; set; }
        public int? MasterView2 { get; set; }
        public int? MasterView3 { get; set; }
        //public int? Flatness { get; set; }
        //public int? Parallelism { get; set; }
        //public int? CreatedBy { get; set; }
        //public int? ModifiedBy { get; set; }
        //public DateTime? CreatedOn { get; set; }
        //public DateTime? ModifiedOn { get; set; }
        public int? SNO { get; set; }
        public int? ObservationType { get; set; }
    }
}
