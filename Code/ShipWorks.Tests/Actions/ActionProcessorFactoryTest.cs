using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Actions;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityInterfaces;
using Xunit;

namespace ShipWorks.Tests.Actions
{
    public class ActionProcessorFactoryTest
    {
        Mock<IConfigurationData> configData;
        Mock<IConfigurationEntity> config;

        public ActionProcessorFactoryTest()
        {
            configData = new Mock<IConfigurationData>();
            config = new Mock<IConfigurationEntity>();
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void CreateStandard_Returns_OneActionProcessorWithStandardGateway_WhenUserInterfaceMode(bool useParallelActionQueue, bool isUiSupported)
        {
            config.Setup(c => c.UseParallelActionQueue).Returns(useParallelActionQueue);
            configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
            configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(ActionQueueType.UserInterface);
            configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(true);

            ActionProcessorFactory testObject = new ActionProcessorFactory(configData.Object);
            IEnumerable<ActionProcessor> actionProcessors = testObject.CreateStandard();

            Assert.Equal(1, actionProcessors.Count());
            Assert.Equal(ActionQueueGatewayType.Standard, actionProcessors.First().GatewayType);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        public void CreateStandard_Returns_TwoProcessors_WhenUserInterfaceMode(bool useParallelActionQueue, bool isUiSupported)
        {
            config.Setup(c => c.UseParallelActionQueue).Returns(useParallelActionQueue);
            configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
            configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(ActionQueueType.UserInterface);
            configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(true);

            ActionProcessorFactory testObject = new ActionProcessorFactory(configData.Object);
            IEnumerable<ActionProcessor> actionProcessors = testObject.CreateStandard();

            Assert.Equal(2, actionProcessors.Count());
            Assert.Equal(1, actionProcessors.Count(ap => ap.GatewayType == ActionQueueGatewayType.DefaultPrint));
            Assert.Equal(1, actionProcessors.Count(ap => ap.GatewayType == ActionQueueGatewayType.Standard));
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void CreateStandard_ReturnsOneActionProcessorWithStandardGateway_WhenScheduledMode(bool useParallelActionQueue, bool includeUserInterfaceActions)
        {
            config.Setup(c => c.UseParallelActionQueue).Returns(useParallelActionQueue);
            configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);
            configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(ActionQueueType.Scheduled);
            configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(includeUserInterfaceActions);

            ActionProcessorFactory testObject = new ActionProcessorFactory(configData.Object);
            IEnumerable<ActionProcessor> actionProcessors = testObject.CreateStandard();

            Assert.Equal(1, actionProcessors.Count());
            Assert.Equal(ActionQueueGatewayType.Standard, actionProcessors.First().GatewayType);
        }

        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        public void CreateStandard_ReturnsTwoProcessors_WhenScheduledMode(bool useParallelActionQueue, bool includeUserInterfaceActions)
        {
            config.Setup(c => c.UseParallelActionQueue).Returns(useParallelActionQueue);
            configData.Setup(cd => cd.FetchReadOnly()).Returns(config.Object);

            ActionProcessorFactory testObject = new ActionProcessorFactory(configData.Object);
            IEnumerable<ActionProcessor> actionProcessors = testObject.CreateStandard();
            configData.Setup(cd => cd.ExecutionModeActionQueueType).Returns(ActionQueueType.Scheduled);
            configData.Setup(cd => cd.IncludeUserInterfaceActions).Returns(includeUserInterfaceActions);

            Assert.Equal(2, actionProcessors.Count());
            Assert.Equal(1, actionProcessors.Count(ap => ap.GatewayType == ActionQueueGatewayType.DefaultPrint));
            Assert.Equal(1, actionProcessors.Count(ap => ap.GatewayType == ActionQueueGatewayType.Standard));
        }
    }
}
