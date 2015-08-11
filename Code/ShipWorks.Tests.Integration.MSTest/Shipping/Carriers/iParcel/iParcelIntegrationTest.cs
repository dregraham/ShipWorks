﻿using System;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.iParcel
{
    public class iParcelIntegrationTest : DataDrivenIntegrationTestBase
    {
        [DataSource("DataSource_Ship_iParcel")]
        [DeploymentItem("DataSources\\iParcel.xlsx")]
        [Fact]
        [Ignore]
        public void Ship_iParcel()
        {
            iParcelFixture testObject = new iParcelFixture();

            if (PopulateTestObject(testObject, null))
            {
                Console.WriteLine(string.Format("Executing test ID {0}", this.TestContext.DataRow["TestID"]));

                testObject.Ship();
            }
        }
    }
}