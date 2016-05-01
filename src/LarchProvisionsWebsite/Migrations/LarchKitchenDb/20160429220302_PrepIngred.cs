using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace LarchProvisionsWebsite.Migrations.LarchKitchenDb
{
    public partial class PrepIngred : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(name: "FK_Order_Menu_MenuId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Order_Recipe_RecipeId", table: "Orders");
            migrationBuilder.DropForeignKey(name: "FK_Prep_Ingredient_IngredientId", table: "Preps");
            migrationBuilder.DropForeignKey(name: "FK_Prep_Recipe_RecipeId", table: "Preps");
            migrationBuilder.DropForeignKey(name: "FK_Serving_Menu_MenuId", table: "Servings");
            migrationBuilder.DropForeignKey(name: "FK_Serving_Recipe_RecipeId", table: "Servings");
            migrationBuilder.AddColumn<double>(
                name: "RecipeMeasurment",
                table: "Preps",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Orders",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "PrepId",
                table: "Ingredients",
                nullable: false,
                defaultValue: 0);
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
            migrationBuilder.DropColumn(name: "RecipeMeasurment", table: "Preps");
            migrationBuilder.DropColumn(name: "PrepId", table: "Ingredients");
            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Orders",
                nullable: false);
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