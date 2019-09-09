using System;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

using SportsStore.Controllers;
using System.Collections.Generic;
using System.Linq;


namespace SportsStore.Test
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            // Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1"},
            new Product {ProductID=2, Name="P2"},
            new Product {ProductID=3, Name="P3"},
            new Product {ProductID=4, Name="P4"},
            new Product {ProductID=5, Name="P5"}
        });

            ProductController controller = new ProductController(mock.Object);
            controller._pageSize = 3;

            //Действие

            var result = controller.List(2).ViewData.Model as ProductsListViewModel;

            //Утверждение
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4",prodArray[0].Name) ;
            Assert.Equal("P5",prodArray[1].Name);
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1"},
            new Product {ProductID=2, Name="P2"},
            new Product {ProductID=3, Name="P3"},
            new Product {ProductID=4, Name="P4"},
            new Product {ProductID=5, Name="P5"}
        });

            ProductController controller = new ProductController(mock.Object);
            controller._pageSize = 3;

            //Действие

            var result = controller.List(2).ViewData.Model as ProductsListViewModel;

            //Утверждение
            var pageInfo = result._pagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}
