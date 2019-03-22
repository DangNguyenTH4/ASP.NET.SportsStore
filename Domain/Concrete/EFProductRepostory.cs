using System.Collections.Generic;
using System.Linq;
using Domain.Abstract;
using Domain.Entities;
namespace Domain.Concrete
{
    public class EFProductRepostory : IProductRepository
    {
        private EFDbContext context = new EFDbContext();
        public IEnumerable<Product> Productss
        {
            get { return context.Products; }
        }
        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }
    }
}
