using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Diagnostics;
using Interapptive.Shared;

namespace ShipWorks.Data.Import.Spreadsheet
{
    /// <summary>
    /// Fully defines the settings and mappings for a CSV\Text file import
    /// </summary>
    public abstract class GenericSpreadsheetMap
    {
        GenericSpreadsheetMapDateSettings dateSettings;

        GenericSpreadsheetTargetSchema targetSchema;
        GenericSpreadsheetTargetSchemaSettings targetSettings;
        GenericSpreadsheetSourceSchema sourceSchema;

        GenericSpreadsheetFieldMappingCollection mappings = new GenericSpreadsheetFieldMappingCollection();

        /// <summary>
        /// De-serialization constructor
        /// </summary>
        protected GenericSpreadsheetMap(GenericSpreadsheetTargetSchema targetSchema, string mapXml)
        {
            this.targetSchema = targetSchema;

            LoadFromXml(mapXml);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        protected GenericSpreadsheetMap(GenericSpreadsheetTargetSchema targetSchema)
        {
            this.targetSchema = targetSchema;
            this.targetSettings = targetSchema.CreateSettings();
            this.dateSettings = new GenericSpreadsheetMapDateSettings();

            ValidateTargetSchema(targetSchema);
        }

        /// <summary>
        /// Copy constructor for use with Clone
        /// </summary>
        protected GenericSpreadsheetMap(GenericSpreadsheetMap copy)
            : this(copy.targetSchema, copy.SerializeToXml())
        {

        }

        /// <summary>
        /// Create a clone of the map
        /// </summary>
        public abstract GenericSpreadsheetMap Clone();

        /// <summary>
        /// The name of the map\definition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The target schema that the map must conform to
        /// </summary>
        public GenericSpreadsheetTargetSchema TargetSchema
        {
            get { return targetSchema; }
        }

        /// <summary>
        /// The schema-specific settings of the map as controlled by custom UI
        /// </summary>
        public GenericSpreadsheetTargetSchemaSettings TargetSettings
        {
            get { return targetSettings; }
        }

        /// <summary>
        /// The source schema of the input document
        /// </summary>
        public GenericSpreadsheetSourceSchema SourceSchema
        {
            get { return sourceSchema ?? (sourceSchema = CreateSourceSchema()); }
            set { sourceSchema = value; }
        }

        /// <summary>
        /// Gets the current date settings for the map
        /// </summary>
        public GenericSpreadsheetMapDateSettings DateSettings
        {
            get { return dateSettings; }
        }

        /// <summary>
        /// Gets the current collection of mappings
        /// </summary>
        public GenericSpreadsheetFieldMappingCollection Mappings
        {
            get { return mappings; }
        }

        /// <summary>
        /// Must be implemented by derived classes to create new SourceSchema instances
        /// </summary>
        protected abstract GenericSpreadsheetSourceSchema CreateSourceSchema();

        /// <summary>
        /// Validate the schema
        /// </summary>
        private void ValidateTargetSchema(GenericSpreadsheetTargetSchema schema)
        {
            HashSet<string> identifiers = new HashSet<string>();

            foreach (GenericSpreadsheetTargetField field in schema.FieldGroups.SelectMany(g => g.GetFields(targetSettings)))
            {
                if (identifiers.Contains(field.Identifier))
                {
                    throw new InvalidOperationException(string.Format("Cannot add field '{0}' because identifier '{1}' already exists for another field.", field.DisplayName, field.Identifier));
                }

                identifiers.Add(field.Identifier.ToString());
            }
        }

        /// <summary>
        /// Persist the map to a string format that can be read later
        /// </summary>
        public string SerializeToXml()
        {
            ValidateTargetSchema(targetSchema);

            // Prepare the source element
            XElement xSource = new XElement("Source");
            sourceSchema.SaveTo(xSource);

            // Prepare the settings element
            XElement xSettings = new XElement("Settings");
            targetSettings.SaveTo(xSettings);

            XElement root = new XElement("ShipWorksMap",
                new XAttribute("version", "1.1"),
                new XElement("Name", Name),

                new XElement("Dates",
                    new XElement("DateTimeFormat", dateSettings.DateTimeFormat),
                    new XElement("DateFormat", dateSettings.DateFormat),
                    new XElement("TimeFormat", dateSettings.TimeFormat),
                    new XElement("TimeZone", (int) dateSettings.TimeZoneAssumption)),

                new XElement("Schema",
                    new XAttribute("type", targetSchema.SchemaType),
                    new XAttribute("version", targetSchema.SchemaVersion)),

                xSource,

                new XElement("Mappings",
                    mappings.Select(mapping =>
                        new XElement("Mapping",
                            new XElement("Target", mapping.TargetField.Identifier),
                            new XElement("Source", mapping.SourceColumnName)))),

                xSettings

                );

            return root.ToString();
        }

        /// <summary>
        /// Load the map from the given persisted XML data
        /// </summary>
        [NDependIgnoreLongMethod]
        public void LoadFromXml(string mapXml)
        {
            try
            {
                XElement root = XElement.Parse(mapXml);

                if (root.Name != "ShipWorksMap")
                {
                    throw new GenericSpreadsheetException("The file is not a valid ShipWorks import map.");
                }

                string schemaType = (string) root.XPathEvaluate("string(Schema/@type)");
                if (schemaType != targetSchema.SchemaType)
                {
                    throw new GenericSpreadsheetException("The map is a valid ShipWorks map, but it is not the correct map type.");
                }

                // Name
                Name = (string) root.Element("Name");

                // Dates
                dateSettings = new GenericSpreadsheetMapDateSettings();
                XElement xDates = root.Element("Dates");
                if (xDates != null)
                {
                    dateSettings.DateTimeFormat = (string) xDates.Element("DateTimeFormat") ?? "Automatic";
                    dateSettings.DateFormat = (string) xDates.Element("DateFormat") ?? "Automatic";
                    dateSettings.TimeFormat = (string) xDates.Element("TimeFormat") ?? "Automatic";
                    dateSettings.TimeZoneAssumption = (GenericSpreadsheetTimeZoneAssumption) ((int?) xDates.Element("TimeZone") ?? 0);
                }

                // Source
                sourceSchema = CreateSourceSchema();
                sourceSchema.LoadFrom(root.Element("Source"));

                // Settings
                targetSettings = targetSchema.CreateSettings();
                targetSettings.LoadFrom(root.Element("Settings"));

                // Create a new collection of mappings to be loaded
                mappings = new GenericSpreadsheetFieldMappingCollection();

                // Mappings
                foreach (XElement xMapping in root.XPathSelectElements("Mappings/Mapping"))
                {
                    string target = (string) xMapping.Element("Target");
                    string source = (string) xMapping.Element("Source");

                    GenericSpreadsheetTargetField targetField = targetSchema.FieldGroups.SelectMany(g => g.GetFields(targetSettings)).FirstOrDefault(f => f.Identifier == target);
                    if (targetField != null)
                    {
                        mappings.SetMapping(new GenericSpreadsheetFieldMapping(targetField, source));
                    }
                }

                ValidateTargetSchema(targetSchema);
            }
            catch (XmlException ex)
            {
                throw new GenericSpreadsheetException(ex.Message, ex);
            }
        }
    }
}
