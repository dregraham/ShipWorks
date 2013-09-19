using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    public class Express1StampsServiceResponseReader : XmlReader
    {
        // the decorated XmlReader
        XmlReader wrappedReader;

        // Details about the remote method being called.  Data in here is used to translate between the two message types.
        LogicalMethodInfo methodInfo;

        // A list of namespaces from which we want to translate
        private static readonly List<string> fromNamespaces = new List<string>
            {
                "xxxxxxxxxxxxxxx", // "www.express1.com/2010/06"
                "yyyyyyyyyyyyyyy"  // ExpressOne.Services
            };

        // The namespace to which we want to translate
        private const string toNamespace = "zzzzzzzzzzzzzzzzzz";

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsServiceResponseReader(LogicalMethodInfo webMethodInfo, XmlReader original)
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

                    // The XML Node for Stamps needs to be the name of the return type
                    // note: have to get the Atomized version of the name or the caller will fail.
                    localName = NameTable.Get(methodInfo.ReturnType.Name);
                }

                return localName;
            }
        }

        /// <summary>
        /// Translates the namespaces apprpriately
        /// </summary>
        private static string GetNamespace(string incomingNamespace)
        {
            string targetNamespace = incomingNamespace;
            if (targetNamespace != null)
            {
                if (fromNamespaces.Any(fn => targetNamespace.Contains(fn)))
                {
                    targetNamespace = toNamespace;
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
}
