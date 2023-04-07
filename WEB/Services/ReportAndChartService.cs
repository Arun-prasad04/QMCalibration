using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
namespace WEB.Services;

public class ReportAndChartService : IReportAndChartService
{
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork { get; set; }
    private IHttpContextAccessor _contextAccessor { get; set; }
    private IEmailService _emailService;
    public ReportAndChartService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _contextAccessor = contextAccessor;
        _emailService = emailService;
    }
    public ResponseViewModel<InstrumentReportchartViewModel> CreateNewInstrumentReport()
    {
        try
        {
            List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
            InstrumentReportchartViewModel instrumentReportchartViewModel = new InstrumentReportchartViewModel();
            instrumentReportchartViewModel.DepartmentList = departmentList;

            return new ResponseViewModel<InstrumentReportchartViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = instrumentReportchartViewModel,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<InstrumentReportchartViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "ReportAndChartService",
                ResponseServiceMethod = "CreateNewInstrumentReport"
            };
        }
    }
    public ResponseViewModel<InstrumentViewModel> GetAllInstrumentQuarantineList(int departmentId)
    {
        var returnObject = new List<InstrumentViewModel>();

        try
        {
            var instrumentList = _unitOfWork.Repository<Instrument>()
                                            .GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 1)
                                            .Include(I => I.QuarantineModel)
                                            .Include(I => I.DepartmenttModel);

            if (departmentId != 0)
            {
                var departmentInstrumentList = instrumentList.Where(x => x.DepartmenttModel.Id == departmentId);

                returnObject = departmentInstrumentList.Select(s => new InstrumentViewModel()
                {
                    Id = s.Id,
                    InstrumentName = s.InstrumentName,
                    Status = s.Status
                })
                .ToList();
            }
            else
            {
                returnObject = instrumentList.Select(s => new InstrumentViewModel()
                {
                    Id = s.Id,
                    InstrumentName = s.InstrumentName,
                    Status = s.Status
                })
                .ToList();
            }

            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseDataList = returnObject
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseServiceMethod = "Instrument",
                ResponseService = "GetAllInstrumentList"
            };
        }
    }
    public ResponseViewModel<InstrumentViewModel> GetAllInstrumentList(int departmentId)
    {
        var returnObject = new List<InstrumentViewModel>();

        try
        {
            var instrumentList = _unitOfWork.Repository<Instrument>()
                                            .GetQueryAsNoTracking(Q => Q.QuarantineModel.Select(s => s.StatusId).FirstOrDefault() == 2)
                                            .Include(I => I.QuarantineModel)
                                            .Include(I => I.DepartmenttModel);

            if (departmentId != 0)
            {
                var departmentInstrumentList = instrumentList.Where(x => x.DepartmenttModel.Id == departmentId);

                returnObject = departmentInstrumentList.Select(s => new InstrumentViewModel()
                {
                    Id = s.Id,
                    InstrumentName = s.InstrumentName,
                    Status = s.Status
                })
                .ToList();
            }
            else
            {
                returnObject = instrumentList.Select(s => new InstrumentViewModel()
                {
                    Id = s.Id,
                    InstrumentName = s.InstrumentName,
                    Status = s.Status
                })
                .ToList();
            }

            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseDataList = returnObject
            };
        }

        catch (Exception e)
        {
            return new ResponseViewModel<InstrumentViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseServiceMethod = "Instrument",
                ResponseService = "GetAllInstrumentList"
            };
        }
    }


    public ResponseViewModel<ChartDataViewModel> GetChartData(int Year, int DepartmentId)
    {
        try
        {
            List<FeedbackInvite> feedbackInviteList = new List<FeedbackInvite>();
            List<ChartDataViewModel> chartDataList = new List<ChartDataViewModel>();

            feedbackInviteList = _mapper.Map<List<FeedbackInvite>>(_unitOfWork.Repository<FeedbackInvite>()
                                        .GetQueryAsNoTracking()
                                        .Where(d => d.InvitedOn.Year == Year))
                                        .ToList();
            Department department = _unitOfWork.Repository<Department>().GetQueryAsNoTracking().Where(d => d.Id == DepartmentId).SingleOrDefault();

            var monthList = feedbackInviteList.GroupBy(x => x.InvitedOn.Month)
                               .Select(x => new
                               {
                                   x.Key
                               }).ToList();

            for (int i = 0; i < monthList.Count(); i++)
            {
                var month = monthList[i].Key;

                var dateTime = new DateTime(DateTime.Now.Year, month, 1); ;

                var monthName = dateTime.ToString("MMM");

                var feedbackInviteListIds = feedbackInviteList
                                            .Where(x => x.InvitedOn.Month == month)
                                            .Select(x => x.Id)
                                            .ToList();
                decimal feedBackAvg = 0;
                int totalPersonCount = 0;
                if (DepartmentId == 0)
                {
                    List<FeedbackData> feedbackDataList = _unitOfWork.Repository<FeedbackData>()
                                                                  .GetQueryAsNoTracking()
                                                                  .Where(d => d.FeedbackStatus == (int)FeedbackDocumentStatus.Reviewed
                                                                          && feedbackInviteListIds.Contains(d.FeedbackInviteId))
                                                                  .ToList();

                    Decimal? totalAvg = feedbackDataList.Select(s => s.OverallPercentage).Sum();
                    totalPersonCount = feedbackDataList.Select(s => s.OverallPercentage).Count();
                    if (totalAvg > 0 && totalPersonCount > 0)
                        feedBackAvg = totalAvg.GetValueOrDefault() / totalPersonCount;

                }
                else
                {
                    List<FeedbackData> feedbackDataList = _unitOfWork.Repository<FeedbackData>()
                                                                  .GetQueryAsNoTracking()
                                                                  .Where(d => d.DepartmentId == DepartmentId
                                                                               && feedbackInviteListIds.Contains(d.FeedbackInviteId)
                                                                               && d.FeedbackStatus == (int)FeedbackDocumentStatus.Reviewed)
                                                                  .ToList();

                    Decimal? totalAvg = feedbackDataList.Select(s => s.OverallPercentage).Sum();
                    totalPersonCount = feedbackDataList.Select(s => s.OverallPercentage).Count();

                    totalPersonCount = feedbackDataList.Select(s => s.OverallPercentage).Count();

                    if (totalAvg > 0 && totalPersonCount > 0)
                        feedBackAvg = totalAvg.GetValueOrDefault() / totalPersonCount;

                }
                if (DepartmentId == 0)
                {
                    ChartDataViewModel chartData = new ChartDataViewModel()
                    {

                        Label = "All",
                        Value = feedBackAvg,
                        Month = monthName,
                        Year = Year
                    };
                    if (feedBackAvg > 0)
                        chartDataList.Add(chartData);
                }
                else
                {
                    ChartDataViewModel chartData = new ChartDataViewModel()
                    {

                        Label = department.Name,
                        Value = feedBackAvg,
                        Month = monthName,
                        Year = Year
                    };
                    if (feedBackAvg > 0)
                        chartDataList.Add(chartData);
                }
            }


            return new ResponseViewModel<ChartDataViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = chartDataList
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<ChartDataViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "Instrument",
                ResponseService = "FeedbackDataService"
            };
        }
    }
    public ResponseViewModel<RequestKPIViewModel> GetCalibrationlab(int Year, int Month, int Request)
    {
        try
        {
            List<RequestViewModel> RequestList = new List<RequestViewModel>();

            if (Request == 0)
            {
                RequestList = _unitOfWork.Repository<Request>()
                                         .GetQueryAsNoTracking()
                                         .Where(s=> s.CreatedOn.Value.Year==(Year == 0 ? s.CreatedOn.Value.Year : Year) 
                                         && s.CreatedOn.Value.Month == (Month == 0 ? s.CreatedOn.Value.Month : Month)
                                         &&(s.StatusId == 27 || s.StatusId == 29 || s.StatusId == 30))                                      
                                         .Include(I => I.RequestStatusModel)
                                         .Select(s => new RequestViewModel()
                                         {
                                             SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved)
                                                      .Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
                                             ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent)
                                                     .Select(S => S.CreatedOn).FirstOrDefault(),
                                             Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn)
                                                 .Select(S => S.StatusId).FirstOrDefault(),
                                             TypeOfRequest = s.TypeOfReqest,
                                             ReqestNo=s.ReqestNo                                             
                                         }).ToList();
            }
            else
            {
                RequestList = _unitOfWork.Repository<Request>()
                                         .GetQueryAsNoTracking()
                                         .Where(s=> s.CreatedOn.Value.Year==(Year == 0 ? s.CreatedOn.Value.Year : Year) 
                                         && s.CreatedOn.Value.Month == (Month == 0 ? s.CreatedOn.Value.Month : Month)
                                         && (s.StatusId == 27 || s.StatusId == 29 || s.StatusId == 30)
                                         && s.TypeOfReqest == Request)
                                         .Include(I => I.RequestStatusModel)
                                         .Select(s => new RequestViewModel()
                                         {
                                             SubmittedOn = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Approved)
                                                      .Select(S => S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
                                             ReturnDate = s.RequestStatusModel.Where(W => W.StatusId == (int)EnumRequestStatus.Sent)
                                                     .Select(S => S.CreatedOn).FirstOrDefault(),
                                             Status = s.RequestStatusModel.OrderByDescending(O => O.CreatedOn)
                                                 .Select(S => S.StatusId).FirstOrDefault(),
                                             TypeOfRequest = s.TypeOfReqest,
                                             ReqestNo=s.ReqestNo
                                         }).ToList();
            }

            List<RequestKPIViewModel> requestKPIModelList = new List<RequestKPIViewModel>();

           var yearList = from requestYear in RequestList
                          group requestYear by requestYear.SubmittedOn.GetValueOrDefault().Year into requests
                          select requests;

            int startMonth = Month == 0 ? 1 : Month;
            int endMonth = Month == 0 ? 12 : Month;
            

            foreach (var year in yearList)
            {
                for (int iMonth = startMonth; iMonth <= endMonth; iMonth++)
                {
                    var dateTime = new DateTime(DateTime.Now.Year, iMonth, 1); ;
                    var monthName = dateTime.ToString("MMM");

                    for (int requestType = 1; requestType <= 3; requestType++)
                    {
                        int samedayCount = 0;
                        int onedayCount = 0;
                        int twodaysCount = 0;
                        int threedaysCount = 0;
                        int morethanThreeDays = 0;
                        int totalRequestReceivd = 0;
                        int overallcount=0;
                        string requestTypeName = requestType == 1 ? "New"
                                                                : requestType == 2
                                                                ? "Regular"
                                                                : "Recalibration";
                        foreach (var monthRequestData in RequestList.Where(x => x.SubmittedOn
                                                                                 .GetValueOrDefault().Month == iMonth
                                                                                && x.SubmittedOn
                                                                                 .GetValueOrDefault().Year == Year 
                                                                                && x.TypeOfRequest == requestType)
                                                                    .ToList())
                        {
                            int weekendDays = 0;
                            DateTime st_date=Convert.ToDateTime(monthRequestData.SubmittedOn);
                            DateTime ed_date=Convert.ToDateTime(monthRequestData.ReturnDate);

                            for(DateTime date = st_date; date.Date <= ed_date; date = date.AddDays(1))
                            {
                                if ((date.DayOfWeek == DayOfWeek.Saturday) || (date.DayOfWeek == DayOfWeek.Sunday))
                                {
                                    weekendDays++;
                                }
                            }
                            TimeSpan days = (monthRequestData.ReturnDate - monthRequestData.SubmittedOn).GetValueOrDefault();
                            int Actual_Days=days.Days-weekendDays;
                            if (Actual_Days <= 0)
                            {
                                samedayCount++;
                                overallcount++;
                            }
                            else if (Actual_Days == 1)
                            {
                                onedayCount++;
                                overallcount++;
                            }
                            else if (Actual_Days == 2)
                            {
                                twodaysCount++;
                                overallcount++;
                            }
                            else if (Actual_Days == 3)
                            {
                                threedaysCount++;
                            }
                            else
                            {
                                morethanThreeDays++;
                            }
                        }

                        totalRequestReceivd = samedayCount + onedayCount + twodaysCount + threedaysCount + morethanThreeDays;
                        //With in 2 days KPI calculation
                        var calculation=0;
                        if(totalRequestReceivd==0)
                        {
                            calculation=0;
                        }
                        else
                        {
                            calculation = overallcount * 100 / totalRequestReceivd;
                        }
    
                        ChartDataViewModel chartData = new ChartDataViewModel()
                        {
                            Label = requestTypeName,
                            Value = calculation,
                            Month = monthName
                        };

                        RequestKPIViewModel requestKPIViewModel = new RequestKPIViewModel
                        {
                            Month = monthName,
                            TypeOfRequest = requestTypeName,
                            NoOfRequestsReceived = totalRequestReceivd,
                            CompletedOnSameDay = samedayCount,
                            WithinOneDay = onedayCount,
                            WithinTwoDays = twodaysCount,
                            WithinThreeDays = threedaysCount,
                            morethanThreeDays=morethanThreeDays,
                            Calculation = calculation,
                            ChartData = chartData
                        };
                        requestKPIModelList.Add(requestKPIViewModel);
                    }
                }
            }
            var MonthList=requestKPIModelList.Select(s=> s.Month).ToList();
            List<ChartDataViewModel>? NewRequestType=requestKPIModelList.Where(x=>x.TypeOfRequest=="New").Select(x=>x.ChartData).ToList();
            var RegularRequestType=requestKPIModelList.Where(x=>x.TypeOfRequest=="Regular").Select(x=>x.ChartData).ToList();
            var RecalibrationRequestType=requestKPIModelList.Where(x=>x.TypeOfRequest=="Recalibration").Select(x=>x.ChartData).ToList();
            requestKPIModelList.First().ChartDataNew=new List<ChartDataViewModel>();
            requestKPIModelList.First().ChartDataNew=NewRequestType;
            requestKPIModelList.First().ChartDataRegular=new List<ChartDataViewModel>();
            requestKPIModelList.First().ChartDataRegular=RegularRequestType;
            requestKPIModelList.First().ChartDataRecalibration=new List<ChartDataViewModel>();
            requestKPIModelList.First().ChartDataRecalibration=RecalibrationRequestType;            
            return new ResponseViewModel<RequestKPIViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = requestKPIModelList,
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<RequestKPIViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "GetCalibrationlab",
                ResponseService = "ReportAndChartService",

            };
        }
    }
}