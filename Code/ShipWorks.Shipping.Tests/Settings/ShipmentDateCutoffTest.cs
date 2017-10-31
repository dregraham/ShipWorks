using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using ShipWorks.Settings;
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

        [Fact]
        public void DictionaryOfShippingDateCutoff_SerializesAndDeserializes()
        {
            TimeSpan cutoffTime = TimeSpan.FromHours(2);

            ShipmentDateCutoff shipmentDateCutoff = new ShipmentDateCutoff(true, cutoffTime);

            Dictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = new Dictionary<ShipmentTypeCode, ShipmentDateCutoff>();
            testObject.Add(ShipmentTypeCode.Endicia, shipmentDateCutoff);
            testObject.Add(ShipmentTypeCode.UpsOnLineTools, new ShipmentDateCutoff(true, TimeSpan.FromHours(17)));

            string json = JsonConvert.SerializeObject(testObject);

            testObject = JsonConvert.DeserializeObject<Dictionary<ShipmentTypeCode, ShipmentDateCutoff>>(json);

            Assert.Equal(2, testObject.Count);
            Assert.Equal(cutoffTime, testObject.First().Value.CutoffTime);
            Assert.Equal(TimeSpan.FromHours(17), testObject.Last().Value.CutoffTime);
        }
    }
}
