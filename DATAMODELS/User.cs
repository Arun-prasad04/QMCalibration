using System.Runtime.Serialization;

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
        public int? UserRoleId { get; set; }
        public string? Password { get; set; }
        public string? AsstForemanShortId  { get;set;}
    public string? AsstForemanName { get;set;}
    public string? AsstForemanEmail { get;set;}
    public string? ForemanShortId { get;set;}
    public string? ForemanName { get;set;}
    public string? ForemanEmail { get;set;}
    public string? KakarichoShortId { get;set;}
    public string? KakarichoName { get;set;}
    public string? KakarichoEmail { get;set;}
    public string? ManagerShortId { get;set;}
    public string? ManagerName { get;set;}
    public string? ManagerEmail { get;set;}
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

        public string? SubSectionCode { get; set; }

        //public List<int> SubSectionCode1 { get; set; }
        public virtual List<UserDepartmentMapping> SubSectionCodeList { get; set; }
        public string? DeptCordShortId { get; set; }
        public string? DeptCordName { get; set; }
        public string? DeptCordEmail { get; set; }

    }

    public class UserDepartmentMapping
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }


    public class UserRoleMapping
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        
        [DataMember]
        public bool IsActive { get; set; }

        public int? CreatedOn { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class UserRoles
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        public int CreatedOn { get; set; }

        public DateTime CreatedDate { get; set; }
		//public List<UserRoleMapping>? UserRoleMapping { get; set; }
	}
}