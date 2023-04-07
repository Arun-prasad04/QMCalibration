namespace CMT.DATAMODELS
{
    public partial class FeedbackData
    {
        public int Id	{get;set;}
        public int FeedbackInviteId {get; set;}
        public int? DesignationId {get; set;}
        public int? DepartmentId {get; set;}
        public Int16? QualityOfService {get;set;}
        public Int16? ReliabilityOfTest {get;set;}
        public Int16? PersonnelCompetency {get;set;}
        public Int16? TechnicalCapability {get;set;}
        public Int16? TestCalibration {get;set;}
        public Int16? OnTimeDeliveryTest {get;set;}
        public Int16? ResponseToCustomer {get;set;}
        public Int16? ComplaintResolution {get;set;}
        public Int16? EmergencySupport {get;set;}
        public Int16? HandlingOfTest {get;set;}
        public Int16? CommunicationAccess {get;set;}
        public Int16? DocReportingSystem {get;set;}
        public Int16? FacilitiesAndEnvironment {get;set;}
        public Int16? FiveSManagement {get;set;}
        public string? UserComments	{get;set;}
        public DateTime? UserSubmitedOn	{get;set;}
        public decimal? OverallScore {get;set;}
        public decimal? OverallPercentage {get;set;}
        public string? ReviewerRemarks	{get;set;}
        public int? ReviewedBy {get;set;}
        public DateTime? ReviewedOn {get;set;}
        public Int16? FeedbackStatus {get;set;}
        public Int16? SafeguardingData {get;set;}
        public Int16? Commitment {get;set;}
        public Int16? Confidentiality {get;set;}
        public Int16? ActionRequired {get;set;}
        public string? ProposedActions {get;set;}
        public DateTime? TargetDate  {get;set;}
        public string? Responsibility {get;set;}
        public string? UpdateStatus {get;set;}
        public DateTime? ClosedDate  {get;set;}
        public string? ReviewFileName {get;set;}
        public virtual User UserModel{get;set;}
        public virtual FeedbackInvite FeedbackInviteModel {get;set;}
    }
}