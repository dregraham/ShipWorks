using ShipWorks.Actions.Triggers;
using Xunit;

namespace ShipWorks.Tests.Actions.Triggers
{
    public class ActionTriggerFactoryTest
    {
        [Fact]
        public void CreateTrigger_ReturnsOrderDownloadedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.OrderDownloaded, null);

            Assert.IsInstanceOfType(trigger, typeof(OrderDownloadedTrigger));
        }

        [Fact]
        public void CreateTrigger_ReturnsDownloadFinishedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.DownloadFinished, null);

            Assert.IsInstanceOfType(trigger, typeof(DownloadFinishedTrigger));
        }

        [Fact]
        public void CreateTrigger_ReturnsShipmentProcessedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.ShipmentProcessed, null);

            Assert.IsInstanceOfType(trigger, typeof(ShipmentProcessedTrigger));
        }

        [Fact]
        public void CreateTrigger_ReturnsShipmentVoidedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.ShipmentVoided, null);

            Assert.IsInstanceOfType(trigger, typeof(ShipmentVoidedTrigger));
        }

        [Fact]
        public void CreateTrigger_ReturnsFilterContentTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.FilterContentChanged, null);

            Assert.IsInstanceOfType(trigger, typeof(FilterContentTrigger));
        }

        [Fact]
        public void CreateTrigger_ReturnsScheduledTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.Scheduled, null);

            Assert.IsInstanceOfType(trigger, typeof(ScheduledTrigger));
        }

    }
}
