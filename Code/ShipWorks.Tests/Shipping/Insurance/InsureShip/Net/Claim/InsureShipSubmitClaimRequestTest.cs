using System;
using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance.InsureShip.Net;
using ShipWorks.Shipping.Insurance.InsureShip.Net.Claim;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Tests.Shipping.Insurance.InsureShip.Net.Claim
{
    public class InsureShipSubmitClaimRequestTest : IDisposable
    {
        private readonly AutoMock mock;

        public InsureShipSubmitClaimRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateInsuranceClaim_DelegatesToWebClient()
        {
            ShipmentEntity shipment = CreateShipment(policyID: 1234);
            var testObject = mock.Create<InsureShipSubmitClaimRequest>();

            testObject.CreateInsuranceClaim(shipment);

            mock.Mock<IInsureShipWebClient>()
                .Verify(x => x.Submit<InsureShipSubmitClaimResponse>("submit_claim", It.Is<Dictionary<string, string>>(y => y["policy_id"] == "1234")));
        }

        [Fact]
        public void CreateInsuranceClaim_ReturnsSuccess_WhenCallIsSuccessful()
        {
            var shipment = CreateShipment();
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipSubmitClaimResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(new InsureShipSubmitClaimResponse { Status = "Created" });

            var testObject = mock.Create<InsureShipSubmitClaimRequest>();

            var result = testObject.CreateInsuranceClaim(shipment);

            Assert.True(result.Success);
        }

        [Fact]
        public void CreateInsuranceClaim_SetsClaimIDOnShipment_WhenCallIsSuccessful()
        {
            var shipment = CreateShipment();
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipSubmitClaimResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(new InsureShipSubmitClaimResponse { Status = "Created", ClaimID = 987 });

            var testObject = mock.Create<InsureShipSubmitClaimRequest>();

            testObject.CreateInsuranceClaim(shipment);

            Assert.Equal(987, shipment.InsurancePolicy.ClaimID);
        }

        [Fact]
        public void CreateInsuranceClaim_ReturnsError_WhenCallFails()
        {
            var shipment = CreateShipment();
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipSubmitClaimResponse>(AnyString, It.IsAny<Dictionary<string, string>>()))
                .Returns(GenericResult.FromError<InsureShipSubmitClaimResponse>("Foo"));

            var testObject = mock.Create<InsureShipSubmitClaimRequest>();

            var result = testObject.CreateInsuranceClaim(shipment);

            result.Do(() => Assert.True(false))
                .OnFailure(ex => Assert.Equal("Foo", ex.Message));
        }

        private static ShipmentEntity CreateShipment(long policyID = 1234)
        {
            var policy = new InsurancePolicyEntity { InsureShipPolicyID = policyID };
            var shipment = new ShipmentEntity { InsurancePolicy = policy, Order = new OrderEntity() };
            return shipment;
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
