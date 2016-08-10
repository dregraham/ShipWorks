using System.IO;
using System.Text;
using System.Xml;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    /// <summary>
    /// Contains utility methods for helping to test ActionTask objects
    /// </summary>
    public static class TaskTestUtility
    {
        /// <summary>
        /// Serializes the settings of an ActionTask to xml
        /// </summary>
        /// <param name="value">ActionTask that should be serialized</param>
        /// <returns></returns>
        public static string SerializeSettings(this ActionTask value)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.Unicode))
                {
                    xmlWriter.Formatting = Formatting.Indented;

                    xmlWriter.WriteStartDocument(true);
                    xmlWriter.WriteStartElement("Settings");

                    value.SerializeXml(xmlWriter);

                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();

                    xmlWriter.Flush();

                    // Move the stream to the beginning
                    stream.Seek(0, SeekOrigin.Begin);

                    // Read it back
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
