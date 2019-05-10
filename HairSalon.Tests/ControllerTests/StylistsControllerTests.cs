using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HairSalon.Controllers;
using HairSalon.Models;
using System;

namespace HairSalon.Test
{
    [TestClass]
    public class StylistControllerTest
    {
        [TestMethod]
        public void Create_ReturnsCorrectActionType_RedirectToActionResult()
        {
            StylistsController controller = new StylistsController();

            IActionResult view = controller.Create(1, "test");

            Assert.IsInstanceOfType(view, typeof(RedirectToActionResult));
        }
    }
}