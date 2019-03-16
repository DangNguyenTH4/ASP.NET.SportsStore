using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using Domain.Entities;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HtmlHelpers;
using System;
using System.Web;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        //[TestMethod]
        //public void Can_Paginate()
        //{
        //    //Arrange
        //    Mock<IProductRepository> mk = new Mock<IProductRepository>();
        //    mk.Setup(m => m.Productss).Returns(new Product[]
        //    {
        //        new Product{ ProductID=1,Name="P1" },
        //        new Product{ProductID=2,Name="P2" },
        //        new Product{ProductID=3,Name="P2" },
        //        new Product{ProductID=4,Name="P4" },
        //        new Product{ProductID=5,Name="P5" }
        //    });
        //    ProductController controller = new ProductController(mk.Object);
        //    controller.PageSize = 3;
        //    //act
        //    var s = controller.List(2) as ViewResult;
        //    ProductsListViewModel result = (ProductsListViewModel)s.Model;

        //    Product[] productArr = result.Productsss.ToArray();
        //    //Assert
        //    Assert.IsTrue(productArr.Length == 2);
        //    Assert.AreEqual(productArr[0].Name, "P4", "Not true");
        //    Assert.AreEqual(productArr[1].Name, "P5", "Not true 2");
        //}

        [TestMethod]
        public void Can_Paginate_2_HaveCurrentPage()
        {
            //Arrange
            Mock<IProductRepository> mk = new Mock<IProductRepository>();
            mk.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product{ ProductID=1,Name="P1" },
                new Product{ProductID=2,Name="P2" },
                new Product{ProductID=3,Name="P2" },
                new Product{ProductID=4,Name="P4" },
                new Product{ProductID=5,Name="P5" }
            });
            ProductController controller = new ProductController(mk.Object);
            controller.PageSize = 3;
            //act
            var s = controller.List(null, 2) as ViewResult;
            ProductsListViewModel result = (ProductsListViewModel)s.Model;

            Product[] productArr = result.Productsss.ToArray();
            //Assert
            Assert.IsTrue(productArr.Length == 2);
            Assert.AreEqual(productArr[0].Name, "P4", "Not true");
            Assert.AreEqual(productArr[1].Name, "P5", "Not true 2");
        }

        

        //[TestMethod]
        //public void Can_Send_Pagination_View_Model()
        //{
        //    //Arrange
        //    Mock<IProductRepository> mock = new Mock<IProductRepository>();
        //    mock.Setup(m => m.Productss).Returns(new Product[]
        //    {
        //        new Product {ProductID = 1, Name = "P1"},
        //        new Product {ProductID = 2, Name = "P2"},
        //        new Product {ProductID = 3, Name = "P3"},
        //        new Product {ProductID = 4, Name = "P4"},
        //        new Product {ProductID = 5, Name = "P5"}
        //    });
        //    //Arrange
        //    ProductController controller = new ProductController(mock.Object);
        //    controller.PageSize = 3;

        //    //act
        //    //ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
        //    //ProductsListViewModel result = new ProductsListViewModel();//= (ProductsListViewModel)controller.List(2);

        //    var s = controller.List(2) as ViewResult;
        //    ProductsListViewModel result = (ProductsListViewModel)s.Model;
        //    //Assert
        //    PagingInfo pageInfo = result.PagingInfo;
        //    Assert.AreEqual(pageInfo.CurrentPage, 2);
        //    Assert.AreEqual(pageInfo.ITemsPerPage, 3);
        //    Assert.AreEqual(pageInfo.TotalItems, 5);
        //    Assert.AreEqual(pageInfo.TotalPages, 2);

        //}

        [TestMethod]
        public void Can_Send_Pagination_View_Model_2_HaveCurrentPage()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            });
            //Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //act
            //ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
            //ProductsListViewModel result = new ProductsListViewModel();//= (ProductsListViewModel)controller.List(2);

            var s = controller.List(null,2) as ViewResult;
            ProductsListViewModel result = (ProductsListViewModel)s.Model;
            //Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ITemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);

        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            //Arrange - define an HTML helper - we need to do this
            //in ordoer to apply the extension method
            HtmlHelper myHelper = null;

            //Arrange -create PagingInfo data
            PagingInfo pagingeInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ITemsPerPage = 10
            };

            //Arrange -set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            MvcHtmlString result = myHelper.PageLink(pagingeInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" +
                @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }


        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange
            // Create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            });

            //Create a controller and make the page size 3 items
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Action

            ViewResult s = controller.List("Cat2", 1) as ViewResult;
            var result = ((ProductsListViewModel)s.Model).Productsss.ToArray();

            //Assert 
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[0].Category == "Cat2");
        }
        public Mock<IProductRepository> CreateAndSetUpMock()
        {
            //Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
                new Product {ProductID = 2, Name = "P2", Category = "Apples"},
                new Product {ProductID = 3, Name = "P3", Category = "Plums"},
                new Product {ProductID = 4, Name = "P4", Category = "Oranges"},
            });
            return mock;
        }
        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange - Create the mock repository
            Mock<IProductRepository> mock = CreateAndSetUpMock();

            //Arrange - Create the controller (Nav)
            NavController controller = new NavController(mock.Object);
            //Act
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            //Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Apples");
            Assert.AreEqual(result[1], "Oranges");
            Assert.AreEqual(result[2], "Plums");
        }

    }
}
