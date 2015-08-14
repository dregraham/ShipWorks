using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xunit;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1.Net;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing.Rating;
using System.Data;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Usps
{
    public class UspsTest : DataDrivenIntegrationTestBase
    {
        private const bool justLabels = true;

        //// [DataSource("DataSource_Ship_Stamps")]
        //// [DeploymentItem("DataSources\\Usps.xlsx")]
        [Trait("Category", "USPS")]
        [Theory]
        public void ProcessBatch_UspsAndExpress1Usps_Test(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            try
            {
                PostalWebUtility.UseTestServer = true;
                UspsWebClient.UseTestServer = true;
                Express1UspsWebClient.UseTestServer = true;

                if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    UspsResellerType resellerType;

                    if (testObject.ShipmentType == "15")
                    {
                        resellerType = UspsResellerType.None;
                    }
                    else
                    {
                        resellerType = UspsResellerType.Express1;
                    }

                    Exception exception = null;

                    // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        try
                        {
                            testObject.Ship(resellerType);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                    }

                    if (exception != null)
                    {
                        throw exception;
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                Console.WriteLine(string.Format("Error running Test ID {0}.  Error message: {1}", row["TestID"], ex.Message));

                // We have a legitimate exception
                throw;
            }
        }

        // [DataSource("DataSource_Ship_Stamps")]
        // [DeploymentItem("DataSources\\Usps.xlsx")]
        [Trait("Category", "USPS")]
        [Theory]
        public void GetRates_UspsAndExpress1Usps_Test(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            try
            {
                PostalWebUtility.UseTestServer = true;
                UspsWebClient.UseTestServer = true;
                Express1UspsWebClient.UseTestServer = true;

                if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    UspsResellerType resellerType;

                    if (testObject.ShipmentType == "15")
                    {
                        resellerType = UspsResellerType.None;
                    }
                    else
                    {
                        resellerType = UspsResellerType.Express1;
                    }

                    List<RateResult> rateResults = testObject.GetRates(resellerType);

                    Debug.Assert(rateResults != null);
                    Debug.Assert(rateResults.Any());
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                Console.WriteLine(string.Format("Error running Test ID {0}.  Error message: {1}", row["TestID"], ex.Message));

                // We have a legitimate exception
                throw;
            }
        }

        // [DataSource("DataSource_Ship_PurchasePostage")]
        // [DeploymentItem("DataSources\\Usps.xlsx")]
        [Trait("Category", "USPS")]
        [Theory]
        public void PurchasePostage_UspsAndExpress1Usps_Test(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            try
            {
                PostalWebUtility.UseTestServer = true;
                UspsWebClient.UseTestServer = true;
                Express1UspsWebClient.UseTestServer = true;

                if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    Exception exception = null;

                    // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        try
                        {
                            testObject.PurchasePostage(25M);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                    }

                    if (exception != null)
                    {
                        throw exception;
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                Console.WriteLine(string.Format("Error running Test ID {0}.  Error message: {1}", row["TestID"], ex.Message));

                // We have a legitimate exception
                throw;
            }
        }

        // [DataSource("DataSource_Ship_PurchasePostage")]
        // [DeploymentItem("DataSources\\Usps.xlsx")]
        [Trait("Category", "USPS")]
        [Theory]
        public void CheckPostage_UspsAndExpress1Usps_Test(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            try
            {
                PostalWebUtility.UseTestServer = true;
                UspsWebClient.UseTestServer = true;
                Express1UspsWebClient.UseTestServer = true;

                if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                    (testObject.IsSaveLabel || !justLabels))
                {
                    Console.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                    Console.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                    Console.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                    Exception exception = null;

                    // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                    using (SqlAdapter adapter = new SqlAdapter(true))
                    {
                        try
                        {
                            decimal balance = -1;
                            balance = testObject.CheckPostage();

                            Debug.Assert(balance != -1);
                        }
                        catch (Exception ex)
                        {
                            exception = ex;
                        }
                    }

                    if (exception != null)
                    {
                        throw exception;
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrWhiteSpace(row[0].ToString().Trim()))
                {
                    // The test framework doesn't seem to know when to stop...so if we don't have a SaveLabel populated, return with no error. 
                    return;
                }

                Console.WriteLine(string.Format("Error running Test ID {0}.  Error message: {1}", row["TestID"], ex.Message));

                // We have a legitimate exception
                throw;
            }
        }
    }
}