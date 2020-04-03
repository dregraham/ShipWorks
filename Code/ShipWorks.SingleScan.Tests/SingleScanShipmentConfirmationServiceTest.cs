﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac.Extras.Moq;
using Interapptive.Shared.Metrics;
using Interapptive.Shared.UI;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Settings;
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
        public async Task GetShipments_ReturnsOneShipment_WhenOrderLoaderReturnsNoShipments()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>()));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Equal(1, result.Count());
        }

        [Fact]
        public async Task GetShipments_ReturnsNewShipment_WhenOrderLoaderReturnsOnlyVoidedShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity> { new ShipmentEntity(10) { Voided = true } }));

            IEnumerable<ShipmentEntity> result = (await testObject.GetShipments(123, "foobar")).ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal(0, result.Single().ShipmentID);
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipments_WhenOrderLoaderReturnsNoShipments_AndShipmentFactoryCreatesNoneShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>()));

            mock.Mock<IShipmentFactory>()
                .Setup(f => f.Create(It.IsAny<long>()))
                .Returns(new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.None });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
            mock.Mock<IMessageHelper>()
                .Verify(m => m.ShowError(SingleScanShipmentConfirmationService.CannotProcessNoneMessage));
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipments_WhenOrderLoaderReturnsMultipleShipments_AndOneShipmentIsNone()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>
                    {
                        new ShipmentEntity(),
                        new ShipmentEntity(),
                        new ShipmentEntity(),
                        new ShipmentEntity {ShipmentTypeCode = ShipmentTypeCode.None}
                    }));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            mock.Mock<IShipmentFactory>().Verify(f => f.Create(It.IsAny<long>()), Times.Never);
            Assert.Empty(result);
            mock.Mock<IMessageHelper>()
                .Verify(m => m.ShowError(SingleScanShipmentConfirmationService.CannotProcessNoneMessage));
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipments_WhenShipmentHasOneProcessedShipment_AndUserConfirmsTheyWantToShip_AndShipmentFactoryReturnsNoneShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>
                    {
                        new ShipmentEntity() {Processed = true}
                    }));

            mock.Mock<IShipmentFactory>()
                .Setup(f => f.Create(It.IsAny<long>()))
                .Returns(new ShipmentEntity { ShipmentTypeCode = ShipmentTypeCode.None });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
            mock.Mock<IMessageHelper>()
                .Verify(m => m.ShowError(SingleScanShipmentConfirmationService.CannotProcessNoneMessage));
        }

        [Fact]
        public async Task GetShipments_ReturnsOneShipment_WhenOrderLoaderReturnsOneUnprocessedShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = false } }));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.False(result.First().Processed);
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipments_WhenOrderLoaderReturnsOneNoneUnprocessedShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity> { new ShipmentEntity() { Processed = false, ShipmentTypeCode = ShipmentTypeCode.None } }));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetShipments_DisplaysCannotProcessNoneMessage_WhenOrderLoaderReturnsOneNoneUnprocessedShipment()
        {
            mock.Mock<ISecurityContext>()
                .Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(true);

            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity> { new ShipmentEntity() { Processed = false, ShipmentTypeCode = ShipmentTypeCode.None } }));

            await testObject.GetShipments(123, "foobar");

            mock.Mock<IMessageHelper>().Verify(h => h.ShowError(SingleScanShipmentConfirmationService.CannotProcessNoneMessage));
        }

        [Fact]
        public async Task GetShipments_ReturnsOneNewShipment_WhenOrderLoaderReturnsOneProcessedShipmentAndUserConfirmsAddingANewShipment()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.OK);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.False(result.First().Processed);
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipment_WhenOrderLoaderReturnsOneProcessedShipmentAndUserDeclinesAddingANewShipment()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.Cancel);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetShipments_ReturnsUnprocessedShipments_WhenOrderLoaderReturnsMultipleUnprocessedShipmentAndUserConfirms()
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
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipments_WhenOrderLoaderReturnsMultipleUnprocessedShipmentAndUserDeclines()
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
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetShipments_ReturnsNoShipments_WhenOrderLoaderReturnsAMultiPackageShipment_AndAutoWeighIsOn_AndUserDeclines()
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .SetupGet(w => w.IsAutoWeighEnabled)
                .Returns(true);

            MockShipmentAdapterToReturnTwoPackages();

            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>() { new ShipmentEntity() { Processed = false } }));

            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.Cancel);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetShipments_ReturnsShipment_WhenOrderLoaderReturnsAMultiPackageShipment_AndAutoWeighIsOn_AndUserAccepts()
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .SetupGet(w => w.IsAutoWeighEnabled)
                .Returns(true);

            MockShipmentAdapterToReturnTwoPackages();

            // Mock up the order loader to return one processed shipment
            var shipment = new ShipmentEntity() { Processed = false };
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>() { shipment }));

            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns(DialogResult.OK);

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            Assert.Equal(shipment, result.Single());
        }

        [Fact]
        public async Task GetShipments_ReturnsShipment_WhenOrderLoaderReturnsAMultiPackageShipment_AndAutoWeighIsOff()
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .SetupGet(w => w.IsAutoWeighEnabled)
                .Returns(false);

            MockShipmentAdapterToReturnTwoPackages();

            var shipment = new ShipmentEntity() { Processed = false };
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>() { shipment }));

            IEnumerable<ShipmentEntity> result = await testObject.GetShipments(123, "foobar");

            mock.Mock<IMessageHelper>().Verify((m => m.ShowDialog(It.IsAny<Func<IForm>>())), Times.Never);
            Assert.Equal(shipment, result.Single());
        }

        [Fact]
        public async Task GetShipments_DelegatesToSecurityContextRetriever()
        {
            Mock<ISecurityContext> securityContext = mock.Mock<ISecurityContext>();
            securityContext.Setup(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123))
                .Returns(false);

            await Assert.ThrowsAsync<ShippingException>(() => testObject.GetShipments(123, "foobar"));

            securityContext.Verify(s => s.HasPermission(PermissionType.ShipmentsCreateEditProcess, 123));
        }

        [Fact]
        public async Task GetShipments_DelegatesToOrderLoader()
        {
            Mock<IOrderLoader> orderLoader = mock.Mock<IOrderLoader>();

            await testObject.GetShipments(123, "foobar");

            orderLoader.Verify(
                o =>
                    o.LoadAsync(new[] { 123L }, ProgressDisplayOptions.NeverShow, true, Timeout.Infinite));
        }

        [Fact]
        public async Task GetShipments_DelegatesToShipmentFactory()
        {
            OrderEntity order = new OrderEntity();
            Mock<IOrderLoader> orderLoader = mock.Mock<IOrderLoader>();
            orderLoader.Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true, Order = order, ShipmentTypeCode = ShipmentTypeCode.FedEx } }));

            await testObject.GetShipments(123, "foobar");

            mock.Mock<IShipmentFactory>().Verify(s => s.Create(123));
        }

        [Fact]
        public async Task GetShipments_DelegatesToMessageHelper()
        {
            Mock<IOrderLoader> orderLoader = mock.Mock<IOrderLoader>();
            orderLoader.Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = true } }));

            await testObject.GetShipments(123, "foobar");

            mock.Mock<IMessageHelper>().Verify(m => m.ShowDialog(It.IsAny<Func<IForm>>()));
        }

        [Fact]
        public async Task GetShipments_DelegatesToDlgFactoryWithNewShipmentMessaging_WhenOrderLoaderReturnsOneProcessedShipment()
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
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body = "The scanned order has been previously processed. Select the action you would like to take:",
                Continue = "Create New Label",
                Title = "Order Previously Processed",
                ContinueOptional = "Reprint Existing Label"
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        [Fact]
        public async Task GetShipments_DelegatesToDlgFactoryWithMultiplePackageMessaging_WhenOrderLoaderReturnsOneShipmentWithMultiplePackages()
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .SetupGet(w => w.IsAutoWeighEnabled)
                .Returns(true);

            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = false } }));

            MockShipmentAdapterToReturnTwoPackages();

            Mock<IAutoPrintConfirmationDlgFactory> dlgFactory = mock.Mock<IAutoPrintConfirmationDlgFactory>();

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Title = "Multiple Packages",
                Body = "The resulting shipment has multiple packages. To create a label for each package, " +
                       "scan the barcode again or click \'Print 2 Labels\'." +
                       "\r\n\r\nNote: ShipWorks will update each package with the weight from the scale.",
                Continue = "Print 2 Labels",
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        private void MockShipmentAdapterToReturnTwoPackages()
        {
            var packageAdapter = mock.Mock<IPackageAdapter>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);
        }

        [Fact]
        public async Task GetShipments_DoesNotDelegateToDlgFactoryWithMultiplePackageMessaging_WhenOrderLoaderReturnsOneShipmentWithMultiplePackages_AndAutoWeighIsOff()
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .SetupGet(w => w.IsAutoWeighEnabled)
                .Returns(false);

            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "", new List<ShipmentEntity>() { new ShipmentEntity() { Processed = false } }));

            var packageAdapter = mock.Mock<IPackageAdapter>();

            var shipmentAdapter = mock.Mock<ICarrierShipmentAdapter>();
            shipmentAdapter.Setup(a => a.GetPackageAdaptersAndEnsureShipmentIsLoaded())
                .Returns(new List<IPackageAdapter> { packageAdapter.Object, packageAdapter.Object });

            mock.Mock<ICarrierShipmentAdapterFactory>().Setup(x => x.Get(It.IsAny<ShipmentEntity>()))
                .Returns(shipmentAdapter.Object);

            Mock<IAutoPrintConfirmationDlgFactory> dlgFactory = mock.Mock<IAutoPrintConfirmationDlgFactory>();

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            await testObject.GetShipments(123, "foobar");

            dlgFactory.Verify(f => f.Create("foobar", It.IsAny<MessagingText>()), Times.Never);
        }

        [Fact]
        public async Task GetShipments_DelegatesToDlgFactoryWithMultiShipmentMessaging_WhenOrderLoaderReturnsMultipleUnprocessedShipment()
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
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body = "The scanned order has multiple shipments. Select the action you would like to take:",
                Continue = "Create 3 Labels",
                Title = "Multiple Shipments",
                ContinueOptional = "Reprint Existing Label"
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        [Fact]
        public async Task GetShipments_DelegatesToDlgFactoryWithAutoWeighMessage_WhenOrderLoaderReturnsMultipleUnprocessedShipment_AndAutoWeighIsOn()
        {
            mock.Mock<ISingleScanAutomationSettings>()
                .SetupGet(w => w.IsAutoWeighEnabled)
                .Returns(true);

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
                .Returns(new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body =
                    "The scanned order has multiple shipments. Select the action you would like to take:" +
                    "\r\n\r\nNote: If 'Create 3 Labels' is selected ShipWorks will update each shipment with the weight from the scale.",
                Continue = "Create 3 Labels",
                Title = "Multiple Shipments",
                ContinueOptional = "Reprint Existing Label"
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        [Fact]
        public async Task GetShipments_DelegatesToDialogFactory_WhenConfirmationModeSetToPrintExisting_AndThereAreNoProcessedShipments()
        {
            // Mock up the order loader to return one un-processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = false},
                        new ShipmentEntity() {Processed = false},
                    }));
            Mock<IAutoPrintConfirmationDlgFactory> dlgFactory = mock.Mock<IAutoPrintConfirmationDlgFactory>();
            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(s => s.ConfirmationMode)
                .Returns(SingleScanConfirmationMode.PrintExisting);
            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body =
                    "The scanned order has multiple shipments. Select the action you would like to take:",
                Continue = "Create 2 Labels",
                Title = "Multiple Shipments",
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        [Fact]
        public async Task GetShipments_ReturnsOnlyProcessedShipments_WhenConfirmationModeSetToPrintExisting()
        {
            // Mock up the order loader to return one un-processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = true},
                        new ShipmentEntity() {Processed = true},
                        new ShipmentEntity() {Processed = false},
                    }));
            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(s => s.ConfirmationMode)
                .Returns(SingleScanConfirmationMode.PrintExisting);

            var shipments = await testObject.GetShipments(123, "foobar");
            Assert.True(shipments.All(s => s.Processed == true));
        }

        [Fact]
        public async Task GetShipments_DelegatesToDialogFactoryWithCorrectMessageText_WhenConfirmationModeSetToSelect()
        {
            // Mock up the order loader to return one un-processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = true},
                        new ShipmentEntity() {Processed = true},
                    }));
            Mock<IAutoPrintConfirmationDlgFactory> dlgFactory = mock.Mock<IAutoPrintConfirmationDlgFactory>();
            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(s => s.ConfirmationMode)
                .Returns(SingleScanConfirmationMode.Select);
            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            await testObject.GetShipments(123, "foobar");

            MessagingText continueText = new MessagingText()
            {
                Body =
                    "The scanned order has been previously processed. Select the action you would like to take:",
                Continue = "Create New Label",
                Title = "Order Previously Processed",
                ContinueOptional = "Reprint Existing Label"
            };

            dlgFactory.Verify(f => f.Create("foobar", continueText));
        }

        [Fact]
        public async Task GetShipments_ReturnsNewShipment_WhenAllShipmentsAreProcessed_AndConfirmationModeSetToCreateNew()
        {
            // Mock up the order loader to return one un-processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = true},
                        new ShipmentEntity() {Processed = true},
                    }));
            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(s => s.ConfirmationMode)
                .Returns(SingleScanConfirmationMode.CreateNew);

            var newShipment = new ShipmentEntity { Processed = false, ShipmentTypeCode = ShipmentTypeCode.FedEx };
            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<long>()))
                .Returns(newShipment);

            var result = await testObject.GetShipments(123, "foobar");
            Assert.True(result.Count() == 1);
            Assert.Equal(result.First(), newShipment);
        }

        [Fact]
        public async Task GetShipments_ReturnsUnprocessedShipments_WhenSomeShipmentsAreUnprocessed_AndConfirmationModeSetToCreateNew()
        {
            var unprocessedShipment = new ShipmentEntity() { Processed = false };
            // Mock up the order loader to return one un-processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = true},
                        unprocessedShipment,
                        new ShipmentEntity() {Processed = true},
                    }));
            mock.Mock<ISingleScanAutomationSettings>()
                .Setup(s => s.ConfirmationMode)
                .Returns(SingleScanConfirmationMode.CreateNew);

            var result = await testObject.GetShipments(123, "foobar");
            Assert.True(result.Count() == 1);
            Assert.Equal(result.First(), unprocessedShipment);
        }

        [Fact]
        public async Task GetShipments_AddsTotalShipmentsTelemetry_WhenAutoPrintEnabledAndMultipleShipmentsReturned()
        {
            var trackedDurationEvent = await SetupTelemetryTests();

            trackedDurationEvent.Verify(
                e => e.AddMetric("SingleScan.AutoPrint.Confirmation.MultipleShipments.Total", 4), Times.Once);
        }

        [Fact]
        public async Task GetShipments_AddsUnprocessedShipmentsTelemetry_WhenAutoPrintEnabledAndMultipleShipmentsReturned()
        {
            var trackedDurationEvent = await SetupTelemetryTests();

            trackedDurationEvent.Verify(
                e => e.AddMetric("SingleScan.AutoPrint.Confirmation.MultipleShipments.Unprocessed", 3), Times.Once);
        }

        [Fact]
        public async Task GetShipments_AddsProcessedShipmentsTelemetry_WhenAutoPrintEnabledAndMultipleShipmentsReturned()
        {
            var trackedDurationEvent = await SetupTelemetryTests();

            trackedDurationEvent.Verify(
                e => e.AddMetric("SingleScan.AutoPrint.Confirmation.MultipleShipments.Processed", 1), Times.Once);
        }

        [Fact]
        public async Task GetShipments_AddsShipmentConfirmationActionTelemetry_WhenAutoPrintEnabledAndMultipleShipmentsReturned()
        {
            var trackedDurationEvent = await SetupTelemetryTests();

            trackedDurationEvent.Verify(
                e => e.AddProperty("SingleScan.AutoPrint.Confirmation.MultipleShipments.Action", "Cancel"), Times.Once);
        }

        private async Task<Mock<ITrackedDurationEvent>> SetupTelemetryTests()
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
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            await testObject.GetShipments(123, "barcode");
            return trackedDurationEvent;
        }

        [Fact]
        public async Task GetShipments_DoesNotAddTelemetry_WhenAutoPrintEnabledAndOnlyOneShipmentReturned()
        {
            // Mock up the order loader to return one processed shipment
            mock.Mock<IOrderLoader>()
                .Setup(o => o.LoadAsync(It.IsAny<long[]>(), ProgressDisplayOptions.NeverShow, true, Timeout.Infinite))
                .ReturnsAsync(new ShipmentsLoadedEventArgs(null, false, "",
                    new List<ShipmentEntity>()
                    {
                        new ShipmentEntity() {Processed = false},
                    }));

            // Mock up the message helper to return DialogResult OK
            mock.Mock<IMessageHelper>().Setup(m => m.ShowDialog(It.IsAny<Func<IForm>>())).Returns<Func<IForm>>(f =>
            {
                IForm form = f();
                return form.ShowDialog(null);
            });

            mock.Mock<IShipmentFactory>()
                .Setup(s => s.Create(It.IsAny<OrderEntity>()))
                .Returns(new ShipmentEntity() { Processed = false });

            var trackedDurationEvent = mock.Mock<ITrackedDurationEvent>();
            mock.MockFunc<string, ITrackedDurationEvent>(trackedDurationEvent);

            await testObject.GetShipments(123, "barcode");

            trackedDurationEvent.Verify(e => e.AddMetric(It.IsAny<string>(), It.IsAny<double>()), Times.Never);
            trackedDurationEvent.Verify(e => e.AddProperty(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}