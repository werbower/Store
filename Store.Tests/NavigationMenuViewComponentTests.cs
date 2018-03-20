﻿using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using Moq;
using Store.Components;
using Store.Models;
using Store.Data;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Store.Tests{
    public class NavigationMenuViewComponentTests_Fixture {
        public IQueryable<Product> Products => (new Product[] {
                new Product {ProductId = 1, Name = "P1",Category="Apples"},
                new Product {ProductId = 2, Name = "P2",Category="Apples"},
                new Product {ProductId = 3, Name = "P3",Category="Plums"},
                new Product {ProductId = 4, Name = "P4",Category="Plums"},
                new Product {ProductId = 5, Name = "P5",Category="Oranges"}
            }).AsQueryable<Product>();


    }
    public class NavigationMenuViewComponentTests : IClassFixture<NavigationMenuViewComponentTests_Fixture> {
        private NavigationMenuViewComponentTests_Fixture _fix;

        public NavigationMenuViewComponentTests(NavigationMenuViewComponentTests_Fixture fix) {
            _fix = fix;
        }
        [Fact]
        public void Can_SelectCategories() {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(_fix.Products);
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            //Act
            string[] results = ((target.Invoke() as ViewViewComponentResult).ViewData.Model as IEnumerable<string>).ToArray();
            //Assert
            Assert.True(Enumerable.SequenceEqual(new[] { "Apples", "Oranges", "Plums" }, results));
        }
        [Fact]
        public void Indicates_Selected_Category() {
            // Arrange
            string categoryToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[] {
                new Product {ProductId = 1, Name = "P1", Category = "Apples"},
                new Product {ProductId = 4, Name = "P2", Category = "Oranges"},
            }).AsQueryable<Product>());
            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext {ViewContext = new ViewContext {RouteData = new RouteData() } };

            target.RouteData.Values["category"] = categoryToSelect;
            // Action
            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            // Assert
            Assert.Equal(categoryToSelect, result);
        }

    }
}
