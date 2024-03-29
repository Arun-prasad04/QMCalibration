﻿var FileData = [];


function logoShowHide() {
    var x = document.getElementById("logoheader");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}
//SweetAlert
function showSuccess(successMsg, lang) {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        text: successMsg,
        customClass: {
            title: 'swal2-trn',
            confirmButton: 'swal2-trn',
        },
        footer: '',
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        },
        didOpen: function (ele) {
            $(ele).find('div.swal2-html-container')
                .addClass('swal2-trn')

            if (lang == "en") {
                translator = $('body').swaltranslate({ lang: "en", t: dict });
            }
            else {
                translator = $('body').swaltranslate({ lang: "jp", t: dict });
            }
        }
    });
}

function showError(errorMsg, lang) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: errorMsg,
        customClass: {
            title: 'swal2-trn',
            confirmButtonText: 'swal2-trn',
        },
        footer: '',
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        },
        didOpen: function (ele) {
            $(ele).find('div.swal2-html-container')
                .addClass('swal2-trn')

            console.log(lang);
            if (lang == "en") {
                translator = $('body').swaltranslate({ lang: "en", t: dict });

            }
            else {
                translator = $('body').swaltranslate({ lang: "jp", t: dict });
            }
        }
    });
}

function showWarning(warningMsg, lang) {
    Swal.fire({
        icon: 'warning',
        title: 'Warning',
        text: warningMsg,
        footer: '',
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        },
        didOpen: function (ele) {
            $(ele).find('div.swal2-html-container')
                .addClass('swal2-trn')
//            console.log(lang);
            if (lang == "en") {
                translator = $('body').swaltranslate({ lang: "en", t: dict });

            }
            else {
                translator = $('body').swaltranslate({ lang: "jp", t: dict });
            }
        },
        customClass: {
            title: 'swal2-trn',
            confirmButtonText: 'swal2-trn',
        }
    });
}
$("#profileUpdate").click(function () {
    var url = document.location.origin + '/User/ProfileUpdate';
    $.ajax({
        url: url,
        success: function (result) {
            $("#modelBody").html(result);
        }
    });
});

function validateSession(sessionvalue) {
    if (sessionvalue == undefined || sessionvalue == '') {
        Swal.fire({
            title: 'Session Expired! Please login to continue your work.',
            icon: 'warning',
            confirmButtonText: 'OK',
            allowOutsideClick: false,
            customClass: {
                title: 'swal2-trn',
                confirmButtonText: 'swal2-trn',
            }

        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                // window.location.href = 'http://s365id1qdg044/cmtlive/Account/Login';
                window.location.href = 'http://s365id1qf042.in365.corpintra.net/DTAQMPortalUAT/';
            }
        });
    }
}
function showPopup(resCode, resMsg, lang) {
    if (resCode != '' && resCode != undefined && resCode == 200) {
        showSuccess(resMsg, lang);
    } else if (resCode != undefined && resCode != '') {
        showError(resMsg, lang);
    }
}
//External Request
function viewExternalRequest(element) {
    $('#ExternalCalibId').val(element.id);
    $.ajax({
        url: '../Tracker/ExternalRequestGetById',
        type: 'POST',
        data: { externalRequestId: element.id }
    }).done(function (resultObject) {
        AssignExternalRequestValues(resultObject);
        $("#trackerViewModel").modal('show');
    });
}
function AssignExternalRequestValues(resultObject) {
    if (resultObject != null) {
        $('#ReqNo').val(resultObject.reqNo);
        $('#RequestDate').val(resultObject.requestDate);
        $('#MasterName').val(resultObject.masterName);
        $('#MasterSerialNo').val(resultObject.masterSerialNo);
        $('#MasterIdNo').val(resultObject.masterIdNo);
        $('#CalibrationDate').val(resultObject.calibrationDate);
        $('#NextDue').val(resultObject.nextDue);
        $('#CertificateNo').val(resultObject.certificateNo);
        $('#Traceability').val(resultObject.traceability);
        $('#AcceptOrReject').val(resultObject.acceptOrReject);
        $('#SubmittedOn').val(resultObject.submittedOn);
        $('#rejectedOn').val(resultObject.submittedOn);
        $('#rejectedReason').val(resultObject.rejectReason);
        $('#VisualCheckFM').val(resultObject.visualCheckFm);
        if (resultObject.recordBy != '' && resultObject.recordBy != null) {
            $('#RecordBy').val(resultObject.recordBy);
        }
        $('#ResultFM').val(resultObject.resultFM);
        $('#ResultLAB').val(resultObject.resultLAB);
        if (resultObject.closedDate != '' && resultObject.closedDate != null) {
            $('#ClosedDate').val(resultObject.closedDate);
        }
        $('#ReturnToLab').val(resultObject.returnToLab);
        if (resultObject.ReturnDate != '' && resultObject.ReturnDate != null) {
            $('#ReturnDate').val(resultObject.returnDate);
        }
        $('#VisualCheckLAB').val(resultObject.visualCheckLab);
        $('#status_rejected').css('display', 'none');
        $('#rejectSection').css('display', 'none');
        $('#status_waitingFM').css('display', 'none');
        $('#requestSection').css('display', 'none');
        $('#status_accepted').css('display', 'none');
        $('#acceptSection').css('display', 'none');
        $('#status_FMVisusalChk').css('display', 'none');
        $('#fmVisualCheckSection').css('display', 'none');
        $('#status_LabVisusalChk').css('display', 'none');
        $('#labVisualCheckSection').css('display', 'none');
        $('#status_rejected').css('display', 'none');
        $('#status_accepted').css('display', 'none');
        $('#status_RequestCompleted').css('display', 'none');


        if (resultObject.status == 28) {
            $('#status_rejected').css('display', 'block');
            $('#rejectSection').css('display', 'block');
        } else if (resultObject.status == 26) {
            $('#status_waitingFM').css('display', 'block');
            if ($('#userRoleId').val() == 3) {
                $('#requestSection').css('display', 'block');
            }
        } else if (resultObject.status == 27) {
            $('#status_accepted').css('display', 'block');
            $('#acceptSection').css('display', 'block');
            if ($('#userRoleId').val() == 3) {
                $('#fmVisualCheckSection').css('display', 'block');
                $('#ResultFM').removeAttr('disabled');
            }
        } else if (resultObject.status == 29) {
            $('#SubmitFMVisual').css('display', 'none');
            $('#fmVisualCheckSection').css('display', 'block');

            $('#VisualCheckFM').attr('disabled', 'disabled');
            $('#RecordBy').attr('disabled', 'disabled');
            $('#ResultFM').attr('disabled', 'disabled');
            $('#ClosedDate').attr('disabled', 'disabled');

            if ($('#userRoleId').val() == 2) {
                $('#labVisualCheckSection').css('display', 'block');
                $('#ReturnToLAB').removeAttr('disabled');
                $('#VisualCheckLAB').removeAttr('disabled');
            }
            $('#status_FMVisusalChk').css('display', 'block');
            $('#fmVisualCheckSection').css('display', 'block');
        } else if (resultObject.status == 30) {
            $('#ResultLAB').attr('disabled', 'disabled');
            $('#ClosedDate').attr('disabled', 'disabled');
            $('#ReturnDate').attr('disabled', 'disabled');
            $('#ResultFM').attr('disabled', 'disabled');
            $('#fmVisualCheckSection').css('display', 'block');
            $('#labVisualCheckSection').css('display', 'block');
            $('#SubmitFMVisual').css('display', 'none');
            $('#SubmitLABVisual').css('display', 'none');
            $('#labVisualCheckSection').css('display', 'block');
            $('#status_RequestCompleted').css('display', 'block');
        }
    }
}

function AcceptExternalRequest() {
    $.ajax({
        url: '../Tracker/AcceptExternalRequest',
        type: 'POST',
        data: { externalRequestId: $('#ExternalCalibId').val() }
    }).done(function (resultObject) {
        AssignExternalRequestValues(resultObject);
        showSuccess("You are accepted the request. LAB admin get notified!", lang);
    });
}

function RejecttExternalRequest() {
    $.ajax({
        url: '../Tracker/RejectExternalRequest',
        type: 'POST',
        data: { externalRequestId: $('#ExternalCalibId').val(), rejectReason: $('#reason').val() }
    }).done(function (resultObject) {
        AssignExternalRequestValues(resultObject);
        showSuccess("You are rejected the request. LAB admin get notified!", lang);
    });
}

function SubmitFMVisual(lang) {

    if ($('#ResultFM').val() == '' || $('#ResultFM').val() == undefined) {
        $('#ResultFM').addClass('is-invalid');
        return false;
    } else {
        $('#ResultFM').removeClass('is-invalid');

    }

    $.ajax({
        url: '../Tracker/SubmitFMExternalRequest',
        type: 'POST',
        data: { externalRequestId: $('#ExternalCalibId').val(), Result: $('#ResultFM').val() }
    }).done(function (resultObject) {
        AssignExternalRequestValues(resultObject);
        showSuccess("Your visual check details recorded", lang);

    });
}

function SubmitLABVisual(lang) {

    if ($('#ResultLAB').val() == '' || $('#ResultLAB').val() == undefined) {
        $('#ResultLAB').addClass('is-invalid');
        return false;
    } else {
        $('#ResultLAB').removeClass('is-invalid');
    }

    $.ajax({
        url: '../Tracker/SubmitLABExternalRequest',
        type: 'POST',
        data: { externalRequestId: $('#ExternalCalibId').val(), Result: $('#ResultLAB').val() }
    }).done(function (resultObject) {
        AssignExternalRequestValues(resultObject);
        showSuccess("Your visual check details recorded", lang);
    });
}

function EnableReason() {
    if ($('input[name="AcceptReject"]:checked').val() == 'Accept') {
        $('#rejectionReasonSection').css('display', 'none');
    } else {
        $('#rejectionReasonSection').css('display', 'block');
    }
}

function submitAcceptReject(lang) {
    if ($('input[name="AcceptReject"]:checked').val() == undefined || $('input[name="AcceptReject"]:checked').val() == '') {
        showWarning("Please choose either Accept / Reject and try again.", lang);
    } else if ($('input[name="AcceptReject"]:checked').val() == 'Accept') {
        AcceptExternalRequest(lang);
    } else {
        if ($('#reason').val() != '') {
            $('#reason').removeClass('is-invalid');
            RejecttExternalRequest(lang);
        } else {
            $('#reason').addClass('is-invalid');
            showWarning("Please enter reason for rejection and try again.", lang);
        }
    }
}
//Request Section

function CloseTrackerPopup() {
    $("#trackerViewModel").modal('hide');
}

function MasterQuarantineClick(element, lang) {
    Swal.fire({
        title: 'Enter Reason for Quarantine',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off'
        },
        confirmButtonText: 'Quarantine',
        showCancelButton: true,
        showLoaderOnConfirm: true,
        closeOnConfirm: false,
        closeOnCancel: false,
        reverseButtons: true,
        customClass: {
            confirmButton: 'swal2-trn',
            cancelButton: 'swal2-trn',
            title: 'swal2-trn',
        },
        preConfirm: (value) => {
            if (!value) {
                Swal.showValidationMessage(
                    '<span class="swal2-trn">Please fill the reason for Quarantine</span>'
                )
                if (lang == "en") {
                    translator = $('body').swaltranslate({ lang: "en", t: dict });
                }
                else {
                    translator = $('body').swaltranslate({ lang: "jp", t: dict });
                }
            }
            else {
                $.ajax({
                    url: '../Master/MasterQuarantine',
                    type: 'POST',
                    data: { masterId: element.id, reason: value }
                }).done(function (resultObject) {
                    showSuccess("Master moved to quarantine list", lang);

                    $('#row_' + element.id).next("tr").remove()
                    $('#row_' + element.id).remove();
                });
            }
        },
        allowOutsideClick: () => !Swal.isLoading()
    });

    if (lang == "en") {
        translator = $('body').swaltranslate({ lang: "en", t: dict });

    }
    else {
        translator = $('body').swaltranslate({ lang: "jp", t: dict });
    }
}

function MasterUnQuarantineClick(element, lang) {
    $.ajax({
        url: '../Master/MasterRemoveQuarantine',
        type: 'POST',
        data: { masterId: element.id }
    }).done(function (resultObject) {
        showSuccess("Master Activated Successfully", lang);
        $('#row_' + element.id).next("tr").remove()
        $('#row_' + element.id).remove();
    });
}

function InstrumentQuarantineClick(element, lang) {
    Swal.fire({
        title: 'Enter Reason for Quarantine',
        input: 'text',
        inputAttributes: {
            autocapitalize: 'off'
        },
        showCancelButton: true,
        confirmButtonText: 'Quarantine',
        closeOnConfirm: false,
        closeOnCancel: false,
        reverseButtons: true,
        showLoaderOnConfirm: true,
        customClass: {
            confirmButton: 'swal2-trn',
            cancelButton: 'swal2-trn',
            title: 'swal2-trn',
        },
        preConfirm: (value) => {
            if (!value) {
                Swal.showValidationMessage(
                    '<span class="swal2-trn">Please fill the reason for Quarantine</span>'
                )
                if (lang == "en") {
                    translator = $('body').swaltranslate({ lang: "en", t: dict });
                }
                else {
                    translator = $('body').swaltranslate({ lang: "jp", t: dict });
                }
            }
            else {
                $.ajax({
                    url: '../Instrument/InstrumentQuarantine',
                    type: 'POST',
                    data: { instrumentId: element.id, reason: value }
                }).done(function (resultObject) {
                    showSuccess("Instrument moved to quarantine list", lang);
                    window.location.href = '../Instrument/Index';
                    $('#row_' + element.id).next("tr").remove()
                    $('#row_' + element.id).remove();
                });

            }
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
    if (lang == "en") {
        translator = $('body').swaltranslate({ lang: "en", t: dict });

    }
    else {
        translator = $('body').swaltranslate({ lang: "jp", t: dict });
    }
}

function InstrumentUnQuarantineClick(element, lang) {
    $.ajax({
        url: '../Instrument/InstrumentRemoveQuarantine',
        type: 'POST',
        data: { instrumentId: element.id }
    }).done(function (resultObject) {
        showSuccess("Instrument Unquarantine successfully", lang);
        window.location.href = '../Instrument/QuratineList';
        $('#row_' + element.id).next("tr").remove()
        $('#row_' + element.id).remove();
    });
}

function viewRequestNew(Id) {
    $('#RequestCalibId').val(Id);
    $.ajax({
        url: '../Tracker/GetRequestById',
        type: 'POST',
        data: { RequestId: Id }
    }).done(function (resultObject) {
        if (resultObject.typeOfRequest == 1) {
            AssignNewRequestValues(resultObject);
            $("#NewInstrument").modal('show');
            $.ajax({
                url: '../Tracker/LoadObservationType',
                type: 'POST',
                data: { attrType: 'ObservationTemplate', attrsubType: '' }
            }).done(function (resultObject) {
                $('#NewObservation')
                    .find('option')
                    .remove();
                for (let index = 0; index < resultObject.length; index++) {
                    optText = resultObject[index].attrValue;
                    optValue = resultObject[index].id;;
                    $('#NewObservation').append(`<option value="${optValue}">${optText}</option>`);
                }
            });
            $.ajax({
                url: '../Tracker/LoadObservationType',
                type: 'POST',
                data: { attrType: 'MUTemplate', attrsubType: '' }
            }).done(function (resultObject) {
                $('#NewMU')
                    .find('option')
                    .remove();
                for (let index = 0; index < resultObject.length; index++) {
                    optText = resultObject[index].attrValue;
                    optValue = resultObject[index].id;;
                    $('#NewMU').append(`<option value="${optValue}">${optText}</option>`);
                }
            });

            $.ajax({
                url: '../Tracker/LoadObservationType',
                type: 'POST',
                data: { attrType: 'CerTemplate', attrsubType: '' }
            }).done(function (resultObject) {
                $('#NewCertification')
                    .find('option')
                    .remove();
                for (let index = 0; index < resultObject.length; index++) {
                    optText = resultObject[index].attrValue;
                    optValue = resultObject[index].id;;
                    $('#NewCertification').append(`<option value="${optValue}">${optText}</option>`);
                }
            });
        } else if (resultObject.typeOfRequest == 2 || resultObject.typeOfRequest == 3) {
            AssignRequestValues(resultObject);
            $("#trackerViewModel").modal('show');
        } else if (resultObject.typeOfRequest == 4) {

            $("#QuarInstrument").modal('show');
            AssignQuarRequestValues(resultObject);
        }
    });
}

function GetRequestDetails(element) {
    window.location.href = '../Tracker/RequestDetailsNew?Id=' + element.id + '';
}
function GetExternalRequestDetails(element) {
    window.location.href = '../Tracker/ExternalRequestNew?Id=' + element.id + '';
}

function viewRequest(element) {
    $('#RequestCalibId').val(element.id);
    $.ajax({
        url: '../Tracker/GetRequestById',
        type: 'POST',
        data: { RequestId: element.id }
    }).done(function (resultObject) {
        if (resultObject.typeOfRequest == 1) {
            AssignNewRequestValues(resultObject);
            $("#NewInstrument").modal('show');
            $.ajax({
                url: '../Tracker/LoadObservationType',
                type: 'POST',
                data: { attrType: 'ObservationTemplate', attrsubType: '' }
            }).done(function (resultObject) {
                $('#NewObservation')
                    .find('option')
                    .remove();
                for (let index = 0; index < resultObject.length; index++) {
                    optText = resultObject[index].attrValue;
                    optValue = resultObject[index].id;;
                    $('#NewObservation').append(`<option value="${optValue}">${optText}</option>`);
                }
            });
            $.ajax({
                url: '../Tracker/LoadObservationType',
                type: 'POST',
                data: { attrType: 'MUTemplate', attrsubType: '' }
            }).done(function (resultObject) {
                $('#NewMU')
                    .find('option')
                    .remove();
                for (let index = 0; index < resultObject.length; index++) {
                    optText = resultObject[index].attrValue;
                    optValue = resultObject[index].id;;
                    $('#NewMU').append(`<option value="${optValue}">${optText}</option>`);
                }
            });

            $.ajax({
                url: '../Tracker/LoadObservationType',
                type: 'POST',
                data: { attrType: 'CerTemplate', attrsubType: '' }
            }).done(function (resultObject) {
                $('#NewCertification')
                    .find('option')
                    .remove();
                for (let index = 0; index < resultObject.length; index++) {
                    optText = resultObject[index].attrValue;
                    optValue = resultObject[index].id;;
                    $('#NewCertification').append(`<option value="${optValue}">${optText}</option>`);
                }
            });
        } else if (resultObject.typeOfRequest == 2 || resultObject.typeOfRequest == 3) {
            AssignRequestValues(resultObject);
            $("#trackerViewModel").modal('show');
        } else if (resultObject.typeOfRequest == 4) {

            $("#QuarInstrument").modal('show');
            AssignQuarRequestValues(resultObject);
        }
    });
}

function AssignRequestValues(resultObject) {
    if (resultObject != null) {
        $('#RequestCalibId').val(resultObject.id);
        $('#ReqestNo').val(resultObject.reqestNo);
        $('#RequestDate').val(resultObject.requestDate);
        $('#InstrumentName').val(resultObject.instrumentName);
        $('#InstrumentSerialNumber').val(resultObject.instrumentSerialNo);
        $('#InstrumentIdNo').val(resultObject.instrumentIdNo);
        $('#ReqestBy').val(resultObject.reqestBy);
        $('#UserDept').val(resultObject.departmentName);
        if (resultObject.isNABL == 0) {
            $('#NABL').val('No');
        } else {
            $('#NABL').val('Yes');
        }
        $('#Range').val(resultObject.range);
        if (resultObject.typeOfRequest == 1) {
            $('#TypeOfRequest').val('New');
        } else if (resultObject.typeOfRequest == 2) {
            $('#TypeOfRequest').val('Regular');
        } else if (resultObject.typeOfRequest == 3) {
            $('#TypeOfRequest').val('Recalibration');
        } else if (resultObject.typeOfRequest == 4) {
            $('#TypeOfRequest').val('UnQuarantine');
        }

        $('#CalibDate').val(resultObject.calibDate);
        $('#NextDue').val(resultObject.nextDue);
        $('#DueDate').val(resultObject.dueDate);
        //$('#CertificateNo').val(resultObject.certificateNo);
        //$('#Traceability').val(resultObject.traceability);
        $('#AcceptOrReject').val(resultObject.acceptOrReject);
        $('#SubmittedOn').val(resultObject.submittedOn);
        $('#rejectedOn').val(resultObject.submittedOn);
        $('#rejectedReason').val(resultObject.rejectReason);
        $('#VisualCheckFM').val(resultObject.visualCheckFm);
        if (resultObject.recordBy != '' && resultObject.recordBy != null) {
            $('#RecordBy').val(resultObject.recordBy);
        }
        if (resultObject.closedDate != '' && resultObject.closedDate != null) {
            $('#ClosedDate').val(resultObject.closedDate);
        }
        if (resultObject.ReturnDate != '' && resultObject.ReturnDate != null) {
            $('#ReturnDate').val(resultObject.returnDate);
        }

        $('#ResultLAB').val(resultObject.resultLAB);
        $('#ResultDEP').val(resultObject.resultDEP);
        $('#ReturnToLab').val(resultObject.returnToLab);
        $('#VisualCheckLAB').val(resultObject.visualCheckLab);
        $('#status_rejected').css('display', 'none');
        $('#rejectSection').css('display', 'none');
        $('#status_waitingFM').css('display', 'none');
        $('#requestSection').css('display', 'none');
        $('#status_accepted').css('display', 'none');
        $('#acceptSection').css('display', 'none');
        $('#status_FMVisusalChk').css('display', 'none');
        $('#fmVisualCheckSection').css('display', 'none');
        $('#status_LabVisusalChk').css('display', 'none');
        $('#labVisualCheckSection').css('display', 'none');
        if (resultObject.receivedBy != null) {
            $('#ReceivedBy').val(resultObject.receivedByName);
            if ($('#ReceivedDate').val() != null) {
                $('#ReceivedDate').val(resultObject.receivedDate);
            }
            $('#InstrumentCondition').val(resultObject.instrumentCondition);
            $('#Feasiblity').val(resultObject.feasiblity);
            $('#TentativeCompletionDate').val(resultObject.tentativeCompletionDate);

            $('#reqReceivedBy').attr('disabled', 'disabled');
            $('#reqReceivedDate').attr('disabled', 'disabled');
            $('#reqInstrumentCondition').attr('disabled', 'disabled');
            $('#reqFeasiblity').attr('disabled', 'disabled');
            $('#reqTentativeCompletionDate').attr('disabled', 'disabled');
            $('#submitReason').css('display', 'none');
        }

        if (resultObject.status == 28) {
            $('#status_rejected').css('display', 'block');
            $('#rejectSection').css('display', 'block');
            $('#requestRequestSection').css('display', 'none');
        } else if (resultObject.status == 26) {
            $('#status_waitingFM').css('display', 'block');
            if ($('#userRoleId').val() == 2) {
                $('#requestRequestSection').css('display', 'block');
            }
        } else if (resultObject.status == 27) {
            $('#requestRequestSection').css('display', 'block');
            $('#submitReason').css('display', 'none');
            $('#reqReceivedDate').attr('disabled', 'disabled');
            $('#reqInstrumentCondition').attr('disabled', 'disabled');
            $('#reqFeasiblity').attr('disabled', 'disabled');
            $('#reqTentativeCompletionDate').attr('disabled', 'disabled');
            $('#status_accepted').css('display', 'block');
            $('#acceptSection').css('display', 'block');
            $('#newRequestSection').css('display', 'block');
            if ($('#userRoleId').val() == 2) {
                $('#fmVisualCheckSection').css('display', 'block');
                $('#ResultLAB').removeAttr('disabled');
            }

        } else if (resultObject.status == 29) {
            $('#SubmitFMVisual').css('display', 'none');
            $('#fmVisualCheckSection').css('display', 'block');
            $('#VisualCheckFM').attr('disabled', 'disabled');
            $('#RecordBy').attr('disabled', 'disabled');
            $('#ResultLAB').attr('disabled', 'disabled');
            $('#ClosedDate').attr('disabled', 'disabled');
            $('#requestRequestSection').css('display', 'block');
            if ($('#userRoleId').val() == 1) {
                $('#labVisualCheckSection').css('display', 'block');
                $('#ReturnToLAB').removeAttr('disabled');
                $('#VisualCheckLAB').removeAttr('disabled');
            }
            $('#status_FMVisusalChk').css('display', 'block');
            $('#fmVisualCheckSection').css('display', 'block');
        } else if (resultObject.status == 30) {
            $('#requestRequestSection').css('display', 'block');
            $('#SubmitFMVisual').css('display', 'none');
            $('#fmVisualCheckSection').css('display', 'block');
            $('#labVisualCheckSection').css('display', 'block');
            $('#SubmitDEPVisual').css('display', 'none');
            $('#labVisualCheckSection').css('display', 'block');
            $('#status_RequestCompleted').css('display', 'block');
            $('#ReturnDate').attr('disabled', 'disabled');
            $('#ResultDEP').attr('disabled', 'disabled');
            if (resultObject.ReceivedBy != null) {
                $('#ReceivedBy').val(resultObject.receivedByName);
                if ($('#ReceivedDate').val() != null) {
                    $('#ReceivedDate').val(resultObject.receivedDate);
                }
                $('#InstrumentCondition').val(resultObject.instrumentCondition);
                $('#Feasiblity').val(resultObject.feasiblity);
                $('#TentativeCompletionDate').val(resultObject.tentativeCompletionDate);

                $('#reqReceivedBy').attr('disabled', 'disabled');
                $('#reqReceivedDate').attr('disabled', 'disabled');
                $('#reqInstrumentCondition').attr('disabled', 'disabled');
                $('#Feasiblity').attr('disabled', 'disabled');
                $('#TentativeCompletionDate').attr('disabled', 'disabled');
                $('#submitReason').css('display', 'none');
                $('#AcceptSection').css('display', 'none');
                $('#RejectSection').css('display', 'none');


            }

        }
    }
}

function ReqEnableReason() {
    if ($('input[name="ReqAcceptReject"]:checked').val() == 'Accept') {
        $('#rejectionReasonSection').css('display', 'none');
    } else {
        $('#rejectionReasonSection').css('display', 'block');
    }
}

function AcceptRejectRequest(lang) {
    // debugger;
    if ($('input[name="ReqAcceptReject"]:checked').val() == undefined || $('input[name="ReqAcceptReject"]:checked').val() == '') {
        showWarning("Please choose either Accept / Reject and try again.", lang);
    } else if ($('input[name="ReqAcceptReject"]:checked').val() == 'Accept') {
        if (ValidateCheck() >= 1) {
            AcceptRequest(2);
        }

    } else {
        if ($('#reason').val() != '') {
            $('#reason').removeClass('is-invalid');
            RejecttRequest(2);
        } else {
            $('#reason').addClass('is-invalid');
            showWarning("Please enter reason for rejection and try again.", lang);
        }
    }
}
function ValidateCheckInstrument() {
    //alert($('#CalibFreq1').val());
    var errCount1 = 0;
    if ((($('#CalibFreq1').val()) == '') || (($('#CalibFreq1').val()) == '0') || (($('#CalibFreq1').val()) == null)) {
        //alert('#CalibFreq1');
       // alert($('#CalibFreq1').val());
        errCount1 = errCount1 + 1;
       // alert(errCount1);
    }
   

    if ($('#DueDate').val().trim() == '') {
        errCount1 = errCount1 + 1;
    }
   
    //return errCount1;
    if (errCount1 >0)
    {
        showWarning('Please Select Due Date, Calibration Frequency Values...!', lang);
        return false;
    }

}
function ValidateCheck() {
    //debugger;
    var errCount = 0;
    $('#StandardRefferedError').hide();
    $('#InstrumentConditionError').hide();
   
    if (($('#StdReffer').val()) == '') {
        errCount = errCount + 1;
        $('#StandardRefferedError').show();
    }
    else {
        //$('#DeptShortNameError').hide();
    }

    if (($('#NewObservation').val()) == '') {
        errCount = errCount + 1;
    }
    else {
        //$('#SourcePlantError').hide();
    }

    if (($('#CalibFreq').val()) == '') {
        errCount = errCount + 1;
    }
    else {
        //$('#CountryError').hide();
    }
  
    if ($('#TentativeCompletionDate').val().trim() == '') {
        errCount = errCount + 1;
    }

    return errCount;
}


function DudeDateCalculation_Old(dt) {

    var duedate;
    if (dt == 13) {
        duedate = getduedate(30);
    }
    else if (dt == 14) {
        duedate = getduedate(60);
    }
    else if (dt == 15) {
        duedate = getduedate(90);
    }
    else if (dt == 16) {
        duedate = getduedate(120);
    }
    else if (dt == 17) {
        duedate = getduedate(150);
    }
    else if (dt == 18) {
        duedate = getduedate(180);
    }
    else if (dt == 19) {
        duedate = getduedate(210);
    }
    else if (dt == 20) {
        duedate = getduedate(240);
    }
    else if (dt == 21) {
        duedate = getduedate(270);
    }
    else if (dt == 22) {
        duedate = getduedate(300);
    }
    else if (dt == 23) {
        duedate = getduedate(330);
    }
    else if (dt == 24) {
        duedate = getduedate(365);
    }
    else if (dt == 25) {
        duedate = getduedate(730);
    }
    else if (dt == 111) {
        duedate = getduedate(1095);
    }
    else if (dt == 154) {
        duedate = getduedate(1460);
    }
    else {
        duedate = getduedate(0);
    }
    return duedate;
}

function DudeDateCalculation(dt) {

    var month;
    if (dt == 13) {
        month = 1;
    }
    else if (dt == 14) {
        month = 2;
    }
    else if (dt == 15) {
        month = 3;
    }
    else if (dt == 16) {
        month = 4;
    }
    else if (dt == 17) {
        month = 5;
    }
    else if (dt == 18) {
        month = 6;
    }
    else if (dt == 19) {
        month = 7;
    }
    else if (dt == 20) {
        month = 8;
    }
    else if (dt == 21) {
        month = 9;
    }
    else if (dt == 22) {
        month = 10;
    }
    else if (dt == 23) {
        month = 11;
    }
    else if (dt == 24) {
        month = 12;
    }
    else if (dt == 25) {
        month = 24;
    }
    else if (dt == 111) {
        month = 36;
    }
    else if (dt == 154) {
        month = 48;
    }
    else {
        month = 0;
    }
    return month;
}

function getduedate(dt) {
    //debugger;
    var nowDate = new Date();
    //console.log(nowDate);
    var addDate = new Date(nowDate.setDate(nowDate.getDate() + dt));
    //console.log(addDate);			
    let lastDay = new Date(nowDate.getFullYear(), nowDate.getMonth() + 1, 0);
    //console.log(lastDay);
    var nowDay = (lastDay.getDate());
    //var nowDay = ((lastDay.getDate().toString().length) == 1) ? '0' + (lastDay.getDate()) : (lastDay.getDate());
    //var nowMonth = ((lastDay.getMonth().toString().length) == 1) ? '0' + (lastDay.getMonth() + 1) : (lastDay.getMonth() + 1);
    var nowMonth = (lastDay.getMonth() + 1);
    var nowYear = lastDay.getFullYear();
    var formatDate = nowYear + "-" + nowMonth + "-" + nowDay + " " + addDate.getHours() + ":" + addDate.getMinutes() + ":" + addDate.getSeconds();

    //console.log(formatDate);
    return formatDate;
}

function AcceptRequest(type, lang) {
  //  $('#dvload').show();
    var data;
    var Id = $('#RequestCalibId').val();

    var dueDate;
    var dt = $('#CalibFreq').val();
    dueDate = DudeDateCalculation(dt);
    if ($('#MasterInstrument1').val() == '' && $('#MasterInstrument2').val() == '' && $('#MasterInstrument3').val() == '' && $('#MasterInstrument4').val() == '') {
        showWarning('Please Select and Add the Master Instrument Name...!', lang);
        return false;
    }

    data = {
        requestId: $('#RequestCalibId').val(),
        ReceivedBy: $('#ReceivedBy').val(),
        InstrumentCondition: $('#InstrumentCondition').val(),
        ToolInventory: $('#ToolInventory').val(),
        Scope: $('#Scope').val(),
        TentativeCompletionDate: $('#TentativeCompletionDate').val(),
        newNABL: $('#IsNABL').val(),
        CalibFreq: $('#CalibFreq').val(),
        newObservation: $('#NewObservation').val(),
        newObservationType: $('#NewObservationType').val(),
        newMU: $('#NewMU').val(),
        newCertification: $('#NewCertification').val(),
        standardReffered: $('#StdReffer').val(),
        MasterInstrument1: $('#MasterInstrument1').val(),
        MasterInstrument2: $('#MasterInstrument2').val(),
        MasterInstrument3: $('#MasterInstrument3').val(),
        MasterInstrument4: $('#MasterInstrument4').val(),
        DueDate: dueDate
    }
    $.ajax({
        url: '../Tracker/AcceptRequest',
        type: 'POST',
        data: data
    }).done(function (resultObject) {
       
        window.location.href = '../Tracker/Request?reqType=4';
        //$('#dvload').hide();
        showSuccess("You are accepted the request. Department User get notified!", lang);
    });
}

function AcceptRequestRecalibration(AcceptValue, lang) {
    var data;
    var Id = $('#RequestCalibId').val();
    var departmentId = $('#hdnDepartmentId').val();
    data = {
        requestId: $('#RequestCalibId').val(),
        AcceptValue: AcceptValue,
        departmentId: departmentId
    }
    $.ajax({
        url: '../Tracker/AcceptRequestReCalibration',
        type: 'POST',
        data: data
    }).done(function (resultObject) {
        window.location.href = '../Tracker/RequestDetailsNew?Id=' + resultObject.id + '';
        showSuccess("You are accepted the request. Department User get notified!", lang);
    });
}

function RejecttRequest(type, lang) {
    var data;
    if (type == 1) {
        data = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            InstrumentCondition: $('#InstrumentCondition').val(),
            Scope: $('#Scope').val(),
            ToolInventory: $('#ToolInventory').val(),
            TentativeCompletionDate: $('#TentativeCompletionDate').val(),
            rejectReason: $('#Newreason').val(),
            standardReffered: $('#StandardReffered').val()
        };
    } else if (type == 2) {
        data = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            InstrumentCondition: $('#InstrumentCondition').val(),
            Scope: $('#Scope').val(),
            ToolInventory: $('#ToolInventory').val(),
            TentativeCompletionDate: $('#TentativeCompletionDate').val(),
            rejectReason: $('#Newreason').val(),
            standardReffered: $('#StandardReffered').val()
        }
    }

    $.ajax({
        url: '../Tracker/RejectRequest',
        type: 'POST',
        data: data
    }).done(function (resultObject) {
        // if (resultObject.TypeOfRequest == 1) {
        //     AssignNewRequestValues(resultObject);
        // } else {
        //     AssignRequestValues(resultObject);
        // }
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("You are rejected the request. LAB admin get notified!", lang);
    });
}

function SubmitReqDepVisual(lang) {

    if ($('#ResultDEP').val() == '' || $('#ResultDEP').val() == undefined) {
        $('#ResultDEP').addClass('is-invalid');
        return false;
    } else {
        $('#ResultDEP').removeClass('is-invalid');

    }
    debugger;
    $.ajax({
        url: '../Tracker/SubmitDepartmentRequestVisual',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), Result: $('#ResultDEP').val() }
    }).done(function (resultObject) {
        AssignRequestValues(resultObject);
        showSuccess("Your visual check details recorded", lang);
    });
}
function disableFields() {
    document.getElementById("NewObservation").disabled = true;
    document.getElementById("NewObservationType").disabled = true;
    document.getElementById("NewMU").disabled = true;
    document.getElementById("NewCertification").disabled = true;
    document.getElementById("newSubmitLABAdmin").disabled = true;
}

