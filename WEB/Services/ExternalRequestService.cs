using CMT.DAL;
using CMT.DATAMODELS;
using WEB.Services.Interface;
using AutoMapper;
using WEB.Models;
using Microsoft.EntityFrameworkCore;
namespace WEB.Services;
public class ExternalRequestService:IExternalRequestService
{
private readonly IMapper _mapper;
private IUnitOfWork _unitOfWork{get;set;}
private IHttpContextAccessor _contextAccessor { get; set; }
    private IEmailService _emailService;
public ExternalRequestService(IUnitOfWork unitOfWork,IMapper mapper,IHttpContextAccessor contextAccessor,IEmailService emailService){
            _unitOfWork=unitOfWork;
            _mapper = mapper;  
            _contextAccessor=contextAccessor;
            _emailService=emailService;
    }
public ResponseViewModel<ExternalRequestViewModel> GetAllExternalRequestList(){
        try
       {
            List<ExternalRequestViewModel> externalRequestList= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking().Include(I=>I.MasterModel).Include(I=>I.ExternalRequestStatusModal).Include(I=>I.ExternalRequestStatusModal).Select(s=>new ExternalRequestViewModel(){
        Id=s.Id,
        ReqNo=s.ReqNo,
        RequestDate=s.RequestDate,
        MasterName=s.MasterModel.EquipName,
        MasterSerialNo=s.MasterModel.SerialNo,
        MasterIdNo=s.MasterModel.LabId,
        CalibrationDate=s.MasterModel.CalibDate,
        NextDue=s.MasterModel.DueDate,
        CertificateNo =s.MasterModel.CertNo,
        Traceability=s.MasterModel.Traceability,
        SubmittedOn=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Approved || W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
        RecordBy =s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        ResultFM=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.Comment).FirstOrDefault(),
        ResultLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.Comment).FirstOrDefault(),
        ClosedDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.CreatedOn).FirstOrDefault(),
        ReturnDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.CreatedOn).FirstOrDefault(),
        RecodedByLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        Status=s.ExternalRequestStatusModal.OrderByDescending(O=>O.CreatedOn).Select(S=>S.StatusId).FirstOrDefault(),
            }).ToList();
            return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=externalRequestList
            };
        }catch(Exception e){
             return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalRequest",
                ResponseServiceMethod="GetAllLabExternalRequestList"
            };
        }
    }
public ResponseViewModel<ExternalRequestViewModel> GetExternalRequestById(int externalRequestId){  
        try
        {
       
        ExternalRequestViewModel externalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).Include(I=>I.MasterModel).Include(I=>I.ExternalRequestStatusModal).Include(I=>I.ExternalRequestStatusModal).Select(s=>new ExternalRequestViewModel(){
        Id=s.Id,
        ReqNo=s.ReqNo,
        RequestDate=s.RequestDate,
        MasterName=s.MasterModel.EquipName,
        MasterSerialNo=s.MasterModel.SerialNo,
        MasterIdNo=s.MasterModel.LabId,
        CalibrationDate=s.MasterModel.CalibDate,
        NextDue=s.MasterModel.DueDate,
        CertificateNo =s.MasterModel.CertNo,
        Traceability=s.MasterModel.Traceability,
        SubmittedOn=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Approved || W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
        RecordBy =s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        ResultFM=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.Comment).FirstOrDefault(),
        ResultLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.Comment).FirstOrDefault(),
        ClosedDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.CreatedOn).FirstOrDefault(),
        ReturnDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.CreatedOn).FirstOrDefault(),
        RecodedByLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        Status=s.ExternalRequestStatusModal.OrderByDescending(O=>O.CreatedOn).Select(S=>S.StatusId).FirstOrDefault(),
        RejectReason=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.Comment).FirstOrDefault(),
        CreatedBy=s.CreatedBy
        
        }).SingleOrDefault();
        return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=externalRequestById,
                ResponseDataList=null
            };
        }catch(Exception e){
             return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="GetExternalCalibrationRequestByID"
            };
        }

    }
