namespace WEB.Models{

public class MasterViewModel
{
 public int Id{get;set;}
    public string? EquipName{get; set;}
    public int LocationId {get;set;}
    public string PONo{get; set;}
    public DateTime? PODate{get;set;}
    public DateTime? CommissionedOn{get;set;}
    public string? Make{get; set;}
    public decimal Cost{get;set;}
    public int CurrencyId{get;set;}
    public int CalibrationSourceId{get;set;}
    public string? Supplier{get;set;}
    public int CalibFreqId{get;set;}
    public DateTime? CalibDate{get;set;}
    public DateTime? DueDate{get;set;}
    public string? Range{get;set;}
    public string SerialNo{get;set;}

    public string? LabId{get;set;}
    public string? CertNo{get;set;}
    public string? Traceability {get;set;}
    public List<IFormFile> ImageUpload{get;set;}
    public string? Name{get;set;}
    public string? PhoneNo{get;set;}
    public string? EmailId{get; set;}
    public string? MobileNo{get;set;}

    public int CreatedBy	{get;set;}
    public int? ModifiedBy	{get;set;}
    public DateTime? CreatedOn	{get;set;}
    public DateTime? ModifiedOn	{get;set;}
    public bool IsActive	{get;set;}
    public bool? IsQuarantine {get;set;}
    public string? QuarantineReason{get;set;}
    public DateTime? QuaraantineOn{get;set;}
    public List<LovsViewModel> LocationList{get;set;}
    public List<LovsViewModel> CurrencyList{get;set;}
     public List<LovsViewModel> CalibrationSourceList{get;set;}
    public List<LovsViewModel> CalibrationFreq{get;set;}
    public int SupplierId { get; set; }
    public List<string> FileList{get;set;}
    public string? CalibrationFrequency {get;set;}

	public string? EquipNameJP { get; set; }
	public string? EquipmentMasterId { get; set; }

	//public string? TypeOfEquipment { get; set; }

    public int? DepartId { get; set; }

	public List<DepartmentViewModel> Departments { get; set; }
	public string? DepartmentName { get; set; }

	}
}