using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Autofac;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ChannelAdvisor;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.ChannelAdvisor
{
    public class ChannelAdvisorDownloaderFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly DbConnection con;

        public ChannelAdvisorDownloaderFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            con = new SqlConnection();
        }

        [Fact]
        public void DownloadWithOrderNumber_ReturnsCompletedTask()
        {
            StoreEntity store = new ChannelAdvisorStoreEntity();
            var task = mock.Create<ChannelAdvisorDownloaderFactory>(TypedParameter.From(store)).Download("order", 1, con);
            Assert.Equal(Task.CompletedTask, task);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}