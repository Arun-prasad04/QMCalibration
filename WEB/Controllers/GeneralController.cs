using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WEB.Models;


namespace WEB.Controllers;

public class GeneralController : Controller
{
    private readonly ILogger<GeneralController> _logger;

    public GeneralController(ILogger<GeneralController> logger)
    {
        _logger = logger;
    }

    public IActionResult General()
    {
        GeneralViewModel obj = new GeneralViewModel();
        
        obj.Name = "Micrometer";
        obj.Range = "0.5 mm";
        obj.Make = "India Tools";
        // obj.TempreatureAtStart = "30";
        // obj.TempreatureAtEnd = "31";
        // obj.Humidity = "15%";
        // obj.ReferenceWI = "0.255";
        // obj.AllValuesIn = "mm";
        // obj.CalibrationPerformedBy = "Srinidhi";
        // obj.CalibrationDoneDate = "14/12/2021";
        // obj.CalibrationReviewedBy = "Ramesh";
        // obj.CalibrationReviewedDate = "24/12/2021";



        List<Repeatability> obj3 = new List<Repeatability>();
        obj3.Add(new Repeatability
        {
            Repeatability1 = 32,
            Repeatability2 = 33,
            Repeatability3 = 34,
            Repeatability4 = 30,
            Repeatability5 = 29
        });
        obj.Repeat = obj3;

        List<LMeasurement> obj1 = new List<LMeasurement>();
        obj1.Add(new LMeasurement { LMeasured = 55, LTrial1 = 10, LTrial2 = 60, LTrial3 = 70 });
        obj1.Add(new LMeasurement { LMeasured = 57, LTrial1 = 10, LTrial2 = 60, LTrial3 = 70 });
        obj.LeftMeasurement = obj1;
        List<RMeasurement> obj2 = new List<RMeasurement>();
        obj2.Add(new RMeasurement { RMeasured = 5, RTrial1 = 10, RTrial2 = 60, RTrial3 = 70 });
        obj2.Add(new RMeasurement { RMeasured = 5, RTrial1 = 10, RTrial2 = 60, RTrial3 = 70 });
        obj2.Add(new RMeasurement { RMeasured = 5, RTrial1 = 10, RTrial2 = 60, RTrial3 = 70 });
        obj2.Add(new RMeasurement { RMeasured = 7, RTrial1 = 10, RTrial2 = 60, RTrial3 = 70 });
        obj.RightMeasurement = obj2;

        return View(obj);


    }

    public IActionResult PlungerDial()
    {
        PlungerDialViewModel obj5 = new PlungerDialViewModel();
        
        obj5.Name = "Mechanical Dial";
        obj5.RangeLC = "0.55";
        obj5.Make = "India Tools";
        obj5.SerialNumber = "125478";
        obj5.IdNumber = "1596";
        obj5.ReferenceStandard = "High Precise";
        obj5.TempStart = "30";
        obj5.TempEnd = "31";
        obj5.Humidity = "15%RH";
        obj5.RefWi = "365.QM.C.077";
        obj5.Allvalues ="mm";
        obj5.Remarks="No Remarks";
        obj5.CalibrationPerformedBy="";
       


        return View(obj5);
    }







   
}