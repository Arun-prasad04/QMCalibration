using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace WEB.Services;


public class UserService : IUserService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	private IConfiguration _configuration;
	private IEmailService _emailService;
	IUtilityService _utilityService;
	//private CMTDL _cmtdl { get; set; }
	public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IUtilityService utilityService, IConfiguration Configuration)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_emailService = emailService;
		_utilityService = utilityService;
		_configuration = Configuration;
		//_cmtdl = cmtdl;
	}
	public ResponseViewModel<UserViewModel> GetAllUserList()
	{
		try
		{
            List<UserViewModel> userViewModelList = new List<UserViewModel>();

			CMTDL _cmtdl = new CMTDL(_configuration);
			DataSet ds = _cmtdl.GetUserMasterList("AllUser");
            //List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    UserViewModel userlist = new UserViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        DepartmentName = dr["DeptName"].ToString(),
                        FirstName = dr["FirstName"].ToString(),
                        LastName = dr["LastName"].ToString(),
                        subSection = dr["SubSectionCode"].ToString(),
                        Email = dr["Email"].ToString(),
                        MobileNo = dr["MobileNo"].ToString(),
                        ShortId = dr["ShortId"].ToString(),
                        CreatedOn = Convert.ToDateTime(dr["CreatedOn"]),
                        DeptCordEmail = dr["DeptCordEmail"].ToString(),
                        //CalibSource = dr["CalibSource"].ToString(),
                        //StandardReffered = dr["StandardReffered"].ToString(),
                        //Remarks = dr["Remarks"].ToString(),
                        //Status = Convert.ToInt16(dr["Status"]),                       
                        //DepartmentName = dr["deptName"].ToString(),
                        ////NewReqAcceptStatus = Convert.ToInt32(dr["NewReqAcceptStatus"]),
                        //RequestStatus = Convert.ToInt32(dr["RequestStatus"]),
                        //UserRoleId = userRoleId,
                    };
                    userViewModelList.Add(userlist);

                }
            }

            #region Command
            /*
            List<UserViewModel> userViewModelList = _unitOfWork.Repository<User>().GetQueryAsNoTracking(s => s.ActiveStatus == true).Include(I => I.Department).Include(u => u.SubSectionCodeList).Select(S => new UserViewModel()
			{
				Id = S.Id,
				DepartmentName = S.Department.Name,
				FirstName = S.FirstName,
				LastName = S.LastName,
				ShortId = S.ShortId,
				Email = S.Email,
				MobileNo = S.MobileNo,
				ActiveStatus = S.ActiveStatus,
				ActivationGuid = S.ActivationGuid,
				CreatedBy = S.CreatedBy,
				ModifiedBy = S.ModifiedBy,
				CreatedOn = S.CreatedOn,
				ModifiedOn = S.ModifiedOn,
				UserRoleId = S.UserRoleId,
				Password = S.Password,
				AsstForemanEmail = S.AsstForemanEmail,
				ForemanEmail = S.ForemanEmail,
				KakarichoEmail = S.KakarichoEmail,
				ManagerEmail = S.ManagerEmail,
				DepartmentId = S.DepartmentId,
				subSection = S.Department.SubSection,
                SubSectionCodeName1 = S.SubSectionCodeList.Where(d => d.SubSectionCode == S.Department.Id).Select(S => new UserDepartmentMapping() { SubSectionCode = S.SubSectionCode }).ToList(),
				DeptCordEmail = S.DeptCordEmail
			}).ToList();
			*/
            #endregion

            return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = userViewModelList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("UserService - GetAllUserList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "GetAllUserList"
			};
		}
	}
    
    public ResponseViewModel<UserViewModel> GetUserById(int UserId)
	{
		try
		{
			CMTDL _cmtdl = new CMTDL(_configuration);
			UserViewModel uv = new UserViewModel();
            DataSet ds = _cmtdl.GetUserMasterDetailById(UserId);

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{ 
				uv.ShortId = Convert.ToString(ds.Tables[0].Rows[0]["ShortId"]);
				uv.FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]);
				uv.LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]);
				uv.Email = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);
				uv.Designation = Convert.ToInt16(ds.Tables[0].Rows[0]["Designation"]);
				uv.MobileNo = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);
				uv.DeptCordEmail = Convert.ToString(ds.Tables[0].Rows[0]["DeptCordEmail"]);
				uv.DeptCordName = Convert.ToString(ds.Tables[0].Rows[0]["DeptCordName"]);
				uv.DeptCordShortId = Convert.ToString(ds.Tables[0].Rows[0]["DeptCordShortId"]);
				uv.UserRoleId = Convert.ToInt16(ds.Tables[0].Rows[0]["UserRoleId"]);
				uv.SignImageName = Convert.ToString(ds.Tables[0].Rows[0]["SignImageName"]);
				uv.Id = Convert.ToInt16(ds.Tables[0].Rows[0]["Id"]);

			}

			//List<LovsViewModel> Lv = new List<LovsViewModel>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                var Lovslist = new List<LovsViewModel>();
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    Lovslist.Add(new LovsViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        AttrName = Convert.ToString(dr["AttrName"]),
                        Attrform = Convert.ToString(dr["Attrform"]),
                        AttrValue = Convert.ToString(dr["AttrValue"]),
                        AttrNameJp = Convert.ToString(dr["AttrNameJp"]),
                        AttrformJp = Convert.ToString(dr["AttrformJp"]),
                        AttrValueJp = Convert.ToString(dr["AttrValueJp"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"])
                    });
                }

                uv.DesignationList = Lovslist;
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
            {
                var deptlist = new List<DepartmentViewModel>();
                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    deptlist.Add(new DepartmentViewModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        Section = Convert.ToString(dr["Section"]),
                        SubSection = Convert.ToString(dr["SubSection"]),
                        SectionCode = Convert.ToString(dr["SectionCode"]),
                        SubSectionCode = Convert.ToString(dr["SubSectionCode"]),
                        Description = Convert.ToString(dr["Description"]),
                        NameJP = Convert.ToString(dr["NameJP"]),
                        DescriptionJP = Convert.ToString(dr["DescriptionJP"]),
                        SectionJP = Convert.ToString(dr["SectionJP"]),
                        SubSectionJP = Convert.ToString(dr["SubSectionJP"]),
                        ActiveStatus = Convert.ToBoolean(dr["ActiveStatus"]),
						DeptCode = Convert.ToString(dr["DeptCode"])
                    });
                }

                uv.DepartmentList = deptlist;
            }

			if (ds != null && ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
			{
				var urselectlist = new List<UserDepartmentMappingView>();
				foreach (DataRow dr in ds.Tables[3].Rows)
				{
					urselectlist.Add(new UserDepartmentMappingView
					{
						Id = Convert.ToInt32(dr["Id"]),
						UserId = Convert.ToInt32(dr["UserId"]),
						DepartmentId = Convert.ToInt32(dr["DepartmentId"]),
						IsActive = Convert.ToBoolean(dr["IsActive"])
                    });
				}

				uv.SubSectionCodeName1 = urselectlist;
			}

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[4].Rows.Count > 0)
            {
                var rolelist = new List<UserRolesView>();
                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    rolelist.Add(new UserRolesView
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        RoleName = Convert.ToString(dr["RoleName"])                        
                    });
                }

                uv.RoleList = rolelist;
            }

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[5].Rows.Count > 0)
            {
                var roleselectlist = new List<UserRoleMappingView>();
                foreach (DataRow dr in ds.Tables[5].Rows)
                {
                    roleselectlist.Add(new UserRoleMappingView
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        UserId = Convert.ToInt32(dr["UserId"]),
                        RoleId = Convert.ToInt32(dr["RoleId"]),
                        IsActive = Convert.ToBoolean(dr["IsActive"])
                    });
                }

                uv.RoleSelectVal = roleselectlist;
            }
            //UserViewModel userById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
            //List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
            //List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "Designation").ToList());
            //userById.DepartmentList = departmentList;
            //userById.DesignationList = lovsList;

            return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = uv,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("UserService - GetUserById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "GetUserByID"
			};
		}

	}    

    public ResponseViewModel<string> CheckEmailAddress(string userEmail)
	{
		try
		{
			var EmailMessage = "";
			User Email = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Email == userEmail).SingleOrDefault();
			if (Email != null)
			{
				EmailMessage = "True";
			}
			else
			{
				EmailMessage = "False";
			}
			return new ResponseViewModel<string>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = EmailMessage,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("UserService - CheckEmailAddress Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<string>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "GetUserByID"
			};
		}

	}


	public ResponseViewModel<UserViewModel>  InsertUser(UserViewModel User)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			// User.Password=_utilityService.Encrypt(User.Password);
			if (User.ImageUpload != null)
			{
				string filePath = _utilityService.UploadImage(User.ImageUpload, Constants.Signature_FolderName);
				IFormFile fileobj = User.ImageUpload;
				User.SignImageName = fileobj.FileName;
				
			}

            User Userdata = _mapper.Map<User>(User);
			Userdata.DepartmentId = 1;
			Userdata.UserRoleId = User.RoleSelect[0];
            _unitOfWork.Repository<User>().Insert(Userdata);
            _unitOfWork.SaveChanges();

			_unitOfWork.Commit();

			int userID = Userdata.Id;

            List<UserDepartmentMappingView> deptList = new List<UserDepartmentMappingView>();
            foreach (int dr in User.SubSectionCodeval)
            {
                UserDepartmentMappingView ud = new UserDepartmentMappingView
                {
                    UserId = userID,
					DepartmentId = dr,
                    CreatedDate = DateTime.Now,
                    CreatedOn = User.CreatedBy,
                    IsActive = true,

                };
                deptList.Add(ud);
            }

            if (deptList != null)
            {                
                var detailData = _mapper.Map<UserDepartmentMapping[]>(deptList
                                        .Where(x => x.UserId > 0).ToList());
				
                detailData = _mapper.Map<UserDepartmentMapping[]>(deptList.Where(x => x.Id == null).ToList());

                if (detailData.Any())
                {
                    _unitOfWork.Repository<UserDepartmentMapping>().InsertRange(detailData);
                    _unitOfWork.SaveChanges();
                }
            }


            List<UserRoleMappingView> roleList = new List<UserRoleMappingView>();
            foreach (int rl in User.RoleSelect)
            {
                UserRoleMappingView ud = new UserRoleMappingView
                {
                    UserId = userID,
                    RoleId = rl,
                    CreatedDate = DateTime.Now,
                    CreatedOn = User.CreatedBy,
                    IsActive = true,

                };
                roleList.Add(ud);
            }

            if (roleList != null)
            {                
                var detailData = _mapper.Map<UserRoleMapping[]>(roleList
                                        .Where(x => x.UserId > 0).ToList());                

                detailData = _mapper.Map<UserRoleMapping[]>(roleList.Where(x => x.Id == null).ToList());

                if (detailData.Any())
                {
                    _unitOfWork.Repository<UserRoleMapping>().InsertRange(detailData);
                    _unitOfWork.SaveChanges();
                }
            }

            List<string> emailList = new List<string>();
			emailList.Add(User.Email);
			EmailViewModel emailViewModel = new EmailViewModel()
			{
				ToList = emailList,
				Subject = "Calibration Management Tool User Activation",
				Body = "Welcome " + User.FirstName + " " + User.LastName + ",<br/> You are successfully added to CMT application as Department User. Please Click the below link and setup your account password to continue you CMT account. </br> <a href='http://s365id1qdg044/cmtlive/Authentication/AuthenticateUser?activationKey=" + User.ActivationGuid + "'>" + "CMT LIVE" + "</a></br> Note: Please contact Administarator if you are facing trouble to activate your account.",
				IsHtml = true
			};
			_emailService.SendEmailAsync(emailViewModel, true);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("UserService - InsertUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "InsertUser"
			};
		}
	}
	public ResponseViewModel<UserViewModel> UpdateUser(UserViewModel user)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == user.Id).SingleOrDefault();
			if (user.FirstName != null)
			{
				userById.FirstName = user.FirstName;
			}
			if (user.LastName != null)
			{
				userById.LastName = user.LastName;
			}
			if (user.Email != null && user.Email != "")
			{
				userById.Email = user.Email;
			}
			if (user.MobileNo != null && user.MobileNo != "")
			{
				userById.MobileNo = user.MobileNo;
			}
            if (user.ShortId != null)
            {
                userById.ShortId = user.ShortId;
            }
            if (user.Designation != null)
            {
                userById.Designation = user.Designation;
            }
			if (user.DepartmentId == 0)
			{
				userById.DepartmentId = user.SubSectionCodeval[0];
			}
			if (user.UserRoleId == null)
			{
				userById.UserRoleId = user.RoleSelect[0];
			}


			#region

			//if (user.Level != null)
			//{
			//	userById.Level = user.Level;
			//}
			//if (user.AsstForemanShortId != null)
			//{
			//	userById.AsstForemanShortId = user.AsstForemanShortId;
			//}
			//if (user.AsstForemanName != null)
			//{
			//	userById.AsstForemanName = user.AsstForemanName;
			//}
			//if (user.AsstForemanEmail != null)
			//{
			//	userById.AsstForemanEmail = user.AsstForemanEmail;
			//}
			//if (user.ForemanShortId != null)
			//{
			//	userById.ForemanShortId = user.ForemanShortId;
			//}
			//if (user.ForemanName != null)
			//{
			//	userById.ForemanName = user.ForemanName;
			//}
			//if (user.ForemanEmail != null)
			//{
			//	userById.ForemanEmail = user.ForemanEmail;
			//}
			//if (user.KakarichoShortId != null)
			//{
			//	userById.KakarichoShortId = user.KakarichoShortId;
			//}
			//if (user.KakarichoName != null)
			//{
			//	userById.KakarichoName = user.KakarichoName;
			//}
			//if (user.KakarichoEmail != null)
			//{
			//	userById.KakarichoEmail = user.KakarichoEmail;
			//}
			//if (user.ManagerShortId != null)
			//{
			//	userById.ManagerShortId = user.ManagerShortId;
			//}
			//if (user.ManagerName != null)
			//{
			//	userById.ManagerName = user.ManagerName;
			//}
			//if (user.ManagerEmail != null)
			//{
			//	userById.ManagerEmail = user.ManagerEmail;
			//}
			//if(user.SubSectionCode != null)
			//{
			//             //userById.SubSectionCode = user.SubSectionCode;
			//         }

			#endregion
			if (user.DeptCordShortId != null)
            {
                userById.DeptCordShortId = user.DeptCordShortId;
            }
            if (user.DeptCordName != null)
            {
                userById.DeptCordName = user.DeptCordName;
            }
            if (user.DeptCordEmail != null)
            {
                userById.DeptCordEmail = user.DeptCordEmail;
            }

            if (user.ImageUpload != null)
			{
				string filePath = _utilityService.UploadImage(user.ImageUpload, Constants.Signature_FolderName);
				IFormFile fileobj = user.ImageUpload;
				userById.SignImageName = fileobj.FileName;
			}


			_unitOfWork.Repository<User>().Update(userById);
			_unitOfWork.SaveChanges();

			List<UserDepartmentMappingView> deptList = new List<UserDepartmentMappingView>();
			foreach (int dr in user.SubSectionCodeval)
			{
				UserDepartmentMappingView ud = new UserDepartmentMappingView
				{
					UserId = userById.Id,
					DepartmentId = dr,
					CreatedDate = DateTime.Now,
					CreatedOn = user.ModifiedBy,
					IsActive = true,

				};
				deptList.Add(ud);
			}

			if (deptList != null)
			{
				//UserDepartmentMapping usersubsec = _unitOfWork.Repository<UserDepartmentMapping>().GetQueryAsNoTracking(Q => Q.UserId == user.Id).SingleOrDefault();
				List<UserDepartmentMapping> requestUser = _unitOfWork.Repository<UserDepartmentMapping>().GetQueryAsNoTracking(Q => Q.UserId == user.Id).ToList();
				// metalrule.MetalRuleAddResultViewModelList.ForEach(x => x.ParentId = tempobsId);
				var detailData = _mapper.Map<UserDepartmentMapping[]>(deptList
										.Where(x => x.UserId > 0).ToList());
				if (detailData.Any())
				{
					foreach (var updateData in requestUser)
					{
						updateData.UserId = user.Id;
						updateData.IsActive = false;
						_unitOfWork.Repository<UserDepartmentMapping>().Update(updateData);
						_unitOfWork.SaveChanges();
					}
				}

				detailData = _mapper.Map<UserDepartmentMapping[]>(deptList.Where(x => x.UserId == user.Id && x.IsActive == true).ToList());

				if (detailData.Any())
				{
					_unitOfWork.Repository<UserDepartmentMapping>().InsertRange(detailData);
					_unitOfWork.SaveChanges();
				}
			}


            List<UserRoleMappingView> roleList = new List<UserRoleMappingView>();
            foreach (int dr in user.RoleSelect)
            {
                UserRoleMappingView ud = new UserRoleMappingView
                {
                    UserId = userById.Id,
                    RoleId = dr,
                    CreatedDate = DateTime.Now,
                    CreatedOn = user.ModifiedBy,
                    IsActive = true,

                };
                roleList.Add(ud);
            }


            if (roleList != null)
            {                
                List<UserRoleMapping> requestUser = _unitOfWork.Repository<UserRoleMapping>().GetQueryAsNoTracking(Q => Q.UserId == user.Id).ToList();                
                var detailData = _mapper.Map<UserRoleMapping[]>(roleList
                                        .Where(x => x.UserId > 0).ToList());
                if (detailData.Any())
                {
                    foreach (var updateData in requestUser)
                    {
                        updateData.UserId = user.Id;
                        updateData.IsActive = false;
                        _unitOfWork.Repository<UserRoleMapping>().Update(updateData);
                        _unitOfWork.SaveChanges();
                    }
                }

                detailData = _mapper.Map<UserRoleMapping[]>(roleList.Where(x => x.UserId == user.Id && x.IsActive == true).ToList());

                if (detailData.Any())
                {
                    _unitOfWork.Repository<UserRoleMapping>().InsertRange(detailData);
                    _unitOfWork.SaveChanges();
                }
            }

            _unitOfWork.Commit();

			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("UserService - UpdateUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "UpdateUser"
			};
		}
	}
	public ResponseViewModel<UserViewModel> AssignUser(UserViewModel user)
	{
		try
		{
			List<int> RequestIdValue = new List<int>();
			_unitOfWork.BeginTransaction();
			//List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == user.DeletingID).ToList();
			List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking().ToList();
			foreach (var item in requestUser)
			{
				if (item.StatusId == 30 || item.StatusId == 28)
				{
					RequestIdValue.Add(item.RequestId);
					//requestUser.RemoveRange(requestUser.Where(x => x.Id == item.Id));
					//requestUser.RemoveAll(requestUser.Where(x => x.Id == item.Id));
					//requestUser.Remove(requestUser.Where(x => x.Id==item.Id));
				}
			}
			// List<RequestStatus> RValue = _unitOfWork.Repository<RequestStatus>()
			//                                         .GetQueryAsNoTracking(j => j.CreatedBy == user.DeletingID && !RequestIdValue
			//                                                                     .Contains(j.RequestId)).ToList();

			List<RequestStatus> RValue = requestUser.Where(j => j.CreatedBy == user.DeletingID && !RequestIdValue
																					 .Contains(j.RequestId)).ToList();

			for (int i = 0; i < RValue.Count; i++)
			{

				if (RValue[i].StatusId == 27 || RValue[i].StatusId == 29)
				{
					RValue[i].CreatedBy = user.Id;
					_unitOfWork.Repository<RequestStatus>().Update(RValue[i]);
					_unitOfWork.SaveChanges();

				}

			}
			_unitOfWork.Commit();
			var data = 1;

			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("UserService - AssignUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "UpdateUser"
			};
		}
	}
	public ResponseViewModel<UserViewModel> PasswordUpdate(UserViewModel user)
	{
		try
		{
			user.Password = _utilityService.Encrypt(user.Password);
			_unitOfWork.BeginTransaction();
			User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ActivationGuid == user.ActivationGuid).SingleOrDefault();
			if (user.Password != null)
			{
				userById.Password = user.Password;
			}

			_unitOfWork.Repository<User>().Update(userById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("UserService - PasswordUpdate Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "UpdateUser"
			};
		}
	}
	public ResponseViewModel<string> DeleteUser(int userId)
	{
		try
		{
			var result = "";
			int a = 0;
			User userById1 = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
			if (userById1.UserRoleId == 2)
			{
				List<int> RequestIdValue = new List<int>();
				List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking().ToList();
				foreach (var item in requestUser)
				{
					if (item.StatusId == 30 || item.StatusId == 28)
					{
						RequestIdValue.Add(item.RequestId);
					}
				}
				List<RequestStatus> RValue = requestUser.Where(j => j.CreatedBy == userId && !RequestIdValue
																						 .Contains(j.RequestId)).ToList();
				if (RValue.Count > 0)
				{
					result = "0";
				}
				else
				{
					userById1.ActiveStatus = false;
					_unitOfWork.BeginTransaction();
					_unitOfWork.Repository<User>().Update(userById1);
					_unitOfWork.SaveChanges();
					_unitOfWork.Commit();
					result = "1";
					a = a + 200;
				}
			}
			else if (userById1.UserRoleId == 3)
			{
				List<int> ExternalRequestIdValue = new List<int>();
				List<ExternalRequestStatus> ExternalRequestUser = _unitOfWork.Repository<ExternalRequestStatus>().GetQueryAsNoTracking().ToList();
				foreach (var item in ExternalRequestUser)
				{
					if (item.StatusId == 30 || item.StatusId == 28)
					{
						ExternalRequestIdValue.Add(item.ExternalRequestId);
					}
				}
				List<ExternalRequestStatus> ERValue = ExternalRequestUser.Where(j => j.CreatedBy == userId && !ExternalRequestIdValue
																						 .Contains(j.ExternalRequestId)).ToList();
				if (ERValue.Count > 0)
				{
					result = "0";
				}
				else
				{
					userById1.ActiveStatus = false;
					_unitOfWork.BeginTransaction();
					_unitOfWork.Repository<User>().Update(userById1);
					_unitOfWork.SaveChanges();
					_unitOfWork.Commit();
					result = "1";
					a = a + 200;
				}

			}

			else
			{
				userById1.ActiveStatus = false;
				_unitOfWork.BeginTransaction();
				_unitOfWork.Repository<User>().Update(userById1);
				_unitOfWork.SaveChanges();
				_unitOfWork.Commit();
				result = "1";
				a += 200;
			}
			return new ResponseViewModel<string>
			{
				ResponseCode = a,
				ResponseMessage = "Success",
				ResponseData = result,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("UserService - DeleteUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<string>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "DeleteUser"
			};
		}
	}
	// public ResponseViewModel<UserViewModel> ValidateUser(string UserName, string Password)
	public ResponseViewModel<UserViewModel> ValidateUser(string email)
	{
		try
		{
			
			UserViewModel validateUser = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q =>
			//(Q.ShortId == UserName || Q.Email.Trim() == UserName.Trim())).Include(I => I.Department).Select(S => new UserViewModel()
			(Q.Email.Trim() == email && Q.ActiveStatus == true)).Include(I => I.Department).Select(S => new UserViewModel()
			{
				DepartmentName = S.Department.Name,
				FirstName = S.FirstName,
				LastName = S.LastName,
				ShortId = S.ShortId,
				Email = S.Email,
				MobileNo = S.MobileNo,
				ActiveStatus = S.ActiveStatus,
				ActivationGuid = S.ActivationGuid,
				CreatedBy = S.CreatedBy,
				ModifiedBy = S.ModifiedBy,
				CreatedOn = S.CreatedOn,
				ModifiedOn = S.ModifiedOn,
				UserRoleId = S.UserRoleId,
				Password = S.Password,
				AsstForemanEmail = S.AsstForemanEmail,
				ForemanEmail = S.ForemanEmail,
				KakarichoEmail = S.KakarichoEmail,
				ManagerEmail = S.ManagerEmail,
				DepartmentId = S.DepartmentId,
				Id = S.Id
			}).SingleOrDefault();
			if (validateUser == null)
			{
				
				return new ResponseViewModel<UserViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = "Invalid UserName",
					ResponseData = null,
					ResponseDataList = null
				};

			}
			else
			{
				return new ResponseViewModel<UserViewModel>
				{
					ResponseCode = 200,
					ResponseMessage = "Success",
					ResponseData = validateUser,
					ResponseDataList = null
				};
			}

			//string decryptUserPass = _utilityService.Decrypt(validateUser.Password);
			//if (Password == decryptUserPass)
			//{
			//    return new ResponseViewModel<UserViewModel>
			//    {
			//        ResponseCode = 200,
			//        ResponseMessage = "Success",
			//        ResponseData = validateUser,
			//        ResponseDataList = null
			//    };
			//}
			//else
			//{
			//    return new ResponseViewModel<UserViewModel>
			//    {
			//        ResponseCode = 500,
			//        ResponseMessage = "Invalid Password",
			//        ResponseData = null,
			//        ResponseDataList = null
			//    };
			//}
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("UserService - ValidateUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "ValidateUser"
			};
		}
	}
	public ResponseViewModel<UserViewModel> CreateNewUser()
	{
		try
		{
			List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking(d => d.SubSectionCode != null).ToList());
			List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "Designation" && Q.IsActive == true).ToList());
			List<UserRolesView> rolelist = _mapper.Map<List<UserRolesView>>(_unitOfWork.Repository<UserRoles>().GetQueryAsNoTracking(d => d.IsActive == true).ToList());
            UserViewModel userEmptyViewModel = new UserViewModel();
			userEmptyViewModel.DepartmentList = departmentList;
			userEmptyViewModel.DesignationList = lovsList;
			userEmptyViewModel.RoleList = rolelist;

			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = userEmptyViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("UserService - CreateNewUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "CreateNewUser"
			};
		}
	}
	public ResponseViewModel<UserViewModel> ActivateUser(ActivationUserViewModel userActivation)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ActivationGuid == userActivation.AuthKey).SingleOrDefault();
			userById.Password = _utilityService.Encrypt(userActivation.Password);
			_unitOfWork.Repository<User>().Update(userById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			List<string> emailList = new List<string>();
			emailList.Add(userById.Email);
			EmailViewModel emailViewModel = new EmailViewModel()
			{
				ToList = emailList,
				Subject = "Calibration Management Tool User Activation Successful",
				Body = "Hi " + userById.FirstName + " " + userById.LastName + ",<br/> Your CMT application activated successfully!. Please Click the below link and started doing your activities with CMT. </br> <a href='" + _configuration["AppUrl"] + "http://s365id1qdg044/cmtlive/Account/Login'" + "'>" + "CMT LIVE" + "</a></br> </br> Note: Please contact Administarator if you are facing trouble to login your account.",
				IsHtml = true
			};
			_emailService.SendEmailAsync(emailViewModel, true);

			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			_unitOfWork.RollBack();
			ErrorViewModelTest.Log("UserService - ActivateUser Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "UpdateUser"
			};
		}
	}
	public ResponseViewModel<UserViewModel> ForgotUserPassword(string userEmail)
	{
		try
		{
			User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Email == userEmail).SingleOrDefault();
			List<string> emailList = new List<string>();
			//emailList.Add("srinidhi.s@intelizign.com");
			emailList.Add(userById.Email);

			// System.Random random = new System.Random();
			// var OTP = random.Next(100000, 999999);

			EmailViewModel emailViewModel = new EmailViewModel()
			{
				ToList = emailList,
				Subject = "Calibration Management Reset Password",
				//Body = "Hi " + "Srinidhi" + " " + "S" + ",<br/> Please Click the below link and reset your account with CMT. </br> " + _configuration["AppUrl"] + "Activation/ActivateUser?userid=" + "Email" + "</br> Note: Please contact Administarator if you are facing trouble to resetting your password.",
				Body = "Hi " + userById.FirstName + " " + userById.LastName + ",<br/> Please Click the below link and reset your account with CMT. </br> " + _configuration["AppUrl"] + "/Account/UpdatePassword?userid=" + userById.ActivationGuid + "</br> Note: Please contact Administarator if you are facing trouble to resetting your password.",
				IsHtml = true
			};
			_emailService.SendEmailAsync(emailViewModel, true);

			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("UserService - ForgotUserPassword Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<UserViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "UpdateUser"
			};
		}
	}


}


