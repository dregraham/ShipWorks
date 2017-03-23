using System;
using System.Linq;
using Autofac;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Endicia.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Express1;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.WorldShip.BestRate;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class CarrierAccountRepositoryRegistrationTest : IDisposable
    {
        private readonly IContainer container;

        public CarrierAccountRepositoryRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Amazon, typeof(NullAccountRepository))]
        [InlineData(ShipmentTypeCode.BestRate, typeof(NullAccountRepository))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaAccountRepository))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(Express1EndiciaAccountRepository))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(Express1UspsAccountRepository))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(FedExAccountRepository))]
        [InlineData(ShipmentTypeCode.iParcel, typeof(iParcelAccountRepository))]
        [InlineData(ShipmentTypeCode.None, typeof(NullAccountRepository))]
        [InlineData(ShipmentTypeCode.OnTrac, typeof(OnTracAccountRepository))]
        [InlineData(ShipmentTypeCode.Other, typeof(NullAccountRepository))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(NullAccountRepository))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(UpsAccountRepository))]
        [InlineData(ShipmentTypeCode.UpsWorldShip, typeof(WorldShipAccountRepository))]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsAccountRepository))]
        public void EnsureCarrierAccountRetrieversAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            ICarrierAccountRetriever retriever = container.ResolveKeyed<ICarrierAccountRetriever>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Theory]
        [InlineData(typeof(ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>), typeof(EndiciaAccountRepository))]
        [InlineData(typeof(ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity>), typeof(FedExAccountRepository))]
        [InlineData(typeof(ICarrierAccountRepository<IParcelAccountEntity, IIParcelAccountEntity>), typeof(iParcelAccountRepository))]
        [InlineData(typeof(ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity>), typeof(OnTracAccountRepository))]
        [InlineData(typeof(ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity>), typeof(UpsAccountRepository))]
        [InlineData(typeof(ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>), typeof(UspsAccountRepository))]
        public void EnsureCarrierRepositoriesAreRegisteredCorrectly(Type interfaceType, Type expectedServiceType)
        {
            object service = container.Resolve(interfaceType);
            Assert.Equal(expectedServiceType, service.GetType());
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Endicia, typeof(EndiciaAccountRepository))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(Express1EndiciaAccountRepository))]
        public void EnsureEndiciaExpress1AccountRepositoriesAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            var retriever = container.ResolveKeyed<ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity>>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps, typeof(UspsAccountRepository))]
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(Express1UspsAccountRepository))]
        public void EnsureUspsExpress1AccountRepositoriesAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            var retriever = container.ResolveKeyed<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Fact]
        public void EnsureAllCarriersHaveAccountRepositoryRegistered()
        {
            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>())
            {
                ICarrierAccountRetriever service = container.ResolveKeyed<ICarrierAccountRetriever>(value);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
