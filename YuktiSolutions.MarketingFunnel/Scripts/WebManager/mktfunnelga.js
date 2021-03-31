function BulkAction(e, Link, Grid, Alert, Confirm) {
    e.preventDefault();
    var deleteURL = $(Link).attr("href");
    var selectedCheckBoxes = $("#" + Grid + " .checked-item:checked");
    if (selectedCheckBoxes.length > 0) {
        if (confirm(Confirm) == true) {
            var items = "";
            $(selectedCheckBoxes).each(function () {
                if (items != "") items += ";";
                items += $(this).data("id");
            });
            $.post(deleteURL, { IDs: items }, function (data) {
                if (data.Success == true) {
                    $("#" + Grid).data("kendoGrid").dataSource.read();
                    $(".checkAllCls").prop("checked", false);
                    $("#Notify").data("kendoNotification").show(data.Message, "info");
                }
                else {
                    $("#Notify").data("kendoNotification").show(data.Message, "error");
                }
            });
        }
    }
    else {
        alert(Alert);
    }
}

function GridCheckUncheck(headerCheckBox, Grid) {
    $("#" + Grid + " .checked-item").prop("checked", $(headerCheckBox).is(":checked"));
}

function GetAnalyticsCredential() {
    var searchText = $("#SearchBox").val();
    $("#hdnAnalyticsCredentialID").val(kendo.guid());
    return {
        SearchText: searchText
    };
}

function EditAnalyticsCredential(e, link) {
    e.preventDefault();
    var grid = $("#GridAnalyticsCredentials").data("kendoGrid");
    var tr = $(link).closest("tr");
    var data = grid.dataItem(tr);
    $("#hdnAnalyticsCredentialID").val(data.ID);
    $("#newApplicationName").val(data.ApplicationName);
    $("#newGAEmail").val(data.GAEmail);
    $("#newGAClientSecret").val(data.GAClientSecret);
    $("#newGAViewID").val(data.GAViewID);
    $("#newAnalyticsCredentialDialog").modal("show");
}

function BindAnalyticsCredentialGrid() {
    $("#GridAnalyticsCredentials").data("kendoGrid").dataSource.read();
}
function onAnalyticsCredentialGroupChanged() {
    BindAnalyticsCredentialGrid();
}

function onAnalyticsCredentialsaved(data) {
    if (data.Success === true) {
        $("#hdnAnalyticsCredentialID").val(kendo.guid());
        $("#newApplicationName").val("");
        $("#newApplicationName").val("");
        $("#newGAEmail").val("");
        $("#newGAClientSecret").val("");
        $("#newGAViewID").val("");
        $("#Notify").data("kendoNotification").show(data.Message, "info");
        BindAnalyticsCredentialGrid();
    }
    else {
        $("#Notify").data("kendoNotification").show(data.Message, "error");
    }
}