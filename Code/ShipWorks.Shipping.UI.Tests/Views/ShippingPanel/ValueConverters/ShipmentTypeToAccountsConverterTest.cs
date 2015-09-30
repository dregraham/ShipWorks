using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Moq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.ShippingPanel.ValueConverters;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Views.ShippingPanel.ValueConverters
{
    public class ShipmentTypeToAccountsConverterTest
    {
        [Fact]
        public void Convert_ReturnsEmptyICarrierAccount_ValueIsNull()
        {
            ShipmentTypeToAccountsConverter testObject = new ShipmentTypeToAccountsConverter();

            Assert.True(CallConvert(testObject, null).None());
        }

        [Fact]
        public void Convert_NullCarrierAccountReturned_WhenNoAvailableAccounts()
        {
            Mock<IShippingAccountListProvider> shippingAccountListProvider = new Mock<IShippingAccountListProvider>();
            shippingAccountListProvider.Setup(p => p.GetAvailableAccounts(It.IsAny<ShipmentTypeCode>())).Returns(Enumerable.Empty<ICarrierAccount>());

            ShipmentTypeToAccountsConverter testObject = new ShipmentTypeToAccountsConverter(shippingAccountListProvider.Object);

            var carrierAccounts = CallConvert(testObject, ShipmentTypeCode.FedEx);

            Assert.IsType<NullCarrierAccount>(carrierAccounts.Single());
        }

        [Fact]
        public void Convert_CarrierAccountReturned_WhenAvailableAccount()
        {
            List<ICarrierAccount> carrierAccounts = new List<ICarrierAccount>()
            {
                new FedExAccountEntity(42) 
            };

            Mock<IShippingAccountListProvider> shippingAccountListProvider = new Mock<IShippingAccountListProvider>();
            shippingAccountListProvider.Setup(p => p.GetAvailableAccounts(It.IsAny<ShipmentTypeCode>())).Returns(carrierAccounts);

            ShipmentTypeToAccountsConverter testObject = new ShipmentTypeToAccountsConverter(shippingAccountListProvider.Object);

            var returnedCarrierAccounts = CallConvert(testObject, ShipmentTypeCode.FedEx);

            Assert.Same(carrierAccounts, returnedCarrierAccounts);
        }

        public IEnumerable<ICarrierAccount> CallConvert(ShipmentTypeToAccountsConverter testObject, object value)
        {
            return (IEnumerable<ICarrierAccount>)testObject.Convert(value, typeof(string), null, null);
        }
    }
}
