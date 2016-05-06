using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarchProvisionsWebsite.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("UserId")]
        public string UserId { get; set; }

        public string UserName { get; set; }

        [NotMapped]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }

        public string RecipeName { get; set; }

        [ForeignKey("MenuId")]
        public int MenuId { get; set; }

        public int OrderSize { get; set; }

        public virtual Menu Menu { get; set; }

        [NotMapped]
        public virtual Recipe Recipe { get; set; }

        //public Order(ApplicationUser user, Recipe recipe, Menu menu)
        //{
        //    UserName = user.NormalizedUserName;
        //    UserId = user.Id;
        //    ApplicationUser = user;
        //    RecipeId = recipe.RecipeId;
        //    RecipeName = recipe.RecipeName;
        //    OrderSize = 1;
        //    Menu = menu;
        //    Recipe = recipe;
        //}
    }
}