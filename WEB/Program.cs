using CMT.DAL;
using CMT.DATAMODELS;
using Microsoft.EntityFrameworkCore;
using WEB.Services.Interface;
using WEB.Services;
using WEB.Mapping;
using WEB.Models;
using AutoMapper;
using WEB.Service;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMvc().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
var mapperConfig = new MapperConfiguration(mc =>
     {
         mc.AddProfile(new MappingProfile());
     });

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


string connString = builder.Configuration.GetSection("ConnectionStrings").GetSection("CMTDatabase").Value;
//string  connString ="Data Source=HP\\SA;Initial Catalog=CMT;Integrated Security=True";

builder.Services.AddDbContext<CMTDatabaseContext>(options =>
{
  options.UseSqlServer(connString, providerOptions =>
  {
	  providerOptions.CommandTimeout(700);  //Timeout in seconds
  });

}, ServiceLifetime.Scoped);
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IMasterService, MasterService>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IExternalRequestService, ExternalRequestService>();
builder.Services.AddScoped<IUtilityService, UtilityService>();
builder.Services.AddScoped<IFeedbackDataService, FeedbackDataService>();
builder.Services.AddScoped<IFeedbackInviteService, FeedbackInviteService>();
builder.Services.AddScoped<IQCAlternateMethodTemplateService, QCAlternateMethodTemplateService>();
builder.Services.AddScoped<IQCIntermediateTemplateService, QCIntermediateTemplateService>();
builder.Services.AddScoped<IObservationTemplateService, ObservationTemplateService>();
builder.Services.AddScoped<IQCReplicateTestTemplateService, QCReplicateTestTemplateService>();
builder.Services.AddScoped<IQCReTestTemplateService, QCReTestTemplateService>();
builder.Services.AddScoped<IReportAndChartService, ReportAndChartService>();
builder.Services.AddScoped<IMasterHistoryService, MasterHistoryService>();
builder.Services.AddScoped<IQRCodeGeneratorService, QRCodeGeneratorService>();
//builder.Services.AddHostedService<TimedHostedService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSession(options =>
{

    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
});
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.Configure<QRCodeSettings>(builder.Configuration.GetSection("QRCodeSettings"));
builder.Services.AddScoped<IQRCodeGeneratorService, QRCodeGeneratorService>();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(name: "ViewPdfFiles",
                pattern: "certification/ViewPdfFiles/{guid}",
                defaults: new { controller = "Certification", action = "ViewPdfFiles" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
