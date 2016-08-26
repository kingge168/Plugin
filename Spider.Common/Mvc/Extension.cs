using System.Web.Mvc;
using System.Web.Routing;

namespace Spider.Mvc
{
    public static class Extension
    {
        public static string Action(this UrlHelper helper, string area, string controller, string action,RouteValueDictionary routeValues)
        {
            if (routeValues != null)
            {
                routeValues["area"] = area;
                return helper.Action(action, controller, routeValues);
            }
            else
            {
                RouteValueDictionary routeValues2 = new RouteValueDictionary();
                routeValues2["area"] = area;
                return helper.Action(action, controller, routeValues2);
            }
            
        }
        public static string Action(this UrlHelper helper, string area, string controller, string action)
        {
            return helper.Action(area,controller,action,new RouteValueDictionary());
        }
    }
}
