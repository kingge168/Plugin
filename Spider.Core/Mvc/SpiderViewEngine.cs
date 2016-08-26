using Spider.Croe;
using System.IO;
using System.Web.Mvc;

namespace Spider.Mvc
{
     class SpiderViewEngine : RazorViewEngine
    {
        private string viewPath=WebSettings.PluginPath.Substring(WebSettings.PluginPath.LastIndexOf(Path.DirectorySeparatorChar)+1);
        public SpiderViewEngine()
        {
            this.AreaMasterLocationFormats = new string[] {"~/"+viewPath+"/{2}/Views/{1}/{0}.cshtml",
    "~/"+viewPath+"/{2}/Views/Shared/{0}.cshtml"};
            this.AreaPartialViewLocationFormats = new string[] {"~/"+viewPath+"/{2}/Views/{1}/{0}.cshtml",
     "~/"+viewPath+"/{2}/Views/Shared/{0}.cshtml"};
            this.AreaViewLocationFormats = new string[] { 
 "~/"+viewPath+"/{2}/Views/{1}/{0}.cshtml", "~/"+viewPath+"/{2}/Views/Shared/{0}.cshtml"
            };
        }
    }
}