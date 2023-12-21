namespace CMT.DATAMODELS
{
	public class ToolRoomMaster
	{
		public int Id { get; set; }
		public string? SubSectionName { get; set; }
		public string? DeptSubSectionCode { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public bool? IsActive { get; set; }
		public virtual List<Department> DepartmentModel { get; set; }
	}
}
