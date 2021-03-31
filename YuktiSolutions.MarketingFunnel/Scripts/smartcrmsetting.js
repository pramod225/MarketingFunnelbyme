
function OnSettingSaved(data) {
    if (data.Success == true) {
        $("#Notify").data("kendoNotification").show(data.Message, "info");
        if (data.RedirectURL != null || data.RedirectURL != undefined) {
            document.location.href = data.RedirectURL;
        }
    }
    else {
        $("#Notify").data("kendoNotification").show(data.Message, "error");
    }
}