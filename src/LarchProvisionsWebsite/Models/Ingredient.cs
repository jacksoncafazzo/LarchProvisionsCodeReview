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

        [ForeignKey("PrepId")]
        public int PrepId { get; set; }

        public double SingleServing(int count, int servingSize)
        {
            double singleServing = Amount / servingSize;
            return singleServing;
        }

        /*Truncate the ingredient return amounts */

        public string ChopNumbers(double n)
        {
            return n.ToString("0.##");
        }
    }
}