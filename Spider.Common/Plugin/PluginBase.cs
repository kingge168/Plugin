using System.Web.Mvc;
using System.Web.Routing;

namespace Spider.Common
{
    public abstract class PluginBase : IPlugin
    {
        public abstract string Area
        {
            get;
        }

        public abstract string NameSpace
        {
            get;
        }

        public abstract string Author
        {
            get;
        }

        public abstract string Version
        {
            get;
        }

        public virtual void RegisterRoute()
        {
            Route route = RouteTable.Routes.MapRoute(
            name: Area,
            url: Area + "/{controller}/{action}",
            namespaces: new string[] { NameSpace }
            );
            route.DataTokens["area"] = Area;
        }
    }
}
