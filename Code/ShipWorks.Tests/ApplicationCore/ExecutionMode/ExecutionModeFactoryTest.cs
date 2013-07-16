using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Tests.ApplicationCore.ExecutionMode
{
    [TestClass]
    public class ExecutionModeFactoryTest
    {
        private ExecutionModeFactory testObject;

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Create_ThrowsNotImplementedException_WhenServiceIsSpecified_WithFullSwitch_Test()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] { "/service=serviceName" });
            testObject = new ExecutionModeFactory(commandLine);

            testObject.Create();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Create_ThrowsNotImplementedException_WhenServiceIsSpecified_WithAbbreviatedSwitch_Test()
        {            
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] {"/s=serviceName"});
            testObject = new ExecutionModeFactory(commandLine);

            testObject.Create();
        }

        [TestMethod]
        public void Create_ReturnsCommandLineExecutionMode_WhenCommandIsSpecified_WithFullSwitch_Test()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] { "/command:opensqlfirewall" });
            testObject = new ExecutionModeFactory(commandLine);

            IExecutionMode executionMode = testObject.Create();

            Assert.IsInstanceOfType(executionMode, typeof(CommandLineExecutionMode));
        }

        [TestMethod]
        public void Create_ReturnsCommandLineExecutionMode_WhenCommandIsSpecified_WithCmdSwitch_Test()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] {"/cmd:opensqlfirewall"});
            testObject = new ExecutionModeFactory(commandLine);

            IExecutionMode executionMode = testObject.Create();

            Assert.IsInstanceOfType(executionMode, typeof(CommandLineExecutionMode));
        }

        [TestMethod]
        public void Create_ReturnsCommandLineExecutionMode_WhenCommandIsSpecified_WithAbbreviatedSwitch_Test()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] { "/c:opensqlfirewall" });
            testObject = new ExecutionModeFactory(commandLine);

            IExecutionMode executionMode = testObject.Create();

            Assert.IsInstanceOfType(executionMode, typeof(CommandLineExecutionMode));
        }

        [TestMethod]
        public void Create_ReturnsUserInterfaceExecutionMode_WhenServiceIsNotSpecified_AndCommandIsNotSpecified_Test()
        {
            testObject = new ExecutionModeFactory(ShipWorksCommandLine.Empty);

            IExecutionMode executionMode = testObject.Create();

            Assert.IsInstanceOfType(executionMode, typeof(UserInterfaceExecutionMode));
        }
    }
}
