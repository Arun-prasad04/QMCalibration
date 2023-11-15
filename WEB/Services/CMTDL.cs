using AutoMapper;
using CMT.DAL;
using CMT.DATAMODELS;
using Microsoft.Data.SqlClient;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data;
using System.Text;
using WEB.Models;
using WEB.Services.Interface;

namespace WEB.Services
{
	public class CMTDL
	{
		private IConfiguration _configuration;
		public CMTDL(IConfiguration Configuration)
		{

			_configuration = Configuration;
		}


		public ResponseViewModel<UserViewModel> GetUserById(int UserId)
		{
			try
			{
				UserViewModel uv = new UserViewModel();
				DataSet ds = GetUserMasterDetailById(UserId);

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
				ErrorViewModelTest.Log("CMTDL - GetUserById Method");
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


        #region Command
        //public UserViewModel GetUserById(int UserId)
        //{
        //	UserViewModel uv = new UserViewModel();
        //	try
        //	{

        //		DataSet ds = GetUserDashboardInfo(UserId);

        //		if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //		{
        //			uv.ShortId = Convert.ToString(ds.Tables[0].Rows[0]["ShortId"]);
        //			uv.FirstName = Convert.ToString(ds.Tables[0].Rows[0]["FirstName"]);
        //			uv.LastName = Convert.ToString(ds.Tables[0].Rows[0]["LastName"]);
        //			uv.Email = Convert.ToString(ds.Tables[0].Rows[0]["Email"]);
        //			uv.Designation = Convert.ToInt16(ds.Tables[0].Rows[0]["Designation"]);
        //			uv.MobileNo = Convert.ToString(ds.Tables[0].Rows[0]["MobileNo"]);
        //			uv.DeptCordEmail = Convert.ToString(ds.Tables[0].Rows[0]["DeptCordEmail"]);
        //			uv.DeptCordName = Convert.ToString(ds.Tables[0].Rows[0]["DeptCordName"]);
        //			uv.DeptCordShortId = Convert.ToString(ds.Tables[0].Rows[0]["DeptCordShortId"]);
        //			uv.UserRoleId = Convert.ToInt16(ds.Tables[0].Rows[0]["UserRoleId"]);
        //			uv.SignImageName = Convert.ToString(ds.Tables[0].Rows[0]["SignImageName"]);
        //			uv.Id = Convert.ToInt16(ds.Tables[0].Rows[0]["Id"]);

        //		}

        //		//List<LovsViewModel> Lv = new List<LovsViewModel>();
        //		if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
        //		{
        //			var Lovslist = new List<LovsViewModel>();
        //			foreach (DataRow dr in ds.Tables[1].Rows)
        //			{
        //				Lovslist.Add(new LovsViewModel
        //				{
        //					Id = Convert.ToInt32(dr["Id"]),
        //					AttrName = Convert.ToString(dr["AttrName"]),
        //					Attrform = Convert.ToString(dr["Attrform"]),
        //					AttrValue = Convert.ToString(dr["AttrValue"]),
        //					AttrNameJp = Convert.ToString(dr["AttrNameJp"]),
        //					AttrformJp = Convert.ToString(dr["AttrformJp"]),
        //					AttrValueJp = Convert.ToString(dr["AttrValueJp"]),
        //					IsActive = Convert.ToBoolean(dr["IsActive"])
        //				});
        //			}

        //			uv.DesignationList = Lovslist;
        //		}

        //		if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
        //		{
        //			var deptlist = new List<DepartmentViewModel>();
        //			foreach (DataRow dr in ds.Tables[2].Rows)
        //			{
        //				deptlist.Add(new DepartmentViewModel
        //				{
        //					Id = Convert.ToInt32(dr["Id"]),
        //					Name = Convert.ToString(dr["Name"]),
        //					Section = Convert.ToString(dr["Section"]),
        //					SubSection = Convert.ToString(dr["SubSection"]),
        //					SectionCode = Convert.ToString(dr["SectionCode"]),
        //					SubSectionCode = Convert.ToString(dr["SubSectionCode"]),
        //					Description = Convert.ToString(dr["Description"]),
        //					NameJP = Convert.ToString(dr["NameJP"]),
        //					DescriptionJP = Convert.ToString(dr["DescriptionJP"]),
        //					SectionJP = Convert.ToString(dr["SectionJP"]),
        //					SubSectionJP = Convert.ToString(dr["SubSectionJP"]),
        //					ActiveStatus = Convert.ToBoolean(dr["ActiveStatus"]),
        //					DeptCode = Convert.ToString(dr["DeptCode"])
        //				});
        //			}

        //			uv.DepartmentList = deptlist;
        //		}

        //		if (ds != null && ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
        //		{
        //			var urselectlist = new List<UserDepartmentMappingView>();
        //			foreach (DataRow dr in ds.Tables[3].Rows)
        //			{
        //				urselectlist.Add(new UserDepartmentMappingView
        //				{
        //					Id = Convert.ToInt32(dr["Id"]),
        //					UserId = Convert.ToInt32(dr["UserId"]),
        //					SubSectionCode = Convert.ToInt32(dr["SubSectionCode"]),
        //					IsActive = Convert.ToBoolean(dr["IsActive"])
        //				});
        //			}

        //			uv.SubSectionCodeName1 = urselectlist;
        //		}
        //		//UserViewModel userById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
        //		//List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
        //		//List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "Designation").ToList());
        //		//userById.DepartmentList = departmentList;
        //		//userById.DesignationList = lovsList;


        //	}
        //	catch (Exception e)
        //	{
        //		ErrorViewModelTest.Log("UserService - GetUserById Method");
        //		ErrorViewModelTest.Log("exception - " + e.Message);
        //	}
        //	return uv;
        //}
        #endregion

        public List<UserViewModel> GetLadAdminUsers()
		{
			try
			{
				List<UserViewModel> userViewModelList = new List<UserViewModel>();


				DataSet ds = GetUserMasterList("AdminOnly");
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

				return userViewModelList;
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


			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetLadAdminUsers Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
				return null;
			}
		}

		public List<DepartmentViewModel> GetUserDepartment(int userId, int userRoleId)
		{
			try
			{
				List<DepartmentViewModel> departmentlList = new List<DepartmentViewModel>();
				DataSet ds = GetUserdepartmentOnly(userId, userRoleId);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						DepartmentViewModel userlist = new DepartmentViewModel
						{
							Id = Convert.ToInt32(dr["Id"]),
							Name = dr["Name"].ToString(),
							NameJP = dr["NameJP"].ToString(),
							Description = dr["Description"].ToString(),
							Section = dr["Section"].ToString(),
							SubSection = dr["SubSection"].ToString(),
							DeptCode = dr["DeptCode"].ToString(),
							PlantId = Convert.ToInt16(dr["PlantId"]),
							DescriptionJP = dr["DescriptionJP"].ToString(),
							SectionJP = dr["SectionJP"].ToString(),
							SubSectionJP = dr["SubSectionJP"].ToString(),
							SectionCode = dr["SectionCode"].ToString(),
							SubSectionCode = dr["SubSectionCode"].ToString(),
							//Status = Convert.ToInt16(dr["Status"]),                       
							//DepartmentName = dr["deptName"].ToString(),
							////NewReqAcceptStatus = Convert.ToInt32(dr["NewReqAcceptStatus"]),
							//RequestStatus = Convert.ToInt32(dr["RequestStatus"]),
							//UserRoleId = userRoleId,
						};
						departmentlList.Add(userlist);

					}
				}

				return departmentlList;

			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetUserDepartment Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
				return null;
			}
		}

