using CMT.DATAMODELS;
using System.Text.Json;

namespace WEB.Models;
public partial class RequestViewModel
{
    public int Id { get; set; }
    public string? ReqestNo { get; set; }
    public DateTime? RequestDate { get; set; }
    public int TypeOfRequest { get; set; }
    public int? LabL4 { get; set; }
    public int? UserL4 { get; set; }
    public int? IsLabL4Accepted { get; set; }
    public int? IsUserL4Accepted { get; set; }
    public bool IsAccept { get; set; }
    public string? InstrumentName { get; set; }
    public string? InstrumentIdNo { get; set; }
    public string? InstrumentSerialNumber { get; set; }
    public string? Range { get; set; }
    public string? LC { get; set; }
    public string? MUTemplateFileName { get; set; }
    public string? Make { get; set; }
    public string? Capacity { get; set; }
    public string? Unit1 { get; set; }
    public string? Unit2 { get; set; }
    public string? Unit3 { get; set; }
    public string? AmountJPY { get; set; }
    public int? Instrument_Type { get; set; }
    public string? Rule_Confirmity { get; set; }
    public string? EquipmentStation { get; set; }
    public string? Comment { get; set; }
    public int CalibFreq { get; set; }
    public string? CalibFrequency { get; set; }
    public int UserDept { get; set; }
    public string? NABL { get; set; }
    public string? ReqestBy { get; set; }
    public int AcceptOrReject { get; set; }
    public string? InstrumentRecordOn { get; set; }
    public string? VisualCheck { get; set; }
    public string? RecordBy { get; set; }
    public string? Result { get; set; }
    public DateTime? ClosedDate { get; set; }
    public string? ReturnTo { get; set; }
    public DateTime? ReturnDate { get; set; }
    public int Status { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? CalibDate { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? SubmittedOn { get; set; }
    public string? VisualCheckFm { get; set; }
    public string? ReturnToLab { get; set; }
    public string? VisualCheckLab { get; set; }
    public string? RecodedByLAB { get; set; }
    public string? RejectReason { get; set; }
    public string? AcceptReason { get; set; }
    public string? ReasonforRejection { get; set; }
    public string? IsFeasibleService { get; set; }
    public string? IsFeasibleYes { get; set; }
    public List<IFormFile> ImageUpload{get; set;}
    public string? ServiceResponsibility { get; set; }
    public string? DepartmentName { get; set; }
    public string? ResultLAB { get; set; }
    public string? ResultDEP { get; set; }
    public string? LabResult { get; set; }
    public int InstrumentId { get; set; }
    public int? ReviewedStatus { get; set; }
    public int? ReceivedBy { get; set; }
    public int? UserRoleId { get; set; }
    public string? InstrumentCondition { get; set; }
    public string? Feasiblity { get; set; }
    public DateTime? TentativeCompletionDate { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public DateTime? InstrumentReturnedOn { get; set; }
    public string? CollectedBy { get; set; }    
    public string? ReceivedByName { get; set; }
    public int? MasterInstrument1 { get; set; }
    public int? MasterInstrument2 { get; set; }
    public int? MasterInstrument3 { get; set; }
    public int? MasterInstrument4 { get; set; }
    public string? CalibSource { get; set; }
    public string? StandardReffered { get; set; }
    public DateTime? DateOfReceipt { get; set; }
    public string? MasterInstrumentName1 { get; set; }
    public string? MasterInstrumentName2 { get; set; }
    public string? MasterInstrumentName3 { get; set; }
    public string? MasterInstrumentName4 { get; set; }
    public bool? IsNABL { get; set; }
    public int? ObservationType { get; set; }
    public int? MUTemplate { get; set; }
    public int? CertificationTemplate { get; set; }
   public int? ObservationTemplate {get;set;}
   public int? TemplateReviewStatus {get;set;}
   public List<UploadFile> FileData{get;set;}
   public List<LovsViewModel> CalibFreqList{get;set;}   
    public List<LovsViewModel> ObservationTemplateList{get;set;}
    public List<LovsViewModel> MUTemplateList{get;set;}
    public List<LovsViewModel> CertificationTemplateList{get;set;}
	//To Bind and Display Master Data
	public List<MasterViewModel> MasterData { get; set; }
	public List<MasterViewModel> MasterEqiupmentList { get; set; }

	public List<LovsViewModel> LovsList { get; set; }

	//To Display Instrument Id Number
	public string? IdNo { get; set; }
	public string? SubSectionCode { get; set; }
	public string? TypeOfEquipment { get; set; }
    public string? SignImageName { get; set; }
    public string? ToolInventory { get; set; }
   

    public int? AdminReviewStatus { get; set; }

    public int? ExObsTemplateReviewStatus { get; set; }
	public DateTime? ReqDueDate { get; set; }
	public DateTime? ReqStartDate { get; set; }

    public List<IdNoModel> IdNoList { get; set; }
    public int? TotalCount { get; set; }
    public string? statusname { get; set; }
    public int? templateId { get; set; }

    public int? ExternalObsStatus { get; set; }
}
public class UploadFile
    {
        public string Name { get; set; }        
        public string Size { get; set; }        
        public string Data { get; set; }        
    }

public class RequestKPIViewModel
{
        public string Month {get;set;}
        public string TypeOfRequest {get;set;}
        public int NoOfRequestsReceived {get;set;}
        public int CompletedOnSameDay {get;set;}
        public int WithinOneDay {get;set;}
        public int WithinTwoDays {get;set;}
        public int WithinThreeDays {get;set;}
        public int morethanThreeDays {get;set;}
        public double Calculation {get;set;}
        public ChartDataViewModel ChartData {get;set;}

        public List<ChartDataViewModel>? ChartDataNew {get;set;}

        public List<ChartDataViewModel>? ChartDataRegular {get;set;}

        public List<ChartDataViewModel>? ChartDataRecalibration {get;set;}

	    public string? Inspectiondetails { get; set; }
}
public class Requestfilterclass
{
    public string sscode { get; set; }
    public string instrumentname { get; set; }
    public string instrumentid { get; set; }
    public string status { get; set; }
    public string requestno { get; set; }
    public string requestdate { get; set; }
    public string range { get; set; }
    public string typeofrequest { get; set; }
    public string typeofequipment { get; set; }
}
public static class SessionExtensions
{
    public static void Set<T>(this ISession session, string key, T value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T Get<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonSerializer.Deserialize<T>(value);
    }
}

