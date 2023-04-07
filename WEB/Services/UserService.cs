using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;


public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork { get; set; }
    private IConfiguration _configuration;
    private IEmailService _emailService;
    IUtilityService _utilityService;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailService emailService, IUtilityService utilityService, IConfiguration Configuration)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _emailService = emailService;
        _utilityService = utilityService;
        _configuration = Configuration;
    }
    public ResponseViewModel<UserViewModel> GetAllUserList()
    {
        try
        {
            List<UserViewModel> userViewModelList = _unitOfWork.Repository<User>().GetQueryAsNoTracking(s => s.ActiveStatus == true).Include(I => I.Department).Select(S => new UserViewModel()
            {
                Id = S.Id,
                DepartmentName = S.Department.Name,
                FirstName = S.FirstName,
                LastName = S.LastName,
                ShortId = S.ShortId,
                Email = S.Email,
                MobileNo = S.MobileNo,
                ActiveStatus = S.ActiveStatus,
                ActivationGuid = S.ActivationGuid,
                CreatedBy = S.CreatedBy,
                ModifiedBy = S.ModifiedBy,
                CreatedOn = S.CreatedOn,
                ModifiedOn = S.ModifiedOn,
                UserRoleId = S.UserRoleId,
                Password = S.Password,
                L3Email = S.L3Email,
                L4Email = S.L4Email,
                L5Email = S.L5Email,
                L6Email = S.L6Email,
                DepartmentId = S.DepartmentId
            }).ToList();

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = userViewModelList
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "GetAllUserList"
            };
        }
    }
    public ResponseViewModel<UserViewModel> GetUserById(int UserId)
    {
        try
        {
            UserViewModel userById = _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == UserId).SingleOrDefault());
            List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
            List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "Designation").ToList());
            userById.DepartmentList = departmentList;
            userById.DesignationList = lovsList;

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = userById,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "GetUserByID"
            };
        }

    }

    public ResponseViewModel<string> CheckEmailAddress(string userEmail)
    {
        try
        {
            var EmailMessage = "";
            User Email = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Email == userEmail).SingleOrDefault();
            if (Email != null)
            {
                EmailMessage = "True";
            }
            else
            {
                EmailMessage = "False";
            }
            return new ResponseViewModel<string>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = EmailMessage,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<string>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "GetUserByID"
            };
        }

    }


    public ResponseViewModel<UserViewModel> InsertUser(UserViewModel User)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            // User.Password=_utilityService.Encrypt(User.Password);
            if (User.ImageUpload != null)
            {
                string filePath = _utilityService.UploadImage(User.ImageUpload, Constants.Signature_FolderName);
                IFormFile fileobj = User.ImageUpload;
                User.SignImageName = fileobj.FileName;
            }

            _unitOfWork.Repository<User>().Insert(_mapper.Map<User>(User));
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            List<string> emailList = new List<string>();
            emailList.Add(User.Email);
            EmailViewModel emailViewModel = new EmailViewModel()
            {
                ToList = emailList,
                Subject = "Calibration Management Tool User Activation",
                Body = "Welcome " + User.FirstName + " " + User.LastName + ",<br/> You are successfully added to CMT application as Department User. Please Click the below link and setup your account password to continue you CMT account. </br> <a href='http://s365id1qdg044/cmtlive/Authentication/AuthenticateUser?activationKey=" + User.ActivationGuid + "'>" + "CMT LIVE" + "</a></br> Note: Please contact Administarator if you are facing trouble to activate your account.",
                IsHtml = true
            };
            _emailService.SendEmailAsync(emailViewModel, true);
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "InsertUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> UpdateUser(UserViewModel user)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == user.Id).SingleOrDefault();
            if (user.FirstName != null)
            {
                userById.FirstName = user.FirstName;
            }
            if (user.LastName != null)
            {
                userById.LastName = user.LastName;
            }
            if (user.Email != null && user.Email != "")
            {
                userById.Email = user.Email;
            }
            if (user.MobileNo != null && user.MobileNo != "")
            {
                userById.MobileNo = user.MobileNo;
            }
            if (user.DepartmentId != null && user.DepartmentId > 0)
            {
                userById.DepartmentId = user.DepartmentId;
            }
            if (user.ShortId != null)
            {
                userById.ShortId = user.ShortId;
            }
            if (user.UserRoleId != null)
            {
                userById.UserRoleId = user.UserRoleId;
            }
            if (user.Designation != null)
            {
                userById.Designation = user.Designation;
            }
            if (user.Level != null)
            {
                userById.Level = user.Level;
            }
            if (user.L3ShortId != null)
            {
                userById.L3ShortId = user.L3ShortId;
            }
            if (user.L3Name != null)
            {
                userById.L3Name = user.L3Name;
            }
            if (user.L3Email != null)
            {
                userById.L3Email = user.L3Email;
            }
            if (user.L4ShortId != null)
            {
                userById.L4ShortId = user.L4ShortId;
            }
            if (user.L4Name != null)
            {
                userById.L4Name = user.L4Name;
            }
            if (user.L4Email != null)
            {
                userById.L4Email = user.L4Email;
            }
            if (user.L5ShortId != null)
            {
                userById.L5ShortId = user.L5ShortId;
            }
            if (user.L5Name != null)
            {
                userById.L5Name = user.L5Name;
            }
            if (user.L5Email != null)
            {
                userById.L5Email = user.L5Email;
            }
            if (user.L6ShortId != null)
            {
                userById.L6ShortId = user.L6ShortId;
            }
            if (user.L6Name != null)
            {
                userById.L6Name = user.L6Name;
            }
            if (user.L6Email != null)
            {
                userById.L6Email = user.L6Email;
            }

            if (user.ImageUpload != null)
            {
                string filePath = _utilityService.UploadImage(user.ImageUpload, Constants.Signature_FolderName);
                IFormFile fileobj = user.ImageUpload;
                userById.SignImageName = fileobj.FileName;
            }


            _unitOfWork.Repository<User>().Update(userById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "UpdateUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> AssignUser(UserViewModel user)
    {
        try
        {
            List<int> RequestIdValue = new List<int>();
            _unitOfWork.BeginTransaction();
            //List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == user.DeletingID).ToList();
            List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking().ToList();
            foreach (var item in requestUser)
            {
                if (item.StatusId == 30 || item.StatusId == 28)
                {
                    RequestIdValue.Add(item.RequestId);
                    //requestUser.RemoveRange(requestUser.Where(x => x.Id == item.Id));
                    //requestUser.RemoveAll(requestUser.Where(x => x.Id == item.Id));
                    //requestUser.Remove(requestUser.Where(x => x.Id==item.Id));
                }
            }
            // List<RequestStatus> RValue = _unitOfWork.Repository<RequestStatus>()
            //                                         .GetQueryAsNoTracking(j => j.CreatedBy == user.DeletingID && !RequestIdValue
            //                                                                     .Contains(j.RequestId)).ToList();

            List<RequestStatus> RValue = requestUser.Where(j => j.CreatedBy == user.DeletingID && !RequestIdValue
                                                                                     .Contains(j.RequestId)).ToList();

            for (int i = 0; i < RValue.Count; i++)
            {

                if (RValue[i].StatusId == 27 || RValue[i].StatusId == 29)
                {
                    RValue[i].CreatedBy = user.Id;
                    _unitOfWork.Repository<RequestStatus>().Update(RValue[i]);
                    _unitOfWork.SaveChanges();

                }

            }
            _unitOfWork.Commit();
            var data = 1;

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "UpdateUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> PasswordUpdate(UserViewModel user)
    {
        try
        {
            user.Password = _utilityService.Encrypt(user.Password);
            _unitOfWork.BeginTransaction();
            User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ActivationGuid == user.ActivationGuid).SingleOrDefault();
            if (user.Password != null)
            {
                userById.Password = user.Password;
            }

            _unitOfWork.Repository<User>().Update(userById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "UpdateUser"
            };
        }
    }
    public ResponseViewModel<string> DeleteUser(int userId)
    {
        try
        {
            var result = "";
            int a = 0;
            User userById1 = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
            if (userById1.UserRoleId == 2)
            {
                List<int> RequestIdValue = new List<int>();
                List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking().ToList();
                foreach (var item in requestUser)
                {
                    if (item.StatusId == 30 || item.StatusId == 28)
                    {
                        RequestIdValue.Add(item.RequestId);
                    }
                }
                List<RequestStatus> RValue = requestUser.Where(j => j.CreatedBy == userId && !RequestIdValue
                                                                                         .Contains(j.RequestId)).ToList();
                if (RValue.Count > 0)
                {
                    result = "0";
                }
                else
                {
                    userById1.ActiveStatus = false;
                    _unitOfWork.BeginTransaction();
                    _unitOfWork.Repository<User>().Update(userById1);
                    _unitOfWork.SaveChanges();
                    _unitOfWork.Commit();
                    result = "1";
                    a = a + 200;
                }
            }
            else if (userById1.UserRoleId == 3)
            {
                List<int> ExternalRequestIdValue = new List<int>();
                List<ExternalRequestStatus> ExternalRequestUser = _unitOfWork.Repository<ExternalRequestStatus>().GetQueryAsNoTracking().ToList();
                foreach (var item in ExternalRequestUser)
                {
                    if (item.StatusId == 30 || item.StatusId == 28)
                    {
                        ExternalRequestIdValue.Add(item.ExternalRequestId);
                    }
                }
                List<ExternalRequestStatus> ERValue = ExternalRequestUser.Where(j => j.CreatedBy == userId && !ExternalRequestIdValue
                                                                                         .Contains(j.ExternalRequestId)).ToList();
                if (ERValue.Count > 0)
                {
                    result = "0";
                }
                else
                {
                    userById1.ActiveStatus = false;
                    _unitOfWork.BeginTransaction();
                    _unitOfWork.Repository<User>().Update(userById1);
                    _unitOfWork.SaveChanges();
                    _unitOfWork.Commit();
                    result = "1";
                    a = a + 200;
                }

            }

            else
            {
                userById1.ActiveStatus = false;
                _unitOfWork.BeginTransaction();
                _unitOfWork.Repository<User>().Update(userById1);
                _unitOfWork.SaveChanges();
                _unitOfWork.Commit();
                result = "1";
                a += 200;
            }
            return new ResponseViewModel<string>
            {
                ResponseCode = a,
                ResponseMessage = "Success",
                ResponseData = result,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            return new ResponseViewModel<string>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "DeleteUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> ValidateUser(string UserName, string Password)
    {
        try
        {
            //string pass =_utilityService.Encrypt(Password);
            UserViewModel validateUser = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q =>
            (Q.ShortId == UserName || Q.Email == UserName)).Include(I => I.Department).Select(S => new UserViewModel()
            {
                DepartmentName = S.Department.Name,
                FirstName = S.FirstName,
                LastName = S.LastName,
                ShortId = S.ShortId,
                Email = S.Email,
                MobileNo = S.MobileNo,
                ActiveStatus = S.ActiveStatus,
                ActivationGuid = S.ActivationGuid,
                CreatedBy = S.CreatedBy,
                ModifiedBy = S.ModifiedBy,
                CreatedOn = S.CreatedOn,
                ModifiedOn = S.ModifiedOn,
                UserRoleId = S.UserRoleId,
                Password = S.Password,
                L3Email = S.L3Email,
                L4Email = S.L4Email,
                L5Email = S.L5Email,
                L6Email = S.L6Email,
                DepartmentId = S.DepartmentId,
                Id = S.Id
            }).SingleOrDefault();
            if (validateUser == null)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    ResponseCode = 500,
                    ResponseMessage = "Invalid UserName",
                    ResponseData = null,
                    ResponseDataList = null
                };

            }
            string decryptUserPass = _utilityService.Decrypt(validateUser.Password);
            if (Password == decryptUserPass)
            {
                return new ResponseViewModel<UserViewModel>
                {
                    ResponseCode = 200,
                    ResponseMessage = "Success",
                    ResponseData = validateUser,
                    ResponseDataList = null
                };
            }
            else
            {
                return new ResponseViewModel<UserViewModel>
                {
                    ResponseCode = 500,
                    ResponseMessage = "Invalid Password",
                    ResponseData = null,
                    ResponseDataList = null
                };
            }
        }
        catch (Exception e)
        {
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "ValidateUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> CreateNewUser()
    {
        try
        {
            List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
            List<LovsViewModel> lovsList = _mapper.Map<List<LovsViewModel>>(_unitOfWork.Repository<Lovs>().GetQueryAsNoTracking(Q => Q.AttrName == "Designation").ToList());
            UserViewModel userEmptyViewModel = new UserViewModel();
            userEmptyViewModel.DepartmentList = departmentList;
            userEmptyViewModel.DesignationList = lovsList;

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = userEmptyViewModel,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "CreateNewUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> ActivateUser(ActivationUserViewModel userActivation)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.ActivationGuid == userActivation.AuthKey).SingleOrDefault();
            userById.Password = _utilityService.Encrypt(userActivation.Password);
            _unitOfWork.Repository<User>().Update(userById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            List<string> emailList = new List<string>();
            emailList.Add(userById.Email);
            EmailViewModel emailViewModel = new EmailViewModel()
            {
                ToList = emailList,
                Subject = "Calibration Management Tool User Activation Successful",
                Body = "Hi " + userById.FirstName + " " + userById.LastName + ",<br/> Your CMT application activated successfully!. Please Click the below link and started doing your activities with CMT. </br> <a href='"  + _configuration["AppUrl"] + "http://s365id1qdg044/cmtlive/Account/Login'" + "'>" + "CMT LIVE" + "</a></br> </br> Note: Please contact Administarator if you are facing trouble to login your account.",  
                IsHtml = true
            };
            _emailService.SendEmailAsync(emailViewModel, true);

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            _unitOfWork.RollBack();
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "UpdateUser"
            };
        }
    }
    public ResponseViewModel<UserViewModel> ForgotUserPassword(string userEmail)
    {
        try
        {
            User userById = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Email == userEmail).SingleOrDefault();
            List<string> emailList = new List<string>();
            //emailList.Add("srinidhi.s@intelizign.com");
            emailList.Add(userById.Email);

            // System.Random random = new System.Random();
            // var OTP = random.Next(100000, 999999);

            EmailViewModel emailViewModel = new EmailViewModel()
            {
                ToList = emailList,
                Subject = "Calibration Management Reset Password",
                //Body = "Hi " + "Srinidhi" + " " + "S" + ",<br/> Please Click the below link and reset your account with CMT. </br> " + _configuration["AppUrl"] + "Activation/ActivateUser?userid=" + "Email" + "</br> Note: Please contact Administarator if you are facing trouble to resetting your password.",
                Body = "Hi " + userById.FirstName + " " + userById.LastName + ",<br/> Please Click the below link and reset your account with CMT. </br> " + _configuration["AppUrl"] + "/Account/UpdatePassword?userid=" + userById.ActivationGuid + "</br> Note: Please contact Administarator if you are facing trouble to resetting your password.",
                IsHtml = true
            };
            _emailService.SendEmailAsync(emailViewModel, true);

            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<UserViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "User",
                ResponseServiceMethod = "UpdateUser"
            };
        }
    }


}


//---------------------------user deleted commented code--------------------------------------------


// var result = "";
// int a = 0;
// User userById1 = _unitOfWork.Repository<User>().GetQueryAsNoTracking(Q => Q.Id == userId).SingleOrDefault();
// if (userById1.UserRoleId == 2)
// {
//     int StatusCode = 0;
//     //List<RequestStatus> requestUser = _mapper.Map<List<RequestStatus>>(_unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == userId).ToList());
//     //List<RequestStatus?> requestUser = _unitOfWork.Repository<RequestStatus?>().GetQueryAsNoTracking(j => j.CreatedBy == userId && (j.StatusId != 28 || j.StatusId != 30 || j.StatusId == 26)).ToList();
//     //List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == userId).Include(q => q.RequestModel.StatusId == 27).ToList();
//     List<RequestStatus> requestUser = _unitOfWork.Repository<RequestStatus>().GetQueryAsNoTracking(j => j.CreatedBy == userId).ToList();
//     foreach (var item in requestUser)
//     {
//         if ((item.StatusId != 28 || item.StatusId != 30) && (item.StatusId == 27 || item.StatusId == 29))
//         {
//             StatusCode += 1;
//         }
//     }
//     if (StatusCode != 0 || StatusCode != null)
//     {
//         result = "0";
//     }
//     else
//     {
//         userById1.ActiveStatus = false;
//         _unitOfWork.BeginTransaction();
//         _unitOfWork.Repository<User>().Update(userById1);
//         _unitOfWork.SaveChanges();
//         _unitOfWork.Commit();
//         result = "1";
//         a = a + 200;
//     }

// }
// else
// {
//     userById1.ActiveStatus = false;
//     _unitOfWork.BeginTransaction();
//     _unitOfWork.Repository<User>().Update(userById1);
//     _unitOfWork.SaveChanges();
//     _unitOfWork.Commit();
//     result = "1";
//     a += 200;
// }

// return new ResponseViewModel<string>
// {
//     ResponseCode = a,
//     ResponseMessage = "Success",
//     ResponseData = result,
//     ResponseDataList = null
// };