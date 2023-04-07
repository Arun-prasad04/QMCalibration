namespace WEB.Models;
public class ReplicateTestViewModel
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
    public string? Remarks { get; set; }
    public int? IsApproved { get; set; }
    public bool? IsRejected { get; set; }
    public DateTime? ReviewedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? DocumentStatus { get; set; }
    public string? Averagex { get; set; }
    public string? AverageX { get; set; }
    public string? ApprovedBy { get; set; }
    public string? MUx { get; set; }
    public string? MUX { get; set; }
    public string? U21 { get; set; }
    public string? U22 { get; set; }
    public int? UserRoleId{ get; set; }
    public string? StudyPerformedBy1 { get; set; }
    public string? StudyPerformedBy2 { get; set; }
    public DateTime? Date1 { get; set; }
    public DateTime? Date2 { get; set; }
    public ReplicateTestDataViewModel Obs1 {get;set;}
    public ReplicateTestDataViewModel Obs2 {get;set;}
    public string? DocumentStatusName {get;set;}
    public string? FinalStatus{get; set;}
    public string? DataUnit {get;set;} 
    public string? MUx1FileName  { get; set; }
    public string? MUx2FileName  { get; set; }
    public IFormFile ImageUpload1 {get;set;}
    public IFormFile ImageUpload2 {get;set;}
    public string ReviewedByName {get;set;}

    public string? RevisionNoAndDate {get;set;}
    public string? InstrumentLabId	{get;set;}
    public string? MasterLabId{get;set;}
}
