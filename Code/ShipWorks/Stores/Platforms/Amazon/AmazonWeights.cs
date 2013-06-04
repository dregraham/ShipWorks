using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Collection of weight fields Amazon provides
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = false, StripAfterObfuscation = false)]
    public enum AmazonWeightField
    {
        PackagingWeight = 0,
        ItemWeight = 1,
        TotalMetalWeight = 2,
        TotalGemWeight = 3,
        TotalDiamondWeight = 4
    }

    /// <summary>
    /// Class for working with the available Amazon weight fields.
    /// </summary>
    public static class AmazonWeights
    {
        /// <summary>
        /// Gets the user-visible string for an Amazon Weight field
        /// </summary>
        public static string GetWeightFieldDisplay(AmazonWeightField field)
        {
            switch (field)
            {
                case AmazonWeightField.ItemWeight:
                    return "Item Weight";
                case AmazonWeightField.TotalMetalWeight:
                    return "Metal Weight";
                case AmazonWeightField.PackagingWeight:
                    return "Packaging Weight";
                case AmazonWeightField.TotalGemWeight:
                    return "Gem Weight";
                case AmazonWeightField.TotalDiamondWeight:
                    return "Diamond Weight";
            }

            return "Unknown";
        }

        /// <summary>
        /// Constructs the internal xml storage format from a list of weights.
        /// </summary>
        private static string GetWeightsAsXml(List<AmazonWeightField> weights)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(writer);
                xmlWriter.WriteStartDocument(true);

                // open tag
                xmlWriter.WriteStartElement("Fields");

                // write all weights sequentially
                weights.ForEach(w => xmlWriter.WriteElementString("Field", ((int)w).ToString()));

                // close tag
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();

                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserializes the internal weight priority definition xml into a collection
        /// of AmazonWeightFields representing the user-selected priority
        /// </summary>
        private static List<AmazonWeightField> GetWeightsFromXml(string weightsXml)
        {
            List<AmazonWeightField> weights = new List<AmazonWeightField>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(weightsXml);

                foreach (XmlNode node in doc.SelectNodes("//Field"))
                {
                    weights.Add((AmazonWeightField)int.Parse(node.InnerText));
                }

                return weights;
            }
            catch (XmlException)
            {
                // invalid xml provided, just return an empty collection
                return new List<AmazonWeightField>();
            }
        }

        /// <summary>
        /// Assigns weight fields to be searched when orders are downloaded in Amazon.  This is an ordered list.
        /// </summary>
        public static void SetWeightsPriority(AmazonStoreEntity store, List<AmazonWeightField> weightSearch)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            if (weightSearch == null)
            {
                throw new ArgumentNullException("weightSearch");
            }

            store.WeightDownloads = GetWeightsAsXml(weightSearch);
        }

        /// <summary>
        /// Returns the user-defined weight download priority used during Amazon 
        /// order downloads.
        /// </summary>
        public static List<AmazonWeightField> GetWeightsPriority(AmazonStoreEntity store)
        {
            if (store == null)
            {
                throw new ArgumentNullException("store");
            }

            return GetWeightsFromXml(store.WeightDownloads);
        }
    }
}
