using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.Language;
using Moq.Language.Flow;
using RestSharp;

namespace ShipWorks.Stores.Tests.Integration.Extensions
{
    /// <summary>
    /// Extension methods for Mock of IRestClient
    /// </summary>
    public static class RestClientMockExtensions
    {
        /// <summary>
        /// Mock a resource call
        /// </summary>
        public static ISetup<IRestClient, IRestResponse> ForResource(this Mock<IRestClient> mock, string resource) =>
            mock.Setup(x => x.Execute(It.Is<IRestRequest>(r => r.Resource == resource)));

        /// <summary>
        /// Mock a resource that will be called multiple times
        /// </summary>
        public static ISetupSequentialResult<IRestResponse> ForResourceSequence(this Mock<IRestClient> mock, string resource) =>
            mock.SetupSequence(x => x.Execute(It.Is<IRestRequest>(r => r.Resource == resource)));
    }
}
