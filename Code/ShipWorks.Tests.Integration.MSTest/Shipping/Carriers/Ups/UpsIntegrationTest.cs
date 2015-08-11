using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Ups
{
    public class UpsIntegrationTest : DataDrivenIntegrationTestBase
    {
        [DataSource("DataSource_Ship_UPSRecert")]
        [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        [Fact]
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
        [Fact]
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