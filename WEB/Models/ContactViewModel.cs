namespace WEB.Models{

public class ContactViewModel
{
    public int Id	{get;set;}
    public string? Name	{get;set;}
    public int  DepartmentId	{get;set;}
    public string? Email	{get;set;}
    public string MobileNo	{get;set;}
    public int CreatedBy	{get;set;}
    public int ModifiedBy	{get;set;}
    public DateTime? CreatedOn	{get;set;}
    public DateTime? ModifiedOn	{get;set;}
    public List<DepartmentViewModel> DepartmentList{get;set;}
}
}