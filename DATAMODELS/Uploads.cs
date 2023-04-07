namespace CMT.DATAMODELS
{
    public partial class Uploads
    {
public int Id {get;set;}
public string? FileName	 {get;set;}
public Guid? FileGuid	 {get;set;}
public DateTime? CreatedOn {get;set;}
public DateTime? ModifiedOn {get;set;}
public string FilePath{get;set;}
public int RequestId	 {get;set;}
public string? TemplateType	 {get;set;}
public virtual List<MasterFileUpload> MasterUpload{get;set;}
    }
}