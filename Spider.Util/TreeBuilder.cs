using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Util
{
    public static class TreeBuilder
    {
        public static List<dynamic> Build(IEnumerable<dynamic> collection, Func<dynamic, int, dynamic> parser, Func<dynamic, bool> rootSelector, Func<dynamic, dynamic, int, bool> childrenSelector)
        {
            if (collection != null && parser != null && rootSelector != null && childrenSelector != null)
            {
                var roots = collection.Where(rootSelector).Select(item => parser(item, 0)).ToList();
                roots.ForEach(node => { BuildChildren(collection, node, parser, 1, childrenSelector); });
                return roots;
            }
            return null;
        }

        private static void BuildChildren(IEnumerable<dynamic> collection, dynamic parent, Func<dynamic, int, dynamic> parser, int level, Func<dynamic, dynamic, int, bool> childrenSelector)
        {
            var children = collection.Where(t => childrenSelector(t, parent, level)).Select(item => parser(item, level)).ToList();
            if (children != null && children.Count > 0)
            {
                parent.state = "closed";
                parent.children = children;
                children.ForEach(node => { BuildChildren(collection, node, parser, level + 1, childrenSelector); });
            }
        }
    }
}
