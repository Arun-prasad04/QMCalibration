namespace WEB.Models
{

    public class InstrumentViewModel
    {
        public int Id { get; set; }
        public string? InstrumentName { get; set; }
        public string? SlNo { get; set; }
        public string? IdNo { get; set; }
        public string? Range { get; set; }
        public string? LC { get; set; }
        public string? Unit1 { get; set; }
        public string? Unit2 { get; set; }
        public string? AmountJPY { get; set; }
        public string? Capacity { get; set; }
        public string? EquipmentStation { get; set; }
        public string? Rule_Confirmity { get; set; }
        public string? Remarks1 { get; set; }
        public string? Comment { get; set; }
        public int? Instrument_Type { get; set; }
        public int CalibFreq { get; set; }
        public DateTime? CalibDate { get; set; }
        public DateTime DueDate { get; set; }
        public int UserDept { get; set; }
        public string? Make { get; set; }
        public string? CalibSource { get; set; }
        public string? StandardReffered { get; set; }
        public string? Remarks { get; set; }
        public int TemplateName { get; set; }
        public int Status { get; set; }
        public List<IFormFile> ImageUpload { get; set; }
        public int CalibrationStatus { get; set; }
        public int InstrumentStatus { get; set; }
        public DateTime? DateOfReceipt { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string MasterInstrumentName1 { get; set; }
        public string MasterInstrumentName2 { get; set; }
        public string MasterInstrumentName3 { get; set; }
        public string MasterInstrumentName4 { get; set; }
        public string Result { get; set; }
        public bool ActiveStatus { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public List<LovsViewModel> InstrumentStatusList { get; set; }
        public List<LovsViewModel> StatusList { get; set; }
        public List<LovsViewModel> TemplateNameList { get; set; }
        public List<LovsViewModel> CalibFreqList { get; set; }
        public List<LovsViewModel> CalibrationStatusList { get; set; }
        public bool? IsQuarantine { get; set; }
        public string? QuarantineReason { get; set; }
        public DateTime? QuaraantineOn { get; set; }
        public bool? IsNABL { get; set; }
        public List<string> FileList { get; set; }
        //public bool IsActive	{get;set;}
        public int NewReqAcceptStatus { get; set; }
        public int? ObservationTemplate { get; set; }
        public int? ObservationType { get; set; }
        public int? MUTemplate { get; set; }
        public int? CertificationTemplate { get; set; }
        public List<LovsViewModel> ObservationTemplateList { get; set; }
        public List<LovsViewModel> MUTemplateList { get; set; }
        public List<LovsViewModel> CertificationTemplateList { get; set; }
        public List<MasterViewModel> MasterData { get; set; }

        public int? MasterInstrument1 { get; set; }
        public int? MasterInstrument2 { get; set; }
        public int? MasterInstrument3 { get; set; }
        public int? MasterInstrument4 { get; set; }
        public List<MasterViewModel> MasterEqiupmentList { get; set; }
        public LeverTypeDialViewModel LeverDialData { get; set; }
        public string CustomerName { get; set; }
        public string DepartmentName { get; set; }
        public string IsDisabled { get; set; }
        public string RuleConfirmityStatement { get; set; }
        public bool isExportCertificate { get; set; }
        public MicrometerViewModel MicrometerData { get; set; }
        public PlungerDialViewModel PlungerDialData { get; set; }
        public ThreadGaugesViewModel ThreadGaugeData { get; set; }
        public GeneralViewModel GeneralData { get; set; }
        public TorqueWrenchesViewModel TorqueData { get; set; }
        public VernierCaliperViewModel VernierData { get; set; }
        public GeneralNewViewModel GeneralNewData { get; set; }
        public List<DepartmentViewModel> Departments { get; set; }
        public int? UserRoleId { get; set; }
        public int? RequestStatus { get; set; }
        public string? Grade { get; set; }
        public int? RequestId { get; set; }
        public string? TypeOfEquipment { get; set; }
        public string? ToolInventory { get; set; }
        public int? ToolInventoryStatus { get; set; }
        public int? InstrumentCount { get; set; }
        public string? ReplacementLabID { get; set; }
        public int? ToolRoomStatus { get; set; }

        public string? SubSecCode { get; set; }
        public CertificateViewModel TempObservationData { get; set; }

        public List<ObservationContentViewModel> obsContent { get; set; }
		public DateTime? CalibrationCloseDate { get; set; }
        public string IssueNo { get; set; }

		public string Inspectiondetails { get; set; }

		public string SectionCode { get; set; }

		public string InstrumentType { get; set; }
		public string? CalibrationMonth { get; set; }

        public string? CalibrationRequestDate { get; set; }
	}
    public class Instrumentids
    {
        public string InstrumentId { get; set; }

        public string ReplacementLabId { get; set; }
		 
       
		


	}

	public class RequestAllView
    {
        public int instrumentId { get; set; }
        public int TypeValue { get; set; }
        public string EmailServiceNo { get; set; }

	}
		public class RequestMailList
    {
        public int SNo { get; set; }
        public string? RequestNo { get; set; }
        public string? LabId { get; set; }
        public string? EquipmentType { get; set; }
        public string? EquipmentName { get; set; }
        public string? SubsectionCode { get; set; }
        public string? CalibrationType { get; set; }

        public string? CreaterFirstName { get; set; }
        public string? CreaterLastName { get; set; }
        public string? CreaterEmail { get; set; }

    }

    public class DueInstrument
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public string IdNo { get; set; }
        public string EquipmentType { get; set; }
        public string InstrumentName { get; set; }
        public string Class { get; set; }
        public string Location { get; set; }
        public string SubSectionCode { get; set; }
        public string SectionName { get; set; }
        public string TypeofScope { get; set; }
        public DateTime DueDate { get; set; }
        public string ToolRoom { get; set; }
        public int DeptId { get; set; }
        public int InstrumentCreatedBy { get; set; }
        public int RequestId { get; set; }
    }
	public class RequestAllData
	{
		public int requestId { get; set; }
		public string Inspectiondetails { get; set; }

	}
}
