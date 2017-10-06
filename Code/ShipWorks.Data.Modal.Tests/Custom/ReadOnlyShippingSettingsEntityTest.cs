using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Data.Model.ReadOnlyEntityClasses;
using ShipWorks.Settings;
using ShipWorks.Shipping;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class ReadOnlyReadOnlyShippingSettingsEntityTest
    {
        private IShippingSettingsEntity readOnlyShippingSettingsEntity;
        private readonly IDictionary<ShipmentTypeCode, ShipmentDateCutoff> source;

        public ReadOnlyReadOnlyShippingSettingsEntityTest()
        {
            ShippingSettingsEntity shippintSettingsEntity = new ShippingSettingsEntity()
            {
                ShipmentDateCutoffJson = "{\"Endicia\":{\"Enabled\":true,\"CutoffTime\":\"02:00:00\"},\"UpsOnLineTools\":{\"Enabled\":false,\"CutoffTime\":\"17:00:00\"}}"
            };

            readOnlyShippingSettingsEntity = shippintSettingsEntity.AsReadOnly();

            source = new Dictionary<ShipmentTypeCode, ShipmentDateCutoff>();
            source.Add(ShipmentTypeCode.Endicia, new ShipmentDateCutoff(true, TimeSpan.FromHours(2)));
            source.Add(ShipmentTypeCode.UpsOnLineTools, new ShipmentDateCutoff(false, TimeSpan.FromHours(17)));
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsReadOnlyDictionary_WithCorrectValues()
        {
            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = readOnlyShippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[ShipmentTypeCode.Endicia], testObject[ShipmentTypeCode.Endicia]);
            Assert.Equal(source[ShipmentTypeCode.UpsOnLineTools], testObject[ShipmentTypeCode.UpsOnLineTools]);
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsReadOnlyDictionary_WithCorrectValues_WhenCalledASecondTime()
        {
            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = readOnlyShippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[ShipmentTypeCode.Endicia], testObject[ShipmentTypeCode.Endicia]);
            Assert.Equal(source[ShipmentTypeCode.UpsOnLineTools], testObject[ShipmentTypeCode.UpsOnLineTools]);

            testObject = readOnlyShippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[ShipmentTypeCode.Endicia], testObject[ShipmentTypeCode.Endicia]);
            Assert.Equal(source[ShipmentTypeCode.UpsOnLineTools], testObject[ShipmentTypeCode.UpsOnLineTools]);
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsEmptyReadOnlyDictionary_WhenJsonIsEmpty()
        {
            ShippingSettingsEntity shippintSettingsEntity = new ShippingSettingsEntity()
            {
                ShipmentDateCutoffJson = string.Empty
            };
            readOnlyShippingSettingsEntity = shippintSettingsEntity.AsReadOnly();

            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = readOnlyShippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(0, testObject.Count);
        }

        [Fact]
        public void ShipmentDateCutoffList_ReturnsEmptyReadOnlyDictionary_WhenJsonIsDefault()
        {
            ShippingSettingsEntity shippintSettingsEntity = new ShippingSettingsEntity();
            readOnlyShippingSettingsEntity = shippintSettingsEntity.AsReadOnly();

            ReadOnlyDictionary<ShipmentTypeCode, ShipmentDateCutoff> testObject = readOnlyShippingSettingsEntity.ShipmentDateCutoffList;

            Assert.Equal(0, testObject.Count);
        }

        [Fact]
        public void GetShipmentDateCutoff_ReturnsDefault_WhenShipmentTypeCodeNotInDictionary()
        {
            ShipmentDateCutoff cutoff = readOnlyShippingSettingsEntity.GetShipmentDateCutoff(ShipmentTypeCode.Amazon);

            Assert.Equal(ShipmentDateCutoff.Default, cutoff);
        }
    }
}
