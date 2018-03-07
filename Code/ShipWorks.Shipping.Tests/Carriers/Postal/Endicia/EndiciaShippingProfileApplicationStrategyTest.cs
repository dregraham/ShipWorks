using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Postal.Endicia
{
    public class EndiciaShippingProfileApplicationStrategyTest
    {
        private readonly AutoMock mock;

        public EndiciaShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

        }

        [Fact]
        public void ApplyProfile_SetsEndiciaAccountID()
        {
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            var profile = new ShippingProfileEntity()
            {
                Postal = new PostalProfileEntity()
                {
                    Endicia = new EndiciaProfileEntity()
                    {
                        EndiciaAccountID = 123
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.Postal.Endicia.EndiciaAccountID);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaStealthPostage()
        {
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            var profile = new ShippingProfileEntity()
            {
                Postal = new PostalProfileEntity()
                {
                    Endicia = new EndiciaProfileEntity()
                    {
                        StealthPostage = true
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Endicia.StealthPostage);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaReferenceID()
        {
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            var profile = new ShippingProfileEntity()
            {
                Postal = new PostalProfileEntity()
                {
                    Endicia = new EndiciaProfileEntity()
                    {
                        ReferenceID = "123"
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("123", shipment.Postal.Endicia.ReferenceID);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaScanBasedReturn()
        {
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            var profile = new ShippingProfileEntity()
            {
                Postal = new PostalProfileEntity()
                {
                    Endicia = new EndiciaProfileEntity()
                    {
                        ScanBasedReturn = true
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Endicia.ScanBasedReturn);
        }

        [Fact]
        public void ApplyProfile_SetsEndiciaInsurance()
        {
            var testObject = mock.Create<EndiciaShippingProfileApplicationStrategy>();

            var shipment = new ShipmentEntity()
            {
                Postal = new PostalShipmentEntity()
                {
                    Endicia = new EndiciaShipmentEntity()
                }
            };

            var profile = new ShippingProfileEntity()
            {
                Insurance = true,
                Postal = new PostalProfileEntity()
                {
                    Endicia = new EndiciaProfileEntity()
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Endicia.Insurance);
        }
    }
}
