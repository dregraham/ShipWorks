using Xunit;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Nudges.Buttons;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    public class NudgeOptionTest
    {
        private NudgeOption testObject;

        public NudgeOptionTest()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, NudgeOptionActionType.None);
        }

        [Fact]
        public void CreateButton_ReturnsAcknowledgeNudgeOptionButton_WhenActionTypeIsNone_Test()
        {
            NudgeOptionButton button = testObject.CreateButton();

            Assert.IsAssignableFrom<AcknowledgeNudgeOptionButton>(button);
        }

        [Fact]
        public void CreateButton_ReturnsAcknowledgeNudgeOptionButton_WhenActionTypeIsShutDown_Test()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, NudgeOptionActionType.Shutdown);

            NudgeOptionButton button = testObject.CreateButton();

            Assert.IsAssignableFrom<ShutdownNudgeOptionButton>(button);
        }

        [Fact]
        public void CreateButton_ReturnsAcknowledgeNudgeOptionButton_WhenActionTypeIsRegisterUspsAccount_Test()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, NudgeOptionActionType.RegisterUspsAccount);

            NudgeOptionButton button = testObject.CreateButton();

            Assert.IsAssignableFrom<RegisterUspsAccountNudgeOptionButton>(button);
        }

        [Fact]
        public void CreateButton_ThrowsNudgeException_WhenActionTypeIsNotRecognized_Test()
        {
            testObject = new NudgeOption(1, 1, "Test Option", null, (NudgeOptionActionType)900);

            Assert.Throws<NudgeException>(() => testObject.CreateButton());
        }

        [Fact]
        public void CreateButton_SetsButtonTextToOptionText_Test()
        {
            NudgeOptionButton button = testObject.CreateButton();

            Assert.Equal(testObject.Text, button.Text);
        }
    }
}
