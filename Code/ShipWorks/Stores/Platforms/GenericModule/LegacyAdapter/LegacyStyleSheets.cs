using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Xsl;
using System.IO;
using System.Reflection;
using System.Xml;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// Class for loading the standard legacy stylesheets
    /// </summary>
    public static class LegacyStyleSheets
    {
        // the collection of standard, loaded stylesheets
        static Dictionary<StoreTypeCode, XslCompiledTransform> loadedStyleSheets = new Dictionary<StoreTypeCode, XslCompiledTransform>();

        /// <summary>
        /// Gets the stylesheet used to transform version 2 XCart based store communications
        /// </summary>
        public static XslCompiledTransform XCartStyleSheet
        {
            get
            {
                return LoadStyleSheet(StoreTypeCode.XCart, "XCartDerivativeStores.xslt");
            }
        }

        /// <summary>
        /// Gets the stylesheet used to transform version 2 Osc based store communications
        /// </summary>
        public static XslCompiledTransform OscStyleSheet
        {
            get
            {
                return LoadStyleSheet(StoreTypeCode.osCommerce, "OscDerivativeStores.xslt");
            }
        }

        /// <summary>
        /// Gets the stylesheet used to transform version 2 of ClickCartPro baesd store communications
        /// </summary>
        public static XslCompiledTransform ClickCartProStyleSheet
        {
            get
            {
                return LoadStyleSheet(StoreTypeCode.ClickCartPro, "ClickCartPro.xslt");
            }
        }

        /// <summary>
        /// Gets the stylesheet used to transform version 2 of CommerceInterface based store communications
        /// </summary>
        public static XslCompiledTransform CommerceInterface
        {
            get
            {
                return LoadStyleSheet(StoreTypeCode.CommerceInterface, "CommerceInterface.xslt");
            }
        }

        /// <summary>
        /// Lazy-load the given stylesheet.  If it has already been loaded, the already loaded one is returned
        /// </summary>
        private static XslCompiledTransform LoadStyleSheet(StoreTypeCode storeTypeCode, string xsltPath)
        {
            // Lock because one store could be downloading while another web client needs to operate
            lock (loadedStyleSheets)
            {
                XslCompiledTransform transform;
                if (!loadedStyleSheets.TryGetValue(storeTypeCode, out transform))
                {
                    transform = LoadStyleSheet(xsltPath);
                    loadedStyleSheets[StoreTypeCode.XCart] = transform;
                }

                return transform;
            }
        }

        /// <summary>
        /// Loads a stylesheet with a certain name from embedded resources.  Exception _____ can be raised
        /// </summary>
        private static XslCompiledTransform LoadStyleSheet(string stylesheetName)
        {
            string resource = @"ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter.Xslt." + stylesheetName;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException(String.Format("Unable to located embedded resource {0}.", resource));
                }

                // load the stream into a stylesheet object for use
                using (XmlReader xmlReader = XmlReader.Create(stream, new XmlReaderSettings() { IgnoreComments = true }))
                {
                    XslCompiledTransform transform = new XslCompiledTransform();
                    transform.Load(xmlReader);

                    return transform;
                }
            }
        }
    }
}