//---------------------------user deleted commented code--------------------------------------------


// var result = "";
// int a = 0;
// User userById1 = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
// if (userById1.UserRoleId == 2)
// {
//     int StatusCode = 0;
//     //List<RequestStatus> requestUser = _mapper.Map<List<RequestStatus>>(_unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == userId).ToList());
//     //List<RequestStatus?> requestUser = _unitOfWork.Repository<RequestStatus?>().GetQueryAsNoTracking(j => j.CreatedBy == userId && (j.StatusId != 28 || j.StatusId != 30 || j.StatusId == 26)).ToList();
//     //List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == userId).Include(q => q.RequestModel.StatusId == 27).ToList();
//     List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == userId).ToList();
//     foreach (var item in requestUser)
//     {
//         if ((item.StatusId != 28 || item.StatusId != 30) && (item.StatusId == 27 || item.StatusId == 29))
//         {
//             StatusCode += 1;
//         }
//     }
//     if (StatusCode != 0 || StatusCode != null)
//     {
//         result = "0";
//     }
//     else
//     {
//         userById1.ActiveStatus = false;
//         _unitOfWork.BeginTransaction();
//         _unitOfWork.Repository<User>().Update(userById1);
//         _unitOfWork.SaveChanges();
//         _unitOfWork.Commit();
//         result = "1";
//         a = a + 200;
//     }

// }
// else
// {
//     userById1.ActiveStatus = false;
//     _unitOfWork.BeginTransaction();
//     _unitOfWork.Repository<User>().Update(userById1);
//     _unitOfWork.SaveChanges();
//     _unitOfWork.Commit();
//     result = "1";
//     a += 200;
// }

// return new ResponseViewModel<string>
// {
//     ResponseCode = a,
//     ResponseMessage = "Success",
//     ResponseData = result,
//     ResponseDataList = null
// };
