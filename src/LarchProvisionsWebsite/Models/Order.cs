using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarchProvisionsWebsite.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }

        [NotMapped]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }

        public int Servings { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}