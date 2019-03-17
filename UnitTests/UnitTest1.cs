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
    public class TestProduct
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
        public Mock<IProductRepository> CreateAndSetUpMock_Cat()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
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

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //Arrange
                //create the mock repository
            Mock<IProductRepository> mock = CreateAndSetUpMock();
                //create the controller
            NavController controller = new NavController(mock.Object);
                //Define the category to selected
            string categoryToSelect = "Apples";
            //Action
            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;
            //Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = CreateAndSetUpMock_Cat();
            //Arrange - create a controller and make the page size 3 items
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;
            //Action
            //var s1 = target.List("Cat1");
            //var s2 = s1 as ViewResult;
            //var s3 = s2.Model as ProductsListViewModel;
            //var s4 = s3.PagingInfo.TotalItems;

            int res1 = ((target.List("Cat1") as ViewResult).Model as ProductsListViewModel).PagingInfo.TotalItems;
            int res2 = ((target.List("Cat2") as ViewResult).Model as ProductsListViewModel).PagingInfo.TotalItems;
            int res3 = ((target.List("Cat3") as ViewResult).Model as ProductsListViewModel).PagingInfo.TotalItems;
            int resAll = ((target.List(null) as ViewResult).Model as ProductsListViewModel).PagingInfo.TotalItems;

            //Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Can_Add_New_Line()
        {
            //Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            //Arrange - create a new cart
            Cart cartTarget = new Cart();
            //Act
            cartTarget.AddItem(p1, 1);
            cartTarget.AddItem(p2, 4);
            CartLine[] lines = cartTarget.Lines.ToArray();

            //Assert 
            Assert.AreEqual(lines.Length, 2);
            Assert.AreEqual(lines[0].Quantity, 1);
            Assert.AreEqual(lines[1].Quantity, 4);
            Assert.AreEqual(lines[0].Product, p1);
            Assert.AreEqual(lines[1].Product, p2);
        }
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            //Arrange - create a new cart
            Cart cartTarget = new Cart();
            //Act
            cartTarget.AddItem(p1, 1);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p1, 10);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p3, 1);
            cartTarget.AddItem(p3, 1);
            CartLine[] lines = cartTarget.Lines.ToArray();

            //Assert 
            Assert.AreEqual(lines.Length, 3);
            Assert.AreEqual(lines[0].Quantity, 11);
            Assert.AreEqual(lines[1].Quantity, 8);
            Assert.AreEqual(lines[2].Quantity, 2);
            Assert.AreEqual(lines[0].Product, p1);
            Assert.AreEqual(lines[1].Product, p2);
            Assert.AreEqual(lines[2].Product, p3);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            //Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            //Arrange - create a new cart
            Cart cartTarget = new Cart();
            //Act
            cartTarget.AddItem(p1, 1);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p1, 10);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p3, 1);
            cartTarget.AddItem(p3, 1);

            //Act remove line
            cartTarget.RemoveLine(p2);

            CartLine[] lines = cartTarget.Lines.ToArray();

            //Assert 
            Assert.AreEqual(lines.Length, 2);
            Assert.AreEqual(lines[0].Quantity, 11);
            Assert.AreEqual(lines[1].Quantity, 2);
            Assert.AreEqual(lines[0].Product, p1);
            Assert.AreEqual(lines[1].Product, p3);
        }
        [TestMethod]
        public void Can_remove_QuatityItem()
        {
            //Arrange - create some test products
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };
            Product p3 = new Product { ProductID = 3, Name = "P3" };
            //Arrange - create a new cart
            Cart cartTarget = new Cart();
            //Act
            cartTarget.AddItem(p1, 1);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p1, 10);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p3, 1);
            cartTarget.AddItem(p3, 1);
            //Act remove
            cartTarget.RemoveLine(p2);
            cartTarget.RemoveItem(p1, 5);
            CartLine[] lines = cartTarget.Lines.ToArray();

            //Assert 
            Assert.AreEqual(lines.Length, 2);
            Assert.AreEqual(lines[0].Quantity, 6);
            Assert.AreEqual(lines[1].Quantity, 2);
            Assert.AreEqual(lines[0].Product, p1);
            Assert.AreEqual(lines[1].Product, p3);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            //Arrange - Create Product
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            //Arrange - Create Cart
            Cart cartTarget = new Cart();

            //Act - Create Line product
            cartTarget.AddItem(p1, 1);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p1, 10);
            cartTarget.AddItem(p2, 4);

            decimal total = cartTarget.ComputeTotalValue();
            //Assert 
            Assert.AreEqual(total, 1500M);
        }
        [TestMethod]
        public void Can_Clear_Contents()
        {
            //Arrange - Create Product
            Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };
            //Arrange - Create Cart
            Cart cartTarget = new Cart();

            //Act - Create Line product
            cartTarget.AddItem(p1, 1);
            cartTarget.AddItem(p2, 4);
            cartTarget.AddItem(p1, 10);
            cartTarget.AddItem(p2, 4);

            //Act - Clear
            cartTarget.Clear();

            //Assert
            Assert.AreEqual(cartTarget.Lines.Count(), 0);
        }
    }
}