public ResponseViewModel<ExternalRequestViewModel> InsertExternalRequest(int masterId,int userId){
        try{
            _unitOfWork.BeginTransaction();
            

            Master masterById = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == masterId).SingleOrDefault();
            ExternalRequest newExternalRequest =new ExternalRequest();
            ExternalRequest getMaxId= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id>0).OrderByDescending(O=>O.Id).FirstOrDefault();
            long maxId=1;
            if(getMaxId!=null){
                maxId=getMaxId.Id+1;
            }
            string requestNumberFormat=maxId.ToString().PadLeft(4, '0');
            newExternalRequest.ReqNo="EXCR"+DateTime.Now.Year+requestNumberFormat;
            newExternalRequest.RequestDate=DateTime.Now;
            newExternalRequest.MasterId=masterId;
            newExternalRequest.CreatedOn=DateTime.Now;
            newExternalRequest.ModifiedOn=DateTime.Now;
            newExternalRequest.CreatedBy=userId;
            newExternalRequest.ModifiedBy=userId;
            newExternalRequest.StatusId=(Int32)EnumRequestStatus.Requested;
            _unitOfWork.Repository<ExternalRequest>().Insert(newExternalRequest);
            _unitOfWork.SaveChanges();
            ExternalRequestStatus exReqestStatus=new ExternalRequestStatus();
            exReqestStatus.ExternalRequestId=newExternalRequest.Id;
            exReqestStatus.StatusId=(Int32)EnumRequestStatus.Requested;
            exReqestStatus.CreatedOn=DateTime.Now;
            exReqestStatus.CreatedBy=userId;
            _unitOfWork.Repository<ExternalRequestStatus>().Insert(exReqestStatus);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            string UserId=_contextAccessor.HttpContext.Session.GetString("LoggedId");
            UserViewModel labUserById= _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q=>Q.Id==Convert.ToInt32(UserId)).Include(I=>I.Department).SingleOrDefault());
            UserViewModel fmUserById= _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q=>Q.UserRoleId==3).SingleOrDefault());
            string mailbody="<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New External Calibration Request has been Created by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>                        <tr>                <td align='left'>                    Master Equipment Name                </td><td>:</td>                <td>                    $MASTERNAME$                </td>            </tr>     <tr>                <td align='left'>                    Lab ID.No                </td>     <td>:</td>           <td>                    $LABIDNO$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p> <p><b>Regards,</b><br />               <b> $REQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            mailbody=mailbody.Replace("$NAME$",fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQNO$",newExternalRequest.ReqNo).Replace("$MASTERNAME$",masterById.EquipName).Replace("$LABIDNO$",masterById.LabId).Replace("$REQNAME$",labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$",Convert.ToString(newExternalRequest.CreatedOn)).Replace("$REQNAME$",labUserById.FirstName + " " + labUserById.LastName).Replace("$REQDEPT$",labUserById.DepartmentName).Replace("$USERNAME$",labUserById.FirstName+" "+labUserById.LastName);
            List<string> emailList=new List<string>();
            emailList.Add(fmUserById.Email);
            EmailViewModel emailViewModel=new EmailViewModel(){
            ToList=emailList,
            Subject = "New External Calibration Request -"+newExternalRequest.ReqNo+"",
            Body=mailbody,
            IsHtml=true
            };
            _emailService.SendEmailAsync(emailViewModel,true);
         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=null
            };
        }catch(Exception e){
            _unitOfWork.RollBack();
            return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="InsertExternalCalibrationRequest"
            };
        }
    }
public ResponseViewModel<ExternalRequestViewModel> UpdateExternalRequest(ExternalRequestViewModel externalRequest)
    {

        try{
            _unitOfWork.BeginTransaction();
            ExternalRequest ExternalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking().SingleOrDefault();
            _unitOfWork.Repository<ExternalRequest>().Update(ExternalRequestById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();

         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=null
            };
        }catch(Exception e){
             return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="UpdateExternalCalibrationRequest"
            };
        }
    }
