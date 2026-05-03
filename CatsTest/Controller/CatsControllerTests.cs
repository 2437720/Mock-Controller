using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using mock.depart.Controllers;
using mock.depart.Models;
using mock.depart.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsTest
{
    [TestClass]
    public class CatsControllerTests
    {
        

        [TestMethod]
        public void Testing_DeleteCat_NotFound()
        {

            Mock<CatsService> serviceMock = new Mock<CatsService>();
            // Notez l'utilisation de CallBase = true
            // On veut un véritable objet CatsController et changer son comportement seulement pour la propriété UserId!
            // L'option CallBase = true nous permet de garder le comportement normal des méthode de la classe. 
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };
            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(value: null);
            controller.Setup(c => c.UserId).Returns("1");

            var actionResult = controller.Object.DeleteCat(0);
            var result = actionResult.Result as NotFoundResult;

            Assert.IsNotNull(result);



        }

        [TestMethod]
        public void Testing_DeleteWrongOwner()
        {

            Mock<CatsService> serviceMock = new Mock<CatsService>();
            // Notez l'utilisation de CallBase = true
            // On veut un véritable objet CatsController et changer son comportement seulement pour la propriété UserId!
            // L'option CallBase = true nous permet de garder le comportement normal des méthode de la classe. 
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };




            CatOwner catOwner = new CatOwner()
            {
                Id = "1111"
            };

            Cat cat = new Cat()
            {
                Id = 1,
                Name = "Nebulosa",
                CatOwner = catOwner,
                CuteLevel = Cuteness.Amazing
            };


            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(cat);
            controller.Setup(c => c.UserId).Returns("1");

            var actionResult = controller.Object.DeleteCat(0);
            var result = actionResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Cat is not yours", result.Value);
        }


        [TestMethod]
        public void Testing_DeleteBadRequest()
        {

            Mock<CatsService> serviceMock = new Mock<CatsService>();
            // Notez l'utilisation de CallBase = true
            // On veut un véritable objet CatsController et changer son comportement seulement pour la propriété UserId!
            // L'option CallBase = true nous permet de garder le comportement normal des méthode de la classe. 
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            CatOwner catOwner = new CatOwner()
            {
                Id = "1111"
            };

            Cat cat = new Cat()
            {
                Id = 1,
                Name = "Nebulosa",
                CatOwner = catOwner,
                CuteLevel = Cuteness.YouCanKeepIt
            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(cat);
            controller.Setup(c => c.UserId).Returns("1111");

            var actionResult = controller.Object.DeleteCat(1);
            var result  = actionResult.Result as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Cat is too cute", result.Value);


        }

        [TestMethod]
        public void Testing_DeleteCat_Ok()
        {
            Mock<CatsService> serviceMock = new Mock<CatsService>();
            // Notez l'utilisation de CallBase = true
            // On veut un véritable objet CatsController et changer son comportement seulement pour la propriété UserId!
            // L'option CallBase = true nous permet de garder le comportement normal des méthode de la classe. 
            Mock<CatsController> controller = new Mock<CatsController>(serviceMock.Object) { CallBase = true };

            CatOwner catOwner = new CatOwner()
            {
                Id = "1111"
            };

            Cat cat = new Cat()
            {
                Id = 1,
                Name = "Nebulosa",
                CatOwner = catOwner,
                CuteLevel = Cuteness.BarelyOk
            };

            serviceMock.Setup(s => s.Get(It.IsAny<int>())).Returns(cat);
            serviceMock.Setup(s => s.Delete(It.IsAny<int>())).Returns(cat);
            controller.Setup(c => c.UserId).Returns("1111");

            var actionResult = controller.Object.DeleteCat(1);
            var result = actionResult.Result as OkObjectResult;

            Assert.IsNotNull(result);

            Cat? catresult = (Cat?)result!.Value;
            Assert.AreEqual(cat.Id, catresult!.Id);


        }

    }




}




