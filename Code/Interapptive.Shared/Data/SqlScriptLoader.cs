using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Can load sql scripts from a folder or resource location
    /// </summary>
    public class SqlScriptLoader
    {
        string resourcePath;
        DirectoryInfo folder;

        // The assembly to load from
        Assembly assembly;

        /// <summary>
        /// Initializes the loader to load scripts from the given resource path
        /// </summary>
        public SqlScriptLoader(string resourcePath)
        {
            if (resourcePath == null)
            {
                throw new ArgumentNullException("resourcePath");
            }

            this.assembly = Assembly.GetCallingAssembly();
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

            this.assembly = Assembly.GetCallingAssembly();
            this.folder = folder;
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
            return new SqlScript(name, GetScript(name, true));
        }

        /// <summary>
        /// Get the sql script of the given name from the configured location.
        /// </summary>
        /// <param name="name">Script Name.</param>
        /// <param name="throwIfNotFound">
        ///     If script not found, this determines if an error is thrown 
        ///     or an empty string is returned.
        /// </param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">name</exception>
        /// <exception cref="SqlScriptException"></exception>
        /// <exception cref="System.InvalidOperationException">SqlScriptLoader not configured with location.</exception>
        public string GetScript(string name, bool throwIfNotFound)
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
                using (Stream stream = assembly.GetManifestResourceStream(resourceToLoad))
                {
                    if (stream == null)
                    {
                        if (throwIfNotFound)
                        {
                            throw new SqlScriptException(String.Format("SqlScriptLoader cannot locate resource '{0}'", resourceToLoad));                            
                        }

                        return string.Empty;
                    }

                    // Return the contents
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }

            // Load from directory
            if (folder != null)
            {
                return File.ReadAllText(Path.Combine(folder.FullName, name));
            }

            throw new InvalidOperationException("SqlScriptLoader not configured with location.");
        }
    }
}
