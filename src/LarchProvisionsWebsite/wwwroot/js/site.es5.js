// Write your Javascript code.
'use strict';

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
        complete: function complete() {
            //$.ajax({
            //    url: '@Url.Action("CreateIngredient")',
            //    type: 'POST',
            //    dataType: 'json',
            //    data: $(this).serialize(),
            //    success: function (result) {
            //        var stringResult = "You rock bro! This is your new ingredient: #" + result.IngredientId + " " + result.Amount + " " + result.Unit + " " + result.Name;

            //        $('#ingredient-result').html(stringResult);
            //    }
            //});
        }
    });
    $(".button-collapse").sideNav({
        menuWidth: 300,
        edge: 'right',
        closeOnClick: true
    });
    $(".carousel").carousel();
    $('select').material_select();
    $('.datepicker').pickadate({
        selectMonths: true });

    // Creates a dropdown to control month
    $('.update-order').change(function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Menus/UpdateOrder/',
            data: $(this).serialize(),
            dataType: 'json',
            type: 'POST',
            success: function success(result) {
                var returnString = "";
                console.log(result);
                if (result.OrderSize < 6) {
                    for (var i = 0; i < result.OrderSize; i++) {
                        returnString = returnString + '<i class="material-icons">redeem</i>';
                    }
                }
                if (result.OrderSize >= 6) {
                    for (var i = 0; i <= 6; i++) {
                        returnString = returnString + '<i class="material-icons">redeem</i>';
                    }
                    returnString = returnString + '<i class="material-icons">more_horiz</i>';
                }
                Materialize.toast(result.RecipeName + ' orders updated to ' + result.OrderSize, 4000, "order-toast");
                returnString = returnString + '<p>Orders total: $' + result.CustPrice * result.OrderSize + '</p>';
                $('#orders_recipe_' + result.RecipeId).html(returnString);
            }
        });
    });

    $('.delete-order').submit(function (event) {
        event.preventDefault();
        var orderId = $('OrderId').serialize();
        console.log(orderId);
        $.ajax({
            url: '/Orders/Delete',
            data: $(this).serialize(),
            ajaxsync: true,
            dataType: 'json',
            type: 'POST',
            success: function success(result) {
                Materialize.toast('order deleted!', 4000);
            }
        });
    });

    $('#prep-ingredients-list').on('submit', 'form.prep-ingredient-ajax', function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Recipes/PrepIngredientAjax/',
            data: $(this).serialize(),
            dataType: 'json',
            ajaxsync: true,
            type: 'POST',
            success: function success(result) {
                var params = {
                    RecipeId: result.RecipeId
                };
                updatePrepIngredients(params);
                updateRemoveIngredients(params);
            }
        });
    });

    $('#recipe-ingredients-list').on('submit', 'form.remove-ingredient-ajax', function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Recipes/RemoveIngredientAjax/',
            data: $(this).serialize(),
            dataType: 'json',
            ajaxsync: true,
            type: 'POST',
            success: function success(result) {
                var params = {
                    RecipeId: result.RecipeId
                };
                updateRemoveIngredients(params);
                updatePrepIngredients(params);
            }
        });
    });
    $('.get-details').on('submit', 'form.getdetails', function (event) {
        console.log(params);
        $.ajax({
            type: "GET",
            async: true,
            data: $(this).serialize(),
            contentType: "application/json; charset=utf-8",
            url: '/Home/GetInstDetails/',
            dataType: "json",
            cache: false,
            beforeSend: function beforeSend() {
                $("#loading").show();
            },
            success: function success(data) {
                console.log(data);
                $('#usernameLabel').text(data.data.username);
                $('#nameLabel').text(data.data.full_name);
                $('#bioLabel').text(data.data.bio);
                document.getElementById("imgProfilePic").src = data.data.profile_picture;
            }
        });
    });
});

var updatePrepIngredients = function updatePrepIngredients(params) {
    return $.ajax({
        type: 'GET',
        data: params,
        dataType: 'json',
        url: '/Recipes/IngredientsDisplay/',
        success: function success(result) {
            var prepIngredientsReturn = "";
            result.Ingredients.forEach(function (ingredient) {
                prepIngredientsReturn = prepIngredientsReturn + '<li>' + '<form action="PrepIngredientAjax" class="prep-ingredient-ajax">' + '<input hidden name="RecipeId" value="' + result.RecipeId + '">' + '<input hidden name="IngredientId" value="' + ingredient.IngredientId + '">' + ingredient.Amount + ' ' + ingredient.Unit + ' ' + ingredient.IngredientName + '<button class="btn waves-effect waves-light delete-button" type="submit"><i class="material-icons">assignment_return</i></button>' + '</form>' + '</li>';
            });
            $('#prep-ingredients-list').html(prepIngredientsReturn);
        }
    });
};

var updateRemoveIngredients = function updateRemoveIngredients(params) {
    return $.ajax({
        type: 'GET',
        data: params,
        dataType: 'json',
        url: '/Recipes/RemoveIngredientsDisplay/',
        success: function success(result) {
            var returnString = "";
            console.log(result);
            result.Ingredients.forEach(function (ingredient) {
                returnString = returnString + '<li>' + '<form action="RemoveIngredientAjax" class="remove-ingredient-ajax">' + '<input hidden name="RecipeId" value="' + result.RecipeId + '">' + '<input hidden name="IngredientId" value="' + ingredient.IngredientId + '">' + ingredient.Amount + ' ' + ingredient.Unit + ' ' + ingredient.IngredientName + '<button type="submit" class="delete-button waves-ripple"><span class="delete-icon">' + '<i class="material-icons">delete</i></span></button>' + '</form>' + '</li>';
            });
            $("#recipe-ingredients-list").html(returnString);
        }
    });
};

