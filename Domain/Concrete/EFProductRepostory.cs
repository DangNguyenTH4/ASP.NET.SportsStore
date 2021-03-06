﻿using System.Collections.Generic;
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
                product.ProductID = GetFinalIDProduct() + 1;
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }
        public Product DeleteProduct(int productID)
        {
            Product dbEntry = context.Products.Find(productID);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
        public int GetFinalIDProduct()
        {
            int count = context.Products.Count();
            var pro = context.Products.ToArray()[count - 1];
            return pro.ProductID;
        }
    }
}
