using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Xml;
using System.Xml.XPath;
using Interapptive.Shared.Utility;
using System.IO;

namespace ShipWorks.Stores
{
    /// <summary>
    /// Base class for dealing with status code > value mappings
    /// </summary>
    public abstract class StatusCodeProvider<T>
    {
        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(StatusCodeProvider<T>));

        // Code -> Value map
        Dictionary<T, string> codeMap = null;

        /// <summary>
        /// Constructor
        /// </summary>
        protected StatusCodeProvider()
        {

        }

        /// <summary>
        /// Get the the XML representing the status codes that has already been persisted\saved locally 
        /// </summary>
        protected abstract string GetLocalStatusCodesXml();

        /// <summary>
        /// Retrieves the name of a status code
        /// </summary>
        public string this[T code]
        {
            get { return GetCodeName(code); }
        }

        /// <summary>
        /// Get the display text for the code
        /// </summary>
        public string GetCodeName(T code)
        {
            string name;
            if (CodeMap.TryGetValue(ConvertCodeValue(code), out name))
            {
                return name;
            }

            log.WarnFormat("Code [{0}] was not found in code map", code);
            return code.ToString();
        }

        /// <summary>
        /// The list of all code names
        /// </summary>
        public ICollection<string> CodeNames
        {
            get { return CodeMap.Values; }
        }

        /// <summary>
        /// The list of all code values
        /// </summary>
        public ICollection<T> CodeValues
        {
            get { return CodeMap.Keys.ToList(); }
        }

        /// <summary>
        /// Get the code value for the given name
        /// </summary>
        public virtual T GetCodeValue(string codeName)
        {
            foreach (var entry in CodeMap)
            {
                if (entry.Value == codeName)
                {
                    return entry.Key;
                }
            }

            throw new InvalidOperationException(string.Format("No code exists for code name '{0}'", codeName));
        }

        /// <summary>
        /// Gives derived classes a chance to convert the given value to a different type (still compatible with T)
        /// </summary>
        public virtual T ConvertCodeValue(T value)
        {
            return value;
        }

        /// <summary>
        /// Loads the status codes on demand from the database.  Needed to be done this way
        /// because when it was loaded in the constructor, a call chain would end up calling a 
        /// virtual function.
        /// </summary>
        protected Dictionary<T, string> CodeMap
        {
            get
            {
                if (codeMap == null)
                {
                    // Note - this uses the setter, not the direct field, so that the conversion stuff runs
                    CodeMap = LoadLocalStatusCodeMap();
                }

                return codeMap;
            }
            set
            {
                codeMap = new Dictionary<T, string>();

                // Ensure each value is typed appropriately
                foreach (var pair in value)
                {
                    codeMap[ConvertCodeValue(pair.Key)] = pair.Value;
                }
            }
        }

        /// <summary>
        /// Load the status code map from the XML that is already known and saved\cached locally
        /// </summary>
        protected virtual Dictionary<T, string> LoadLocalStatusCodeMap()
        {
            string codeXml = GetLocalStatusCodesXml();

            if (string.IsNullOrEmpty(codeXml))
            {
                codeMap = new Dictionary<T, string>();
            }
            else
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(codeXml);

                XPathNavigator xpath = xmlDocument.CreateNavigator();

                // Load the map
                codeMap = DeserializeCodeMapFromXml(xpath);
            }

            return codeMap;
        }

        /// <summary>
        /// Load the map of codes from the given navigator.
        /// </summary>
        protected Dictionary<T, string> DeserializeCodeMapFromXml(XPathNavigator xpath)
        {
            Dictionary<T, string> localMap = new Dictionary<T, string>();

            XPathNodeIterator nodes = xpath.Select("//StatusCode");
            while (nodes.MoveNext())
            {
                ReadStatusCodeXml(localMap, nodes.Current);
            }

            return localMap;
        }

        /// <summary>
        /// Read a serialized status code from the navigator and populates the codeMap with the new entry.
        /// Virtual so derived classes can read custom data
        /// </summary>
        private void ReadStatusCodeXml(Dictionary<T, string> localMap, XPathNavigator xpath)
        {
            T code = (T) Convert.ChangeType(XPathUtility.Evaluate(xpath, "Code", ""), typeof(T));
            string name = XPathUtility.Evaluate(xpath, "Name", "");

            code = ConvertCodeValue(code);

            if (IsValidCode(code))
            {
                localMap[code] = name;

                ReadExtendedStatusCodeXml(code, xpath);
            }
        }

        /// <summary>
        /// Tests whether the specified code is valid
        /// </summary>
        public virtual bool IsValidCode(T code)
        {
            return true;
        }

        /// <summary>
        /// Read custom data related to a status code from the xml blob in the database
        /// </summary>
        protected virtual void ReadExtendedStatusCodeXml(T code, XPathNavigator xpath)
        {
            // most implementations do not need to read extra data, but this is the extension point
        }

        /// <summary>
        /// Convert a status code map to xml for storage in the db
        /// </summary>
        protected string SerializeCodeMapToXml(Dictionary<T, string> localMap)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.WriteStartElement("StatusCodes");

                    // write all of the code values out
                    foreach (KeyValuePair<T, string> pair in localMap)
                    {
                        xmlWriter.WriteStartElement("StatusCode");

                        WriteStatusCodeXml(pair, xmlWriter);

                        xmlWriter.WriteEndElement();
                    }

                    xmlWriter.WriteEndElement();
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Writes an invdividual status code as Xml.  If necessary, derived class could output more data
        /// </summary>
        private void WriteStatusCodeXml(KeyValuePair<T, string> pair, XmlTextWriter xmlWriter)
        {
            xmlWriter.WriteElementString("Code", pair.Key.ToString());
            xmlWriter.WriteElementString("Name", pair.Value);

            WriteExtendedStatusCodeXml(pair, xmlWriter);
        }

        /// <summary>
        /// Write custom data related to a status code to be saved to the database
        /// </summary>
        protected virtual void WriteExtendedStatusCodeXml(KeyValuePair<T, string> pair, XmlTextWriter xmlWriter)
        {
            // most implementations do not need to write extra data, but this is the extension point
        }
    }
}
