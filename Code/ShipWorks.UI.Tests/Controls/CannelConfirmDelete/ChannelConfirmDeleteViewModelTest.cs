using System.Collections.Generic;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.CannelConfirmDelete
{
    public class ChannelConfirmDeleteViewModelTest
    {
        [Fact]
        public void Load_SetsMessageWithAmazonStores_WhenStoreTypeCodeIsAmazon()
        {
            using (var mock = AutoMock.GetLoose())
            {
                List<StoreEntity> storeList = new List<StoreEntity>
                {
                    new StoreEntity
                    {
                        StoreName = "test store name",
                        TypeCode = (int)StoreTypeCode.Amazon
                    }
                };
                
                mock.Mock<IStoreManager>().Setup(m => m.GetAllStores()).Returns(storeList);

                var testObject = mock.Create<ChannelConfirmDeleteViewModel>();

                testObject.Load(StoreTypeCode.Amazon);
                Assert.Contains("test store name", testObject.Message);
            }
        }

        [Fact]
        public void Load_SetsIntroWithAmazonDescription_WhenStoreTypeCodeIsAmazon()
        {
            using (var mock = AutoMock.GetLoose())
            {
                List<StoreEntity> storeList = new List<StoreEntity>
                {
                    new StoreEntity
                    {
                        TypeCode = (int)StoreTypeCode.Amazon
                    }
                };

                mock.Mock<IStoreManager>().Setup(m => m.GetAllStores()).Returns(storeList);

                var testObject = mock.Create<ChannelConfirmDeleteViewModel>();

                testObject.Load(StoreTypeCode.Amazon);
                Assert.Contains(EnumHelper.GetDescription(StoreTypeCode.Amazon), testObject.Intro);
            }
        }
    }
}