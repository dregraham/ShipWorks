using Autofac;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using ShipWorks.Shipping.Editing.Rating;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.Carriers.Dhl
{
    public class DhlExpressServiceControlTest
    {
        private AutoMock mock;
        
        public DhlExpressServiceControlTest()
        {
            mock = AutoMock.GetLoose();
        }

        [Fact]
        public void LoadAccounts_DelegatesToDhlExpressAccountRepository()
        {
            Mock<IDhlExpressAccountRepository> repo = mock.Mock<IDhlExpressAccountRepository>();

            var accounts = new[] { new DhlExpressAccountEntity() };
            repo.SetupGet(r => r.AccountsReadOnly).Returns(accounts);

            DhlExpressServiceControl testObject = mock.Create<DhlExpressServiceControl>(TypedParameter.From(new RateControl()));

            testObject.LoadAccounts();

            repo.VerifyGet(r => r.AccountsReadOnly, Times.Exactly(2));
        }
    }
}
