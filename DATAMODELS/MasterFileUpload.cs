namespace CMT.DATAMODELS
{
    public partial class MasterFileUpload
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int UploadId { get; set; }

        public virtual Master Masters{get;set;}
         public virtual Uploads Upload{get;set;}
    }
}