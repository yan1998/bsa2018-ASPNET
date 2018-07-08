// Write your JavaScript code.

$(".showTable").click(function (event) {
    var idTable = "todos" + $(this).attr("data-id");
    if ($(this).text() == "Click for show") {
        $(this).text("Click for hide");
        $("#" + idTable).show();
    }
    else {
        $(this).text("Click for show");
        $("#" + idTable).hide();
    }
});