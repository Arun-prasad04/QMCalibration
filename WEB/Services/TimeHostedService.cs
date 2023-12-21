using CMT.DATAMODELS;
using WEB.Models;
using WEB.Services.Interface;
using System.Net;
using System.Net.Mail;
public class TimedHostedService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<TimedHostedService> _logger;
    private Timer _timer = null!;
    private IInstrumentService _instrumentService;
    private IUserService _userService;
    private IEmailService _emailService;

    private IConfiguration _configuration;

    public TimedHostedService(ILogger<TimedHostedService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
    {
        _logger = logger;
        _instrumentService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IInstrumentService>();

        _userService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IUserService>();
        _emailService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IEmailService>();
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromHours(1));
            //TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }
    

    private void DontWork(object? state)
    {
        _logger.LogInformation("Timed Hosted Service running, but not reached execution time");
    }

    private void DoWork(object? state)
    {
        var executionHour = _configuration["ServiceExecution"];
        if (DateTime.Now.Hour.ToString().Equals(executionHour))
        {
            int daysInCurrentMonth = System.DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            int twodaysBeforeDate = daysInCurrentMonth - 2;
            List<InstrumentViewModel> instrumentList = new List<InstrumentViewModel>();
            ResponseViewModel<InstrumentViewModel> response = _instrumentService.GetCurrentMonthDueList();
            ResponseViewModel<UserViewModel> responseUser = _userService.GetAllUserList();
            List<UserViewModel> userList = responseUser.ResponseDataList;
            instrumentList = response.ResponseDataList;

            foreach (var item in instrumentList)
            {
                UserViewModel userdate = userList.Where(W => W.Id == item.CreatedBy).SingleOrDefault();
                List<string> emailList = new List<string>();
                if (userdate != null)
                {
                    if (daysInCurrentMonth == (int)DateTime.Now.DayOfWeek)
                    {
                        emailList.Add(userdate.Email);
                        emailList.Add(userdate.ForemanName);
                        emailList.Add(userdate.AsstForemanEmail);
                    }
                    else if (twodaysBeforeDate == 29)
                    {
                        emailList.Add(userdate.Email);
                        emailList.Add(userdate.ForemanName);
                        emailList.Add(userdate.AsstForemanEmail);
                        emailList.AddRange(userList.Where(W => W.UserRoleId == 2 && W.UserRoleId == 4).Select(s => s.Email).ToList());
                    }
                    string duedate = Convert.ToDateTime(item.DueDate).ToShortDateString();
                    string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p> Your Instrument due on this month on <b>$DUEDATE$</b>.</p>    <p><b>Instrument Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>                       <tr>                <td align='left'>                    Instrument Name                </td><td>:</td>                <td>                    $INSTRUMENTNAME$                </td>            </tr>     <tr>                <td align='left'>                    Instrument ID.No                </td>     <td>:</td>           <td>                    $INSTRUMENTID$                </td>            </tr>    <tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qf042.in365.corpintra.net/DTAQMPortalUAT/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
                    mailbody = mailbody.Replace("$NAME$", userdate.FirstName + " " + userdate.LastName).Replace("$DUEDATE$", duedate).Replace("$INSTRUMENTNAME$", item.InstrumentName).Replace("$INSTRUMENTID$", item.IdNo).Replace("$DATE$", Convert.ToDateTime(item.DueDate).ToShortDateString());

                    // EmailViewModel emailViewModel = new EmailViewModel()
                    // {
                    //     ToList = emailList,
                    //     Subject = "Instrument Due -" + item.InstrumentName + "",
                    //     Body = mailbody,
                    //     IsHtml = true
                    // };
        for(var i=0;i<emailList.Count;i++)
        {
            if(!string.IsNullOrEmpty(emailList[i]))
            {                           
                try {  
                    MailMessage message = new MailMessage();  
                    SmtpClient smtp = new SmtpClient();  
                    message.From = new MailAddress("DICV-CMT@DAIMLER.COM");  
                    //message.To.Add("gurushev.p@daimlertruck.com");
                    //message.Bcc.Add("mohammedashik.s@intelizign.com");
                    message.Subject = "Instrument Due -" + item.InstrumentName + "";  
                    message.IsBodyHtml = true; //to make message body as html  
                    message.Body = mailbody;  
                    smtp.Port = 25;  
                    smtp.Host = "53.151.100.102"; //for gmail host  
                    smtp.EnableSsl = false;  
                    smtp.UseDefaultCredentials = false;  
                    smtp.Credentials = new NetworkCredential("DICV-EBOM@DAIMLER.COM", "Dicv@123");  
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;  
                    smtp.Send(message);  
                } catch (Exception) {}
            }
        }



                   // _emailService.SendEmailAsync(emailViewModel, true);
                }
            }
        }
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation(
            "Timed Hosted Service is working. Count: {Count}", count);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}