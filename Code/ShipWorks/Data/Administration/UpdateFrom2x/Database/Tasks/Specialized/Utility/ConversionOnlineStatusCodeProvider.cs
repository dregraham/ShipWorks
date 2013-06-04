using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores;
using System.Xml;
using Interapptive.Shared.Utility;
using System.Xml.XPath;
using log4net;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized.Utility
{
    /// <summary>
    /// Handles the conversion of a V2 Store's status code blob to a V3 one
    /// </summary>
    public class ConversionOnlineStatusCodeProvider : StatusCodeProvider<string>
    {
        static readonly ILog log = LogManager.GetLogger(typeof(ConversionOnlineStatusCodeProvider));

        string version2StatusCodes = "";
        int storeTypeCode = 0;

        // Cache used to hold Network Solutions Id -> Next Id  workflow paths
        Dictionary<string, string> netSolWorkflow = new Dictionary<string, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ConversionOnlineStatusCodeProvider(int storeTypeCode, string version2StatusCodes)
        {
            this.storeTypeCode = storeTypeCode;
            this.version2StatusCodes = version2StatusCodes;
        }

        /// <summary>
        /// Get the persisted local status codes XML.  Not implemented.
        /// </summary>
        protected override string GetLocalStatusCodesXml()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the ShipWorks 3 XML value 
        /// </summary>
        public string GetShipWorks3Xml()
        {
            return base.SerializeCodeMapToXml(CodeMap);
        }

        /// <summary>
        /// Read the Code/Name pairs from the V2 status field
        /// </summary>
        protected override Dictionary<string, string> LoadLocalStatusCodeMap()
        {
            Dictionary<string, string> codes = new Dictionary<string, string>();

            try
            {
                if (version2StatusCodes.Length > 0)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(version2StatusCodes);

                    XPathNavigator xpath = doc.CreateNavigator();

                    // There are 3 different status code formats
                    if (storeTypeCode == (int)StoreTypeCode.osCommerce ||
                        storeTypeCode == (int)StoreTypeCode.CreLoaded ||
                        storeTypeCode == (int)StoreTypeCode.AmeriCommerce ||
                        storeTypeCode == (int)StoreTypeCode.AuctionSound ||
                        storeTypeCode == (int)StoreTypeCode.ClickCartPro ||
                        storeTypeCode == (int)StoreTypeCode.CommerceInterface ||
                        storeTypeCode == (int)StoreTypeCode.Magento ||
                        storeTypeCode == (int)StoreTypeCode.OrderDynamics ||
                        storeTypeCode == (int)StoreTypeCode.ZenCart ||
                        storeTypeCode == (int)StoreTypeCode.VirtueMart ||
                        storeTypeCode == (int)StoreTypeCode.WebShopManager ||
                        storeTypeCode == (int)StoreTypeCode.ThreeDCart ||
                        storeTypeCode == (int)StoreTypeCode.BigCommerce)
                    {
                        LoadOscStatusFormat(xpath, codes);
                    }
                    else if (storeTypeCode == (int)StoreTypeCode.Miva ||
                             storeTypeCode == (int)StoreTypeCode.XCart ||
                             storeTypeCode == (int)StoreTypeCode.SearchFit)
                    {
                        LoadXCartFormat(xpath, codes);
                    }
                    else if (storeTypeCode == (int)StoreTypeCode.NetworkSolutions)
                    {
                        LoadNetSolFormat(xpath, codes);
                    }
                    else
                    {
                        throw new MigrationException("ConversionOnlineStatusCodeProvider encountered an unsupported store type: " + storeTypeCode);
                    }
                }
            }
            catch (XmlException ex)
            {
                log.ErrorFormat("Failed loading status code xml for conversion", ex);
            }

            return codes;
        }

        /// <summary>
        /// Loads status codes that are Osc formatted
        /// </summary>
        private static void LoadOscStatusFormat(XPathNavigator docXPath, Dictionary<string, string> codes)
        {
            foreach (XPathNavigator xpath in docXPath.Select("//StatusCode"))
            {
                string code = XPathUtility.Evaluate(xpath, "Code", "");
                string name = XPathUtility.Evaluate(xpath, "Name", "");

                // make sure we actually have a status code
                if (code.Length > 0)
                {
                    // don't import duplicates
                    if (!codes.ContainsKey(code))
                    {
                        codes.Add(code, name);
                    }
                }
            }
        }

        /// <summary>
        /// Loads status codes that are XCart formatted
        /// </summary>
        private static void LoadXCartFormat(XPathNavigator docXPath, Dictionary<string, string> codes)
        {
            foreach (XPathNavigator xpath in docXPath.Select("//Status"))
            {
                string code = XPathUtility.Evaluate(xpath, "Code", "");
                string name = XPathUtility.Evaluate(xpath, "Name", "");

                if (code.Length > 0)
                {
                    codes[code] = name;
                }
            }
        }

        /// <summary>
        /// Load status codes that are Network Solutions formatted
        /// </summary>
        private void LoadNetSolFormat(XPathNavigator docXPath, Dictionary<string, string> codes)
        {
            foreach (XPathNavigator xpath in docXPath.Select("//StatusCode"))
            {
                string code = XPathUtility.Evaluate(xpath, "@Id", "");
                string name = XPathUtility.Evaluate(xpath, "text()", "");

                if (code.Length > 0)
                {
                    // Custom NetworkSolutions data
                    string nextIds = XPathUtility.Evaluate(xpath, "@NextIds", "");
                    netSolWorkflow[code] = nextIds;

                    // add the code
                    if (!codes.ContainsKey(code))
                    {
                        codes.Add(code, name);
                    }
                }
            }
        }

        /// <summary>
        /// Write any custom data per status code
        /// </summary>
        protected override void WriteExtendedStatusCodeXml(KeyValuePair<string, string> pair, XmlTextWriter xmlWriter)
        {
            base.WriteExtendedStatusCodeXml(pair, xmlWriter);

            // Network Solutions has some custom fields that need to be written
            if (storeTypeCode == (int)StoreTypeCode.NetworkSolutions)
            {
                // write out the custom Network Solutions data
                string nextStatuses = netSolWorkflow[(string)pair.Key];
                xmlWriter.WriteElementString("NextStatuses", nextStatuses);
            }
        }
    }
}
