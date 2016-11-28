using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgodaApiExercise.Core;
using System.Linq;

namespace AgodaApiExercise.Tests.Core
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CsvLoaderShould_LoadDataFromCsvFileWHeader()
        {
            // Arrange
            var testFile = AppDomain.CurrentDomain.BaseDirectory + "\\TestData\\testDataWHeader.csv";
            var csvLoader = new CsvLoader();

            // Act
            var actualResult = csvLoader.ReadLines(testFile);

            // Assert
            Assert.AreEqual(4, actualResult.Count());
            Assert.AreEqual("Bangkok,1,Deluxe,1000", actualResult.First());
        }
    }
}
