using Spider.Common;

namespace Demo
{
    public class DemoPlugin:PluginBase
    {
        public override string Area
        {
            get { return "Demo"; }
        }

        public override string NameSpace
        {
            get { return "Demo.Controllers"; }
        }

        public override string Author
        {
            get { return "zhoulq"; }
        }

        public override string Version
        {
            get { return "1.0.0.0"; }
        }
    }
}