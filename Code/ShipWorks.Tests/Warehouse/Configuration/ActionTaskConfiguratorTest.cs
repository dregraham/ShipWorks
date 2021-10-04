using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;
using Xunit;

namespace ShipWorks.Tests.Warehouse.Configuration
{
    public class ActionTaskConfiguratorTest
    {
        private readonly AutoMock mock;

        public ActionTaskConfiguratorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Theory]
        [InlineData(true, true, false)]
        [InlineData(true, false, true)]
        [InlineData(false, true, true)]
        public void ActionTaskNotCreated_IfStoreDoesNotNeedIt(bool managedInHub, bool uploadShipmentDetails, bool isNew)
        {
            var config = new StoreConfiguration()
            {
                ManagedInHub = managedInHub,
                UploadShipmentDetails = uploadShipmentDetails
            };
            var store = new StoreEntity();
            var testObject = mock.Create<ActionTaskConfigurator>();
            testObject.Configure(config, store, isNew);
            
            mock.Mock<IActionTaskFactory>().Verify(f=>f.Create(It.IsAny<Type>(), It.IsAny<StoreEntity>(), 0), Times.Never);
        }

        [Fact]
        public void ActionTaskCreated_IsStoreNeedsIt()
        {
            var config = new StoreConfiguration()
            {
                ManagedInHub = true,
                UploadShipmentDetails = true
            };
            var store = new StoreEntity();
            var testObject = mock.Create<ActionTaskConfigurator>();
            
            // expected error when saving the ActionTask
            var exception = Assert.Throws<InvalidOperationException>(() => testObject.Configure(config, store, true));
            Assert.Equal("Attempt to save ActionTask that has no backing entity.", exception.Message);    
        }
    }
}