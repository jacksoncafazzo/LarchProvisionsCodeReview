// Write your Javascript code.
$(document).ready(function () {
    $(".slider").slider({ full_width: true });
    $(".button-collapse").sideNav({
        menuWidth: 300, // Default is 240
        edge: 'right', // Choose the horizontal origin
        closeOnClick: true // Closes side-nav on <a> clicks, useful for Angular/Meteor
    });
    $(".carousel").carousel();
    $('select').material_select();
    $('.datepicker').pickadate({
        selectMonths: true, // Creates a dropdown to control month
    });
});