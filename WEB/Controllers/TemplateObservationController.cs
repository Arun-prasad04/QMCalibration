using AutoMapper;
using CMT.DAL;
using Microsoft.AspNetCore.Mvc;
using WEB.Services.Interface;

namespace WEB.Controllers
{
	public class TemplateObservationController : BaseController
    {
        private readonly IMapper _mapper;
        private IUnitOfWork _unitOfWork { get; set; }
        private IRequestService _requestService { get; set; }
        private ITemplateObservationService _ITemplateObservationService { get; set; }
        private IInstrumentService _instrumentService { get; set; }
        private IMasterService _iMasterService { get; set; }
        public TemplateObservationController(ITemplateObservationService TemplateObservationService, ILogger<BaseController> logger, IHttpContextAccessor contextAccessor, IRequestService requestService, IInstrumentService instrumentService, IUnitOfWork unitOfWork) : base(logger, contextAccessor)
        {
            _ITemplateObservationService = TemplateObservationService;
            _requestService = requestService;
            _instrumentService = instrumentService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
		{
			return View();
		}
	}
}
