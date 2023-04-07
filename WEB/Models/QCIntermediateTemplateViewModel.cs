namespace WEB.Models
{


    public class QCIntermediateTemplateViewModel
    {
        public int Id { get; set; }
        public string FormatNo { get; set; }
        public string RevisionNo { get; set; }
        public DateTime RevisionDate { get; set; }
        public int? MasterEquipmentId1 { get; set; }
        public string? EquipmentName1 { get; set; }
        public string? RangeOrSize1 { get; set; }
        public int? LeastCount1 { get; set; }
        public int? MasterEquipmentId2 { get; set; }
        public string? EquipmentName2 { get; set; }
        public int? MasterEquipmentId3 { get; set; }
        public string? EquipmentName3 { get; set; }
        public string? RangeOrSize3 { get; set; }
        public int? LeastCount3 { get; set; }
        public string? DamageDent { get; set; }
        public string? RustAndOxidation { get; set; }
        public string? Accessories { get; set; }
        public string? PhysicalAbnormalities { get; set; }
        public string? PoweringIssue { get; set; }
        public string? SmoothMovement { get; set; }
        public string? NoiseObserved { get; set; }
        public string? SoftwareFaults { get; set; }
        public string? CommentsOfDamageDent { get; set; }
        public string? CommentsOfRustAndOxidation { get; set; }
        public string? CommentsOfAccessories { get; set; }
        public string? CommentsOfPhysicalAbnormalities { get; set; }
        public string? CommentsOfPoweringIssue { get; set; }
        public string? CommentsOfSmoothMovement { get; set; }
        public string? CommentsOfNoiseObserved { get; set; }
        public string? CommentsOfSoftwareFaults { get; set; }
        public string? CommentsOfOtherAbnormalities { get; set; }
        public string? OtherAbnormalities { get; set; }
        public string? DoubtsOfPerformance { get; set; }
        public string? CommentsOfAcceptance { get; set; }
        public string? OtherComments { get; set; }
        public int? StudyPerformedBy { get; set; }
        public DateTime? SignDate { get; set; }
        public int? TechnicalManager { get; set; }
        public DateTime? TMSignDate { get; set; }
        public Int16 DocumentStatus { get; set; }
        public string? TMRemarks { get; set; }
        public string? StudyPerformedByName { get; set; }
        public string? TechnicalManagerName {get;set;}
        public string? Status { get; set; }
        public string? DataUnit {get;set;} 

        public string? RevisionNoAndDate {get;set;}
        public string? Master1LabId{get;set;}        
        public string? Master2LabId{get;set;}
        public string? Master3LabId{get;set;}
    

        public List<QCIntermediateTemplateResultViewModel> QCIntermediateResultViewModelList { get; set; }
    }
}
public class InstrumentListViewModel
{
    public int InstrumentId { get; set; }
    public string InstrumentName { get; set; }
    public string IdNo	{get;set;}

    public string RangeOrSize { get; set; }
    public string LeastCount { get; set; }
}

public class MasterListViewModel
{
    public int MasterId { get; set; }
    public string LabId{get;set;}
    public string EquipmentName { get; set; }
    public DateTime? CalibDate { get; set; }
    public string? Range { get; set; }

}

