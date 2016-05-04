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

        public string IngredientName { get; set; }

        public string Source { get; set; }

        public double Amount { get; set; }

        public string Unit { get; set; }

        public virtual ICollection<Prep> Preps { get; set; }

        //public Ingredient(double amount, string unit, string name, string source)
        //{
        //    Amount = Amount;
        //    Name = name;
        //    Unit = unit;
        //    Source = source;
        //}
    }
}