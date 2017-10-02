using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using Xunit;

namespace ShipWorks.Data.Modal.Tests.Custom
{
    public class YahooStoreEntityTest
    {
        [Fact]
        public void AsReadOnly_CopiesCorrectly_WhenUsingActualReference()
        {
            var store = new YahooStoreEntity();
            var result = store.AsReadOnly();
            Assert.IsAssignableFrom<IYahooStoreEntity>(result);
        }

        [Fact]
        public void AsReadOnly_CopiesCorrectly_WhenUsingStoreReference()
        {
            StoreEntity store = new YahooStoreEntity();
            var result = store.AsReadOnly();
            Assert.IsAssignableFrom<IYahooStoreEntity>(result);
        }
    }
}
