namespace CMT.DATAMODELS
{
    public partial class Master
    {
        public int Id { get; set; }
        public string? EquipName { get; set; }
        public int LocationId { get; set; }
        public string PONo { get; set; }
        public DateTime? PODate { get; set; }
        public DateTime? CommissionedOn { get; set; }
        public string? Make { get; set; }
        public decimal Cost { get; set; }
        public int CurrencyId { get; set; }
        public int CalibrationSourceId { get; set; }
        public int SupplierId { get; set; }
        public int CalibFreqId { get; set; }
        public DateTime? CalibDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Range { get; set; }
        public string SerialNo { get; set; }
        public string LabId { get; set; }
        public string CertNo { get; set; }
        public string? Traceability { get; set; }
        public int CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool IsActive { get; set; }

        public virtual Supplier SupplierModel{get;set;}
        public virtual List<MasterQuarantine> QuarantineModel{get;set;}
        public virtual List<MasterFileUpload> FileUploadModel{get;set;}
        public virtual List<ExternalRequest> ExternalRequestModel{get;set;}
        public virtual Lovs Lovs {get;set;}

		public string? EquipNameJP { get; set; }
	}
}