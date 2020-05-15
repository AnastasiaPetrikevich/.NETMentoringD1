using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace ReportGenerator.Handlers
{
	public class RouteHandler : IRouteHandler
	{
		public IHttpHandler GetHttpHandler(RequestContext requestContext) => new ReportHandler();

	}
}