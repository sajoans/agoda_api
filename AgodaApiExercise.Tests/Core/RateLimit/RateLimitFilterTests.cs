using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgodaApiExercise.Core.RateLimit;
using NSubstitute;
using System.Web.Http.Controllers;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Runtime.Caching;

namespace AgodaApiExercise.Tests.Core.RateLimit
{
    [TestClass]
    public class RateLimitFilterTests
    {
        private Random _random = new Random(DateTime.Now.Millisecond);


        [TestMethod]
        public async Task ShouldAllowRequestsWithinRateLimitAndNotBlacklisted()
        {
            // Arrange
            var testKey = "key" + _random.Next();
            var mockedRateLimit = Substitute.For<IRateLimiter>();
            var mockedBlackList = Substitute.For<IBlackList>();
            mockedRateLimit.IsWitinLimit(Arg.Any<string>()).Returns(true);
            var testFilter = new RateLimitFilter(mockedRateLimit, mockedBlackList);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");
            request.Headers.Add("Key", testKey);
            var ControllerContext = new HttpControllerContext()
            {
                Request = request
            };
            var actionContext = new HttpActionContext()
            {
                ControllerContext = ControllerContext
            };


            // Act

            await testFilter.OnActionExecutingAsync(actionContext, new System.Threading.CancellationToken(false));

            // Assert
            Assert.IsNull(actionContext.Response);
        }

        [TestMethod]
        public async Task ShouldDisallowRequestsOverRateLimitAndNotBlacklisted()
        {

            // Arrange
            var testKey = "key" + _random.Next();
            var mockedRateLimit = Substitute.For<IRateLimiter>();
            var mockedBlackList = Substitute.For<IBlackList>();
            mockedRateLimit.IsWitinLimit(Arg.Any<string>()).Returns(false);
            var testFilter = new RateLimitFilter(mockedRateLimit, mockedBlackList);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");
            request.Headers.Add("Key", testKey);
            var ControllerContext = new HttpControllerContext()
            {
                Request = request
            };
            var actionContext = new HttpActionContext()
            {
                ControllerContext = ControllerContext
            };


            // Act
            try
            {
                await testFilter.OnActionExecutingAsync(actionContext, new System.Threading.CancellationToken(false));
            }
            catch (HttpResponseException ex)
            {
                // Assert
                Assert.AreEqual((HttpStatusCode)429, ex.Response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }


        [TestMethod]
        public async Task ShouldDisallowRequestsWithinRateLimitAndBlacklisted()
        {

            // Arrange
            var testKey = "key" + _random.Next();
            var mockedRateLimit = Substitute.For<IRateLimiter>();
            var mockedBlackList = Substitute.For<IBlackList>();
            // is within limit
            mockedRateLimit.IsWitinLimit(Arg.Any<string>()).Returns(true);
            // is black listed
            mockedBlackList.IsBlackListed(Arg.Any<string>()).Returns(true);

            var testFilter = new RateLimitFilter(mockedRateLimit, mockedBlackList);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");
            request.Headers.Add("Key", testKey);
            var ControllerContext = new HttpControllerContext()
            {
                Request = request
            };
            var actionContext = new HttpActionContext()
            {
                ControllerContext = ControllerContext
            };


            // Act
            try
            {
                await testFilter.OnActionExecutingAsync(actionContext, new System.Threading.CancellationToken(false));
            }
            catch (HttpResponseException ex)
            {
                // Assert
                Assert.AreEqual((HttpStatusCode)429, ex.Response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task ShouldDisallowRequestsOverRateLimitAndBlacklisted()
        {

            // Arrange
            var testKey = "key" + _random.Next();
            var mockedRateLimit = Substitute.For<IRateLimiter>();
            var mockedBlackList = Substitute.For<IBlackList>();

            // is within limit
            mockedRateLimit.IsWitinLimit(Arg.Any<string>()).Returns(false);
            // is black listed
            mockedBlackList.IsBlackListed(Arg.Any<string>()).Returns(true);

            var testFilter = new RateLimitFilter(mockedRateLimit, mockedBlackList);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");
            request.Headers.Add("Key", testKey);
            var ControllerContext = new HttpControllerContext()
            {
                Request = request
            };
            var actionContext = new HttpActionContext()
            {
                ControllerContext = ControllerContext
            };


            // Act
            try
            {
                await testFilter.OnActionExecutingAsync(actionContext, new System.Threading.CancellationToken(false));
            }
            catch (HttpResponseException ex)
            {
                // Assert
                Assert.AreEqual((HttpStatusCode)429, ex.Response.StatusCode);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public async Task ShouldBlackListKeyWhenOverRateLimit()
        {

            // Arrange
            var testKey = "key" + _random.Next();
            var mockedRateLimit = Substitute.For<IRateLimiter>();
            var mockedBlackList = Substitute.For<IBlackList>();
            // is within limit
            mockedRateLimit.IsWitinLimit(Arg.Any<string>()).Returns(false);

            var testFilter = new RateLimitFilter(mockedRateLimit, mockedBlackList);
            var request = new HttpRequestMessage(HttpMethod.Get, "http://test.com");
            request.Headers.Add("Key", testKey);
            var ControllerContext = new HttpControllerContext()
            {
                Request = request
            };
            var actionContext = new HttpActionContext()
            {
                ControllerContext = ControllerContext
            };


            // Act
            try
            {
                await testFilter.OnActionExecutingAsync(actionContext, new System.Threading.CancellationToken(false));
            }
            catch (Exception ex)
            {
            }

            // Assert
            mockedBlackList.Received().Add(Arg.Is<string>(x => x == testKey));
        }
    }
}
