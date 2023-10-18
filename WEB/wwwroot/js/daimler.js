var FileData = [];

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

            console.log(lang);
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

function ValidateCheck() {
    //debugger;
    var errCount = 0;
    $('#StandardRefferedError').hide();
    $('#InstrumentConditionError').hide();
    //alert('NewObservation' + $('#NewObservation').val())
    if (($('#InstrumentCondition').val()) == '') {
        errCount = errCount + 1;
        $('#InstrumentConditionError').show();
    }
    else {
        //$('#').hide();
    }

    if (($('#StandardReffered').val()) == '') {
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
    if (($('#NewCertification').val()) == '') {
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


function DudeDateCalculation (dt) {

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
    //debugger;
    var data;
    var Id = $('#RequestCalibId').val();
    
    var dueDate;      
    var dt = $('#CalibFreq').val();
    dueDate = DudeDateCalculation(dt);
    console.log('duedate');
    console.log(dueDate);
    
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
        // if (resultObject.TypeOfRequest == 1) {
        //     AssignNewRequestValues(resultObject);
        // } else {
        //     AssignRequestValues(resultObject);
        // }
        window.location.href = '../Tracker/Request?reqType=4';

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

function NewReqEnableReason() {
   // debugger;
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
            showWarning('Please Select Tentative Closing Date, Observation Templates, Observation Template Type, Certification Template Values...!', lang);
        }

    } else {
        if ($('#Newreason').val() != '') {
            $('#Newreason').removeClass('is-invalid');
            RejecttRequest(type, lang);
        } else {
            $('#Newreason').addClass('is-invalid');
            showWarning("Please enter reason for rejection and try again.", lang);
        }
    }
}

function AcceptRejectExternalRequest(lang) {
    debugger;
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

function ExternalRejecttRequest(type, lang)
{
    debugger;
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

function SaveInventoryCalibration(lang) {
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
            var Masterid = "ReplacementLabID_" + objInstrumentId;
            var POPoutId = "popupcount_" + objInstrumentId;
            var CHKoutId = "ChkInput_" + objInstrumentId;
            var objReplacementLabId = $(tr).find("td[id='" + Masterid + "']").text().trim();
            //var objReplacementLabId = $(tr).find("input[id='" + Masterid + "']").text().trim();
            var objPopUpRecordCount = $(tr).find("input[id='" + POPoutId + "']").val(); // $(tr).find("input[id= '" + ChkoutId + "']").val().trim();
            var objChkRecordCount = $(tr).find("input[id='" + CHKoutId + "']").prop('checked');
                 
          
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
                    var InstrumentData =
                    {
                        InstrumentId: objInstrumentId,
                        ReplacementLabId: objReplacementLabId

                    }

                    InstrumentDataList.push(InstrumentData);
                }
            }

            else if ((checkedvalue == false) && (objReplacementLabId == "") && (objPopUpRecordCount == 0)) {
                UNCheckedCount += 1;
            }
            //else if ((checkedvalue == false) && (objReplacementLabId == "") && (objPopUpRecordCount == 1)) {
            //    UNCheckedCount += 1;
            //}
        });
      

    }
    
    if ((InstrumentDataList.length > 0) && (CheckedCount == 0)){
      
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
        data: { requestId: $('#RequestCalibId').val(), Result: $('#newResultDEP').val(), CollectedBy: $('#CollectedBy').val(), InstrumentIdNo: $('#IdNo').val(), DueDate: dueDate }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        //AssignNewRequestValues(resultObject);
        showSuccess("Your visual check details recorded", lang);
    });
}

function SubmitReview(lang) {
    $.ajax({
        url: '../Observation/SubmitReview',
        type: 'POST',
        data: { observationId: $('#TemplateObservationId').val(), reviewDate: $('#ReviewDate').val(), reviewStatus: $('#ReviewStatus').val() }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        showSuccess("Your details recorded", lang);
    });
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

    return Filetype;
}

