using AgodaApiExercise.Controllers;
using AgodaApiExercise.Core;
using AgodaApiExercise.Core.RateLimit;
using AgodaApiExercise.Repositories;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using System.Configuration;
using System.Reflection;
using System.Web.Http;

namespace AgodaApiExercise
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Register types
            var blackListPeriod = ConfigurationManager.AppSettings["BlackListPeriodInSeconds"] ?? "10";
            var rateLimitPeriod = ConfigurationManager.AppSettings["RateLimitPeriodInSeconds"] ?? "10";
            var rateLimitThreshold = ConfigurationManager.AppSettings["RateLimitThreshold"] ?? "5";
            var rateLimiter = new RateLimiter(int.Parse(rateLimitPeriod), int.Parse(rateLimitThreshold));
            var blackList = new BlackList(int.Parse(blackListPeriod));

            builder.RegisterInstance(blackList).AsImplementedInterfaces();
            builder.RegisterInstance(rateLimiter).AsImplementedInterfaces();
            builder.RegisterType<CsvLoader>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<HotelRepository>().AsImplementedInterfaces().SingleInstance();

            // Register Action Filters
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            builder.Register(c => new RateLimitFilter(rateLimiter, blackList))
                .AsWebApiActionFilterFor<HotelsController>().SingleInstance();

            // Register your Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

          

          
            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
