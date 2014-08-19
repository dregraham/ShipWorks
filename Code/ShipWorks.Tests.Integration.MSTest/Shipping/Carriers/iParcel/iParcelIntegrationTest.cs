using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.iParcel
{
    [TestClass]
    public class iParcelIntegrationTest : DataDrivenIntegrationTestBase
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
    }
}