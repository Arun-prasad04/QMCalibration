namespace CMT.DATAMODELS
{
    public partial class Lovs
    {
    public int Id {get;set;}
	 public string? AttrName{get;set;}
	 public string? AttrValue{get;set;}
	 public string? Attrform{get;set;}
	 public bool IsActive{get;set;}
	 public int SortOrder{get;set;}
	 public virtual List<Master> Master {get; set;}
    }
}