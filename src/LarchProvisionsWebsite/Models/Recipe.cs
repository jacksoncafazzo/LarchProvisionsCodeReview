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

        public ICollection<Serving> Servings { get; set; }

        public string RecipeName { get; set; }

        public string Description { get; set; }

        public string Notes { get; set; }
        public int CustPrice { get; set; }

        public string Image { get; set; }

        public int ServingSize { get; set; }

        public virtual ICollection<Prep> Preps { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        [NotMapped]
        public virtual ICollection<Menu> Menus { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Recipe()
        {
            Ingredients = new HashSet<Ingredient>();
            Orders = new HashSet<Order>();
            Menus = new HashSet<Menu>();
        }
    }
}