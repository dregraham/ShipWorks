using System;
using System.IO;
using System.Reflection;
using log4net;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Utility class for working with embedded resources
    /// </summary>
    public static class ResourceUtility
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ResourceUtility));

        /// <summary>
        /// Read the given embedded resource path from the current assembly as a string
        /// </summary>
        public static string ReadString(string resourcePath)
        {
            using (Stream stream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourcePath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Helper method to save an embeded resource to a file on disk.  
        /// If the output file exists and overwriteIfExists = false, the resource will NOT be written to disk.
        /// </summary>
        /// <param name="resourcePath">Internal path to the resource.</param>
        /// <param name="fullPathAndFileName">Full path and name of the file to which the resource will be saved.</param>
        /// <param name="overwriteIfExists">If true, the output file will be overwritten with the resource content.  If false, and the output file exists, 
        /// nothing will be written to the file.</param>
        /// <exception cref="T:System.ResourceUtilityException" />
        public static void SaveManifestResourceStreamToFile(string resourcePath, string fullPathAndFileName, bool overwriteIfExists)
        {
            try
            {
                // Get the resource stream 
                using (Stream resourceStream = Assembly.GetCallingAssembly().GetManifestResourceStream(resourcePath))
                {
                    // See if the file exists
                    if (File.Exists(fullPathAndFileName))
                    {
                        // It exists, but we will only overwrite if told to do so
                        if (overwriteIfExists)
                        {
                            // Get the output file stream
                            using (var fileStream = File.OpenWrite(fullPathAndFileName))
                            {
                                // Copy the resource into the file
                                resourceStream.CopyTo(fileStream);
                            }
                        }
                    }
                    else
                    {
                        // Get the output file stream
                        using (var fileStream = File.Create(fullPathAndFileName))
                        {
                            // Copy the resource into the file
                            resourceStream.CopyTo(fileStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FileLoadException ||
                    ex is FileNotFoundException ||
                    ex is BadImageFormatException ||
                    ex is UnauthorizedAccessException ||
                    ex is PathTooLongException ||
                    ex is DirectoryNotFoundException ||
                    ex is ObjectDisposedException ||
                    ex is IOException)
                {
                    log.Error("An error occurred attempting to retrieve/save resource.", ex);
                    throw new ResourceUtilityException("ShipWorks was unable retrieve or save the requested resource file.", ex);
                }

                throw;
            }
        }

    }
}
