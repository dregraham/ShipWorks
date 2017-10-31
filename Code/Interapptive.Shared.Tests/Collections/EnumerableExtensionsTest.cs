using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using ShipWorks.Shipping;
using Xunit;

namespace Interapptive.Shared.Tests.Collections
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void ToReadonlyDictionary_CreatesReadonlyDictionary_FromIDictionary()
        {
            IDictionary<bool, string> source = new Dictionary<bool, string>();
            source.Add(false, "one");
            source.Add(true, "two");

            ReadOnlyDictionary<bool, string> testObject = source.ToReadOnlyDictionary();

            Assert.Equal(2, testObject.Count);
            Assert.Equal(source[true], testObject[true]);
            Assert.Equal(source[false], testObject[false]);
        }
    }
}
