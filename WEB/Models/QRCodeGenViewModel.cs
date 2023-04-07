namespace WEB.Models;
public class QRCodeGenViewModel
{
        public string UserId { get; set; }
        public string InstrumentId { get; set; }
        public string RequestId { get; set; }

        public string TemplateName { get; set; }
        public string EncodeInputText { get; set; }
        public string QRFilepath { get; set; }
        public string QRFilename { get; set; }
        public string QRImageUrl { get; set; }
        public string DrawText { get; set; }
}