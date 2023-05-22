using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace WEB.Models
{
    public class ErrorViewModelTest
    {

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public ErrorViewModelTest(string message)
        {
            var path = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["AErrorlog"];
            string sTemp = path + "_" + DateTime.Now.ToString("dd_MM") + ".txt";
            FileStream Fs = new FileStream(sTemp, FileMode.OpenOrCreate | FileMode.Append);
            StreamWriter st = new StreamWriter(Fs);
            string dttemp = DateTime.Now.ToString("[dd:MM:yyyy] [HH:mm:ss:ffff]");
            st.WriteLine(dttemp + "\t" + message);
            st.Close();
        }

        public static void Log(string message)
        {
			ErrorViewModelTest currenterror = new ErrorViewModelTest(message);
        }
    }



}
