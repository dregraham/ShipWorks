using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Insure;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net.Insure
{
    public class InsureShipInsureShipmentRequestTest : IDisposable
    {
        private readonly AutoMock mock;

        public InsureShipInsureShipmentRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateInsurancePolicy_CreatesPolicyEntity_WhenShipmentDoesNotHaveOne()
        {
            var shipment = CreateShipment();
            var testObject = mock.Create<InsureShipInsureShipmentRequest>();

            testObject.CreateInsurancePolicy(shipment);

            Assert.NotNull(shipment.InsurancePolicy);
        }

        [Fact]
        public void CreateInsurancePolicy_DoesNotCreatePolicyEntity_WhenShipmentHasOne()
        {
            var policy = new InsurancePolicyEntity();
            var shipment = CreateShipment();
            shipment.InsurancePolicy = policy;
            var testObject = mock.Create<InsureShipInsureShipmentRequest>();

            testObject.CreateInsurancePolicy(shipment);

            Assert.Same(policy, shipment.InsurancePolicy);
        }

        [Fact]
        public void CreateInsurancePolicy_DelegatesToWebClient()
        {
            var shipment = CreateShipment();
            var testObject = mock.Create<InsureShipInsureShipmentRequest>();

            testObject.CreateInsurancePolicy(shipment);

            mock.Mock<IInsureShipWebClient>()
                .Verify(x => x.Submit<InsureShipNewPolicyResponse>("new_policy", It.IsAny<Dictionary<string, string>>()));
        }

        [Fact]
        public void CreateInsurancePolicy_SetsPolicyInformationOnShipment_WhenPolicyIsCreatedSuccessfully()
        {
            var shipment = CreateShipment();
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipNewPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(new InsureShipNewPolicyResponse { PolicyID = 456, Status = "Success" });
            var testObject = mock.Create<InsureShipInsureShipmentRequest>();

            testObject.CreateInsurancePolicy(shipment);

            Assert.True(shipment.InsurancePolicy.CreatedWithApi);
            Assert.Equal(456, shipment.InsurancePolicy.InsureShipPolicyID);
        }

        [Fact]
        public void CreateInsurancePolicy_ReturnsSuccess_WhenPolicyIsCreatedSuccessfully()
        {
            var shipment = CreateShipment();
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipNewPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(new InsureShipNewPolicyResponse { PolicyID = 456, Status = "Success" });
            var testObject = mock.Create<InsureShipInsureShipmentRequest>();

            var result = testObject.CreateInsurancePolicy(shipment);

            Assert.True(result.Success);
        }

        [Fact]
        public void CreateInsurancePolicy_ReturnsFailure_WhenPolicyCreationFailed()
        {
            var exception = new InsureShipException();
            var shipment = CreateShipment();
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipNewPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(exception);
            var testObject = mock.Create<InsureShipInsureShipmentRequest>();

            var result = testObject.CreateInsurancePolicy(shipment);

            result
                .Do(() => Assert.False(true))
                .OnFailure(ex => Assert.Same(exception, ex));
        }

        private ShipmentEntity CreateShipment() =>
            new ShipmentEntity { Order = new OrderEntity() };

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
