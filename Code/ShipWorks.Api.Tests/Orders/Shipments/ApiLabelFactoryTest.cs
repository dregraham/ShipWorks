using Xunit;
using ShipWorks.Tests.Shared;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data;
using Moq;

namespace ShipWorks.Api.Tests.Orders.Shipments
{
    public class ApiLabelFactoryTest
    {
        [Fact]
        public void GetLabels_DelegatesToDataResourceManager_GetConsumerResourceReferences()
        {
            using(var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<ApiLabelFactory>();
                testObject.GetLabels(42);
                mock.Mock<IDataResourceManager>().Verify(m => m.GetConsumerResourceReferences(42), Times.Once);
            }
        }
    }
}