function SaveLABAdminUpdates(lang) {
    $.ajax({
        url: '../Tracker/SubmitLABAdminUpdates',
        type: 'POST',
        data: {
            requestId: $('#RequestCalibId').val(),
            ObservationTemplate: $('#NewObservationEd').val(),
            ObservationTemplateType: $('#NewObservationTypeEd').val(),
            //MUTemplate: $('#NewMU').val(),
            CertificationTemplate: $('#NewCertificationEd').val()

        }
    }).done(function (resultObject) {
        AssignRequestValues(resultObject);
        showSuccess("Certificate Updated Successfully", lang);
        window.location.href = '../Tracker/Request?reqType=4';
        // disableFields();
    });
}
function SubmitReqLABVisual(lang) {

    if ($('#ResultLAB').val() == '' || $('#ResultLAB').val() == undefined) {
        $('#ResultLAB').addClass('is-invalid');
        return false;
    } else {
        $('#ResultLAB').removeClass('is-invalid');
    }

    $.ajax({
        url: '../Tracker/SubmitLABRequestVisual',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), Result: $('#ResultLAB').val() }
    }).done(function (resultObject) {
        AssignRequestValues(resultObject);
        showSuccess("Your visual check details recorded", lang);
    });
}



//New Instrument Section  

function AssignNewRequestValues(resultObject) {
    if (resultObject != null) {
        $('#newResultLAB').val(resultObject.resultLAB);
        $('#newResultDEP').val(resultObject.resultDEP);
        $('#newReturnToLab').val(resultObject.returnToLab);
        $('#newVisualCheckLAB').val(resultObject.visualCheckLab);

        $('#RequestCalibId').val(resultObject.id);
        $('#NewReqestNo').val(resultObject.reqestNo);
        $('#NewRequestDate').val(resultObject.requestDate);
        $('#NewInstrumentName').val(resultObject.instrumentName);
        $('#NewInstrumentSerialNumber').val(resultObject.instrumentSerialNumber);
        $('#NewReqestBy').val(resultObject.reqestBy);
        $('#NewUserDept').val(resultObject.departmentName);
        $('#NewRange').val(resultObject.range);
        $('#NewUnit1').val(resultObject.unit1);
        $('#NewUnit2').val(resultObject.unit2);
        $('#NewUnit3').val(resultObject.unit3);
        $('#NewLC').val(resultObject.lc);
        $('#NewTW_Type').val(resultObject.tw);
        $('#NewMake').val(resultObject.make);
        $('#NewRefCalibration').val(resultObject.standardReffered1);
        $('#NewCalibrationFrequency').val(resultObject.calibFrequency);
        if (resultObject.typeOfRequest == 1) {
            $('#NewTypeOfRequest').val('New');
        } else if (resultObject.typeOfRequest == 2) {
            $('#NewTypeOfRequest').val('Regular');
        } else if (resultObject.typeOfRequest == 3) {
            $('#NewTypeOfRequest').val('Recalibration');
        } else if (resultObject.typeOfRequest == 4) {
            $('#NewTypeOfRequest').val('UnQuarantine');
        }
        $('#Instrument_Type').val(resultObject.instrument_Type);
        $('#Drawing_Attached').val(resultObject.drawing_Attached);
        $('#Rule_Confirmity').val(resultObject.rule_Confirmity);

        $('#NewrejectedOn').val(resultObject.submittedOn);
        $('#NewrejectedReason').val(resultObject.rejectReason);
        $('#status_newrequested').css('display', 'none');
        $('#status_newaccepted').css('display', 'none');
        $('#newRequestSection').css('display', 'none');
        $('#NewrejectSection').css('display', 'none');
        if (resultObject.status == 28) {
            $('#status_newrejected').css('display', 'block');
            $('#NewrejectSection').css('display', 'block');
        } else if (resultObject.status == 26) {
            $('#status_newrequested').css('display', 'block');
            if ($('#userRoleId').val() == 2) {
                $('#newRequestSection').css('display', 'block');
            }
        } else if (resultObject.status == 27) {
            $('#status_newaccepted').css('display', 'block');
            $('#newRequestSection').css('display', 'block');
            $('#NewacceptSection').css('display', 'block');

        } else if (resultObject.status == 29) {

            if ($('#userRoleId').val() == 1) {
                $('#newlabVisualCheckSection').css('display', 'block');
                $('#newReturnToLAB').removeAttr('disabled');
                $('#newVisualCheckLAB').removeAttr('disabled');

            }
            $('#newRecordBy').attr('disabled', 'disabled');
            $('#newResultLAB').attr('disabled', 'disabled');
            $('#newClosedDate').attr('disabled', 'disabled')
            $('#newSubmitFMVisual').css('display', 'none');
            $('#newRequestSection').css('display', 'block');
            $('#NewacceptSection').css('display', 'block');
        } else if (resultObject.status == 30) {
            $('#newRequestSection').css('display', 'block');
            $('#NewacceptSection').css('display', 'block');
            $('#newRecordBy').attr('disabled', 'disabled');
            $('#newResultLAB').attr('disabled', 'disabled');
            $('#newClosedDate').attr('disabled', 'disabled')
            $('#newSubmitFMVisual').css('display', 'none');
            $('#newReturnDate').attr('disabled', 'disabled');
            $('#newResultDEP').attr('disabled', 'disabled');
            $('#newSubmitDEPVisual').css('display', 'none');
            $('#newlabVisualCheckSection').css('display', 'block');
        }

        if (resultObject.receivedBy != null) {
            $('#ReceivedBy').val(resultObject.receivedByName);
            if ($('#ReceivedDate').val() != null) {
                $('#ReceivedDate').val(resultObject.receivedDate);
            }
            $('#InstrumentCondition').val(resultObject.instrumentCondition);
            $('#Feasiblity').val(resultObject.feasiblity);
            $('#TentativeCompletionDate').val(resultObject.tentativeCompletionDate);

            $('#ReceivedBy').attr('disabled', 'disabled');
            $('#ReceivedDate').attr('disabled', 'disabled');
            $('#InstrumentCondition').attr('disabled', 'disabled');
            $('#Feasiblity').attr('disabled', 'disabled');
            $('#TentativeCompletionDate').attr('disabled', 'disabled');
            $('#submitReasonNewRequest').css('display', 'none');
            $('#AcceptSection').css('display', 'none');
            $('#RejectSection').css('display', 'none');
            if (resultObject.instrumentIdNo != null) {
                if (resultObject.resultLAB == null) {
                    $('#newResultLAB').removeAttr('disabled');
                } else {
                    $('#newResultLAB').attr('disabled', 'disbaled');
                }

                $('#submitInsDetails').css('display', 'none');
                $('#NewLabId').val(resultObject.instrumentIdNo);
                //$('#NewNABL').val(resultObject.isNABL);
                $('#NewObservation').val(resultObject.observationTemplate.toString());
                $('#NewObservationType').val(resultObject.observationType.toString());
                $('#NewMU').val(resultObject.muTemplate.toString());
                $('#NewCertification').val(resultObject.certificationTemplate.toString());
                $('#CalibSource').val(resultObject.calibSource);
                $('#StandardReffered').val(resultObject.standardReffered);
                $('#CalibDate').val(resultObject.calibDate);
                $('#DueDate').val(resultObject.dueDate);
                $('#DateOfReceipt').val(resultObject.dateOfReceipt);

                $('#NewLabId').attr('disabled', 'disabled');
                $('#NewNABL').attr('disabled', 'disabled');
                $('#NewObservation').attr('disabled', 'disabled');
                $('#NewObservationType').attr('disabled', 'disabled');
                $('#NewMU').attr('disabled', 'disabled');
                $('#NewCertification').attr('disabled', 'disabled');
                $('#CalibSource').attr('disabled', 'disabled');
                $('#StandardReffered').attr('disabled', 'disabled');
                $('#CalibDate').attr('disabled', 'disabled');
                $('#DueDate').attr('disabled', 'disabled');
                $('#DateOfReceipt').attr('disabled', 'disabled');
                $('#newfmVisualCheckSection').css('display', 'block');

            }
        }

    }
}

function CloseNewRequestPopup() {
    $("#NewInstrument").modal('hide');
}

function NewReqEnableReason1() {
   // alert($('input[name = "NewAcceptReject"]: checked').val());
    if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
       
        $('#NewReasonSection').css('display', 'none');
        $('#NewacceptSection').css('display', 'block');
        $('#ExternalRejectSection').css('display', 'none');
        $('#ExternalAcceptSection').css('display', 'block');
    } else {
        $('#ExternalAcceptSection').css('display', 'none');
        $('#NewReasonSection').css('display', 'block');
        $('#NewacceptSection').css('display', 'none');
        $('#ExternalRejectSection').css('display', 'block');

    }
}

function AcceptRejectNewRequest(lang) {

    var type = $('#hdntype').val();
    if ($('input[name="NewAcceptReject"]:checked').val() == undefined || $('input[name="NewAcceptReject"]:checked').val() == '') {
        showWarning("Please choose either Accept / Reject and try again.", lang);
    } else if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
        if (ValidateCheck() == 0) {
            AcceptRequest(type, lang);
        }
        else {
            showWarning('Please Select Tentative Closing Date, Observation Templates, Standard Refered/Calibration Method, Calibration Frequency Values...!', lang);
        }

    } else {
        if ($('#Newreason').val() != '' && $('#TentativeCompletionDate').val() != '') {
            $('#Newreason').removeClass('is-invalid');
            RejecttRequest(type, lang);
        } else {
            $('#Newreason').addClass('is-invalid');
            showWarning("Please enter Tentative Closing Date & reason for rejection and try again.", lang);
        }
    }
}

function AcceptRejectExternalRequest(lang) {
   // debugger;
    var type = $('#hdntype').val();
    if ($('input[name="NewAcceptReject"]:checked').val() == undefined || $('input[name="NewAcceptReject"]:checked').val() == '') {
        showWarning("Please choose either Accept / Reject and try again.", lang);
    } else if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
        if ($('#CalibFreq').val() != '' && $('#Acceptreason').val().trim() != '' && $('#ImageUpload')[0].files.length != 0 && $('#StdReffer').val().trim() != '') {
            ExternalAcceptRequest(type, lang);
        }
        else {
            showWarning('Please fill the Calibration Frequency and Accept Reason and Upload Image...!', lang);
        }

    } else {
        if ($('#Newreason').val() != '') {
            $('#Newreason').removeClass('is-invalid');
            ExternalRejecttRequest(type, lang);
        } else {
            $('#Newreason').addClass('is-invalid');
            showWarning("Please enter reason for rejection and try again.", lang);
        }
    }
}

function ExternalRejecttRequest(type, lang) {
   // debugger;
    console.log($('#TentativeCompletionDate').val());
    var data1;
    if (type == 1) {
        data1 = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            InstrumentCondition: $('#InstrumentCondition').val(),
            Feasiblity: $('#Feasiblity').val(),
            TentativeCompletionDate: $('#TentativeCompletionDate').val(),
            rejectReason: $('#Newreason').val(),
            standardReffered: $('#StandardReffered').val()
        };
    } else if (type == 2) {
        data1 = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            InstrumentCondition: $('#InstrumentCondition').val(),
            Feasiblity: $('#Feasiblity').val(),
            TentativeCompletionDate: $('#TentativeCompletionDate').val(),
            rejectReason: $('#Newreason').val(),
            standardReffered: $('#StandardReffered').val()
        }
    }
    console.log(data1);
    $.ajax({
        type: 'POST',
        url: '../Tracker/ExternalRejectRequest',
        data: data1,
        dataType: 'json',
        success: function (data) {
            window.location.href = '../Tracker/Request?reqType=4';
            showSuccess("You are rejected the External request. LAB admin get notified!", lang);
        },
        error: function () {
            alert('error');
        }
    });
}

