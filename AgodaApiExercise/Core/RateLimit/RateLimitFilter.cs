using AgodaApiExercise.Core.RateLimit;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace AgodaApiExercise.Core.RateLimit
{
    public class RateLimitFilter : IAutofacActionFilter
    {

        private readonly IRateLimiter _rateLimiter;
        private readonly IBlackList _blackList;

        public RateLimitFilter(IRateLimiter rateLimiter, IBlackList blackList)
        {
            _rateLimiter = rateLimiter;
            _blackList = blackList;
        }

        public Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }

        public Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var controllerContext = actionContext.ControllerContext;
            var ApiKey = controllerContext.Request.Headers.GetValues("Key").FirstOrDefault();
            if (_rateLimiter.IsWitinLimit(ApiKey) && !_blackList.IsBlackListed(ApiKey))
            {
                return Task.FromResult(0);
            }
            // Suspend apiKey for a duration
            _blackList.Add(ApiKey);
            throw new HttpResponseException((HttpStatusCode)429);
        }
    }
}