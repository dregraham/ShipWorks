using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.ExecutionMode;

namespace ShipWorks.Tests.ApplicationCore.ExecutionMode
{
    public class ExecutionModeFactoryTest
    {
        private ExecutionModeFactory testObject;

        [Fact]
        public void Create_ThrowsNotImplementedException_WhenServiceIsSpecified_WithFullSwitch()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] { "/service=serviceName" });
            testObject = new ExecutionModeFactory(commandLine);

            var executionMode = testObject.Create();

            Assert.IsAssignableFrom<ServiceExecutionMode>(executionMode);
        }

        [Fact]
        public void Create_ThrowsNotImplementedException_WhenServiceIsSpecified_WithAbbreviatedSwitch()
        {            
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] {"/s=serviceName"});
            testObject = new ExecutionModeFactory(commandLine);

            var executionMode = testObject.Create();

            Assert.IsAssignableFrom<ServiceExecutionMode>(executionMode);
        }

        [Fact]
        public void Create_ReturnsCommandLineExecutionMode_WhenCommandIsSpecified_WithFullSwitch()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] { "/command:opensqlfirewall" });
            testObject = new ExecutionModeFactory(commandLine);

            var executionMode = testObject.Create();

            Assert.IsAssignableFrom<CommandLineExecutionMode>(executionMode);
        }

        [Fact]
        public void Create_ReturnsCommandLineExecutionMode_WhenCommandIsSpecified_WithCmdSwitch()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] {"/cmd:opensqlfirewall"});
            testObject = new ExecutionModeFactory(commandLine);

            var executionMode = testObject.Create();

            Assert.IsAssignableFrom<CommandLineExecutionMode>(executionMode);
        }

        [Fact]
        public void Create_ReturnsCommandLineExecutionMode_WhenCommandIsSpecified_WithAbbreviatedSwitch()
        {
            ShipWorksCommandLine commandLine = ShipWorksCommandLine.Parse(new string[] { "/c:opensqlfirewall" });
            testObject = new ExecutionModeFactory(commandLine);

            var executionMode = testObject.Create();

            Assert.IsAssignableFrom<CommandLineExecutionMode>(executionMode);
        }

        [Fact]
        public void Create_ReturnsUserInterfaceExecutionMode_WhenServiceIsNotSpecified_AndCommandIsNotSpecified()
        {
            testObject = new ExecutionModeFactory(ShipWorksCommandLine.Empty);

            var executionMode = testObject.Create();

            Assert.IsAssignableFrom<UserInterfaceExecutionMode>(executionMode);
        }
    }
}
