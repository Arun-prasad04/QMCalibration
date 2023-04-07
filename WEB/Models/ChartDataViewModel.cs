namespace WEB.Models
{

    public class ChartDataViewModel
    {
        public string? Label { get; set; }
        public string? Month { get; set; }
        public decimal Value { get; set; }
        public string? Color { get; set; }
        public int Year { get; set; }
        public string? Department { get; set; }
        public int Request { get; set; }   
        public List<DepartmentViewModel> DepartmentList { get; set; }

    }

    public class BarChartViewModel
    {
        public string? Label { get; set; }
        public string? Month { get; set; }
        public decimal Value { get; set; }
        public int Year { get; set; }
        public string? Department { get; set; }
    }
}