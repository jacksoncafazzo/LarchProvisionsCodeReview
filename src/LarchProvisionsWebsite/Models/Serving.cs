using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Models
{
    [Table("Servings")]
    public class Serving
    {
        [Key]
        public int ServingId { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        [ForeignKey("Menu")]
        public int MenuId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Menu Menu { get; set; }
    }
}