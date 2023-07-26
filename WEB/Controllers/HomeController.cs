using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using WEB.Models;
using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services;
using WEB.Services.Interface;
using Microsoft.EntityFrameworkCore;


namespace WEB.Controllers;

public class HomeController : BaseController
{
	private readonly IMapper _mapper;

	private CMTDatabaseContext db;
	private IUnitOfWork _unitOfWork { get; set; }
	private IMasterService _masterService { get; set; }

	private IConfiguration _configuration;
	public HomeController(IUnitOfWork unitOfWork, IMapper mapper, IMasterService masterService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IConfiguration Configuration) : base(logger, contextAccessor)
	{
		_masterService = masterService;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_configuration = Configuration;
	}

	public IActionResult Index()
	{		
		ViewBag.PageTitle = "DashBoard";
		int userId = Convert.ToInt32(base.SessionGetString("LoggedId"));		
		int userRoleId = Convert.ToInt32(base.SessionGetString("UserRoleId"));
		UserViewModel labUserById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault());
		List<Master> MasterList = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(g => g.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2).ToList();
		if (MasterList != null)
		{
			ViewBag.MasterCount = MasterList.Count;
		}
		else
		{
			ViewBag.MasterCount = 0;
		}
		if (userRoleId == 2 || userRoleId == 4)
		{
			List<Instrument> InstrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && (Q.IdNo != "" && Q.IdNo != null) && Q.ActiveStatus == true).ToList();//.Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(I => I.RequestModel).Include(G => G.DepartmenttModel).ToList();
			if (InstrumentList != null)
			{
				ViewBag.InstrumentCount = InstrumentList.Count;
			}
			else
			{
				ViewBag.InstrumentCount = 0;
			}
		}
		else
		{
			List<Instrument> InstrumentList = _unitOfWork.Repository<Instrument>().GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2 && (Q.CreatedBy == userId || Q.UserDept == labUserById.DepartmentId)).ToList(); //.Include(I => I.QuarantineModel).Include(I => I.FileUploadModel).Include(G => G.DepartmenttModel)
			if (InstrumentList != null)
			{
				ViewBag.InstrumentCount = InstrumentList.Count;
			}
			else
			{
				ViewBag.InstrumentCount = 0;
			}
		}
		if (userRoleId == 2 || userRoleId == 4 || userRoleId == 3)
		{
			if (userRoleId == 3)
			{
				List<ExternalRequest> RequestList = _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking().Include(I => I.MasterModel).Include(I => I.ExternalRequestStatusModal).Include(I => I.ExternalRequestStatusModal).ToList();
				if (RequestList != null)
				{
					ViewBag.RequestCount = RequestList.Count;
				}
				else
				{
					ViewBag.RequestCount = 0;
				}
			}
			else
			{
				List<Request> RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking().Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).ToList();
				if (RequestList != null)
				{
					ViewBag.RequestCount = RequestList.Count;
				}
				else
				{
					ViewBag.RequestCount = 0;
				}
			}
		}
		else
		{
			List<Request> RequestList = _unitOfWork.Repository<Request>().GetQueryAsNoTracking(x => x.CreatedBy == userId || x.LabL4 == userId || x.UserL4 == userId).Include(I => I.InstrumentModel).Include(I => I.RequestStatusModel).ToList();
			if (RequestList != null)
			{
				ViewBag.RequestCount = RequestList.Count;
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

			});
		}
		return Json(MStranslater);
	}

	public IActionResult ObservationTypeTranslation()
	{
		//List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.Attrform == "Instrument").ToList());
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
		List<Department> DepartmentList = _unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList();
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

			});
		}
		return Json(DepartTranslater);

	}
	public class DepartmentLangTranslate
	{
		public int id { get; set; }
		public string? Name { get; set; }

		public string? NameJp { get; set; }

	}

	public class MasterLangTranslate
	{
		public int id { get; set; }
		public string? NameEng { get; set; }

		public string? NameJp { get; set; }

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
