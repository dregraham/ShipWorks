using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.IO.Text.Csv;
using System.IO;
using System.Xml;
using log4net;
using System.Xml.XPath;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// For working with the Payment Methods in the Volusion system
    /// </summary>
    public class VolusionPaymentMethods
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(VolusionPaymentMethods));

        VolusionStoreEntity store;

        // cache of loaded payment methods
        Dictionary<int, VolusionPaymentMethod> paymentMethodMap = new Dictionary<int, VolusionPaymentMethod>();

        /// <summary>
        /// Constructor
        /// </summary>
        public VolusionPaymentMethods(VolusionStoreEntity store)
        {
            this.store = store;

            LoadPaymentMethods();
        }

        /// <summary>
        /// Gets the number of payment methods in the cache
        /// </summary>
        public int Count
        {
            get { return paymentMethodMap.Count; }
        }

        /// <summary>
        /// Loads the payment mehod cache from the xml stored in the databaes
        /// </summary>
        private void LoadPaymentMethods()
        {
            try
            {
                string paymentMethodsXml = store.PaymentMethods;

                if (paymentMethodsXml.Length == 0)
                {
                    paymentMethodMap.Clear();
                    return;
                }

                // load the new values from the xml blob
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(paymentMethodsXml);

                // clear the current value
                paymentMethodMap.Clear();

                // load from the xml
                XPathNavigator navigator = doc.CreateNavigator();
                XPathNodeIterator iterator = navigator.Select("//PaymentMethod");
                while (iterator.MoveNext())
                {
                    int id = XPathUtility.Evaluate(iterator.Current, "@id", 0);
                    string type = XPathUtility.Evaluate(iterator.Current, "@type", "");
                    string name = XPathUtility.Evaluate(iterator.Current, "@name", "");

                    VolusionPaymentMethod method = new VolusionPaymentMethod
                    {
                        ID = id,
                        PaymentType = type,
                        Name = name
                    };

                    paymentMethodMap[id] = method;
                }
            }
            catch (XmlException)
            {
                // nothing, just treat it as invalid
                log.ErrorFormat("Invalid Volusion PaymentMethod XML in the database.");
            }
        }

        /// <summary>
        /// Ges the payment method information for a particular payment method id from volusion
        /// </summary>
        public VolusionPaymentMethod GetPaymentMethod(int id)
        {
            // if it doesn't exist, return null
            if (!paymentMethodMap.ContainsKey(id))
            {
                return null;
            }

            // get the payment method
            return paymentMethodMap[id];
        }

        /// <summary>
        /// Loads the CSV data into a format ShipWorks will work with.
        /// </summary>
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
                    writer.WriteStartElement("PaymentMethods");

                    // Read the CSV data
                    using (StringReader reader = new StringReader(csvData))
                    {
                        using (CsvReader csvReader = new CsvReader(reader, true))
                        {
                            while (csvReader.ReadNextRecord())
                            {
                                string paymentMethodID = csvReader["paymentmethodid"];
                                string paymentType = csvReader["paymentmethodtype"];
                                string paymentMethod = csvReader["paymentmethod"];

                                writer.WriteStartElement("PaymentMethod");
                                writer.WriteAttributeString("id", paymentMethodID);
                                writer.WriteAttributeString("type", paymentType);
                                writer.WriteAttributeString("name", paymentMethod);
                                writer.WriteEndElement();
                            }
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                    writer.Close();
                }

                store.PaymentMethods = xmlString.ToString();
            }
            catch (ArgumentException ex)
            {
                throw new VolusionException("There was an error importing the payment methods.\n\nError: " + ex.Message, ex);
            }
            catch (EndOfStreamException ex)
            {
                throw new VolusionException("There was an error importing the payment methods.\n\nError: " + ex.Message, ex);
            }
            catch (MalformedCsvException ex)
            {
                throw new VolusionException("There was an error importing the payment methods.\n\nError: " + ex.Message, ex);
            }
        }
    }
}
