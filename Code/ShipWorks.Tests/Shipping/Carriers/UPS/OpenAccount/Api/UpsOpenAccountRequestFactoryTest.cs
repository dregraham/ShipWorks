using System;
using System.Linq;
using Autofac;
using Autofac.Extras.Moq;
using Autofac.Features.Indexed;
using Moq;
using Xunit;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.LinkNewAccount.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request;
using ShipWorks.Shipping.Carriers.UPS.OpenAccount.Api.Request.Manipulators;
using ShipWorks.Shipping.Carriers.UPS.WebServices.OpenAccount;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.OpenAccount.Api
{
    public class UpsOpenAccountRequestFactoryTest : IDisposable
    {
        private AutoMock mock;
        private UpsOpenAccountRequestFactory testObject;

        public UpsOpenAccountRequestFactoryTest()
        {
            mock = AutoMock.GetLoose();

            var carrierResponseFactory = mock.Mock<ICarrierResponseFactory>();

            var upsAccountRepoProvider = mock.MockRepository.Create<IIndex<ShipmentTypeCode, ICarrierResponseFactory>>();
            upsAccountRepoProvider.Setup(x => x[ShipmentTypeCode.UpsOnLineTools]).Returns(carrierResponseFactory.Object);
            mock.Provide(carrierResponseFactory.Object);

            testObject = mock.Create<UpsOpenAccountRequestFactory>(
                new TypedParameter(typeof(UpsAccountEntity), new UpsAccountEntity()),
                new TypedParameter(typeof(IIndex<ShipmentTypeCode, ICarrierResponseFactory>), upsAccountRepoProvider.Object));
        }

        [Fact]
        public void CreateOpenAccountRequest_ReturnsUpsOpenAccountRequest()
        {
            CarrierRequest request = testObject.CreateOpenAccountRequest(new OpenAccountRequest());

            Assert.IsAssignableFrom<UpsOpenAccountRequest>(request);
        }

        [Fact]
        public void CreateOpenAccountRequest_PopulatesManipulators()
        {
            CarrierRequest request = testObject.CreateOpenAccountRequest(new OpenAccountRequest()) as UpsOpenAccountRequest;

            // This will obviously need to change as manipulators are added in the factory and also serve as a
            // reminder that to write the tests to ensure the manipulator type is present in the list
            Assert.Equal(1, request.Manipulators.Count());
        }

        [Fact]
        public void CreateOpenAccountRequest_AddsUpsOpenAccountAddEndUserInformation()
        {
            CarrierRequest request = testObject.CreateOpenAccountRequest(new OpenAccountRequest()) as UpsOpenAccountRequest;

            Assert.True(request.Manipulators.Count(m => m.GetType() == typeof(UpsOpenAccountAddEndUserInformation)) == 1);
        }

        [Fact]
        public void CreateLinkNewAccountRequestFactory_AddsUpsLinkNewAccountInfoManipulatorToManiupulators()
        {
            CarrierRequest request = testObject.CreateLinkNewAccountRequestFactory();

            Assert.True(request.Manipulators.Single().GetType() == typeof(UpsLinkNewAccountInfoManipulator));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}