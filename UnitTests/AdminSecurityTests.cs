using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace UnitTests
{
    [TestClass]
    public class AdminSecurityTests
    {
        [TestMethod]
        public void Can_Login_With_Valid_Credentials()
        {
            //Arrange - create a mock authentication provider
            Mock<IAuthProvider> mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "secret")).Returns(true);
            //Arrange - create the view model
            LoginViewModel model = new LoginViewModel
            {
                UserName = "admin",
                Password = "secret"
            };
            //Arrange - create the controller
            AccountController target = new AccountController(mock.Object);
            //Act - authenticate using valid credentials
            ActionResult result = target.Login(model, "/MyUrl");
            //Assert = 
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