public ResponseViewModel<ExternalRequestViewModel> DeleteExternalRequest(int externalRequestId){
        try{
            ExternalRequest ExternalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking().SingleOrDefault();
            _unitOfWork.Repository<ExternalRequest>().Delete(ExternalRequestById);
         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=null,
                ResponseDataList=null
            };
        }catch(Exception e){
             return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="DeleteExternalCalibrationRequest"
            };
        }
    }

public ResponseViewModel<ExternalRequestViewModel> AcceptExternalRequest(int externalRequestId,int userId){
        try{
            _unitOfWork.BeginTransaction();
            
            ExternalRequestStatus exReqestStatus=new ExternalRequestStatus();
            exReqestStatus.ExternalRequestId=externalRequestId;
            exReqestStatus.StatusId=(Int32)EnumRequestStatus.Approved;
            exReqestStatus.CreatedOn=DateTime.Now;
            exReqestStatus.CreatedBy=userId;
            _unitOfWork.Repository<ExternalRequestStatus>().Insert(exReqestStatus);
            
            ExternalRequest externalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).SingleOrDefault();
            externalRequestById.StatusId=(Int32)EnumRequestStatus.Approved;
            externalRequestById.ModifiedOn=DateTime.Now;
            externalRequestById.ModifiedBy=userId;
            _unitOfWork.Repository<ExternalRequest>().Update(externalRequestById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            Master masterById = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == externalRequestById.MasterId).SingleOrDefault();
            long UserId=externalRequestById.CreatedBy;
            UserViewModel labUserById= _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q=>Q.Id==Convert.ToInt32(UserId)).Include(I=>I.Department).SingleOrDefault());
            UserViewModel fmUserById= _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q=>Q.UserRoleId==3).Include(I=>I.Department).SingleOrDefault());
            string mailbody="<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New External Calibration Request has been Accepted by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>                        <tr>                <td align='left'>                    Master Equipment Name                </td><td>:</td>                <td>                    $MASTERNAME$                </td>            </tr>     <tr>                <td align='left'>                    Lab ID.No                </td>     <td>:</td>           <td>                    $LABIDNO$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p> <p><b>Regards,</b><br />               <b> $REQQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            mailbody=mailbody.Replace("$NAME$",labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$",externalRequestById.ReqNo).Replace("$MASTERNAME$",masterById.EquipName).Replace("$LABIDNO$",masterById.LabId).Replace("$REQNAME$",labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$",Convert.ToString(externalRequestById.CreatedOn)).Replace("$REQQNAME$",fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQDEPT$",fmUserById.DepartmentName).Replace("$USERNAME$",fmUserById.FirstName+" "+fmUserById.LastName);
            List<string> emailList=new List<string>();
            emailList.Add(fmUserById.Email);
            EmailViewModel emailViewModel=new EmailViewModel(){
            ToList=emailList,
            Subject="External Calibration Request Accept Notification - "+externalRequestById.ReqNo+"",
            Body=mailbody,
            IsHtml=true
            };
            _emailService.SendEmailAsync(emailViewModel,true);
            ExternalRequestViewModel externalRequest= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).Include(I=>I.MasterModel).Include(I=>I.ExternalRequestStatusModal).Include(I=>I.ExternalRequestStatusModal).Select(s=>new ExternalRequestViewModel(){
        Id=s.Id,
        ReqNo=s.ReqNo,
        RequestDate=s.RequestDate,
        MasterName=s.MasterModel.EquipName,
        MasterSerialNo=s.MasterModel.SerialNo,
        MasterIdNo=s.MasterModel.LabId,
        CalibrationDate=s.MasterModel.CalibDate,
        NextDue=s.MasterModel.DueDate,
        CertificateNo =s.MasterModel.CertNo,
        Traceability=s.MasterModel.Traceability,
        SubmittedOn=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Approved || W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
        RecordBy =s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        ResultFM=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.Comment).FirstOrDefault(),
        ResultLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.Comment).FirstOrDefault(),
        ClosedDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.CreatedOn).FirstOrDefault(),
        ReturnDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.CreatedOn).FirstOrDefault(),
        RecodedByLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        Status=s.ExternalRequestStatusModal.OrderByDescending(O=>O.CreatedOn).Select(S=>S.StatusId).FirstOrDefault(),
        RejectReason=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.Comment).FirstOrDefault(),
        }).SingleOrDefault();
         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=externalRequest,
                ResponseDataList=null
            };
        }catch(Exception e){
            return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="InsertExternalCalibrationRequest"
            };
        }
    }
