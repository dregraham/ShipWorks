using System;
using Newtonsoft.Json;
using ShipWorks.Shipping.Settings;
using Xunit;

namespace ShipWorks.Shipping.Tests.Settings
{
    public class ShipmentDateCutoffTest
    {
        [Fact]
        public void ShippingDateCutoff_SerializesAndDeserializes()
        {
            TimeSpan cutoffTime = TimeSpan.FromHours(2);

            ShipmentDateCutoff shipmentDateCutoff = new ShipmentDateCutoff(true, cutoffTime);

            string json = JsonConvert.SerializeObject(shipmentDateCutoff);

            ShipmentDateCutoff testObject = JsonConvert.DeserializeObject<ShipmentDateCutoff>(json);

            Assert.Equal(true, testObject.Enabled);
            Assert.Equal(cutoffTime, testObject.CutoffTime);
        }
    }
}
