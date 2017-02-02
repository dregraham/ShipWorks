﻿using System.Linq;
using Autofac.Features.Indexed;
using Moq;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Services.Accounts;
using Xunit;

namespace ShipWorks.Shipping.Tests.Services.Account
{
    public class ShippingAccountListProviderTest
    {
        private readonly Mock<IIndex<ShipmentTypeCode, ICarrierAccountRetriever>> carrierAccountRetrieverLookup;
        private readonly Mock<ICarrierAccountRetriever> mockedFedExRepository;
        private readonly Mock<ICarrierAccount> carrierAccount;

        public ShippingAccountListProviderTest()
        {
            carrierAccount = new Mock<ICarrierAccount>();
            carrierAccount.Setup(a => a.AccountId).Returns(42);

            mockedFedExRepository = new Mock<ICarrierAccountRetriever>();
            mockedFedExRepository.Setup(r => r.AccountsReadOnly).Returns(() => new[] { carrierAccount.Object });

            carrierAccountRetrieverLookup = new Mock<IIndex<ShipmentTypeCode, ICarrierAccountRetriever>>();
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

            Assert.IsType<NullCarrierAccount>(testObject.GetAvailableAccounts(ShipmentTypeCode.Other).Single());
        }
    }
}
