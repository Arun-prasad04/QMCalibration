namespace WEB.Models
{	public class ToolRoomMasterViewModel
	{
			public int Id { get; set; }
			public string? SubSectionName { get; set; }
			public string? SubSectionCode { get; set; }
			public int? CreatedBy { get; set; }
			public DateTime? CreatedOn { get; set; }
			public bool? IsActive { get; set; }
			public string? Name { get; set; }
			public string? NameJP { get; set; }
			public int? DepartmentId { get; set; }

	}
}
