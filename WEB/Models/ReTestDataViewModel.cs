namespace WEB.Models
{

  public class ReTestDataViewModel
  {
    public int Id { get; set; }
    public int ParentId { get; set; }
    public int AppraiserNo { get; set; }
    public string AppraiserName { get; set; }
    public DateTime AppraiserDate { get; set; }
    public decimal Observation1 { get; set; }
    public decimal Observation2 { get; set; }
    public decimal Observation3 { get; set; }
    public decimal Observation4 { get; set; }
    public decimal Observation5 { get; set; }
    public decimal Observation6 { get; set; }
    public decimal Observation7 { get; set; }
    public decimal Observation8 { get; set; }
    public decimal Observation9 { get; set; }
    public decimal Observation10 { get; set; }
    public decimal Average { get; set; }
    public decimal MU { get; set; }
    public decimal U2 { get; set; }
    public int SINo { get; set; }
     public string AppraiserFullName {get;set;}
  }
}