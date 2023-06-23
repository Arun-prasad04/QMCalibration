using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using System.Net;
using System.Net.Mail;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;

public class FeedbackInviteService : IFeedbackInviteService
{
	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }

	private IConfiguration _configuration;
	private IEmailService _emailService;
	IUtilityService _utilityService;
	public FeedbackInviteService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IUtilityService utilityService, IConfiguration Configuration)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_emailService = emailService;
		_utilityService = utilityService;
		_configuration = Configuration;
	}
	public ResponseViewModel<FeedbackInviteViewModel> InsertFeedbackInvite(List<FeedbackInviteViewModel> feedbackInviteList)
	{
		try
		{
			var labData = _unitOfWork.Repository<Lovs>()
									.GetQueryAsNoTracking(Q => Q.Attrform == "Master"
																&& Q.AttrName == "Lab"
																&& Q.AttrValue == feedbackInviteList.First().LabName)
									.ToList();
			if (!labData.Any())
			{
				return new ResponseViewModel<FeedbackInviteViewModel>
				{
					ResponseCode = 500,
					ResponseMessage = "Failure",
					ErrorMessage = "Lab data is not available",
					ResponseData = null,
					ResponseDataList = null,
					ResponseService = "FeedbackInvite",
					ResponseServiceMethod = "InsertFeedbackInvite"
				};
			}
			int labId = labData.First().Id;
			feedbackInviteList.ForEach(x => x.LabId = labId);

			var inviteMonth = feedbackInviteList.First().InvitedOn.Month;

			//Get Existing Feedback Invite users list
			var existingInviteUsersList = GetFeedbackInviteUsers(inviteMonth);

			var newInviteDataList = feedbackInviteList.Where(x => existingInviteUsersList.All(y => y.UserId != x.UserId))
														   .ToList();

			string responseMessage = "Successfully sent invite...";
			if (newInviteDataList.Count() > 0)
			{
				//Insert into FeedbackInvite table       
				_unitOfWork.BeginTransaction();
				_unitOfWork.Repository<FeedbackInvite>().InsertRange(_mapper.Map<FeedbackInvite[]>(newInviteDataList));
				_unitOfWork.SaveChanges();
				_unitOfWork.Commit();

				//Send To Email
				List<string> emailList = new List<string>();
				foreach (var user in feedbackInviteList)
				{
					//emailList.Add(user.Email.Trim());
					string mailbody = "<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 14pt; font-weight: Normal;'>        <p>            Dear Customer,</p>        <p>            DICV ($LABNAME$) team requested feedback from you about services. Your support in this regard much appreciated.</p>      <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p>     <p>                <b> Regards</b>,                <br />                <b>$LABNAME$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
					mailbody = mailbody.Replace("$LABNAME$", user.LabName);
					MailMessage message = new MailMessage();
					SmtpClient smtp = new SmtpClient();
					SmtpSettings smtpvalue = new SmtpSettings();
					message.From = new MailAddress(smtpvalue.FromAddress);
					message.To.Add(new MailAddress(user.Email.Trim()));
					//message.To.Add("gurushev.p@daimlertruck.com");
					//message.Bcc.Add("mohammedashik.s@intelizign.com");  
					message.Subject = "Customer Feedback request - " + user.LabName + "";
					message.IsBodyHtml = true; //to make message body as html  
					message.Body = mailbody;
					smtp.Port = int.Parse(smtpvalue.Port);
					smtp.Host = smtpvalue.Server; //for gmail host  
					smtp.EnableSsl = false;
					smtp.UseDefaultCredentials = false;
					smtp.Credentials = new NetworkCredential(smtpvalue.UserId, smtpvalue.Pwd);
					smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
					smtp.Send(message);
				}

				// EmailViewModel emailViewModel = new EmailViewModel()
				// {
				//     ToList = emailList,
				//     Subject = Constants.FeedbackForm_Subject,
				//     Body = Constants.FeedbackForm_Body+" </br> <a href='"+_configuration["AppUrl"]+"'>"+_configuration["AppUrl"]+"</a></br>",
				//     IsHtml = true
				// };

				// _emailService.SendEmailAsync(emailViewModel, true);
			}
			else
			{
				responseMessage = "Already user(s) has invited..!";
			}


			return new ResponseViewModel<FeedbackInviteViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = responseMessage,
				ResponseData = null,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("FeedbackInviteService - InsertFeedbackInvite Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<FeedbackInviteViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "User",
				ResponseServiceMethod = "InsertFeedbackInvite"
			};
		}
	}
	public class SmtpSettings
	{
		public string Server = "53.151.100.102";
		public string Port = "25";
		public string FromAddress = "DICV-CAL@DAIMLER.COM";
		public string UserId = "DICV-EBOM@DAIMLER.COM";
		public string Pwd = "Dicv@123";
		public bool IsDevelopmentMode = true;
	}
	public ResponseViewModel<FeedbackInviteViewModel> GetFeedbackInviteList(int feedbackStatus)
	{
		try
		{
			List<FeedbackInviteViewModel> feedbacklist = _unitOfWork.Repository<FeedbackData>()
			.GetQueryAsNoTracking(Q => Q.FeedbackStatus == feedbackStatus)
			.Include(I => I.FeedbackInviteModel.UserModel)
			.Include(I => I.FeedbackInviteModel)
			.Select(s => new FeedbackInviteViewModel()
			{
				Id = s.FeedbackInviteId,
				FirstName = s.FeedbackInviteModel.UserModel.FirstName,
				LastName = s.FeedbackInviteModel.UserModel.LastName,
				Email = s.FeedbackInviteModel.UserModel.Email,
				MobileNo = s.FeedbackInviteModel.UserModel.MobileNo,
				ShortId = s.FeedbackInviteModel.UserModel.ShortId,
				DepartmentName = s.FeedbackInviteModel.UserModel.Department.Name,
				InvitedOn = s.FeedbackInviteModel.InvitedOn,
				InvitedBy = s.FeedbackInviteModel.InvitedBy,
				InvitedByName = s.FeedbackInviteModel.UserModel.FirstName + " " + s.FeedbackInviteModel.UserModel.LastName,
				Comments = s.FeedbackInviteModel.Comments
			}).OrderByDescending(x => x.InvitedOn)
			  .ToList();

			return new ResponseViewModel<FeedbackInviteViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = feedbacklist
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("FeedbackInviteService - GetFeedbackInviteList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<FeedbackInviteViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "FeedbackInviteService",
				ResponseServiceMethod = "GetFeedbackInviteList"
			};
		}

	}

	public ResponseViewModel<FeedbackInviteViewModel> GetFeedbackInviteUserData(int userId)
	{
		try
		{
			FeedbackInviteViewModel feedbackInviteViewModel = _unitOfWork.Repository<FeedbackInvite>()
		   .GetQueryAsNoTracking(Q => Q.UserId == userId)
		   .OrderByDescending(x => x.InvitedOn)
		   .Include(I => I.UserModel)
		   .Include(I => I.InviteUserModel)
		   .Select(s => new FeedbackInviteViewModel()
		   {
			   Id = s.Id,
			   FirstName = s.UserModel.FirstName,
			   LastName = s.UserModel.LastName,
			   Email = s.UserModel.Email,
			   MobileNo = s.UserModel.MobileNo,
			   ShortId = s.UserModel.ShortId,
			   DepartmentName = s.UserModel.Department.Name,
			   InvitedOn = s.InvitedOn,
			   InvitedBy = s.InvitedBy,
			   InvitedByName = string.Concat(s.InviteUserModel.FirstName.Trim(), " ", s.InviteUserModel.LastName.Trim()),
			   LabId = s.LabId,
			   Comments = s.Comments
		   }).OrderByDescending(x => x.InvitedOn)
			 .First();

			List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork
													   .Repository<Lovs>()
													   .GetQueryAsNoTracking(Q => Q.Attrform == "Master")
													   .ToList());

			feedbackInviteViewModel.LabName = lovsList.Where(x => x.AttrName.ToUpper().Equals("Lab", StringComparison.OrdinalIgnoreCase) && x.Id == feedbackInviteViewModel.LabId)
												.First().AttrValue;

			return new ResponseViewModel<FeedbackInviteViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = feedbackInviteViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("FeedbackInviteService - GetFeedbackInviteUserData Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<FeedbackInviteViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "FeedbackInviteService",
				ResponseServiceMethod = "GetFeedbackInviteUserData"
			};
		}
	}
	public List<FeedbackInvite> GetFeedbackInviteUsers(int inviteMonth)
	{
		var inviteUserList = _unitOfWork.Repository<FeedbackInvite>()
							  .GetQueryAsNoTracking(x => x.InvitedOn.Month == inviteMonth)
							  .ToList();

		return inviteUserList;
	}
}
