using System;
using Xunit;
using Moq;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Controllers;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SportsStore.Tests
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

            var result = controller.List(null,2).ViewData.Model as ProductsListViewModel;

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

            var result = controller.List(null,2).ViewData.Model as ProductsListViewModel;

            //Утверждение
            var pageInfo = result._pagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
        [Fact]
        public void Can_Filter_Products()
        {
            // Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1",Category="Cat1"},
            new Product {ProductID=2, Name="P2",Category="Cat2"},
            new Product {ProductID=3, Name="P3",Category="Cat1"},
            new Product {ProductID=4, Name="P4",Category="Cat2"},
            new Product {ProductID=5, Name="P5",Category="Cat3"}
        });

            ProductController controller = new ProductController(mock.Object);
            controller._pageSize = 3;

            //Действие

            var result = controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel;

            //Утверждение
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.True(prodArray[0].Name=="P2" && prodArray[0].Category == "Cat2");
            Assert.True(prodArray[1].Name == "P4" && prodArray[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Организация
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1",Category="Cat1"},
            new Product {ProductID=2, Name="P2",Category="Cat2"},
            new Product {ProductID=3, Name="P3",Category="Cat1"},
            new Product {ProductID=4, Name="P4",Category="Cat2"},
            new Product {ProductID=5, Name="P5",Category="Cat3"}
        });

            ProductController controller = new ProductController(mock.Object);
            controller._pageSize = 3;

            Func<ViewResult, ProductsListViewModel> GetModel = (result => result?.ViewData?.Model as ProductsListViewModel);

            //Действие
            int? res1 = GetModel(controller.List("Cat1"))?._pagingInfo.TotalItems;
            int? res2 = GetModel(controller.List("Cat2"))?._pagingInfo.TotalItems;
            int? res3 = GetModel(controller.List("Cat3"))?._pagingInfo.TotalItems;
            int? resAll = GetModel(controller.List(null))?._pagingInfo.TotalItems;


            

            //Утверждение
            
            Assert.Equal(2,res1);
            Assert.Equal(2, res2);
            Assert.Equal(1, res3);

            Assert.Equal(5, resAll);

            
        }
    }
}
