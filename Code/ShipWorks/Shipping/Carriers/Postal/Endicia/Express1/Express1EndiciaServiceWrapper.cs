using System;
using ShipWorks.Shipping.Carriers.Postal.Endicia.WebServices.LabelService;
using ShipWorks.ApplicationCore.Logging;
using System.Xml;
using System.Web.Services.Protocols;
using System.Reflection;
using System.Net;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Handles translating an Express1 response into one deserializable by the Endicia classes.
    /// </summary>
    public class Express1EndiciaResponseReader : XmlReader
    {
        // the decorated XmlReader
        XmlReader wrappedReader;

        // Details about the remote method being called.  Data in here is used to translate between the two message types.
        LogicalMethodInfo methodInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaResponseReader(LogicalMethodInfo webMethodInfo, XmlReader original)
        {
            this.wrappedReader = original;
            this.methodInfo = webMethodInfo;
        }

        /// <summary>
        /// Translates Xml Element names when appropriate
        /// </summary>
        public override string LocalName
        {
            get
            {
                string localName = wrappedReader.LocalName;
                if (String.Compare(localName, methodInfo.Name + "Result", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // GetAccountStatusResult -> AccountStatusResponse

                    // The XML Node for Endicia needs to be the name of the return type
                    // note: have to get the Atomized version of the name or the caller will fail.
                    localName = NameTable.Get(methodInfo.ReturnType.Name);
                }

                return localName;
            }
        }

        /// <summary>
        /// Remove
        /// </summary>
        public override bool IsStartElement(string localname, string ns)
        {
            return base.IsStartElement(localname, ns);
        }

        /// <summary>
        /// Translates the namespaces apprpriately
        /// </summary>
        private static string GetNamespace(string incomingNamespace)
        {
            string targetNamespace = incomingNamespace;
            if (targetNamespace != null)
            {
                if (targetNamespace.Contains("www.express1.com/2010/06") ||
                    targetNamespace.Contains("ExpressOne.Services"))
                {
                    targetNamespace = "www.envmgr.com/LabelService";
                }
            }

            return targetNamespace;
        }

        /// <summary>
        /// Translate the MoveToAttribute call's namespace
        /// </summary>
        public override bool MoveToAttribute(string name, string ns)
        {
            return wrappedReader.MoveToAttribute(name, GetNamespace(ns));
        }

        /// <summary>
        /// Translate the MoveToAttribute call's namespace
        /// </summary>
        public override string GetAttribute(string name, string namespaceURI)
        {
            return wrappedReader.GetAttribute(name, GetNamespace(namespaceURI));
        }

        #region pass through

        public override int AttributeCount
        {
            get { return wrappedReader.AttributeCount; }
        }

        public override string BaseURI
        {
            get { return wrappedReader.BaseURI; }
        }

        public override void Close()
        {
            wrappedReader.Close();
        }

        public override int Depth
        {
            get { return wrappedReader.Depth; }
        }

        public override bool EOF
        {
            get { return wrappedReader.EOF; }
        }

        public override string GetAttribute(int i)
        {
            return wrappedReader.GetAttribute(i);
        }


        public override string GetAttribute(string name)
        {
            return wrappedReader.GetAttribute(name);
        }

        public override bool IsEmptyElement
        {
            get { return wrappedReader.IsEmptyElement; }
        }



        public override string LookupNamespace(string prefix)
        {
            return wrappedReader.LookupNamespace(prefix);
        }

        public override bool MoveToAttribute(string name)
        {
            return wrappedReader.MoveToAttribute(name);
        }

        public override bool MoveToElement()
        {
            return wrappedReader.MoveToElement();
        }

        public override bool MoveToFirstAttribute()
        {
            return wrappedReader.MoveToFirstAttribute();
        }

        public override bool MoveToNextAttribute()
        {
            return wrappedReader.MoveToNextAttribute();
        }

        public override XmlNameTable NameTable
        {
            get { return wrappedReader.NameTable; }
        }

        public override string NamespaceURI
        {
            get
            {
                return GetNamespace(wrappedReader.NamespaceURI);
            }
        }

        public override XmlNodeType NodeType
        {
            get { return wrappedReader.NodeType; }
        }

        public override string Prefix
        {
            get { return wrappedReader.Prefix; }
        }

        public override bool Read()
        {
            return wrappedReader.Read();
        }

        public override bool ReadAttributeValue()
        {
            return wrappedReader.ReadAttributeValue();
        }

        public override ReadState ReadState
        {
            get { return wrappedReader.ReadState; }
        }

        public override void ResolveEntity()
        {
            wrappedReader.ResolveEntity();
        }

        public override string Value
        {
            get { return wrappedReader.Value; }
        }

        #endregion
    }

    /// <summary>
    /// Decorating XmlWriter to allow ShipWorks to communicate transparently with Endicia and Express1
    /// </summary>
    public class Express1EndiciaRequestWriter : XmlWriter
    {
        // the writer being decorated
        XmlWriter wrappedWriter;

        // Details about the remote method being called.  Data in here is used to translate between the two message types.
        LogicalMethodInfo methodInfo;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaRequestWriter(LogicalMethodInfo webMethodInfo, XmlWriter originalWriter)
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

            if (targetNamespace != null && targetNamespace.Contains("www.envmgr.com/LabelService"))
            {
                targetNamespace = "http://www.express1.com/2010/06";
            }

            return targetNamespace;
        }

        /// <summary>
        /// Write an attribute under a namespace, but translate from Endicia to Express1
        /// </summary>
        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            wrappedWriter.WriteStartAttribute(prefix, localName, GetNamespace(ns));
        }

        /// <summary>
        /// Express1 parameters are named 'request', so when we see the node being written that is Endicia's parameter name then
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
        /// Express1 parameters are named 'request', so when we see the node being written that is Endicia's parameter name then
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
        /// Lookup the namespace prefix for a translated Endicia -> Express1 namespace
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

    /// <summary>
    /// Custome web service proxy that enables ShipWorks to use Express1's web service via the Endicia Label Server 
    /// classes.
    /// </summary>
    public class Express1EndiciaServiceWrapper : EwsLabelService
    {
        // using relction, we need to set message.method.action, which is what message.Action is
        FieldInfo methodField;
        FieldInfo actionField;

        // the namespaces being swapped
        string expressNamespace = "http://www.express1.com/2010/06/IEwsLabelService";
        string endiciaNamespace = "www.envmgr.com/LabelService";

        WebRequest webRequest = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaServiceWrapper(IApiLogEntry logEntry)
            : base(logEntry)
        {

        }

        /// <summary>
        /// Adjusts the Soap Action to reflect the new endpoint
        /// </summary>
        private void FixupOutgoingSoapMessage(SoapClientMessage message)
        {
            string newAction = message.Action;

            // Change the Outgoing message
            if (newAction.Contains(endiciaNamespace))
            {
                newAction = expressNamespace + message.Action.Remove(0, endiciaNamespace.Length);
            }

            // see if the action on the message needs to be changed
            if (String.Compare(newAction, message.Action, StringComparison.OrdinalIgnoreCase) != 0)
            {
                // use reflection to get access to the underlying object where we need to set the Action
                if (methodField == null)
                {
                    // using relction, we need to set message.method.action, which is what message.Action is
                    methodField = message.GetType().GetField("method", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (methodField == null)
                    {
                        throw new InvalidOperationException("Unable to get the method field");
                    }
                }

                object methodObject = methodField.GetValue(message);
                if (actionField == null)
                {
                    actionField = methodObject.GetType().GetField("action", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (actionField == null)
                    {
                        throw new InvalidOperationException("Unable to get the action field.");
                    }
                }

                actionField.SetValue(methodObject, newAction);
            }

            // Overwrite the SOAPAction header in the request every time, even if we didn't change the action.
            // This is due to a race condition relating to SoapClientMethod.action, which gets used as a prototype for
            // creating SoapClientMessages.  If there are multiple simultineous Express1 calls made for the first time,
            // some will get sent with an invalid SOAPAction if we don't overwrite it every time.
            webRequest.Headers["SOAPAction"] = '"' + newAction + '"';
        }

        /// <summary>
        /// Creates the webrequest
        /// </summary>
        protected override WebRequest GetWebRequest(Uri uri)
        {
            // keep the webrequest so its headers can be changed as necessary
            webRequest = base.GetWebRequest(uri);

            return webRequest;
        }

        /// <summary>
        /// Use a custom message reader so we can change what the incoming XML looks like to callers.
        /// </summary>
        protected override XmlReader GetReaderForMessage(System.Web.Services.Protocols.SoapClientMessage message, int bufferSize)
        {
            // return the custom XmlReader
            return new Express1EndiciaResponseReader(message.MethodInfo, base.GetReaderForMessage(message, bufferSize));
        }

        /// <summary>
        /// Use a custom message writer so we can change the outgoing XML
        /// </summary>
        protected override XmlWriter GetWriterForMessage(System.Web.Services.Protocols.SoapClientMessage message, int bufferSize)
        {
            // Manipulate the outgoing message 
            FixupOutgoingSoapMessage(message);

            // return the custome XmlWriter
            return new Express1EndiciaRequestWriter(message.MethodInfo, base.GetWriterForMessage(message, bufferSize));
        }
    }
}
