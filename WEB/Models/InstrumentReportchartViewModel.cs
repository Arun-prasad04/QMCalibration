namespace WEB.Models;

public class InstrumentReportchartViewModel
{
    public int DepartmentId { get; set; }
    public int InstrumentType { get; set; }
    public int Year { get; set; }

    public string Departments { get; set; }

    public string TargetValue { get; set; }
    public List<DepartmentViewModel> DepartmentList { get; set; }

    public string? Month { get; set; }
    public int Request { get; set; }
    public string? TypeofRequest { get; set; }
    public string? Noofrequestsreceived { get; set; }
    public string? Completedonsameday { get; set; }
    public string? Within1day { get; set; }
    public string? Within2day { get; set; }
    public string? Within3day { get; set; }
    public string? Calculation { get; set; }


}

public class PieChartViewModel
{
    public string? Label { get; set; }
    public decimal? Value { get; set; }
    //public string? Color { get; set; }
    public string? DepartmenName { get; set; }
}
