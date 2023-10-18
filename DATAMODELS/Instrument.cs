namespace CMT.DATAMODELS
{
    public partial class Instrument
    {
    public int Id{get; set;}
    public string? InstrumentName{get;set;}
    public string? SlNo	{get;set;}
    public string? IdNo	{get;set;}
    public string? Range {get;set;}
    public string? LC {get;set;}
    public string? Unit1 {get;set;}
    public string? Unit2 {get;set;}
    public string? AmountJPY { get;set;}
    public string? Capacity { get;set;}
    public string? EquipmentStation { get;set;}
    public string? Rule_Confirmity {get;set;}
    public string? Remarks1 {get;set;}
    public string? Comment { get;set;}
    public int? Instrument_Type {get; set;}
    public int CalibFreq {get; set;}
    public DateTime? CalibDate {get; set;}
    public DateTime DueDate {get; set;}
    public int UserDept{get; set;}
    public string? Make{get; set;}
    public string? CalibSource{get; set;}
    public string? StandardReffered{get; set;}
    public string? Remarks {get; set;}
    public int Status{get; set;}
    public int CalibrationStatus{get; set;}
    public int InstrumentStatus{get; set;}
    public DateTime? DateOfReceipt{get; set;}
     public bool ActiveStatus {get;set;}
    public int CreatedBy {get;set;}
    public int? ModifiedBy {get;set;}
    public DateTime? CreatedOn {get;set;}
    public DateTime? ModifiedOn {get;set;}
    public bool? IsNABL{get;set;}
    public virtual List<InstrumentQuarantine> QuarantineModel{get;set;}
    public virtual List<InstrumentFileUpload> FileUploadModel{get;set;}

    public virtual List<Request> RequestModel{get;set;}
    public virtual Department DepartmenttModel{get;set;}
     public int? ObservationTemplate {get;set;}
    public int? ObservationType {get;set;}
    public int? MUTemplate {get;set;}
    public int? CertificationTemplate {get;set;}

    public int? MasterInstrument1 {get;set;}
    public int? MasterInstrument2 {get;set;}
    public int? MasterInstrument3 {get;set;}
    public int? MasterInstrument4 {get;set;}
    public virtual User UserModel{get;set;}
	public string? Grade { get; set; }
	public string? TypeOfEquipment { get; set; }
	public string? ToolInventory { get; set; }
	public int? ToolInventoryStatus { get; set; }
    public string? ReplacementLabID { get; set; }
	public int? ToolRoomStatus { get; set; }	

	}
}