using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;
using Nop.Web.Framework.Localization;

namespace Nop.Search.Plugin.GSA
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {

            var route = routes.MapLocalizedRoute("ProductSearch",
                        "search/",
                        new { controller = "GsaSearch", action = "Search" },
                        new[] { "Nop.Search.Plugin.GSA.Controllers" });
            routes.Remove(route);
            routes.Add(route);
        }
        public int Priority
        {
            get
            {
                return 10;
            }
        }
    }
}
