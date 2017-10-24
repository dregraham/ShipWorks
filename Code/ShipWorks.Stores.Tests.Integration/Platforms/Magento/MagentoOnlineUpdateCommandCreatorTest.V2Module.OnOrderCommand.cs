using System;
using System.Threading.Tasks;
using Moq;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.Tests.Shared;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Magento
{
    public partial class MagentoOnlineUpdateCommandCreatorTest
    {
        private void SetupV2Module()
        {
            store = Modify.Store(store)
                .Set(x => x.MagentoTrackingEmails, true)
                .Set(x => x.ModuleOnlineStatusDataType, (int) GenericVariantDataType.Text)
                .Set(x => x.ModuleOnlineStatusSupport, (int) GenericOnlineStatusSupport.StatusOnly)
                .Set(x => x.MagentoVersion, (int) MagentoVersion.MagentoTwo)
                .Save();

            MagentoUploadAction uploadAction = new MagentoUploadAction()
            {
                OrderNumber = AnyLong,
                Action = AnyString,
                Comments = AnyString,
                Carrier = AnyString,
                TrackingNumber = AnyString,
                SendEmail = AnyBool
            };

            webClientModule.Setup(x => x.ExecuteAction(uploadAction))
                .Returns("Bar");
        }

        [Fact]
        public async Task V2Module_OrderCommand_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            SetupV2Module();
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            MagentoUploadAction uploadAction = new MagentoUploadAction()
            {
                OrderNumber = 10000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-123",
                SendEmail = true
            };

            webClientModule.Verify(x => x.ExecuteAction(uploadAction));
        }

        [Fact]
        public async Task V2Module_OrderCommand_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            SetupV2Module();
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            MagentoUploadAction uploadActionOne = new MagentoUploadAction()
            {
                OrderNumber = 20000,
                Action = AnyString,
                Comments = AnyString,
                Carrier = AnyString,
                TrackingNumber = AnyString,
                SendEmail = AnyBool
            };

            MagentoUploadAction uploadActionTwo = new MagentoUploadAction()
            {
                OrderNumber = 10000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-123",
                SendEmail = true
            };

            webClientModule.Verify(x => x.ExecuteAction(uploadActionOne));
            webClientModule.Verify(x => x.ExecuteAction(uploadActionTwo));
        }

        [Fact]
        public async Task V2Module_OrderCommand_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            SetupV2Module();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            MagentoUploadAction uploadActionOne = new MagentoUploadAction()
            {
                OrderNumber = 20000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-123",
                SendEmail = true
            };

            MagentoUploadAction uploadActionTwo = new MagentoUploadAction()
            {
                OrderNumber = 30000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-123",
                SendEmail = true
            };

            webClientModule.Verify(x => x.ExecuteAction(uploadActionOne));
            webClientModule.Verify(x => x.ExecuteAction(uploadActionTwo));
        }

        [Fact]
        public async Task V2Module_OrderCommand_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            SetupV2Module();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            MagentoUploadAction uploadActionOne = new MagentoUploadAction()
            {
                OrderNumber = 20000,
                Action = AnyString,
                Comments = AnyString,
                Carrier = AnyString,
                TrackingNumber = AnyString,
                SendEmail = AnyBool
            };

            MagentoUploadAction uploadActionTwo = new MagentoUploadAction()
            {
                OrderNumber = 30000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-123",
                SendEmail = true
            };

            webClientModule.Verify(x => x.ExecuteAction(uploadActionOne));
            webClientModule.Verify(x => x.ExecuteAction(uploadActionTwo));
        }

        [Fact]
        public async Task V2Module_OrderCommand_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            SetupV2Module();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            MagentoUploadAction uploadActionOne = new MagentoUploadAction()
            {
                OrderNumber = 10000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-123",
                SendEmail = true
            };

            MagentoUploadAction uploadActionTwo = new MagentoUploadAction()
            {
                OrderNumber = 50000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-456",
                SendEmail = true
            };

            MagentoUploadAction uploadActionThree = new MagentoUploadAction()
            {
                OrderNumber = 60000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-456",
                SendEmail = true
            };

            webClientModule.Verify(x => x.ExecuteAction(uploadActionOne));
            webClientModule.Verify(x => x.ExecuteAction(uploadActionTwo));
            webClientModule.Verify(x => x.ExecuteAction(uploadActionThree));
        }

        [Fact]
        public async Task V2Module_OrderCommand_ContinuesUploading_WhenFailuresOccur()
        {
            SetupV2Module();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));
            OrderEntity normalOrder2 = CreateNormalOrder(7, "track-789", false, 8);

            webClientRest.Setup(x => x.UploadShipmentDetails(AnyString, AnyString, 10000))
                .Throws(new MagentoException("Foo"));
            webClientRest.Setup(x => x.UploadShipmentDetails(AnyString, AnyString, 50000))
                .Throws(new MagentoException("Foo"));

            menuContext.SetupGet(x => x.MenuCommand).Returns(context.Mock.CreateMock<IMenuCommand>(x => x.Setup(z => z.Tag).Returns(MagentoUploadCommand.Complete)));
            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID, normalOrder2.OrderID });

            await commandCreator.OnOrderCommand(menuContext.Object, store);

            MagentoUploadAction uploadActionOne = new MagentoUploadAction()
            {
                OrderNumber = 60000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-456",
                SendEmail = true
            };

            MagentoUploadAction uploadActionTwo = new MagentoUploadAction()
            {
                OrderNumber = 70000,
                Action = "Complete",
                Comments = string.Empty,
                Carrier = "other|Foo Bar",
                TrackingNumber = "track-789",
                SendEmail = true
            };

            webClientModule.Verify(x => x.ExecuteAction(uploadActionOne));
            webClientModule.Verify(x => x.ExecuteAction(uploadActionTwo));
        }
    }
}