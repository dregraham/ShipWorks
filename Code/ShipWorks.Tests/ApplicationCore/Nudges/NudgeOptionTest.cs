using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Nudges.Buttons;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    [TestClass]
    public class NudgeOptionTest
    {
        private readonly NudgeOption testObject;

        public NudgeOptionTest()
        {
            testObject = new NudgeOption(1, "Test Option", null, "Test Action", "Test Result");
        }

        [TestMethod]
        public void CreateButton_ReturnsAcknowledgeNudgeOptionButton_Test()
        {
            // This will change once additional button types get added
            NudgeOptionButton button = testObject.CreateButton();

            Assert.IsInstanceOfType(button, typeof(AcknowledgeNudgeOptionButton));
        }

        [TestMethod]
        public void CreateButton_SetsButtonTextToOptionText_Test()
        {
            NudgeOptionButton button = testObject.CreateButton();

            Assert.AreEqual(testObject.Text, button.Text);
        }
    }
}
