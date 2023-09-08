

using System.Runtime.Serialization;

namespace WEB.Models
{

    public class UserViewModel
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
        public string? AsstForemanShortId { get; set; }
        public string? AsstForemanName { get; set; }
        public string? AsstForemanEmail { get; set; }
        public string? ForemanShortId { get; set; }
        public string? ForemanName { get; set; }
        public string? ForemanEmail { get; set; }
        public string? KakarichoShortId { get; set; }
        public string? KakarichoName { get; set; }
        public string? KakarichoEmail { get; set; }
        public string? ManagerShortId { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerEmail { get; set; }
        public int DepartmentId { get; set; }
        public int Designation { get; set; }
        public string? SignImageName { get; set; }
        public List<DepartmentViewModel> DepartmentList { get; set; }
        public List<LovsViewModel> DesignationList { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public string? Level { get; set; }
        public IFormFile ImageUpload { get; set; }
        public List<UserViewModel> Labadminuserlist { get; set; }
        public int DeletingID { get; set; }

		public string? subSection { get; set; }

        public string? SubSectionCodes { get; set; }
        public List<int> SubSectionCodeval { get; set; }
        //public List<string> SubSectionCodeName { get; set; }
        public List<UserDepartmentMappingView> SubSectionCodeName1 { get; set; }
        public List<UserDepartmentMappingView> SubSectionCodeList { get; set; }

        public string? DeptCordShortId { get; set; }
        public string? DeptCordName { get; set; }
        public string? DeptCordEmail { get; set; }

        public List<UserRolesView> RoleList { get; set; }

        public List<int> RoleSelect { get; set; }
        public List<UserRoleMappingView> RoleSelectVal { get; set; }

    }
    public class InviteUsers
    {
        public string? Email { get; set; }
    }

    public class UserDepartmentMappingView
    {        
        public int? Id { get; set; }        
        public int? UserId { get; set; }        
        public int? DepartmentId { get; set; }        
        public DateTime CreatedDate { get; set; }
        public int? CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserRoleMappingView
    {
        [DataMember]
        public int? Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int RoleId { get; set; }
        
        [DataMember]
        public bool IsActive { get; set; }
        public int? CreatedOn { get; set; }

        public DateTime CreatedDate { get; set; }
    }

    public class UserRolesView
    {
        [DataMember]
        public int Id { get; set; }
              
        [DataMember]
        public string RoleName { get; set; }
        [DataMember]
        public bool IsActive { get; set; }

        public int CreatedOn { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}