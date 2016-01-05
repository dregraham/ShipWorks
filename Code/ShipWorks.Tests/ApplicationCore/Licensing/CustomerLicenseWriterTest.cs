using System;
using System.Linq;
using System.Security;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Controls;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing
{
    public class CustomerLicenseWriterTest
    {
        [Fact]
        public void Save_Updates_Existing_ConfigurationEntity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                // Create a default config entity
                ConfigurationEntity config = new ConfigurationEntity();
                config.CustomerKey = "the old license key";

                mock.Mock<IConfigurationDataWrapper>().Setup(c => c.GetConfiguration()).Returns(config);
                
                // Create a mock customer license
                CustomerLicense customerLicense = mock.Create<CustomerLicense>();
                customerLicense.Key = "the new license key";

                CustomerLicenseWriter licenseWriter = mock.Create<CustomerLicenseWriter>();

                licenseWriter.Write(customerLicense);

                Assert.Equal("the new license key", config.CustomerKey);
            }
        }
    }
}