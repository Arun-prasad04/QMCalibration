namespace CMT.DATAMODELS  
{
 public class Department{
public int Id {get;set;}
public string? Name {get;set;}
public string? Description {get;set;}
public bool ActiveStatus {get;set;}
public int CreatedBy {get;set;}
public int? ModifiedBy {get;set;}
public DateTime? CreatedOn {get;set;}
public DateTime? ModifiedOn {get;set;}
public virtual List<User> User {get;set;}
public virtual List<Instrument> Instrument {get;set;}
  }
}