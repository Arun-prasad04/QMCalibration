namespace WEB.Models
{

	public class DepartmentViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? DeptCode { get; set; }
		public string? Section { get; set; }
		public string? SubSection { get; set; }
		public string? NameJP { get; set; }
		public string? DescriptionJP { get; set; }
		public string? SectionJP { get; set; }
		public string? SubSectionJP { get; set; }
		public int? PlantId { get; set; }
		public bool ActiveStatus { get; set; }
		public int CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }
		public List<LocationViewModel> locationList { get; set; }
		//public virtual Location LocationModel { get; set; }
		public string? PlantLocation { get; set; }
		public string? PlantCode { get; set; }

		public string? SectionCode { get; set; }
		public string? SubSectionCode { get; set; }

	}
}