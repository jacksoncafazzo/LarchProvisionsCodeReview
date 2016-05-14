using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using LarchProvisionsWebsite.Models;

namespace LarchProvisionsWebsite.Migrations.LarchKitchenDb
{
    [DbContext(typeof(LarchKitchenDbContext))]
    [Migration("20160514025514_CleanPreps")]
    partial class CleanPreps
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Ingredient", b =>
                {
                    b.Property<int>("IngredientId")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<string>("IngredientName");

                    b.Property<int?>("RecipeRecipeId");

                    b.Property<string>("Source");

                    b.Property<string>("Unit");

                    b.HasKey("IngredientId");

                    b.HasAnnotation("Relational:TableName", "Ingredients");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Menu", b =>
                {
                    b.Property<int>("MenuId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("OrderBy");

                    b.Property<DateTime>("PickupBy");

                    b.Property<string>("Title");

                    b.HasKey("MenuId");

                    b.HasAnnotation("Relational:TableName", "Menus");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustPrice");

                    b.Property<int>("MenuId");

                    b.Property<int>("OrderSize");

                    b.Property<int>("RecipeId");

                    b.Property<string>("RecipeName");

                    b.Property<string>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("OrderId");

                    b.HasAnnotation("Relational:TableName", "Orders");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Prep", b =>
                {
                    b.Property<int>("PrepId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("IngredientId");

                    b.Property<int>("RecipeId");

                    b.HasKey("PrepId");

                    b.HasAnnotation("Relational:TableName", "Preps");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Recipe", b =>
                {
                    b.Property<int>("RecipeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustPrice");

                    b.Property<string>("Description");

                    b.Property<string>("Image");

                    b.Property<int?>("MenuMenuId");

                    b.Property<string>("Notes");

                    b.Property<string>("RecipeName");

                    b.Property<int>("ServingSize");

                    b.HasKey("RecipeId");

                    b.HasAnnotation("Relational:TableName", "Recipes");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Serving", b =>
                {
                    b.Property<int>("ServingId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("MenuId");

                    b.Property<int>("RecipeId");

                    b.HasKey("ServingId");

                    b.HasAnnotation("Relational:TableName", "Servings");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Ingredient", b =>
                {
                    b.HasOne("LarchProvisionsWebsite.Models.Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeRecipeId");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Order", b =>
                {
                    b.HasOne("LarchProvisionsWebsite.Models.Menu")
                        .WithMany()
                        .HasForeignKey("MenuId");

                    b.HasOne("LarchProvisionsWebsite.Models.Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Prep", b =>
                {
                    b.HasOne("LarchProvisionsWebsite.Models.Ingredient")
                        .WithMany()
                        .HasForeignKey("IngredientId");

                    b.HasOne("LarchProvisionsWebsite.Models.Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Recipe", b =>
                {
                    b.HasOne("LarchProvisionsWebsite.Models.Menu")
                        .WithMany()
                        .HasForeignKey("MenuMenuId");
                });

            modelBuilder.Entity("LarchProvisionsWebsite.Models.Serving", b =>
                {
                    b.HasOne("LarchProvisionsWebsite.Models.Menu")
                        .WithMany()
                        .HasForeignKey("MenuId");

                    b.HasOne("LarchProvisionsWebsite.Models.Recipe")
                        .WithMany()
                        .HasForeignKey("RecipeId");
                });
        }
    }
}
