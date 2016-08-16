using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.iParcel.BestRate;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Tests.Shipping.Carriers.iParcel.BestRate
{
    public class iParcelBestRateBrokerTest
    {
        [Fact]
        public void GetInsuranceProvider_ReturnsShipWorks_iParcelSettingSpecfiesShipWorks()
        {
            var mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });
            
            iParcelBestRateBroker testObject = mock.Create<iParcelBestRateBroker>();
            testObject.GetRatesAction = (shipment, type) => new RateGroup(new List<RateResult>());

            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { IParcelInsuranceProvider = (int)InsuranceProvider.ShipWorks }));
        }

        [Fact]
        public void GetInsuranceProvider_ReturnsCarrier_iParcelSettingSpecfiesCarrier()
        {
            var mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

            iParcelBestRateBroker testObject = mock.Create<iParcelBestRateBroker>();
            testObject.GetRatesAction = (shipment, type) => new RateGroup(new List<RateResult>());

            Assert.Equal(InsuranceProvider.Carrier, testObject.GetInsuranceProvider(new ShippingSettingsEntity() { IParcelInsuranceProvider = (int)InsuranceProvider.Carrier }));
        }
    }
}
