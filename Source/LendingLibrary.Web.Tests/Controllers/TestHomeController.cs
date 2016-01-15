using System.Web.Mvc;
using LendingLibrary.Web.Controllers;
using NUnit.Framework;

namespace LendingLibrary.Web.Tests.Controllers
{
    [TestFixture]
    public class TestHomeController
    {
        [Test]
        public void Index_ShouldReturnView()
        {
            //---------------Set up test pack-------------------
            var controller = CreateController();
            //---------------Assert Precondition----------------
            //---------------Execute Test ----------------------
            var result = controller.Index() as ViewResult;
            //---------------Test Result -----------------------
            Assert.IsNotNull(result);
        }

        private static HomeController CreateController()
        {
            return new HomeController();
        }
    }
}