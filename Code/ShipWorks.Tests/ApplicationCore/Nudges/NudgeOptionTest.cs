using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Nudges.Buttons;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    [TestClass]
    public class NudgeOptionTest
    {
        private NudgeOption testObject;

        public NudgeOptionTest()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, NudgeOptionActionType.None, "Test Result");
        }

        [TestMethod]
        public void CreateButton_ReturnsAcknowledgeNudgeOptionButton_WhenActionTypeIsNone_Test()
        {
            NudgeOptionButton button = testObject.CreateButton();

            Assert.IsInstanceOfType(button, typeof(AcknowledgeNudgeOptionButton));
        }

        [TestMethod]
        public void CreateButton_ReturnsAcknowledgeNudgeOptionButton_WhenActionTypeIsShutDown_Test()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, NudgeOptionActionType.Shutdown, "Test Result");

            NudgeOptionButton button = testObject.CreateButton();

            Assert.IsInstanceOfType(button, typeof(ShutdownNudgeOptionButton));
        }

        [TestMethod]
        [ExpectedException(typeof(NudgeException))]
        public void CreateButton_ThrowsNudgeException_WhenActionTypeIsNotRecognized_Test()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, (NudgeOptionActionType)900, "Test Result");
            
            testObject.CreateButton();
        }

        [TestMethod]
        public void CreateButton_SetsButtonTextToOptionText_Test()
        {
            NudgeOptionButton button = testObject.CreateButton();

            Assert.AreEqual(testObject.Text, button.Text);
        }
    }
}
