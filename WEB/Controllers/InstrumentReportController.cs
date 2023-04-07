using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;
using WEB.Services.Interface;
using System;
using System.Collections.Generic;
namespace WEB.Controllers;

public class InstrumentReportController : BaseController
{
    private IReportAndChartService _reportAndChartService{get;set;}
    

    private IDepartmentService _departmentService { get; set; }
    public InstrumentReportController(IReportAndChartService reportAndChartService,IDepartmentService departmentService,ILogger<BaseController>logger,IHttpContextAccessor contextAccessor):base(logger,contextAccessor)
    {
        _reportAndChartService=reportAndChartService;
        _departmentService = departmentService;
    }
    public IActionResult donutchart()
    {
        ResponseViewModel<InstrumentReportchartViewModel> response=_reportAndChartService.CreateNewInstrumentReport();
        DepartmentViewModel departmentViewModel=new DepartmentViewModel
        {
            Id=0,
            Name="All"
        };
        response.ResponseData.DepartmentList.Add(departmentViewModel);
        response.ResponseData.DepartmentList=response.ResponseData.DepartmentList.OrderBy(x=>x.Id).ToList();
        return View(response.ResponseData);   
    }
    public IActionResult InstrumentPieChart(InstrumentReportchartViewModel instrumentpiechart)
    {
        int departmentId=instrumentpiechart.DepartmentId;
        int instrumentType = instrumentpiechart.InstrumentType;
        int userId=Convert.ToInt32(base.SessionGetString("LoggedId"));
        

        if(instrumentType==1){

            var vm = new List<PieChartViewModel>();

            var reportData = _reportAndChartService.GetAllInstrumentList(departmentId).ResponseDataList;

            foreach(var row in reportData)
            {
                if (vm.Any(x=>x.Label == row.InstrumentName))
                    {
                        var d = vm.Where(x=>x.Label == row.InstrumentName).SingleOrDefault();
                        d.Value++;
                    }
                    else
                    {
                        vm.Add(new PieChartViewModel(){ Label = row.InstrumentName, Value = 1});

                    }
            }
            return Json(vm);  
        }
        
        else{
            var vm = new List<PieChartViewModel>();
            var reportData = _reportAndChartService.GetAllInstrumentQuarantineList(departmentId).ResponseDataList;
            foreach(var row in reportData)
            {
                if (vm.Any(x=>x.Label == row.InstrumentName))
                    {
                        var d = vm.Where(x=>x.Label == row.InstrumentName).SingleOrDefault();
                        d.Value++;
                    }
                    else
                    {
                        vm.Add(new PieChartViewModel(){ Label = row.InstrumentName, Value = 1});

                    }
            }
            return Json(vm);
        }
    }
    public IActionResult CustomerReport(InstrumentReportchartViewModel instrumentVM)
    {
        var Year = instrumentVM.Year;
        var DepartmentId = instrumentVM.DepartmentId;
        var TargetValue = instrumentVM.TargetValue;

        var vm = new List<ChartDataViewModel>();
        var reportData = _reportAndChartService.GetChartData(instrumentVM.Year, instrumentVM.DepartmentId).ResponseDataList;

        ResponseViewModel<ChartDataViewModel> response = _reportAndChartService.GetChartData(instrumentVM.Year, instrumentVM.DepartmentId);
        ResponseViewModel<DepartmentViewModel> departmentresponse = _departmentService.GetAllDepartmentList();
        ViewBag.departmentList = departmentresponse.ResponseDataList;
        ViewBag.departmentId = instrumentVM.DepartmentId;

        var currentYear = DateTime.Now.Year;
        var yearsList = new List<int>();
        for(int year = Constants.REPORT_START_YEAR ;year<=currentYear;year++)
        {
            yearsList.Add(year);
        }

        ViewBag.currentYear = currentYear;
        ViewBag.yearsList = yearsList;
        
        return View(response.ResponseDataList);
    }

    public IActionResult GetChartData(int year, int depid)
    {
        ResponseViewModel<ChartDataViewModel> response = _reportAndChartService.GetChartData(year, depid);
        return Json(response.ResponseDataList);
    }

    public IActionResult CalibrationLab(InstrumentReportchartViewModel instrumentReportchartViewModel)
    {
        var Year = instrumentReportchartViewModel.Year == null ? DateTime.Now.Year
                                                               : instrumentReportchartViewModel.Year;           
        var Month = instrumentReportchartViewModel.Month == null ? DateTime.Now.Month
                                                                 : int.Parse(instrumentReportchartViewModel.Month);
        var RequestType = instrumentReportchartViewModel.Request;
        var TargetValue = instrumentReportchartViewModel.TargetValue;

        ResponseViewModel<RequestKPIViewModel> response = _reportAndChartService.GetCalibrationlab(Year,Month, RequestType);

        var currentYear = DateTime.Now.Year;
        var yearsList = new List<int>();
        for(int year = Constants.REPORT_START_YEAR ;year<=currentYear;year++)
        {
            yearsList.Add(year);
        }

        ViewBag.currentYear = currentYear;
        ViewBag.yearsList = yearsList;
        if (response.ResponseDataList==null || !response.ResponseDataList.Any())
        {
            return View(new List<RequestKPIViewModel>());
        }
        return View(response.ResponseDataList);
    }
    public IActionResult GetCalibrationlab(int year, int month, int Request)
    {
        ResponseViewModel<RequestKPIViewModel> response = _reportAndChartService.GetCalibrationlab(year,month, Request);
        return Json(response.ResponseDataList);
    }
}
