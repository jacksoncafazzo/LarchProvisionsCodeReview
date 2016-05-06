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

    $('#remove_order').click(function (event) {
        event.preventDefault();
        $.ajax({
            url: '/Recipes/RemoveOrderAjax',
            data: { menuId: $('input#MenuId').val(), recipeId: $('input#RecipeId').val() },
            ajaxsync: true,
            dataType: 'json',
            success: function (result) {
                alert('booyaka');
                window.location.replace("/Menus/Edit/" + MenuId)
            }
        });
    });

    $('.show-recipes').click(function () {
        $.ajax({
            type: 'GET',
            dataType: 'html',
            url: '/Menus/RecipesDisplay',
            success: function (result) {
                $('#menuRecipesDisplay').html(result);
            }
        });
    });

    //AJAX new ingredient
    //$('.delete-recipe-ajax').on("click", function (event) {
    //    event.preventDefault();
    //    $.ajax({
    //        url: '/Recipes/DeleteRecipeAjax',
    //        data: { id: $('input#recipeId').val() },
    //        ajaxasync: true,
    //        dataType: 'json',
    //        success: function (result) {
    //            alert("You rock bro!");
    //            window.location.replace("/Menus/Edit/" + MenuId);
    //        }
    //    });
    //});
});