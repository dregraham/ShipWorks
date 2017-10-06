using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class ShippingSettingsEntityTest
    {
        private ShippingSettingsEntity shippingSettingsEntity;
        private readonly IDictionary<ShipmentTypeCode, ShipmentDateCutoff> source;

        public ShippingSettingsEntityTest()
        {
            shippingSettingsEntity = new ShippingSettingsEntity()
            {
                ShipmentDateCutoffJson = "{\"Endicia\":{\"Enabled\":true,\"CutoffTime\":\"02:00:00\"},\"UpsOnLineTools\":{\"Enabled\":false,\"CutoffTime\":\"17:00:00\"}}"
            };

            source = new Dictionary<ShipmentTypeCode, ShipmentDateCutoff>();
            source.Add(ShipmentTypeCode.Endicia, new ShipmentDateCutoff(true, TimeSpan.FromHours(2)));
            source.Add(ShipmentTypeCode.UpsOnLineTools, new ShipmentDateCutoff(false, TimeSpan.FromHours(17)));
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsReadOnlyDictionary_WithCorrectValues()
        {
            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[ShipmentTypeCode.Endicia], testObject[ShipmentTypeCode.Endicia]);
            Assert.Equal(source[ShipmentTypeCode.UpsOnLineTools], testObject[ShipmentTypeCode.UpsOnLineTools]);
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsReadOnlyDictionary_WithCorrectValues_WhenCalledASecondTime()
        {
            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[ShipmentTypeCode.Endicia], testObject[ShipmentTypeCode.Endicia]);
            Assert.Equal(source[ShipmentTypeCode.UpsOnLineTools], testObject[ShipmentTypeCode.UpsOnLineTools]);

            testObject = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[ShipmentTypeCode.Endicia], testObject[ShipmentTypeCode.Endicia]);
            Assert.Equal(source[ShipmentTypeCode.UpsOnLineTools], testObject[ShipmentTypeCode.UpsOnLineTools]);
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsEmptyReadOnlyDictionary_WhenJsonIsEmpty()
        {
            shippingSettingsEntity.ShipmentDateCutoffJson = string.Empty;
            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(0, testObject.Count);
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsEmptyReadOnlyDictionary_WhenJsonIsDefault()
        {
            shippingSettingsEntity = new ShippingSettingsEntity();
            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(0, testObject.Count);
        }

        [Fact]
        public void SetShipmentDateCutoff_UpdatesShipmentDateCutoff()
        {
            string newJson = "{\"Endicia\":{\"Enabled\":true,\"CutoffTime\":\"02:00:00\"},\"UpsOnLineTools\":{\"Enabled\":true,\"CutoffTime\":\"11:00:00\"}}";
            ShipmentDateCutoff newShipmentDateCutoff = new ShipmentDateCutoff(true, TimeSpan.FromHours(11));
            shippingSettingsEntity.SetShipmentDateCutoff(ShipmentTypeCode.UpsOnLineTools, newShipmentDateCutoff);

            var cutoffList = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, cutoffList.Count);
            Assert.Equal(cutoffList[ShipmentTypeCode.UpsOnLineTools], newShipmentDateCutoff);
            Assert.Equal(newJson, shippingSettingsEntity.ShipmentDateCutoffJson);
        }

        [Fact]
        public void SetShipmentDateCutoff_ThrowsArgumentNullException_WhenNullShipmentDateCutoff()
        {
            Assert.Throws<ArgumentNullException>(() => shippingSettingsEntity.SetShipmentDateCutoff(ShipmentTypeCode.UpsOnLineTools, null));
        }

        [Fact]
        public void GetShipmentDateCutoff_ReturnsDefault_WhenShipmentTypeCodeNotInDictionary()
        {
            ShipmentDateCutoff cutoff = shippingSettingsEntity.GetShipmentDateCutoff(ShipmentTypeCode.Amazon);

            Assert.Equal(ShipmentDateCutoff.Default, cutoff);
        }

        [Fact]
        public void SetShipmentDateCutoff_AddsShipmentDateCutoff_WhenShipmentTypeCodeNotInDictionary()
        {
            string newJson = "{\"Endicia\":{\"Enabled\":true,\"CutoffTime\":\"02:00:00\"},\"UpsOnLineTools\":{\"Enabled\":false,\"CutoffTime\":\"17:00:00\"},\"FedEx\":{\"Enabled\":true,\"CutoffTime\":\"13:00:00\"}}";
            ShipmentDateCutoff newShipmentDateCutoff = new ShipmentDateCutoff(true, TimeSpan.FromHours(13));
            shippingSettingsEntity.SetShipmentDateCutoff(ShipmentTypeCode.FedEx, newShipmentDateCutoff);

            var cutoffList = shippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(3, cutoffList.Count);
            Assert.Equal(cutoffList[ShipmentTypeCode.FedEx], newShipmentDateCutoff);
            Assert.Equal(newJson, shippingSettingsEntity.ShipmentDateCutoffJson);
        }
    }
}
