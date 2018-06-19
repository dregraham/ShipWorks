using System;
using System.Transactions;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using ShipWorks.Users;
using Xunit;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class TransactionDtcTest : IDisposable
    {
        private readonly DataContext context;

        public TransactionDtcTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void ForceDtcException_WhenUserIDChanges()
        {
            Assert.False(IsMsdtcRunning(), "MSDTC is running.  Shut it off!");
            
            OrderEntity order = new OrderEntity(1006);

            using (TransactionScope transactionScope = new TransactionScope())
            {
                using (var adapter = SqlAdapter.Create(false))
                {
                    adapter.FetchEntity(order);
                }

                // Changing the current UserID will change the WorkstationID which used to 
                // change the connection string which would cause an MSDTC error.
                UserSession.User.UserID = UserSession.User.UserID + 1000;

                using (var adapter = SqlAdapter.Create(false))
                {
                    order = new OrderEntity(2006);
                    adapter.FetchEntity(order);
                }

                transactionScope.Complete();
            }
        }

        private bool IsMsdtcRunning()
        {
            using (var msDtcSvc = new System.ServiceProcess.ServiceController("MSDTC"))
            {
                return msDtcSvc?.Status == System.ServiceProcess.ServiceControllerStatus.Running;
            }
        }

        public void Dispose() => context.Dispose();
    }
}
