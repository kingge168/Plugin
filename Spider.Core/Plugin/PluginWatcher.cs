using Spider.Croe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace Spider.Core
{
    internal class PluginWatcher
    {
        private FileSystemWatcher _watcher;
        public PluginWatcher()
        {
            _watcher = new FileSystemWatcher(WebSettings.PluginPath,"*.dll");
            _watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.LastWrite;
        }
        public void Watch()
        {
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Deleted += OnChanged;
            _watcher.Renamed += OnChanged;   
        }
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            HttpRuntime.UnloadAppDomain();
        }
    }
}
