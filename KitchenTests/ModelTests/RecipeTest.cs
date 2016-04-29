using LarchProvisionsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LarchProvisionsWebsite.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class RecipeTest
    {
        [Fact]
        public void GetRecipeDescriptionTest()
        {
            var recipe = new Recipe();
            recipe.Description = "stew";
            var result = recipe.Description;
            Assert.Equal("stew", result);
        }
    }
}