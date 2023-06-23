namespace WEB.Models
{

	public class DepartmentViewModel
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public bool ActiveStatus { get; set; }
		public int CreatedBy { get; set; }
		public int? ModifiedBy { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ModifiedOn { get; set; }

        public string? Section { get; set; }

		public string? Subsection { get; set; }
		public string? DeptCode { get; set;}

		public int? PlantId { get; set;}

    }
}