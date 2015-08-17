using System;
using System.Data;
using Xunit;

namespace ShipWorks.Tests.Integration.MSTest.Shipping.Carriers.iParcel
{
    public class iParcelIntegrationTest : DataDrivenIntegrationTestBase
    {
        //[DataSource("DataSource_Ship_iParcel")]
        //[DeploymentItem("DataSources\\iParcel.xlsx")]
        
        public void Ship_iParcel(DataRow row)
        {
            iParcelFixture testObject = new iParcelFixture();

            if (PopulateTestObject(row, testObject, null))
            {
                Console.WriteLine(string.Format("Executing test ID {0}", row["TestID"]));

                testObject.Ship();
            }
        }
    }
}