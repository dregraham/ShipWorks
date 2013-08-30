using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Tests.Actions.Tasks
{
    [TestClass]
    public class TaskValidationErrorTest
    {
        [TestMethod]
        public void ToString_ReturnsOnlyMessage_WhenDetailsAreEmpty()
        {
            TaskValidationError testObject = new TaskValidationError("Foo");
            Assert.AreEqual("Foo", testObject.ToString());
        }

        [TestMethod]
        public void ToString_ReturnsFormattedError_WhenDetailsAreNotEmpty()
        {
            TaskValidationError testObject = new TaskValidationError("Foo");
            testObject.Details.Add("Bar");
            Assert.AreEqual("Foo" + Environment.NewLine + "  - Bar", testObject.ToString());  
        }
    }
}
