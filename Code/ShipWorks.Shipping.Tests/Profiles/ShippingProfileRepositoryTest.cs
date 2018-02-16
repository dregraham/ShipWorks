using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Connection;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class ShippingProfileLoaderTest
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileLoader testObject;
        private readonly Mock<ISqlAdapterFactory> sqlAdapterFactory;
        private readonly Mock<ISqlAdapter> sqlAdapter;

        public ShippingProfileLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            sqlAdapter = mock.Mock<ISqlAdapter>();
            sqlAdapterFactory = mock.Mock<ISqlAdapterFactory>();
            sqlAdapterFactory.Setup(a => a.Create()).Returns(sqlAdapter);

            testObject = mock.Create<ShippingProfileLoader>();
        }

        public void LoadProfileData()
        {

        }
    }
}
