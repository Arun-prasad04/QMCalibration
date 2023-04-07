using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;

namespace WEB.Services;

public class QCReTestTemplateService : IQCReTestTemplateService
{
    private readonly IMapper _mapper;
    private IUnitOfWork _unitOfWork { get; set; }
    IUtilityService _utilityService;
    public QCReTestTemplateService(IUnitOfWork unitOfWork, IMapper mapper, IUtilityService utilityService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _utilityService = utilityService;
    }
    public ResponseViewModel<ReTestViewModel> GetByTemplateData(int ReTestId)
    {
        try
        {
            QCReTestTemplate qcReTestTemplateModel = _unitOfWork.Repository<QCReTestTemplate>().GetQueryAsNoTracking(x => x.Id == ReTestId).SingleOrDefault();
            ReTestViewModel qcReTestTemplateviewModel = _mapper.Map<ReTestViewModel>(qcReTestTemplateModel);

            if (qcReTestTemplateviewModel.ReviewedBy != null)
                qcReTestTemplateviewModel.ReviewedByName = GetUserName(qcReTestTemplateviewModel.ReviewedBy);

            QCReTestTemplateData data = _unitOfWork.Repository<QCReTestTemplateData>().GetQueryAsNoTracking(x => x.ParentId == ReTestId && x.SINo == 1).SingleOrDefault();
            ReTestDataViewModel ReTestTemplateDetailVMList = _mapper.Map<ReTestDataViewModel>(data);
            qcReTestTemplateviewModel.Obs1 = ReTestTemplateDetailVMList;

            if (qcReTestTemplateviewModel.Obs1 != null && qcReTestTemplateviewModel.Obs1.AppraiserName != null)
                qcReTestTemplateviewModel.Obs1.AppraiserFullName = GetUserName(qcReTestTemplateviewModel.Obs1.AppraiserName);

            data = _unitOfWork.Repository<QCReTestTemplateData>().GetQueryAsNoTracking(x => x.ParentId == ReTestId && x.SINo == 2)
                                                                 .SingleOrDefault();
            ReTestTemplateDetailVMList = _mapper.Map<ReTestDataViewModel>(data);
            qcReTestTemplateviewModel.Obs2 = ReTestTemplateDetailVMList;

            if ( qcReTestTemplateviewModel.Obs2 != null && qcReTestTemplateviewModel.Obs2.AppraiserName != null)
                qcReTestTemplateviewModel.Obs2.AppraiserFullName = GetUserName(qcReTestTemplateviewModel.Obs2.AppraiserName);


            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = qcReTestTemplateviewModel,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {

            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "QCReTestTemplateService",
                ResponseServiceMethod = "GetByTemplateData"
            };
        }

    }

