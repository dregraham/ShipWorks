using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Interaction;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.OnlineUpdating;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Yahoo.OnlineUpdating
{
    public class OnlineUpdateCommandCreatorTest : IDisposable
    {
        readonly AutoMock mock;

        public OnlineUpdateCommandCreatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_OnlyReturnsUploadShipmentDetailsCommand_WhenEmailUser()
        {
            var testObject = mock.Create<OnlineUpdateCommandCreator>();
            IEnumerable<IMenuCommand> commands = testObject.CreateOnlineUpdateInstanceCommands(new YahooStoreEntity { StoreTypeCode = StoreTypeCode.Yahoo });

            Assert.Equal(1, commands.Count());

            Assert.Equal("Upload Shipment Details", commands.FirstOrDefault()?.Text);
        }

        [Fact]
        public void CreateOnlineUpdateInstanceCommands_ReturnsUploadShipmentDetailsAndUpdateStatusCommands_WhenApiUser()
        {
            var testObject = mock.Create<OnlineUpdateCommandCreator>();
            var store = new YahooStoreEntity { StoreTypeCode = StoreTypeCode.Yahoo, YahooStoreID = "ysht-123456789" };
            List<string> commandNames = testObject.CreateOnlineUpdateInstanceCommands(store)
                .Select(command => command.Text).ToList();

            Assert.Equal(7, commandNames.Count);

            foreach (EnumEntry<YahooApiOrderStatus> orderStatus in EnumHelper.GetEnumList<YahooApiOrderStatus>().
                Where(orderStatus => orderStatus.Value != YahooApiOrderStatus.PartiallyShipped &&
                orderStatus.Value != YahooApiOrderStatus.FullyShipped
                && orderStatus.Value != YahooApiOrderStatus.Tracked))
            {
                Assert.Contains(orderStatus.Description, commandNames);
            }

            Assert.Contains("Upload Shipment Details", commandNames);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
