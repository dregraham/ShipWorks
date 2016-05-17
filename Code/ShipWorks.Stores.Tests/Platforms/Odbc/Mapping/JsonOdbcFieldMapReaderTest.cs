using System;
using System.IO;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class JsonOdbcFieldMapReaderTest : IDisposable
    {
        private readonly AutoMock mock;

        public JsonOdbcFieldMapReaderTest()
        {
            mock = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}