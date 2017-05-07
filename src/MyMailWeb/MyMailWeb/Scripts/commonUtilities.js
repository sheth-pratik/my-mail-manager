var CommonUtilities = new function () {
    this.TOAST_TYPE = {
        DANGER: 'danger',
        INFO: 'info',
        SUCCESS: 'success',
        WARNING: 'warning'
    };

    this.showToast = function (toastType, title, message, alwaysShow) {
        $("#toastMessage").html("");
        $("#toastMessage").hide();
        $("#toastMessage").html($("#toastSample > div").clone().removeClass("").addClass("alert alert-" + toastType + " alert-dismissable"));
        $("#toastMessage > div > #lblMessage").html(message);
        $("#toastMessage").slideDown();
        $(document).scrollTop(0);
        if (!alwaysShow) {
            setTimeout(function () {
                $("#toastMessage").slideUp();
                $("#toastMessage").html("");
            }, 10000);
        }
        //$.toaster({ 'priority': toastType, 'title': title, 'message': message });
    }
};

var index = 0;
$(document).ajaxSend(function (event, request, settings) {
    $('#loading-indicator').show();
    index++;
});

$(document).ajaxComplete(function (event, request, settings) {
    index--;
    if (index <= 0) {
        $('#loading-indicator').hide();
    }
});