    public ResponseViewModel<ReTestViewModel> GetReTestGridData()
    {
        try
        {
            var data = _unitOfWork.Repository<QCReTestTemplate>().GetAll()
                                                                 .Where(x=>x.DocumentStatus != (int)(ReplicateTestStatus.Rejected))
                                                                 .OrderByDescending(x => x.CreatedOn);

            var ReTestList = _mapper.Map<List<ReTestViewModel>>(data);
            foreach (var reTestData in ReTestList)
            {
                if (reTestData.DocumentStatus == (int)ReplicateTestStatus.ResultOneSubmitted)
                {
                    reTestData.DocumentStatusName = Constants.RESULT_ONE_SUBMITTED;
                }
                else if (reTestData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
                {
                    reTestData.DocumentStatusName = Constants.RESULT_TWO_SUBMITTED;
                }
                else if (reTestData.DocumentStatus == (int)ReplicateTestStatus.Approved)
                {
                    reTestData.DocumentStatusName = Constants.APPROVED;
                }
                else if (reTestData.DocumentStatus == (int)ReplicateTestStatus.Rejected)
                {
                    reTestData.DocumentStatusName = Constants.REJECTED;
                }
            }

            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = ReTestList
            };
        }
        catch (Exception e)
        {
            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseServiceMethod = "QCReTestTemplate",
                ResponseService = "GetReTestGridData"
            };
        }
    }
    public ResponseViewModel<ReTestViewModel> InsertData(ReTestViewModel qcReTestTemplateViewModel)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            QCReTestTemplate qcReTestTemplate = new QCReTestTemplate()
            {
                Id = qcReTestTemplateViewModel.Id,
                FormatNo = qcReTestTemplateViewModel.FormatNo,
                RevisionNo = qcReTestTemplateViewModel.RevisionNo,
                RevisionDate = qcReTestTemplateViewModel.RevisionDate,
                DateConducted = qcReTestTemplateViewModel.DateConducted,
                InstrumentId = 0,
                RangeOrSize = qcReTestTemplateViewModel.RangeOrSize,
                LC = qcReTestTemplateViewModel.LC,
                InstrumentName = qcReTestTemplateViewModel.InstrumentName,
                MasterEquipmentId = 0,
                MasterEquipmentName = qcReTestTemplateViewModel.MasterEquipmentName,
                Temperature = qcReTestTemplateViewModel.Temperature,
                Humidity = qcReTestTemplateViewModel.Humidity,
                EnValue = qcReTestTemplateViewModel.EnValue,
                Conclusion = qcReTestTemplateViewModel.Conclusion,
                ReviewedBy = qcReTestTemplateViewModel.ReviewedBy,
                ReviewedOn = qcReTestTemplateViewModel.ReviewedOn,
                DocumentStatus = Convert.ToInt16(qcReTestTemplateViewModel.DocumentStatus),
                CreatedBy = qcReTestTemplateViewModel.CreatedBy,
                CreatedOn = qcReTestTemplateViewModel.CreatedOn,
                ModifiedBy = qcReTestTemplateViewModel.ModifiedBy,
                ModifiedOn = qcReTestTemplateViewModel.ModifiedOn,
                DataUnit = qcReTestTemplateViewModel.DataUnit,
                MasterLabId = qcReTestTemplateViewModel.MasterLabId,
                InstrumentLabId =  qcReTestTemplateViewModel.InstrumentLabId
            };

            if (qcReTestTemplateViewModel.ImageUpload1 != null)
            {
                string filePath = _utilityService.UploadImage(qcReTestTemplateViewModel.ImageUpload1,
                                                              Constants.QCReeTest_FolderName);
                IFormFile fileobj1 = qcReTestTemplateViewModel.ImageUpload1;
                qcReTestTemplate.MUx1FileName = fileobj1.FileName;
            }

            _unitOfWork.Repository<QCReTestTemplate>().Insert(qcReTestTemplate);
            _unitOfWork.SaveChanges();

            qcReTestTemplateViewModel.Obs1.ParentId = qcReTestTemplate.Id;
            qcReTestTemplateViewModel.Obs1.SINo = 1;

            List<ReTestDataViewModel> qcReTestChildListData = new List<ReTestDataViewModel>();
            qcReTestChildListData.Add(qcReTestTemplateViewModel.Obs1);            
            _unitOfWork.Repository<QCReTestTemplateData>().InsertRange(_mapper.Map<QCReTestTemplateData[]>(qcReTestChildListData));
            _unitOfWork.SaveChanges();

            _unitOfWork.Commit();

            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 200,
                ResponseMessage = "Success",
                ResponseData = null,
                ResponseDataList = null
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = null,
                ResponseDataList = null,
                ResponseService = "QCReTestTemplateService",
                ResponseServiceMethod = "InsertData"
            };
        }

    }
    public ResponseViewModel<ReTestViewModel> UpdateData(ReTestViewModel qcReTestUpdateData)
    {
        try
        {
            QCReTestTemplate qcReTestTemplate = _unitOfWork.Repository<QCReTestTemplate>().GetQueryAsNoTracking(Q => Q.Id == qcReTestUpdateData.Id).SingleOrDefault();
            if (qcReTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
            {
                qcReTestTemplate.EnValue = qcReTestUpdateData.EnValue;
                qcReTestTemplate.DocumentStatus = Convert.ToInt16(qcReTestUpdateData.DocumentStatus);
                qcReTestTemplate.ModifiedBy = qcReTestUpdateData.ModifiedBy;
                qcReTestTemplate.ModifiedOn = qcReTestUpdateData.ModifiedOn;
                qcReTestTemplate.Conclusion = qcReTestTemplate.Conclusion;

                if (qcReTestUpdateData.ImageUpload2 != null)
                {
                    string filePath = _utilityService.UploadImage(qcReTestUpdateData.ImageUpload2,
                                                                Constants.QCReeTest_FolderName);
                    IFormFile fileobj2 = qcReTestUpdateData.ImageUpload2;
                    qcReTestTemplate.MUx2FileName = fileobj2.FileName;
                }
            }
            if (qcReTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.Approved
                || qcReTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.Rejected)
            {
                qcReTestTemplate.DocumentStatus = Convert.ToInt16(qcReTestUpdateData.DocumentStatus);
                qcReTestTemplate.ReviewedBy = qcReTestUpdateData.ReviewedBy;
                qcReTestTemplate.ReviewedOn = qcReTestUpdateData.ReviewedOn;
                qcReTestTemplate.FinalStatus = qcReTestUpdateData.FinalStatus;
            }
            _unitOfWork.BeginTransaction();
            _unitOfWork.Repository<QCReTestTemplate>().Update(qcReTestTemplate);
            _unitOfWork.SaveChanges();
            if (qcReTestUpdateData.DocumentStatus == (int)ReplicateTestStatus.ResultTwoSubmitted)
            {
                List<ReTestDataViewModel> qcReTestChildListData = new List<ReTestDataViewModel>();
                qcReTestUpdateData.Obs2.SINo =2;
                qcReTestUpdateData.Obs2.ParentId = qcReTestTemplate.Id;
                qcReTestChildListData.Add(qcReTestUpdateData.Obs2);
                _unitOfWork.Repository<QCReTestTemplateData>().InsertRange(_mapper.Map<QCReTestTemplateData[]>
                                                                                    (qcReTestChildListData));
                _unitOfWork.SaveChanges();
            }
            _unitOfWork.Commit();
            return new ResponseViewModel<ReTestViewModel>
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
            return new ResponseViewModel<ReTestViewModel>
            {
                ResponseCode = 500,
                ResponseMessage = "Failure",
                ErrorMessage = e.Message,
                ResponseData = qcReTestUpdateData,
                ResponseDataList = null,
                ResponseService = "QCReTestTemplateService",
                ResponseServiceMethod = "UpdateData"
            };
        }
    }
    public string GetUserName(string shortId)
    {
        var userName = string.Empty;

        var user = _unitOfWork.Repository<User>().GetQueryAsNoTracking(x => x.ShortId == shortId);
        if (user.Any())
            userName = user.First().FirstName + " " + user.First().LastName;

        return userName;
    }
}