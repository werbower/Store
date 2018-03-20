using System;
using Xunit;
using Moq;
using Store.Controllers;
using Store.Data;
using Store.Models;
using System.Linq;
using System.Collections.Generic;
using Store.ViewModels;

namespace Store.Tests{
    public class Fixture {
        public IQueryable<Product> Products => (new Product[] {
                new Product {ProductId = 1, Name = "P1",Category="Apples"},
                new Product {ProductId = 2, Name = "P2",Category="Apples"},
                new Product {ProductId = 3, Name = "P3",Category="Plums"},
                new Product {ProductId = 4, Name = "P4",Category="Plums"},
                new Product {ProductId = 5, Name = "P5",Category="Oranges"}
            }).AsQueryable<Product>();


    }

    public class ProductControllerTests : IClassFixture<Fixture> {
        Fixture fix;
        public ProductControllerTests(Fixture fix) {
            this.fix = fix;
        }
        [Fact]
        public void Can_Paginate() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(fix.Products);
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            //Act
            var model = controller.List(null,2).ViewData.Model as ProductsListViewModel;
            //Assert
            Product[] prodArray = model.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
        [Fact]
        public void Can_Send_Pagination_View_Model() {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(fix.Products);
            // Arrange
            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };
            // Act
            ProductsListViewModel result = controller.List(null,2).ViewData.Model as ProductsListViewModel;
            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
        [Fact]
        public void Can_Filter_Products() {
            // Arrange
            // - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(fix.Products);
            // Arrange - create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;
            // Action
            Product[] result =(controller.List("Plums", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();
            // Assert
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P3" && result[0].Category == "Plums");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Plums");
        }
    }

}
