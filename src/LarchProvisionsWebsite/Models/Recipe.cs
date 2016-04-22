using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarchProvisionsWebsite.Models
{
    [Table("Recipes")]
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }
        public int Price { get; set; }
        public virtual Menu Menu { get; set; }
        public int Servings { get; set; }

        //[NotMapped]
        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Recipe()
        {
            Ingredients = new HashSet<Ingredient>();
            Orders = new HashSet<Order>();
        }
    }
}