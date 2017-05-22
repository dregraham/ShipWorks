using System;
using System.IO;
using System.Reflection;

namespace ShipWorks.Tests.Shared
{
    /// <summary>
    /// Helper class for getting embedded resources.
    /// </summary>
    public static class EmbeddedResourceHelper
    {
        /// <summary>
        /// Get a string from the resource at the path relative to the assembly namespace
        /// </summary>
        public static string GetEmbeddedResourceString(this Assembly assembly, string path)
        {
            string resourcePath = assembly.GetName().Name + "." + path;
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Error getting resource {resourcePath}");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Gets the embedded resource string.
        /// </summary>
        public static string GetEmbeddedResourceString(string embeddedResourceName)
        {
            string txt;
            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Error getting the embedded resource");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    txt = reader.ReadToEnd();
                }
            }

            return txt;
        }

        /// <summary>
        /// Gets the embedded resource stream.
        /// </summary>
        public static Stream GetEmbeddedResourceStream(string embeddedResourceName)
        {
            Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(embeddedResourceName);

            if (stream == null)
            {
                throw new InvalidOperationException("Error getting the embedded resource");
            }

            return stream;
        }
    }
}
