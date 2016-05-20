using System;
using System.IO;
using System.Reflection;

namespace ShipWorks.Stores.Tests
{
    public class EmbeddedResourceHelper
    {
        public static string GetEmbeddedResourceString(string embeddedResourceName)
        {
            string txt = string.Empty;
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
    }
}
