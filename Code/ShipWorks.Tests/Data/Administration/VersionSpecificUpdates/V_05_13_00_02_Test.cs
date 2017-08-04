﻿using System;
using Autofac.Extras.Moq;
using ShipWorks.Data.Administration.VersionSpecificUpdates;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Data.Administration.VersionSpecificUpdates
{
    public class V_05_13_00_02_Test : IDisposable
    {
        readonly AutoMock mock;

        public V_05_13_00_02_Test()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void AppliesTo_ReturnsCorrectVersion()
        {
            var testObject = mock.Create<V_05_13_00_02>();
            var result = testObject.AppliesTo;
            Assert.Equal(new Version(5, 13, 0, 2), result);
        }

        [Fact]
        public void AlwaysRuns_ReturnsFalse()
        {
            var testObject = mock.Create<V_05_13_00_02>();
            var result = testObject.AlwaysRun;
            Assert.False(result);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
