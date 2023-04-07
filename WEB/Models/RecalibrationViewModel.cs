namespace WEB.Models;
public class RecalibrationViewModel
{
    public string FormatNo{get;set;}
    public string RevNo{get;set;}
    public string DeviceName{get;set;}
    public string Device{get;set;}
    public string Range{get;set;}
    public string LeastCount{get;set;}
    public string MasterEquipmentUsed{get;set;}
    public string MasterEquipmentIdNo{get;set;}
    public string Date1{get;set;}
    public string Date2{get;set;}
    public string Averagex{get;set;}
    public string AverageX{get;set;}
    public string MUx{get;set;}
    public string MUX{get;set;}
    public string U21{get;set;}
    public string U22{get;set;}
    public string EnValue{get;set;}
    public string StudyPerformedBy1{get;set;}
    public string StudyPerformedBy2{get;set;}
    public string Date3{get;set;}
    public string Date4{get;set;}
    public string Conclusion{get;set;}
    public string ApprovedBy{get;set;}
    public List<Observation> ObsValue{get;set;}
}
 public class Observation{
     public string? Obs1{get;set;}
     public string? Obs2{get;set;}
     public string? Obs3{get;set;}
     public string? Obs4{get;set;}
     public string? Obs5{get;set;}
     public string? Obs6{get;set;}
     public string? Obs7{get;set;}
     public string? Obs8{get;set;}
     public string? Obs9{get;set;}
     public string? Obs10{get;set;}
 }