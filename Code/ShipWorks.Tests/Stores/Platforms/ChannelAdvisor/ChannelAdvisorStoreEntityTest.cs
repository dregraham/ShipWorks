using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Amazon;
using Xunit;

namespace ShipWorks.Tests.Stores.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorStoreEntityTest
    {

        [Fact]
        public void ChannelAdvisorStoreEntity_Implements_IAmazonCredentials()
        {
            IAmazonCredentials testObject = new ChannelAdvisorStoreEntity() as IAmazonCredentials;

            Assert.True(testObject != null);
        }

        [Fact]
        public void ParsedAttributesToDownload_ReturnsEmptyCollection_WhenNoAttributes()
        {
            var store = new ChannelAdvisorStoreEntity {AttributesToDownload = "<Attributes />"};
            Assert.Empty(store.ParsedAttributesToDownload);
        }

        [Fact]
        public void ParsedAttributesToDownload_ReturnsPopulatedCollection_WhenAttributes()
        {
            var xmlWithTwoAttributes = @"<Attributes><Attribute>a1</Attribute><Attribute>a2</Attribute></Attributes>";
            var store = new ChannelAdvisorStoreEntity { AttributesToDownload = xmlWithTwoAttributes };
            var parsedAttributes = store.ParsedAttributesToDownload;

            Assert.Equal(2, parsedAttributes.Count());
            Assert.Contains("a1", parsedAttributes);
            Assert.Contains("a2", parsedAttributes);
        }

    }
}