function ExternalAcceptRequest(type, lang) {
    var data1;
    var formData = new FormData();
    var Id = $('#RequestCalibId').val();
    //var files = $("#ImageUpload")[0].files;

    var file = $('#ImageUpload')[0].files[0];
    //formData.append("file", file);
    //data1 = {
    //    requestId: $('#RequestCalibId').val(),
    //    ReceivedBy: $('#ReceivedBy').val(),
    //    InstrumentCondition: $('#InstrumentCondition').val(),
    //    Feasiblity: $('#Feasiblity').val(),
    //    TentativeCompletionDate: $('#TentativeCompletionDate').val(),
    //    newNABL: $('#IsNABL').val(),
    //    InstrumentIdNo: $('#txtIdNo').val(),
    //    rejectReason: $('#Newreason').val(),
    //    photo: FileData
    //}

    var dueDate;
    var dt = $('#CalibFreq').val();
    dueDate = DudeDateCalculation(dt);
    console.log('duedate');
    console.log(dueDate);

    formData.append("requestId", $('#RequestCalibId').val());
    formData.append("ReceivedBy", $('#ReceivedBy').val());
    formData.append("InstrumentCondition", $('#InstrumentCondition').val());
    formData.append("Feasiblity", $('#Feasiblity').val());
    formData.append("TentativeCompletionDate", $('#TentativeCompletionDate').val());
    formData.append("newNABL", $('#IsNABL').val());
    formData.append("InstrumentIdNo", $('#txtIdNo').val());
    formData.append("acceptReason", $('#Acceptreason').val());
    formData.append("httpPostedFileBase", file);
    formData.append("StandardReffered", $('#StdReffer').val());
    formData.append("CalibFreq", $('#CalibFreq').val());
    formData.append("DueDate", dueDate);

    ////formData.append("data", JSON.stringify(data1));

    //formData.append("httpPostedFileBase", $("#ImageUpload")[0].files[0]);
    //var totalFiles = document.getElementById('ImageUpload').files.length;
    //for (var i = 0; i < totalFiles; i++) {
    //    var file = document.getElementById('ImageUpload').files[i];
    //    formData.append("httpPostedFileBase", file);
    //}
    $.ajax({
        type: 'POST',
        url: '../Tracker/ExternalAcceptRequest',
        data: formData,
        dataType: 'json',
        processData: false,
        contentType: false,
        success: function (data) {
            window.location.href = '../Tracker/Request?reqType=4';
            showSuccess("You are rejected the External request. LAB admin get notified!", lang);
        },
        error: function () {
            alert('error');
        }
    });
}

function ValidateCheckForExternal() {
    debugger;
    var errCount = 0;
    $('#txtIdNoError').hide();
    if (($('#txtIdNo').val()) == '') {
        errCount = errCount + 1;
        $('#txtIdNoError').show();
    }
    return errCount;
}

function AcceptRejectReCalibrationRequest(lang) {
    var type = $('#hdntype').val();
    if ($('input[name="NewAcceptReject"]:checked').val() == undefined || $('input[name="NewAcceptReject"]:checked').val() == '') {
        showWarning("Please choose either Accept / Reject and try again.");
    } else if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
        AcceptRequestRecalibration(1, lang);
    } else {
        AcceptRequestRecalibration(0, lang);
    }
}

// Instrument UnQuarantine Section
function AssignQuarRequestValues(resultObject) {
    if (resultObject != null) {
        $('#RequestCalibId').val(resultObject.id);
        $('#QuarReqestNo').val(resultObject.reqestNo);
        $('#QuarRequestDate').val(resultObject.requestDate);
        $('#QuarInstrumentName').val(resultObject.instrumentName);
        $('#QuarInstrumentSerialNumber').val(resultObject.instrumentSerialNumber);
        $('#QuarReqestBy').val(resultObject.reqestBy);
        $('#QuarUserDept').val(resultObject.departmentName);
        $('#QuarRange').val(resultObject.range);
        if (resultObject.typeOfRequest == 1) {
            $('#QuarTypeOfRequest').val('New');
        } else if (resultObject.typeOfRequest == 2) {
            $('#QuarTypeOfRequest').val('Regular');
        } else if (resultObject.typeOfRequest == 3) {
            $('#QuarTypeOfRequest').val('Recalibration');
        } else if (resultObject.typeOfRequest == 4) {
            $('#QuarTypeOfRequest').val('UnQuarantine');
        }
        $('#QuarrejectedOn').val(resultObject.submittedOn);
        $('#QuarrejectedReason').val(resultObject.rejectReason);
        $('#status_Quaraccepted').css('display', 'none');
        $('#status_Quarrejected').css('display', 'none');
        $('#QuarrejectSection').css('display', 'none');
        $('#status_Quarrequested').css('display', 'none');
        $('#QuarRequestSection').css('display', 'none');

        if (resultObject.status == 28) {
            $('#status_Quarrejected').css('display', 'block');
            $('#QuarrejectSection').css('display', 'block');
        } else if (resultObject.status == 26) {
            $('#status_Quarrequested').css('display', 'block');
            if ($('#userRoleId').val() == 2) {
                $('#QuarRequestSection').css('display', 'block');
            }
        } else if (resultObject.status == 27) {
            $('#status_Quaraccepted').css('display', 'block');
        }
    }
}

function CloseQuarRequestPopup() {
    $("#QuarInstrument").modal('hide');
}

function QuarReqEnableReason() {
    if ($('input[name="QuarAcceptReject"]:checked').val() == 'Accept') {
        $('#QuarReasonSection').css('display', 'none');
    } else {

        $('#QuarReasonSection').css('display', 'block');

    }
}

function AcceptRejectQuarRequest() {
    if ($('input[name="QuarAcceptReject"]:checked').val() == undefined || $('input[name="QuarAcceptReject"]:checked').val() == '') {
        showWarning("Please choose either Accept / Reject and try again.");
    } else if ($('input[name="QuarAcceptReject"]:checked').val() == 'Accept') {
        AcceptQuarRequest();
    } else {
        if ($('#Quarreason').val() != '') {
            $('#Quarreason').removeClass('is-invalid');
            RejecttQuarRequest();
        } else {
            $('#Quarreason').addClass('is-invalid');
            showWarning("Please enter reason for rejection and try again.");
        }
    }
}

function AcceptQuarRequest() {
    $.ajax({
        url: '../Tracker/ApproveQuarRequest',
        type: 'POST',
        data: {
            requestId: $('#RequestCalibId').val()
        }
    }).done(function (resultObject) {
        AssignQuarRequestValues(resultObject);
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "You are accepted the request. Department User get notified!",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });

    });
}

function RejecttQuarRequest() {
    $.ajax({
        url: '../Tracker/RejectRequest',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), rejectReason: $('#Quarreason').val() }
    }).done(function (resultObject) {
        AssignQuarRequestValues(resultObject);
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "You are rejected the request. LAB admin get notified!",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });

    });
}



function LoadObservationType(lang) {

    var optionhtmlOBType;
    $.ajax({
        url: '../Tracker/LoadObservationType',
        type: 'POST',
        data: { attrType: '', attrsubType: $('#ObservationTemplate option:selected').text(), LangType: lang }
    }).done(function (resultObject) {
        $('#ObservationType')
            .find('option')
            .remove();
        $('#ObservationType').empty();
        if (lang == 'en') {
            optionhtmlOBType = '<option value="">--Select Observation Type--</option>';
        }
        else {
            optionhtmlOBType = '<option value="">--観測タイプの選択--</option>';
        }
        $('#ObservationType').append(optionhtmlOBType);
        for (let index = 0; index < resultObject.length; index++) {

            optText = resultObject[index].attrValue;
            optValue = resultObject[index].id;

            optTextJp = resultObject[index].attrValueJp;


            if (lang == 'en') {

                $('#ObservationType').append(`<option value="${optValue}">${optText}</option>`);
            }
            else {

                $('#ObservationType').append(`<option value="${optValue}">${optTextJp}</option>`);
            }

        }

    });

}

function SaveLeverDial(lang) {
    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWI').val(),
        Allvalues: $('#Allvalues').val(),
        MeasuringRangeSpec: $('#MeasuringRange').val(),
        MeasuringRangeDirectionA1: $('#MeasuringRangeDirectionA1').val(),
        MeasuringRangeDirectionB1: $('#MeasuringRangeDirectionB1').val(),
        ScaleDivisionSpec: $('#ScaleDivisionSpec').val(),
        ScaleDivisionDirectionA2: $('#ScaleDivisionDirectionA2').val(),
        ScaleDivisionDirectionB2: $('#ScaleDivisionDirectionB2').val(),
        HysteresisSpec: $('#HysteresisSpec').val(),
        HysteresisDirectionA3: $('#HysteresisDirectionA3').val(),
        HysteresisDirectionB3: $('#HysteresisDirectionB3').val(),
        RepeatabilitySpec: $('#RepeatabilitySpec').val(),
        RepeatabilityDirectionA4: $('#RepeatabilityDirectionA4').val(),
        RepeatabilityDirectionB4: $('#RepeatabilityDirectionB4').val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        CalibrationReviewedBy: $('#CalibrationReviewedBy').val(),
        // CalibrationPerformedDate:$('#CalibrationPerformedDate').val(),
        // CalibrationReviewedDate:$('#CalibrationReviewedDate').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        DialIndicatiorCondition: $('#DialIndicatiorCondition').val(),
    }
    $.ajax({
        url: '../Observation/InsertLeverDial',
        type: 'POST',
        data: { levertypedial: data }

    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Data Saved Successfully", lang);
    });
}

 function SaveInventoryCalibration(DueMonth,lang) {
    //var DueMonth = @Json.Serialize(@ViewBag.DueMonth); 
    //alert(DueMonth);
    var sPageURLPaarameter = window.location.search.substring(1);
    var InstrumentDataList = new Array();
    var CheckedCount = 0;
    var UNCheckedCount = 0;
    var tblLength = $('#tblTool tbody tr').length;
    var UserDept = 0;

    if ($('#tblTool tbody tr').length > 0) {
        $('#tblTool tbody tr').each(function (row, tr) {

            var checkedvalue = $(tr).find("input[name=ChkInput]").prop('checked');
            var objInstrumentId = $(tr).find("input[name=instrumentid]").val();//$(tr).find("td[id='instrumentid'] input[type='hidden']").val();
            var POPoutId = "popupcount_" + objInstrumentId;
            var CHKoutId = "ChkInput_" + objInstrumentId;
            var Masterid = "ReplacementLabID_" + objInstrumentId;
            //var ReplaceRequestId = "HiddenRequestId_" + objInstrumentId;
           // var objReplacementLabId = $(tr).find("td[id='" + Masterid + "']").text().trim();
            var objPopUpRecordCount = $(tr).find("input[id='" + POPoutId + "']").val(); // $(tr).find("input[id= '" + ChkoutId + "']").val().trim();
            var objChkRecordCount = $(tr).find("input[id='" + CHKoutId + "']").prop('checked');
           // var objReplaceRequestId = $(tr).find("input[id='" + ReplaceRequestId + "']").val();
            var DueDate = $(this).closest('tr').find('.clsDueDate').val();
            var CalibFreq = $(this).closest('tr').find('.clsCalibFreq').val();
            var UserDept = $(this).closest('tr').find('.clsUserDept').val();// $(tr).find("input[name=UserDept]").val();
            var RequestId = $(this).closest('tr').find('.clsRequestId').val();
            var objReplacementLabId = $(this).closest('tr').find('.clsLabId').val();
            var objIdNo = $(this).closest('tr').find('.clsIdno').val();
            var ReplacementDeptId = $(this).closest('tr').find('.clsReplacementDeptId').val(); //clsReplacementDeptId
            //alert(objIdNo);
            if (checkedvalue == true) {

                if ((objReplacementLabId == "") && (objPopUpRecordCount > 0)) {
                    CheckedCount += 1;
                }
                else if ((objReplacementLabId == "") && (objPopUpRecordCount == "")) {

                    CheckedCount += 1;
                }
                else if (objReplacementLabId == "") {

                    CheckedCount += 1;
                }
                else {
                   // alert("message");
                    var InstrumentData =
                    {
                        InstrumentId: objInstrumentId,
                        ReplacementLabId: objReplacementLabId,
                        DueMonth: DueMonth,
                        CalibFrequency: CalibFreq,
                        UserDept: UserDept,
                        RequestId: RequestId,
                        IdNo: objIdNo,
                        DueDate: DueDate,
                        ReplacementDeptId:ReplacementDeptId,
                    }

                    InstrumentDataList.push(InstrumentData);
                }
            }

            else if ((checkedvalue == false) && (objReplacementLabId == "") && (objPopUpRecordCount == 0)) {
                UNCheckedCount += 1;
            }
            
        });


    }
    console.log("InstrumentDataList");
     console.log(InstrumentDataList);
     //alert(InstrumentDataList);

    if ((InstrumentDataList.length > 0) && (CheckedCount == 0)) {

        $.ajax({
            dataType: 'json',
            url: '../Instrument/SaveInventoryCalibration',
            type: 'POST',
            data: { InstrumentList: InstrumentDataList },
            success: function () {


                window.location.href = '../Instrument/ToolInventory?' + sPageURLPaarameter + '';

                showSuccess("Data Saved Successfully", lang);
            },
            failure: function (response) {
                showWarning("Try Again Process Failed", lang);

            }

        });
    }
    if (CheckedCount > 0) {
        showWarning("Please Select the Replacement-LabId", lang);
        return false;
    }
    if (UNCheckedCount == tblLength) {
        showWarning("Please Select the Replacement-LabId", lang);
        return false;
    }
}
function DueForCalibrationInstruments_olds() {

    var tblRowsCoun = $("#example1 th").length;
    if (tblRowsCoun > 0) {
        $('#example1 > tbody > tr').each(function (row, tr) {
            var currentRow = $(this).closest("tr");



            if (currentRow.find("td:eq(8)").text() != " ") {
                var checkedvalue = $(tr).find("td:eq(8) input[type='checkbox']")[0].checked;

                currentRow.show();
            }
            else {
                currentRow.hide();
            }

        });
    }

}

function InsertRequestListold() {
    var Request = new Array();
    $('#example1 > tbody > tr').each(function (row, tr) {

        var currentRow = $(this).closest("tr");


        if (currentRow.find("td:eq(8)").text() != " ") {

            var checkedvalue = $(tr).find("td:eq(8) input[type='checkbox']")[0].checked;

            if (checkedvalue == true) {


                var TypeValue = $(tr).find("td:eq(9) input[type='hidden']").val();


                var RequestData = {
                    instrumentId: checkedvalue,
                    typeId: TypeValue
                }
                Request.push(RequestData);
            }

        }


    });
    $.ajax({
        url: '../Instrument/DueRequest',
        type: 'POST',
        data: { Request }// JSON.stringify(ToolInventoryList);//{ ToolInventoryList: ToolInventoryList }
    }).done(function (resultObject) {

        showSuccess("Data Saved Successfully", lang);

        $.ajax({
            url: '../Instrument/MailInsertDueRequest',
            type: 'POST',
            data: { userViewModelList: Request },
            dataType: "json",
        }).done(function (resultObject) {
            showSuccess("Data Saved Successfully");
        });
        window.location.href = '../Instrument/ToolInventory';

    });


}

function SaveMicrometer(lang) {
    var MicroResult;
    var ObjParent = '';

    var Unit = $('#Allvalues').val();
    if (Unit == null || Unit == "") {
        showWarning("Please enter the Unit !!!", lang);
        return true;
    }

    var Temprature = $('#TempStart').val();
    if (Temprature == null || Temprature == "") {
        showWarning("Please enter the Temprature !!!", lang);
        return true;
    }
    var Humidity = $('#Humidity').val();
    if (Humidity == null || Humidity == "") {
        showWarning("Please enter the Humidity !!!", lang);
        return true;
    }
    var MicrometerCondition = $('#MicrometerCondition').val();
    if (MicrometerCondition == null || MicrometerCondition == "") {
        showWarning("Please enter the Visual Check !!!", lang);
        return true;
    }
    var MeasuredValue;
    var Actualvalue;

    MicroResult = new Array();
    $('#Microadd tbody tr').each(function (row, tr) {



        if ($(tr).find("td:eq(1) input[type='text']").val() == 0 || $(tr).find("td:eq(1) input[type='text']").val() == '') {
            MeasuredValue = 1;

        }
        if ($(tr).find("td:eq(2) input[type='text']").val() == 0 || $(tr).find("td:eq(1) input[type='text']").val() == '') {
            Actualvalue = 1;

        }

        if ($('#Microadd >tbody >tr').length > 2) {

            if ($(tr).index() > 2) {
                ObjParent = $(tr).find("td:eq(5) input[type='hidden']").val() || null || '' ? $('#TemplateObservationId').val() : $(tr).find("td:eq(5) input[type='hidden']").val();

                if ($(tr).find("td:eq(1) input[type='text']").val() > 0) {

                    var MicroData = {
                        SNO: $(tr).find("td:eq(0) input[type='text']").val(),
                        MeasuedValue: $(tr).find("td:eq(1) input[type='text']").val(),
                        ActualsT1: $(tr).find("td:eq(2) input[type='text']").val(),
                        Diff1: $(tr).find("td:eq(3) input[type='text']").val(),
                        Id: $(tr).find("td:eq(4) input[type='hidden']").val(),
                        ParentId: ObjParent,
                        InstrumentError: 1//$(tr).find("td:eq(6) input[type='hidden']").val()

                    }

                }
                MicroResult.push(MicroData);
            }

        }
        else {
            MicroResult.pop(MicroData);
        }




    });
    //To validated the tbl values
    //if (MeasuredValue == 1)
    //{
    //showWarning("Please enter the MeasuedValue in the Table!!!", lang);
    //return true;
    //}
    //if (Actualvalue == 1)
    //{
    //showWarning("Please enter the Actual Value in the Table!!!", lang);
    //    return true;
    //}

    var data = {
        Id: $('#IdMicro').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        TempStart: $('#TempStart').val(),
        //  TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWi').val(),
        Allvalues: $('#Allvalues').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        MicrometerCondition: $('#MicrometerCondition').val(),

        Avg1: $('#Avg1').val(),
        ActualsT11: $('#ActualsT11').val(),
        MuInterval1: $('#MuInterval1').val(),
        //ActualsT21: $('#ActualsT21').val(),
        //ActualsT31: $('#ActualsT31').val(),


        //ActualsT22: $('#ActualsT22').val(),
        //ActualsT32: $('#ActualsT32').val(),
        Avg2: $('#Avg2').val(),
        ActualsT12: $('#ActualsT12').val(),
        MuInterval2: $('#MuInterval2').val(),

        //ActualsT23: $('#ActualsT23').val(),
        //ActualsT33: $('#ActualsT33').val(),
        Avg3: $('#Avg3').val(),
        ActualsT13: $('#ActualsT13').val(),
        MuInterval3: $('#MuInterval3').val(),

        Avg4: $('#Avg4').val(),
        ActualsT14: $('#ActualsT14').val(),
        Measurement4: $('#Measurement4').val(),

        Avg5: $('#Avg5').val(),
        ActualsT15: $('#ActualsT15').val(),
        Measurement5: $('#Measurement5').val(),

        Avg6: $('#Avg6').val(),
        ActualsT16: $('#ActualsT16').val(),
        Measurement6: $('#Measurement6').val(),

        Avg7: $('#Avg7').val(),
        ActualsT17: $('#ActualsT17').val(),
        Measurement7: $('#Measurement7').val(),

        //ActualsT13: $('#ActualsT13').val(),
        //ActualsT23: $('#ActualsT23').val(),
        //ActualsT33: $('#ActualsT33').val(),
        //Avg3: $('#Avg3').val(),
        //MuInterval3: $('#MuInterval3').val(),
        //ActualsT14: $('#ActualsT14').val(),
        //ActualsT24: $('#ActualsT24').val(),
        //ActualsT34: $('#ActualsT34').val(),
        //Avg4: $('#Avg4').val(),
        //MuInterval4: $('#MuInterval4').val(),
        //ActualsT15: $('#ActualsT15').val(),
        //ActualsT25: $('#ActualsT25').val(),
        //ActualsT35: $('#ActualsT35').val(),
        //Avg5: $('#Avg5').val(),
        //MuInterval5: $('#MuInterval5').val(),
        //ActualsT16: $('#ActualsT16').val(),
        //ActualsT26: $('#ActualsT26').val(),
        //ActualsT36: $('#ActualsT36').val(),
        //Avg6: $('#Avg6').val(),
        //ActualsT17: $('#ActualsT17').val(),
        //ActualsT27: $('#ActualsT27').val(),
        //ActualsT37: $('#ActualsT37').val(),
        //Avg7: $('#Avg7').val(),
        //ActualsT18: $('#ActualsT18').val(),
        //ActualsT28: $('#ActualsT28').val(),
        //ActualsT38: $('#ActualsT38').val(),
        //Avg8: $('#Avg8').val(),
        //ActualsT19: $('#ActualsT19').val(),
        //ActualsT29: $('#ActualsT29').val(),
        //ActualsT39: $('#ActualsT39').val(),
        //Avg9: $('#Avg9').val(),
        //ActualsT110: $('#ActualsT110').val(),
        //ActualsT210: $('#ActualsT210').val(),
        //ActualsT310: $('#ActualsT310').val(),
        //Avg10: $('#Avg10').val(),
        //ActualsT111: $('#ActualsT111').val(),
        //ActualsT211: $('#ActualsT211').val(),
        //ActualsT311: $('#ActualsT311').val(),
        //Avg11: $('#Avg11').val(),
        // Flatness1: $('#Flatness1').val(),
        // Flatness2: $('#Flatness2').val(),
        // ParallelismSpec: $('#ParallelismSpec').val(),
        // Actuals: $('#Actuals').val(),
        ReviewedBy: $('#ReviewedBy').val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        CalibrationPerformedDate: $('#CalibrationPerformedDate').val(),
        ReveiwedByDate: $('#ReveiwedByDate').val(),
        //Measurement1: $('#Measurement1').val(),
        //Measurement2: $('#Measurement2').val(),
        //Measurement3: $('#Measurement3').val(),

        //Measurement8: $('#Measurement8').val(),
        //Measurement9: $('#Measurement9').val(),
        //Measurement10: $('#Measurement10').val(),
        //Measurement11: $('#Measurement11').val(),
        //MURemarks: $('#MURemarks').val(),

        InstrumentErrValue: $('#InstrumentErrValue').val(),
        FlatnessMeasure: $('#tablerowFlatness').find("td:eq(0) input[type='text']").val(),
        FlatnessActual: $('#tablerowFlatness').find("td:eq(1) input[type='text']").val(),
        FlatnessInserr: $('#tablerowFlatness').find("td:eq(2) input[type='text']").val(),
        MicrometerAddResultViewModelList: MicroResult,

    }
    $.ajax({
        url: '../Observation/InsertMicrometer',
        type: 'POST',
        data: { micrometer: data }
    }).done(function (resultObject) {

        showSuccess("Data Saved Successfully", lang);
        window.location.href = '../Tracker/Request?reqType=4';

    });
}

