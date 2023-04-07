namespace CMT.DATAMODELS
{
    public partial class FeedbackInvite
    {
        public int Id	{get;set;}
        public int UserId {get; set;}
        public int InvitedBy {get; set;}
        public DateTime InvitedOn	{get;set;}
        public string? Comments	{get;set;}

        public int LabId {get;set;}

        public virtual User UserModel{get;set;}
        public virtual User InviteUserModel{get;set;}
        public virtual List<FeedbackData> FeedbackUsers {get; set;}

        
    }
}