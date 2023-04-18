using CMT.DATAMODELS;
using WEB.Models;
namespace WEB.Services.Interface;
public interface IUserService
{
    ResponseViewModel<UserViewModel> GetAllUserList();
    ResponseViewModel<UserViewModel> GetUserById(int UserId);
    ResponseViewModel<UserViewModel> InsertUser(UserViewModel User);
    ResponseViewModel<UserViewModel> UpdateUser(UserViewModel User);
    ResponseViewModel<UserViewModel> PasswordUpdate(UserViewModel User);
    ResponseViewModel<string> DeleteUser(int userId);
    ResponseViewModel<UserViewModel> ValidateUser(string UserName, string Password, string email);
    ResponseViewModel<UserViewModel> CreateNewUser();
    ResponseViewModel<UserViewModel> ActivateUser(ActivationUserViewModel userActivation);

    ResponseViewModel<UserViewModel> ForgotUserPassword(string userEmail);
    ResponseViewModel<string> CheckEmailAddress(string userEmail);
    ResponseViewModel<UserViewModel> AssignUser(UserViewModel User);
}