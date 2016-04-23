using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Models
{
    [Table("Ingredients")]
    public class Ingredient
    {
        //private LarchProvisionsDbContext db = new LarchProvisionsDbContext();

        [Key]
        public int IngredientId { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public double Amount { get; set; }

        public string Unit { get; set; }

        [NotMapped]
        public virtual Prep Prep { get; set; }

        [NotMapped]
        public virtual Recipe Recipe { get; set; }

        public double SingleServing(int count)
        {
            double singleServing = Amount / Recipe.ServingSize;
            return singleServing;
        }

        /*Truncate the ingredient return amounts */

        public string ChopNumbers(double n)
        {
            return n.ToString("0.##");
        }
    }
}