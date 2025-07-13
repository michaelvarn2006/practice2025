using System.Reflection;
using PluginInterface;
using PluginAttributes;

namespace PluginLoader
{
    public class PluginLoader
    {
        public void LoadAndExecutePlugins(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory '{path}' not found.");

            var pluginDlls = Directory.EnumerateFiles(path, "*.dll", SearchOption.TopDirectoryOnly)
                .Where(f => Path.GetFileName(f) != "PluginInterface.dll" && Path.GetFileName(f) != "task10.dll")
                .ToList();

            var assemblies = pluginDlls.Select(Assembly.LoadFrom).ToList();

            var pluginTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.GetCustomAttributes(typeof(PluginLoadAttribute), false).Any())
                .ToList();

            var pluginInfos = pluginTypes
                .Select(t => new
                {
                    Type = t,
                    Name = t.Name,
                    Dependencies = ((PluginLoadAttribute)t.GetCustomAttributes(typeof(PluginLoadAttribute), false).First()).Dependencies
                })
                .ToList();

            var nameToInfo = pluginInfos.ToLookup(p => p.Name).ToDictionary(g => g.Key, g => g.First());

            var visited = new HashSet<string>();
            var order = new List<string>();
            void Dfs(string name)
            {
                if (visited.Contains(name)) return;
                visited.Add(name);
                foreach (var dep in nameToInfo[name].Dependencies)
                    Dfs(dep);
                order.Add(name);
            }
            foreach (var name in nameToInfo.Keys)
                Dfs(name);

            order
                .Select(name => nameToInfo[name].Type)
                .Select(t => (IPlugin)Activator.CreateInstance(t)!)
                .ToList()
                .ForEach(plugin => plugin.Execute());
        }
    }
}

