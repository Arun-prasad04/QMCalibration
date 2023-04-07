namespace CMT.DATAMODELS
{
    public partial class MasterEquipmentHistory
    {
    public int Id{get;set;}
    public int MasterId{get;set;}    
    public DateTime Date{get;set;}
    public string Category{get; set;}
    public string Description{get; set;}
    public string Actions_Taken{get; set;}
    public DateTime FromDate{get;set;}
    public DateTime ToDate{get;set;}
    public DateTime Date_of_Completion{get;set;}      
    public string Breakdown_hrs_days{get;set;}
    public string Status{get;set;}
    public string Maintainence{get;set;}
    public string CreatedBy{get;set;}
    public DateTime CreatedOn{get;set;}
    public int StatusId{get;set;}    
    }
}