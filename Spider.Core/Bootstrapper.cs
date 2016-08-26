using System;
using System.IO;
using System.Collections.Generic;
using Spider.Util;
using System.Web.Http;
using Spider.Core;
using Spider.Mvc;
using System.Web.Mvc;
using Spider.Core.Mvc;
using Spider.Common;
namespace Spider.Croe
{
    public class Bootstrapper
    {
        public static void Initialize()
        {
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyFiles = "true";
            AppDomain.CurrentDomain.SetupInformation.ShadowCopyDirectories = WebSettings.PluginBinPath;

            MoveFiles(new DirectoryInfo(WebSettings.PluginPath), new DirectoryInfo(WebSettings.PluginBinPath));

            DomainLoader loader = new DomainLoader("app", "loader", WebSettings.PluginBinPath.Substring(WebSettings.PluginBinPath.LastIndexOf(Path.DirectorySeparatorChar)+1));
            loader.Load();
            loader.Unload();

            RegisterPlugins();
            //ObjectResolver.Init();


            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new SpiderViewEngine());
            ControllerBuilder.Current.SetControllerFactory(new SpiderControllerFactory());

            PluginWatcher watcher = new PluginWatcher();
            watcher.Watch();
        }

        private static void RegisterPlugins()
        {
            IEnumerable<Type> pluginTypes = TypeFinder.FindFromAssemblies<IPlugin>(AppDomain.CurrentDomain.GetAssemblies());
            if (pluginTypes != null)
            {
                foreach (Type pluginType in pluginTypes)
                {
                    IPlugin plugin = Activator.CreateInstance(pluginType) as IPlugin;
                    plugin.RegisterRoute();
                }
            }
        }

        private static void MoveFiles(DirectoryInfo source, DirectoryInfo target)
        {
            DirectoryInfo[] directories = source.GetDirectories("bin", SearchOption.AllDirectories);

            foreach (DirectoryInfo directory in directories)
            {
                FileHelper.MoveFiles(directory, target, "*.dll");
            }
        }
    }
}
