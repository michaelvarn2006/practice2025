using System;
using PluginInterface;
using PluginAttributes;

namespace ThirdPlugin
{
    [PluginLoadAttribute("FirstPlugin")]
    public class ThirdPlugin : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Third plugin is executing");
        }
    }
}
