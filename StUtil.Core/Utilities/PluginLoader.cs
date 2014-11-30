using StUtil.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace StUtil.Utilities
{
    public class PluginLoader<T>
    {
        /// <summary>
        /// The loaded types
        /// </summary>
        private List<Type> loadedTypes = new List<Type>();

        /// <summary>
        /// Loaded plugins and their file paths
        /// </summary>
        private Dictionary<T, string> pluginPaths = new Dictionary<T, string>();

        /// <summary>
        /// Paths to resolve assemblies
        /// </summary>
        public virtual IEnumerable<string> AssemblyResolvePaths { get; set; }

        /// <summary>
        /// Get a list of loaded plugins
        /// </summary>
        public IEnumerable<T> LoadedPlugins
        {
            get
            {
                return pluginPaths.Keys;
            }
        }

        /// <summary>
        /// List of loaded plugin types
        /// </summary>
        public IEnumerable<Type> LoadedTypes
        {
            get
            {
                return loadedTypes;
            }
        }

        /// <summary>
        /// The file extension filter to use
        /// </summary>
        public virtual string SearchFilter { get; set; }

        /// <summary>
        /// The path to search for plugins
        /// </summary>
        public virtual string SearchPath { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoader{T}" /> class.
        /// </summary>
        /// <param name="searchPath">The search path.</param>
        public PluginLoader(string searchPath)
            : this(searchPath, "*.dll", new string[] { searchPath, Application.StartupPath })
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginLoader{T}" /> class.
        /// </summary>
        /// <param name="searchPath">The search path.</param>
        /// <param name="searchFilter">The search filter.</param>
        /// <param name="assemblyResolvePaths">The assembly resolve paths.</param>
        public PluginLoader(string searchPath, string searchFilter, IEnumerable<string> assemblyResolvePaths)
        {
            this.SearchPath = searchPath;
            this.SearchFilter = searchFilter;
            this.AssemblyResolvePaths = assemblyResolvePaths;
        }

        /// <summary>
        /// Setup an event handler for the current domain assembly resolve event
        /// </summary>
        public void AddAssemblyResolveHandler()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Get the file path to the specific plugin
        /// </summary>
        /// <param name="plugin">The plugin to get the declaring assembly of</param>
        /// <returns>The path to the declaring assembly of the plugin</returns>
        public string GetPluginPath(T plugin)
        {
            return this.pluginPaths.GetOrDefault(plugin, null);
        }

        /// <summary>
        /// Load all plugins found in the set search path
        /// </summary>
        /// <returns>A list of the plugins found</returns>
        public List<T> Load()
        {
            List<T> plugins = new List<T>();

            foreach (string file in Directory.GetFiles(SearchPath, SearchFilter))
            {
                if (pluginPaths.Values.Contains(file))
                    continue;

                try
                {
                    Assembly asm = Assembly.Load(file);
                    IEnumerable<Type> exports = asm.GetTypes().Where(t => typeof(T).IsAssignableFrom(t));
                    foreach (Type t in exports)
                    {
                        try
                        {
                            T obj = (T)Activator.CreateInstance(t);
                            plugins.Add(obj);
                            pluginPaths.Add(obj, file);
                            loadedTypes.Add(t);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            return plugins;
        }

        /// <summary>
        /// Load assembly from a specified file
        /// </summary>
        /// <param name="token">The token to load from</param>
        /// <returns>An assembly loaded from the specified token</returns>
        public Assembly LoadAssemblyFromFile(string token)
        {
            foreach (string path in AssemblyResolvePaths)
            {
                if (System.IO.File.Exists(path + "\\" + token))
                {
                    //Else return the path to the Providers folder
                    return Assembly.LoadFile(path + "\\" + token);
                }

                if (System.IO.File.Exists(path + "\\" + token + ".dll"))
                {
                    //Else return the path to the Providers folder
                    return Assembly.LoadFile(path + "\\" + token + ".dll");
                }
            }
            return null;
        }

        /// <summary>
        /// Remove a specific module
        /// </summary>
        /// <param name="plugin">The module to remove</param>
        public void Remove(T plugin)
        {
            this.loadedTypes.Remove(plugin.GetType());
            this.pluginPaths.Remove(plugin);
        }

        /// <summary>
        /// Remove the event handler for the current domain assembly resolve event
        /// </summary>
        public void RemoveAssemblyResolveHandler()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
        }

        /// <summary>
        /// Resolve any assemblies that have not been automatically located
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="args">The assembly to resolve</param>
        /// <returns></returns>
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //Get the assembly to resolve
            string[] tokens = args.Name.Replace("\\\\", "\\").Split(",".ToCharArray());

            //Ignore resources
            if (tokens[0].EndsWith(".resources"))
                return null;

            //If it is rooted, just return the path
            if (Path.IsPathRooted(tokens[0]))
            {
                return Assembly.LoadFile(tokens[0]);
            }
            else
            {
                return LoadAssemblyFromFile(tokens[0]);
            }
        }
    }
}