//$(document).ready(function () {
//    //ManageMailConfiguration.form.validate();
//});

var ManageMailConfiguration = new function() {
    
    this.submitMailConfiguration= function () {
        var form = $('#manageMailConfiguration');
        form.validate();
        if (form.valid()) {
            var mailConfiguration = {
                ImapEmailId: $('#ImapEmailId').val(),
                ImapEmailPassword: $("#ImapEmailPassword").val(),
                ConfirmImapEmailPassword: $("#ConfirmImapEmailPassword").val(),
                SMTPEmailId: $("#SMTPEmailId").val(),
                SMTPEmailPassword: $("#SMTPEmailPassword").val(),
                ConfirmSMTPEmailPassword: $("#ConfirmSMTPEmailPassword").val(),
                UserId: $("#UserId").val(),
                Id: $("#Id").val()
            }
            $.post(manageMailConfigurationUrl, mailConfiguration, this.mailConfigurationResponse);
        }
        return false;
    };

    this.resetMailConfiguration= function () {
        var form = $('#manageMailConfiguration')[0];
        form.reset();
        return false;
    };

    this.mailConfigurationResponse = function (data) {
        if (data.Status) {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.INFO, 'Success', "Mail configuraiton updated successfully. Please wait while we verify connection.");
            ManageMailConfiguration.verifyMailConnectivity();
        } else {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.DANGER, 'Error', data.Message);
            //$.toaster({ 'priority': 'danger', 'title': 'Error', 'message': data.Message, 'settings': { 'donotdismiss': ['danger'] } });
        }
    };

    this.verifyMailConnectivity = function () {
        $.post(verifyMailConnectivityUrl, this.verifyMailConnectivityResponse);
    };

    this.verifyMailConnectivityResponse = function (data) {
        if (data.Status) {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.SUCCESS, 'Success', "Mail connectivity verified.");
            $("#dvManageMailConfiguration").load(manageMailConfigurationUrl);
        } else {
            CommonUtilities.showToast(CommonUtilities.TOAST_TYPE.DANGER, 'Error', data.Message);
            //$.toaster({ 'priority': 'danger', 'title': 'Error', 'message': data.Message, 'settings': { 'donotdismiss': ['danger'] } });
        }
    }

};