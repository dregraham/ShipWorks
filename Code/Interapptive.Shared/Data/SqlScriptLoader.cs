using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Utility;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Can load sql scripts from a folder or resource location
    /// </summary>
    public class SqlScriptLoader
    {
        private readonly string resourcePath;
        private readonly DirectoryInfo folder;

        // The assemblies from which to load
        private readonly IEnumerable<Assembly> assemblies;

        /// <summary>
        /// Initializes the loader to load scripts from the given resource path
        /// </summary>
        public SqlScriptLoader(string resourcePath)
        {
            this.resourcePath = MethodConditions.EnsureArgumentIsNotNull(resourcePath, nameof(resourcePath));

            assemblies = LoadAssemblies();
        }

        /// <summary>
        /// Initializes the loader to load scripts from the given directory on the disk
        /// </summary>
        public SqlScriptLoader(DirectoryInfo folder)
        {
            this.folder = MethodConditions.EnsureArgumentIsNotNull(folder, nameof(folder));

            assemblies = LoadAssemblies();
        }

        /// <summary>
        /// Load needed assemblies and ManifestResourceNames
        /// </summary>
        private static IEnumerable<Assembly> LoadAssemblies()
        {
            return new[] { Assembly.Load("ShipWorks.Core"), Assembly.Load("ShipWorks.Res") };
        }

        /// <summary>
        /// List of script resources in the resource path
        /// </summary>
        public IEnumerable<string> ScriptResources
        {
            get
            {
                return assemblies.SelectMany(x => x.GetManifestResourceNames())
                    .Where(r => r.StartsWith(resourcePath));
            }
        }

        /// <summary>
        /// Indexer.  Used to load a script by the given name from the configured location.
        /// </summary>
        public SqlScript this[string name]
        {
            get
            {
                return LoadScript(name);
            }
        }

        /// <summary>
        /// Load the sql script of the given name from the configured location.
        /// </summary>
        public virtual SqlScript LoadScript(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (!name.EndsWith(".sql"))
            {
                name += ".sql";
            }

            // Load from resources
            if (resourcePath != null)
            {
                name = name.Replace(@"\", ".");

                // Open the embedded stream
                string resourceToLoad = resourcePath + "." + name;

                Assembly assembly = assemblies.First(a => a.GetManifestResourceNames().Contains(resourceToLoad));

                using (Stream stream = assembly.GetManifestResourceStream(resourceToLoad))
                {
                    if (stream == null)
                    {
                        throw new SqlScriptException(string.Format("SqlScriptLoader cannot locate resource '{0}'", resourceToLoad));
                    }

                    // Return the contents
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return new SqlScript(name, reader.ReadToEnd());
                    }
                }
            }

            // Load from directory
            if (folder != null)
            {
                return new SqlScript(name, File.ReadAllText(Path.Combine(folder.FullName, name)));
            }

            throw new InvalidOperationException("SqlScriptLoader not configured with location.");
        }
    }
}
