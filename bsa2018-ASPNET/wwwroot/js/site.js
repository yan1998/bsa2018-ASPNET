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

$("#showPosts").click(function () {
    $("#showToDos").removeClass("active");
    $(this).addClass("active");
    $("#posts").show("quick");
    $("#toDos").hide("quick");
});

$("#showToDos").click(function () {
    $("#showPosts").removeClass("active");
    $(this).addClass("active");
    $("#posts").hide("quick");
    $("#toDos").show("quick");
});