using System;
using System.IO;
using System.Reflection;

namespace ShipWorks.Stores.Tests
{
    public static class EmbeddedResourceHelper
    {
        public static string GetEmbeddedResourceString(string embeddedResourceName)
        {
            string txt;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
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

        public static Stream GetEmbeddedResourceStream(string embeddedResourceName)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName);

            if (stream == null)
            {
                throw new InvalidOperationException("Error getting the embedded resource");
            }
            
            return stream;
        }
    }
}