public ResponseViewModel<ExternalRequestViewModel> RejectExternalRequest(int externalRequestId,string RejectReason,int userId){
        try{
            _unitOfWork.BeginTransaction();
            
            ExternalRequestStatus exReqestStatus=new ExternalRequestStatus();
            exReqestStatus.ExternalRequestId=externalRequestId;
            exReqestStatus.StatusId=(Int32)EnumRequestStatus.Rejected;
            exReqestStatus.CreatedOn=DateTime.Now;
            exReqestStatus.CreatedBy=userId;
            exReqestStatus.Comment=RejectReason;
            _unitOfWork.Repository<ExternalRequestStatus>().Insert(exReqestStatus);
            
            
            ExternalRequest externalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).SingleOrDefault();
            externalRequestById.StatusId=(Int32)EnumRequestStatus.Rejected;
            externalRequestById.ModifiedOn=DateTime.Now;
            externalRequestById.ModifiedBy=userId;
            _unitOfWork.Repository<ExternalRequest>().Update(externalRequestById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            Master masterById = _unitOfWork.Repository<Master>().GetQueryAsNoTracking(Q => Q.Id == externalRequestById.MasterId).SingleOrDefault();
            long UserId=externalRequestById.CreatedBy;
            UserViewModel labUserById= _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q=>Q.Id==Convert.ToInt32(UserId)).Include(I=>I.Department).SingleOrDefault());
            UserViewModel fmUserById= _mapper.Map<UserViewModel>(_unitOfWork.Repository<User>().GetQueryAsNoTracking(Q=>Q.UserRoleId==3).Include(I=>I.Department).SingleOrDefault());
            string mailbody="<!DOCTYPE html><html><head><title></title></head><body>    <div style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>        <p>            Dear <b>$NAME$,</b></p>        <p>            New External Calibration Request has been Rejected by <b>$USERNAME$</b>.</p>    <p><b>Request Details :</b></p>  <table style='font-family: CorpoS; font-size: 10pt; font-weight: Normal;'>            <tr>                <td align='left'>                    Request No.                </td>  <td>:</td>              <td>                    $REQNO$                </td>            </tr>                        <tr>                <td align='left'>                    Master Equipment Name                </td><td>:</td>                <td>                    $MASTERNAME$                </td>            </tr>     <tr>                <td align='left'>                    Lab ID.No                </td>     <td>:</td>           <td>                    $LABIDNO$                </td>            </tr>    <tr>                <td align='left'>                    Requested By                </td>     <td>:</td>           <td>                    $REQNAME$                </td>            </tr><tr>                <td align='left'>                    Date                </td><td>:</td>                <td>                    $DATE$                </td>            </tr><tr>                <td align='left'>                    Reason for Rejection                </td><td>:</td>                <td>                    $REASON$                </td>            </tr></table>  <p><a href='http://s365id1qdg044/cmtlive/' style='font-family: CorpoS; font-size: 10pt; font-weight: Bold;'>CMT Portal</a></p> <p><b>Regards,</b><br />               <b> $REQQNAME$</b>,                <br />                <b>$REQDEPT$</b></p>         <p>            This is a system generated message. So do not reply to this email.</p>    </div></body></html>";
            mailbody=mailbody.Replace("$NAME$",labUserById.FirstName + " " + labUserById.LastName).Replace("$REQNO$",externalRequestById.ReqNo).Replace("$MASTERNAME$",masterById.EquipName).Replace("$LABIDNO$",masterById.LabId).Replace("$REQNAME$",labUserById.FirstName + " " + labUserById.LastName).Replace("$DATE$",Convert.ToString(externalRequestById.CreatedOn)).Replace("$REQQNAME$",fmUserById.FirstName + " " + fmUserById.LastName).Replace("$REQDEPT$",fmUserById.DepartmentName).Replace("$USERNAME$",fmUserById.FirstName+" "+fmUserById.LastName).Replace("$REASON$",RejectReason);
            List<string> emailList=new List<string>();
            emailList.Add(fmUserById.Email);
            EmailViewModel emailViewModel=new EmailViewModel(){
            ToList=emailList,
            Subject="External Calibration Request Reject Notification-"+externalRequestById.ReqNo+"",
            Body=mailbody,
            IsHtml=true
            };
            _emailService.SendEmailAsync(emailViewModel,true);
         ExternalRequestViewModel externalRequest= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).Include(I=>I.MasterModel).Include(I=>I.ExternalRequestStatusModal).Include(I=>I.ExternalRequestStatusModal).Select(s=>new ExternalRequestViewModel(){
        Id=s.Id,
        ReqNo=s.ReqNo,
        RequestDate=s.RequestDate,
        MasterName=s.MasterModel.EquipName,
        MasterSerialNo=s.MasterModel.SerialNo,
        MasterIdNo=s.MasterModel.LabId,
        CalibrationDate=s.MasterModel.CalibDate,
        NextDue=s.MasterModel.DueDate,
        CertificateNo =s.MasterModel.CertNo,
        Traceability=s.MasterModel.Traceability,
        SubmittedOn=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Approved || W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
        RecordBy =s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        ResultFM=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.Comment).FirstOrDefault(),
        ResultLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.Comment).FirstOrDefault(),
        ClosedDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.CreatedOn).FirstOrDefault(),
        ReturnDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.CreatedOn).FirstOrDefault(),
        RecodedByLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        Status=s.ExternalRequestStatusModal.OrderByDescending(O=>O.CreatedOn).Select(S=>S.StatusId).FirstOrDefault(),
        RejectReason=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.Comment).FirstOrDefault(),
        }).SingleOrDefault();
         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=externalRequest,
                ResponseDataList=null
            };
        }catch(Exception e){
            return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="AcceptExternalCalibrationRequest"
            };
        }
    }
