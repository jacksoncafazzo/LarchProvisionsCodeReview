using LarchProvisionsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LarchProvisionsWebsite.Services
{
    public class RecipePriceTotaler
    {
        public int RecipeTotal(Recipe recipe)
        {
            var total = 0;
            foreach (var order in recipe.Orders.ToList())
            {
                total = +recipe.CustPrice;
            }
            return total;
        }
    }
}