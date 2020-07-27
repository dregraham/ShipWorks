using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Ups.OneBalance;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.UI.Settings.OneBalance;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Settings.OneBalance
{
    public class OneBalanceAccountHelperTest
    {
        private readonly AutoMock mock;
        private readonly Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>> uspsAccountRepo;
        private readonly OneBalanceAccountHelper testObject;

        public OneBalanceAccountHelperTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            
            uspsAccountRepo = mock.Mock<ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity>>();
            uspsAccountRepo.SetupGet(r => r.AccountsReadOnly).Returns(new[] 
                { 
                    new UspsAccountEntity() { ShipEngineCarrierId = null }, 
                    new UspsAccountEntity() { ShipEngineCarrierId = null } 
                });


            testObject = mock.Create<OneBalanceAccountHelper>();
        }

        [Fact]
        public void CustomerSupportPhoneNumber_IsCorrect()
        {
            var result = testObject.GetUspsAccount(ShipmentTypeCode.Usps);
            Assert.True(result.Failure);
            Assert.True(result.Message.Contains("314-821-5888"));
        }
    }
}
