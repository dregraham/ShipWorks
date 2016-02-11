using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Can load sql scripts from a folder or resource location
    /// </summary>
    public class SqlScriptLoader
    {
        private readonly string resourcePath;
        private readonly DirectoryInfo folder;

        // The calling assembly.  This has to be set in the constructor, otherwise, LoadAssemblies will
        // use Interapptive.Shared as the calling assembly.
        private Assembly callingAssembly;

        // The assemblies from which to load
        private readonly List<Assembly> assemblies;

        // The manifest resource names from all of the assemblies.
        private List<string> manifestResourceNames; 

        /// <summary>
        /// Initializes the loader to load scripts from the given resource path
        /// </summary>
        public SqlScriptLoader(string resourcePath)
        {
            if (resourcePath == null)
            {
                throw new ArgumentNullException("resourcePath");
            }

            callingAssembly = Assembly.GetCallingAssembly();
            assemblies = new List<Assembly>();
            LoadAssemblies();

            this.resourcePath = resourcePath;
        }

        /// <summary>
        /// Initializes the loader to load scripts from the given directory on the disk
        /// </summary>
        public SqlScriptLoader(DirectoryInfo folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }

            assemblies = new List<Assembly>();
            LoadAssemblies();

            this.folder = folder;
        }

        /// <summary>
        /// Load needed assemblies and ManifestResourceNames
        /// </summary>
        private void LoadAssemblies()
        {
            assemblies.Add(callingAssembly);

            Assembly resourcesAssembly = Assembly.Load("ShipWorks.Res");

            if (resourcesAssembly != null)
            {
                assemblies.Add(resourcesAssembly);
            }

            manifestResourceNames = assemblies.SelectMany(a => a.GetManifestResourceNames()).ToList();
        }

        /// <summary>
        /// All resource names from assemblies
        /// </summary>
        public List<string> ManifestResourceNames
        {
            get { return manifestResourceNames; }
        }

        /// <summary>
        /// The resource path configured for this loader
        /// </summary>
        public string ResourcePath
        {
            get { return resourcePath; }
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
                        throw new SqlScriptException(String.Format("SqlScriptLoader cannot locate resource '{0}'", resourceToLoad));
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
