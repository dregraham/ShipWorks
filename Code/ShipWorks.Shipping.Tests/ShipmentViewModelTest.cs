using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Shipping.Tests
{
    public class ShipmentViewModelTest
    {
        readonly ShipmentEntity shipment = new ShipmentEntity();

        [Fact]
        public void Save_UpdatesShipmentEntity_WhenTotalWeightChanged_Test()
        {
            using (var mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                ShipmentViewModel testObject = mock.Create<ShipmentViewModel>();
                testObject.Load(shipment);

                testObject.TotalWeight = 2.93;
                testObject.Save(shipment);

                Assert.Equal(2.93, shipment.TotalWeight);
            }
        }
    }
}
