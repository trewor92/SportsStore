using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using Microsoft.AspNetCore.Http;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        public IProductRepository _repository;
        public CartController(IProductRepository repo)
        {
            _repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        }

        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart",cart);
        }

        public RedirectToActionResult AddToCart(int productID, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
            {
                var cart = GetCart();
                cart.AddItem(product, 1);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productID, string returnUrl)
        {
            var product = _repository.Products.FirstOrDefault(p => p.ProductID == productID);

            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveLine(product);
                SaveCart(cart);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

    }
}