function SaveMetalRule(lang) {
    debugger;
    var MicroResult;
    var validationcheck = true;
    var ObjParent = '';

    var Unit = $('#Allvalues').val();
    if (Unit == null || Unit == "") {
        showWarning("Please enter the Unit !!!", lang);
        return true;
    }

    var Temprature = $('#TempStart').val();
    if (Temprature == null || Temprature == "") {
        showWarning("Please enter the Temprature !!!", lang);
        return true;
    }
    var Humidity = $('#Humidity').val();
    if (Humidity == null || Humidity == "") {
        showWarning("Please enter the Humidity !!!", lang);
        return true;
    }
    var MicrometerCondition = $('#MetalRulesCondition').val();
    if (MicrometerCondition == null || MicrometerCondition == "") {
        showWarning("Please enter the Visual Check !!!", lang);
        return true;
    }
    MicroResult = new Array();

    $('#MetalRule1 tbody tr').each(function (row, tr) {

        if ($('#MetalRule1 >tbody >tr').length > 0) {
            console.log("0000000000-------ididididi");
            console.log($(tr).index());
            // if ($(tr).index() >= 0) {
            //ObjParent = $(tr).find("td:eq(5) input[type='hidden']").val() || null || '' ? $('#TemplateObservationId').val() : $(tr).find("td:eq(5) input[type='hidden']").val();
            console.log("0000000000");

            if ($(tr).find("td:eq(1) input[type='text']").val() > 0) {
                var MicroData = {
                    Id: $(tr).find("td:eq(0) input[id='id']").val(),
                    SNO: $(tr).find("td:eq(0) input[type='text']").val(),
                    MeasuedValue: $(tr).find("td:eq(1) input[type='text']").val(),
                    Actuals: $(tr).find("td:eq(2) input[type='text']").val(),
                    InstrumentError: $(tr).find("td:eq(3) input[type='text']").val(),
                    MasterView1: 1
                    // Id: $(tr).find("td:eq(4) input[type='hidden']").val(),
                    // ParentId: ObjParent,
                    // InstrumentError: 1//$(tr).find("td:eq(6) input[type='hidden']").val()
                }
                //MicroResult.push(MicroData);
            }
            else {
                validationcheck = false;
                showWarning('Enter the Squareness values....', lang)

            }
            MicroResult.push(MicroData);
            // }

        }
        else {
            MicroResult.pop(MicroData);
        }
    });

    var obsType = $('#ObservationTypeMetalId').val();
    if (obsType != 165) {

        $('#MetalRule2 tbody tr').each(function (row, tr) {
            if ($('#MetalRule2 >tbody >tr').length > 0) {
                if ($(tr).index() >= 0) {
                    //ObjParent = $(tr).find("td:eq(5) input[type='hidden']").val() || null || '' ? $('#TemplateObservationId').val() : $(tr).find("td:eq(5) input[type='hidden']").val();                   
                    if ($(tr).find("td:eq(1) input[type='text']").val() > 0) {
                        var MicroData = {
                            Id: $(tr).find("td:eq(0) input[id='id']").val(),
                            SNO: $(tr).find("td:eq(0) input[type='text']").val(),
                            MeasuedValue: $(tr).find("td:eq(1) input[type='text']").val(),
                            Actuals: $(tr).find("td:eq(2) input[type='text']").val(),
                            InstrumentError: $(tr).find("td:eq(3) input[type='text']").val(),
                            MasterView2: 1
                            // Id: $(tr).find("td:eq(4) input[type='hidden']").val(),
                            // ParentId: ObjParent,
                            // InstrumentError: 1//$(tr).find("td:eq(6) input[type='hidden']").val()

                        }
                        //MicroResult.push(MicroData);
                    }
                    else {
                        showWarning('Enter the Straightness values....', lang)
                        validationcheck = false;
                    }
                    MicroResult.push(MicroData);
                }

            }
            else {
                MicroResult.pop(MicroData);
            }
        });
    }
    if (validationcheck) {
        var data = {
            Id: $('#IdMicro').val(),
            TemplateObservationId: $('#TemplateObservationId').val(),
            RefWi: $('#RefWi').val(),
            InstrumentId: $('#instrumentId').val(),
            RequestId: $('#requestId').val(),

            TempStart: $('#TempStart').val(),
            Humidity: $('#Humidity').val(),
            Allvalues: $('#Allvalues').val(),
            MetalRulesCondition: $('#MetalRulesCondition').val(),

            ReviewedBy: $('#ReviewedBy').val(),
            CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
            CalibrationPerformedDate: $('#CalibrationPerformedDate').val(),
            ReveiwedByDate: $('#ReveiwedByDate').val(),
            MetalRuleAddResultViewModelList: MicroResult,

        }
        $.ajax({
            url: '../Observation/InsertMetalRule',
            type: 'POST',
            data: { metalrule: data }
        }).done(function (resultObject) {
            window.location.href = '../Tracker/Request?reqType=4';
            showSuccess("Data Saved Successfully", lang);
        });
    }
}

function SaveGeneral(lang) {
    //debugger;
    var GeneralResult = new Array();
    $('#Generaladd tbody tr').each(function (row, tr) {

        var GeneralData = {
            MeasuedValue: $(tr).find("td:eq(0) input[type='text']").val(),
            Trial1: $(tr).find("td:eq(1) input[type='text']").val(),
            Trial2: $(tr).find("td:eq(2) input[type='text']").val(),
            Trial3: $(tr).find("td:eq(3) input[type='text']").val(),
            Average: $(tr).find("td:eq(4) input[type='text']").val(),
            Id: $(tr).find("td:eq(6) input[type='hidden']").val(),
            ParentId: $(tr).find("td:eq(7) input[type='hidden']").val()
        }
        GeneralResult.push(GeneralData);
    });

    var GeneralManualResult = new Array();
    $('#Manualadd tbody tr').each(function (row, tr) {

        var GeneralManualData = {
            Column1: $(tr).find("td:eq(0) input[type='text']").val(),
            Column2: $(tr).find("td:eq(1) input[type='text']").val(),
            Column3: $(tr).find("td:eq(2) input[type='text']").val(),
            Column4: $(tr).find("td:eq(3) input[type='text']").val(),
            Column5: $(tr).find("td:eq(4) input[type='text']").val(),
            Column6: $(tr).find("td:eq(5) input[type='text']").val(),
            Id: $(tr).find("td:eq(7) input[type='hidden']").val(),
            ParentId: $(tr).find("td:eq(8) input[type='hidden']").val()
        }
        GeneralManualResult.push(GeneralManualData);
    });

    var data = {
        Id: $('#Id').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        RefStd: $('#RefStd').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWi').val(),
        Allvalues: $('#Allvalues').val(),
        DialIndicatiorCondition: $('#DialIndicatiorCondition').val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        ReviewedBy: $('#ReviewedBy').val(),
        CalibrationDoneDate: $('#CalibrationDoneDate').val(),
        ReviewedDate: $('#ReviewedDate').val(),
        GeneralAddResultViewModelList: GeneralResult,
        GeneralManualAddResultViewModelList: GeneralManualResult,
    }
    $.ajax({
        url: '../Observation/InsertGeneral',
        type: 'POST',
        data: { general: data },
        dataType: "json",
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Data Saved Successfully", lang);
    });
}

function SaveVernierCaliper(lang) {
    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWi').val(),
        Allvalues: $('#Allvalues').val(),
        ConditionOfVernierCaliper: $('#condition').val(),
        Measured1_1: $('#Measured1_1').val(),
        Actuals1_T_1: $('#Actuals1_T_1').val(),
        Actuals1_T_2: $('#Actuals1_T_2').val(),
        Actuals1_T_3: $('#Actuals1_T_3').val(),
        Avg1_1: $('#Avg1_1').val(),
        Measured1_2: $('#Measured1_2').val(),
        Actuals1_T_4: $('#Actuals1_T_4').val(),
        Actuals1_T_5: $('#Actuals1_T_5').val(),
        Actuals1_T_6: $('#Actuals1_T_6').val(),
        Avg1_2: $('#Avg1_2').val(),
        Measured1_3: $('#Measured1_3').val(),
        Actuals1_T_7: $('#Actuals1_T_7').val(),
        Actuals1_T_8: $('#Actuals1_T_8').val(),
        Actuals1_T_9: $('#Actuals1_T_9').val(),
        Avg1_3: $('#Avg1_3').val(),
        Measured1_4: $('#Measured1_4').val(),
        Actuals1_T_10: $('#Actuals1_T_10').val(),
        Actuals1_T_11: $('#Actuals1_T_11').val(),
        Actuals1_T_12: $('#Actuals1_T_12').val(),
        Avg1_4: $('#Avg1_4').val(),
        Measured1_5: $('#Measured1_5').val(),
        Actuals1_T_13: $('#Actuals1_T_13').val(),
        Actuals1_T_14: $('#Actuals1_T_14').val(),
        Actuals1_T_15: $('#Actuals1_T_15').val(),
        Avg1_5: $('#Avg1_5').val(),
        Measured2_1: $('#Measured2_1').val(),
        Actuals2_T_1: $('#Actuals2_T_1').val(),
        Actuals2_T_2: $('#Actuals2_T_2').val(),
        Actuals2_T_3: $('#Actuals2_T_3').val(),
        Avg2_1: $('#Avg2_1').val(),
        Measured2_2: $('#Measured2_2').val(),
        Actuals2_T_4: $('#Actuals2_T_4').val(),
        Actuals2_T_5: $('#Actuals2_T_5').val(),
        Actuals2_T_6: $('#Actuals2_T_6').val(),
        Avg2_2: $('#Avg2_2').val(),
        Measured2_3: $('#Measured2_3').val(),
        Actuals2_T_7: $('#Actuals2_T_7').val(),
        Actuals2_T_8: $('#Actuals2_T_8').val(),
        Actuals2_T_9: $('#Actuals2_T_9').val(),
        Avg2_3: $('#Avg2_3').val(),
        Measured2_4: $('#Measured2_4').val(),
        Actuals2_T_10: $('#Actuals2_T_10').val(),
        Actuals2_T_11: $('#Actuals2_T_11').val(),
        Actuals2_T_12: $('#Actuals2_T_12').val(),
        Avg2_4: $('#Avg2_4').val(),
        Measured2_5: $('#Measured2_5').val(),
        Actuals2_T_13: $('#Actuals2_T_13').val(),
        Actuals2_T_14: $('#Actuals2_T_14').val(),
        Actuals2_T_15: $('#Actuals2_T_15').val(),
        Avg2_5: $('#Avg2_5').val(),
        Measured3_1: $('#Measured3_1').val(),
        Actuals3_T_1: $('#Actuals3_T_1').val(),
        Actuals3_T_2: $('#Actuals3_T_2').val(),
        Actuals3_T_3: $('#Actuals3_T_3').val(),
        Avg3_1: $('#Avg3_1').val(),
        Measured3_2: $('#Measured3_2').val(),
        Actuals3_T_4: $('#Actuals3_T_4').val(),
        Actuals3_T_5: $('#Actuals3_T_5').val(),
        Actuals3_T_6: $('#Actuals3_T_6').val(),
        Avg3_2: $('#Avg3_2').val(),
        Measured3_3: $('#Measured3_3').val(),
        Actuals3_T_7: $('#Actuals3_T_7').val(),
        Actuals3_T_8: $('#Actuals3_T_8').val(),
        Actuals3_T_9: $('#Actuals3_T_9').val(),
        Avg3_3: $('#Avg3_3').val(),
        Measured3_4: $('#Measured3_4').val(),
        Actuals3_T_10: $('#Actuals3_T_10').val(),
        Actuals3_T_11: $('#Actuals3_T_11').val(),
        Actuals3_T_12: $('#Actuals3_T_12').val(),
        Avg3_4: $('#Avg3_4').val(),
        Measured3_5: $('#Measured3_5').val(),
        Actuals3_T_13: $('#Actuals3_T_13').val(),
        Actuals3_T_14: $('#Actuals3_T_14').val(),
        Actuals3_T_15: $('#Actuals3_T_15').val(),
        Avg3_5: $('#Avg3_5').val(),
        Measured4_1: $('#Measured4_1').val(),
        Actuals4_T_1: $('#Actuals4_T_1').val(),
        Actuals4_T_2: $('#Actuals4_T_2').val(),
        Actuals4_T_3: $('#Actuals4_T_3').val(),
        Avg4_1: $('#Avg4_1').val(),
        Measured4_2: $('#Measured4_2').val(),
        Actuals4_T_4: $('#Actuals4_T_4').val(),
        Actuals4_T_5: $('#Actuals4_T_5').val(),
        Actuals4_T_6: $('#Actuals4_T_6').val(),
        Avg4_2: $('#Avg4_2').val(),
        Measured4_3: $('#Measured4_3').val(),
        Actuals4_T_7: $('#Actuals4_T_7').val(),
        Actuals4_T_8: $('#Actuals4_T_8').val(),
        Actuals4_T_9: $('#Actuals4_T_9').val(),
        Avg4_3: $('#Avg4_3').val(),
        Measured4_4: $('#Measured4_4').val(),
        Actuals4_T_10: $('#Actuals4_T_10').val(),
        Actuals4_T_11: $('#Actuals4_T_11').val(),
        Actuals4_T_12: $('#Actuals4_T_12').val(),
        Avg4_4: $('#Avg4_4').val(),
        Measured4_5: $('#Measured4_5').val(),
        Actuals4_T_13: $('#Actuals4_T_13').val(),
        Actuals4_T_14: $('#Actuals4_T_14').val(),
        Actuals4_T_15: $('#Actuals4_T_15').val(),
        Avg4_5: $('#Avg4_5').val(),
        Measured5_1: $('#Measured5_1').val(),
        Actuals5_T_1: $('#Actuals5_T_1').val(),
        Actuals5_T_2: $('#Actuals5_T_2').val(),
        Actuals5_T_3: $('#Actuals5_T_3').val(),
        Avg5_1: $('#Avg5_1').val(),
        Measured5_2: $('#Measured5_2').val(),
        Actuals5_T_4: $('#Actuals5_T_4').val(),
        Actuals5_T_5: $('#Actuals5_T_5').val(),
        Actuals5_T_6: $('#Actuals5_T_6').val(),
        Avg5_2: $('#Avg5_2').val(),
        Measured5_3: $('#Measured5_3').val(),
        Actuals5_T_7: $('#Actuals5_T_7').val(),
        Actuals5_T_8: $('#Actuals5_T_8').val(),
        Actuals5_T_9: $('#Actuals5_T_9').val(),
        Avg5_3: $('#Avg5_3').val(),
        Measured5_4: $('#Measured5_4').val(),
        Actuals5_T_10: $('#Actuals5_T_10').val(),
        Actuals5_T_11: $('#Actuals5_T_11').val(),
        Actuals5_T_12: $('#Actuals5_T_12').val(),
        Avg5_4: $('#Avg5_4').val(),
        Measured5_5: $('#Measured5_5').val(),
        Actuals5_T_13: $('#Actuals5_T_13').val(),
        Actuals5_T_14: $('#Actuals5_T_14').val(),
        Actuals5_T_15: $('#Actuals5_T_15').val(),
        Avg5_5: $('#Avg5_5').val(),
        Measured6_1: $('#Measured6_1').val(),
        Actuals6_T_1: $('#Actuals6_T_1').val(),
        Actuals6_T_2: $('#Actuals6_T_2').val(),
        Actuals6_T_3: $('#Actuals6_T_3').val(),
        Avg6_1: $('#Avg6_1').val(),
        Measured6_2: $('#Measured6_2').val(),
        Actuals6_T_4: $('#Actuals6_T_4').val(),
        Actuals6_T_5: $('#Actuals6_T_5').val(),
        Actuals6_T_6: $('#Actuals6_T_6').val(),
        Avg6_2: $('#Avg6_2').val(),
        Measured6_3: $('#Measured6_3').val(),
        Actuals6_T_7: $('#Actuals6_T_7').val(),
        Actuals6_T_8: $('#Actuals6_T_8').val(),
        Actuals6_T_9: $('#Actuals6_T_9').val(),
        Avg6_3: $('#Avg6_3').val(),
        Measured6_4: $('#Measured6_4').val(),
        Actuals6_T_10: $('#Actuals6_T_10').val(),
        Actuals6_T_11: $('#Actuals6_T_11').val(),
        Actuals6_T_12: $('#Actuals6_T_12').val(),
        Avg6_4: $('#Avg6_4').val(),
        Measured6_5: $('#Measured6_5').val(),
        Actuals6_T_13: $('#Actuals6_T_13').val(),
        Actuals6_T_14: $('#Actuals6_T_14').val(),
        Actuals6_T_15: $('#Actuals6_T_15').val(),
        Avg6_5: $('#Avg6_5').val(),
        ExternalParallelismDetails: $('#ExternalParallelismDetails').val(),
        DepthParallelismDetails: $('#DepthParallelismDetails').val(),
        MuLeftValue1: $('#MuLeftValue1').val(),
        MuLeftValue2: $('#MuLeftValue2').val(),
        MuLeftValue3: $('#MuLeftValue3').val(),
        MuLeftValue4: $('#MuLeftValue4').val(),
        MuLeftValue5: $('#MuLeftValue5').val(),
        MuRightValue1: $('#MuRightValue1').val(),
        MuRightValue2: $('#MuRightValue2').val(),
        MuRightValue3: $('#MuRightValue3').val(),
        MuRightValue4: $('#MuRightValue4').val(),
        MuRightValue5: $('#MuRightValue5').val(),
        InternaljawParallelismDetails: $('#InternaljawParallelismDetails').val(),
        MuRightValue5: $('#MuRightValue5').val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        ReviewedBy: $('#ReviewedBy').val(),
        CalibrationPerformedDate: $('#CalibrationPerformedDate').val(),
        ReviewedDate: $('#ReviewedDate').val(),


    }
    $.ajax({
        url: '../Observation/InsertVernierCaliper',
        type: 'POST',
        data: { verniercaliper: data }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Data Saved Successfully", lang);
    });
}

function SaveGeneralNew(lang) {
    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWi').val(),
        Allvalues: $('#Allvalues').val(),
        ConditionOfVernierCaliper: $('#condition').val(),
        ErrorinDMS1_1: $('#ErrorinDMS1_1').val(),
        ErrorinDMS1_2: $('#ErrorinDMS1_2').val(),
        ErrorinDMS1_3: $('#ErrorinDMS1_3').val(),
        ErrorinDMS1_4: $('#ErrorinDMS1_4').val(),
        ErrorinDMS1_5: $('#ErrorinDMS1_5').val(),
        ErrorinDMS2_1: $('#ErrorinDMS2_1').val(),
        ErrorinDMS2_2: $('#ErrorinDMS2_2').val(),
        ErrorinDMS2_3: $('#ErrorinDMS2_3').val(),
        ErrorinDMS2_4: $('#ErrorinDMS2_4').val(),
        ErrorinDMS3_1: $('#ErrorinDMS3_1').val(),
        ErrorinDMS3_2: $('#ErrorinDMS3_2').val(),
        ErrorinDMS3_3: $('#ErrorinDMS3_3').val(),
        ErrorinDMS3_4: $('#ErrorinDMS3_4').val(),
        ErrorinDMS4_1: $('#ErrorinDMS4_1').val(),
        ErrorinDMS4_2: $('#ErrorinDMS4_2').val(),
        ErrorinDMS4_3: $('#ErrorinDMS4_3').val(),
        Straightness_spec: $('#Straightness_spec').val(),
        Straightness_Actual: $('#Straightness_Actual').val(),
        Straightness_DevfromNom: $('#Straightness_DevfromNom').val(),
        Parallelism_Spec: $('#Parallelism_Spec').val(),
        Parallelism_Actual: $('#Parallelism_Actual').val(),
        Parallelism_DevfromNom: $('#Parallelism_DevfromNom').val(),
        FlatnessofBlade_spec_1: $('#FlatnessofBlade_spec_1').val(),
        FlatnessofBlade_Actual_1: $('#FlatnessofBlade_Actual_1').val(),
        FlatnessofBlade_DevfromNom_1: $('#FlatnessofBlade_DevfromNom_1').val(),
        FlatnessofBlade_spec_2: $('#FlatnessofBlade_spec_2').val(),
        FlatnessofBlade_Actual_2: $('#FlatnessofBlade_Actual_2').val(),
        FlatnessofBlade_DevfromNom_2: $('#FlatnessofBlade_DevfromNom_2').val(),
        EnvironmentCondition: $("#EnvironmentCondition").val(),
        Uncertainity: $("#Uncertainity").val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        ReviewedBy: $('#ReviewedBy').val(),
        CalibrationPerformedDate: $('#CalibrationPerformedDate').val(),
        ReviewedDate: $('#ReviewedDate').val(),
    }
    $.ajax({
        url: '../Observation/InsertGeneralnewobs',
        type: 'POST',
        data: { GeneralNew: data }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Data Saved Successfully", lang);
    });
}

function SavePlungerDial(lang) {

    var obsSubType = $('#ObsSubType').val();

    var spec1 = '';
    var spec2 = '';
    var spec3 = '';
    var spec4 = '';
    var spec5 = '';
    var spec6 = '';

    var actual1 = '';
    var actual2 = '';
    var actual3 = '';
    var actual4 = '';
    var actual5 = '';
    var actual6 = '';

    var interval1 = '';
    var interval2 = '';
    var interval3 = '';
    var interval4 = '';
    var interval5 = '';
    var interval6 = '';
    var remarks = '';

    if (obsSubType == 79) {
        spec1 = $('#Digital_Spec1').val();
        spec2 = $('#Digital_Spec2').val();
        spec3 = $('#Digital_Spec3').val();
        spec4 = $('#Digital_Spec4').val();
        spec5 = $('#Digital_Spec5').val();

        actual1 = $('#Digital_Actual1').val();
        actual2 = $('#Digital_Actual2').val();
        actual3 = $('#Digital_Actual3').val();
        actual4 = $('#Digital_Actual4').val();
        actual5 = $('#Digital_Actual5').val();

        interval1 = $('#Digital_Interval1').val();
        interval2 = $('#Digital_Interval2').val();
        interval3 = $('#Digital_Interval3').val();
        interval4 = $('#Digital_Interval4').val();
        interval5 = $('#Digital_Interval5').val();
        remarks = $('#Remarks').val();
    }
    else {
        spec1 = $('#Spec1').val();
        spec2 = $('#Spec2').val();
        spec3 = $('#Spec3').val();
        spec4 = $('#Spec4').val();
        spec5 = $('#Spec5').val();
        spec6 = $('#Spec6').val();

        actual1 = $('#Actual1').val();
        actual2 = $('#Actual2').val();
        actual3 = $('#Actual3').val();
        actual4 = $('#Actual4').val();
        actual5 = $('#Actual5').val();
        actual6 = $('#Actual6').val();

        interval1 = $('#Interval1').val();
        interval2 = $('#Interval2').val();
        interval3 = $('#Interval3').val();
        interval4 = $('#Interval4').val();
        interval5 = $('#Interval5').val();
        interval6 = $('#Interval6').val();
    }

    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        Allvalues: $('#Allvalues').val(),
        ConditionAndObservation: $('#ConditionAndObservation').val(),
        RefWi: $('#RefWi').val(),

        Spec1: spec1,
        Spec2: spec2,
        Spec3: spec3,
        Spec4: spec4,
        Spec5: spec5,
        Spec6: spec6,

        Actual1: actual1,
        Actual2: actual2,
        Actual3: actual3,
        Actual4: actual4,
        Actual5: actual5,
        Actual6: actual6,

        Interval1: interval1,
        Interval2: interval2,
        Interval3: interval3,
        Interval4: interval4,
        Interval5: interval5,
        Interval6: interval6,

        ObsSubType: $('#ObsSubType').val(),
        Remarks: remarks,
    }
    $.ajax({
        url: '../Observation/InsertPlungerDial',
        type: 'POST',
        data: { plungerDial: data }
    }).done(function (resultObject) {
        showSuccess("Data Saved Successfully", lang);
        window.location.href = '../Tracker/Request?reqType=4';

    });
}
function SaveThreadGauges(lang) {

    var RefWi = '';
    var Max1 = '';
    var Max2 = '';
    var Min1 = '';
    var Min2 = '';
    var WearLimit1 = '';
    var WearLimit2 = '';
    var Plane1 = '';
    var Plane2 = '';
    var Plane3 = '';
    var Plane4 = '';
    var Plane5 = '';
    var Repeatability1 = '';
    var Repeatability2 = '';
    var Repeatability3 = '';
    var Repeatability4 = '';
    var Repeatability5 = '';

    var obsSubType = $('#ObsSubType').val();

    if (obsSubType == 80) {

        RefWi = $('#RefWi').val();
        Max1 = $('#Max1').val();
        Max2 = $('#Max2').val();
        Min1 = $('#Min1').val();
        Min2 = $('#Min2').val();
        WearLimit1 = $('#WearLimit1').val();
        WearLimit2 = $('#WearLimit2').val();
        Plane1 = $('#Plane1').val();
        Plane2 = $('#Plane2').val();
        Plane3 = $('#Plane3').val();
        Plane4 = $('#Plane4').val();
        Plane5 = $('#Plane5').val();
        Repeatability1 = $('#Repeatability1').val();
        Repeatability2 = $('#Repeatability2').val();
        Repeatability3 = $('#Repeatability3').val();
        Repeatability4 = $('#Repeatability4').val();
        Repeatability5 = $('#Repeatability5').val();
    } else {

        RefWi = $('#Ring_RefWi').val();
        Max1 = $('#Ring_Max1').val();
        Max2 = $('#Ring_Max2').val();
        Min1 = $('#Ring_Min1').val();
        Min2 = $('#Ring_Min2').val();
        WearLimit1 = $('#Ring_WearLimit1').val();
        WearLimit2 = $('#Ring_WearLimit2').val();
        Plane1 = $('#Ring_Plane1').val();
        Plane2 = $('#Ring_Plane2').val();
        Plane3 = $('#Ring_Plane3').val();
        Plane4 = $('#Ring_Plane4').val();
        Plane5 = $('#Ring_Plane5').val();
        Repeatability1 = $('#Ring_Repeatability1').val();
        Repeatability2 = $('#Ring_Repeatability2').val();
        Repeatability3 = $('#Ring_Repeatability3').val();
        Repeatability4 = $('#Ring_Repeatability4').val();
        Repeatability5 = $('#Ring_Repeatability5').val();
    }

    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        Allvalues: $('#Allvalues').val(),
        ThreadgaugeCondtion: $('#ThreadgaugeCondtion').val(),
        RefWi: RefWi,
        Max1: Max1,
        Max2: Max2,
        Min1: Min1,
        Min2: Min2,
        WearLimit1: WearLimit1,
        WearLimit2: WearLimit2,
        Plane1: Plane1,
        Plane2: Plane2,
        Plane3: Plane3,
        Plane4: Plane4,
        Plane5: Plane5,
        Repeatability1: Repeatability1,
        Repeatability2: Repeatability2,
        Repeatability3: Repeatability3,
        Repeatability4: Repeatability4,
        Repeatability5: Repeatability5,

    }
    $.ajax({
        url: '../Observation/InsertThreadGuages',
        type: 'POST',
        data: { threadGauges: data }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Data Saved Successfully", lang);
    });
}

