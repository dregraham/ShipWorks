using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Email.Accounts;
using ShipWorks.Tests.Shared.EntityBuilders;
using Xunit;

namespace ShipWorks.Stores.Tests.Integration.Platforms.Yahoo
{
    public partial class YahooOnlineUpdateCommandCreatorTest
    {
        private void SetupUploadShipmentDetails()
        {
            var emailAccount = Create.Entity<EmailAccountEntity>()
                .Set(x => x.OutgoingServer, "http://www.example.com")
                .Save();

            store = Modify.Store<YahooStoreEntity>(store)
                .Set(x => x.TrackingUpdatePassword, "Foo")
                .Set(x => x.YahooEmailAccountID, emailAccount.EmailAccountID)
                .Save();

            EmailAccountManager.CheckForChangesNeeded();
        }

        [Fact]
        public async Task UploadShipmentDetails_MakesOneWebRequest_WhenOrderIsNotCombined()
        {
            SetupUploadShipmentDetails();
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 2, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            emailCommunicator.Verify(x => x.StartEmailingMessages(It.Is<IEnumerable<EmailOutboundEntity>>(e => e.Count() == 1)));
        }

        [Fact]
        public async Task UploadShipmentDetails_MakesOneWebRequest_WhenOneOfTwoNonCombinedOrdersIsManual()
        {
            SetupUploadShipmentDetails();
            OrderEntity manualOrder = CreateNormalOrder(2, "track-456", true, 4);
            OrderEntity order = CreateNormalOrder(1, "track-123", false, 3);

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { manualOrder.OrderID, order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            emailCommunicator.Verify(x => x.StartEmailingMessages(It.Is<IEnumerable<EmailOutboundEntity>>(e => e.Count() == 1)));
        }

        [Fact]
        public async Task UploadShipmentDetails_MakesTwoWebRequests_WhenOrderIsCombined()
        {
            SetupUploadShipmentDetails();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, false), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            emailCommunicator.Verify(x => x.StartEmailingMessages(It.Is<IEnumerable<EmailOutboundEntity>>(e => e.Count() == 2)));
        }

        [Fact]
        public async Task UploadShipmentDetails_MakesOneWebRequest_WhenOrderIsCombinedAndOneIsManual()
        {
            SetupUploadShipmentDetails();
            OrderEntity order = CreateCombinedOrder(1, "track-123", Tuple.Create(2, true), Tuple.Create(3, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { order.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            emailCommunicator.Verify(x => x.StartEmailingMessages(It.Is<IEnumerable<EmailOutboundEntity>>(e => e.Count() == 1)));
        }

        [Fact]
        public async Task UploadShipmentDetails_MakesThreeWebRequests_WhenBothCombinedAndNonCombinedAreUploaded()
        {
            SetupUploadShipmentDetails();
            OrderEntity normalOrder = CreateNormalOrder(1, "track-123", false, 2, 3);
            OrderEntity combinedOrder = CreateCombinedOrder(4, "track-456", Tuple.Create(5, false), Tuple.Create(6, false));

            menuContext.SetupGet(x => x.SelectedKeys).Returns(new[] { normalOrder.OrderID, combinedOrder.OrderID });

            await commandCreator.OnUploadShipmentDetails(menuContext.Object);

            emailCommunicator.Verify(x => x.StartEmailingMessages(It.Is<IEnumerable<EmailOutboundEntity>>(e => e.Count() == 3)));
        }
    }
}