
using System.Globalization;

namespace WEB.Controllers;
public static class Constants
{
    public static string FORMATNUMER = "1221";
    public static string REVISION_AND_DATE = "011 & 2021";
    public static string REVISION_NUMBER = "1001";
    public static DateTime REVISION_DATE = new DateTime(2022, 01, 02); // DateTime.Parse("02-01-2022");
    public static string DEFAULT_PASSWORD = "Passwd@123";
    public static int REPORT_START_YEAR = 2020;
    public static string DEFAULT_DESIGNATION = "Product Executive";
    public static string CONTROLLERNAME = "Certification/ViewPdfFiles/";

    public static string FOLDERNAME= "Certification";
    public static string INTERMEDIATE_FORMATNUMER = "365.QM.D.855";
    public static string INTERMEDIATE_REVISIONNO_DATE = "00/28.03.2016";
    public static string INTERMEDIATE_REVISIONNO = "00";
    public static DateTime INTERMEDIATE_REVISIONDATE = new DateTime(2016,03,28); //DateTime.ParseExact("28-03-2016", "MM/dd/yyyy", CultureInfo.InvariantCulture);//DateTime.Parse("28.03.2016");

	public static string ALTERNATIVE_FORMATNUMER = "365.QM.D.886";
    public static string ALTERNATIVE_REVISIONNO_DATE = "00/26.07.2018";
    public static string ALTERNATIVE_REVISIONNO = "00";
    public static DateTime ALTERNATIVE_REVISIONDATE = new DateTime(2018, 07, 26); //DateTime.ParseExact("26.07.2018", "dd/MM/yyyy", null);

	public static string REPLICATE_FORMATNUMER = "365.QM.D.865";
    public static string REPLICATE_REVISIONNO_DATE = "00/28.03.2016";
    public static string REPLICATE_REVISIONNO = "00";
    public static DateTime REPLICATE_REVISIONDATE = new DateTime(2016, 03, 28);// DateTime.ParseExact("28.03.16", "dd/MM/yyyy", null);
	public static string RESTEST_FORMATNUMER = "365.QM.D.855";
    public static string RESTEST_REVISIONNO_DATE = "00/28.03.16";
    public static string RESTEST_REVISIONNO = "00";
    public static DateTime RETEST_REVISIONDATE = new DateTime(2016, 03, 28);// DateTime.ParseExact("28.03.16", "dd/MM/yyyy", null);
	public static string MICROMETER_REFERENCE_WITH_INDICATOR = "365.QM.C.074";
    public static string VERNIER_CALIPER_REFERENCE_WITH_INDICATOR = "365.QM.C.072";
    public static string PLUNGER_DIAL_REFERENCE_WITH_INDICATOR = "365.QM.C.077";
    public static string LEVER_DIAL_REFERENCE_WITH_INDICATOR = "365.QM.C.076";
    public static string TORQUE_WRENCH_REFERENCE_WITH_INDICATOR = "365.QM.C.";
     public static string THREAD_GAUGE_REFERENCE_WITH_INDICATOR = "365.QM.C.";
    public static string TORQUE_WRENCH_NOBAR_REFERENCE_WITH_INDICATOR = "365.QM.C.812";
    public static string TORQUE_WRENCH_AWS_REFERENCE_WITH_INDICATOR = "3365.QM.C.075";
    public static string THREAD_RING_GAUGE_REFERENCE_WITH_INDICATOR = "365.QM.C.150";
    public static string THREAD_PLUG_GAUGE_REFERENCE_WITH_INDICATOR = "365.QM.C.149";
    public static string DECISION_RULE_CONFIRMITY = "Statement of confirmity is given based on decision rule {0} agreed by requestor.";

    public static string PDF_CERTIFICATE_RESULTS = "PDF_Certificate_Results";
    public static string PDF_CERTIFICATE_REMARKS = "PDF_Certificate_Remarks";
    public static string PDF_CERTIFICATE_UNCERTAINTY = "PDF_Certificate_Uncertainty";

	public static string INSCONTROLLERNAME = "Home/ControlCard?instrumentId=";
   
} 