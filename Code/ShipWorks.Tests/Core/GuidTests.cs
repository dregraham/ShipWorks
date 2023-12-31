using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Interapptive.Shared;
using System.Diagnostics;
using Interapptive.Shared.Utility;

namespace ShipWorks.Tests.Core
{
    public class GuidTests
    {
        [Fact]
        public void RealGuid1()
        {
            string value = "{1F9A8F64-7769-46A1-B9E9-88038ACF158E}";

            Guid guid;
            bool valid = GuidHelper.TryParse(value, out guid);

            Assert.True(valid, "TryParse returned false.");
            Assert.Equal(new Guid(value), guid);
        }

        [Fact]
        public void NonGuid1()
        {
            string value = "Whatever";

            Guid guid;
            bool valid = GuidHelper.TryParse(value, out guid);

            Assert.False(valid, "TryParse should not have returned true.");
        }

        [Fact]
        public void ReadNullValue()
        {
            Guid guid;
            Assert.Throws<ArgumentNullException>(() => GuidHelper.TryParse(null, out guid));
        }
    }
}
