using MvcMusicStore.App_Start;
using MvcMusicStore.Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
			DependencyResolver.SetResolver(DependencyResolverConfig.GetConfiguredDependencyResolver());
			AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
			CountersConfig.ConfigureCounters();
		}

		protected void Application_Error(object s, EventArgs e)
		{
			Exception exception = Server.GetLastError();
			var logger = DependencyResolver.Current.GetService(typeof(ILogger)) as ILogger;
			logger?.Error(exception.Message, exception);
		}
	}
}
