using CMT.DATAMODELS;
using WEB.Models;

namespace WEB.Services.Interface; 
public interface IContactService{
ResponseViewModel<ContactViewModel> GetAllContactList();
ResponseViewModel<ContactViewModel> GetContactById(int contactId);
ResponseViewModel<ContactViewModel> InsertContact(ContactViewModel contact);
ResponseViewModel<ContactViewModel> UpdateContact(ContactViewModel contact);
ResponseViewModel<ContactViewModel> DeleteContact(int contactId);
ResponseViewModel<ContactViewModel> CreateNewContact();
}