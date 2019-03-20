using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using System.Web.Mvc;
using WebUI.Models;
using Domain.Entities;
using System.Linq;
using WebUI.Controllers;

namespace UnitTests
{
    [TestClass]
    public class CartTest2
    {
        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            //Arrange
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            CartController target = new CartController(null, mock.Object);
            //Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            //Assert
            //--Check that order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),Times.Once());
            //--check that the method is returning the Complelted view
            Assert.AreEqual("Completed", result.ViewName);
            //- check that i am passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);

            
        }
        [TestMethod]
        public void Cannot_Checou_Invalid_ShippingDetails()
        {
            //Arrange 
            //create a mock order processor
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //create a create a cart with an item
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            //create an instance of the controller 
            CartController target = new CartController(null, mock.Object);
            //add an error to the model
            target.ModelState.AddModelError("error", "error");
            //Act - Try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            //Assert
            //check that the order hasn'n been passer on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            //chech that me thod is returning the default view 
            Assert.AreEqual("", result.ViewName);
            //checck that i am passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);


        }
        [TestMethod]
        public void Cannot_CheckOut_Empty_Cart()
        {
            //Arrange - create a mock order processor 
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            //Arrange -create an empty cart
            Cart cart = new Cart();
            //Arrange - create shipping details
            ShippingDetails shippingDetails = new ShippingDetails();
            //Arrange - create an instance of the controller
            CartController target = new CartController(null, mock.Object);

            //Act 
            ViewResult result = target.Checkout(cart, shippingDetails);
            //Assert - check that the orther hasn't bneen passed on to processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());
            //Assert - check the the method is returning the default view 
            Assert.AreEqual("", result.ViewName);
            //Assert - check that i am passing an invalid model to the view 
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void CanAddToCart()
        {
            //Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());

            //Arrange - create a Cart
            Cart cart = new Cart();
            //Arrange - create the controller 
            CartController target = new CartController(mock.Object, null);
            //Act -add a product to the cart
            target.AddToCart(cart, 1, null);
            //Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }
        [TestMethod]
        public void AddingProductToCartGoesToCartScreen()
        {
            //Arrange - create teh mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Productss).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Apples"},
            }.AsQueryable());
            //Arrange - create a Cart
            Cart cart = new Cart();
            //Arrange - create the controller 
            CartController target = new CartController(mock.Object, null);

            //Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");
            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }
        [TestMethod]
        public void CanViewCartContents()
        {
            //Arrange - create a Cart
            Cart cart = new Cart();
            //Arrange - create the controller
            CartController target = new CartController(null,null);

            //Act - cadll the Index action method
            CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            //Asserrt
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
    }
}
