using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class UspsShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly UspsShippingProfileApplicationStrategy testObject;

        public UspsShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<UspsShippingProfileApplicationStrategy>();
        }

        [Fact]
        public void ApplyProfile_AppliesBaseProfile()
        {
            var baseShippingProfileApplicationStrategy = mock.Mock<IShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity()
                }
            };
            var profile = new ShippingProfileEntity
            {
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity()
                }
            };

            testObject.ApplyProfile(profile, shipment);
            
            baseShippingProfileApplicationStrategy.Verify(b => b.ApplyProfile(profile, shipment));
        }

        [Fact]
        public void ApplyProfile_AppliesUspsAccountID()
        {
            var shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity
                    {
                        UspsAccountID = 2
                    }
                }
            };
            var profile = new ShippingProfileEntity
            {
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity
                    {
                        UspsAccountID = 1
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(1, shipment.Postal.Usps.UspsAccountID);
        }

        [Fact]
        public void ApplyProfile_AppliesRequireFullAddressValidation()
        {
            var shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity
                    {
                        RequireFullAddressValidation = false
                    }
                }
            };
            var profile = new ShippingProfileEntity
            {
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity
                    {
                        RequireFullAddressValidation = true
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Usps.RequireFullAddressValidation);
        }

        [Fact]
        public void ApplyProfile_AppliesHidePostage()
        {
            var shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity
                    {
                        HidePostage = false
                    }
                }
            };
            var profile = new ShippingProfileEntity
            {
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity
                    {
                        HidePostage = true
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Usps.HidePostage);
        }

        [Fact]
        public void ApplyProfile_AppliesRateShop()
        {
            var shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity
                    {
                        RateShop = false
                    }
                }
            };
            var profile = new ShippingProfileEntity
            {
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity
                    {
                        RateShop = true
                    }
                }
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Usps.RateShop);
        }

        [Fact]
        public void ApplyProfile_AppliesPostalProfile()
        {
            var shipment = new ShipmentEntity
            {
                Postal = new PostalShipmentEntity
                {
                    Usps = new UspsShipmentEntity
                    {
                        Insurance = false
                    }
                }
            };
            var profile = new ShippingProfileEntity
            {
                Postal = new PostalProfileEntity
                {
                    Usps = new UspsProfileEntity()
                },
                Insurance = true
            };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Postal.Usps.Insurance);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}