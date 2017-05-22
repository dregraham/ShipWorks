using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Threading;
using Moq;
using RestSharp;
using ShipWorks.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.BigCommerce
{
    public class BigCommerceWebClientFactoryTest
    {
        readonly AutoMock mock;

        public BigCommerceWebClientFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Create_Returns_BigCommerceWebClient()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;

            IBigCommerceWebClientFactory testObj = mock.Create<BigCommerceWebClientFactory>();
            IBigCommerceWebClient webClient = testObj.Create(store);

            Assert.NotNull(webClient);
        }

        [Fact]
        public void CreateWithProgress_Returns_BigCommerceWebClient()
        {
            BigCommerceStoreEntity store = new BigCommerceStoreEntity(1);
            store.BigCommerceAuthentication = BigCommerceAuthenticationType.Oauth;

            IBigCommerceWebClientFactory testObj = mock.Create<BigCommerceWebClientFactory>();
            IProgressReporter progressReporter = new ProgressItem("testing");

            IBigCommerceWebClient webClient = testObj.Create(store, progressReporter);

            Assert.NotNull(webClient);
            Assert.NotNull(webClient.ProgressReporter);
        }
    }
}
