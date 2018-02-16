using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class ShippingProfileLoaderTest
    {
        private readonly AutoMock mock;

        public ShippingProfileLoaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }
    }
}
