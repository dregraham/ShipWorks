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
    public class InsureShipClaimStatusRequestTest : IDisposable
    {
        private readonly AutoMock mock;

        public InsureShipClaimStatusRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void GetClaimStatus_DelegatesToWebClient()
        {
            var policy = new InsurancePolicyEntity { ClaimID = 1234 };
            var shipment = new ShipmentEntity { Processed = true, InsurancePolicy = policy, Order = new OrderEntity { Store = new StoreEntity() } };
            var testObject = mock.Create<InsureShipClaimStatusRequest>();

            testObject.GetClaimStatus(shipment).Match(x => x, ex => string.Empty);

            mock.Mock<IInsureShipWebClient>()
                .Verify(x => x.Submit<InsureShipGetClaimStatusResponse>("get_claim_status", AnyIStore, It.Is<Dictionary<string, string>>(y => y["claim_id"] == "1234")));
        }

        [Fact]
        public void GetClaimStatus_ReturnsStatus_WhenCallIsSuccessful()
        {
            var policy = new InsurancePolicyEntity();
            var shipment = new ShipmentEntity { Processed = true, InsurancePolicy = policy, Order = new OrderEntity { Store = new StoreEntity() } };
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipGetClaimStatusResponse>(AnyString, AnyIStore, It.IsAny<Dictionary<string, string>>()))
                .Returns(new InsureShipGetClaimStatusResponse { Status = "Created" });

            var testObject = mock.Create<InsureShipClaimStatusRequest>();

            var result = testObject.GetClaimStatus(shipment).Match(x => x, ex => string.Empty);

            Assert.Equal("Created", result);
        }

        [Fact]
        public void GetClaimStatus_ReturnsError_WhenCallFails()
        {
            var shipment = new ShipmentEntity { Processed = true, InsurancePolicy = new InsurancePolicyEntity(), Order = new OrderEntity { Store = new StoreEntity() } };
            mock.Mock<IInsureShipWebClient>()
                .Setup(x => x.Submit<InsureShipGetClaimStatusResponse>(AnyString, AnyIStore, It.IsAny<Dictionary<string, string>>()))
                .Returns(GenericResult.FromError<InsureShipGetClaimStatusResponse>("Foo"));

            var testObject = mock.Create<InsureShipClaimStatusRequest>();

            var result = testObject.GetClaimStatus(shipment);

            result.Do(x => Assert.True(false))
                .OnFailure(ex => Assert.Equal("Foo", ex.Message));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