function SaveTWobs(lang) {
    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        Allvalues: $('#Allvalues').val(),
        ConditionOfTW: $('#ConditionOfTW').val(),
        RefWi: $('#RefWi').val(),
        ActualInOne: $('#ActualInOne').val(),
        ActualInSix: $('#ActualInSix').val(),
        SpecMax: $('#SpecMax').val(),
        ActualInTwo: $('#ActualInTwo').val(),
        ActualInSeven: $('#ActualInSeven').val(),
        ActualInThree: $('#ActualInThree').val(),
        ActualInEight: $('#ActualInEight').val(),
        SpecMin: $('#SpecMin').val(),
        ActualInFour: $('#ActualInFour').val(),
        ActualInNine: $('#ActualInNine').val(),
        ActualInFive: $('#ActualInFive').val(),
        ActualInTen: $('#ActualInTen').val(),
        Nominal20: $('#Nominal20').val(),
        Nominal60: $('#Nominal60').val(),
        Nominal100: $('#Nominal100').val(),
        Spec20: $('#Spec20').val(),
        Spec60: $('#Spec60').val(),
        Spec100: $('#Spec100').val(),
        Value1: $('#Value1').val(),
        Value2: $('#Value2').val(),
        Value3: $('#Value3').val(),
        Value4: $('#Value4').val(),
        Value5: $('#Value5').val(),
        Value6: $('#Value6').val(),
        Value7: $('#Value7').val(),
        Value8: $('#Value8').val(),
        Value9: $('#Value9').val(),
        Value10: $('#Value10').val(),
        Value11: $('#Value11').val(),
        Value12: $('#Value12').val(),
        Value13: $('#Value13').val(),
        Value14: $('#Value14').val(),
        Value15: $('#Value15').val(),
        SetValue: $('#SetValue').val(),
        Comments: $('#Comments').val(),
        CalibBy: $('#CalibBy').val(),
        Reviewed_By: $('#Reviewed_By').val(),
        Calib_Date: $('#Calib_Date').val(),
        Review_Date: $('#Review_Date').val(),
        AWSTransducers: $('#AWSTransducers').val(),
        NorbarTransducers: $('#NorbarTransducers').val(),
    }


    $.ajax({
        url: '../Observation/InsertTWobs',
        type: 'POST',
        data: { torquewrenches: data }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Data Saved Successfully", lang);
    });
}

function ResultAcceptReject() {
    var data = $('#newResultLAB').val();
    if (data == "Accepted") {
        $('#dvnewrequestclosedAccept').css('display', 'block');
        $('#dvnewrequestclosedReject').css('display', 'none');
    } else if (data == "Rejected") {
        $('#dvnewrequestclosedReject').css('display', 'block');
        $('#dvnewrequestclosedAccept').css('display', 'block');
    } else {
        $('#dvnewrequestclosedAccept').css('display', 'none');
        $('#dvnewrequestclosedReject').css('display', 'none');
    }
}

function showfeasibleservice() {
    var data = $('#newFeasibleService').val();
    if (data == "Yes") {
        $('#dvshowfeasibleserviceyes').css('display', 'block');
        $('#dvshowfeasibleserviceno').css('display', 'none');
    } else {
        $('#dvshowfeasibleserviceyes').css('display', 'none');
        $('#dvshowfeasibleserviceno').css('display', 'none');
    }
}

function LoadObservationTypePopup(lang) {
    $.ajax({
        url: '../Tracker/LoadObservationType',
        type: 'POST',
        data: { attrType: '', attrsubType: $('#NewObservation option:selected').text(), langtype: lang }
    }).done(function (resultObject) {
        $('#NewObservationType')
            .find('option')
            .remove();

        $('#NewObservationType').empty();
        $('#NewObservationType').append(`<option value="0">--Select--</option>`);
        for (let index = 0; index < resultObject.length; index++) {
            if (lang == 'en') {

                optText = resultObject[index].attrValue;
                optValue = resultObject[index].id;;
                $('#NewObservationType').append(`<option value="${optValue}">${optText}</option>`);
            }
            else {
                optTextjp = resultObject[index].attrValueJp;
                optValuejp = resultObject[index].id;;
                $('#NewObservationType').append(`<option value="${optValuejp}">${optTextjp}</option>`);
            }
        }
    });

}

function LoadObservationTypePopupEd(lang) {
    $.ajax({
        url: '../Tracker/LoadObservationType',
        type: 'POST',
        data: { attrType: '', attrsubType: $('#NewObservationEd option:selected').text(), langtype: lang }
    }).done(function (resultObject) {
        $('#NewObservationTypeEd')
            .find('option')
            .remove();

        $('#NewObservationTypeEd').empty();
        $('#NewObservationTypeEd').append(`<option value="0">--Select--</option>`);
        for (let index = 0; index < resultObject.length; index++) {
            if (lang == 'en') {
                console.log(1)
                optText = resultObject[index].attrValue;
                optValue = resultObject[index].id;;
                $('#NewObservationTypeEd').append(`<option value="${optValue}">${optText}</option>`);
            }
            else {
                console.log(2)
                optTextjp = resultObject[index].attrValueJp;
                optValuejp = resultObject[index].id;;
                $('#NewObservationTypeEd').append(`<option value="${optValuejp}">${optTextjp}</option>`);
            }
        }
    });

}

function AddNewInstrumentMaster(lang) {
    if ($('#MasterInstrument option:selected').val() != undefined && $('#MasterInstrument option:selected').val() != "") {
        if ($('#MasterInstrument1').val() == 0) {
            $('#MasterInstrument1').val($('#MasterInstrument option:selected').val());
            $('#MasterInstrument1').attr('value', ($('#MasterInstrument option:selected').val()));
            $('#masterEquipmentValue').append('<div id="masvalue1">' + $('#MasterInstrument option:selected').text() + '<i class="fas fa-trash" onclick="DeleteMasterEqiupment(1)"></i>' + '<br></div>');
        } else if ($('#MasterInstrument2').val() == 0) {
            $('#MasterInstrument2').val($('#MasterInstrument option:selected').val());
            $('#MasterInstrument2').attr('value', ($('#MasterInstrument option:selected').val()));
            $('#masterEquipmentValue').append('<div id="masvalue2">' + $('#MasterInstrument option:selected').text() + '<i class="fas fa-trash" onclick="DeleteMasterEqiupment(2)"></i>' + '<br></div>');
        } else if ($('#MasterInstrument3').val() == 0) {
            $('#MasterInstrument3').val($('#MasterInstrument option:selected').val());
            $('#MasterInstrument3').attr('value', ($('#MasterInstrument option:selected').val()));
            $('#masterEquipmentValue').append('<div id="masvalue3">' + $('#MasterInstrument option:selected').text() + '<i class="fas fa-trash" onclick="DeleteMasterEqiupment(3)"></i>' + '<br></div>');
        } else if ($('#MasterInstrument4').val() == 0) {
            $('#MasterInstrument4').val($('#MasterInstrument option:selected').val());
            $('#MasterInstrument4').attr('value', ($('#MasterInstrument option:selected').val()));
            $('#masterEquipmentValue').append('<div id="masvalue4">' + $('#MasterInstrument option:selected').text() + '<i class="fas fa-trash" onclick="DeleteMasterEqiupment(4)"></i>' + '<br></div>');
        } else {
            showWarning("Maximum 4 Equipment Allowed", lang);
        }
    } else {
        showSuccess("Master Equipment Added Successfully", lang);
    }
}

function DeleteMasterEqiupment(id) {
    $('#MasterInstrument' + id).remove();
    $('#masvalue' + id).remove();
}
function DeleteMasterEqiupmentFile(id) {
    //$('#MasterInstrument' + id).remove();
    //$('#masvalue' + id).remove();
}

function SaveCertificate(templtatename, lang) {

    var temptName = templtatename;
    var result = $('#CalibrationResult').val();
    if (result == null || result == "") {
        showWarning("Please enter the Calibration Result!!!", lang);
        return true;
    }

    Swal.fire({
        title: "Are you want To Generate QR Code with Pdf file?",
        text: "You will save Certificate and Generate QR Code with Pdf file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, generate it!",
        closeOnConfirm: false
    }).then((result) => {
        // }, function (isConfirm) {
        //       if (!isConfirm) return;

        if (result.isConfirmed) {
            $.ajax({
                url: '../Certification/SaveLeverDialCertificate',
                type: 'POST',
                data: {
                    requestId: $('#requestId').val(),
                    instrumentId: $('#instrumentId').val(),
                    EnvironmentCondition: '0', //$('#EnvironmentCondition').val(),
                    Uncertainity: $('#Uncertainity').val(),
                    CalibrationResult: $('#CalibrationResult').val(),
                    Remarks: $('#Remarks').val(),
                    ExportData: $("#Pdfhtml").html(),
                    TempltateName: temptName
                }
            }).done(function (resultObject) {
                showSuccess("Master Activated Successfully", lang);
                window.location.reload();
            });
        }
    });
}

function SaveInstrumentDetails(lang) {
    $.ajax({
        url: '../Tracker/SaveInstrumentFromRequest',
        type: 'POST',
        data: {
            requestId: $('#RequestCalibId').val(),
            newLabId: $('#NewLabId').val(),
            newNABL: $('#NewNABL').val(),
            newObservation: $('#NewObservation').val(),
            newObservationType: $('#NewObservationType').val(),
            newMU: $('#NewMU').val(),
            newCertification: $('#NewCertification').val(),
            masterInstrument1: $('#MasterInstrument1').val(),
            masterInstrument2: $('#MasterInstrument2').val(),
            masterInstrument3: $('#MasterInstrument3').val(),
            masterInstrument4: $('#MasterInstrument4').val(),
            calibSource: $('#CalibSource').val(),
            standardReffered: $('#StandardReffered').val(),
            calibDate: $('#CalibDate').val(),
            dueDate: $('#DueDate').val(),
            dateOfReceipt: $('#DateOfReceipt').val(),
            // Grade: $('#Grade').val(),
        }
    }).done(function (resultObject) {
        showSuccess("Certificate Updated Successfully", lang);
        window.location.reload();
    });
    console.log(" $('#Grade').val()");
    console.log($('#Grade').val());
}

function newSubmitReqLABVisual(lang) {

    if ($('#newResultLAB').val() == '0') {
        $('#newResultLAB').addClass('is-invalid');
        return false;
    } else {
        $('#newResultLAB').removeClass('is-invalid');
    }
    var DueDate = null;
    if ($('#newResultLAB').val() != 'Rejected') {
        DueDate = $('#DueDate').val();
    }

    $.ajax({
        url: '../Tracker/SubmitLABRequestVisual',
        type: 'POST',
        data: {
            requestId: $('#RequestCalibId').val(),
            Result: $('#newResultLAB').val(),
            RecordBy: $('#RecordBy').val(),
            ClosedDate: $('#newClosedDate').val(),
            Remarks: $('#newRemarks').val(),
            InstrumentReturnedDate: $('#newInstrumentReturnedDate').val(),
            CollectedBy: $('#CollectedBy').val(),
            ReasonforRejection: $('#newReasonRejection').val(),
            IsFeasibleService: $('#newFeasibleService').val(),
            IsFeasibleYes: $('#newIsFeasibleService').val(),
            ServiceResponsibility: $('#newServiceResponsibility').val(),
            RequestType: $('#hdntype').val(),
            InstrumentId: $('#hdnInstrumentId').val(),
            IdNo: $('#IdNo').val(),
            CalibDate: $('#CalibDate').val(),
            DueDate: DueDate,
            newNABL: $('#IsNABL').val(),
            FileData: FileData,

        }
    }).done(function (resultObject) {
        var Id = $('#RequestCalibId').val();
        window.location.href = '../Tracker/RequestDetailsNew?Id=' + resultObject.id + '';
        showSuccess("Your visual check details recorded", lang);
    });
}

function newSubmitReqDepVisual(lang) {

    if ($('#newResultDEP').val() == '' || $('#newResultDEP').val() == undefined) {
        $('#newResultDEP').addClass('is-invalid');
        return false;
    } else {
        $('#newResultDEP').removeClass('is-invalid');

    }
    if ($('#IdNo').val() == '' || $('#IdNo').val() == undefined) {
        $('#IdNo').addClass('is-invalid');
        return false;
    } else {
        $('#IdNo').removeClass('is-invalid');

    }

    var dueDate;
    var dt = $('#CalibFreqDue').val();
    //debugger;
    dueDate = DudeDateCalculation(dt);

    $.ajax({
        url: '../Tracker/SubmitDepartmentRequestVisual',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), Result: $('#newResultDEP').val(), CollectedBy: $('#CollectedBy').val(), InstrumentIdNo: $('#IdNo').val(), CalibFreq: dueDate }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        //AssignNewRequestValues(resultObject);
        showSuccess("Your visual check details recorded", lang);
    });
}

