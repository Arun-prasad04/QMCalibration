namespace CMT.DATAMODELS
{
    public partial class Request
    {
        public int Id{get;set;}
        public string? ReqestNo{get; set;}
        public DateTime? RequestDate{get;set;}
        public int InstrumentId{get;set;}
        public int TypeOfReqest{get;set;}
        public int StatusId{get; set;}
        public int CreatedBy{get;set;}
        public DateTime? CreatedOn{get;set;}
        public virtual Instrument InstrumentModel{get;set;}
        public virtual List<RequestStatus> RequestStatusModel{get;set;}

		public int? ReceivedBy{get;set;}
        public int? UserL4{get;set;}
        public int? IsUserL4Accepted{get;set;}
        public int? LabL4{get;set;}
        public int? IsLabL4Accepted{get;set;}
    public string? InstrumentCondition {get;set;}
    public string? Feasiblity {get;set;}
    public string? CollectedBy {get;set;}
    public string? Result {get;set;}
    public string? ReasonforRejection{get;set;}
    public string? IsFeasibleService{get;set;}
    public string? IsFeasibleYes{get;set;}
    public string? ServiceResponsibility{get;set;}
    public DateTime? TentativeCompletionDate {get;set;}
    public DateTime? ReceivedDate {get;set;}
    public DateTime? InstrumentReturnedOn {get;set;}

    
    
    }
}