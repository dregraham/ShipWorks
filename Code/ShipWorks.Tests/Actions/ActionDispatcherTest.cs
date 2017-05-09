using ShipWorks.Actions;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Model.EntityClasses;
using Xunit;

namespace ShipWorks.Tests.Actions
{
    public class ActionDispatcherTest
    {

        [Theory]
        [InlineData(ActionTriggerType.ShipmentProcessed, "Ship something Print", ActionQueueType.DefaultPrint)]
        [InlineData(ActionTriggerType.ShipmentProcessed, "Not a default Print", ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.ShipmentProcessed, "Ship Print Not a default one", ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.ShipmentProcessed, null, ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.None, "FinishProcessingBatch", ActionQueueType.DefaultPrint)]
        [InlineData(ActionTriggerType.ShipmentProcessed, "FinishProcessingBatch", ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.ShipmentProcessed, "Finish Processing Batch", ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.ShipmentProcessed, "FinishProcessingBatch Something", ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.Scheduled, "Ship something Print", ActionQueueType.Scheduled)]
        [InlineData(ActionTriggerType.UserInitiated, "Ship something Print", ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.UserInitiated, null, ActionQueueType.UserInterface)]
        [InlineData(ActionTriggerType.Scheduled, null, ActionQueueType.Scheduled)]
        public void DetermineActionQueueType_ReturnsCorrectValue(ActionTriggerType actionTriggerType, string internalOwner, ActionQueueType expectedValue)
        {
            ActionEntity action = new ActionEntity()
            {
                TriggerType = (int) actionTriggerType,
                InternalOwner = internalOwner
            };

            Assert.Equal(expectedValue, ActionDispatcher.DetermineActionQueueType(action));
        }
    }
}
