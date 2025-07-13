using System;
using PluginInterface;
using PluginAttributes;

namespace FourthPlugin
{
    [PluginLoadAttribute("SecondPlugin")]
    public class FourthPlugin : IPlugin
    {
        public void Execute()
        {
            Console.WriteLine("Fourth plugin is executing");
        }
    }
}
