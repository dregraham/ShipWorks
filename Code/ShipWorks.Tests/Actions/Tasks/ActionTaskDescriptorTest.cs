using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;

namespace ShipWorks.Tests.Actions.Tasks
{
    [TestClass]
    public class ActionTaskDescriptorTest
    {
        [TestMethod]
        public void IsAllowedForTrigger_ReturnsTrue_WithScheduledTypeAndOnlyScheduledIsAllowed_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(SchedulableTaskTest));

            Assert.IsTrue(testObject.IsAllowedForTrigger(ActionTriggerType.Scheduled));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsFalse_WithScheduledTypeAndOnlyNonScheduledIsAllowed_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(NonSchedulableTaskTest));

            Assert.IsFalse(testObject.IsAllowedForTrigger(ActionTriggerType.Scheduled));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsTrue_WithScheduledTypeAndDefaultAllowance_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(DefaultTaskTest));

            Assert.IsTrue(testObject.IsAllowedForTrigger(ActionTriggerType.Scheduled));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsTrue_WithScheduledTypeAndAllAllowances_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(AllTaskTest));

            Assert.IsTrue(testObject.IsAllowedForTrigger(ActionTriggerType.Scheduled));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsTrue_WithNonScheduledTypeAndOnlyScheduledIsAllowed_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(SchedulableTaskTest));

            Assert.IsFalse(testObject.IsAllowedForTrigger(ActionTriggerType.DownloadFinished));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsFalse_WithNonScheduledTypeAndOnlyNonScheduledIsAllowed_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(NonSchedulableTaskTest));

            Assert.IsTrue(testObject.IsAllowedForTrigger(ActionTriggerType.DownloadFinished));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsTrue_WithNonScheduledTypeAndDefaultAllowance_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(DefaultTaskTest));

            Assert.IsTrue(testObject.IsAllowedForTrigger(ActionTriggerType.DownloadFinished));
        }

        [TestMethod]
        public void IsAllowedForTrigger_ReturnsTrue_WithNonScheduledTypeAndAllAllowances_Test()
        {
            ActionTaskDescriptor testObject = new ActionTaskDescriptor(typeof(AllTaskTest));

            Assert.IsTrue(testObject.IsAllowedForTrigger(ActionTriggerType.DownloadFinished));
        }

        #region Support classes
        [ActionTask("Schedulable Task", "SchedulableTaskTest", ActionTaskCategory.Administration, ActionTriggerClassifications.Scheduled)]
        private class SchedulableTaskTest : ActionTask
        {
            public override ActionTaskEditor CreateEditor()
            {
                throw new NotImplementedException();
            }
        }

        [ActionTask("Non Schedulable Task", "NonSchedulableTaskTest", ActionTaskCategory.Administration, ActionTriggerClassifications.Nonscheduled)]
        private class NonSchedulableTaskTest : ActionTask
        {
            public override ActionTaskEditor CreateEditor()
            {
                throw new NotImplementedException();
            }
        }

        [ActionTask("Default Task", "DefaultTaskTest", ActionTaskCategory.Administration)]
        private class DefaultTaskTest : ActionTask
        {
            public override ActionTaskEditor CreateEditor()
            {
                throw new NotImplementedException();
            }
        }

        [ActionTask("All Task", "AllTaskTest", ActionTaskCategory.Administration, ActionTriggerClassifications.Nonscheduled | ActionTriggerClassifications.Scheduled)]
        private class AllTaskTest : ActionTask
        {
            public override ActionTaskEditor CreateEditor()
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
