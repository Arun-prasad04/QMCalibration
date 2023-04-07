namespace CMT.DATAMODELS
{
    public partial class Supplier
    {
public int Id	{get;set;}
public string? Name	 {get;set;}
public string? ContactName {get;set;} 
public string? PhoneNo {get;set;}
public string? EmailId {get;set;}
public string? MobileNo	 {get;set;}
public DateTime? CreatedOn{get;set;}
public DateTime? ModifiedOn{get;set;}
public virtual List<Master> Masters { get; set; }
    }
}