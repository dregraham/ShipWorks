using System.Linq;
using Interapptive.Shared.Extensions;
using Xunit;

namespace Interapptive.Shared.Tests.Collections
{
    public class CollectionExtensionsTest
    {
        [Fact]
        public void RemoveWhere_RemovesItems_ThatMatchPredicate()
        {
            var input = Enumerable.Range(1, 4).ToList();

            input.RemoveWhere(x => x % 2 == 0);

            Assert.Equal(new[] { 1, 3 }, input);
        }

        [Fact]
        public void RemoveWhere_DoesNotRemoveAnything_WhenPredicateDoesNotMatch()
        {
            var input = Enumerable.Range(1, 4).ToList();

            input.RemoveWhere(x => x == 100);

            Assert.Equal(new[] { 1, 2, 3, 4 }, input);
        }

        [Fact]
        public void RemoveWhere_ReturnsRemovedItems_ThatMatchPredicate()
        {
            var input = Enumerable.Range(1, 4).ToList();

            var deleted = input.RemoveWhere(x => x % 2 == 0);

            Assert.Equal(new[] { 2, 4 }, deleted);
        }

        [Fact]
        public void RemoveWhere_DoesEmptyList_WhenPredicateDoesNotMatch()
        {
            var input = Enumerable.Range(1, 4).ToList();

            var deleted = input.RemoveWhere(x => x == 100);

            Assert.Empty(deleted);
        }
    }
}
