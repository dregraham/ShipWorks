using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;
using ShipWorks.Tests.Shared;
using ShipWorks.Users.Security;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class SingleScanShipmentConfirmationServiceTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly SingleScanShipmentConfirmationService testObject;

        public SingleScanShipmentConfirmationServiceTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, It.IsAny<long>()))
                .Returns(true);

            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.OK);

            testObject = mock.Create<SingleScanShipmentConfirmationService>();
        }

        [Fact]
        public void GetShipments_ThrowsShippingException_WhenUserDoesNotHavePermission()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(false);

            Task<ShippingException> result = Assert.ThrowsAsync<ShippingException>(() => testObject.GetShipments(123, "foobar"));
            Assert.Equal("Auto printing is not allowed for the scanned order.", result.Result.Message);
        }

        [Fact]
        public async void GetShipments_ReturnsEmptyShipments_WhenOrderLoaderReturnsNoShipments()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>()));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async void GetShipments_ReturnsOneShipment_WhenOrderLoaderReturnsOneUnprocessedShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() {new ShipmentEntity() {Processed = false} }));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.False(result.First().Processed);
        }


        [Fact]
        public async void GetShipments_ReturnsOneNewShipment_WhenOrderLoaderReturnsOneprocessedShipmentAndUserConfirmsAddingANewShipment()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.OK);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.False(result.First().Processed);
        }

        [Fact]
        public async void GetShipments_ReturnsNoShipment_WhenOrderLoaderReturnsOneprocessedShipmentAndUserDeclinesAddingANewShipment()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.Cancel);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async void GetShipments_ReturnsUnprocessedShipments_WhenOrderLoaderReturnsMultipleUnprocessedShipmentAndUserConfirms()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = true}
                    }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.OK);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async void GetShipments_ReturnsNoShipments_WhenOrderLoaderReturnsMultipleUnprocessedShipmentAndUserDeclines()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = true}
                    }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.Cancel);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async void GetShipments_DelegatesToSecurityContextRetriever()
        {
            Mock<ISecurityContext> securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(false);

            await Assert.ThrowsAsync<ShippingException>(() => testObject.GetShipments(123, "foobar"));

            securityContext.Verify(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123));
        }

        [Fact]
        public async void GetShipments_DelegatesToOrderLoader()
        {
            Mock<IOrderLoader> orderLoader = mock.Mock<IOrderLoader>();

            await testObject.GetShipments(123, "foobar");

            orderLoader.Verify(
                o =>
                    o.LoadAsync(new[] {123L}, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite));
        }

        [Fact]
        public async void GetShipments_DelegatesToShipmentFactory()
        {
            OrderEntity order = new OrderEntity();
            Mock<IOrderLoader> orderLoader = mock.Mock<IOrderLoader>();
            orderLoader.Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true, Order = order } }));

            await testObject.GetShipments(123, "foobar");

            mock.Mock<IShipmentFactory>().Verify(s => s.Create(order));
        }

        [Fact]
        public async void GetShipments_DelegatesToMessageHelper()
        {
            Mock<IOrderLoader> orderLoader = mock.Mock<IOrderLoader>();
            orderLoader.Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            await testObject.GetShipments(123, "foobar");

            mock.Mock<IMessageHelper>().Verify(m => m.ShowDialog(It.IsAny<Func<IForm>>()));
        }

        [Fact]
        public async void GetShipments_DelegatesToDlgFactoryWithNewShipmentMessaging_WhenOrderLoaderReturnsOneprocessedShipment()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            Mock<IAutoPrintConfirmationDlgFactory> dlgFactory = mock.Mock<IAutoPrintConfirmationDlgFactory>();

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body =
                    "The scanned Order has been previously processed. To create and print a new label, scan the barcode again or click 'Create New Label'.",
                Continue = "Create New Label",
                Title = "Order Previously Processed"
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        [Fact]
        public async void GetShipments_DelegatesToDlgFactoryWithMultiShipmentMessaging_WhenOrderLoaderReturnsMultipleUnprocessedShipment()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = true}
                    }));

            Mock<IAutoPrintConfirmationDlgFactory> dlgFactory = mock.Mock<IAutoPrintConfirmationDlgFactory>();

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body =
                    "The scanned Order has multiple shipments. To create a label for each unprocessed shipment in the order, scan the barcode again or click 'Create 3 Labels'.",
                Continue = "Create 3 Labels",
                Title = "Multiple Shipments"
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}