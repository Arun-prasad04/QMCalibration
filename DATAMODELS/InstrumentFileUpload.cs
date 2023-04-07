namespace CMT.DATAMODELS
{
    public partial class InstrumentFileUpload
    {
        public int Id { get; set; }
        public int InstrumentId { get; set; }
        public int UploadId { get; set; }

        public virtual Instrument Instruments{get;set;}
         public virtual Uploads Upload{get;set;}
    }
}