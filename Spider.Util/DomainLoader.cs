using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Web.Compilation;

namespace Spider.Util
{
    public sealed class DomainLoader
    {
        private AppDomain _appDomain;
        private AppDomain AppDomain
        {
            get { return _appDomain; }
            set { _appDomain = value; }
        }
        private string _applicationName;
        private string ApplicationName
        {
            get { return _applicationName; }
            set
            {
                ArgumentValidator.Validate("applicationName", value, arg => string.IsNullOrEmpty(arg));
                _applicationName = value;
            }
        }
        private string _friendName;
        private string FriendName
        {
            get { return _friendName; }
            set
            {
                ArgumentValidator.Validate("friendName", value, arg => string.IsNullOrEmpty(arg));
                _friendName = value;
            }
        }
        private string _privatePath;
        private string PrivatePath
        {
            get { return _privatePath; }
            set
            {
                ArgumentValidator.Validate("privatePath", value, arg => string.IsNullOrEmpty(arg));
                _privatePath = value;
            }
        }
        public DomainLoader(string applicationName, string friendName, string privatePath)
        {
            ApplicationName = applicationName;
            FriendName = friendName;
            PrivatePath = privatePath;
            Init();
        }
        private void Init()
        {
            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = ApplicationName;
            setup.ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            setup.PrivateBinPath = "bin;" + PrivatePath;
            AppDomain = AppDomain.CreateDomain(FriendName, AppDomain.CurrentDomain.Evidence, setup);
        }
        public void Load()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, PrivatePath);
            DirectoryInfo dir = new DirectoryInfo(path);
            if (dir.Exists)
            {
                FileInfo[] files = dir.GetFiles("*.dll", SearchOption.AllDirectories);
                if (files != null && files.Length > 0)
                {
                    foreach (FileInfo file in files)
                    {
                        Assembly ass = AppDomain.Load(File.ReadAllBytes(file.FullName));
                        BuildManager.AddReferencedAssembly(ass);
                    }
                }
            }
            else
            {
                throw new DirectoryNotFoundException(path);
            }
        }
        public void Unload()
        {
            AppDomain.Unload(AppDomain);
        }
    }
}
