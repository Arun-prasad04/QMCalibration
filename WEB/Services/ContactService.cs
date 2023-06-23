using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using WEB.Models;
using AutoMapper;
namespace WEB.Services;


public class ContactService : IContactService
{

	private readonly IMapper _mapper;
	private IUnitOfWork _unitOfWork { get; set; }
	public ContactService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}
	public ResponseViewModel<ContactViewModel> GetAllContactList()
	{
		try
		{
			List<ContactViewModel> contactList = _mapper.Map<List<ContactViewModel>>(_unitOfWork.Repository<Contact>().GetQueryAsNoTracking().ToList());
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = null,
				ResponseDataList = contactList
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ContactService - GetAllContactList Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ContactService",
				ResponseServiceMethod = "GetAllContactList"
			};
		}
	}
	public ResponseViewModel<ContactViewModel> GetContactById(int contactId)
	{
		try
		{
			ContactViewModel contactById = _mapper.Map<ContactViewModel>(_unitOfWork.Repository<Contact>().GetQueryAsNoTracking(Q => Q.Id == contactId).SingleOrDefault());
			List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
			contactById.DepartmentList = departmentList;

			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = contactById,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ContactService - GetContactById Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ContactService",
				ResponseServiceMethod = "GetContactById"
			};
		}

	}
	public ResponseViewModel<ContactViewModel> InsertContact(ContactViewModel contact)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			_unitOfWork.Repository<Contact>().Insert(_mapper.Map<Contact>(contact));
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<ContactViewModel>
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
			ErrorViewModelTest.Log("ContactService - InsertContact Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ContactService",
				ResponseServiceMethod = "InsertContact"
			};

		}
	}
	public ResponseViewModel<ContactViewModel> UpdateContact(ContactViewModel contact)
	{

		try
		{

			Contact contactById = _unitOfWork.Repository<Contact>().GetQueryAsNoTracking(Q => Q.Id == contact.Id).SingleOrDefault();
			contactById.Name = contact.Name;
			contactById.DepartmentId = contact.DepartmentId;
			contactById.Email = contact.Email;
			contactById.MobileNo = contact.MobileNo;
			contactById.ModifiedBy = 1;
			contactById.ModifiedOn = DateTime.Now;
			_unitOfWork.BeginTransaction();
			_unitOfWork.Repository<Contact>().Update(contactById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();

			return new ResponseViewModel<ContactViewModel>
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
			ErrorViewModelTest.Log("ContactService - UpdateContact Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ContactService",
				ResponseServiceMethod = "UpdateContact"
			};
		}
	}
	public ResponseViewModel<ContactViewModel> DeleteContact(int contactId)
	{
		try
		{
			_unitOfWork.BeginTransaction();
			Contact contactById = _unitOfWork.Repository<Contact>().GetQueryAsNoTracking(Q => Q.Id == contactId).SingleOrDefault();
			_unitOfWork.Repository<Contact>().Delete(contactById);
			_unitOfWork.SaveChanges();
			_unitOfWork.Commit();
			return new ResponseViewModel<ContactViewModel>
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
			ErrorViewModelTest.Log("ContactService - DeleteContact Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ContactService",
				ResponseServiceMethod = "DeleteContact"
			};
		}
	}
	public ResponseViewModel<ContactViewModel> CreateNewContact()
	{
		try
		{
			List<DepartmentViewModel> departmentList = _mapper.Map<List<DepartmentViewModel>>(_unitOfWork.Repository<Department>().GetQueryAsNoTracking().ToList());
			ContactViewModel contactEmptyViewModel = new ContactViewModel();
			contactEmptyViewModel.DepartmentList = departmentList;

			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 200,
				ResponseMessage = "Success",
				ResponseData = contactEmptyViewModel,
				ResponseDataList = null
			};
		}
		catch (Exception e)
		{
			ErrorViewModelTest.Log("ContactService - CreateNewContact Method");
			ErrorViewModelTest.Log("exception - " + e.Message);
			return new ResponseViewModel<ContactViewModel>
			{
				ResponseCode = 500,
				ResponseMessage = "Failure",
				ErrorMessage = e.Message,
				ResponseData = null,
				ResponseDataList = null,
				ResponseService = "ContactService",
				ResponseServiceMethod = "CreateNewContact"
			};
		}

	}

}
