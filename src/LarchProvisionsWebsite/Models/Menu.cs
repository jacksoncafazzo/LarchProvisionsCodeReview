using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LarchProvisionsWebsite.Models
{
    [Table("Menus")]
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }

        public DateTime PickupTime { get; set; }
        public DateTime OrderBy { get; set; }
        public string Title { get; set; }

        //[NotMapped] maybe necessary?
        public virtual ICollection<Recipe> Recipes { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public Menu()
        {
            Recipes = new HashSet<Recipe>();
            Orders = new HashSet<Order>();
        }
    }
}