public ResponseViewModel<ExternalRequestViewModel> SubmitFMExternalRequest(int externalRequestId,string Result,int userId){
        try{
            _unitOfWork.BeginTransaction();
            ExternalRequestStatus exReqestStatus=new ExternalRequestStatus();
            exReqestStatus.ExternalRequestId=externalRequestId;
            exReqestStatus.StatusId=(Int32)EnumRequestStatus.Sent;
            exReqestStatus.CreatedOn=DateTime.Now;
            exReqestStatus.CreatedBy=userId;
            exReqestStatus.Comment=Result;
            _unitOfWork.Repository<ExternalRequestStatus>().Insert(exReqestStatus);
            
            
            ExternalRequest externalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).SingleOrDefault();
            externalRequestById.StatusId=(Int32)EnumRequestStatus.Sent;
            externalRequestById.ModifiedOn=DateTime.Now;
            externalRequestById.ModifiedBy=userId;
            _unitOfWork.Repository<ExternalRequest>().Update(externalRequestById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
       ExternalRequestViewModel externalRequest= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).Include(I=>I.MasterModel).Include(I=>I.ExternalRequestStatusModal).Include(I=>I.ExternalRequestStatusModal).Select(s=>new ExternalRequestViewModel(){
        Id=s.Id,
        ReqNo=s.ReqNo,
        RequestDate=s.RequestDate,
        MasterName=s.MasterModel.EquipName,
        MasterSerialNo=s.MasterModel.SerialNo,
        MasterIdNo=s.MasterModel.LabId,
        CalibrationDate=s.MasterModel.CalibDate,
        NextDue=s.MasterModel.DueDate,
        CertificateNo =s.MasterModel.CertNo,
        Traceability=s.MasterModel.Traceability,
        SubmittedOn=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Approved || W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
        RecordBy =s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        ResultFM=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.Comment).FirstOrDefault(),
        ResultLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.Comment).FirstOrDefault(),
        ClosedDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.CreatedOn).FirstOrDefault(),
        ReturnDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.CreatedOn).FirstOrDefault(),
        RecodedByLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        Status=s.ExternalRequestStatusModal.OrderByDescending(O=>O.CreatedOn).Select(S=>S.StatusId).FirstOrDefault(),
        RejectReason=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.Comment).FirstOrDefault(),
        }).SingleOrDefault();
         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=externalRequest,
                ResponseDataList=null
            };
        }catch(Exception e){
            return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="AcceptExternalCalibrationRequest"
            };
        }
    }
