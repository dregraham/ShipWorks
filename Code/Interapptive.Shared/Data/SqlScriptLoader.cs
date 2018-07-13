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
        /// Load needed assemblies and ManifestResourceNames
        /// </summary>
        private static IEnumerable<Assembly> LoadAssemblies() =>
            new[] { Assembly.Load("ShipWorks.Core"), Assembly.Load("ShipWorks.Res") };

        /// <summary>
        /// List of script resources in the resource path
        /// </summary>
        public IEnumerable<string> ScriptResources =>
            assemblies.SelectMany(x => x.GetManifestResourceNames())
                .Where(r => r.StartsWith(resourcePath))
                .Where(r => r.EndsWith(".sql"));

        /// <summary>
        /// Indexer.  Used to load a script by the given name from the configured location.
        /// </summary>
        public ISqlScript this[string name] => LoadScript(name);

        /// <summary>
        /// Load the sql script of the given name from the configured location.
        /// </summary>
        public virtual ISqlScript LoadScript(string name)
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

                Assembly assembly = assemblies.FirstOrDefault(a => a.GetManifestResourceNames().Contains(resourceToLoad));

                if (assembly == null)
                {
                    resourceToLoad = name;
                    assembly = assemblies.First(a => a.GetManifestResourceNames().Contains(resourceToLoad));
                }

                return GetResourceString(assembly, resourceToLoad)
                    .Map(script => new SqlScript(name, script))
                    .Map(x => AddMetadata(assembly, name, x))
                    .Match(x => x, ex => throw ex);
            }

            throw new InvalidOperationException("SqlScriptLoader not configured with location.");
        }

        /// <summary>
        /// Try and add metadata for the script, if any exists
        /// </summary>
        private ISqlScript AddMetadata(Assembly assembly, string name, ISqlScript script) =>
            GetResourceString(assembly, name + ".xml")
                .Match(x => (ISqlScript) new SqlScriptWithMetadata(script, x), _ => script);

        /// <summary>
        /// Get a resource string from the given assembly
        /// </summary>
        private static GenericResult<string> GetResourceString(Assembly assembly, string resource)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    return new SqlScriptException(string.Format("SqlScriptLoader cannot locate resource '{0}'", resource)); ;
                }

                // Return the contents
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
