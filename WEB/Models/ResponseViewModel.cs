namespace WEB.Models;
public class ResponseViewModel<T> where T:class
{
    public int ResponseCode{get;set;}
    public string ResponseMessage {get;set;}
    public string ErrorMessage {get;set;}
    public T ResponseData{get;set;}
    public List<T> ResponseDataList{get;set;}
    public string ResponseService{get;set;}
    public string ResponseServiceMethod{get;set;}
}