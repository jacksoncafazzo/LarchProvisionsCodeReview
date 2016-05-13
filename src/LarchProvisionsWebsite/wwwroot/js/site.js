// Write your Javascript code.
$(document).ready(function () {
    $('.slider').slider({
        full_width: true,
        transition: 1000,
        interval: 9999
    });
    $('.modal-trigger').leanModal({
        dismissable: true,
        opacity: 0.9,
        in_duration: 300,
        out_duration: 200,
        complete: function () {
            $.ajax({
                url: '@Url.Action("CreateIngredient")',
                type: 'POST',
                dataType: 'json',
                data: $(this).serialize(),
                success: function (result) {
                    var stringResult = "You rock bro! This is your new ingredient: #" + result.IngredientId + " " + result.Amount + " " + result.Unit + " " + result.Name;

                    $('#ingredient-result').html(stringResult);
                }
            });
        }
    });
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

    $('.update-order').change(function (event) {
        event.preventDefault();
        theForms = $(this).serialize();
        $.ajax({
            url: '/Orders/UpdateOrder',
            data: theForms,
            ajaxsync: true,
            dataType: 'json',
            type: 'POST',
            success: function (result) {
                var returnString = "";
                console.log(result);
                if (result.OrderSize <= 6) {
                    for (var i = 0 ; i < result.OrderSize; i++) {
                        returnString = returnString + '<i class="material-icons">redeem</i>';
                    }
                }
                if (result.OrderSize > 6) {
                    for (var i = 0 ; i < 6; i++) {
                        returnString = returnString + '<i class="material-icons">redeem</i>';
                    }
                    returnString = returnString + '<i class="material-icons">more_horiz</i>';
                }
                Materialize.toast('order size updated to ' + result.OrderSize, 4000)
                console.log(theForms.custPrice, result.OrderSize);
                returnString = returnString + '<p>Orders total: $' + (parseInt(theForms.custPrice, 0) + result.OrderSize) + '</p>'
                $('#orders_recipe_' + result.RecipeId).html(returnString);
            }
        });
        console.log($(this).serialize());
    });

    $('.delete-order').submit(function (event) {
        event.preventDefault();
        var orderId = $('OrderId').serialize();
        console.log(orderId);
        $.ajax({
            url: '/Orders/Delete/',
            data: $(this).serialize(),
            ajaxsync: true,
            dataType: 'json',
            type: 'POST',
            success: function (result) {
                // Materialize.toast(message, displayLength, className, completeCallback);
                Materialize.toast('order deleted!', 4000) // 4000 is the duration of the toast
            }
        });
    });

    // $('.show-recipes').click(function () {
    //     $.ajax({
    //         type: 'GET',
    //         dataType: 'html',
    //         url: '/Menus/RecipesDisplay',
    //         success: function (result) {
    //             $('#menuRecipesDisplay').html(result);
    //         }
    //     });
    // });

    //AJAX prep new ingredient
    $('.prep-ingredient-ajax').on("click", function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Recipes/PrepIngredientAjax/',
            data: $(this).serialize(),
            ajaxasync: true,
            dataType: 'json',
            type: 'POST',
            success: function (result) {
                console.log(result);
                var returnString = "";
                result.forEach(function (ingredient) {
                    returnString = returnString + '<li>' +
                    ingredient.Amount + ' ' + ingredient.Unit + ' ' + ingredient.IngredientName +
                    '<button name="ingredientId" value="' + ingredient.IngredientId + '" class="delete-button waves-ripple">' +
                    '<span class="delete-icon"><i class="material-icons">delete</i></span></button></li>'
                });
                $("#recipe-ingredients-list").html(returnString);
                window.location.update("/Menus/Edit/" + MenuId);
            }
        });
    });
});