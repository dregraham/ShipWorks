using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Decorating XmlWriter to allow ShipWorks to communicate transparently with Stamps and Express1
    /// </summary>
    public class Express1StampsServiceRequestWriter : XmlWriter
    {
        // the writer being decorated
        XmlWriter wrappedWriter;

        // Details about the remote method being called.  Data in here is used to translate between the two message types.
        LogicalMethodInfo methodInfo;
        
        // A list of namespaces from which we want to translate
        private static readonly List<string> fromNamespaces = new List<string>
            {
                "xxxxxxxxxxxxxxxxxxx" //"www.envmgr.com/LabelService";
            };

        // The namespace to which we want to translate
        private const string toNamespace = "zzzzzzzzzzzzzzzzzz"; //"http://www.express1.com/2010/06";

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsServiceRequestWriter(LogicalMethodInfo webMethodInfo, XmlWriter originalWriter)
        {
            this.wrappedWriter = originalWriter;
            this.methodInfo = webMethodInfo;
        }

        /// <summary>
        /// Determines the name of the first parameter used in the target web method.  
        /// We use this to see how we need to rename response nodes
        /// </summary>
        private string GetWebMethodParameterName()
        {
            if (methodInfo.Parameters.Length > 0)
            {
                return methodInfo.Parameters[0].Name;
            }

            return "";
        }

        /// <summary>
        /// Translates the namespaces apprpriately
        /// </summary>
        private static string GetNamespace(string incomingNamespace)
        {
            string targetNamespace = incomingNamespace;

            if (targetNamespace != null && fromNamespaces.Any(fn => targetNamespace.Contains(fn)))
            {
                targetNamespace = toNamespace;
            }

            return targetNamespace;
        }

        /// <summary>
        /// Write an attribute under a namespace, but translate from Stamps to Express1
        /// </summary>
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartAttribute(prefix, localName, GetNamespace(ns));
        }

        /// <summary>
        /// Express1 parameters are named 'request', so when we see the node being written that is Stamps's parameter name then
        /// we need to rewrite it as 'request'
        /// </summary>
        public override void WriteQualifiedName(string localName, string ns)
        {
            if (String.Compare(localName, GetWebMethodParameterName(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                localName = "request";
            }

            wrappedWriter.WriteQualifiedName(localName, GetNamespace(ns));
        }

        /// <summary>
        /// Express1 parameters are named 'request', so when we see the node being written that is Stamps's parameter name then
        /// we need to rewrite it as 'request'
        /// </summary>
        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            if (String.Compare(localName, GetWebMethodParameterName(), StringComparison.OrdinalIgnoreCase) == 0)
            {
                localName = "request";
            }

            wrappedWriter.WriteStartElement(prefix, localName, GetNamespace(ns));
        }

        /// <summary>
        /// Lookup the namespace prefix for a translated Stamps -> Express1 namespace
        /// </summary>
        public override string LookupPrefix(string ns)
        {
            return wrappedWriter.LookupPrefix(GetNamespace(ns));
        }

        #region pass through
        public override void Close()
        {
            wrappedWriter.Close();
        }

        public override void Flush()
        {
            wrappedWriter.Flush();
        }


        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            wrappedWriter.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            wrappedWriter.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            wrappedWriter.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            wrappedWriter.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            wrappedWriter.WriteComment(text);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            wrappedWriter.WriteDocType(name, pubid, sysid, subset);
        }

        public override void WriteEndAttribute()
        {
            wrappedWriter.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            wrappedWriter.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            wrappedWriter.WriteEndElement();
        }

        public override void WriteEntityRef(string name)
        {
            wrappedWriter.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            wrappedWriter.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            wrappedWriter.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(string data)
        {
            wrappedWriter.WriteRaw(data);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            wrappedWriter.WriteRaw(buffer, index, count);
        }

        public override void WriteStartDocument(bool standalone)
        {
            wrappedWriter.WriteStartDocument(standalone);
        }

        public override void WriteStartDocument()
        {
            wrappedWriter.WriteStartDocument();
        }

        public override WriteState WriteState
        {
            get
            {
                return wrappedWriter.WriteState;
            }
        }

        public override void WriteString(string text)
        {
            wrappedWriter.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            wrappedWriter.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            wrappedWriter.WriteWhitespace(ws);
        }
        #endregion

    }
}
