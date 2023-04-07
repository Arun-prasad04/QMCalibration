namespace WEB.Models
{
    public class FeedbackInviteViewModel
    {
        public int Id {get;set;}
        public int UserId {get;set;}
        public int InvitedBy {get;set;}
        public DateTime InvitedOn {get;set;}
        public string? Comments {get;set;}
        public string? Email {get;set;}
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNo { get; set; }
        public string? ShortId { get; set; }
        public string? DepartmentName{get;set;}
        public string? SelectedInviteDate {get; set;}
        public string InvitedByName {get;set;}
        public List<UserViewModel> UserModelList {get;set;}
        public string LabName { get; set; }
        public int LabId { get; set; }

    }

   public class UserView{
        public string email {get; set;}
        public string firstName {get; set;}
        public string lastName {get; set;}
        public string mobileNumber {get; set;}
        public string department {get; set;}
        public string id {get; set;}
        public string LabName{get; set;}
    }
}