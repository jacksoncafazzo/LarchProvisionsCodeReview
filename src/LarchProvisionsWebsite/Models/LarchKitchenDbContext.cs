using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Models
{
    public class LarchKitchenDbContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Serving> Servings { get; set; }
        public DbSet<Prep> Preps { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<Recipe>().HasOne(r => r.Menu)
            //    .WithMany(m => m.Recipes)
            //    .HasForeignKey(r => r.MenuId)
            //    .Metadata.DeleteBehavior = DeleteBehavior.Restrict;
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}