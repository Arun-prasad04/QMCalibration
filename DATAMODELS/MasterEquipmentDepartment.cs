namespace CMT.DATAMODELS
{
    public class MasterEquipmentDepartment
    {
        public int Id { get; set; }
        public string? SubSectionName { get; set; }
        public string? SubSectionCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool? IsActive { get; set; }
    }
}
