using ShipWorks.Actions.Triggers;
using Xunit;

namespace ShipWorks.Tests.Actions.Triggers
{
    public class ActionTriggerFactoryTest
    {
        [Fact]
        public void CreateTrigger_ReturnsOrderDownloadedTrigger()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.OrderDownloaded, null);

            Assert.IsAssignableFrom<OrderDownloadedTrigger>(trigger);
        }

        [Fact]
        public void CreateTrigger_ReturnsDownloadFinishedTrigger()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.DownloadFinished, null);

            Assert.IsAssignableFrom<DownloadFinishedTrigger>(trigger);
        }

        [Fact]
        public void CreateTrigger_ReturnsShipmentProcessedTrigger()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.ShipmentProcessed, null);

            Assert.IsAssignableFrom<ShipmentProcessedTrigger>(trigger);
        }

        [Fact]
        public void CreateTrigger_ReturnsShipmentVoidedTrigger()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.ShipmentVoided, null);

            Assert.IsAssignableFrom<ShipmentVoidedTrigger>(trigger);
        }

        [Fact]
        public void CreateTrigger_ReturnsFilterContentTrigger()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.FilterContentChanged, null);

            Assert.IsAssignableFrom<FilterContentTrigger>(trigger);
        }

        [Fact]
        public void CreateTrigger_ReturnsScheduledTrigger()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.Scheduled, null);

            Assert.IsAssignableFrom<ScheduledTrigger>(trigger);
        }

    }
}
