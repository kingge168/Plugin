using System.Configuration;
using System.Web.Hosting;

namespace Spider.Croe
{
    public class WebSettings
    {
        public static readonly string PluginPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["PluginPath"]);
        public static readonly string PluginBinPath = HostingEnvironment.MapPath(ConfigurationManager.AppSettings["PluginBinPath"]);
    }
}
