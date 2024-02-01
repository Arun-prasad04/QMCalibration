using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WEB.Models;
using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services;
using WEB.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace WEB.Controllers;

public class HomeController : BaseController
{
	private readonly IMapper _mapper;

	private CMTDatabaseContext db;
	private IUnitOfWork _unitOfWork { get; set; }
	private IMasterService _masterService { get; set; }

	private IConfiguration _configuration;
	private IUserService _userService { get; set; }
	//private CMTDL _cmtdl { get; set; }
	public HomeController(IUnitOfWork unitOfWork, IMapper mapper, IMasterService masterService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IConfiguration Configuration, IUserService userService) : base(logger, contextAccessor)
	{
		_masterService = masterService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_configuration = Configuration;
		_userService = userService;
		//_cmtdl = cmtdl;
	}

	public IActionResult Index()
	{
		
		ViewBag.PageTitle = "DashBoard";
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		string SessionLang = base.SessionGetString("Language");
		ViewBag.RoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		//var objtype = 1159;
		//Lovs objlovs = _unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Id == objtype).SingleOrDefault();

		//Convert.ToInt32(instrumentresponse.ResponseData.ObservationType) 1159


		//UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
		//UserViewModel labUserById = GetUserById(userId);
		CMTDL _cmtdl = new CMTDL(_configuration);
		DataSet ds = _cmtdl.GetUserDashboardInfo(userId);
		if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
		{
			ViewBag.MasterCount = Convert.ToString(ds.Tables[0].Rows[0]["MasterCount"]);
		}
		else
		{
			ViewBag.MasterCount = 0;
		}
		if (userRoleId == 2 || userRoleId == 4)// || userRoleId == 5)
		{
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
			{
				ViewBag.InstrumentCount = Convert.ToString(ds.Tables[1].Rows[0]["InstrumentCount"]);
			}
			else
			{
				ViewBag.InstrumentCount = 0;
			}
		}
		else
		{
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[2].Rows.Count > 0)
			{
				ViewBag.InstrumentCount = Convert.ToString(ds.Tables[2].Rows[0]["InstrumentCount2"]);
			}
			else
			{
				ViewBag.InstrumentCount = 0;
			}
		}
        //if (userRoleId == 2 || userRoleId == 4 || userRoleId == 3)
        if (userRoleId == 2 || userRoleId == 3)
        {
            #region Commend
            //if (userRoleId == 3)
            //{
            //	List<ExternalRequest> RequestList = _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking().Include(I => I.MasterModel).Include(I => I.ExternalRequestStatusModal).Include(I => I.ExternalRequestStatusModal).ToList();
            //	if (RequestList != null)
            //	{
            //		ViewBag.RequestCount = RequestList.Count;
            //	}
            //	else
            //	{
            //		ViewBag.RequestCount = 0;
            //	}
            //}
            //else
            //{
            //	List<Request> RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking().Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).ToList();
            //	if (RequestList != null)
            //	{
            //		ViewBag.RequestCount = Convert.ToString(ds.Tables[3].Rows[0]["RequestCount"]);
            //	}
            //	else
            //	{
            //		ViewBag.RequestCount = 0;
            //	}
            //}
            #endregion
            //List<Request> RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking().Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).ToList();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[3].Rows.Count > 0)
            {
                ViewBag.RequestCount = Convert.ToString(ds.Tables[3].Rows[0]["RequestCount"]);
            }
            else
            {
                ViewBag.RequestCount = 0;
            }
        }
		else
		{
			if (ds != null && ds.Tables.Count > 0 && ds.Tables[4].Rows.Count > 0)
			{
				ViewBag.RequestCount = Convert.ToString(ds.Tables[4].Rows[0]["RequestCount2"]);
			}
			else
			{
				ViewBag.RequestCount = 0;
			}
		}
		if (userRoleId == 4)
		{

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[5].Rows.Count > 0)
            {
                ViewBag.RequestCount = Convert.ToString(ds.Tables[5].Rows[0]["RequestCount3"]);
            }
            else
            {
                ViewBag.RequestCount = 0;
            }

        }
		return View();
	}

	public IActionResult Privacy()
	{
		return View();
	}

	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	}

	public IActionResult HomePage()
	{

		var Path = _configuration["HomePage"];

		return Redirect(Path);
	}
	public IActionResult MasterTranslate()
	{
		List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2).ToList();
		//MasterLangTranslate MStranslater = new MasterLangTranslate();
		var MStranslater = new List<MasterLangTranslate>();
		foreach (var item in MasterList)
		{

			MStranslater.Add(new MasterLangTranslate
			{
				id = item.Id,
				NameEng = item.EquipName,
				NameJp = item.EquipNameJP,
				EquipmentMasterId = item.EquipmentMasterId,

			});
		}
		return Json(MStranslater);

	}

	public IActionResult ObservationTypeTranslation()
	{		
		List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.IsActive == true).ToList());
		var ObservationType = new List<ObservationTypeModel>();
		foreach (var item in lovsList)
		{

			ObservationType.Add(new ObservationTypeModel
			{
				Id = item.Id,
				AttrName = item.AttrName,
				AttrValue = item.AttrValue,
				Attrform = item.Attrform,
				AttrNameJp = item.AttrNameJp,
				AttrValueJp = item.AttrValueJp,
				AttrformJp = item.AttrformJp,

				});
			}
			return Json(ObservationType);
		}
	public IActionResult DepartmentTranslate()
		{
			int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
			int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
			List<DepartmentViewModel> DepartmentList;
			CMTDL _cmtdl = new CMTDL(_configuration);
			//if (userRoleId == 2)
			//{
				DepartmentList = _cmtdl.GetUserDepartment(userId, userRoleId);
			//}
			//else
			//{
			//	DepartmentList = _cmtdl.GetUserDepartment(userId, userRoleId);
			//}
			//var DepartmentData
			//List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2).ToList();
			//MasterLangTranslate MStranslater = new MasterLangTranslate();
			var DepartTranslater = new List<DepartmentLangTranslate>();
			foreach (var item in DepartmentList)
			{

				DepartTranslater.Add(new DepartmentLangTranslate
				{
					id = item.Id,
					Name = item.Name,
					NameJp = item.NameJP,
					SubSectionCode = item.SubSectionCode


				});
			}
			return Json(DepartTranslater);

		}

	public IActionResult LoadRole()
		{
			int LoggedId = Convert.ToInt32(base.SessionGetString("LoggedId"));

			//List<UserRoles> MasterList = _unitOfWork.Repository<UserRoles>().GetQueryAsNoTracking().Include(I => I.UserRoleMapping.);

			CMTDL _cmtdl = new CMTDL(_configuration);
			List<UserRolesView> UserRoles = _cmtdl.GetUserRoles(LoggedId);
			return Json(UserRoles);
		}
	public IActionResult MasterDepartmentTranslate()
	{
		//int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));
		//int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		List<DepartmentViewModel> DepartmentList;		
		CMTDL _cmtdl = new CMTDL(_configuration);		
		DepartmentList = _cmtdl.GetMasterDepartmentSubSection();	
		var DepartTranslaterMaster = new List<DepartmentLangTranslate>();
		foreach (var item in DepartmentList)
		{
			DepartTranslaterMaster.Add(new DepartmentLangTranslate
			{
				id = item.Id,
				Name = item.Name,
				NameJp = item.NameJP,
				SubSectionCode = item.SubSectionCode


			});
		}
		return Json(DepartTranslaterMaster);

	}
	public class DepartmentLangTranslate
	{
		public int id { get; set; }
		public string? Name { get; set; }

		public string? NameJp { get; set; }
		public string? SubSectionCode { get; set; }

	}

	public class MasterLangTranslate
	{
		public int id { get; set; }
		public string? NameEng { get; set; }
		public string? NameJp { get; set; }
		public string? EquipmentMasterId { get; set; }
	
	}

	public class ObservationTypeModel
	{
		public int Id { get; set; }
		public string AttrName { get; set; }
		public string AttrValue { get; set; }
		public string Attrform { get; set; }
		public string AttrNameJp { get; set; }

		public string AttrformJp { get; set; }
		public string AttrValueJp { get; set; }
	}	
}
