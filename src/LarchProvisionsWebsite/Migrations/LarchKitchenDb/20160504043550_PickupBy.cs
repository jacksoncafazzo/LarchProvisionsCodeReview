using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace LarchProvisionsWebsite.Migrations.LarchKitchenDb
{
    public partial class PickupBy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Order_Menu_MenuId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Order_Recipe_RecipeId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Prep_Ingredient_IngredientId", table: "Preps");
            migrationBuilder.DropForeignKey(name: "FK_Prep_Recipe_RecipeId", table: "Preps");
            migrationBuilder.DropForeignKey(name: "FK_Serving_Menu_MenuId", table: "Servings");
            migrationBuilder.DropForeignKey(name: "FK_Serving_Recipe_RecipeId", table: "Servings");
            migrationBuilder.DropColumn(name: "PickupTime", table: "Menus");
            migrationBuilder.AddColumn<DateTime>(
                name: "PickupBy",
                table: "Menus",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddForeignKey(
                name: "FK_Order_Menu_MenuId",
                table: "Orders",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Order_Recipe_RecipeId",
                table: "Orders",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Prep_Ingredient_IngredientId",
                table: "Preps",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Prep_Recipe_RecipeId",
                table: "Preps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Serving_Menu_MenuId",
                table: "Servings",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Cascade);
            migrationBuilder.AddForeignKey(
                name: "FK_Serving_Recipe_RecipeId",
                table: "Servings",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Order_Menu_MenuId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Order_Recipe_RecipeId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Prep_Ingredient_IngredientId", table: "Preps");
            migrationBuilder.DropForeignKey(name: "FK_Prep_Recipe_RecipeId", table: "Preps");
            migrationBuilder.DropForeignKey(name: "FK_Serving_Menu_MenuId", table: "Servings");
            migrationBuilder.DropForeignKey(name: "FK_Serving_Recipe_RecipeId", table: "Servings");
            migrationBuilder.DropColumn(name: "PickupBy", table: "Menus");
            migrationBuilder.AddColumn<DateTime>(
                name: "PickupTime",
                table: "Menus",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.AddForeignKey(
                name: "FK_Order_Menu_MenuId",
                table: "Orders",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Order_Recipe_RecipeId",
                table: "Orders",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Prep_Ingredient_IngredientId",
                table: "Preps",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "IngredientId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Prep_Recipe_RecipeId",
                table: "Preps",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Serving_Menu_MenuId",
                table: "Servings",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.AddForeignKey(
                name: "FK_Serving_Recipe_RecipeId",
                table: "Servings",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
