using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Reflection;
using System.IO;
using log4net;

namespace ShipWorks.Data.Import.Xml.Schema
{
    /// <summary>
    /// Validates XML input against the XML Schemas
    /// </summary>
    public static class ShipWorksSchemaValidator
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ShipWorksSchemaValidator));

        // cached instances of our Schemas
        private static Dictionary<string, XmlSchema> schemas = new Dictionary<string, XmlSchema>();

        /// <summary>
        /// Validates the documentToValidate against one of the ShipWorks schemas
        /// </summary>
        public static List<string> FindValidationErrors(XmlDocument documentToValidate, ShipWorksSchema schemaToValidateWith)
        {
            XmlSchema coreSchema = GetSchema("ShipWorksCore");
            XmlSchema inputSchema = GetSchema( schemaToValidateWith == ShipWorksSchema.FileImport ? "ShipWorksFileImport" : "ShipWorksModule");

            // set resolver to null to avoid looking on disk/elsewhere for schema include/imports
            documentToValidate.Schemas.XmlResolver = null;

            // add our schemas
            documentToValidate.Schemas.Add(coreSchema);
            documentToValidate.Schemas.Add(inputSchema);

            List<string> validationErrors = new List<string>();
            documentToValidate.Validate((o, e) =>
            {
                if (e.Severity == XmlSeverityType.Error)
                {
                    log.ErrorFormat("ShipWorks Module XML response error: {0}", e.Message);
                    validationErrors.Add(e.Message);
                }
                else
                {
                    log.WarnFormat("ShipWorks Module XML response warning: {0}", e.Message);
                }
            }, documentToValidate.DocumentElement);

            return validationErrors;
        }

        /// <summary>
        /// Loads a schema with the given name
        /// </summary>
        private static XmlSchema GetSchema(string schemaName)
        {
            lock (schemas)
            {
                if (!schemas.ContainsKey(schemaName))
                {
                    // First Read the core Schema
                    using (Stream stream = Assembly.GetAssembly(typeof(ShipWorksSchemaValidator)).GetManifestResourceStream(String.Format(@"ShipWorks.Data.Import.Xml.Schema.{0}.xsd", schemaName)))
                    {
                        schemas.Add(schemaName, XmlSchema.Read(stream, null));
                    }
                }

                return schemas[schemaName];
            }
        }
    }
}
