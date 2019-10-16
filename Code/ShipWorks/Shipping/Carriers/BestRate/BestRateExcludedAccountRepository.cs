using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Autofac;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.ComponentRegistration.Ordering;
using ShipWorks.ApplicationCore;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Utility;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    [Order(typeof(IInitializeForCurrentSession), Order.Unordered)]
    [Component]
    public class BestRateExcludedAccountRepository : IBestRateExcludedAccountRepository, IInitializeForCurrentSession
    {
        private static TableSynchronizer<BestRateExcludedAccountEntity> synchronizer;
        private static bool needCheckForChanges;
        private static IEnumerable<BestRateExcludedAccountEntity> accounts;

        /// <summary>
        /// Initialize the repository
        /// </summary>
        public void InitializeForCurrentSession()
        {
            Initialize();
        }

        public static IBestRateExcludedAccountRepository Current => IoC.UnsafeGlobalLifetimeScope.Resolve<IBestRateExcludedAccountRepository>();

        /// <summary>
        /// Save the given excluded best rate accounts
        /// </summary>
        /// <param name="accountIDs"></param>
        public void Save(IEnumerable<long> accountIDs)
        {
            using (var adapter = new SqlAdapter())
            {
                // delete the existing excluded accounts
                adapter.DeleteEntitiesDirectly(typeof(BestRateExcludedAccountEntity), null);

                foreach (long accountID in accountIDs)
                {
                    adapter.SaveEntity(new BestRateExcludedAccountEntity(accountID));
                }
            }

            CheckForChangesNeeded();
        }

        /// <summary>
        /// Get all of the best rate excluded accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<long> GetAll()
        {
            lock (synchronizer)
            {
                if (needCheckForChanges)
                {
                    InternalCheckForChanges();
                }

                return accounts.Select(x => x.AccountID);
            }
        }

        /// <summary>
        /// Initialize BestRateExcludedAccountRepository
        /// </summary>
        private static void Initialize()
        {
            synchronizer = new TableSynchronizer<BestRateExcludedAccountEntity>();
            InternalCheckForChanges();
        }

        /// <summary>
        /// Check for any changes made in the database since initialization or the last check
        /// </summary>
        private static void CheckForChangesNeeded()
        {
            lock (synchronizer)
            {
                needCheckForChanges = true;
            }
        }

        /// <summary>
        /// Do the actual trip to the database to check for changes
        /// </summary>
        private static void InternalCheckForChanges()
        {
            lock (synchronizer)
            {
                if (synchronizer.Synchronize())
                {
                    synchronizer.EntityCollection.Sort((int) BestRateExcludedAccountFieldIndex.AccountID, ListSortDirection.Ascending);
                }

                accounts = EntityUtility.CloneEntityCollection(synchronizer.EntityCollection);

                needCheckForChanges = false;
            }
        }
    }
}
