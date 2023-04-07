namespace CMT.DATAMODELS
{
    public partial class QCReplicateTestTemplate
    {
    public int Id{get;set;}
    public string? FormatNo { get; set; }
    public string? RevisionNo { get; set; }
    public DateTime? RevisionDate { get; set; }
    public DateTime? DateConducted { get; set; }
    public string? InstrumentName { get; set; }
    public int? InstrumentId { get; set; }
    public string? RangeOrSize { get; set; }
    public int? LC { get; set; }
    public int? MasterEquipmentId { get; set; }
    public string? MasterEquipmentName { get; set; }
    public Decimal? Temperature { get; set; }
    public Decimal? Humidity { get; set; }    
    public Decimal? EnValue { get; set; }
    public string? Conclusion { get; set; }     
    public string? ReviewedBy { get; set; }
    public DateTime? ReviewedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public Int16 DocumentStatus	{get; set;}
    public string? FinalStatus{get; set;}
    public string? DataUnit {get;set;} 
    public string? MUx1FileName  { get; set; }
    public string? MUx2FileName  { get; set; }

    public string? InstrumentLabId	{get;set;}
    public string? MasterLabId{get;set;}
    
    }
}