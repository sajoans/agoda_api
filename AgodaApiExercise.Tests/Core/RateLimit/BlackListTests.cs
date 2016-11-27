using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Caching;
using AgodaApiExercise.Core.RateLimit;

namespace AgodaApiExercise.Tests.Core.RateLimit
{
    [TestClass]
    public class BlackListTests
    {

        private BlackList _blackList = new BlackList(10);

        [TestMethod]
        public void IsBlackListed_shouldReturnFalseIfKeyIsBlackListed()
        {
            // Arrange
            string testKey = "test234";
            var blackList = new BlackList(10);
           
            // Act
            var actualResult = _blackList.IsBlackListed(testKey);

            // Assert
            Assert.IsFalse(actualResult);
        }

        [TestMethod]
        public void IsBlackListed_shouldReturnTrueIfKeyIsBlackListed()
        {
            // Arrange
            string testKey = "test1";
            MemoryCache.Default.Set(testKey, true, DateTime.Now.AddSeconds(10));

            // Act
            var actualResult = _blackList.IsBlackListed(testKey);

            // Assert
            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        public void Add_shouldAddKeyToBlackListed()
        {
            // Arrange
            string testKey = "test2341" + new Random(DateTime.Now.Millisecond).Next();

            // Act
            _blackList.Add(testKey);

            // Assert
            Assert.IsTrue((bool)MemoryCache.Default[testKey]);
        }
    }
}
