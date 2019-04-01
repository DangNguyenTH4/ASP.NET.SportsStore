using System;
using System.Linq;
using System.Web.Mvc;
using Domain.Abstract;
using Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            //Arrange - create a product with image data
            Product pro = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };
            //arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product{ProductID =1,Name="P1" },
                pro,
                new Product{ProductID=3,Name="P3" }
            }.AsQueryable());
            //Arrange - create the controller
            ProductController target = new ProductController(mock.Object);
            //Act - call the getimage action method
            ActionResult result = target.GetImage(2);
            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(pro.ImageMimeType, ((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cannot_Retrueve_Image_Data_For_Invalid_ID()
        {
            //Arrange create mock
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product{ProductID=1,Name="P1" },
                new Product{ProductID=2,Name="P2" }
            }.AsQueryable());
            //Arrange - creaate controler
            ProductController controller = new ProductController(mock.Object);
            //Act - call the GetImage action method
            ActionResult result = controller.GetImage(100);
            //Assert
            Assert.IsNull(result);
        }
    }
}
