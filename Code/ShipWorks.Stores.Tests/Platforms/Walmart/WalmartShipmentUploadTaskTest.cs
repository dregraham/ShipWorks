using System;
using Autofac.Extras.Moq;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Walmart.CoreExtensions.Actions;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Walmart
{
    public class WalmartShipmentUploadTaskTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly WalmartShipmentUploadTask testObject;

        public WalmartShipmentUploadTaskTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<WalmartShipmentUploadTask>();
        }

        [Fact]
        public void InputEntityType_IsShipmentEntity()
        {
            Assert.Equal(EntityType.ShipmentEntity, testObject.InputEntityType);
        }

        [Fact]
        public void CreateEditor_CreatesBasicShipmentUploadTaskEditor()
        {
            Assert.IsAssignableFrom<BasicShipmentUploadTaskEditor>(testObject.CreateEditor());
        }

        [Fact]
        public void SupportsStore_ReturnsTrueWhenGivenWalmartStore()
        {
            Assert.True(testObject.SupportsStore(new WalmartStoreEntity()));
        }

        [Fact]
        public void SupportStore_ReturnsFalseWhenGivenNonWalmartStore()
        {
            Assert.False(testObject.SupportsStore(new StoreEntity()));
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}