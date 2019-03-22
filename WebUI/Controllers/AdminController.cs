using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository repository;
        public AdminController(IProductRepository repository)
        {
            this.repository = repository;
        }
        // GET: Admin
        public ActionResult Index()
        {
            return View(repository.Productss);
        }
        public ViewResult Edit(int productId)
        {
            Product product = repository.Productss.FirstOrDefault(p => p.ProductID == productId);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["Message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                //There is something wrong with the data va
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }
    }
}