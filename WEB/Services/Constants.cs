namespace WEB.Services;
public static class Constants
{
    public static string FeedbackForm_Subject = "Customer Feedback Form";
    public static string FeedbackForm_Body = "Welcome to all, please fill your comments...";
    public static string QCAlternative_FolderName = "QCAlternateMethodFiles";
    public static string QCReplicateTest_FolderName = "QCReplicateTestFiles";
    public static string QCReeTest_FolderName = "QCReTestFiles";

    public static string Signature_FolderName = "UserSignature";

    public static string Certification_FolderName = "Certification";   
    public static string CustomerFeedback_FolderName = "CustomerFeedbackFiles";

    public static string QRCODE_FILENAME = "CMTQRfile";
    public static string QRCODE_FILE_EXTENSION = ".pdf";
    public static string QRCODE_IMAGE_EXTENSION = ".png";
    public static string QRCODE_IMAGE_FORMAT  = "data:image/png;base64,{0}";

    //Font Style
    public static string QRCODE_FONT_NAME = "Arial";
    public static int QRCODE_FONT_SIZE = 48;

    //Request Status
    public static string REQUEST_STATUS = "Pending";

    //AmendNo generation
    public static string AMENDNO_CHARACTER = "A";

    public static string RESULT_ONE_SUBMITTED = "Result One Submitted";
    public static string RESULT_TWO_SUBMITTED = "Result Two Submitted";
    public static string APPROVED = "Approved";
    public static string REJECTED = "Rejected";

    public static string CONTROLLERNAME = "Certification/ViewPdfFiles/";

    public static string ULRNumberFormat = "CC2021{0}0{1}F";
    public static string CertificateNumberFormat="DICV/CL/{0}/{1}";

    public static int ZeroCount = 13;

      public static int certificateZeroCount = 9;

}