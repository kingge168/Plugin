
namespace Spider.Common
{
    public interface IPlugin
    {
        string Area { get; }
        string NameSpace { get; }
        string Author { get; }
        string Version { get; }

        void RegisterRoute();
    }
}
