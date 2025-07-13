using System;
using PluginInterface;
using PluginAttributes;

namespace FirstPlugin
{
    [PluginLoadAttribute]
    public class FirstPlugin : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("First plugin is executing");
        }
    }
}
