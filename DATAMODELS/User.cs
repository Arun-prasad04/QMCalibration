namespace CMT.DATAMODELS
{
    public partial class User
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? ShortId { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public bool ActiveStatus { get; set; }
        public Guid ActivationGuid { get; set; }
        public int CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int UserRoleId { get; set; }
        public string? Password { get; set; }
        public string? L3ShortId	{get;set;}
    public string? L3Name	{get;set;}
    public string? L3Email	{get;set;}
    public string? L4ShortId	{get;set;}
    public string? L4Name	{get;set;}
    public string? L4Email	{get;set;}
    public string? L5ShortId	{get;set;}
    public string? L5Name	{get;set;}
    public string? L5Email	{get;set;}
    public string? L6ShortId	{get;set;}
    public string? L6Name	{get;set;}
    public string? L6Email	{get;set;}
    public string? Level	{get;set;}
        public int DepartmentId { get; set; }
        public int Designation { get; set; }
         public string? SignImageName	{get;set;}
        public virtual List<ExternalRequestStatus> ExtStatus { get; set; }
        public virtual List<RequestStatus> ReqStatus { get; set; }
        public virtual Department Department { get; set; }
        public virtual List<FeedbackInvite> FeedbackUsers { get; set; }
        public virtual List<FeedbackInvite> FeedbackInviters { get; set; }
        public virtual List<FeedbackData> FeedbackDataReviewer {get; set;}
        public virtual List<Instrument> Instrument { get; set; }
        public virtual List<TemplateObservation> TemplateObservatinoCreateData {get; set;}
        public virtual List<TemplateObservation> TemplateObservationsREviewData {get; set;}

    }
}