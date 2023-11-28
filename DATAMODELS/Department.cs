namespace CMT.DATAMODELS
{
 public class Department{
public int Id { get;set;}
public string? Name {get;set;}
public string? Description {get;set;}
public string? DeptCode { get; set; }
public string? Section { get; set; }
public string? SubSection { get; set; }
public string? NameJP {get;set;}
public string? DescriptionJP { get; set; }
public string? SectionJP { get; set; }
public string? SubSectionJP { get; set; }
public int? PlantId { get; set; }
public bool ActiveStatus {get;set;}
public int CreatedBy {get;set;}
public int? ModifiedBy {get;set;}
public DateTime? CreatedOn {get;set;}
public DateTime? ModifiedOn {get;set;}
public virtual List<User> User {get;set;}
public virtual List<Instrument> Instrument {get;set;}

public virtual Location Location { get; set; }
public string? SectionCode { get; set; }
public string? SubSectionCode { get; set; }
public virtual List<Master> MasterModel { get; set; }

	}
}