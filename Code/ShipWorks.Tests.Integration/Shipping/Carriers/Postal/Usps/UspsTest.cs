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
using Xunit.Abstractions;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Postal.Usps
{
    public class UspsTest : DataDrivenIntegrationTestBase
    {
        private const bool justLabels = true;
        private readonly ITestOutputHelper output;

        public UspsTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        // [DataSource("DataSource_Ship_Stamps")]
        [ExcelData("DataSources\\Usps.xlsx", "Stamps")]
        [Trait("Category", "USPS")]
        [Theory]
        public void ProcessBatch_UspsAndExpress1Usps(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            PostalWebUtility.UseTestServer = true;
            UspsWebClient.UseTestServer = true;
            Express1UspsWebClient.UseTestServer = true;

            if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                output.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                UspsResellerType resellerType;

                if (testObject.ShipmentType == "15")
                {
                    resellerType = UspsResellerType.None;
                }
                else
                {
                    resellerType = UspsResellerType.Express1;
                }

                // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    testObject.Ship(resellerType);
                }
            }
        }

        [ExcelData("DataSources\\Usps.xlsx", "Stamps")]
        [Trait("Category", "USPS")]
        [Theory]
        public void GetRates_UspsAndExpress1Usps(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            PostalWebUtility.UseTestServer = true;
            UspsWebClient.UseTestServer = true;
            Express1UspsWebClient.UseTestServer = true;

            if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                output.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

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

                Assert.NotNull(rateResults);
                Assert.True(rateResults.Any());
            }
        }

        [ExcelData("DataSources\\Usps.xlsx", "PurchasePostage")]
        [Trait("Category", "USPS")]
        [Theory]
        public void PurchasePostage_UspsAndExpress1Usps(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            PostalWebUtility.UseTestServer = true;
            UspsWebClient.UseTestServer = true;
            Express1UspsWebClient.UseTestServer = true;

            if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                output.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);

                // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    testObject.PurchasePostage(25M);
                }
            }
        }

        [ExcelData("DataSources\\Usps.xlsx", "PurchasePostage")]
        [Trait("Category", "USPS")]
        [Theory]
        public void CheckPostage_UspsAndExpress1Usps(DataRow row)
        {
            UspsMapping testObject = new UspsMapping();

            PostalWebUtility.UseTestServer = true;
            UspsWebClient.UseTestServer = true;
            Express1UspsWebClient.UseTestServer = true;

            if (PopulateTestObject(row, testObject, UspsMapping.Mapping) &&
                (testObject.IsSaveLabel || !justLabels))
            {
                output.WriteLine(@"{0}{0}--------------------------------------------------------------------------------", Environment.NewLine);
                output.WriteLine(string.Format("Executing Test ID {0}", row["TestID"]));
                output.WriteLine(@"--------------------------------------------------------------------------------{0}{0}", Environment.NewLine);
                
                // Yes, this looks stupid, but we get a debug assert if we aren't in a transaction...
                using (SqlAdapter adapter = new SqlAdapter(true))
                {
                    decimal balance = -1;
                    balance = testObject.CheckPostage();

                    Assert.NotEqual(-1, balance);
                }
            }
        }
    }
}