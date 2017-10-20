using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Insurance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressBestRateBrokerTest
    {
        [Fact]
        public void GetInsuranceProvider_ReturnsInvalid()
        {
            var mock = AutoMock.GetFromRepository(new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock });

            var testObject = mock.Create<DhlExpressBestRateBroker>();
            
            Assert.Equal(InsuranceProvider.ShipWorks, testObject.GetInsuranceProvider(new ShippingSettingsEntity()));
        }
    }
}
