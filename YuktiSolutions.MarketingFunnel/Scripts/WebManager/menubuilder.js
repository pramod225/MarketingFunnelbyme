
function showTest(ID, e) {
    $("#hfmenu").val(ID);
    if (e !== null && e !== undefined) {
        e.preventDefault();
    }
    debugger;

    $.get("/MenuBuilder/GetMenuText?ID=" + ID, function (data) {
        console.log(data);
        $("#texten").val(data.Menu_TextEN);
        $("#textde").val(data.Menu_TextDE);
        $("#textfr").val(data.Menu_TextFR);
        $("#textja").val(data.Menu_TextJA);
    });

    $("#modalmenutitle").modal("show");

}



    $('#savemenutitle').click(function () {

        debugger;
       
        var menutextlanguages = new Array();
        var languages = new Array();

        $('.menutextlang').each(function (index, obj) {
            menutextlanguages.push($(this).val());

            languages.push($(this).attr("lang"));


        });
      
        
            $.ajax({
                type: "POST",
                url: "/MenuBuilder/SaveMenuText",
                data: {
                    "menutextlanguages": menutextlanguages, "IDs": $("#hfmenu").val(), "lang": languages
                },
                success: function (data) {
                    if (data.Success == true) {
                        //$("#MenuBuilder").data("kendoGrid").dataSource.read();
                        $("#Notify").data("kendoNotification").show(data.Message, "info");
                    }
                    else {

                        $("#Notify").data("kendoNotification").show(data.Message, "error");
                    }
                }
            });
        

    });



function onSync(e) {
    if (e != null && e !== undefined) {
        e.preventDefault();
    }
    $("#MenuBuilder").data("kendoGrid").refresh();
    $("#MenuBuilder").data("kendoGrid").dataSource.read();


}


