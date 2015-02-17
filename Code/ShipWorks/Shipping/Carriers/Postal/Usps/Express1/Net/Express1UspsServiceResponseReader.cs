using System;
using System.Collections.Generic;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net
{
    public class Express1UspsServiceResponseReader : XmlReader
    {
        // the decorated XmlReader
        XmlReader wrappedReader;

        // A list of namespaces from which we want to translate
        private static readonly Dictionary<string, string> stampsNamespaceMap =
            new Dictionary<string, string> {
                { "http://www.express1.com/2011/08", "http://stamps.com/xml/namespace/2013/05/swsim/swsimv29" }
            };

        // A list of local names we want to translate
        private static readonly Dictionary<string, string> stampsLocalNameMap =
            new Dictionary<string, string> {
                { "AuthenticateUserResult", "Authenticator" },
                { "GetAccountInfoResult", "Authenticator" },
                { "PurchasePostageResult", "Authenticator" },
                { "PurchasePostageViaProPayResult", "Authenticator" },
                { "GetRatesResult", "Authenticator" },
                { "CreateIndiciumResult", "Authenticator" },
                { "CancelIndiciumResult", "Authenticator" },
                { "CreateScanFormResult", "Authenticator" },
                { "CleanseAddressResult", "Authenticator" },
                { "RateV11", "Rate" }
            };

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsServiceResponseReader(XmlReader originalReader)
        {
            this.wrappedReader = originalReader;
        }

        /// <summary>
        /// Translates the Express1 local name to Stamps.
        /// </summary>
        public override string LocalName
        {
            get
            {
                if(wrappedReader.LocalName == null)
                    return null;

                string stampsLocalName;
                if(stampsLocalNameMap.TryGetValue(wrappedReader.LocalName, out stampsLocalName))
                    return stampsLocalName;

                return wrappedReader.LocalName;
            }
        }

        /// <summary>
        /// Translates the Express1 namespace to Stamps.
        /// </summary>
        public override string NamespaceURI
        {
            get
            {
                return GetStampsNamespace(wrappedReader.NamespaceURI);
            }
        }

        /// <summary>
        /// Translates an Express1 namespace to Stamps.
        /// </summary>
        private static string GetStampsNamespace(string express1Namespace)
        {
            if(express1Namespace == null)
                return null;

            string stampsNamespace;
            if(stampsNamespaceMap.TryGetValue(express1Namespace, out stampsNamespace))
                return stampsNamespace;

            return express1Namespace;
        }

        /// <summary>
        /// Translates the MoveToAttribute call's namespace.
        /// </summary>
        public override bool MoveToAttribute(string name, string ns)
        {
            return wrappedReader.MoveToAttribute(name, GetStampsNamespace(ns));
        }

        /// <summary>
        /// Translates the MoveToAttribute call's namespace.
        /// </summary>
        public override string GetAttribute(string name, string namespaceURI)
        {
            return wrappedReader.GetAttribute(name, GetStampsNamespace(namespaceURI));
        }

        /// <summary>
        /// Translates the current value.
        /// </summary>
        public override string Value
        {
            get
            {
                var value = wrappedReader.Value;

                // Express1 is sending zero times in date-only fields
                var zeroTime = value.LastIndexOf("T00:00:00", StringComparison.Ordinal);
                if(zeroTime > 0)
                    return value.Remove(zeroTime);

                return value;
            }
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

        #endregion
    }
}
