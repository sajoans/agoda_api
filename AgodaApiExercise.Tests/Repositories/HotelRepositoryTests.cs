using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System.Linq;
using AgodaApiExercise.Core;
using AgodaApiExercise.Repositories;
using System.Collections.Generic;

namespace AgodaApiExercise.Tests.Repositories
{
    [TestClass]
    public class HotelRepositoryTests
    {
        [TestMethod]
        public void HotelRepository_ShouldReturnHotelsByCity()
        {
            // Arrange
            var mockedCsvLoader = Substitute.For<ICsvLoader>();
            var testData = new List<string>() { "Bangkok,1,Deluxe,1000", "Amsterdam,2,Superior,2000", "Ashburn,3,Sweet Suite,1300" };
            mockedCsvLoader.ReadLines(Arg.Any<string>()).Returns(testData);
            var hotelRepository = new HotelRepository(mockedCsvLoader,"");
            var findCity = "Bangkok";

            // Act
            var actualResult = hotelRepository.Get(findCity, null);

            // Assert
            Assert.AreEqual(1, actualResult.Count());
            Assert.AreEqual(findCity, actualResult.First().City);
        }

        [TestMethod]
        public void HotelRepository_ShouldReturnHotelsByCityAndOrderByPrice()
        {
            // Arrange
            var mockedCsvLoader = Substitute.For<ICsvLoader>();
            var testData = new List<string>() { "Bangkok,1,Deluxe,1000", "Bangkok,2,Superior,2000", "Bangkok,3,Sweet Suite,1300" };
            mockedCsvLoader.ReadLines(Arg.Any<string>()).Returns(testData);
            var hotelRepository = new HotelRepository(mockedCsvLoader, "");
            var findCity = "Bangkok";

            // Act
            var actualResult = hotelRepository.Get(findCity, SortOrder.ASC).ToArray();

            // Assert
            Assert.AreEqual(3, actualResult.Count());
            Assert.AreEqual(1000, actualResult[0].Price);
            Assert.AreEqual(1300, actualResult[1].Price);
            Assert.AreEqual(2000, actualResult[2].Price);
        }

        [TestMethod]
        public void HotelRepository_ShouldReturnHotelsByCityAndOrderByPriceDescending()
        {
            // Arrange
            var mockedCsvLoader = Substitute.For<ICsvLoader>();
            var testData = new List<string>() { "Bangkok,1,Deluxe,1000", "Bangkok,2,Superior,2000", "Bangkok,3,Sweet Suite,1300" };
            mockedCsvLoader.ReadLines(Arg.Any<string>()).Returns(testData);
            var hotelRepository = new HotelRepository(mockedCsvLoader, "");
            var findCity = "Bangkok";

            // Act
            var actualResult = hotelRepository.Get(findCity, SortOrder.DESC).ToArray();

            // Assert
            Assert.AreEqual(3, actualResult.Count());
            Assert.AreEqual(2000, actualResult[0].Price);
            Assert.AreEqual(1300, actualResult[1].Price);
            Assert.AreEqual(1000, actualResult[2].Price);
        }
    }
}
