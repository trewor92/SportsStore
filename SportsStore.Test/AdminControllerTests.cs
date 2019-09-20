using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    
    public class AdminControllerTests
    {

        [Fact]
        public void Index_Contains_All_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1"},
            new Product {ProductID=2, Name="P2"},
            new Product {ProductID=3, Name="P3"}
            });

            AdminController target = new AdminController(mock.Object);

            Product[] result = (GetViewModel<IEnumerable<Product>>(target.Index()))?.ToArray();

            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);

        }

        private T GetViewModel<T> (IActionResult result) where T:class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Can_Edit_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1"},
            new Product {ProductID=2, Name="P2"},
            new Product {ProductID=3, Name="P3"}
            });

            AdminController target = new AdminController(mock.Object);

            Product p1 = GetViewModel<Product>(target.Edit(1));
            Product p2 = GetViewModel<Product>(target.Edit(2));
            Product p3 = GetViewModel<Product>(target.Edit(3));


            Assert.Equal("P1", p1.Name);
            Assert.Equal("P2", p2.Name);
            Assert.Equal("P3", p3.Name);
        }


        [Fact]
        public void Cannot_Edit_Nonexisting_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
            new Product {ProductID=1, Name="P1"},
            new Product {ProductID=2, Name="P2"},
            new Product {ProductID=3, Name="P3"}
            });

            AdminController target = new AdminController(mock.Object);

            Product result = GetViewModel<Product>(target.Edit(4));
 
 

            Assert.Null(result);
            
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            var prod = new Product { ProductID = 2, Name = "Test" };

            mock.Setup(m => m.Products).Returns(new Product[] {
          
            new Product {ProductID=1, Name="P1"},
            prod,
            new Product {ProductID=3, Name="P3"}
            });

            AdminController target = new AdminController(mock.Object);

            target.Delete(prod.ProductID);
                     
            mock.Verify(m=>m.DeleteProduct(prod.ProductID));

        }
    }
}
