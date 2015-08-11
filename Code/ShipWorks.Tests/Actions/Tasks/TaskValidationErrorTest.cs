﻿using System;
using Xunit;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Tests.Actions.Tasks
{
    public class TaskValidationErrorTest
    {
        [Fact]
        public void ToString_ReturnsOnlyMessage_WhenDetailsAreEmpty()
        {
            TaskValidationError testObject = new TaskValidationError("Foo");
            Assert.AreEqual("Foo", testObject.ToString());
        }

        [Fact]
        public void ToString_ReturnsFormattedError_WhenDetailsAreNotEmpty()
        {
            TaskValidationError testObject = new TaskValidationError("Foo");
            testObject.Details.Add("Bar");
            Assert.AreEqual("Foo" + Environment.NewLine + "  - Bar", testObject.ToString());  
        }
    }
}
