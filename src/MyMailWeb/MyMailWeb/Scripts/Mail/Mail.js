var Mail = new function () {
    var defaultPage = 0;
    var count = 10;

    this.loadMailBoxes = function () {
        $("#mainMailContainer").load(mailContainerUrl);
    };
    this.loadMailBox = function (index) {

    };
    this.onTabLoad = function (e) {
        var index = $(e.target).attr("index"); // newly activated tab
        Mail.loadMailList(index, defaultPage)
    };
    this.loadMailList = function (index, page) {
        if ($('#messageContent').is(":visible") || $('#messageCompose').is(":visible")) {
            Mail.backToMailBoxList();
        }
        $("#mailListContainer-" + index).fadeOut(function () {
            $(this).load(mailListUrl + "/" + index + "/" + page + "/" + count, function () {
                $(this).slideDown("slow")
            });
        })
    };
    this.loadMessage = function (index, uid) {
        $("#mailBoxList").slideUp("slow", function () {
            $("#messageContent").load(messageContentUrl + "/" + index + "/" + uid, function () {
                $(this).slideDown("slow")
            });
        });
    };
    this.backToMailBoxList = function () {
        var visibleDiv = "";
        if ($('#messageContent').is(":visible")) {
            visibleDiv = "messageContent";
        } else if ($('#messageCompose').is(":visible")) {
            visibleDiv = "messageCompose";
        }
        $("#" + visibleDiv).slideUp("slow", function () {
            $("#messageContent").html("");
            $("#mailBoxList").slideDown("slow");
        });
    };
    this.composeNewMail = function () {
        $("#mailBoxList").slideUp("slow", function () {
            $("#messageCompose").load(messageComposeUrl, function () {
                $(this).slideDown("slow")
            });
        });
    };
    this.sendMail = function () {
        var form = $("#messageCompose form");

        form.removeData("validator");
        form.removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse(form);

        form.validate();
        if (form.valid()) {
            var messageCompose = {
                ToEmailAddress: $("#ToEmailAddress").val(),
                Subject: $("#Subject").val(),
                Body: $("#Body").val(),
                IsHTMLBody: JSON.stringify($("#IsHTMLBody").val())
            };
            $.post(messageComposeUrl, messageCompose, this.messageComposeResponse);
        }
        return false;
    };
    this.messageComposeResponse = function (data) {
        if (data.Status) {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.SUCCESS, 'Success', "Mail sent successfully.");
            Mail.backToMailBoxList();
        } else {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.DANGER, 'Error', data.Message);
        }
    }
    this.resetMail = function () {
        var form = ($("#messageCompose form")[0]).reset();
        return false;
    };

    this.deleteSelectedMails = function (index) {
        var selectedIds = []
        $('#tblMessageList > tbody').find('input[type="checkbox"]:checked').each(function () {
            selectedIds.push($(this).attr("uid"));
        })
        if (selectedIds.length > 0) {
            var objectToSend = {
                "index": index,
                "messageIds": selectedIds
            };
            $.post(deleteSelectedMessegesUrl, objectToSend, function (data) {
                Mail.deleteSelectedMessegesResponse(data, index);
            });
        }
    };
    this.deleteSelectedMessegesResponse = function (data, index) {
        if (data.Status) {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.SUCCESS, 'Success', "Mail(s) deleted successfully.");
            Mail.loadMailList(index, defaultPage);
        } else {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.DANGER, 'Error', data.Message);
        }
    };

}

$(document).ready(function () {
    Mail.loadMailBoxes();
});