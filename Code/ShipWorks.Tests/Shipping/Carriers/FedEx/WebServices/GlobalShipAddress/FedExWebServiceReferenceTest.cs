using Xunit;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress
{
    public class FedExWebServiceReferenceTest
    {
        [Fact]
        public void DataTypeOfGlobalShipAddressDistanceValue_IsDouble_Test()
        {
            // The Distance.Value is a decimal datatype in the original reference that gets generated from 
            // the WSDL. There are cases where the distance coming back from FedEx are so small, such as
            // 1.3818644514151153E-5, that decimal.Parse will blow up when trying to deserialize response from FedEx.
            // We had to manually update the Reference.cs file to change the data type of GlobalShipAddress.Distance.Value
            // to be a double. This is just a test to serve as a reminder that the Reference.cs needs to be updated
            // if it gets regenerated in the future.
            ShipWorks.Shipping.Carriers.FedEx.WebServices.GlobalShipAddress.Distance d = new Distance();
            Assert.IsInstanceOfType(d.Value, typeof(double));
        }
    }
}