		public UserViewModel GetUserMasterById(int UserId)
		{
			UserViewModel uv = new UserViewModel();
			try
			{

				DataSet ds = GetUserMasterDetailByIds(UserId);

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
				//if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
				//{
				//	var Lovslist = new List<LovsViewModel>();
				//	foreach (DataRow dr in ds.Tables[1].Rows)
				//	{
				//		Lovslist.Add(new LovsViewModel
				//		{
				//			Id = Convert.ToInt32(dr["Id"]),
				//			AttrName = Convert.ToString(dr["AttrName"]),
				//			Attrform = Convert.ToString(dr["Attrform"]),
				//			AttrValue = Convert.ToString(dr["AttrValue"]),
				//			AttrNameJp = Convert.ToString(dr["AttrNameJp"]),
				//			AttrformJp = Convert.ToString(dr["AttrformJp"]),
				//			AttrValueJp = Convert.ToString(dr["AttrValueJp"]),
				//			IsActive = Convert.ToBoolean(dr["IsActive"])
				//		});
				//	}

				//	uv.DesignationList = Lovslist;
				//}

				//if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
				//{
				//	var deptlist = new List<DepartmentViewModel>();
				//	foreach (DataRow dr in ds.Tables[2].Rows)
				//	{
				//		deptlist.Add(new DepartmentViewModel
				//		{
				//			Id = Convert.ToInt32(dr["Id"]),
				//			Name = Convert.ToString(dr["Name"]),
				//			Section = Convert.ToString(dr["Section"]),
				//			SubSection = Convert.ToString(dr["SubSection"]),
				//			SectionCode = Convert.ToString(dr["SectionCode"]),
				//			SubSectionCode = Convert.ToString(dr["SubSectionCode"]),
				//			Description = Convert.ToString(dr["Description"]),
				//			NameJP = Convert.ToString(dr["NameJP"]),
				//			DescriptionJP = Convert.ToString(dr["DescriptionJP"]),
				//			SectionJP = Convert.ToString(dr["SectionJP"]),
				//			SubSectionJP = Convert.ToString(dr["SubSectionJP"]),
				//			ActiveStatus = Convert.ToBoolean(dr["ActiveStatus"]),
				//			DeptCode = Convert.ToString(dr["DeptCode"])
				//		});
				//	}

				//	uv.DepartmentList = deptlist;
				//}

				//if (ds != null && ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
				//{
				//	var urselectlist = new List<UserDepartmentMappingView>();
				//	foreach (DataRow dr in ds.Tables[3].Rows)
				//	{
				//		urselectlist.Add(new UserDepartmentMappingView
				//		{
				//			Id = Convert.ToInt32(dr["Id"]),
				//			UserId = Convert.ToInt32(dr["UserId"]),
				//			SubSectionCode = Convert.ToInt32(dr["SubSectionCode"]),
				//			IsActive = Convert.ToBoolean(dr["IsActive"])
				//		});
				//	}

				//	uv.SubSectionCodeName1 = urselectlist;
				//}
				//UserViewModel userById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
				//List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
				//List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "Designation").ToList());
				//userById.DepartmentList = departmentList;
				//userById.DesignationList = lovsList;


			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetUserMasterById Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
			}
			return uv;
		}

