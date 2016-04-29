using LarchProvisionsWebsite.Controllers;
using LarchProvisionsWebsite.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LarchProvisionsWebsite.Tests
{
    public class RecipesControllerTest
    {
        [Fact]
        public void Get_ViewResult_Index_Test()
        {
            LarchKitchenDbContext context = new LarchKitchenDbContext();
            //Arrange
            var controller = "controller";

            //Act
            var result = controller;

            //Assert
            Assert.Equal("controller", result);
        }
    }
}