using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.OrderLookup.ShipmentHistory;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.OrderLookup.Tests.ShipmentHistory
{
    public class PreviousShipmentVoidActionHandlerTest
    {
        private readonly AutoMock mock;
        private readonly PreviousShipmentVoidActionHandler testObject;

        public PreviousShipmentVoidActionHandlerTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IShippingManager>()
                .Setup(x => x.VoidShipment(AnyLong, It.IsAny<IShippingErrorManager>()))
                .Returns(GenericResult.FromSuccess(mock.Build<ICarrierShipmentAdapter>()));

            mock.Mock<IOrderLookupPreviousShipmentLocator>()
                .Setup(x => x.GetLatestShipmentDetails())
                .ReturnsAsync(new PreviousProcessedShipmentDetails(123, false));

            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowQuestion(AnyString))
                .Returns(DialogResult.OK);

            testObject = mock.Create<PreviousShipmentVoidActionHandler>();
        }

        [Fact]
        public void Void_DelegatesToShippingManager()
        {
            testObject.Void(new ProcessedShipmentEntity { ShipmentID = 123 });

            mock.Mock<IShippingManager>().Verify(x => x.VoidShipment(123, It.IsAny<IShippingErrorManager>()));
        }

        [Fact]
        public void Void_DoesNotDelegateToShippingManager_WhenPopupCanceled()
        {
            mock.Mock<IMessageHelper>()
                .Setup(x => x.ShowQuestion(AnyString))
                .Returns(DialogResult.Cancel);

            testObject.Void(new ProcessedShipmentEntity { ShipmentID = 123 });

            mock.Mock<IShippingManager>().Verify(x => x.VoidShipment(AnyLong, It.IsAny<IShippingErrorManager>()), Times.Never);
        }
    }
}
