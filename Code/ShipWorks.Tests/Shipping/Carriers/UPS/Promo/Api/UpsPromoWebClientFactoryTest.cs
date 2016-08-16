﻿using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Promo.Api
{
    public class UpsPromoWebClientFactoryTest
    {
        private readonly UpsPromoWebClientFactory testObject;
        private readonly Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>> upsAccountRepository;
        private readonly Mock<ICarrierSettingsRepository> upsSettingsRepository;

        public UpsPromoWebClientFactoryTest()
        {
            testObject = new UpsPromoWebClientFactory();
            upsAccountRepository = new Mock<ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>>();
            upsAccountRepository
                .Setup(r => r.GetAccount(It.IsAny<long>()))
                .Returns(new UpsAccountEntity()
                {
                    AccountNumber = "22"
                });

            upsSettingsRepository = new Mock<ICarrierSettingsRepository>();
            upsSettingsRepository
                .Setup(r => r.GetShippingSettings())
                .Returns(new ShippingSettingsEntity()
                {
                    UpsAccessKey = "/zJ4i4UGkI+TqGUaylfws+lDqbv4EV2K" // Decrypted: 3CECFFF7FF6F3365
                });
        }
    }
}