function SubmitReview(lang) {
  
    $('#dvload').show();
    var revstat = $('#ReviewStatus').val();
   
    if (revstat == '') {
        showWarning("Please select Judgement", lang);
        return false;
    }

    if (revstat == 2) {

        if ($('#Remarks').val().trim() == '') {
            showWarning("Please Enter Instrument Reject Reason", lang);
            return false;
        }
    }
    // for content updates
    var ObservationContentValues = new Array();
    var ObservationContent = new Array();
    var ObservationContenMapping = new Array();
    var footerPermissiable = "";
    $(".content").each(function () {

        $(this).find('table').each(function () {

            var tbl_id = $(this).attr("id");//.split("-")[2];

            if (tbl_id.substring(0, 2) == "CW") {
                footerPermissiable = $('#tblCW_footer').find("input.PermissibleLimit").val();
            }
            $(this).find('tr').each(function () {
                var rowid = $(this).find('tr').attr('id');
                var currentRow = $(this).find('td').attr('id');

                var contentvalueid = $(this).find("input[name = HiddenContentvalueId]").val();


                if ((contentvalueid == "0") || (contentvalueid == "")) {
                    contentvalueid = null
                }

                if ($(this).find("input[name = HiddenContentId]").val() > 0) {

                    var ObservationContenMappingData = {

                        Id: $(this).find("input[name = HiddenMappingId]").val(),
                        Sno: $(this).find("td:eq(1) input[type='text']").val(),
                        ContentId: $(this).find("input[name = HiddenContentId]").val(),
                        ObservationId: $('#TemplateObservationId').val(),
                        InstrumentId: $('#InstrumentId').val(),
                        CreatedBy: "",
                        CreatedOn: "",
                        IsActive: true
                    }
                    ObservationContenMapping.push(ObservationContenMappingData);
                    if (currentRow == 'CW') {

                        var ObservationContenValuesData = {

                            Id: contentvalueid,
                            ParentId: $('#TemplateObservationId').val(),
                            Sno: 0,// $(this).find("td:eq(1) input[type='text']").val(),
                            MeasuedValue: "",
                            ActualValue: "",
                            InstrumentError: "",
                            Diff: $(this).find('td').attr('id'),
                            MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                            MeasuedValue1: $(this).find("td:eq(2) input[type='text']").val(),
                            MeasuedValue2: $(this).find("td:eq(3) input[type='text']").val(),
                            MeasuedValue3: $(this).find("td:eq(4) input[type='text']").val(),
                            Average: $(this).find("td:eq(5) input[type='text']").val(),
                            Percent: $(this).find("td:eq(6) input[type='text']").val(),
                            ContentId: $(this).find("input[name = HiddenContentId]").val(),
                            PermissibleLimit: footerPermissiable
                            //<th rowspan="1" colspan="1">Parcela</th> $('tfoot input').
                        }
                        //}
                        ObservationContentValues.push(ObservationContenValuesData);
                    }
                    else if (currentRow == 'SW') {

                        var ObservationContenValuesData = {

                            Id: contentvalueid,
                            ParentId: $('#TemplateObservationId').val(),
                            Sno: 0,// $(this).find("td:eq(1) input[type='text']").val(),
                            MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                            ActualValue: "",
                            InstrumentError: "",
                            Diff: $(this).find('td').attr('id'),
                            MeasuedValue1: $(this).find("td:eq(2) input[type='text']").val(),//3,4,5
                            MeasuedValue2: $(this).find("td:eq(3) input[type='text']").val(),
                            MeasuedValue3: $(this).find("td:eq(4) input[type='text']").val(),
                            Average: "",
                            Percent: "",
                            ContentId: $(this).find("input[name = HiddenContentId]").val(),
                            PermissibleLimit: ""
                        }
                        //}
                        ObservationContentValues.push(ObservationContenValuesData);
                    }
                    else if (currentRow == 'IN') {

                        //console.log($(this).closest("tr").find("td:eq(3)").text());


                        var ObservationContenValuesData = {

                            Id: contentvalueid,// $(this).find("input[name = HiddenContentvalueId]").val(),
                            ParentId: $('#TemplateObservationId').val(),
                            Sno: 0,//$(this).find("td:eq(1) input[type='text']").val(),//2,3,4
                            MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                            ActualValue: $(this).find("td:eq(2) input[type='text']").val(),
                            InstrumentError: $(this).find("td:eq(3) input[type='text']").val(),
                            Diff: $(this).find('td').attr('id'),
                            MeasuedValue1: "",
                            MeasuedValue2: "",
                            MeasuedValue3: "",
                            Average: "",
                            Percent: "",
                            ContentId: $(this).find("input[name = HiddenContentId]").val(),
                            PermissibleLimit: ""
                            //}
                        }
                        ObservationContentValues.push(ObservationContenValuesData);

                    }
                    else if (currentRow == 'SE') {
                        //alert($(this).closest("tr").attr('id'));
                        console.log($(this).closest("tr").attr('id'));
                        console.log($(this).closest("tr").find("td:eq(3)").text());

                        var data = $(this).closest("tr").attr('id');//$(this).closest("tr").find("td:eq(3)").text();

                        var ObservationContenValuesData = {

                            Id: contentvalueid,// $(this).find("input[name = HiddenContentvalueId]").val(),
                            ParentId: $('#TemplateObservationId').val(),
                            Sno: data,//$(this).find("td:eq(1) input[type='text']").val(),//2,3,4
                            MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                            ActualValue: $(this).find("td:eq(2) input[type='text']").val(),
                            InstrumentError: $(this).closest("tr").find("td:eq(3)").text(),
                            Diff: $(this).find('td').attr('id'),
                            MeasuedValue1: "",
                            MeasuedValue2: "",
                            MeasuedValue3: "",
                            Average: "",
                            Percent: "",
                            ContentId: $(this).find("input[name = HiddenContentId]").val(),
                            PermissibleLimit: ""
                            //}
                        }
                        ObservationContentValues.push(ObservationContenValuesData);

                    }
                }

            });
        });

    });
    var filedataOBS = [];
    var filenameOBS = [];
    var filesizeOBS = [];
    var fileSerialNoOBS = [];
    $.each(FileData, function (key, value) {

        filesizeOBS.push(value.size);
        filenameOBS.push(value.name.trim());
        filedataOBS.push(value.data);
        fileSerialNoOBS.push(value.slno);

    })

    var data = {
        Id: $('#Id').val(),
        InstrumentId: $('#InstrumentId').val(),
        RequestId: $('#RequestId').val(),
        TempStart: $('#TempStart').val(),
        Humidity: $('#Humidity').val(),
        Units: $('#Units').val(),
        Condition: $('#VisualCheckCondition').val(),
        ObservationContentValuesList: ObservationContentValues,
        ObservationContentMappingList: ObservationContenMapping,
        FileName: filenameOBS,
        FileData: filedataOBS,
        FileSize: filesizeOBS,
        Serialno: fileSerialNoOBS,
        PermissibleLimit: $('#PermissibleLimit').val()
    }

    console.log(data);



    //
    var dueDate;
    var dt = $('#InsCalibFreq').val();
   
    dueDate = DudeDateCalculation(dt);

    $.ajax({
        url: '../Observation/SubmitReview',
        type: 'POST',
        data: { observationId: $('#TemplateObservationId').val(), reviewDate: $('#ReviewDate').val(), reviewStatus: $('#ReviewStatus').val(), Remarks: $('#Remarks').val(), RequestId: $('#RequestId').val(), DueDate: dueDate, dynamic: data }
    }).done(function (resultObject) {
        $('#dvload').hide();
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Your details recorded", lang);
    });
}
$(document).on('change', '.clsFile', (e) => {
    if (window.File && window.FileList && window.FileReader) {
       
        var serial = e.target.name;
       
            var filedata = [];
            var filename = [];
        var filesize = [];
        var fileslno = [];
            var files = e.target.files,
                filesLength = files.length, loaded = 0;
            var filejson;

            for (var i = 0; i < filesLength; i++) {
                var fileReader = new FileReader();
                var f = files[i];
                
                var namesfile = guid();
                var fna = f.name;
               
                var finalname = fna.replace(/[0-9`~!@#$%^&*()_|+\-=?;:'",<>\{\}\[\]\\\/]/gi, '', '');
                if (finalname == "")
                    finalname = namesfile;
               
                filename.push(finalname);
                filesize.push(f.size);
                fileslno.push(e.target.name);
                fileReader.onload = (function (e) {

                    var file = e.target;
                    filedata.push(e.target.result);
                    loaded++;

                    if (loaded == filesLength) {
                        console.log(fileslno);
                        FileUpload1(filedata, filename, filesize, fileslno);
                    }


                });
                fileReader.readAsDataURL(f);
            }

       // });
    }
});
function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
        s4() + '-' + s4() + s4() + s4();
}
$(document).ready(function () {
    if (window.File && window.FileList && window.FileReader) {
        $("#ImageUpload").on("change", function (e) {
            var filedata = [];
            var filename = [];
            var filesize = [];
            var files = e.target.files,
                filesLength = files.length,
                loaded = 0;
            var filejson;

            for (var i = 0; i < filesLength; i++) {
                var fileReader = new FileReader();
                var f = files[i];
                filename.push(f.name);
                filesize.push(f.size);
                fileReader.onload = (function (e) {
                    var file = e.target;
                    filedata.push(e.target.result);
                    loaded++;

                    if (loaded == filesLength) {
                        if (CheckFileExtension(filename)) {
                            FileUpload(filedata, filename, filesize);
                        } else {
                            AlertPopup('Please check uploaded files are Invalid!');
                            return false;
                        }
                    }
                });
                fileReader.readAsDataURL(f);
            }
        });
        $("#files1").find('.clsFile').on("change", function (e) {
       // $(".clsFile").on("change", function (e) {
           // alert("filess");
            //var filedata = [];
            //var filename = [];
            //var filesize = [];
            //var files = e.target.files,
            //    filesLength = files.length, loaded = 0;
            //var filejson;

            //for (var i = 0; i < filesLength; i++) {
            //    var fileReader = new FileReader();
            //    var f = files[i];
            //    // filename.push(f.name.replace('.', '$'));
            //    var namesfile = guid();
            //    var fna = f.name;
            //    // filename.push(namesfile.substring(0, 5) + "." + fna.substring(fna.lastIndexOf(".") + 1));
            //    //filename.push(fna.replace(/[^a-z0-9\s]/gi, '').replace(/[_\s]/g, '') + "." + fna.substring(fna.lastIndexOf(".") + 1));

            //    var finalname = fna.replace(/[0-9`~!@#$%^&*()_|+\-=?;:'",.<>\{\}\[\]\\\/]/gi, '', '');

            //    if (finalname == "")
            //        finalname = namesfile;

            //    filename.push(finalname + "." + fna.substring(fna.lastIndexOf(".") + 1));

            //    filesize.push(f.size);
            //    fileReader.onload = (function (e) {

            //        var file = e.target;
            //        filedata.push(e.target.result);
            //        loaded++;

            //        if (loaded == filesLength) {
            //            FileUpload1(filedata, filename, filesize);
            //        }


            //    });
            //    fileReader.readAsDataURL(f);
            //}

        });
    }
});

function CheckFileExtension(filename) {    
    var Filetype = false;
    for (var i = 0; i < filename.length; i++) {
        var ext = filename[i].split('.')[1];       
        if (ext == "pptx" || ext == "ppt") {
            Filetype = true;
        } else if (ext == "xls" || ext == "xlsx" || ext == "csv") {
            Filetype = true;
        } else if (ext == "jpg" || ext == "jpeg" || ext == "png" || ext == "svg") {
            Filetype = true;
        } else if (ext == "doc" || ext == "docx") {
            Filetype = true;
        } else if (ext == "pdf") {
            Filetype = true;
        } else if (ext == "mp4") {
            Filetype = true;
        } else if (ext == "txt") {
            Filetype = true;
        } else if (ext == "zip") {
            Filetype = true;
        } else {
            Filetype = false;
        }
    }
    return Filetype;
}

function CheckFileType(filename) {
    
    var ext = filename.split('.')[1];
    var Filetype = '';
   
    if (ext == "pptx" || ext == "ppt") {
        Filetype = "ppt.png";
    } else if (ext == "xls" || ext == "xlsx" || ext == "csv") {
       
        Filetype = "excel.png";
    } else if (ext == "jpg" || ext == "jpeg" || ext == "png" || ext == "svg") {
        Filetype = "image.png";
    } else if (ext == "doc" || ext == "docx") {
        Filetype = "word.png";
    } else if (ext == "pdf") {
        Filetype = "pdf.png";
    } else if (ext == "mp4") {
        Filetype = "video.png";
    } else if (ext == "txt") {
        Filetype = "text.png";
    } else if (ext == "zip") {
        Filetype = "zip.png";
    } else {
        Filetype = "notype.png";
    }
    //alert(Filetype);
    return Filetype;
}

function FileUpload(filedata, filename, filesize) {
    var json = {};
    for (var i = 0; i < filename.length; i++) {
        var fs = (filesize[i] / 1024 / 1024).toFixed(2);
        json = { "Name": filename[i], "Size": fs, "Data": filedata[i] };
        FileData.push(json);
       
      /*  <i id="@img" class="fas fa-trash" onclick="MasterFileDeleteAlert(this,'@img','@Model.Id');"></i>*/
    }
}
function downloadFile() {
    alert('Please save the data and then download the file');
}
function deleteFile(id, filename) {
    FileData = $.grep(FileData, function (element, index) {
        return element.name != filename;
    });
    $('#' + id).remove();
}
function FileUpload1(filedata, filename, filesize,serialno) {
   
    var json = {};
    var html = '';
    for (var i = 0; i < filename.length; i++) {
        var ids = guid();
        var fs = (filesize[i] / 1024 / 1024).toFixed(2);
        var joinfilename = filename[i];
       
        html += '<div id="' + ids + '" ><span class=""><img height="13" width="13" src="../image/' + CheckFileType(joinfilename) + '" /></span>'
            + joinfilename + ''
            + ' <a onclick="deleteFile(\'' + ids + '\',\'' + joinfilename + '\');" data-container="body" data-toggle="tooltip" data-placement="top" title="Delete"  type="button" class="fa fa-fw fa-trash"></a>'
            + '</div>';
    
        json = { "name": joinfilename, "size": fs, "data": filedata[i], "slno":serialno };
        FileData.push(json);

    }
    console.log(html);
    $('#dfileattach').append(html);
}
function Filedownload(fileguid, filename) {

   // var json = {};id
    var html = '';
    for (var i = 0; i < filename.length; i++) {
        var ids = fileguid[i];
        var fs = (filesize[i] / 1024 / 1024).toFixed(2);
        var joinfilename = filename[i];

        html += '<div id="' + ids + '" class=" doctabledesign" ><img alt="" class="img-responsive" src="../image/' + CheckFileType(joinfilename) + '" />'
            + '<p class="dfileuploadon"> ' + joinfilename + ''// new Date().toString().split('GMT')[0] + ''
            + '<span class="pull-right btn-group">'
            // + ' <a onclick="downloadFile();" data-container="body" data-toggle="tooltip" data-placement="top" title="Download"  type="button" class=" btn btn-success rejectbtn btn-xs"><span class="glyphicon glyphicon-save"></span></a>'
            + ' <a onclick="filename[i]" data-container="body" data-toggle="tooltip" data-placement="top" title="Download"  type="button" class=" btn btn-success fa fa-fw fa-trash btn-xs"><span class="glyphicon glyphicon-save"></span></a>'
            + ' <a onclick="deleteFile(\'' + ids + '\',\'' + joinfilename + '\');" data-container="body" data-toggle="tooltip" data-placement="top" title="Delete"  type="button" class=" btn btn-danger fa fa-fw fa-trash btn-xs"><span class="glyphicon glyphicon-remove"></span></a>'
            + '</span></p></div>';
       

        //json = { "name": joinfilename, "size": fs, "data": filedata[i], "slno": serialno };
        //FileData.push(json);

    }
    console.log(html);
    $('#dfileattach').append(html);
}
function getrequest(type) {

    if ($('input[name="ReqTracker"]:checked').val() == 'New') {
        window.location.href = '../Tracker/GetAllRequestList?reqType=' + type + '';
        //GetAllRequest(type) 
    } else if ($('input[name="ReqTracker"]:checked').val() == 'Regular') {
        window.location.href = '../Tracker/GetAllRequestList?reqType=' + type + '';
        //GetAllRequest(type) 
    } else if ($('input[name="ReqTracker"]:checked').val() == 'ReCalibration') {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
        //GetAllRequest(type) 
    } else {
        window.location.href = '../Tracker/GetAllRequestList?reqType=' + type + '';
        //GetAllRequest(type) 
    }
}

function DueForCalibrationInstruments() {
   
    if ($("#checkdueonly").is(":checked")) {
    //    alert("checked");
        var tblRowsCoun = $("#example1 td").length;
        if (tblRowsCoun > 0) {
          console.log('count');
           var oTable = $("#example1").dataTable();
          $(".one", oTable.fnGetNodes()).each(function (i, row) {
               var currentRow3 = $(this).closest("tr");
               if (typeof ($(this).closest('tr').find('td:eq(11) input[type="checkbox"]').html()) === "undefined") {
                   currentRow3.hide();
               }
                else {
                    currentRow3.show();
               }
           });
       }
       // GetInstruments();
    }
    else {
        // alert("un checked");
        window.location.href = '../Instrument/Index';
    }
}
function DueForCalibrationInstruments_Old() {
    //debugger;
    if ($("#checkdueonly").is(":checked")) {
        var tblRowsCoun = $("#example1 th").length;
        if (tblRowsCoun > 0) {
            $('#example1 > tbody > tr').each(function (row, tr) {
                var currentRow = $(this).closest("tr");
                //console.log(currentRow)
                if (currentRow.find(".class1").text() != " ") {
                   
                    currentRow.show();
                }
                else {
                    currentRow.hide();
                }
            });
        }
    }
    else {
        window.location.href = '../Instrument/Index';
    }

}

function InsertRequestList() {
   // debugger;
    var Request = new Array();
    $('#dvload').show();
    var oTable = $("#example1").dataTable();
    $(".class1:checked", oTable.fnGetNodes()).each(function (i, row) {
        var UserView = {
            instrumentId:$(this).closest('tr').find('td:eq(11) input[type="checkbox"]').val(),
            TypeValue: $(this).closest('tr').find("td:eq(11) input[type='hidden']").val(),
            RequestId: $(this).closest('tr').find('.clsRequestId').val(),
            DueDate: $(this).closest('tr').find('.clsDueDate').val(),
            //ReplacementStartDate: $(this).closest('tr').find('.clsReplacementStartDate').val(),
        } 
        
        Request.push(UserView);
    });
  
    $.ajax({
       url: '../Instrument/RegularRecaliRequest',
        type: 'POST',
        data: { userViewModelList: Request },
       dataType: "json",
    }).done(function (resultObject) {
        showSuccess("Data Saved Successfully");
        $('#dvload').hide();

        $.ajax({
            url: '../Instrument/MailInsertDueRequest',
            type: 'POST',
            data: { userViewModelList: Request },
            dataType: "json",
        }).done(function (resultObject) {
            showSuccess("Data Saved Successfully");
        });
       window.location.href = '../Instrument/Index';
    });
}
function DueInstrumentList() {

    var Request = new Array();
    var id = "";
    var oTable = $("#example2").dataTable();
    $(".class1:checked", oTable.fnGetNodes()).each(function (i, row) {
        // console.log($(this).closest('tr').find('td:eq(0)').html()); //get the enclosing tr
        var UserView = {
            instrumentId: $(this).closest('tr').find('td:eq(5) input[type="checkbox"]').val(),
            InstrumentName: $(this).closest('tr').find('td:eq(0)').html(),
            IdNo: $(this).closest('tr').find('td:eq(1)').html(),
            SubSectionCode: $(this).closest('tr').find('td:eq(2)').html(),
            TypeofScope: $(this).closest('tr').find('td:eq(3)').html(),
            DueDate: $(this).closest('tr').find('td:eq(4)').html(),
            DeptId: $(this).closest('tr').find('td:eq(5) input[name="deptId"]').val(),
            EquipmentType: $(this).closest('tr').find('td:eq(5) input[name="equipType"]').val(),
            SectionName: $(this).closest('tr').find('td:eq(5) input[name="subname"]').val(),
            Location: $(this).closest('tr').find('td:eq(5) input[name="loc"]').val(),
            ToolRoom: $(this).closest('tr').find('td:eq(5) input[name="troom"]').val(),
            InstrumentCreatedBy: $(this).closest('tr').find('td:eq(5) input[name="InstrumentCreatedBy"]').val(),
            RequestId: $(this).closest('tr').find('td:eq(5) input[name="RequestId"]').val(),
        }
        Request.push(UserView);
    });
    console.log(Request);
    $.ajax({
        url: '../Tracker/DueInstrumentAdminApprove',
        type: 'POST',
        data: { DueList: Request },
        dataType: "json",
    }).done(function (resultObject) {
        showSuccess("Data Saved Successfully");
        window.location.href = '../Tracker/DueInstrument';
    });
}


    function DueInstrumentManagerApprove() {
        var Request = new Array();
        var id = "";
        var oTable = $("#example2").dataTable();
        $(".class1:checked", oTable.fnGetNodes()).each(function (i, row) {
            // console.log($(this).closest('tr').find('td:eq(0)').html()); //get the enclosing tr
            var UserView = {
                instrumentId: $(this).closest('tr').find('td:eq(5) input[type="checkbox"]').val(),
                RequestId: $(this).closest('tr').find('td:eq(5) input[name="RequestId"]').val(),
                //InstrumentName: $(this).closest('tr').find('td:eq(0)').html(),
                //IdNo: $(this).closest('tr').find('td:eq(1)').html(),
                //SubSectionCode: $(this).closest('tr').find('td:eq(2)').html(),
                //TypeofScope: $(this).closest('tr').find('td:eq(3)').html(),
                //DueDate: $(this).closest('tr').find('td:eq(4)').html(),
                //DeptId: $(this).closest('tr').find('td:eq(5) input[name="deptId"]').val(),
                //EquipmentType: $(this).closest('tr').find('td:eq(5) input[name="equipType"]').val(),
                //SectionName: $(this).closest('tr').find('td:eq(5) input[name="subname"]').val(),
                //Location: $(this).closest('tr').find('td:eq(5) input[name="loc"]').val(),
                //ToolRoom: $(this).closest('tr').find('td:eq(5) input[name="troom"]').val(),
            }
            Request.push(UserView);
        });
        console.log(Request);
        $.ajax({
            url: '../Tracker/DueInstrumentManagerApprove',
            type: 'POST',
            data: { DueList: Request },
            dataType: "json",
        }).done(function (resultObject) {
            showSuccess("Data Saved Successfully");
            window.location.href = '../Tracker/DueTracker';
        });
    }

    function ExternalUserSubmit(type, lang) {
        var data1;
        var formData = new FormData();
        var Id = $('#RequestCalibId').val();
        //var files = $("#ImageUpload")[0].files;

        var file = $('#ImageUpload')[0].files[0];
        //formData.append("file", file);
        //data1 = {
        //    requestId: $('#RequestCalibId').val(),
        //    ReceivedBy: $('#ReceivedBy').val(),
        //    InstrumentCondition: $('#InstrumentCondition').val(),
        //    Feasiblity: $('#Feasiblity').val(),
        //    TentativeCompletionDate: $('#TentativeCompletionDate').val(),
        //    newNABL: $('#IsNABL').val(),
        //    InstrumentIdNo: $('#txtIdNo').val(),
        //    rejectReason: $('#Newreason').val(),
        //    photo: FileData
        //}
        //formData.append("ReceivedBy", $('#ReceivedBy').val());
        //formData.append("InstrumentCondition", $('#InstrumentCondition').val());
        //formData.append("Feasiblity", $('#Feasiblity').val());
        //formData.append("TentativeCompletionDate", $('#TentativeCompletionDate').val());
        //formData.append("newNABL", $('#IsNABL').val());
        //formData.append("InstrumentIdNo", $('#txtIdNo').val());
        //formData.append("acceptReason", $('#Acceptreason').val());

        formData.append("requestId", $('#RequestCalibId').val());
        formData.append("httpPostedFileBase", file);

        ////formData.append("data", JSON.stringify(data1));

        //formData.append("httpPostedFileBase", $("#ImageUpload")[0].files[0]);
        //var totalFiles = document.getElementById('ImageUpload').files.length;
        //for (var i = 0; i < totalFiles; i++) {
        //    var file = document.getElementById('ImageUpload').files[i];
        //    formData.append("httpPostedFileBase", file);
        //}
        $.ajax({
            type: 'POST',
            url: '../Tracker/ExternalUserSubmit',
            data: formData,
            dataType: 'json',
            processData: false,
            contentType: false,
            success: function (data) {
                window.location.href = '../Tracker/Request?reqType=4';
                showSuccess("You are rejected the External request. LAB admin get notified!", lang);
            },
            error: function () {
                alert('error');
            }
        });
    }

    function SaveExternalObservationSheet(lang) {
        //debugger;    

        var data = {
            Id: $('#Id').val(),
            InstrumentId: $('#instrumentId').val(),
            RequestId: $('#RequestId').val(),
            RefStd: $('#RefStd').val(),
            TempStart: $('#TempStart').val(),
            TempEnd: $('#TempEnd').val(),
            Humidity: $('#Humidity').val(),
            RefWi: $('#RefWi').val(),
            Allvalues: $('#Units').val(),
            ExternalObsCondition: $('#ExtIndicatiorCondition').val(),
            CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
            ReviewedBy: $('#ReviewedBy').val(),
            CalibrationDoneDate: $('#CalibrationDoneDate').val(),
            ReviewedDate: $('#ReviewedDate').val(),
            AdminReviewStatus: $('#AdminReviewStatus').val(),
            AdRemarks: $('#AdRemarks').val(),
        }
        $.ajax({
            url: '../Observation/InsertExternalObs',
            type: 'POST',
            data: { ExObs: data },
            dataType: "json",
        }).done(function (resultObject) {
            window.location.href = '../Tracker/Request?reqType=4';
            showSuccess("Data Saved Successfully", lang);
        });
    }

    function SaveExternalObs(lang) {
       // debugger;
        if ($('#txtIdNo').val().trim() == '') {
            showWarning("Please Enter Instrument Id Number", lang);
            return false;
        }


        var dt = $('#CalibFreqDue').val();
        var dueDate = DudeDateCalculation(dt);

        $.ajax({
            url: '../Tracker/SaveExternalObs',
            type: 'POST',
            data: { requestId: $('#RequestCalibId').val(), InstrumentID: $('#hdnInstrumentId').val(), InstrumentIdNo: $('#txtIdNo').val(), CalibFreq: dueDate },
            success: function (data) {
                window.location.href = '../Tracker/Request?reqType=4';
                //showSuccess("You are rejected the External request. LAB admin get notified!", lang);
            },
            error: function () {
                alert('error');
            }
        });
    }

    function ExternalCalibrationRequest(lang) {
        if ($('#Newreason').val().trim() == '') {
            return false;
        }
        var data1;
        data1 = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            // InstrumentCondition: $('#InstrumentCondition').val(),
            // Feasiblity: $('#Feasiblity').val(),
            // TentativeCompletionDate: $('#TentativeCompletionDate').val(),
            rejectReason: $('#Newreason').val(),
            //standardReffered: $('#StandardReffered').val()
        }

    $.ajax({
        type: 'POST',
        url: '../Tracker/ExternalCalibrationReject',
        data: data1,
        dataType: 'json',
        success: function (data) {
            window.location.href = '../Tracker/Request?reqType=4';
            showSuccess("You are rejected the External request. LAB admin get notified!", lang);
        },
        error: function () {
            alert('error');
        }
    });
    }

function ValidateObservation()
{

    var Unit = $('#Units').val();
    if (Unit == null || Unit == "") {
        showWarning("Please enter the Units", language);
        return false;;
    }
    var Temprature = $('#TempStart').val();
    if (Temprature == null || Temprature == "") {
        showWarning("Please enter the Temprature !!!", language);
        return false;
    }
    var Humidity = $('#Humidity').val();
    if (Humidity == null || Humidity == "") {
        showWarning("Please enter the Humidity", language);
        return false;
    }
    var VisualCheckCondition = $('#VisualCheckCondition').val();
    if (VisualCheckCondition == null || VisualCheckCondition == "") {
        showWarning("Please enter the Visual Check", language);
        return false;
    }
}

function SaveCertificateTemp(lang) {

    var temptName = 'test';
    //var result = $('#CalibrationResult').val();
    //var remarks = $('#Remarks').val();
    //if (result == null || result == "") {
    //    showWarning("Please enter the Calibration Result!!!", lang);
    //    return true;
    //}
    //else if (remarks == null || remarks == "") {
    //    showWarning("Please enter the Calibration Result!!!", lang);
    //    return true;
    //}
   // alert("temp");
    Swal.fire({
        title: "Are you want To Generate QR Code with Pdf file?",
        text: "You will save Certificate and Generate QR Code with Pdf file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, generate it!",
        closeOnConfirm: false
    }).then((result) => {
        // }, function (isConfirm) {
        //       if (!isConfirm) return;

        if (result.isConfirmed) {
            $.ajax({
                url: '../Certification/SaveCertificateTemp', 
                type: 'POST',
                data: {
                    requestId: $('#requestId').val(),
                    instrumentId: $('#instrumentId').val(),
                    EnvironmentCondition: '0', //$('#EnvironmentCondition').val(),
                    //Uncertainity: $('#Uncertainity').val(),
                    CalibrationResult: $('#CalibrationResult').val(),
                    Remarks: $('#Remarks').val(),
                    ExportData: $("#Pdfhtml").html(),
                    TempltateName: temptName
                }
            }).done(function (resultObject) {
                showSuccess("Result Of Calibration & Remarks Successfully", lang);
                window.location.reload();
            });
        }
    });
}
function UpdateControlCardRequest() {
    //debugger;
    var Issueno1 = $('#Issueno').val();
    if (Issueno1 == null || Issueno1 == "") {
        showWarning("Please enter the Issue No/発行№", language);
        return false;
    }
    var InstrumentId = $('#InstrumentId').val();
    var Issueno = $('#Issueno').val();
    var Request = new Array();

    var oTable = $("#TblRequest").dataTable();
    $(".Inspect:text", oTable.fnGetNodes()).each(function (i, row) {
        var InspectData = {
            Inspectiondetails: $(this).val(),
            //Inspectiondetails: $(this).closest('tr').find("td:eq(6) input[type='text']").val(),
            requestId: $(this).closest('tr').find("td:eq(6) input[type='hidden']").val(),
        }

        Request.push(InspectData);
    });

    $.ajax({
        url: '../Instrument/updateRequestforInstrument',
        type: 'POST',
        data: { reqlist: Request, InstrumentId: InstrumentId, IssueNo: Issueno },
        dataType: "json",
    }).done(function (resultObject) {
        //showSuccess("Data Saved Successfully");
        window.location.href = '../Instrument/Index';
    });

}

//function UpdateControlCardRequest() {
//    //debugger;
//    var InstrumentId = $('#InstrumentId').val();
//    var Issueno = $('#Issueno').val();
//    var Request = new Array();

//    var oTable = $("#TblRequest").dataTable();
//    $(".Inspect:text", oTable.fnGetNodes()).each(function (i, row) {
//        console.log(row);
//        var InspectData = {
//            Inspectiondetails: $(this).val(),
//           // rowdta: row,
//            //Inspectiondetails: $(this).closest('tr').find("td:eq(6) input[type='text']").val(),
//            requestId: $(this).closest('tr').find("td:eq(6) input[type='hidden']").val(),
//        }

//        Request.push(InspectData);
//    });
//    console.log("InspectData");
//    console.log(Request);
//    $.ajax({
//        url: '../Instrument/updateRequestforInstrument',
//        type: 'POST',
//        data: { reqlist: Request, InstrumentId: InstrumentId, IssueNo: Issueno },
//        dataType: "json",
//    }).done(function (resultObject) {
//        //showSuccess("Data Saved Successfully");
//        window.location.href = '../Instrument/Index';
//    });

//}

function getDepartmantDueInstruments(DueMonth) {
    window.location.href = '../Instrument/ToolRoomDepartment?DueMonth=' + DueMonth + '';
      
    if ($('input[name="Month1"]:checked').val() == '1') {
        window.location.href = '../Instrument/ToolRoomDepartment?DueMonth=' + DueMonth + '';
    } else if ($('input[name="Month2"]:checked').val() == '2') {
        window.location.href = '../Instrument/ToolRoomDepartment?DueMonth=' + DueMonth + '';
    } 
    else if ($('input[name="Month3"]:checked').val() == '3') {
        window.location.href = '../Instrument/ToolRoomDepartment?DueMonth=' + DueMonth + '';
    } 

}

function getDueInstruments(DueMonth) {
    window.location.href = '../Tracker/DueInstrument?month=' + DueMonth + '';

    if ($('input[name="Month1"]:checked').val() == '1') {
        window.location.href = '../Tracker/DueInstrument?month=' + DueMonth + '';
    } else if ($('input[name="Month2"]:checked').val() == '2') {
        window.location.href = '../Tracker/DueInstrument?month=' + DueMonth + '';
    }
    else if ($('input[name="Month3"]:checked').val() == '3') {
        window.location.href = '../Tracker/DueInstrument?month=' + DueMonth + '';
    }

}
function removeTrow(Row) {
     
    $(Row).closest('tr').remove();
}
  
function addIN() {
    var counter = $('#INTblObservation >tbody >tr').length;
    
    var headingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + counter + '" name = "Head[' + counter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "" disabled />';
    
    $('#INTblObservation > tbody:last-child').append('<tr>' + headingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value=" "/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + tbLINContentId + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
       // + '<td id="IN"><input id="SNO' + counter + '" name="SNO[' + counter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + counter + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + tbLINContentId + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
        '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + counter + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
        '<td id="IN"><input id="Actual" name="Actual[' + counter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "" /> </td>' +
        '<td id="IN"><input id="Avg" name = "Avg[' + counter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "" disabled /> </td>' +
        '<td><i style="width: 50px;" class="btn-remove-tr fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>');
    counter++;

    return false;
}

