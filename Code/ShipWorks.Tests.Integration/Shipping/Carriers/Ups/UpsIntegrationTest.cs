using System.Data;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.Ups
{
    public class UpsIntegrationTest : DataDrivenIntegrationTestBase
    {
        // [DataSource("DataSource_Ship_UPSRecert")]
        // [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        
        public void Ship_UPSRecert(DataRow row)
        {
            System.Diagnostics.Debug.WriteLine(row["TestID"].ToString());
            UpsFixture testObject = new UpsFixture();

            if (PopulateTestObject(row, testObject, null))
            {
                testObject.Ship();
            }
        }

        // [DataSource("DataSource_Ship_UPSRecert_MI")]
        // [DeploymentItem("DataSources\\UPSRecert.xlsx")]
        
        public void Ship_UPSRecert_MI(DataRow row)
        {
            System.Diagnostics.Debug.WriteLine(row["TestID"].ToString());
            UpsMIFixture testObject = new UpsMIFixture();

            if (PopulateTestObject(row, testObject, null))
            {
                testObject.Ship();
            }
        }
    }
}