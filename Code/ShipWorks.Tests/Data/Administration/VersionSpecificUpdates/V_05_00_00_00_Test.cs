using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration.VersionSpecifcUpdates;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data.Administration.VersionSpecificUpdates
{
    public class V_05_00_00_00_Test
    {
        [Fact]
        public void Update_CustomerLicenseNotChangedIfCustomerKeyHasValue()
        {
            using(var mock = AutoMock.GetLoose())
            {
                var customerLicense = mock.MockRepository.Create<ICustomerLicense>();
                mock.MockFunc<string, ICustomerLicense>(customerLicense);

                var configEntity = mock.MockRepository.Create<IConfigurationEntity>();
                configEntity.SetupGet(c => c.CustomerKey).Returns("key");

                var configData = mock.Mock<IConfigurationData>();
                configData.Setup(c => c.FetchReadOnly()).Returns(configEntity.Object);

                mock.Create<V_05_00_00_00>().Update();

                customerLicense.Verify(l => l.Save(), Times.Never);
            }
        }

        [Fact]
        public void Update_CustomerLicenseSavedIfCustomerKeyIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var customerLicense = mock.MockRepository.Create<ICustomerLicense>();
                mock.MockFunc<string, ICustomerLicense>(customerLicense);

                var configEntity = mock.MockRepository.Create<IConfigurationEntity>();
                configEntity.SetupGet(c => c.CustomerKey).Returns((string) null);

                var configData = mock.Mock<IConfigurationData>();
                configData.Setup(c => c.FetchReadOnly()).Returns(configEntity.Object);

                mock.Create<V_05_00_00_00>().Update();

                customerLicense.Verify(l => l.Save(), Times.Once);
            }
        }

        [Fact]
        public void Update_CheckForChangesNeedeCalled()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var customerLicense = mock.MockRepository.Create<ICustomerLicense>();
                mock.MockFunc<string, ICustomerLicense>(customerLicense);

                var configEntity = mock.MockRepository.Create<IConfigurationEntity>();
                configEntity.SetupGet(c => c.CustomerKey).Returns((string) null);

                var configData = mock.Mock<IConfigurationData>();
                configData.Setup(c => c.FetchReadOnly()).Returns(configEntity.Object);

                mock.Create<V_05_00_00_00>().Update();

                configData.Verify(l => l.CheckForChangesNeeded(), Times.Once);
            }
        }
    }
}
