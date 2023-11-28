namespace CMT.DATAMODELS
{
	public class ToolRoomHistory
	{
		public int Id { get; set; }
		public int? InstrumentId { get; set; }
		public string? LabId { get; set; }
		public string? ReplacementId { get; set; }
		public int? StatusId { get; set; }
		public DateTime? CreatedOn { get; set; }
		public int? CreatedBy { get; set; }
		public bool? IsActive { get; set; }
		public string? Comment { get; set; }
	}
}
