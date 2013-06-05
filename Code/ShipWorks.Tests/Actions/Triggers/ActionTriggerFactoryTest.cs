using ShipWorks.Actions.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ShipWorks.Tests.Actions.Triggers
{
    [TestClass]
    public class ActionTriggerFactoryTest
    {
        [TestMethod]
        public void CreateTrigger_ReturnsOrderDownloadedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.OrderDownloaded, null);

            Assert.IsInstanceOfType(trigger, typeof(OrderDownloadedTrigger));
        }

        [TestMethod]
        public void CreateTrigger_ReturnsDownloadFinishedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.DownloadFinished, null);

            Assert.IsInstanceOfType(trigger, typeof(DownloadFinishedTrigger));
        }

        [TestMethod]
        public void CreateTrigger_ReturnsShipmentProcessedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.ShipmentProcessed, null);

            Assert.IsInstanceOfType(trigger, typeof(ShipmentProcessedTrigger));
        }

        [TestMethod]
        public void CreateTrigger_ReturnsShipmentVoidedTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.ShipmentVoided, null);

            Assert.IsInstanceOfType(trigger, typeof(ShipmentVoidedTrigger));
        }

        [TestMethod]
        public void CreateTrigger_ReturnsFilterContentTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.FilterContentChanged, null);

            Assert.IsInstanceOfType(trigger, typeof(FilterContentTrigger));
        }

        [TestMethod]
        public void CreateTrigger_ReturnsCronTrigger_Test()
        {
            ActionTrigger trigger = ActionTriggerFactory.CreateTrigger(ActionTriggerType.Cron, null);

            Assert.IsInstanceOfType(trigger, typeof(CronTrigger));
        }

    }
}
