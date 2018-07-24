using System.Collections.Generic;
using System.Linq;
using Moq;
using ShipWorks.Actions.Tasks.Common;
using Xunit;

namespace ShipWorks.Tests.Actions.Tasks.Common
{
    public class MissingIndexResolverTest
    {
        private readonly MockRepository mock;

        public MissingIndexResolverTest()
        {
            mock = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Empty };
        }

        [Fact]
        public void GetIndexesToEnable_ReturnsEmptyList_WhenNoIndexesAreMissing()
        {
            var testObject = new MissingIndexResolver();

            var results = testObject.GetIndexesToEnable(
                Enumerable.Empty<MissingIndex>(),
                new[] { new DisabledIndex("Foo", "Bar", "", Enumerable.Empty<IndexColumn>()) });

            Assert.Empty(results);
        }

        [Fact]
        public void GetIndexesToEnable_ReturnsEmptyList_WhenNoDisabledIndexesSatisfyMissingIndexes()
        {
            var testObject = new MissingIndexResolver();

            var results = testObject.GetIndexesToEnable(
                new[] { mock.Create<MissingIndex>().Object },
                new[] { new DisabledIndex("Foo", "Bar", "", Enumerable.Empty<IndexColumn>()) });

            Assert.Empty(results);
        }

        [Fact]
        public void GetIndexesToEnable_ReturnsSingleIndex_WhenOneDisabledIndexSatisfiesTwoMissingIndexes()
        {
            var disabledIndex = new DisabledIndex("Foo", "Bar", "", Enumerable.Empty<IndexColumn>());
            var missingIndex1 = mock.Create<MissingIndex>();
            missingIndex1.Setup(x => x.FindBestMatchingIndex(It.IsAny<IEnumerable<DisabledIndex>>())).Returns(disabledIndex);
            var missingIndex2 = mock.Create<MissingIndex>();
            missingIndex2.Setup(x => x.FindBestMatchingIndex(It.IsAny<IEnumerable<DisabledIndex>>())).Returns(disabledIndex);
            var testObject = new MissingIndexResolver();

            var results = testObject.GetIndexesToEnable(
                new[] { missingIndex1.Object, missingIndex2.Object },
                new[] { disabledIndex });

            Assert.Equal(new[] { disabledIndex }, results);
        }
    }
}
