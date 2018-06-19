using System.Threading;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model.Custom
{
    /// <summary>
    /// Unit of work that opens the connection before committing
    /// </summary>
    /// <remarks>
    /// This class ensures that PrepareConnection is called on the ConnectionMonitor so the DB context is set correctly.
    /// </remarks>
    public class ManagedConnectionUnitOfWork2 : UnitOfWork2
    {
        /// <summary>
        /// Commits this unit of work. It will first add all entities in the added collections
        /// to the correct bins, then it will start by first inserting all new entities,
        /// then saving all updates and then performing the deletes.
        /// </summary>
        /// <param name="adapterToUse">Adapter to use. It will start a new transaction if no transaction is in progress.</param>
        /// <param name="autoCommit">if true, it will commit/rollback the transaction passed in using the following
        ///     rules: - if no exception is thrown: commit - if an exception is thrown: rollback</param>
        /// <returns>The total # of entities affected by all actions performed in the Commit method</returns>
        /// <exception cref="System.ArgumentNullException">when adapterToUse is null</exception>
        public override int Commit(IDataAccessAdapter adapterToUse, bool autoCommit)
        {
            adapterToUse.OpenConnection();
            return base.Commit(adapterToUse, autoCommit);
        }

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.UnitOfWork2.Commit(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,System.Boolean).
        /// Commits this unit of work. It will first add all entities in the added collections
        /// to the correct bins, then it will start by first inserting all new entities,
        /// then saving all updates and then performing the deletes.
        /// </summary>
        /// <param name="adapterToUse">Adapter to use. It will start a new transaction if no transaction is in progress.</param>
        /// <param name="autoCommit">if true, it will commit/rollback the transaction passed in using the following
        ///     rules: - if no exception is thrown: commit - if an exception is thrown: rollback</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The total # of entities affected by all actions performed in the Commit method</returns>
        /// <exception cref="System.ArgumentNullException">when adapterToUse is null</exception>
        public override async Task<int> CommitAsync(IDataAccessAdapter adapterToUse, bool autoCommit, CancellationToken cancellationToken)
        {
            await adapterToUse.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
            return await base.CommitAsync(adapterToUse, autoCommit, cancellationToken).ConfigureAwait(false);
        }
    }
}
