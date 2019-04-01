using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;
        public ProductController(IProductRepository productRepo)
        {
            this.repository = productRepo;
        }
        // GET: Product
        public ActionResult List(string category, int page = 1 )
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Productsss = repository.Productss
                    .Where(p => category == null || p.Category.Equals(category))
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ITemsPerPage = PageSize,
                    TotalItems = category == null ? repository.Productss.Count()
                                                    //repository.Productss.Where(x => x.Category.Equals(category)).Count()
                                                    //repository.Productss.Count().Where(x => x.Category.Equals(category))
                                                    :repository.Productss.Count(c=>c.Category.Equals(category))
                },
                CurrentCategory = category

            };
            return View(model);
        }
        //public ActionResult List( int page = 1)
        //{
        //    ProductsListViewModel model = new ProductsListViewModel
        //    {
        //        Productsss = repository.Productss
        //            .OrderBy(p => p.ProductID)
        //            .Skip((page - 1) * PageSize)
        //            .Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = page,
        //            ITemsPerPage = PageSize,
        //            TotalItems = repository.Productss.Count()
        //        },
        //        CurrentCategory = null
        //    };
        //    return View(model);
        //}
        public FileContentResult GetImage(int productID)
        {
            Product prod = repository.Productss.FirstOrDefault(p => p.ProductID == productID);
            if (prod != null)
            {
                return File(prod.ImageData, prod.ImageMimeType);
            }
            else
            {
                return null;
            }
        }
    }
}