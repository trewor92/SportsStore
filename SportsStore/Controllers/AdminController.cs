using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository _repository;

        public AdminController(IProductRepository repo)
        {
            _repository = repo;
        }

        public IActionResult Index()
        {
            return View(_repository.Products);
        }


        public IActionResult Edit(int productID)
        {

            return View(_repository.Products.
                FirstOrDefault(p => p.ProductID == productID));
        }


        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _repository.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                //Что-то не так с model
                return View(product);
            }
        }

        public ViewResult Create() => View("Edit", new Product());

        [HttpPost]
        public IActionResult Delete(int productID)
        {
            
            Product deleteProduct = _repository.DeleteProduct(productID);

            if (deleteProduct != null)
            {
                TempData["message"] = $"{deleteProduct.Name} was deleted";
            }
            return RedirectToAction("Index");
            
        }
    }
}