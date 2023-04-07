namespace CMT.DATAMODELS
{
    public class ObsGeneralMeasuredValues
    {
        public int? Id { get; set; }
        public int? ParentId { get; set; }
        public decimal? MeasuedValue { get; set; }
        public decimal? Trial1 { get; set; }
        public decimal? Trial2 { get; set; }
        public decimal? Trial3 { get; set; }
        public decimal? Average { get; set; }
    }
}