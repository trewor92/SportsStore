using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;



namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int _pageSize=4;

        public ProductController(IProductRepository repo)
        {
            _repository = repo;
        }
        public ViewResult List(string category, int pageNum=1)
        {

            return View(new ProductsListViewModel()
            {
                Products = _repository.Products
                .Where(p => category == null || p.Category == category)
                .OrderBy(p => p.ProductID)
                .Skip((pageNum - 1) * _pageSize)
                .Take(_pageSize),
                _pagingInfo = new PagingInfo()
                {
                    CurrentPage = pageNum,
                    ItemsPerPage = _pageSize,
                    TotalItems = category == null ? _repository.Products.Count() :
                    _repository.Products.Count(p => p.Category == category)
                },
                CurrentCategory = category
            });

        }
    }
}