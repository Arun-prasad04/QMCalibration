var FileData = [];
var lang = $('#hdnLanguage').val();
console.log(lang);


function logoShowHide() {
    var x = document.getElementById("logoheader");
    if (x.style.display === "none") {
        x.style.display = "block";
    } else {
        x.style.display = "none";
    }
}
//SweetAlert
function showSuccess(successMsg) {
    Swal.fire({
        icon: 'success',
        title: 'Success',
        text: successMsg,
        footer: '',
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    });

    if (lang == 'jp') {
        Swal.update({
            title: '成功'
        });
    }
}

function showError(errorMsg) {
    Swal.fire({
        icon: 'error',
        title: 'Error',
        text: errorMsg,
        footer: '',
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    });

    if (lang == 'jp') {
        Swal.update({
            title: 'エラー'
        });
    }
}

function showWarning(warningMsg) {
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
            allowOutsideClick: false

        }).then((result) => {
            /* Read more about isConfirmed, isDenied below */
            if (result.isConfirmed) {
                window.location.href = 'http://s365id1qdg044/cmtlive/Account/Login';
            }
        });

        if (lang == 'jp') {
            Swal.update({
                title: 'セッションの有効期限が切れ！作業を続けるにはログインしてください。',
                confirmButtonText: 'わかった'
            });
        }
    }
}



