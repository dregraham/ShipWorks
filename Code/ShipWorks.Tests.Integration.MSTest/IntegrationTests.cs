using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.Ups;
using ShipWorks.Tests.Integration.MSTest.Fixtures.Shipping.Carriers.iParcel;

namespace ShipWorks.Tests.Integration.MSTest
{
    [TestClass]
    public class IntegrationTests : IntegrationTestBase
    {
        [DataSource("DataSource_Ship_iParcel")]
        [DeploymentItem("DataSources\\iParcel.xlsx")]
        [TestMethod]
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

        [DataSource("DataSource_Ship_UPSRecert")]
        [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_UPSRecert()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsFixture testObject = new UpsFixture();

            if (PopulateTestObject(testObject, null))
            {
                testObject.Ship();
            }
        }

        [DataSource("DataSource_Ship_UPSRecert_MI")]
        [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        [TestMethod]
        [Ignore]
        public void Ship_UPSRecert_MI()
        {
            System.Diagnostics.Debug.WriteLine(this.TestContext.DataRow["TestID"].ToString());
            UpsMIFixture testObject = new UpsMIFixture();

            if (PopulateTestObject(testObject, null))
            {
                testObject.Ship();
            }
        }
    }
}