using Domain.Abstract;
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
        public ActionResult List(int page = 1 )
        {
            ProductsListViewModel model = new ProductsListViewModel
            {
                Productsss = repository.Productss
                    .OrderBy(p=>p.ProductID)
                    .Skip((page-1)*PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ITemsPerPage = PageSize,
                    TotalItems = repository.Productss.Count()
                }

            };
            return View(model);
        }
    }
}