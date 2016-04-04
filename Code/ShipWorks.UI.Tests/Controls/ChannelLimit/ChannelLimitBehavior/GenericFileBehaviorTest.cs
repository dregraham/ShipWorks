using System.Collections.ObjectModel;
using System.Linq;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.ChannelLimit.ChannelLimitBehavior
{
    public class GenericFileBehaviorTest
    {
        [Fact]
        public void PopulateChannels_OnlyChannelIsGenericFile()
        {
            ObservableCollection<StoreTypeCode> channels =
                new ObservableCollection<StoreTypeCode>()
                {
                    StoreTypeCode.Amazon, StoreTypeCode.AmeriCommerce
                };

            GenericFileBehavior testObject = new GenericFileBehavior();

            testObject.PopulateChannels(channels, null);

            Assert.Equal(StoreTypeCode.GenericFile,channels.Single());
        }

        [Fact]
        public void EditionFeature_IsGenericFile()
        {
            var testObject = new GenericFileBehavior();

            Assert.Equal(EditionFeature.GenericFile, testObject.EditionFeature);
        }

        [Fact]
        public void EditionFeature_TitleContainsGenericFile()
        {
            var testObject = new GenericFileBehavior();

            Assert.Contains("Generic File", testObject.Title);
        }

    }
}
