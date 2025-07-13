using System;
using PluginInterface;
using PluginAttributes;

namespace SecondPlugin
{
    [PluginLoadAttribute("ThirdPlugin")]
    public class SecondPlugin : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Second plugin is executing");
        }
    }
}
