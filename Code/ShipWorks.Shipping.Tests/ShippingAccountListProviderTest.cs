using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Collections;
using Moq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Shipping.Carriers;
using Xunit;

namespace ShipWorks.Shipping.Tests
{
    public class ShippingAccountListProviderTest
    {
        private Mock<IIndex<ShipmentTypeCode, ICarrierAccountRetriever<ICarrierAccount>>> carrierAccountRetrieverLookup;
        private Mock<ICarrierAccountRetriever<ICarrierAccount>> mockedFedExRepository;
        private Mock<ICarrierAccount> carrierAccount;
        
        public ShippingAccountListProviderTest()
        {
            carrierAccount = new Mock<ICarrierAccount>();
            carrierAccount.Setup(a => a.AccountId).Returns(42);

            mockedFedExRepository = new Mock<ICarrierAccountRetriever<ICarrierAccount>>();
            mockedFedExRepository.Setup(r => r.Accounts).Returns(() => new[] { carrierAccount.Object });

            carrierAccountRetrieverLookup = new Mock<IIndex<ShipmentTypeCode, ICarrierAccountRetriever<ICarrierAccount>>>();
            carrierAccountRetrieverLookup.Setup(x => x[ShipmentTypeCode.FedEx]).Returns(mockedFedExRepository.Object);
            carrierAccountRetrieverLookup.Setup(x => x[ShipmentTypeCode.Other]).Returns(new NullAccountRepository());
        }

        [Fact]
        public void AvailableAccounts_ContainsNoItems_WhenNoShipmentTypeCodeIsSet()
        {
            var testObject = new ShippingAccountListProvider(null);
            Assert.Equal(0, testObject.AvailableAccounts.Count);
        }

        [Fact]
        public void AvailableAccounts_ContainsCorrectAccounts_WhenShipmentTypeCodeIsSet()
        {
            var testObject = new ShippingAccountListProvider(carrierAccountRetrieverLookup.Object);
            testObject.ShipmentTypeCode = ShipmentTypeCode.FedEx;
            
            Assert.Same(carrierAccount.Object, testObject.AvailableAccounts.Single());
        }

        [Fact]
        public void AvailableAccounts_SameObservableAccount_AfterShipmentTypeChanged()
        {
            var testObject = new ShippingAccountListProvider(carrierAccountRetrieverLookup.Object);
            var originalAvailableAccounts = testObject.AvailableAccounts;

            testObject.ShipmentTypeCode = ShipmentTypeCode.FedEx;
            Assert.Same(originalAvailableAccounts, testObject.AvailableAccounts);

            testObject.ShipmentTypeCode = ShipmentTypeCode.Other;
            Assert.Same(originalAvailableAccounts, testObject.AvailableAccounts);
        }
    }
}
