using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Xunit;
using ShipWorks.ApplicationCore.Nudges;
using ShipWorks.ApplicationCore.Nudges.Buttons;

namespace ShipWorks.Tests.ApplicationCore.Nudges
{
    public class NudgeTest
    {
        private readonly Nudge testObject;

        public NudgeTest()
        {
            testObject = new Nudge(1, "Nudge 1", NudgeType.ShipWorksUpgrade, new Uri("http://www.shipworks.com"), new Size(625, 500));

            testObject.AddNudgeOption(new NudgeOption(1, 2, "Two", testObject, NudgeOptionActionType.None));
            testObject.AddNudgeOption(new NudgeOption(2, 0, "Zero", testObject, NudgeOptionActionType.None));
            testObject.AddNudgeOption(new NudgeOption(3, 3, "A really long string for a button", testObject, NudgeOptionActionType.None));
            testObject.AddNudgeOption(new NudgeOption(4, 1, "One", testObject, NudgeOptionActionType.None));
        }

        [Fact]
        public void CreateButtons_OrdersButtonsByOptionIndex()
        {
            List<NudgeOptionButton> buttons = testObject.CreateButtons();

            // Kind of a cheap test for sort order, but we don't have access to the underlying nudge option's index
            Assert.Equal("Zero", buttons[0].Text);
            Assert.Equal("One", buttons[1].Text);
            Assert.Equal("Two", buttons[2].Text);
            Assert.Equal("A really long string for a button", buttons[3].Text);
        }

        [Fact]
        public void CreateButtons_SetsWidthOfButtonsToMatchWidestButton()
        {
            List<NudgeOptionButton> buttons = testObject.CreateButtons();
            
            int maximumWidth = buttons.Max(b => b.Width);
            
            Assert.True(buttons.All(b => b.Width == maximumWidth));
        }
    }
}
