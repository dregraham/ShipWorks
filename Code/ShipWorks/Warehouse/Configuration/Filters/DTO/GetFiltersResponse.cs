using System;
using System.IO;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Warehouse.Configuration.Filters.DTO
{
    /// <summary>
    /// The response from a GetAllFilters call
    /// </summary>
    public class GetFiltersResponse
    {
        /// <summary>
        /// The ID for this filter in the Hub
        /// </summary>
        [JsonProperty("SK")]
        public string SK { get; set; }

        /// <summary>
        /// The Hub ID as a GUID
        /// </summary>
        public Guid HubFilterID
        {
            get => Guid.Parse(SK);
        }

        /// <summary>
        /// The HubFilterID of this filter's parent
        /// </summary>
        [JsonProperty("ParentFilterId")]
        public string ParentFilterString { get; set; }

        /// <summary>
        /// The parent ID as a GUID
        /// </summary>
        public Guid ParentFilterID
        {
            get
            {
                Guid result;
                Guid.TryParse(ParentFilterString, out result);
                return result;
            }
        }

        /// <summary>
        /// The name of the filter
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// The path of this filter in the filter tree
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The filter's definition
        /// </summary>
        [JsonProperty("definition")]
        public JToken DefinitionJson { get; set; }

        [JsonIgnore]
        public string DefinitionXML
        {
            get
            {
                using (var stringWriter = new StringWriter())
                {
                    using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                    {
                        xmlWriter.WriteStartElement("FilterDefinition");
                        xmlWriter.WriteAttributeString("Target", "0");

                        WriteGroupContainer(xmlWriter, DefinitionJson["rootContainer"]);

                        xmlWriter.WriteEndElement();
                    }
                    return stringWriter.ToString();
                }

            }

            set
            {

            }
        }

        public void WriteGroupContainer(XmlWriter writer, JToken groupContainer)
        {
            writer.WriteStartElement("JoinType");
            writer.WriteAttributeString("value", groupContainer["joinType"].Value<string>());
            writer.WriteEndElement();

            writer.WriteStartElement("FirstGroup");
            writer.WriteAttributeString("identifier", "Group");
            {
                writer.WriteStartElement("JoinType");
                writer.WriteAttributeString("value", groupContainer["firstGroup"]["joinType"].Value<string>());
                writer.WriteEndElement();

                writer.WriteStartElement("Conditions");
                {
                    foreach (var condition in groupContainer["firstGroup"]["conditions"].Children<JObject>())
                    {
                        writer.WriteStartElement("Item");
                        writer.WriteAttributeString("identifier", condition["identifier"].ToString().Replace(".Person.", "."));

                        foreach (var key in condition.Properties())
                        {
                            if (!(key.Name.Equals("path", StringComparison.OrdinalIgnoreCase) || key.Name.Equals("identifier", StringComparison.OrdinalIgnoreCase)))
                            {
                                writer.WriteStartElement(char.ToUpper(key.Name[0]) + key.Name.Substring(1));
                                writer.WriteAttributeString("value", key.Value.ToString());
                                writer.WriteEndElement();
                            }
                        }

                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("SecondGroup");
            if (groupContainer["secondGroup"]?.HasValues ?? false)
            {
                WriteGroupContainer(writer, groupContainer["secondGroup"]);
            }
            writer.WriteEndElement();
        }
    }
}