function addCW() {
    
    var CWcounter = $('#CWTblObservation >tbody >tr').length;

    
    $('#CWTblObservation > tbody').append('<tr><td style="width: 30%" id="CW"><input id="Head' + CWcounter + '" name="Head[' + CWcounter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + tbLCWContentId + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
       //'<td id="CW"><input id="SNO' + CWcounter + '" name="SNO[' + CWcounter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + tbLCWSNo + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + tbLCWContentId + '"/><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
        '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + CWcounter + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValueCW" value = "" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value=""/></td>' +
        '<td id="CW"><input id="Value1" name="Value1[' + CWcounter + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "" /> </td>' +
        '<td id="CW"><input id="Value2" name="Value2[' + CWcounter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "" /> </td>' +
        '<td id="CW"><input id="Value3" name="Value3[' + CWcounter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "" /></td>' +
        '<td id="CW"><input id="Average" name="Average[' + CWcounter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "" /></td>' +
        '<td id="CW"><input id="Percentage" name = "Percentage[' + CWcounter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = ""  disabled > </td>'+
        '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>');
    CWcounter++;
 //   CWSNo++;
    return false;
}
function addSW() {
  var SWcounter = $('#SWTblObservation >tbody >tr').length;
    //var SWSNo = $('#SWTblObservation >tbody >tr').length;

    //tbLSWSNo += 1;
   
    $('#SWTblObservation > tbody:last-child').append('<tr><td style="width: 30%" id="SW"><input id="Head' + SWcounter + '" name="Head[' + SWcounter + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + tbLSWContentId + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
        //'<td id="SW"><input id="SNO' + SWcounter + '" name="SNO[' + SWcounter + 1 + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + tbLSWSNo + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + tbLSWContentId + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
        '<td id="SW"><input id="SWValue1" name="SWValue1[' + SWcounter + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
        '<td id="SW"><input id="SWValue2" name="SWValue1[' + SWcounter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
        '<td id="SW"><input id="SWValue3" name="SWValue1[' + SWcounter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
        '<td id="SW"><input id="SWValue4" name="SWValue1[' + SWcounter + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = ""  /> </td>' +
        '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>');

   SWcounter++;
    //SWSNo++;
    return false;
}

