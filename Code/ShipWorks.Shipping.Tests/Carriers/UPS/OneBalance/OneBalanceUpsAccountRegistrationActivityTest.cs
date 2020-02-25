using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Shipping.Tests.Carriers.UPS.OneBalance
{
    public  class OneBalanceUpsAccountRegistrationActivityTest
    {
        private static AutoMock mock;

        public OneBalanceUpsAccountRegistrationActivityTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

    }
}
