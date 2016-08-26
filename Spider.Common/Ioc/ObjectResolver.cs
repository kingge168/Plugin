using Autofac;
using Autofac.Configuration;
using Autofac.Integration.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Spider.Core
{
    public static class ObjectResolver
    {
        private static IContainer _container;
        internal static void Init()
        {
            ContainerBuilder cb = new ContainerBuilder();
            cb.RegisterModule(new ConfigurationSettingsReader("autofac"));
            cb.RegisterControllers(AppDomain.CurrentDomain.GetAssemblies());
            _container = cb.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(_container));  
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static T Resolve<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }
    }
}
