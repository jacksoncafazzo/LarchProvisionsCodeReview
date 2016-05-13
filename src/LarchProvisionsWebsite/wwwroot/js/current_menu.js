$(document).ready(function () {
    $.ajax({
        url: '/Menus/GetMenuAjax',
        dataType: 'json',
        type: 'GET',
        success: function (result) {
            $(body).addClass('loaded');
        }
    });
});