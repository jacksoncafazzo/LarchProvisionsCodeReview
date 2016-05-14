using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Models
{
    [Table("Preps")]
    public class Prep
    {
        [Key]
        public int PrepId { get; set; }

        [ForeignKey("RecipeId")]
        public int RecipeId { get; set; }

        [ForeignKey("IngredientId")]
        public int IngredientId { get; set; }

    }
}