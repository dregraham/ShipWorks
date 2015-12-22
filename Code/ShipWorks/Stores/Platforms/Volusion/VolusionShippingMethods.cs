using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.IO.Text.Csv;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared;
using Interapptive.Shared.Utility;
using log4net;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// For working with the Volusion Shipping Methods data
    /// </summary>
    public class VolusionShippingMethods
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionShippingMethods));

        VolusionStoreEntity store;

        // cache of loaded shipment methods
        Dictionary<int, string> shippingMethodMap = new Dictionary<int, string>();


        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionShippingMethods(VolusionStoreEntity store)
        {
            this.store = store;

            LoadShippingMethods();
        }

        /// <summary>
        /// Gets the number of shipping methods in the cache
        /// </summary>
        public int Count
        {
            get
            {
                return shippingMethodMap.Count;
            }
        }

        /// <summary>
        /// Loads the cached xml representation of shipping methods from the store
        /// </summary>
        private void LoadShippingMethods()
        {
            try
            {
                string shippingMethodsXml = store.ShipmentMethods;

                if (shippingMethodsXml.Length == 0)
                {
                    shippingMethodMap.Clear();
                    return;
                }

                // load the new values from the xml blob
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(shippingMethodsXml);

                // clear the current value
                shippingMethodMap.Clear();

                XPathNavigator navigator = doc.CreateNavigator();
                XPathNodeIterator iterator = navigator.Select("//ShippingMethod");
                while (iterator.MoveNext())
                {
                    int id = XPathUtility.Evaluate(iterator.Current, "@id", 0);
                    string name = XPathUtility.Evaluate(iterator.Current, "@name", "");

                    shippingMethodMap[id] = name;
                }
            }
            catch (XmlException)
            {
                // nothing, treat it is just invalid
                log.Error("Invalid Volusion Shipping Method XML in the database.");
            }
        }

        /// <summary>
        /// Gets a shipping method name from its id
        /// </summary>
        public string GetShippingMethods(int methodId)
        {
            if (!shippingMethodMap.ContainsKey(methodId))
            {
                return string.Format("Unknown ({0})", methodId);
            }

            return shippingMethodMap[methodId];
        }

        /// <summary>
        /// Loads the CSV data into a format ShipWorks will work with.
        /// </summary>
        [NDependIgnoreLongMethod]
        public void ImportCsv(string csvData)
        {
            try
            {
                StringBuilder xmlString = new StringBuilder(100);

                // create and write xml
                using (TextWriter textWriter = new StringWriter(xmlString))
                {
                    XmlTextWriter writer = new XmlTextWriter(textWriter);

                    writer.WriteStartDocument();
                    writer.WriteStartElement("ShippingMethods");

                    // Read the CSV data
                    using (StringReader reader = new StringReader(csvData))
                    {
                        using (CsvReader csvReader = new CsvReader(reader, true))
                        {
                            bool swFormat = 
                                !csvReader
                                .GetFieldHeaders()
                                .Contains("id_shippingmethods_flat", StringComparer.InvariantCultureIgnoreCase);

                            while (csvReader.ReadNextRecord())
                            {
                                string shippingMethodID = "";
                                string shippingMethodName = "";
                                if (swFormat)
                                {
                                    shippingMethodID = csvReader["shippingmethodid"];
                                    shippingMethodName = csvReader["shippingmethodname"];
                                }
                                else
                                {
                                    // support for user-generated export by exporting the shippingmethods_flat table in Volusion
                                    shippingMethodID = csvReader["id_shippingmethods_flat"];
                                    shippingMethodName = csvReader["name"];
                                }

                                writer.WriteStartElement("ShippingMethod");
                                writer.WriteAttributeString("id", shippingMethodID);
                                writer.WriteAttributeString("name", shippingMethodName);

                                writer.WriteEndElement();
                            }
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    writer.Close();
                }

                store.ShipmentMethods = xmlString.ToString();

                shippingMethodMap.Clear();
                LoadShippingMethods();
            }
            catch (ArgumentException ex)
            {
                throw new VolusionException("There was an error importing the shipping methods.\n\nError: " + ex.Message, ex);
            }
            catch (EndOfStreamException ex)
            {
                throw new VolusionException("There was an error importing the shipping methods.\n\nError: " + ex.Message, ex);
            }
            catch (MalformedCsvException ex)
            {
                throw new VolusionException("There was an error importing the shipping methods.\n\nError: " + ex.Message, ex);
            }
        }
    }
}
