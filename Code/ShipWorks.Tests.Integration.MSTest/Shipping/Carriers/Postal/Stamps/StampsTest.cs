using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Shipping.Carriers.Postal.Stamps.Api;
using ShipWorks.Shipping.Carriers.Postal.WebTools;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Stamps
{
    [TestClass]
    public class StampsTest : DataDrivenIntegrationTestBase
    {
        private const bool justLabels = true;

        [DataSource("DataSource_Ship_Stamps")]
        [DeploymentItem("DataSources\\Stamps.xlsx")]
        [TestCategory("Stamps")]
        [TestMethod]
        public void ProcessBatch_USPS_Test()
        {
            StampsMapping testObject = new StampsMapping();

            try
            {
                PostalWebUtility.UseTestServer = true;
                StampsWebClient.UseTestServer = true;
                Express1StampsWebClient.UseTestServer = true;

                if (PopulateTestObject(testObject, StampsMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing Test ID {0}", TestContext.DataRow["TestID"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    StampsResellerType stampsResellerType;

                    if (testObject.ShipmentType == "15")
                    {
                        stampsResellerType = StampsResellerType.StampsExpedited;
                    }
                    else
                    {
                        stampsResellerType = StampsResellerType.Express1;  
                    }

                    Exception exception = null;

                    // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        try
                        {
                            testObject.Ship(stampsResellerType);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }

                        adapter.Commit();
                    }

                    if (exception != null)
                    {
                        throw exception;
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(TestContext.DataRow[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                Console.WriteLine(string.Format("Error running Test ID {0}.  Error message: {1}", TestContext.DataRow["TestID"], ex.Message));

                // We have a legitimate exception
                throw;
            }
        }

    }
}