function BindInternalObservationTable(UserRole)
{
    $.ajax({
        type: 'GET',
        url: '../Observation/GetObservationById',
        dataType: 'json',
        data: { InstrumentId: $('#InstrumentId').val(), RequestId: $('#RequestId').val(), TemplateObservationId: $('#TemplateObservationId').val()},
    }).done(function (resultObject) {
        var rowNewContentCW = "";
        var rowNewContentIN = "";
        var rowNewContentSW = "";
        var rowNewContentSE = "";

        var rowhead = "";
        var rowContentCW = '';
        var rowContentIN = '';
        var rowContentSW = '';
        var rowContentSE = '';
        
        
        var icount = 0;
        var InCount = 0;
        var tblContentRowcount = 0;
        var Permissible = "";
       
        for (let i = 0; i < resultObject.length; i++) {
            console.log("resultObject");
            console.log(resultObject);
            var IdName = resultObject[i].typeOfContent;

             tableIN = '<table id="INTblObservation" style="text-align:center;">';
             tableCW = '<table id="CWTblObservation" style="text-align:center;">';
            tableSW = '<table id="SWTblObservation" style="text-align:center;">';
            tableSE = '<table id="SETblObservation" style="text-align:center;">';
            if (IdName == 'CW') {
               // var rowheadCW = '<thead><tr height="50" ><td>説明/ Description</td><td>いいえ／No.</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td><td>' + resultObject[i].contentSubTitle5 + '</td></tr></thead>';
                var rowheadCW = '<thead><tr height="50" ><td>説明/ Description</td><td>' + resultObject[i].contentSubTitle6 + '</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td><td>' + resultObject[i].contentSubTitle5 + '</td></tr></thead>';

            }
            else if (IdName == 'SW') {
               // var rowheadSW = '<thead><tr><td height="50">説明/ Description</td><td>いいえ／No.</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td></tr></thead>';
                var rowheadSW = '<thead><tr><td height="50">説明/ Description</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td></tr></thead>';

            }
            else if (IdName == 'IN') {
               // var rowheadIN = '<tr><td height="50">説明/ Description</td><td>いいえ／No.</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td></tr>';
                var rowheadIN = '<tr><td height="50">説明/ Description</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td></tr>';
            }
            else if (IdName == 'SE') {
                // var rowheadIN = '<tr><td height="50">説明/ Description</td><td>いいえ／No.</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td></tr>';
                var rowheadSE = '<thead><tr><td height="50">説明/ Description</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td></tr></thead>';
            }
            
            rowBodyCW = '<tbody>';
            rowBodyIN = '<tbody>';
            rowBodySW = '<tbody>';
            rowBodySE = '<tbody>';
            var objcontentCount = resultObject[i].contentCount;

            var title = "";
            var contentsubheadingIN = "";
            var contentsubheadingCW = "";
            var contentsubheadingSW = "";
            var contentid = resultObject[i].id;

            if (tbLINContentId == resultObject[i].id) {

                tblContentRowcount += 1;
            }
            else {

                tblContentRowcount = 1;
            }

            if (IdName == 'IN') {

                if (InCount >= 3) {// if (InCount == 3) {
                    InCount = 0
                }
                if (objcontentCount > 1) {
                    InCount = InCount + 1;
                    if ((InCount == 2) || (InCount == 4) || (InCount == 5) || (InCount == 6)) {
                        title = resultObject[i].contentValue.trim();

                        contentsubheadingIN = '<td class="TblInHeader" style="width: 45%;border:2px solid #f6f6f6;border-left:none;border-right:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';

                    }
                    else {

                        title = "";// resultObject[i].contentValue.trim();
                        contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                    }
                }
                else //if (InCount == 1)
                {

                    title = resultObject[i].contentValue.trim();
                    contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                }

            }
            if (tblContentRowcount == 1) {

                title = resultObject[i].contentValue.trim(); // resultObject[i].contentValue.trim();
             
                contentsubheadingIN = '<td style="width: 45%;border:2px;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                contentsubheadingSW = '<td style="width: 30%;border:none;"  id="SW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title + '" disabled />';
                contentsubheadingCW = '<td style="width: 30%;border:none;"  id="CW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title.slice(0, title.length - 4) + '" disabled />';
                contentsubheadingSE = '<td style="width: 30%;border:2px;"  id = "SE" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                console.log(contentsubheadingSE);
            }
            else {
                title = "";

                contentsubheadingIN = '<td style="width: 45%;border:2px;border-left:none;border-right:none;border-top:2px solid #f6f6f6;"  id = "IN"> <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                contentsubheadingSW = '<td style="width: 30%;border:2px solid #f6f6f6;border-left:none;border-right:none;border-top:none;"  id = "SW" id="SW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title + '" disabled />';
                contentsubheadingCW = '<td style="width: 30%;border:2px solid;border-left:none;border-right:none;border-top:none;"  id = "CW" id="CW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title.slice(0, title.length - 4) + '" disabled />';
                contentsubheadingSE = '<td style="width: 30%;border:2px;border-left:none;border-right:none;border-top:2px solid #f6f6f6;"  id = "SE"> <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                
            }
            var sno = resultObject[i].sno;
            var addsno = sno + 1;
            tbLINContentId = resultObject[i].id;
            tbLCWContentId = resultObject[i].id;
            tbLSWContentId = resultObject[i].id;
            if (UserRole == 4) {
                deleteRow = '</tr>';
            }
            else {
                deleteRow = '<td><i style="width: 40px;" class="btn-remove-tr fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';

            }
            if (IdName == 'CW') {
               
                rowContentCW += '<tr>' + contentsubheadingCW + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" class="class="HiddenContentvalueId"  value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" class="HiddenContentId" value="' + resultObject[i].id + '"/></td>' +
                    '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValueCW" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                    '<td id="CW"><input id="Value1" name="Value1[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "' + resultObject[i].measuedValue1 + '" /> </td>' +
                    '<td id="CW"><input id="Value2" name="Value2[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "' + resultObject[i].measuedValue2 + '" /> </td>' +
                    '<td id="CW"><input id="Value3" name="Value3[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "' + resultObject[i].measuedValue3 + '" /></td>' +
                    '<td id="CW"><input id="Average" name="Average[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "' + resultObject[i].average + '" /></td>' +
                    '<td id="CW"><input id="Percentage" name = "Percentage[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = "' + resultObject[i].percent + '"  disabled > </td>' + deleteRow
                 
                rowNewContentCW = '<tr><td style="width: 30%" id="CW"><input id="Head' + icount + '" name="Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "" disabled /><input type="hidden" id="HiddenContentvalueId" class="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" class="HiddenContentId" value="' + resultObject[i].id + '"/></td>' +
                    '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValueCW" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                    '<td id="CW"><input id="Value1" name="Value1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "" /> </td>' +
                    '<td id="CW"><input id="Value2" name="Value2[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "" /> </td>' +
                    '<td id="CW"><input id="Value3" name="Value3[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "" /></td>' +
                    '<td id="CW"><input id="Average" name="Average[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "" /></td>' +
                    '<td id="CW"><input id="Percentage" name = "Percentage[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = ""  disabled > </td>' + deleteRow
                   
                Permissible ='<tr><td> Note : Permissible Limit in % is :</td><td><input type="text" value="' + resultObject[i].permissibleLimit + '" id="PermissibleLimit" class="PermissibleLimit" required="required"></td></tr>';
           
            }
            else if (IdName == 'SW') {
            
                rowContentSW += '<tr>' + contentsubheadingSW + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                    //'<td id="SW"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                    '<td id="SW"><input id="SWValue1" name="SWValue1[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue + '" /> </td>' +
                    '<td id="SW"><input id="SWValue2" name="SWValue1[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue1 + '" /> </td>' +
                    '<td id="SW"><input id="SWValue3" name="SWValue1[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue2 + '" /> </td>' +
                    '<td id="SW"><input id="SWValue4" name="SWValue1[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue3 + '"  /> </td>' + deleteRow
                    //'<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';
              
                rowNewContentSW = '<tr><td style="width: 30%" id="SW"><input id="Head' + icount + '" name="Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                    //'<td id="SW"><input id="SNO' + i + i + '" name="SNO[' + i + 1 + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + addsno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                    '<td id="SW"><input id="SWValue1" name="SWValue1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                    '<td id="SW"><input id="SWValue2" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                    '<td id="SW"><input id="SWValue3" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                    '<td id="SW"><input id="SWValue4" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = ""  /> </td>' + deleteRow
                   // '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';
               // tbLSWSNo = addsno;
            }
            else if (IdName == 'IN') {
                var deleteRow = "";
                if (UserRole == 4) {
                    deleteRow = '</tr>';
                }
                else {
                    deleteRow = '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';

                }
                rowContentIN += '<tr>' + contentsubheadingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                   // + '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                    '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                    '<td id="IN"><input id="Actual" name="Actual[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "' + resultObject[i].actualValue + '" /> </td>' +
                    '<td id="IN"><input id="Avg" name = "Avg[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "' + resultObject[i].instrumentError + '" disabled /> </td>' + deleteRow
                
                title = "";
                var headingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + icount + '" name = "Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "" disabled />';

                rowNewContentIN = '<tr>' + headingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value=" "/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
                    //+ '<td id="IN"><input id="SNO' + icount + '" name="SNO[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + addsno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
                    '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                    '<td id="IN"><input id="Actual" name="Actual[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "" /> </td>' +
                    '<td id="IN"><input id="Avg" name = "Avg[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "" disabled /> </td>' + deleteRow
                    //'<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';
                
            }
            else if (IdName == 'SE')
            {
                
                var filedatamap = resultObject[i].fileuploadmapping;
               
                var filecount = filedatamap.length;
                var enable = "";
                var hideval = "";
                
                if (IsStatus > 28) {
                   
                    enable = "disabled";
                    hideval = "hidden";
                }
                var html = "";
                for (var j = 0; j < filecount; j++)
                {              
                    var joinfilename = filedatamap[j].fileName;                   
                    var ids = filedatamap[j].id;
                    html += '<div id="' + ids + '" ><span class=""><img height="15" width="15" src="../image/' + CheckFileType(joinfilename) + '" />'
                        + '<a id="pic" href="../Observation/' + joinfilename + '">' + joinfilename +'</a>'
                        + ' <a onclick="ObservationFileDeleteAlert(\'' + ids + '\',\'' + joinfilename + '\');" data-container="body" data-toggle="tooltip" data-placement="top" title="Delete"  type="button" class="fa fa-fw fa-trash" ' + hideval +'></a>'
                        + '</div>';
                }
                tbLSEContentId = resultObject[i].id;
                rowContentSE += '<tr id=SE_' + addsno + '>' + contentsubheadingSE + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                    // +btn btn-danger rejectbtn btn-xs '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                    '<td id="SE"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="" value = "Refer attached file/添付ファイル参照" disabled/><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '" '+enable+'/></td>' +
                    '<td id="SE"><input id="files_' + j + '" name="SE_' + j + '" class="clsFile" type="file" multiple ContentEditable="false" ' + enable +'/></td>' +
                    '<td id="SE"><div class="form-group dynAttachmentfiles" id="dfileattach">'+html+'</div></td></td></tr>';
                addsno = sno + 1;
            }
           
            /*<button id="cbrowse" type="button" class="btn btn-success btn-xs" data-toggle="tooltip" data - placement="top" onclick = "document.getElementById("files1").click();" > <span class="trn">Browse</span></button >*/
        }
        if (UserRole == 4) {
            rowNewContentCW = "";
            rowNewContentIN = "";
            rowNewContentSW = "";
            rowNewContentSE = "";
        }
        rowNewContentCW = "";      
        rowNewContentSW = "";
        rowBodyCW += rowContentCW;
        rowBodyIN += rowContentIN;
        rowBodySW += rowContentSW;
        rowBodySE += rowContentSE;
        rowBodyCW = rowBodyCW + rowNewContentCW + '</tbody><tfoot id="tblCW_footer" class="tblCW_footer" style="font-weight:900;">' + Permissible +'</tfoot></table><br/>';
        rowBodyIN = rowBodyIN + rowNewContentIN + '</tbody></table><br/>';
        rowBodySW = rowBodySW + rowNewContentSW + '</tbody></table><br/>';
        rowBodySE = rowBodySE + rowNewContentSE + '</tbody></table><br/>';
       
        tableCW += rowheadCW;
        tableCW += rowBodyCW;

        tableIN = tableIN + rowheadIN;
        tableIN += rowBodyIN;

        tableSW += rowheadSW;
        tableSW += rowBodySW;

        tableSE += rowheadSE;
        
        tableSE += rowBodySE;
       
        if (UserRole != 4) {
            addInRow = '<br><tr><p align="right"><button style="align-content: right;" class="addIN" id = "addIN" name = "addIN" type = "button"	class="btn btn-primary trn">Add New Row	</button></p></tr>';

            addCWRow = '<br><tr><p align="right"><button style="align-content: right;" class="addCW" id = "addCW" name = "addCW" type = "button"	class="btn btn-primary trn">Add New Row	</button></p></tr>';

            addSWRow = '<br><tr><p align="right"><button style="align-content: right;" class="addSW" id = "addSW" name = "addSW" type = "button"	class="btn btn-primary trn">Add New Row	</button></p></tr>';
        }
        else {
            addSWRow = "";
            addInRow = "";
            addCWRow = "";

        }
        if (rowContentSW == "") {
            
            tableSW = "";
            addSWRow = "";
        }
        if (rowContentIN == "") {
            
            tableIN = "";
            addInRow = "";
        }
        if (rowContentCW == "") {
           
            tableCW = "";
            addCWRow = "";
        }

        if (rowContentSE == "") {
            
            tableSE = "";
            addSERow = "";
        }
        //temp
       
        $("#dvTable").html("");
        var tables = addCWRow + tableCW + "<br/>" + addInRow + tableIN + "<br/>" + addSWRow + tableSW + "<br/>" +  tableSE;
       
        var dvTable = $("#dvTable");
        dvTable.append(tables);
        
    });

}
function removeTrowQuarantine(Row,id) {

   $(Row).closest('tr').remove();
  
    $.ajax({
        url: '../Instrument/InActiveQuarantineInstrument',
        type: 'POST',
        data: { instrumentId: id },
        dataType: "json",
    }).done(function (resultObject) {
        window.location.href = '../Instrument/QuratineList';
      
    });
}
function GenerateInternalObservationContent() {
        $('#dvTable').html("");
        var rowNewContentCW = "";
        var rowNewContentIN = "";
    var rowNewContentSW = "";
    var rowNewContentSE = "";
        var obsContentValueId = "";
        var rowContentCW = "";
        var rowContentIN = "";
    var rowContentSW = "";
    var rowContentSE = "";
        var rowBodyCW = "";
        var rowBodyIN = "";
    var rowBodySW = "";
    var rowBodySE = "";
        var tblId = "";
        var tableIN = "";
    var tableCW = "";
    var tableCW = "";
    var tableSE = "";
    var tableSM = "";

        var rowheadCW = "";
        var rowheadSW = "";
        var rowheadIN = "";
    var rowheadSE = "";
        var icount = 0;
        var ContentDataList = new Array();
        var addInRow = "";
        var addCWRow = "";
        var addSWRow = "";
        var tblContentRowcount = 0;
    var Permissible = "";
    var serialno = 1;
        $('#ContentSelect > :selected').each(function () {
            //for xml start       
            var ContentData =
            {
                ContentId: $(this).val()//,

            }
            ContentDataList.push(ContentData);

        });
        $.ajax({
            dataType: 'json',
            url: '../Observation/GetObservationContentSelectedList',
            type: 'POST',
            data: { Contents: ContentDataList, InstrumentId: $('#InstrumentId').val(), TemplateObservationId: $('#TemplateObservationId').val() },
        }).done(function (resultObject) {

            tbllengthcount = resultObject.length;
            var InCount = 0;
            for (let i = 0; i < resultObject.length; i++) {
                var IdName = resultObject[i].typeOfContent;
               // alert(IdName);
                if (tbLINContentId == resultObject[i].id) {

                    tblContentRowcount += 1;
                }
                else {

                    tblContentRowcount = 1;
                }
                tblId = 'TblObservation' + i;
                if (IdName == 'CW') {

                    rowheadCW = '<thead><tr height="50" ><td>説明/ Description</td><td>' + resultObject[i].contentSubTitle6 + '</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td><td>' + resultObject[i].contentSubTitle5 + '</td></tr></thead>';

                }
                else if (IdName == 'SW') {

                    //rowheadSW = '<thead><tr><td height="50">説明/ Description</td><td>いいえ／No.</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td></tr></thead>';
                    rowheadSW = '<thead><tr><td height="50">説明/ Description</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td><td>' + resultObject[i].contentSubTitle4 + '</td></tr></thead>';
                }
                else if (IdName == 'IN') {

                    rowheadIN = '<tr><td height="50">説明/ Description</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td></tr>';
                }
                else if (IdName == 'SE') {
                   // alert(IdName);
                    rowheadSE = '<tr><td height="50">説明/ Description</td><td>' + resultObject[i].contentSubTitle1 + '</td><td>' + resultObject[i].contentSubTitle2 + '</td><td>' + resultObject[i].contentSubTitle3 + '</td></tr>';
                }
                ///////////////////// contentvalueid>0////////////////<td style="width: 15%">
                tbLINContentId = resultObject[i].id;
                tbLCWContentId = resultObject[i].id;
                tbLSWContentId = resultObject[i].id;

                if (tblContentRowcount == 1) {
                   // alert("1");
                    title = resultObject[i].contentValue.trim(); // resultObject[i].contentValue.trim();
                   // alert(title);
                    contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                    contentsubheadingSW = '<td style="width: 30%;border:none;"  id = "SW" id="SW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title + '" disabled />';
                    contentsubheadingCW = '<td style="width: 30%;border:none;"  id = "CW" id="CW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title.slice(0, title.length - 4) + '" disabled />';
                    contentsubheadingSE = '<td style="width: 30%;border:none;"  id = "SE" id="SE"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title + '" disabled />';
                    //alert(contentsubheadingSE);
                }
                else {
                   // alert("2");
                    title = "";
                    contentsubheadingIN = '<td style="width: 45%;border:2px solid #f6f6f6;border-left:none;border-right:none;border-top:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                    contentsubheadingSW = '<td style="width: 30%;border:2px solid #f6f6f6;border-left:none;border-right:none;border-top:none;"  id = "SW" id="SW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title + '" disabled />';
                    contentsubheadingCW = '<td style="width: 30%;border:2px solid #f6f6f6;border-left:none;border-right:none;border-top:none;"  id = "CW" id="CW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title.slice(0, title.length - 4) + '" disabled />';
                    contentsubheadingSE = '<td style="width: 45%;border:2px solid #f6f6f6;border-left:none;border-right:none;border-top:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                    // contentsubheadingSE = '<td style="width: 30%;border:none;"  id = "SM" id="SM"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + title + '" disabled />';
                }

                if (resultObject[i].obsContentValueId > 0) {
                    var sno = resultObject[i].sno;
                    var addsno = sno + 1;
                    var objcontentCount = resultObject[i].contentCount;
                  
                    var title = "";
                    var contentsubheadingIN = "";
                    var contentsubheadingSE = "";
                    if (IdName == 'IN') {

                        if (InCount == 3) {
                            InCount = 0;
                        }
                        if (objcontentCount > 1) {
                            InCount = InCount + 1;
                            if (InCount == 2) {
                                title = resultObject[i].contentValue.trim();

                                contentsubheadingIN = '<td style="width: 45%;border:2px solid #f6f6f5;border-left:none;border-right:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';

                            }
                            else {

                                title = "";// resultObject[i].contentValue.trim();
                                contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                            }
                        }
                        else //if (InCount == 1)
                        {
                            title = resultObject[i].contentValue.trim();
                            contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';

                        }

                    }
                    //if (IdName == 'SE') {
                    //    alert("se");
                    //    title = resultObject[i].contentValue.trim();
                    //    contentsubheadingSM = '<td style="width: 45%;border:none;"  id = "SM" > <input id="Head' + i + i + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';

                    //}
                   // sno = resultObject[i].sno;
                    if (IdName == 'CW') {
                        tbLCWContentId = resultObject[i].id;
                        rowContentCW += '<tr><td style="width: 30%" id="CW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].contentValue.trim().slice(0, resultObject[i].contentValue - 3) + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/></td>' +
                            '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                            '<td id="CW"><input id="Value1" name="Value1[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "' + resultObject[i].measuedValue1 + '" /> </td>' +
                            '<td id="CW"><input id="Value2" name="Value2[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "' + resultObject[i].measuedValue2 + '" /> </td>' +
                            '<td id="CW"><input id="Value3" name="Value3[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "' + resultObject[i].measuedValue3 + '" /></td>' +
                            '<td id="CW"><input id="Average" name="Average[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "' + resultObject[i].average + '" /></td>' +
                            '<td id="CW"><input id="Percentage" name = "Percentage[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = "' + resultObject[i].percent + '"  disabled ></td>';
                        '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';

                        // addsno = sno + 1;
                        //  tbLCWSNo = addsno;

                        rowNewContentCW = '<tr><td style="width: 30%" id="CW"><input id="Head' + icount + '" name="Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "" disabled /><input type="hidden" id="HiddenContentvalueId" class="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId" class="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/></td>' +
                            '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                            // '<td id="CW"><input id="SNO' + i + i + '" name="SNO[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + addsno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                            '<td id="CW"><input id="Value1" name="Value1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "" /> </td>' +
                            '<td id="CW"><input id="Value2" name="Value2[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "" /> </td>' +
                            '<td id="CW"><input id="Value3" name="Value3[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "" /></td>' +
                            '<td id="CW"><input id="Average" name="Average[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "" /></td>' +
                            '<td id="CW"><input id="Percentage" name = "Percentage[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = ""  disabled > </td>' +
                            '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';

                    }
                    else if (IdName == 'SW') {
                        tbLSWContentId = resultObject[i].id;
                        rowContentSW += '<tr><td style="width: 30%" id="SW"><input id="Head' + icount + '" name="Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].contentValue.trim() + '" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                            //'<td id="SW"><input id="SNO' + i + i + '" name="SNO[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                            '<td id="SW"><input id="SWValue1" name="SWValue1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue + '" /> </td>' +
                            '<td id="SW"><input id="SWValue2" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue1 + '" /> </td>' +
                            '<td id="SW"><input id="SWValue3" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue2 + '" /> </td>' +
                            '<td id="SW"><input id="SWValue4" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue3 + '"  /> </td>' +
                            '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';
                        // addsno = sno + 1;
                        // tbLSWSNo = addsno;

                        rowNewContentSW = '<tr><td style="width: 30%" id="SW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "0" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                            // '<td id="SW"><input id="SNO' + icount + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + addsno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                            '<td id="SW"><input id="SWValue1" name="SWValue1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                            '<td id="SW"><input id="SWValue2" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                            '<td id="SW"><input id="SWValue3" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                            '<td id="SW"><input id="SWValue4" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = ""  /> </td>' +
                            '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';

                    }
                    else if (IdName == 'IN') {
                        tbLINContentId = resultObject[i].id;
                        rowContentIN += '<tr>' + contentsubheadingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                            // + '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                            '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                            '<td id="IN"><input id="Actual" name="Actual[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "' + resultObject[i].actualValue + '" /> </td>' +
                            '<td id="IN"><input id="Avg" name = "Avg[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "' + resultObject[i].instrumentError + '" disabled /> </td>' +
                            '<td><i style="width: 40px;" class="fa fa-fw fa-trash btn-remove-tr" onclick="removeTrow(this);"></i></td></tr>';
                        addsno = sno + 1;
                        // tbLINSNo = addsno;
                        title = "";
                        rowNewContentIN = '<tr>' + contentsubheadingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value=""/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
                            //+ '<td id="IN"><input id="SNO' + icount + '" name="SNO[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + addsno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
                            '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                            '<td id="IN"><input id="Actual" name="Actual[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "' + resultObject[i].actualValue + '" /> </td>' +
                            '<td id="IN"><input id="Avg" name = "Avg[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "' + resultObject[i].instrumentError + '" disabled /> </td>' +
                            '<td><i style="width: 40px;" class="fa fa-fw fa-trash btn-remove-tr" onclick="removeTrow(this);"></i></td></tr>';
                    }
                    else if (IdName == 'SE') {
                        //alert(IdName);
                        tbLSEContentId = resultObject[i].id;
                        rowContentSE += '<tr id=SE_' + serialno + '>' + contentsubheadingSE + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                        // + '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                            '<td id="SE"><label id="MeasuedValue" name="MeasuedValue[' + i + i + '] " class="Tables-AndTablesTextBox TblInHeader MeasuedValue">"Refer attached file/添付ファイル参照"</label><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '" /></td > ' +
//                            < input id = "MeasuedValue" name = "MeasuedValue[' + i + i + '] " type = "text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "Refer attached file/添付ファイル参照" /> <input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '" /></td > ' +
                            '<td id="SE"><input id="files_' + serialno + '" name="SE_' + serialno + '" class="clsFile" type="file" multiple ContentEditable="false"/></td>' +
                        '<td id="SE" ><div class="Tables-AndTablesTextBox form-group dynAttachmentfiles" id="dfileattach"></div></td></td></tr>';
                        addsno = sno + 1;
                    }
                        //else if (IdName == 'SE') {
                    //    tbLSEContentId = resultObject[i].id;
                    //    rowContentSE += '<tr>' + contentsubheadingSE + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                    //        // + '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                    //        '<td id="SM"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                    //        '<td id="SM"><input id="Actual" name="Actual[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "' + resultObject[i].actualValue + '" /> </td>' +
                    //        '<td id="SM"><input id="Avg" name = "Avg[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "' + resultObject[i].instrumentError + '" disabled /> </td>' +
                    //        '<td><i style="width: 40px;" class="fa fa-fw fa-trash btn-remove-tr" onclick="removeTrow(this);"></i></td></tr>';
                    //    addsno = sno + 1;
                    //}
                    icount += i;

                }

                ///////////////////// contentvalueid>0 ////////////////
                else if (resultObject[i].obsContentValueId == 0) {

                    for (let j = 1; j <= resultObject[i].contentCount; j++) {

                        var objcontentCount = resultObject[i].contentCount;

                        var title = "";
                        var contentsubheadingIN = "";

                        if ((objcontentCount > 1) && (j == 2)) {//new mode

                            title = resultObject[i].contentValue.trim();
                            contentsubheadingIN = '<td class="TblInHeader" style="width: 45%;border:2px solid #f6f6f5;border-left:none;border-right:none;"  id = "IN" > <input id="Head' + i + j + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';

                        }
                        else if (objcontentCount == 1) {

                            title = resultObject[i].contentValue.trim();
                            contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + j + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                        }
                        else {

                            title = " ";
                            contentsubheadingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + i + j + '" name = "Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "' + title + '" disabled />';
                        }
                        if (IdName == 'CW') {

                            rowContentCW += '<tr><td style="width: 30%" id="CW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].contentValue.slice(0, resultObject[i].contentValue.length - 4) + '" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" class="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" class="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/></td>' +
                                //   '<td id="CW"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = ' + j + ' disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValueCW" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                '<td id="CW"><input id="Value1" name="Value1[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "' + resultObject[i].measuedValue1 + '" /> </td>' +
                                '<td id="CW"><input id="Value2" name="Value2[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "' + resultObject[i].measuedValue2 + '" /> </td>' +
                                '<td id="CW"><input id="Value3" name="Value3[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "' + resultObject[i].measuedValue3 + '" /></td>' +
                                '<td id="CW"><input id="Average" name="Average[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "' + resultObject[i].average + '" /></td>' +
                                '<td id="CW"><input id="Percentage" name = "Percentage[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = "' + resultObject[i].percent + '"  disabled > </td>' +
                                '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';

                            rowNewContentCW = '<tr><td style="width: 30%" id="CW"><input id="Head' + icount + '" name="Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = ""  /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId"  class="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><td> ' +
                                //  '<td id="CW"><input id="SNO' + icount + '" name="SNO[' + icount + 1 + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = ' + j + 1 + ' disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                                '<td id="CW"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValueCW" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                '<td id="CW"><input id="Value1" name="Value1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader Value1" value = "" /> </td>' +
                                '<td id="CW"><input id="Value2" name="Value2[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value2" value = "" /> </td>' +
                                '<td id="CW"><input id="Value3" name="Value3[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Value3" value = "" /></td>' +
                                '<td id="CW"><input id="Average" name="Average[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Average" value = "" /></td>' +
                                '<td id="CW"><input id="Percentage" name = "Percentage[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Percentage" value = ""  disabled > </td>' +
                                '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';
                        
                            Permissible = '<tr><td> Note : Permissible Limit in % is :</td><td><input type="text" value="' + resultObject[i].permissibleLimit + '" id="PermissibleLimit" class="PermissibleLimit" required="required"></td></tr>';

                        }
                        else if (IdName == 'SW') {
                            rowContentSW += '<tr><td style="width: 30%" id="SW"><input id="Head' + i + i + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].contentValue.trim() + '" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                //'<td id="SW"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + j + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                '<td id="SW"><input id="SWValue1" name="SWValue1[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue + '" /> </td>' +
                                '<td id="SW"><input id="SWValue2" name="SWValue1[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue1 + '" /> </td>' +
                                '<td id="SW"><input id="SWValue3" name="SWValue1[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue2 + '" /> </td>' +
                                '<td id="SW"><input id="SWValue4" name="SWValue1[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + resultObject[i].measuedValue3 + '"  /> </td>' +
                                '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>'


                            rowNewContentSW = '<tr><td style="width: 30%" id="SW"><input id="Head' + icount + '" name="Head[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "" disabled /><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                               // '<td id="SW"><input id="SNO' + icount + '" name="SNO[' + icount + 1 + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader"  value = "' + j + 1 + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                                '<td id="SW"><input id="SWValue1" name="SWValue1[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                                '<td id="SW"><input id="SWValue2" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                                '<td id="SW"><input id="SWValue3" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = "" /> </td>' +
                                '<td id="SW"><input id="SWValue4" name="SWValue1[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader"  value = ""  /> </td>' +
                                '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>'

                        }
                        else if (IdName == 'IN') {

                            rowContentIN += '<tr>' + contentsubheadingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                               // + '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + j + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                                '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "' + resultObject[i].measuedValue + '" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                '<td id="IN"><input id="Actual" name="Actual[' + i + i + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "' + resultObject[i].actualValue + '" /> </td>' +
                                '<td id="IN"><input id="Avg" name = "Avg[' + i + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "' + resultObject[i].instrumentError + '" disabled /> </td>' +
                                '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>';
                            var headingIN = '<td style="width: 45%;border:none;"  id = "IN" > <input id="Head' + icount + '" name = "Head[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader" value = "" disabled />';

                            rowNewContentIN = '<tr>' + headingIN + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value=""/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
                                //+ '<td id="IN"><iheadingINnput id="SNO' + icount + '" name="SNO[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + j + 1 + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="0"/></td>' +
                                '<td id="IN"><input id="MeasuedValue" name="MeasuedValue[' + icount + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="0"/></td>' +
                                '<td id="IN"><input id="Actual" name="Actual[' + icount + ']"  type="text" class="Tables-AndTablesTextBox TblInHeader Actual" value = "" /> </td>' +
                                '<td id="IN"><input id="Avg" name = "Avg[' + icount + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader Avg"   value = "" disabled /> </td>' +
                                '<td><i style="width: 40px;" class="fa fa-fw fa-trash" onclick="removeTrow(this);"></i></td></tr>'

                            tbLINContentId = resultObject[i].id;
                           // tbLINSNo = j;

                        }
                        else if (IdName == 'SE') {
                           // alert("test");
                            rowContentSE += '<tr id=SE_'+serialno +'>' + contentsubheadingSE + '<input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].obsContentValueId + '"/><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                                // + '<td id="IN"><input id="SNO' + i + i + '" name="SNO[' + i + ']" type = "text" class="Tables-AndTablesTextBox TblInHeader order"  value = "' + resultObject[i].sno + '" disabled /><input type="hidden" id="HiddenContentId" name="HiddenContentId" value="' + resultObject[i].id + '"/><input type="hidden" id="HiddenContentvalueId" name="HiddenContentvalueId" value="' + resultObject[i].ObsContentValueId + '"/></td>' +
                               // '<td id="SE"><input id="MeasuedValue" name="MeasuedValue[' + i + i + '] " type="text" class="Tables-AndTablesTextBox TblInHeader MeasuedValue" value = "Refer attached file/添付ファイル参照" /><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '"/></td>' +
                                '<td id="SE"><label id="MeasuedValue" name="MeasuedValue[' + i + i + '] " class="Tables-AndTablesTextBox TblInHeader MeasuedValue">"Refer attached file/添付ファイル参照"</label><input type="hidden" id="HiddenMappingId" name="HiddenMappingId" value="' + resultObject[i].contentMappingId + '" /></td > ' +
                                '<td id="SE"><input id="files_' + serialno + '" name="SE_' + serialno + '" class="clsFile" type="file" multiple/></td>' +
                                '<td id="SE" style=""><div class="form-group dynAttachmentfiles" id="dfileattach"></div></td></td></tr>';
                           // console.log(rowContentSE);
                           //<button id="cbrowse" type="button" class="btn btn-success btn-xs" data-toggle="tooltip" data - placement="top" onclick = "document.getElementById("files1").click();" ><span class="trn">Browse</span></button >
                            // '<td id="SE"><button id="bfile" type="button" class="btn btn-success btn-xs" data-toggle="tooltip" data-placement="top" title="" onclick="document.getElementById("files1").click();" data-title="Add Attachments"> <span class="trn">Browse</span></button><input type="file" id="files1" name="files[]" multiple style="display: none;" ></input></td>' +
                            //<input id="files1" name="files1" class="clsFile" type="file" multiple/><button id="cbrowse" type="button" class="btn btn-success btn-xs" data-toggle="tooltip" data - placement="top" onclick = "document.getElementById("files1").click();" > <span class="trn">Browse</span></button >
                            //style="display:none;" <button id="cbrowse" type="button" class="btn btn-success btn-xs" data-toggle="tooltip" data - placement="top" onclick = "document.getElementById("files1").click();" > <span class="trn">Browse</span></button >
                            addsno = sno + 1;
                            serialno = serialno + 1;
                        }
                        icount = i + j;

                    }

                }

            }

            icount = icount + 1;

            rowBodyCW = rowContentCW;
            rowBodyIN = rowContentIN;
            rowBodySW = rowContentSW;
            rowBodySE = rowContentSE;

            tableIN = '<table id="INTblObservation" style="text-align:center;">';
            tableCW = '<table id="CWTblObservation" style="text-align:center;">';
            tableSW = '<table id="SWTblObservation" style="text-align:center;">';
            tableSE = '<table id="SETblObservation" style="text-align:center;">';
            rowNewContentCW = "";//no new row for and add row for CW
            var Permissible = '<tr><td> Note : Permissible Limit in % is :</td><td><input type="text" value="" id="PermissibleLimit" class="PermissibleLimit" required="required"></td></tr>';
            rowBodyCW = rowBodyCW + rowNewContentCW + '</tbody><tfoot id="tblCW_footer" class="tblCW_footer" style="font-weight:900;">' + Permissible + '</tfoot></table><br/>';
            // rowBodyCW = '<tbody>' + rowContentCW + '</tbody></table><br/>';
            tableCW += rowheadCW;
            tableCW += rowBodyCW;

            rowBodyIN = '<tbody>' + rowContentIN + '</tbody></table><br/>';
            tableIN += rowheadIN;
            tableIN += rowBodyIN;

            rowBodySW = '<tbody>' + rowContentSW + '</tbody></table><br/>';
            tableSW += rowheadSW;
            tableSW += rowBodySW;

            rowBodySE = '<tbody>' + rowContentSE + '</tbody></table><br/>';
            tableSE += rowheadSE;
            tableSE += rowBodySE;
            //alert(tableSE);
            addInRow = '<br><tr><p align="right"><button style="align-content: right;" class="addIN" id = "addIN" name = "addIN" type = "button"	class="btn btn-primary trn">Add New Row	</button></p></tr>';

            addCWRow = '<br><tr><p align="right"><button style="align-content: right;" class="addCW" id = "addCW" name = "addCW" type = "button"	class="btn btn-primary trn">Add New Row	</button></p></tr>';

            addSWRow = '<br><tr><p align="right"><button style="align-content: right;" class="addSW" id = "addSW" name = "addSW" type = "button"	class="btn btn-primary trn">Add New Row	</button></p></tr>';

            // add new button
            if (rowContentSW == "") {

                tableSW = "";
                addSWRow = "";
            }
            if (rowContentIN == "") {

                tableIN = "";
                addInRow = "";
            }
            if (rowContentCW == "") {

                tableCW = "";
                addCWRow = "";
            }
            //alert(tableSE);
            console.log(tableIN);
            $("#dvTable").html("");
            var tables = addCWRow + tableCW + "<br/>" + addInRow + tableIN + "<br/>" + addSWRow + tableSW + "<br/>" + tableSE ;
            var dvTable = $("#dvTable");
            dvTable.append(tables);
        });

}
function SaveInternalObservation() {

        var Unit = $('#Units').val();
        if (Unit == null || Unit == "") {
            showWarning("Please enter the Units", language);
            return false;
        }
        var Temprature = $('#TempStart').val();
        if (Temprature == null || Temprature == "") {
            showWarning("Please enter the Temprature !!!", language);
            return false;
        }
        var Humidity = $('#Humidity').val();
        if (Humidity == null || Humidity == "") {
            showWarning("Please enter the Humidity", language);
            return false;
        }
        var VisualCheckCondition = $('#VisualCheckCondition').val();
        if (VisualCheckCondition == null || VisualCheckCondition == "") {
            showWarning("Please enter the Visual Check", language);
            return false;
        }
        // var PermissibleLimit = $('#PermissibleLimit').val();
        $('#dvload').show();
        var ObservationContentValues = new Array();
        var ObservationContent = new Array();
        var ObservationContenMapping = new Array();
        var footerPermissiable = "";
        $(".content").each(function () {

            $(this).find('table').each(function () {

                var tbl_id = $(this).attr("id");//.split("-")[2];

                if (tbl_id.substring(0, 2) == "CW") {
                    footerPermissiable = $('#tblCW_footer').find("input.PermissibleLimit").val();
                }
                $(this).find('tr').each(function () {
                    var rowid = $(this).find('tr').attr('id');
                    var currentRow = $(this).find('td').attr('id');

                    var contentvalueid = $(this).find("input[name = HiddenContentvalueId]").val();


                    if ((contentvalueid == "0") || (contentvalueid == "")) {
                        contentvalueid = null
                    }

                    if ($(this).find("input[name = HiddenContentId]").val() > 0) {

                        var ObservationContenMappingData = {

                            Id: $(this).find("input[name = HiddenMappingId]").val(),
                            Sno: $(this).find("td:eq(1) input[type='text']").val(),
                            ContentId: $(this).find("input[name = HiddenContentId]").val(),
                            ObservationId: $('#TemplateObservationId').val(),
                            InstrumentId: $('#InstrumentId').val(),
                            CreatedBy: "",
                            CreatedOn: "",
                            IsActive: true
                        }
                        ObservationContenMapping.push(ObservationContenMappingData);
                        if (currentRow == 'CW') {

                            var ObservationContenValuesData = {

                                Id: contentvalueid,
                                ParentId: $('#TemplateObservationId').val(),
                                Sno:0,// $(this).find("td:eq(1) input[type='text']").val(),
                                MeasuedValue: "",
                                ActualValue: "",
                                InstrumentError: "",
                                Diff: $(this).find('td').attr('id'),
                                MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                                MeasuedValue1: $(this).find("td:eq(2) input[type='text']").val(),
                                MeasuedValue2: $(this).find("td:eq(3) input[type='text']").val(),
                                MeasuedValue3: $(this).find("td:eq(4) input[type='text']").val(),
                                Average: $(this).find("td:eq(5) input[type='text']").val(),
                                Percent: $(this).find("td:eq(6) input[type='text']").val(),
                                ContentId: $(this).find("input[name = HiddenContentId]").val(),
                                PermissibleLimit: footerPermissiable
                                //<th rowspan="1" colspan="1">Parcela</th> $('tfoot input').
                            }
                            //}
                            ObservationContentValues.push(ObservationContenValuesData);
                        }
                        else if (currentRow == 'SW') {

                            var ObservationContenValuesData = {

                                Id: contentvalueid,
                                ParentId: $('#TemplateObservationId').val(),
                                Sno:0,// $(this).find("td:eq(1) input[type='text']").val(),
                                MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                                ActualValue: "",
                                InstrumentError: "",
                                Diff: $(this).find('td').attr('id'),
                                MeasuedValue1: $(this).find("td:eq(2) input[type='text']").val(),//3,4,5
                                MeasuedValue2: $(this).find("td:eq(3) input[type='text']").val(),
                                MeasuedValue3: $(this).find("td:eq(4) input[type='text']").val(),
                                Average: "",
                                Percent: "",
                                ContentId: $(this).find("input[name = HiddenContentId]").val(),
                                PermissibleLimit: ""
                            }
                            //}
                            ObservationContentValues.push(ObservationContenValuesData);
                        }
                        else if (currentRow == 'IN') {

                            //console.log($(this).closest("tr").find("td:eq(3)").text());
                          
                          
                            var ObservationContenValuesData = {

                                Id: contentvalueid,// $(this).find("input[name = HiddenContentvalueId]").val(),
                                ParentId: $('#TemplateObservationId').val(),
                                Sno: 0,//$(this).find("td:eq(1) input[type='text']").val(),//2,3,4
                                MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                                ActualValue: $(this).find("td:eq(2) input[type='text']").val(),
                                InstrumentError: $(this).find("td:eq(3) input[type='text']").val(),
                                Diff: $(this).find('td').attr('id'),
                                MeasuedValue1: "",
                                MeasuedValue2: "",
                                MeasuedValue3: "",
                                Average: "",
                                Percent: "",
                                ContentId: $(this).find("input[name = HiddenContentId]").val(),
                                PermissibleLimit: ""
                                //}
                            }
                            ObservationContentValues.push(ObservationContenValuesData);

                        }
                        else if (currentRow == 'SE') {
                            //alert($(this).closest("tr").attr('id'));
                            console.log($(this).closest("tr").attr('id'));
                            console.log($(this).closest("tr").find("td:eq(3)").text());

                            var data = $(this).closest("tr").attr('id');//$(this).closest("tr").find("td:eq(3)").text();

                            var ObservationContenValuesData = {

                                Id: contentvalueid,// $(this).find("input[name = HiddenContentvalueId]").val(),
                                ParentId: $('#TemplateObservationId').val(),
                                Sno: data,//$(this).find("td:eq(1) input[type='text']").val(),//2,3,4
                                MeasuedValue: $(this).find("td:eq(1) input[type='text']").val(),
                                ActualValue: $(this).find("td:eq(2) input[type='text']").val(),
                                InstrumentError: $(this).closest("tr").find("td:eq(3)").text(),
                                Diff: $(this).find('td').attr('id'),
                                MeasuedValue1: "",
                                MeasuedValue2: "",
                                MeasuedValue3: "",
                                Average: "",
                                Percent: "",
                                ContentId: $(this).find("input[name = HiddenContentId]").val(),
                                PermissibleLimit: ""
                                //}
                            }
                            ObservationContentValues.push(ObservationContenValuesData);

                        }
                    }

                });
            });

        });
    var filedataOBS = [];
    var filenameOBS = [];
    var filesizeOBS = [];
    var fileSerialNoOBS = [];
    $.each(FileData, function (key, value) {
       
        filesizeOBS.push(value.size);
        filenameOBS.push(value.name.trim());
        filedataOBS.push(value.data);
        fileSerialNoOBS.push(value.slno);
       
    })

        var data = {
            Id: $('#Id').val(),
            InstrumentId: $('#InstrumentId').val(),
            RequestId: $('#RequestId').val(),
            TempStart: $('#TempStart').val(),
            Humidity: $('#Humidity').val(),
            Units: $('#Units').val(),
            Condition: $('#VisualCheckCondition').val(),
            ObservationContentValuesList: ObservationContentValues,
            ObservationContentMappingList: ObservationContenMapping,
            FileName: filenameOBS,
            FileData: filedataOBS,
            FileSize: filesizeOBS,
            Serialno: fileSerialNoOBS,
            PermissibleLimit: $('#PermissibleLimit').val()
    }

        console.log(data);
        $.ajax({
            url: '../Observation/InsertDynamicObservationContent',
            type: 'POST',
            data: { dynamic: data },
            dataType: "json",
        }).done(function (resultObject) {
            $('#dvload').hide();
            window.location.href = '../Tracker/Request?reqType=4';
            showSuccess("Data Saved Successfully", lang);
        });
}

function IfIdNoExist(idno) {
    var res;
    $.ajax({
        url: '../Instrument/IfIdNoExist',
        type: 'GET',
        data: '',
        dataType: "json",
    }).done(function (resultObject) {
        //debugger;
        GlIDNo = resultObject;
        
     
    });    
}
function MailInsertInstrument() {

    $.ajax({
        url: '../Instrument/MailInsertInstrument',
        type: 'POST',
        data: { userViewModelList: Request },
        dataType: "json",
    }).done(function (resultObject) {
       // showSuccess("Data Saved Successfully");
    
    });
}
function MasterFileDeleteAlert(MasterId, EquipFilename, ids)
{
            Swal.fire({
            title: "Are you Sure You Want to Delete the File",
            text: "Master Equipment File",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Delete it!",
            closeOnConfirm: false
        }).then((result) => {
            
            if (result.isConfirmed) {

                $.ajax({
                    url: '../Master/DeleteMasterFile',
                    type: 'POST',
                    data: { MasterId: MasterId, filename: EquipFilename }
                }).done(function (resultObject) {
                 
                $("#" + ids).remove();
                $("#" + ids).hide();
                
                });
          }
        });
    }

function ObservationFileDeleteAlert(ids,Filename) {
    Swal.fire({
        title: "Are you Sure You Want to Delete the File",
        text: "Special Equipment File",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, Delete it!",
        closeOnConfirm: false
    }).then((result) => {

        if (result.isConfirmed) {

            $.ajax({
                url: '../Observation/DeleteObservationFile',
                type: 'POST',
                data: { Id: ids, filename: Filename }
            }).done(function (resultObject) {
             
                $("#" + ids).remove();
                $("#" + ids).hide();
               // element.remove();
                //showSuccess("Master Activated Successfully", lang);
                //window.location.reload();
                // }
            });
        }
    });
}

function removeRequestFromControlCard(Row,Id) {
    //alert(Id);
        Swal.fire({
            title: "Are you Sure You Want to Delete the Request,Will be Removed Permanently from the Tracker and Control Card / リクエストを削除してもよろしいですか。リクエストはトラッカーとコントロール カードから永久に削除されます",
            text: "",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Delete it!",
            closeOnConfirm: false
        }).then((result) => {

            if (result.isConfirmed) {

                $.ajax({
                    url: '../Instrument/removeRequestRowFromControlCard',
                    type: 'POST',
                    data: { RequestId: Id}
                }).done(function (resultObject) {
                    $(Row).closest('tr').remove();                  
                    // element.remove();
                    showSuccess("Master Activated Successfully", lang);
                    window.location.reload();
                   
                });
            }
        });
    }

    
