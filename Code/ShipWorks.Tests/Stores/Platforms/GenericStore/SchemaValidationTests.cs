using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using ShipWorks.Data.Import.Xml.Schema;
using ShipWorks.Stores.Platforms.GenericModule;
using Xunit;

namespace ShipWorks.Tests.Stores.GenericStore
{
    /// <summary>
    /// Test cases to ensure the ShipWorks Module Schema is validating.
    /// </summary>
    public class SchemaValidationTests
    {
        /// <summary>
        /// Ensures the ShipWorks.xsd is a well-formed schema
        /// </summary>
        [Fact]
        public void IsValidSchema()
        {
            using (Stream stream = Assembly.GetAssembly(typeof(GenericStoreWebClient)).GetManifestResourceStream(@"ShipWorks.Data.Import.Xml.Schema.ShipWorksModule.xsd"))
            {
                Assert.NotNull(stream);

                XmlSchema.Read(stream, (o, e) =>
                    {
                        // want to know about Errors and Warnings
                        Assert.False(true, e.Message);
                    });
            }
        }

        /// <summary>
        /// Runs validation tests against the xml file with the given resourceName
        /// </summary>
        private void ValidateResource(string resourceName, bool shouldValidate)
        {
            // load the sample xml instance
            using (Stream xmlStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                XmlDocument instanceDocument = new XmlDocument();
                instanceDocument.Load(xmlStream);

                List<string> errors = ShipWorksSchemaValidator.FindValidationErrors(instanceDocument, ShipWorksSchema.Module);

                // Raise failure if needed
                if (!((errors.Count > 0) ^ shouldValidate))
                {
                    Assert.False(true, string.Format("'{0}' {1} have validated but it {2}. {3}",
                                    resourceName,
                                    shouldValidate ? "should" : "should not",
                                    shouldValidate ? "didn't" : "did",
                                    shouldValidate ? "Validation Errors: " + String.Join(",", errors) : ""));
                }
            }
        }

        /// <summary>
        /// Tests the validity of the collection of sample xml files
        /// </summary>
        [Fact]
        public void TestSampleXml()
        {
            Dictionary<string, bool> fileMap = new Dictionary<string, bool>();
            foreach (string resource in GetSampleResources())
            {
                string resourceString = "ShipWorks.Tests.Stores.Platforms.GenericStore.SampleXML." + resource;
                bool shouldValidate = Regex.Match(resource, @"_pass\.xml", RegexOptions.IgnoreCase).Success;

                fileMap[resourceString] = shouldValidate;
            }

            foreach (string key in fileMap.Keys)
            {
                string resource = key;
                bool shouldValidate = fileMap[resource];

                ValidateResource(resource, shouldValidate);
            }
        }

        /// <summary>
        /// Gets the collection of sample test cases
        /// </summary>
        private List<string> GetSampleResources()
        {
            return new List<string> {
                "GetOrders_fail.xml",
                "GetOrdersNone_pass.xml",
                "GetCount_pass.xml",
                "GetCount_fail.xml",
                "GetStore_pass.xml",
                "UpdateStatusSuccess_pass.xml",
                "UpdateStatusFail_pass.xml",
                "UpdateStatusFail_fail.xml",
                "GetStatusCodes_pass.xml",
                "GetStatusCodes_fail.xml",
                "ModuleVersion_fail.xml",
                "ModuleVersion2_fail.xml"
            };
        }

    }
}
