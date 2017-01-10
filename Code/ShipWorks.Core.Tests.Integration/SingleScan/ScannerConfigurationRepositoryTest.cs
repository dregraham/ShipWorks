using System;
using System.IO;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.SingleScan;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.SingleScan
{
    public class ScannerConfigurationRepositoryTest : IDisposable
    {
        private readonly ScannerConfigurationRepository testObject;
        private readonly string value;

        public ScannerConfigurationRepositoryTest()
        {
            AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            // Create a new shipworks instance
            Guid newInstance = Guid.NewGuid();
            ShipWorksSession.Initialize(newInstance);

            // Create the folder where we store instance settings
            Directory.CreateDirectory(DataPath.InstanceSettings);

            testObject = mock.Create<ScannerConfigurationRepository>();

            // Generate a random string value to save to the repository
            Random random = new Random();
            value = new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }

        [Fact]
        public void Get_ReturnsEmptyString_WhenFileDoesNotExist()
        {
            Assert.Equal(string.Empty, testObject.Get());
        }

        [Fact]
        public void Get_ReturnsScannerName()
        {
            testObject.Save(value);
            Assert.Equal(value, testObject.Get());
        }

        [Fact]
        public void Save_SavesScannerName()
        {
            testObject.Save(value);
            Assert.Equal(value, testObject.Get());
        }

        public void Dispose()
        {
            // Clean up after ourselves
            try
            {
                File.Delete(Path.Combine(DataPath.InstanceSettings, "scanner.xml"));
                Directory.Delete(DataPath.InstanceSettings);
            }
            catch (Exception)
            {
                // just some house keeping
            }
        }
    }
}