public ResponseViewModel<ExternalRequestViewModel> SubmitLABExternalRequest(int externalRequestId,string Result,int userId){
        try{
            _unitOfWork.BeginTransaction();
            ExternalRequestStatus exReqestStatus=new ExternalRequestStatus();
            exReqestStatus.ExternalRequestId=externalRequestId;
            exReqestStatus.StatusId=(Int32)EnumRequestStatus.Closed;
            exReqestStatus.CreatedOn=DateTime.Now;
            exReqestStatus.CreatedBy=userId;
            exReqestStatus.Comment=Result;
            _unitOfWork.Repository<ExternalRequestStatus>().Insert(exReqestStatus);
            
            
            ExternalRequest externalRequestById= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).SingleOrDefault();
            externalRequestById.StatusId=(Int32)EnumRequestStatus.Closed;
            externalRequestById.ModifiedOn=DateTime.Now;
            externalRequestById.ModifiedBy=userId;
            _unitOfWork.Repository<ExternalRequest>().Update(externalRequestById);
            _unitOfWork.SaveChanges();
            _unitOfWork.Commit();
            
            ExternalRequestViewModel externalRequest= _unitOfWork.Repository<ExternalRequest>().GetQueryAsNoTracking(Q=>Q.Id==externalRequestId).Include(I=>I.MasterModel).Include(I=>I.ExternalRequestStatusModal).Include(I=>I.ExternalRequestStatusModal).Select(s=>new ExternalRequestViewModel(){
        Id=s.Id,
        ReqNo=s.ReqNo,
        RequestDate=s.RequestDate,
        MasterName=s.MasterModel.EquipName,
        MasterSerialNo=s.MasterModel.SerialNo,
        MasterIdNo=s.MasterModel.LabId,
        CalibrationDate=s.MasterModel.CalibDate,
        NextDue=s.MasterModel.DueDate,
        CertificateNo =s.MasterModel.CertNo,
        Traceability=s.MasterModel.Traceability,
        SubmittedOn=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Approved || W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.CreatedOn.GetValueOrDefault()).FirstOrDefault(),
        RecordBy =s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        ResultFM=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.Comment).FirstOrDefault(),
        ResultLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.Comment).FirstOrDefault(),
        ClosedDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Sent).Select(S=>S.CreatedOn).FirstOrDefault(),
        ReturnDate=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.CreatedOn).FirstOrDefault(),
        RecodedByLAB=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Closed).Select(S=>S.UserModel.FirstName+" "+ S.UserModel.LastName).FirstOrDefault(),
        Status=s.ExternalRequestStatusModal.OrderByDescending(O=>O.CreatedOn).Select(S=>S.StatusId).FirstOrDefault(),
        RejectReason=s.ExternalRequestStatusModal.Where(W=>W.StatusId==(int)EnumRequestStatus.Rejected).Select(S=>S.Comment).FirstOrDefault(),
        }).SingleOrDefault();
         return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=200,
                ResponseMessage="Success",
                ResponseData=externalRequest,
                ResponseDataList=null
            };
        }catch(Exception e){
            return new ResponseViewModel<ExternalRequestViewModel>{
                ResponseCode=500,
                ResponseMessage="Failure",
                ErrorMessage=e.Message,
                ResponseData=null,
                ResponseDataList=null,
                ResponseService="ExternalCalibrationRequest",
                ResponseServiceMethod="AcceptExternalCalibrationRequest"
            };
        }
    }
}
