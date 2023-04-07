namespace WEB.Models
{


    public class MechanicalCalibration
    {
        public string FormatNo { get; set; }
        public string RevNo { get; set; }
        public string IntermediateCheck { get; set; }
        public string MasterEquipmentID { get; set; }
        public string RangeSizeLeastCount { get; set; }
        public string MasterEquipment { get; set; }
        public string DamagesDentAndScratches { get; set; }
        
        public string DamagesDentAndScratchesComments { get; set; }
        public string RustAndOxidation { get; set; }
        
        public string RustAndOxidationComments { get; set; }
        public string AccessoriesFixturesAndOthers { get; set; }
        
        public string AccessoriesFixturesAndOthersComments { get; set; }
        public string AbnormalitiesFound { get; set; }
        
        public string AbnormalitiesFoundComments { get; set; }
        public string PoweringIssue { get; set; }
        public string PoweringIssueComments { get; set; }
        public string SmoothMovement { get; set; }
        
        public string SmoothMovementComments { get; set; }
        
        public string NoiseObserverWhileFunctioning { get; set; }
        
        public string NoiseObserverWhileFunctioningComments { get; set; }
        public string Faults { get; set; }
       
        public string FaultsComments { get; set; }
       
        public string OtherAbnormalities { get; set; }
        public string OtherAbnormalitiesComments { get; set; }
        public string EquipmentusedToChecktTheMasterEquipment { get; set; }
        public string 	EquipmentID { get; set; }
        public string RangeSize { get; set; }
        public string Accuracy { get; set; }
        public string Acceptance { get; set; }
        public string LeastCount { get; set; }
        public string OtherComments { get; set; }
        public string StudyPerformedBy { get; set; }
        public string TechnicalManager { get; set; }
        public string StudyPerformerSignature { get; set; }
        public string TechnicalManagerSignature { get; set; }
         public string StudyPerformingDate { get; set; }
        public string TechnicalManagerSignaturedDate { get; set; }

        public List<Accuracychecks> Accuracycheck {get;set;}

        

    }
    public class Accuracychecks{
        public string MasterEquipmentCalibrationResults1 { get; set; }
        public string MasterEquipmentCalibrationResults2 { get; set; }
         public string CurrentInternalCheck1 { get; set; }
        public string CurrentInternalCheck2 { get; set; }
         public string Difference1 { get; set; }
        public string Difference2 { get; set; }
         public string AcceptanceCriteria { get; set; }
        public string Decision { get; set; }

    }
}