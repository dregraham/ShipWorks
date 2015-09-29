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
        public void AvailableAccounts_ContainsCorrectAccounts_WhenShipmentTypeCodeIsSet()
        {
            var testObject = new ShippingAccountListProvider(carrierAccountRetrieverLookup.Object);
            
            Assert.Same(carrierAccount.Object, testObject.GetAvailableAccounts(ShipmentTypeCode.FedEx).Single());
        }

        [Fact]
        public void AvailableAccounts_ContainsNullRepo_AfterShipmentTypeHasNullAccountRepository()
        {
            var testObject = new ShippingAccountListProvider(carrierAccountRetrieverLookup.Object);

            Assert.IsType<NullEntity>(testObject.GetAvailableAccounts(ShipmentTypeCode.Other).Single());
        }
    }
}
