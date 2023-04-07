namespace CMT.DATAMODELS
{
    public partial class QCAlternateMethodTemplate
    {
        public int Id { get; set; }
        public string FormatNo { get; set; }
        public string RevisionNo { get; set; }
        public DateTime RevisionDate { get; set; }
        public int InstrumentId { get; set; }
        public string? RangeOrSize { get; set; }
        public int? LC { get; set; }
        public string? InstrumentName { get; set; }
        public int? MasterId1 { get; set; }
        public string? Master1Name { get; set; }
        public DateTime? Master1DateOfCalibration { get; set; }
        public decimal? Mux1 { get; set; }
        public int? MasterId2 { get; set; }
        public string? Master2Name { get; set; }
        public DateTime? Master2DateOfCalibration { get; set; }
        public decimal? Mux2 { get; set; }
        public decimal? Mux1AvgValue { get; set; }
        public decimal? Mux2AvgValue { get; set; }
        public decimal? Mux1SqrValue { get; set; }
        public decimal? Mux2SqrValue { get; set; }
        public decimal? EnValue { get; set; }
        public string? Conclusion { get; set; }
        public int? EvaluatedBy { get; set; }
        public DateTime? EvaluationOn { get; set; }
        public Int16? TemplateStatus { get; set; }
        public decimal? MesuredValuexOne { get; set; }
        public decimal? MesuredValueXTwo { get; set; }
        public int? ReviewedBy { get; set; }
        public DateTime? ReviewedOn { get; set; }
        public string? ReviewerRemarks { get; set; }
        public string? MUx1FileName  { get; set; }
        public string? MUx2FileName  { get; set; }
        public string? DataUnit {get;set;} 
        public string? InstrumentLabId	{get;set;}
        public string? Master1LabId{get;set;}
        public string? Master2LabId{get;set;}
    }
}