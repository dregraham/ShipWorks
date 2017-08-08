using System;
using System.Reactive.Disposables;
using System.Xml;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions for XmlTextWriter
    /// </summary>
    public static class XmlTextWriterExtensions
    {
        /// <summary>
        /// Write start document which will automatically close
        /// </summary>
        public static IDisposable WriteStartDocumentDisposable(this XmlTextWriter writer)
        {
            writer.WriteStartDocument();

            return Disposable.Create(() => writer.WriteEndDocument());
        }

        /// <summary>
        /// Write start element which will automatically close
        /// </summary>
        public static IDisposable WriteStartElementDisposable(this XmlTextWriter writer, string localName)
        {
            writer.WriteStartElement(localName);

            return Disposable.Create(() => writer.WriteEndElement());
        }
    }
}
