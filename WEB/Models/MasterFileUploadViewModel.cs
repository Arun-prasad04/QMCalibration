namespace WEB.Models
{
    public class MasterFileUploadViewModel
    {
        public int Id { get; set; }
        public int MasterId { get; set; }
        public int UploadId { get; set; }

        public string Filename { get; set; }
    }
}