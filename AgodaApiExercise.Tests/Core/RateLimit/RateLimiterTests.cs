using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgodaApiExercise.Core.RateLimit;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Collections;

namespace AgodaApiExercise.Tests.Core.RateLimit
{
    [TestClass]
    public class RateLimiterTests
    {

        private Random _random = new Random(DateTime.Now.Millisecond);

        [TestMethod]
        public void IsWithinLimit_ShouldReturnTrue_IfRequestCountPerPeriodIsWithinLimit()
        {
            // Arrange
            var testKey = "key" + _random.Next();
            var threshold = 2;
            var period = 10;
            // simulate 1 previous requst 4 seconds ago
            var timestamps = new List<DateTime>() { DateTime.Now.AddSeconds(-4) };
            MemoryCache.Default.Set(testKey, timestamps, DateTime.Now.AddSeconds(period));
            var rateLimiter = new RateLimiter(period, threshold);

            // Act
            var actualResult = rateLimiter.IsWitinLimit(testKey);

            // Assert
            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        public void IsWithinLimit_ShouldReturnTrueIf_RequestCountPerPeriodIsWithinLimitWithOlderRequests()
        {
            // Arrange
            var testKey = "key" + _random.Next();
            var threshold = 2;
            var period = 10;
            // simulate 1 previous requst 4 seconds ago and 1 over 12 seconds ago
            var timestamps = new List<DateTime>() { DateTime.Now.AddSeconds(-4), DateTime.Now.AddSeconds(-12) };
            MemoryCache.Default.Set(testKey, timestamps, DateTime.Now.AddSeconds(period));
            var rateLimiter = new RateLimiter(period, threshold);

            // Act
            var actualResult = rateLimiter.IsWitinLimit(testKey);

            // Assert
            Assert.IsTrue(actualResult);
        }


        [TestMethod]
        public void IsWithinLimit_ShouldReturnFalse_IfRequestCountPerPeriodIsOverLimit()
        {
            // Arrange
            var testKey = "key" + _random.Next();
            var threshold = 2;
            var period = 10;
            // simulate 2 previous requst 4 seconds ago and 2 seconds ago
            var timestamps = new List<DateTime>() { DateTime.Now.AddSeconds(-4), DateTime.Now.AddSeconds(-2) };
            MemoryCache.Default.Set(testKey, timestamps, DateTime.Now.AddSeconds(period));
            var rateLimiter = new RateLimiter(period, threshold);

            // Act
            var actualResult = rateLimiter.IsWitinLimit(testKey);

            // Assert
            Assert.IsFalse(actualResult);
        }
    }
}