function showPopup(resCode, resMsg) {
    if (resCode != '' && resCode != undefined && resCode == 200) {
        showSuccess(resMsg);
    } else if (resCode != undefined && resCode != '') {
        showError(resMsg);
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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "You are accepted the request. LAB admin get notified!",
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

function RejecttExternalRequest() {
    $.ajax({
        url: '../Tracker/RejectExternalRequest',
        type: 'POST',
        data: { externalRequestId: $('#ExternalCalibId').val(), rejectReason: $('#reason').val() }
    }).done(function (resultObject) {
        AssignExternalRequestValues(resultObject);
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

function SubmitFMVisual() {

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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your visual check details recorded.",
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

function SubmitLABVisual() {

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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your visual check details recorded",
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

function EnableReason() {
    if ($('input[name="AcceptReject"]:checked').val() == 'Accept') {
        $('#rejectionReasonSection').css('display', 'none');
    } else {
        $('#rejectionReasonSection').css('display', 'block');
    }
}

function submitAcceptReject() {
    if ($('input[name="AcceptReject"]:checked').val() == undefined || $('input[name="AcceptReject"]:checked').val() == '') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: "Please choose either Accept / Reject and try again.",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    } else if ($('input[name="AcceptReject"]:checked').val() == 'Accept') {
        AcceptExternalRequest();
    } else {
        if ($('#reason').val() != '') {
            $('#reason').removeClass('is-invalid');
            RejecttExternalRequest();
        } else {
            $('#reason').addClass('is-invalid');
            Swal.fire({
                icon: 'warning',
                title: 'Warning',
                text: "Please enter reason for rejection and try again.",
                footer: '',
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
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

        inputValidator: (value) => {
            if (!value) {
                if (lang == 'jp') {
                    return '検疫の理由を記入してください'
                }
                else {
                    return 'please fill the reason for Quarantine'
                }
            }
            else
                return null
        },
        preConfirm: (msg) => {
            $.ajax({
                url: '../Master/MasterQuarantine',
                type: 'POST',
                data: { masterId: element.id, reason: msg }
            }).done(function (resultObject) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: "Master moved to quarantine list",
                    footer: '',
                    showClass: {
                        popup: 'animate__animated animate__fadeInDown'
                    },
                    hideClass: {
                        popup: 'animate__animated animate__fadeOutUp'
                    }
                });

                if (lang == 'jp') {
                    Swal.update({
                        title: '成功',
                        text: 'マスターは検疫リストに移動しました'
                    });
                }

                $('#row_' + element.id).next("tr").remove()
                $('#row_' + element.id).remove();
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });

    if (lang == 'jp') {
        Swal.update({
            title: '検疫の理由を入力してください',
            confirmButtonText: '検疫',
            cancelButtonText: 'キャンセル',
            
        });
    }
}

function MasterUnQuarantineClick(element) {
    $.ajax({
        url: '../Master/MasterRemoveQuarantine',
        type: 'POST',
        data: { masterId: element.id }
    }).done(function (resultObject) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Master Activated Successfully",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
        $('#row_' + element.id).next("tr").remove()
        $('#row_' + element.id).remove();
    });
}

function InstrumentQuarantineClick(element) {
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
        inputValidator: (value) => {
            if (!value) return 'please fill the reason for Quarantine'
            else return null
        },


        preConfirm: (msg) => {
            $.ajax({
                url: '../Instrument/InstrumentQuarantine',
                type: 'POST',
                data: { instrumentId: element.id, reason: msg }
            }).done(function (resultObject) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: "Instrument moved to quarantine list",
                    footer: '',
                    showClass: {
                        popup: 'animate__animated animate__fadeInDown'
                    },
                    hideClass: {
                        popup: 'animate__animated animate__fadeOutUp'
                    }
                });
                $('#row_' + element.id).next("tr").remove()
                $('#row_' + element.id).remove();
            });
        },
        allowOutsideClick: () => !Swal.isLoading()
    });
}

function InstrumentUnQuarantineClick(element) {
    $.ajax({
        url: '../Instrument/InstrumentRemoveQuarantine',
        type: 'POST',
        data: { instrumentId: element.id }
    }).done(function (resultObject) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Instrument Unquarantine successfully",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
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

function AcceptRejectRequest() {
    if ($('input[name="ReqAcceptReject"]:checked').val() == undefined || $('input[name="ReqAcceptReject"]:checked').val() == '') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: "Please choose either Accept / Reject and try again.",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    } else if ($('input[name="ReqAcceptReject"]:checked').val() == 'Accept') {
        AcceptRequest(2);
    } else {
        if ($('#reason').val() != '') {
            $('#reason').removeClass('is-invalid');
            RejecttRequest(2);
        } else {
            $('#reason').addClass('is-invalid');
            Swal.fire({
                icon: 'warning',
                title: 'Warning',
                text: "Please enter reason for rejection and try again.",
                footer: '',
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
        }
    }
}

function AcceptRequest(type) {
    var data;
    var Id = $('#RequestCalibId').val();
    data = {
        requestId: $('#RequestCalibId').val(),
        ReceivedBy: $('#ReceivedBy').val(),
        InstrumentCondition: $('#InstrumentCondition').val(),
        Feasiblity: $('#Feasiblity').val(),
        TentativeCompletionDate: $('#TentativeCompletionDate').val(),
        newNABL: $('#IsNABL').val(),
        newObservation: $('#NewObservation').val(),
        newObservationType: $('#NewObservationType').val(),
        newMU: $('#NewMU').val(),
        newCertification: $('#NewCertification').val(),
        standardReffered: $('#StandardReffered').val(),
        MasterInstrument1: $('#MasterInstrument1').val(),
        MasterInstrument2: $('#MasterInstrument2').val(),
        MasterInstrument3: $('#MasterInstrument3').val(),
        MasterInstrument4: $('#MasterInstrument4').val()
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

function AcceptRequestRecalibration(AcceptValue) {
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

function RejecttRequest(type) {
    var data;
    if (type == 1) {
        data = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            InstrumentCondition: $('#InstrumentCondition').val(),
            Feasiblity: $('#Feasiblity').val(),
            TentativeCompletionDate: $('#TentativeCompletionDate').val(),
            rejectReason: $('#Newreason').val(),
            standardReffered: $('#StandardReffered').val()
        };
    } else if (type == 2) {
        data = {
            requestId: $('#RequestCalibId').val(),
            ReceivedBy: $('#ReceivedBy').val(),
            InstrumentCondition: $('#InstrumentCondition').val(),
            Feasiblity: $('#Feasiblity').val(),
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

function SubmitReqDepVisual() {

    if ($('#ResultDEP').val() == '' || $('#ResultDEP').val() == undefined) {
        $('#ResultDEP').addClass('is-invalid');
        return false;
    } else {
        $('#ResultDEP').removeClass('is-invalid');

    }

    $.ajax({
        url: '../Tracker/SubmitDepartmentRequestVisual',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), Result: $('#ResultDEP').val() }
    }).done(function (resultObject) {
        AssignRequestValues(resultObject);
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your visual check details recorded.",
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
function disableFields() {
    document.getElementById("NewObservation").disabled = true;
    document.getElementById("NewObservationType").disabled = true;
    document.getElementById("NewMU").disabled = true;
    document.getElementById("NewCertification").disabled = true;
    document.getElementById("newSubmitLABAdmin").disabled = true;
}

function SaveLABAdminUpdates() {
    $.ajax({
        url: '../Tracker/SubmitLABAdminUpdates',
        type: 'POST',
        data: {
            requestId: $('#RequestCalibId').val(),
            ObservationTemplate: $('#NewObservation').val(),
            ObservationTemplateType: $('#NewObservationType').val(),
            MUTemplate: $('#NewMU').val(),
            CertificationTemplate: $('#NewCertification').val()

        }
    }).done(function (resultObject) {
        AssignRequestValues(resultObject);
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Certificate Updated Successfully",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
        disableFields();
    });
}
function SubmitReqLABVisual() {

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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your visual check details recorded",
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
    if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
        $('#NewReasonSection').css('display', 'none');
        $('#NewacceptSection').css('display', 'block');
    } else {

        $('#NewReasonSection').css('display', 'block');
        $('#NewacceptSection').css('display', 'none');
    }
}

function AcceptRejectNewRequest() {
    var type = $('#hdntype').val();
    if ($('input[name="NewAcceptReject"]:checked').val() == undefined || $('input[name="NewAcceptReject"]:checked').val() == '') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: "Please choose either Accept / Reject and try again.",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    } else if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
        AcceptRequest(type);
    } else {
        if ($('#Newreason').val() != '') {
            $('#Newreason').removeClass('is-invalid');
            RejecttRequest(type);
        } else {
            $('#Newreason').addClass('is-invalid');
            Swal.fire({
                icon: 'warning',
                title: 'Warning',
                text: "Please enter reason for rejection and try again.",
                footer: '',
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
        }
    }
}

function AcceptRejectReCalibrationRequest() {
    var type = $('#hdntype').val();
    if ($('input[name="NewAcceptReject"]:checked').val() == undefined || $('input[name="NewAcceptReject"]:checked').val() == '') {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: "Please choose either Accept / Reject and try again.",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    } else if ($('input[name="NewAcceptReject"]:checked').val() == 'Accept') {
        AcceptRequestRecalibration(1);
    } else {
        AcceptRequestRecalibration(0);
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
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: "Please choose either Accept / Reject and try again.",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    } else if ($('input[name="QuarAcceptReject"]:checked').val() == 'Accept') {
        AcceptQuarRequest();
    } else {
        if ($('#Quarreason').val() != '') {
            $('#Quarreason').removeClass('is-invalid');
            RejecttQuarRequest();
        } else {
            $('#Quarreason').addClass('is-invalid');
            Swal.fire({
                icon: 'warning',
                title: 'Warning',
                text: "Please enter reason for rejection and try again.",
                footer: '',
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
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

function LoadObservationType() {
    $.ajax({
        url: '../Tracker/LoadObservationType',
        type: 'POST',
        data: { attrType: '', attrsubType: $('#ObservationTemplate option:selected').text() }
    }).done(function (resultObject) {
        $('#ObservationType')
            .find('option')
            .remove();
        for (let index = 0; index < resultObject.length; index++) {
            optText = resultObject[index].attrValue;
            optValue = resultObject[index].id;;
            $('#ObservationType').append(`<option value="${optValue}">${optText}</option>`);
        }
    });

}

function SaveLeverDial() {
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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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

function SaveMicrometer() {
    var data = {
        Id: $('#Id').val(),
        TemplateObservationId: $('#TemplateObservationId').val(),
        TempStart: $('#TempStart').val(),
        TempEnd: $('#TempEnd').val(),
        Humidity: $('#Humidity').val(),
        RefWi: $('#RefWi').val(),
        Allvalues: $('#Allvalues').val(),
        InstrumentId: $('#instrumentId').val(),
        RequestId: $('#requestId').val(),
        MicrometerCondition: $('#MicrometerCondition').val(),
        ActualsT11: $('#ActualsT11').val(),
        ActualsT21: $('#ActualsT21').val(),
        ActualsT31: $('#ActualsT31').val(),
        Avg1: $('#Avg1').val(),
        MuInterval1: $('#MuInterval1').val(),
        ActualsT12: $('#ActualsT12').val(),
        ActualsT22: $('#ActualsT22').val(),
        ActualsT32: $('#ActualsT32').val(),
        Avg2: $('#Avg2').val(),
        MuInterval2: $('#MuInterval2').val(),
        ActualsT13: $('#ActualsT13').val(),
        ActualsT23: $('#ActualsT23').val(),
        ActualsT33: $('#ActualsT33').val(),
        Avg3: $('#Avg3').val(),
        MuInterval3: $('#MuInterval3').val(),
        ActualsT13: $('#ActualsT13').val(),
        ActualsT23: $('#ActualsT23').val(),
        ActualsT33: $('#ActualsT33').val(),
        Avg3: $('#Avg3').val(),
        MuInterval3: $('#MuInterval3').val(),
        ActualsT14: $('#ActualsT14').val(),
        ActualsT24: $('#ActualsT24').val(),
        ActualsT34: $('#ActualsT34').val(),
        Avg4: $('#Avg4').val(),
        MuInterval4: $('#MuInterval4').val(),
        ActualsT15: $('#ActualsT15').val(),
        ActualsT25: $('#ActualsT25').val(),
        ActualsT35: $('#ActualsT35').val(),
        Avg5: $('#Avg5').val(),
        MuInterval5: $('#MuInterval5').val(),
        ActualsT16: $('#ActualsT16').val(),
        ActualsT26: $('#ActualsT26').val(),
        ActualsT36: $('#ActualsT36').val(),
        Avg6: $('#Avg6').val(),
        ActualsT17: $('#ActualsT17').val(),
        ActualsT27: $('#ActualsT27').val(),
        ActualsT37: $('#ActualsT37').val(),
        Avg7: $('#Avg7').val(),
        ActualsT18: $('#ActualsT18').val(),
        ActualsT28: $('#ActualsT28').val(),
        ActualsT38: $('#ActualsT38').val(),
        Avg8: $('#Avg8').val(),
        ActualsT19: $('#ActualsT19').val(),
        ActualsT29: $('#ActualsT29').val(),
        ActualsT39: $('#ActualsT39').val(),
        Avg9: $('#Avg9').val(),
        ActualsT110: $('#ActualsT110').val(),
        ActualsT210: $('#ActualsT210').val(),
        ActualsT310: $('#ActualsT310').val(),
        Avg10: $('#Avg10').val(),
        ActualsT111: $('#ActualsT111').val(),
        ActualsT211: $('#ActualsT211').val(),
        ActualsT311: $('#ActualsT311').val(),
        Avg11: $('#Avg11').val(),
        Flatness1: $('#Flatness1').val(),
        Flatness2: $('#Flatness2').val(),
        ParallelismSpec: $('#ParallelismSpec').val(),
        Actuals: $('#Actuals').val(),
        ReviewedBy: $('#ReviewedBy').val(),
        CalibrationPerformedBy: $('#CalibrationPerformedBy').val(),
        CalibrationPerformedDate: $('#CalibrationPerformedDate').val(),
        ReveiwedByDate: $('#ReveiwedByDate').val(),
        Measurement1: $('#Measurement1').val(),
        Measurement2: $('#Measurement2').val(),
        Measurement3: $('#Measurement3').val(),
        Measurement4: $('#Measurement4').val(),
        Measurement5: $('#Measurement5').val(),
        Measurement6: $('#Measurement6').val(),
        Measurement7: $('#Measurement7').val(),
        Measurement8: $('#Measurement8').val(),
        Measurement9: $('#Measurement9').val(),
        Measurement10: $('#Measurement10').val(),
        Measurement11: $('#Measurement11').val(),
        MURemarks: $('#MURemarks').val(),


    }
    $.ajax({
        url: '../Observation/InsertMicrometer',
        type: 'POST',
        data: { micrometer: data }
    }).done(function (resultObject) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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


function SaveGeneral() {

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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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


function SaveVernierCaliper() {
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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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


function SaveGeneralNew() {
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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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








function SavePlungerDial() {

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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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


function SaveThreadGauges() {

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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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

function SaveTWobs() {
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
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Data Saved Successfully",
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

function LoadObservationTypePopup() {
    $.ajax({
        url: '../Tracker/LoadObservationType',
        type: 'POST',
        data: { attrType: '', attrsubType: $('#NewObservation option:selected').text() }
    }).done(function (resultObject) {
        $('#NewObservationType')
            .find('option')
            .remove();
        for (let index = 0; index < resultObject.length; index++) {
            optText = resultObject[index].attrValue;
            optValue = resultObject[index].id;;
            $('#NewObservationType').append(`<option value="${optValue}">${optText}</option>`);
        }
    });

}

function AddNewInstrumentMaster() {
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
            Swal.fire({
                icon: 'warning',
                title: 'Warning',
                text: "Maximum 4 Equipment Allowed",
                footer: '',
                showClass: {
                    popup: 'animate__animated animate__fadeInDown'
                },
                hideClass: {
                    popup: 'animate__animated animate__fadeOutUp'
                }
            });
        }
    } else {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Master Equipment Added Successfully",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
    }
}


function DeleteMasterEqiupment(id) {
    $('#MasterInstrument' + id).remove();
    $('#masvalue' + id).remove();
}



function SaveCertificate(templtatename) {

    var result = $('#CalibrationResult').val();
    if (result == null || result == "") {
        Swal.fire({
            icon: 'warning',
            title: 'Warning',
            text: "Please enter the Calibration Result!!!",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
        return true;
    }

    Swal.fire({
        title: "Are you want Generate QR Code with Pdf file?",
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
                    TempltateName: templtatename
                }
            }).done(function (resultObject) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: "Certificate Updated Successfully",
                    footer: '',
                    showClass: {
                        popup: 'animate__animated animate__fadeInDown'
                    },
                    hideClass: {
                        popup: 'animate__animated animate__fadeOutUp'
                    }
                });
                window.location.reload();
            });
        }
    });
}

function SaveInstrumentDetails() {
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

        }
    }).done(function (resultObject) {
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Certificate Updated Successfully",
            footer: '',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        });
        window.location.reload();
    });
}


function newSubmitReqLABVisual() {

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
        //AssignNewRequestValues(resultObject);
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your visual check details recorded",
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

function newSubmitReqDepVisual() {

    if ($('#newResultDEP').val() == '' || $('#newResultDEP').val() == undefined) {
        $('#newResultDEP').addClass('is-invalid');
        return false;
    } else {
        $('#newResultDEP').removeClass('is-invalid');

    }

    $.ajax({
        url: '../Tracker/SubmitDepartmentRequestVisual',
        type: 'POST',
        data: { requestId: $('#RequestCalibId').val(), Result: $('#newResultDEP').val(), CollectedBy: $('#CollectedBy').val() }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        //AssignNewRequestValues(resultObject);
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your visual check details recorded.",
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

function SubmitReview() {
    $.ajax({
        url: '../Observation/SubmitReview',
        type: 'POST',
        data: { observationId: $('#TemplateObservationId').val(), reviewDate: $('#ReviewDate').val(), reviewStatus: $('#ReviewStatus').val() }
    }).done(function (resultObject) {
        window.location.href = '../Tracker/Request?reqType=4';
        Swal.fire({
            icon: 'success',
            title: 'Success',
            text: "Your details recorded.",
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
    } else if ($('input[name="ReqTracker"]:checked').val() == 'Regular') {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
    } else if ($('input[name="ReqTracker"]:checked').val() == 'ReCalibration') {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
    } else {
        window.location.href = '../Tracker/Request?reqType=' + type + '';
    }
}