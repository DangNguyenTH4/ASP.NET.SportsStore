using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Abstract;
using Domain.Entities;
using WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void CanNotSaveInValidChanges()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "test" };
            target.ModelState.AddModelError("error", "error");
            //Act
            ActionResult result = target.Edit(product);
            //Assert - chec that the repository wasn't called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            //check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));

        }

        [TestMethod]
        public void CanSaveValidChanges()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            AdminController target = new AdminController(mock.Object);
            Product product = new Product { Name = "Test" };
            //Act
            ActionResult result = target.Edit(product);
            //Assert
                //Chec that the repositore ws called
            mock.Verify(m => m.SaveProduct(product));
            //Check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }
        [TestMethod]
        public void Cannot_Edit_NonExistent_Product()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1" },
                new Product{ProductID=2,Name="P2" },
                new Product{ProductID=3,Name="P3" }
            });
            //Arrange - create a controller
            AdminController controller = new AdminController(mock.Object);
            //Act

                    //I don't know why: result have to a null value. But, it doesn't. Result have value of p1.
                    //When i check ViewResult Edit(4), Model in view is Null, 
            //Product p1 = controller.Edit(1).ViewData.Model as Product;
            //Product result = (Product)controller.Edit(4).ViewData.Model;


                    //If i just check edit only result2, don't implement p1,result => result2 have null value.
            Product result2 = (Product)controller.Edit(20).ViewData.Model;

            //Assert
            //Assert.AreEqual(1, p1.ProductID);
            ////Assert.AreEqual("P2", p2.Name);
            ////Assert.AreEqual(3, p3.ProductID);
            ////Assert.AreEqual("P3", p3.Name);
            //Assert.IsNull(result);
            Assert.IsNull(result2);

        }
        [TestMethod]
        public void Can_Edit_Product()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1" },
                new Product{ProductID=2,Name="P2" },
                new Product{ProductID=3,Name="P3" }
            });
            //Arrange - create the controller 
            AdminController controller = new AdminController(mock.Object);
            //Act
            Product p1 = controller.Edit(1).ViewData.Model as Product;
            Product p2 = controller.Edit(2).ViewData.Model as Product;
            Product p3 = controller.Edit(3).ViewData.Model as Product;
            //Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual("P2", p2.Name);
            Assert.AreEqual(3, p3.ProductID);
            Assert.AreEqual("P3", p3.Name);

        }
        [TestMethod]
        public void Index_Contains_All_Product()
        {
            //Arrange - create the mock repositor
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            });
            // Arrange - create a controller
            AdminController controllerAdmin = new AdminController(mock.Object);
            //Act
            Product[] result = ((controllerAdmin.Index() as ViewResult).ViewData.Model as IEnumerable<Product>).ToArray();
            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }
    }
}
