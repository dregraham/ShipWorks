using System.Threading.Tasks;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using Xunit;
using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.UI;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Tests;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ShipWorks.Shipping.UI.Tests
{
    public class UnitTest1
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