		public UserViewModel GetLadTechnicalManagerUsers()
		{
			UserViewModel uv = new UserViewModel();
			try
			{

				DataSet ds = GetUserMasterList("Technicalmanager");

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
			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetLadTechnicalManagerUsers Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
			}
			return uv;
		}

		public UserViewModel GetLadInspectorUsers()
		{
			UserViewModel uv = new UserViewModel();
			try
			{

				DataSet ds = GetUserMasterList("Facilitymanager");

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
			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetLadInspectorUsers Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
			}
			return uv;
		}


		public User GetCalibrationLabUsers()
		{
			User uv = new User();
			try
			{

				DataSet ds = GetUserMasterList("CalibrationLab");

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
			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetLadInspectorUsers Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
			}
			return uv;
		}


		public List<UserRolesView> GetUserRoles(int userid)
		{
			try
			{
				List<UserRolesView> UserRoleslList = new List<UserRolesView>();


				DataSet ds = GetUserRoleslList(userid);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						UserRolesView userlist = new UserRolesView
						{
							Id = Convert.ToInt32(dr["Id"]),
							RoleName = dr["RoleName"].ToString()
						};
						UserRoleslList.Add(userlist);

					}
				}

				return UserRoleslList;
			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetLadAdminUsers Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
				return null;
			}
		}
		public List<RequestMailList> GetRequestDetailsForEMail(int RequestId)
		{
			try
			{
				List<RequestMailList> requestMailLists = new List<RequestMailList>();
				DataSet ds = GetRequestsForEMail(RequestId);
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{ 
				foreach (DataRow dr in ds.Tables[0].Rows)
					{
						RequestMailList requestMail=new RequestMailList();
						{
							requestMail.SNo = Convert.ToInt16(dr["S.No"]);
							requestMail.RequestNo = Convert.ToString(dr["Request No"]);
							requestMail.LabId = Convert.ToString(dr["Lab ID"]);
							requestMail.EquipmentType = Convert.ToString(dr["Equipment Type"]);
							requestMail.EquipmentName = Convert.ToString(dr["Equipment Name"]);
							requestMail.SubsectionCode = Convert.ToString(dr["Sub Section Code"]);
							requestMail.CalibrationType = Convert.ToString(dr["Calibration Type"]);
							requestMail.CreaterFirstName = Convert.ToString(dr["CreaterFirstName"]);
							requestMail.CreaterLastName = Convert.ToString(dr["CreaterLastName"]);
							requestMail.CreaterEmail = Convert.ToString(dr["CreaterEmail"]);
						}
						requestMailLists.Add(requestMail);
					}
				
				}

				return requestMailLists;
			}
			catch(Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetRequestDetailsForEMail Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
				return null;

			}
		
		}
		public RequestMailList GetDataForInstrumentRequest(int InstId, int reqtype)
		{
			RequestMailList uv = new RequestMailList();
			try
			{

				DataSet ds = GetDataForDetailRequest(InstId, reqtype);

				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					uv.SNo = Convert.ToInt16(ds.Tables[0].Rows[0]["S.No"]);
					uv.RequestNo = Convert.ToString(ds.Tables[0].Rows[0]["Request No"]);
					uv.LabId = Convert.ToString(ds.Tables[0].Rows[0]["Lab ID"]);
					uv.EquipmentType = Convert.ToString(ds.Tables[0].Rows[0]["Equipment Type"]);
					uv.EquipmentName = Convert.ToString(ds.Tables[0].Rows[0]["Equipment Name"]);
					uv.SubsectionCode = Convert.ToString(ds.Tables[0].Rows[0]["Sub Section Code"]);
					uv.CalibrationType = Convert.ToString(ds.Tables[0].Rows[0]["Calibration Type"]);
					uv.CreaterFirstName = Convert.ToString(ds.Tables[0].Rows[0]["CreaterFirstName"]);
					uv.CreaterLastName = Convert.ToString(ds.Tables[0].Rows[0]["CreaterLastName"]);
					uv.CreaterEmail = Convert.ToString(ds.Tables[0].Rows[0]["CreaterEmail"]);

				}
			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetDataForInstrumentRequest Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
			}
			return uv;
		}

		public DataSet GetUserRoleslList(int userid)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserRoleslList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@UserId", userid);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}
		public DataSet GetCalibrationLabUsersData()
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserMasterList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Type", "CalibrationLab");
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

		// GetUserMasterList
		public DataSet GetUserMasterList(string typ)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserMasterList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Type", typ);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

		//GetRequestList
		public DataSet GetRequestList(int userid, int userroleid)
		{
			
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetRequestList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", userid);
			cmd.Parameters.AddWithValue("@userroleid", userroleid);
			//cmd.Parameters.AddWithValue("@deptid", deptid);
			//SqlConnection sqlConn = new SqlConnection("Data Source=(localdb)\\Local;Initial Catalog=QM_CMT;user id=sa;password=sql@123;");
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}


		//GetUserMasterDetailById
		public DataSet GetUserMasterDetailById(int UserId)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserMasterDetailById");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", UserId);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}
		//GetUserDashboardInfo

		
		public DataSet GetUserDashboardInfo(int UserId)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserDashboardInfo");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", UserId);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}
		//GetInstrumentList
		public DataSet GetInstruentList(int userid, int userroleid)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetInstrumentList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", userid);
			cmd.Parameters.AddWithValue("@userroleid", userroleid);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

		// GetInstruentQuartineList
		public DataSet GetInstruentQuartineList(int userid, int userroleid)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetInstruentQuartineList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", userid);
			cmd.Parameters.AddWithValue("@userroleid", userroleid);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}
		//GetUserDepartmentList
		public DataSet GetUserdepartmentOnly(int userId, int userRoleId)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserDepartmentList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", userId);
			cmd.Parameters.AddWithValue("@userRoleid", userRoleId);
			//cmd.Parameters.AddWithValue("@deptid", deptid);
			//SqlConnection sqlConn = new SqlConnection("Data Source=(localdb)\\Local;Initial Catalog=QM_CMT;user id=sa;password=sql@123;");
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}
		//GetUserMasterDetailById
		public DataSet GetUserMasterDetailByIds(int UserId)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserMasterDetailById");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", UserId);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

		public DataSet InsertDueRequest(string reqist, int userId)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("InsertDueRequestlist");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@userid", userId);
			cmd.Parameters.AddWithValue("@reqist", reqist);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;

		}

		public DataSet GetDataForDetailRequest(int InstId, int reqtype)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetDataForDetailRequest");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@InstrumentId", InstId);
			cmd.Parameters.AddWithValue("@TypeReq", reqtype);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

		#region
		/*
        //GetUserMasterList
        public DataSet GetLadInspectorUsersData()
        {
            var connectionString = _configuration.GetConnectionString("CMTDatabase");
            SqlCommand cmd = new SqlCommand("GetUserMasterList");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", "Facilitymanager");
            SqlConnection sqlConn = new SqlConnection(connectionString);
            DataSet dsResults = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            cmd.Connection = sqlConn;
            cmd.CommandTimeout = 2000;
            sqlAdapter.SelectCommand = cmd;
            sqlAdapter.Fill(dsResults);

            return dsResults;
        }
        //GetUserMasterList
        public DataSet GetLabTechnicalManagerOnly()
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserMasterList");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@Type", "Technicalmanager");
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

        //GetUserMasterList
        public DataSet GetUserMasterList()
        {
            var connectionString = _configuration.GetConnectionString("CMTDatabase");
            SqlCommand cmd = new SqlCommand("GetUserMasterList");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Type", "AllUser");
            SqlConnection sqlConn = new SqlConnection(connectionString);
            DataSet dsResults = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            cmd.Connection = sqlConn;
            cmd.CommandTimeout = 2000;
            sqlAdapter.SelectCommand = cmd;
            sqlAdapter.Fill(dsResults);

            return dsResults;
        }
		*/
		#endregion`

		public List<DueInstrument> GetAllDueInstrumentList(int month)
		{
			try
			{
				List<DueInstrument> InstrumentlList = new List<DueInstrument>();


				DataSet ds = GetAllDueInstrumentListData(month);
				//List<InstrumentViewModel> Details = new List<InstrumentViewModel>();
				if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					foreach (DataRow dr in ds.Tables[0].Rows)
					{
						DueInstrument Instlist = new DueInstrument
						{
							IdNo = dr["IdNo"].ToString(),
							InstrumentId = Convert.ToInt32(dr["InstrumentId"]),
							EquipmentType = dr["EquipmentType"].ToString(),
							InstrumentName = dr["EquipmentName"].ToString(),
							Class = dr["Class"].ToString(),
							Location = dr["Class"].ToString(),
							SubSectionCode = dr["SubSectionCode"].ToString(),
							SectionName = dr["SectionName"].ToString(),
							TypeofScope = dr["CalibrationType"].ToString(),
							DueDate = Convert.ToDateTime(dr["DueDate"]),
							ToolRoom = dr["ToolRoom"].ToString(),
							DeptId = Convert.ToInt32(dr["DeptId"]),
                            InstrumentCreatedBy = Convert.ToInt32(dr["InstrumentCreatedBy"]),							
							RequestId = Convert.ToInt32(dr["RequestId"]),
						};
						InstrumentlList.Add(Instlist);

					}
				}

				return InstrumentlList;
			}
			catch (Exception e)
			{
				ErrorViewModelTest.Log("CMTDL - GetAllDueInstrumentList Method");
				ErrorViewModelTest.Log("exception - " + e.Message);
				return null;
			}
		}


		public DataSet GetAllDueInstrumentListData(int month)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetUserInstrumentData");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@DueMonth", month);
			//cmd.Parameters.AddWithValue("@UserRoleId", UserRole);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);

			return dsResults;
		}

        public DataSet InsertDueInstrumentList(string duelist, int userId)
        {
            var connectionString = _configuration.GetConnectionString("CMTDatabase");
            SqlCommand cmd = new SqlCommand("InsertDueInstrumentListForMail");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userid", userId);
            cmd.Parameters.AddWithValue("@Duelist", duelist);
            SqlConnection sqlConn = new SqlConnection(connectionString);
            DataSet dsResults = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            cmd.Connection = sqlConn;
            cmd.CommandTimeout = 2000;
            sqlAdapter.SelectCommand = cmd;
            sqlAdapter.Fill(dsResults);

            return dsResults;

        }

        public DataSet GetAdminApproveInstrumentList()
        {
            var connectionString = _configuration.GetConnectionString("CMTDatabase");
            SqlCommand cmd = new SqlCommand("GetAdminApproveInstrumentList");
            cmd.CommandType = CommandType.StoredProcedure;          
            SqlConnection sqlConn = new SqlConnection(connectionString);
            DataSet dsResults = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            cmd.Connection = sqlConn;
            cmd.CommandTimeout = 2000;
            sqlAdapter.SelectCommand = cmd;
            sqlAdapter.Fill(dsResults);

            return dsResults;

        }


        public DataSet InsertMgApproveDueInstrumentList(string duelist, int userId)
        {
            var connectionString = _configuration.GetConnectionString("CMTDatabase");
            SqlCommand cmd = new SqlCommand("InsertMgApproveDueInstrumentList");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userid", userId);
            cmd.Parameters.AddWithValue("@Duelist", duelist);
            SqlConnection sqlConn = new SqlConnection(connectionString);
            DataSet dsResults = new DataSet();
            SqlDataAdapter sqlAdapter = new SqlDataAdapter();
            cmd.Connection = sqlConn;
            cmd.CommandTimeout = 2000;
            sqlAdapter.SelectCommand = cmd;
            sqlAdapter.Fill(dsResults);

            return dsResults;

        }

		public DataSet GetRequestsForEMail(int RequestId)
		{
			var connectionString = _configuration.GetConnectionString("CMTDatabase");
			SqlCommand cmd = new SqlCommand("GetRequestDetailsForEMail");
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("@RequestId", RequestId);
			SqlConnection sqlConn = new SqlConnection(connectionString);
			DataSet dsResults = new DataSet();
			SqlDataAdapter sqlAdapter = new SqlDataAdapter();
			cmd.Connection = sqlConn;
			cmd.CommandTimeout = 2000;
			sqlAdapter.SelectCommand = cmd;
			sqlAdapter.Fill(dsResults);
			return dsResults;
		}

		
	}

}
