using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests
{
    public class ShipmentViewModelTest
    {
        ShipmentEntity shipment = new ShipmentEntity();

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
