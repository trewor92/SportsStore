using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using SportsStore.Controllers;
using SportsStore.Infrastructure;
using SportsStore.Components;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Routing;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
      
            [Fact]
            public void Can_Select_Categories()
            {
                //Организация
                var mock = new Mock<IProductRepository>();

                mock.Setup(m => m.Products)
                    .Returns(new Product[]{
                        new Product { ProductID=1,Name="P1",Category="Apples"},
                        new Product { ProductID=2,Name="P2",Category="Apples"},
                        new Product { ProductID=3,Name="P3",Category="Plums"},
                        new Product { ProductID=4,Name="P4",Category="Oranges"}
                    });

                NavigationMenuViewComponent target =
                    new NavigationMenuViewComponent(mock.Object);

           
                // Действие
               string[] results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult)
                .ViewData.Model).ToArray();
                    

                // Утверждение
                Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, results));

            }

            [Fact]
            public void Indicates_Select_Categories()
            {
                //Организация
                string categoryToSelect = "Apples";

                var mock = new Mock<IProductRepository>();

                mock.Setup(m => m.Products)
                    .Returns(new Product[]{
                            new Product { ProductID=1,Name="P1",Category="Apples"},
                            new Product { ProductID=4,Name="P2",Category="Oranges"},
                        
                });

            NavigationMenuViewComponent target =
                new NavigationMenuViewComponent(mock.Object);

            target.ViewComponentContext = new ViewComponentContext { 
            
                ViewContext = new ViewContext
                {
                    RouteData = new RouteData()
                }
            };

            target.RouteData.Values["category"] = categoryToSelect;

            // Действие
            string results = (string)(target.Invoke() as ViewViewComponentResult)
             .ViewData["SelectedCategory"];

            // Утверждение
            Assert.Equal(categoryToSelect, results);

        }
    }
}
