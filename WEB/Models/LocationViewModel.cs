namespace WEB.Models
{
	public class LocationViewModel
	{
		public int Id { get; set; }
		public string? PlantLocation{ get; set; }
		public string? PlantCode{ get; set; }
		public bool? ActiveStatus{ get; set; }
		public int? CreatedBy {get; set;}
		public int? ModifiedBy {get; set;}
		public DateTime? CreatedOn {get; set;}
		public DateTime? ModifiedOn { get; set; }
		
	}
}