function FileUpload(filedata, filename, filesize) {
    var json = {};
    for (var i = 0; i < filename.length; i++) {
        var fs = (filesize[i] / 1024 / 1024).toFixed(2);
        json = { "Name": filename[i], "Size": fs, "Data": filedata[i] };
        FileData.push(json);
        console.log(FileData);
    }
}

function getrequest(type) {

    if ($('input[name="ReqTracker"]:checked').val() == 'New') {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
        //GetAllRequest(type) 
    } else if ($('input[name="ReqTracker"]:checked').val() == 'Regular') {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
        //GetAllRequest(type) 
    } else if ($('input[name="ReqTracker"]:checked').val() == 'ReCalibration') {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
        //GetAllRequest(type) 
    } else {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
        //GetAllRequest(type) 
    }
}

function DueForCalibrationInstruments() {
    //debugger;
    if ($("#checkdueonly").is(":checked")) {
        var tblRowsCoun = $("#example1 td").length;
        if (tblRowsCoun > 0) {
            console.log('count');
            var oTable = $("#example1").dataTable();
            $(".one", oTable.fnGetNodes()).each(function (i, row) {                                                                   
                var currentRow3 = $(this).closest("tr");                
                if (typeof($(this).closest('tr').find('td:eq(9) input[type="checkbox"]').html()) === "undefined") { 
                     currentRow3.hide();
                }
                else {
                     currentRow3.show();
                }
            });            
        }
    }
    else {
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
                if (currentRow.find("td:eq(9)").text() != " ") {
                    //alert(currentRow.find("td:eq(9)").text());
                    //console.log(currentRow.find("td:eq(9)").text());
                    var checkedvalue = $(tr).find("td:eq(9) input[type='checkbox']")[0].checked;
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
    //debugger;
    var Request = new Array();

    var oTable = $("#example1").dataTable();
    $(".class1:checked", oTable.fnGetNodes()).each(function (i, row) {
        var UserView = {            
            instrumentId: $(this).closest('tr').find('td:eq(9) input[type="checkbox"]').val(),
            TypeValue: $(this).closest('tr').find("td:eq(9) input[type='hidden']").val()
        }

        Request.push(UserView);
    });
    //$('#example1 > tbody > tr').each(function (row, tr) {
    //    var currentRow = $(this).closest("tr");
    //    if (currentRow.find("td:eq(9)").text() != " ") {
    //        var checkedvalue = $(tr).find("td:eq(9) input[type='checkbox']")[0].checked;
    //        if (checkedvalue == true) {               

    //            var UserView = {
    //                instrumentId: $(tr).find("td:eq(9) input[type='checkbox']").val(),
    //                TypeValue: $(tr).find("td:eq(9) input[type='hidden']").val()                   
    //            }

    //            Request.push(UserView);
    //        }
    //    }
    //});
    console.log(Request);
    $.ajax({
        url: '../Instrument/RegularRecaliRequest',
        type: 'POST',
        data: { userViewModelList: Request },
        dataType: "json",    
    }).done(function (resultObject) {
        //showSuccess("Data Saved Successfully");
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
        RequestId: $('#requestId').val(),
        RefStd: $('#RefStd').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWi').val(),
        Allvalues: $('#Allvalues').val(),
        ExternalObsCondition: $('#ExtIndicatiorCondition').val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        ReviewedBy: $('#ReviewedBy').val(),
        CalibrationDoneDate: $('#CalibrationDoneDate').val(),
        ReviewedDate: $('#ReviewedDate').val(),        
        AdminReviewStatus: $('#AdminReviewStatus').val(),
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
    debugger;
    if ($('#txtIdNo').val().trim() == '') {
        showWarning("Please Enter Instrument Id Number", lang);
        return false; 
    }
   

    $.ajax({
        url: '../Tracker/SaveExternalObs',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), InstrumentID: $('#hdnInstrumentId').val(), InstrumentIdNo: $('#txtIdNo').val() },    
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
    var result = $('#CalibrationResult').val();
    var remarks = $('#Remarks').val();
    if (result == null || result == "") {
        showWarning("Please enter the Calibration Result!!!", lang);
        return true;
    }
    else if (remarks == null || remarks == "") {
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
