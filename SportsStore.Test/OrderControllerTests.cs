﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{

    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();

            Order order = new Order();

            OrderController target = new OrderController(cart, mock.Object);

            ViewResult result = target.Checkout(order) as ViewResult;

            // не вызывается(never) метод сохранить заказ
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // возвращается стандарное представление
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            Assert.False(result.ViewData.ModelState.IsValid);

        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetail()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();

            cart.AddItem(new Product(), 1);

            
            OrderController target = new OrderController(cart, mock.Object);

            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // не вызывается(never) метод сохранить заказ
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            // возвращается стандарное представление
            Assert.True(string.IsNullOrEmpty(result.ViewName));

            Assert.False(result.ViewData.ModelState.IsValid);

        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();

            Cart cart = new Cart();

            cart.AddItem(new Product(), 1);

            OrderController target = new OrderController(cart, mock.Object);

            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            // не вызывается(never) метод сохранить заказ
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            // возвращается стандарное представление
            Assert.Equal("Completed", result.ActionName);
           
        }
    }
}
