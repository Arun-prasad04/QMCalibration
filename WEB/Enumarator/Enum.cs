namespace WEB.Models;
public enum EnumRequestStatus{
		Requested=26,
		Approved=27,
		Rejected=28,
		Sent=29,
		Closed=30,
		CalibrationReject = 31
}
public enum FeedbackDocumentStatus{
		ActionRequired = 0,
		Submitted = 1,
		Reviewed = 2,

}
public enum QualityCheckDocumentStatus{
		Submitted = 1,
		Reviewed = 2,

}
public enum QualityCheckSerialNo {
	SerialNoOne = 1,
	SerialNoTwo = 2
}
public enum DocumentStatus{
		Submitted = 1,
		Approved = 2,
		Rejected = 3
}

public enum ReplicateTestStatus
{
		ResultOneSubmitted = 1,
		ResultTwoSubmitted = 2,
		Approved = 3,
		Rejected = 4
}
public enum ToolInventoryStatus
{
	UserTool = 1,
	RequestTool = 2,
	AcceptTool = 3,
	RejectedTool = 4,
	SentTool = 5,
	ClosedTool = 6,
	CalibrationReject = 7,
	BackToToolRoom = 8
	}
public enum ToolRoomStatus
{
	Completed = 1,
	Pending = 0
}

