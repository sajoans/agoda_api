using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgodaApiExercise.Controllers;
using NSubstitute;
using AgodaApiExercise.Repositories;
using AgodaApiExercise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AgodaApiExercise.Tests.Controllers
{
    [TestClass]
    public class HotelsControllerTests
    {
        [TestMethod]
        public void GetHotelsByCity_shouldReturnListOfHotels()
        {
            var mockedRepository = Substitute.For<IHotelRepository>();
            var expectedData = new List<Hotel>()
            {
                new Hotel() {HotelId=123,City="Kansas", Price=200, Room="delux" },
                 new Hotel() {HotelId=123,City="Kansas", Price=200, Room="delux" },
                  new Hotel() {HotelId=123,City="Kansas", Price=200, Room="delux" }
            };
            mockedRepository.Get(Arg.Any<string>(), null).Returns(expectedData);
            var controller = new HotelsController(mockedRepository);

            // Act
            var result = controller.GetHotelsByCity("Kansas");

            // Assert
            Assert.IsTrue(expectedData.SequenceEqual(result));
        }

        [TestMethod]
        public void GetHotelsByCity_shouldReturn404IfNoHotelsFound()
        {
            var mockedRepository = Substitute.For<IHotelRepository>();
            var expectedData = new List<Hotel>();
            mockedRepository.Get(Arg.Any<string>(), null).Returns(expectedData);
            var controller = new HotelsController(mockedRepository);

            // Act
            try
            {
                var result = controller.GetHotelsByCity("Kansas");
            }
            catch (HttpResponseException ex)
            {
                // Assert
                Assert.AreEqual(System.Net.HttpStatusCode.NotFound, ex.Response.StatusCode);
            }

        }
    }
}
