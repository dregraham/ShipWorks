using System.Collections.ObjectModel;
using System.Linq;
using ShipWorks.Editions;
using ShipWorks.Stores;
using ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior;
using Xunit;

namespace ShipWorks.UI.Tests.Controls.ChannelLimit.ChannelLimitBehavior
{
    public class GenericModuleBehaviorTest
    {
        [Fact]
        public void PopulateChannels_OnlyChannelIsGenericModule()
        {
            ObservableCollection<StoreTypeCode> channels =
                new ObservableCollection<StoreTypeCode>
                {
                    StoreTypeCode.Amazon,
                    StoreTypeCode.AmeriCommerce
                };

            GenericModuleBehavior testObject = new GenericModuleBehavior();

            testObject.PopulateChannels(channels, null);

            Assert.Equal(StoreTypeCode.GenericModule, channels.Single());
        }

        [Fact]
        public void EditionFeature_IsGenericFile()
        {
            GenericModuleBehavior testObject = new GenericModuleBehavior();

            Assert.Equal(EditionFeature.GenericModule, testObject.EditionFeature);
        }

        [Fact]
        public void EditionFeature_TitleContainsGenericFile()
        {
            GenericModuleBehavior testObject = new GenericModuleBehavior();

            Assert.Contains("Generic Module", testObject.Title);
        }
    }
}