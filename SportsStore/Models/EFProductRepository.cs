using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public class EFProductRepository:IProductRepository
    {
        private ApplicationDbContext _context;
        public EFProductRepository(ApplicationDbContext ctx)
        {
            _context = ctx;
        }
        public IEnumerable<Product> Products => _context.Products;

        public Product DeleteProduct(int productID)
        {
            Product dbEntry = _context.Products
                    .FirstOrDefault(p => p.ProductID == productID);
            if (dbEntry != null)
            {
                _context.Products.Remove(dbEntry);
                _context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                _context.Products.Add(product);
            }
            else
            {
                Product dbEntry = _context.Products
                    .FirstOrDefault(p => p.ProductID == product.ProductID);


                if(dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }

            }


            _context.SaveChanges();
        }
    }
}
