using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Spider.Core.Mvc
{
    class SpiderControllerFactory : IControllerFactory
    {
        private class SpiderControllerActivator:IControllerActivator
        {
            private IDependencyResolver _resolver;
            public SpiderControllerActivator():this(null)
            {
            }
            public SpiderControllerActivator(IDependencyResolver resolver)
            {
                _resolver = resolver;
            }
            public IController Create(RequestContext requestContext, Type controllerType)
            {
                if (_resolver!=null)
                {
                    return _resolver.GetService(controllerType) as IController;
                }
                return Activator.CreateInstance(controllerType) as IController;
            }
        }
        private IControllerActivator _activator;
        private List<Type> controllerTypes;
        private Dictionary<string, Type> ControllerTypeCache = new Dictionary<string, Type>();
        private Dictionary<Type, SessionStateBehavior> SessionStateBehaviorCache = new Dictionary<Type, SessionStateBehavior>();
        public SpiderControllerFactory(IDependencyResolver resolver)
            : this(new SpiderControllerActivator(resolver))
        {            
        }
        public SpiderControllerFactory(IControllerActivator activator)
        {
            if (activator != null)
            {
                _activator = activator;
            }
            else
            {
                activator = new SpiderControllerActivator();
            }
            controllerTypes = new List<Type>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                controllerTypes.AddRange(assembly.GetTypes().Where(type => typeof(IController).IsAssignableFrom(type) && !type.IsAbstract));
            }
        }
        public SpiderControllerFactory():this(DependencyResolver.Current)
        {
        }
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            Type controllerType = null;
            string key = requestContext.RouteData.DataTokens["area"] as string;
            key = string.IsNullOrEmpty(key) ? controllerName : key + controllerName;
            if (ControllerTypeCache.ContainsKey(key))
            {
                controllerType = ControllerTypeCache[key];
            }
            else
            {
                controllerType = GetControllerType(requestContext.RouteData, controllerName);
                if (controllerType != null)
                {
                    lock (this)
                    {
                        if (!ControllerTypeCache.ContainsKey(key))
                        {
                            ControllerTypeCache.Add(key, controllerType);
                        }
                    }
                }
                else
                {
                    throw new HttpException(404, "controller not found");
                }
            }
            return _activator.Create(requestContext, controllerType);
        }
        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            Type controllerType = this.GetControllerType(requestContext.RouteData, controllerName);
            if (null == controllerType)
            {
                return SessionStateBehavior.Default;
            }
            else
            {
                if (SessionStateBehaviorCache.ContainsKey(controllerType))
                {
                    return SessionStateBehaviorCache[controllerType];
                }
                else
                {
                    SessionStateAttribute attribute = controllerType.GetCustomAttributes(true).OfType<SessionStateAttribute>()
               .FirstOrDefault();
                    attribute = attribute ?? new SessionStateAttribute(SessionStateBehavior.Default);
                    lock(this)
                    {
                        if (!SessionStateBehaviorCache.ContainsKey(controllerType))
                        {
                            SessionStateBehaviorCache.Add(controllerType,attribute.Behavior); 
                        }
                    }
                    return attribute.Behavior;
                }
            }           
        }
        protected virtual Type GetControllerType(RouteData routeData, string controllerName)
        {
            IEnumerable<Type> types = controllerTypes.Where(t => string.Compare(controllerName + "Controller", t.Name, true) == 0);
            if (types == null || types.Count() == 0)
            {
                return null;
            }
            var namespaces = routeData.DataTokens["Namespaces"] as IEnumerable<string>;
            namespaces = namespaces ?? new string[0];
            Type controllerType = GetControllerType(namespaces, types);
            if (null != controllerType)
            {
                return controllerType;
            }
            bool useNamespaceFallback = true;
            if (null != routeData.DataTokens["UseNamespaceFallback"])
            {
                useNamespaceFallback = (bool)(routeData.DataTokens["UseNamespaceFallback"]);
            }

            if (!useNamespaceFallback)
            {
                return null;
            }
            controllerType = this.GetControllerType(ControllerBuilder.Current.DefaultNamespaces, types);
            if (controllerType != null)
            {
                return controllerType;
            }
            if (types.Count() == 1)
            {
                return types.ToArray()[0];
            }
            throw new InvalidOperationException("Multiple types were found that match the requested controller name.");
        }
        private static bool IsNamespaceMatch(string requestedNamespace, string targetNamespace)
        {
            if (!requestedNamespace.EndsWith(".*", StringComparison.OrdinalIgnoreCase))
            {
                return string.Equals(requestedNamespace, targetNamespace, StringComparison.OrdinalIgnoreCase);
            }
            requestedNamespace = requestedNamespace.Substring(0, requestedNamespace.Length - ".*".Length);
            if (!targetNamespace.StartsWith(requestedNamespace, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return ((requestedNamespace.Length == targetNamespace.Length) || (targetNamespace[requestedNamespace.Length] == '.'));
        }
        private Type GetControllerType(IEnumerable<string> namespaces, IEnumerable<Type> controllerTypes)
        {
            var types = controllerTypes.Where(t => namespaces.Any(ns => IsNamespaceMatch(ns, t.Namespace))).ToArray();
            switch (types.Length)
            {
                case 0: return null;
                case 1: return types[0];
                default: throw new InvalidOperationException("Multiple types were found that match the requested controller name.");
            }
        }
        public void ReleaseController(IController controller)
        {
            IDisposable disposable = controller as IDisposable;
            if (null != disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
