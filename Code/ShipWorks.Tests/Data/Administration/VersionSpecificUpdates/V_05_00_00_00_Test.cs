using Autofac.Extras.Moq;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data;
using ShipWorks.Data.Administration.VersionSpecificUpdates;
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
    public class V_05_00_00_00_Test : IDisposable
    {
        AutoMock mock;
        V_05_00_00_00 testObject;

        public V_05_00_00_00_Test()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testObject = mock.Create<V_05_00_00_00>();
        }

        [Fact]
        public void Update_CustomerLicenseNotChangedIfCustomerKeyHasValue()
        {
            var config = mock.Mock<IConfigurationEntity>();
            config.SetupGet(c => c.CustomerKey).Returns("key");

            mock.Mock<IConfigurationData>().Setup(d => d.FetchReadOnly()).Returns(config.Object);

            mock.Create<V_05_00_00_00>().Update();

            mock.Mock<ICustomerLicense>().Verify(l => l.Save(), Times.Never);
        }

        [Fact]
        public void Update_CustomerLicenseSavedIfCustomerKeyIsNull()
        {
            mock.Create<V_05_00_00_00>().Update();

            mock.Mock<ICustomerLicense>().Verify(l => l.Save(), Times.Once);
        }

        [Fact]
        public void Update_CheckForChangesNeedeCalled()
        {
            testObject.Update();

            mock.Mock<IConfigurationData>().Verify(l => l.CheckForChangesNeeded(), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
