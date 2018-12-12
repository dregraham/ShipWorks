using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Void;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net.Void
{
    public class InsureShipVoidPolicyRequestTest : IDisposable
    {
        private readonly AutoMock mock;

        public InsureShipVoidPolicyRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void VoidInsurancePolicy_DoesNotDelegateToWebClient_WhenValidationReturnsError()
        {
            var shipment = new ShipmentEntity();

            mock.Mock<IInsureShipVoidValidator>().Setup(x => x.IsVoidable(shipment)).Returns(GenericResult.FromError<bool>("Foo"));
            var testObject = mock.Create<InsureShipVoidPolicyRequest>();

            testObject.VoidInsurancePolicy(shipment);

            mock.Mock<IInsureShipWebClient>()
                .Verify(x => x.Submit<InsureShipVoidPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [Fact]
        public void VoidInsurancePolicy_DelegatesToWebClient_WhenValidationReturnsFalse()
        {
            var shipment = new ShipmentEntity();

            mock.Mock<IInsureShipVoidValidator>().Setup(x => x.IsVoidable(shipment)).Returns(false);
            var testObject = mock.Create<InsureShipVoidPolicyRequest>();

            testObject.VoidInsurancePolicy(shipment);

            mock.Mock<IInsureShipWebClient>()
                .Verify(x => x.Submit<InsureShipVoidPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()), Times.Never);
        }

        [Fact]
        public void VoidInsurancePolicy_DelegatesToWebClient_WhenValidationReturnsTrue()
        {
            var shipment = new ShipmentEntity { InsurancePolicy = new InsurancePolicyEntity() };

            mock.Mock<IInsureShipVoidValidator>().Setup(x => x.IsVoidable(shipment)).Returns(true);
            var testObject = mock.Create<InsureShipVoidPolicyRequest>();

            testObject.VoidInsurancePolicy(shipment);

            mock.Mock<IInsureShipWebClient>()
                .Verify(x => x.Submit<InsureShipVoidPolicyResponse>("void_policy", It.IsAny<Dictionary<string, string>>()));
        }

        [Fact]
        public void VoidInsurancePolicy_ReturnsSuccess_WhenWebClientReturnsSuccess()
        {
            var shipment = new ShipmentEntity { InsurancePolicy = new InsurancePolicyEntity() };

            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipVoidPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(GenericResult.FromSuccess(new InsureShipVoidPolicyResponse()));
            mock.Mock<IInsureShipVoidValidator>().Setup(x => x.IsVoidable(shipment)).Returns(true);
            var testObject = mock.Create<InsureShipVoidPolicyRequest>();

            var result = testObject.VoidInsurancePolicy(shipment);

            Assert.True(result.Success);
        }

        [Fact]
        public void VoidInsurancePolicy_ReturnsSuccess_WhenValidatorReturnsFalse()
        {
            var shipment = new ShipmentEntity { InsurancePolicy = new InsurancePolicyEntity() };

            mock.Mock<IInsureShipVoidValidator>().Setup(x => x.IsVoidable(shipment)).Returns(false);
            var testObject = mock.Create<InsureShipVoidPolicyRequest>();

            var result = testObject.VoidInsurancePolicy(shipment);

            Assert.True(result.Success);
        }

        [Fact]
        public void VoidInsurancePolicy_ReturnsFailure_WhenWebClientReturnsFailure()
        {
            var shipment = new ShipmentEntity { InsurancePolicy = new InsurancePolicyEntity() };

            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipVoidPolicyResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(GenericResult.FromError<InsureShipVoidPolicyResponse>("Foo"));
            mock.Mock<IInsureShipVoidValidator>().Setup(x => x.IsVoidable(shipment)).Returns(true);
            var testObject = mock.Create<InsureShipVoidPolicyRequest>();

            var result = testObject.VoidInsurancePolicy(shipment);

            Assert.False(result.Success);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
