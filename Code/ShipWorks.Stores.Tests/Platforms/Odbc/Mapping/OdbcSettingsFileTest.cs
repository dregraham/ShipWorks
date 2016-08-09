using System;
using Autofac.Extras.Moq;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Mapping
{
    public class OdbcSettingsFileTest : IDisposable
    {
        private AutoMock mock;

        public OdbcSettingsFileTest()
        {
            mock = AutoMock.GetLoose();
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}