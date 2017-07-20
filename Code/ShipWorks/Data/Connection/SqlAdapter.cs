using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.DQE.SqlServer;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.SqlServer.General;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    public sealed class SqlAdapter : DataAccessAdapter, ISqlAdapter
    {
        private const int ForeignKeyReferentialIntegrityError = 547;

        // Logger
        private static readonly ILog log = LogManager.GetLogger(typeof(SqlAdapter));

        // Allows us to use a user-configured database.
        private static CatalogNameOverwriteHashtable catalogNameOverwrites = new CatalogNameOverwriteHashtable();

        // Indicates if we should be logging all InfoMessage events from the underlying connection
        private bool logInfoMessages = false;

        // Indicates if the SET IDENTITY INSERT should be set before inserts, allowing pk values to be explicitly set
        private bool identityInsert = false;

        // Lets us track if any entity is saved in a recursive save operation
        private bool entitySaved = false;

        // The user passes in the connection to use
        private DbConnection overrideConnection;

        // The active TransactionScope, of the adapter was instructed to be required to be in a transaction
        private System.Transactions.TransactionScope transactionScope;

        // LLBLgen starts a new "regular" transaction when recursive saving\deleting\updating.  The problem is then if there is an error, it does a direct call to "Rollback",
        // even though it should really wait and see what the wrapping transaction scope vote ends up doing.  This way LLBLgen sees that there is already
        // a regular transaction and doesn't do that.  Doing "StartTransaction" when InSystemTransaction is true is basically just setting the internal
        // isTransactionInProgress flag to true.
        private bool inFakePhysicalTranscation = false;

        /// <summary>
        /// Raised when the transaction completes if the adapter participates in an ambient transaction.
        /// </summary>
        public event System.Transactions.TransactionCompletedEventHandler TransactionCompleted;

        // Needed to keep LLBLgen from trying to create its own trans for a recursive save
        private static System.Reflection.FieldInfo fieldIsTransactionInProgress;

        // Needed to allow SqlAdapter to use an existing DbTransaction
        private static System.Reflection.FieldInfo fieldPhysicalTransaction;

        /// <summary>
        /// Static constructor
        /// </summary>
        static SqlAdapter()
        {
            catalogNameOverwrites.Add("ShipWorks", "");
            catalogNameOverwrites.Add("ShipWorksLocal", "");

            fieldIsTransactionInProgress = typeof(DataAccessAdapterCore).GetField("_isTransactionInProgress", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (fieldIsTransactionInProgress == null)
            {
                throw new InvalidOperationException("Could not get _isTransactionInProgress field");
            }

            fieldPhysicalTransaction = typeof(DataAccessAdapterCore).GetField("_physicalTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (fieldPhysicalTransaction == null)
            {
                throw new InvalidOperationException("Could not get _physicalTransaction field");
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SqlAdapter() : this(false)
        {
            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);
        }

        /// <summary>
        /// Constructor that specifies the connection that the adapter should use
        /// </summary>
        public SqlAdapter(DbConnection connectionToUse)
            : base("", true, catalogNameOverwrites, null)
        {
            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);
            this.overrideConnection = connectionToUse;

            InitializeCommon();
        }

        /// <summary>
        /// Constructor that specifies the connection that the adapter should use
        /// </summary>
        public SqlAdapter(DbConnection connectionToUse, DbTransaction transactionToUse)
            : base("", true, catalogNameOverwrites, null)
        {
            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);
            overrideConnection = connectionToUse;

            if (transactionToUse != null)
            {
                fieldPhysicalTransaction.SetValue(this, transactionToUse);
                fieldIsTransactionInProgress.SetValue(this, true);
            }

            InitializeCommon();
        }

        /// <summary>
        /// If transacted is true, all calls to the adapter will be executed on the same connection within a transaction.
        /// </summary>
        public SqlAdapter(bool ensureTransacted) :
            this(ensureTransacted, System.Transactions.IsolationLevel.ReadCommitted)
        {
            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);
        }

        /// <summary>
        /// If transacted is true, all calls to the adapter will be executed on the same connection within a transaction.
        /// </summary>
        public SqlAdapter(bool ensureTransacted, System.Transactions.IsolationLevel isolation) :
            this(StartTransactionScopeIfNeeded(ensureTransacted, isolation))
        {
            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);
        }

        /// <summary>
        /// If transacted is true, all calls to the adapter will be executed on the same connection within a transaction.
        /// </summary>
        private SqlAdapter(System.Transactions.TransactionScope transactionScope) :
            base(SqlSession.Current.Configuration.GetConnectionString(), false, catalogNameOverwrites, null)
        {
            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);
            this.transactionScope = transactionScope;

            System.Transactions.Transaction ambient = System.Transactions.Transaction.Current;
            if (ambient != null)
            {
                ambient.TransactionCompleted += new System.Transactions.TransactionCompletedEventHandler(OnTransactionCompleted);
            }

            InitializeCommon();
        }

        /// <summary>
        /// Start a new TransactionScope if needed.  We don't call the DataAccessAdapater.StartTrans
        /// </summary>
        private static System.Transactions.TransactionScope StartTransactionScopeIfNeeded(bool ensureTransacted, System.Transactions.IsolationLevel isolation)
        {
            if (ensureTransacted)
            {
                // If there's already an ambient transaction just use it's isolation
                if (System.Transactions.Transaction.Current != null)
                {
                    isolation = System.Transactions.Transaction.Current.IsolationLevel;
                }

                return new System.Transactions.TransactionScope(
                    System.Transactions.TransactionScopeOption.Required,
                    new System.Transactions.TransactionOptions { IsolationLevel = isolation, Timeout = DbCommandProvider.DefaultTimeout },
                    System.Transactions.TransactionScopeAsyncFlowOption.Enabled);
            }

            return null;
        }

        /// <summary>
        /// Common initialization that should be called by all constructors
        /// </summary>
        private void InitializeCommon()
        {
            CommandTimeOut = (int) DbCommandProvider.DefaultTimeout.TotalSeconds;

            if (Debugger.IsAttached)
            {
                LogInfoMessages = true;
            }
        }

        /// <summary>
        /// Disposing
        /// </summary>
        protected override void Dispose(bool isDisposing)
        {
            bool ensureOverrideOpenAfterDispose = false;
            DbConnection existingConnection = overrideConnection;

            // If a connection was passed in, we have to prevent the base implementation from
            // closing it.
            if (overrideConnection != null && overrideConnection.State == ConnectionState.Open)
            {
                ensureOverrideOpenAfterDispose = true;

                System.Reflection.FieldInfo activeConnection = typeof(DataAccessAdapterCore).GetField(
                    "_activeConnection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (activeConnection == null)
                {
                    throw new InvalidOperationException("Could not find DataAccessAdapterCore._activeConnection.");
                }

                // Clear it, so it doesn't get disposed by base. OverrideConnection will be used when activeConnection is
                // null, so we need to clear it, too
                activeConnection.SetValue(this, null);
                overrideConnection = null;
            }

            base.Dispose(isDisposing);

            if (ensureOverrideOpenAfterDispose)
            {
                // Restore the overridden connection if we expect it to still be open
                overrideConnection = existingConnection;

                if (overrideConnection.State != ConnectionState.Open)
                {
                    throw new InvalidOperationException("The OverrideConnection was not kept open through disposal.");
                }
            }

            if (transactionScope != null)
            {
                transactionScope.Dispose();
                transactionScope = null;
            }
        }

        #region Connection \ Transaction

        /// <summary>
        /// Opens a SQL connection
        /// </summary>
        /// <remarks>InComPlusTransaction was removed because all the COM+ code was removed from LLBLgen v3.5</remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OpenConnection()
        {
            DbConnection connection = GetActiveConnection();

            if (connection.State != ConnectionState.Open)
            {
                ConnectionMonitor.OpenConnection(connection);
            }
        }

        /// <summary>
        /// Intercept creation of the connection to override connection if necessary
        /// </summary>
        protected override DbConnection CreateNewPhysicalConnection(string connectionString)
        {
            DbConnection con;

            if (overrideConnection != null)
            {
                con = overrideConnection;
            }
            else
            {
                con = base.CreateNewPhysicalConnection(connectionString);
            }

            if (logInfoMessages)
            {
                SqlConnection sqlConn = con as SqlConnection;
                if (sqlConn != null)
                {
                    sqlConn.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
                }
            }

            return con;
        }

        /// <summary>
        /// Transaction is committing
        /// </summary>
        public override void Commit()
        {
            bool wasLLBLGenTransInProgress = IsTransactionInProgress;

            base.Commit();

            if (transactionScope != null)
            {
                // LLBLGen sometimes calls StartTransaction during recursive saves to ensure a transaction.  In this case, Commit isn't a true
                // completion, but just a vote.  We can detect this case with this flag...
                if (!wasLLBLGenTransInProgress)
                {
                    transactionScope.Complete();
                }
            }
        }

        /// <summary>
        /// Don't hide the real exception if rolling back due to a connection failure
        /// </summary>
        public override void Rollback()
        {
            ConnectionState state = GetActiveConnection().State;

            // If we are trying to rollback, and the connection is already closed or broken, we must be being rolled back due to
            // an exception on the connection.
            if (state == ConnectionState.Broken ||
                state == ConnectionState.Closed)
            {
                log.InfoFormat("Ignoring rollback due to connection state {0}.", state);
                return;
            }

            base.Rollback();
        }

        /// <summary>
        /// Called when the current System.Transactions Transaction completes
        /// </summary>
        private void OnTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            TransactionCompleted?.Invoke(this, e);
        }

        /// <summary>
        /// Fakes out LLBLGen to thinking a physical transaction is open.  See the docs of the field for information.
        /// </summary>
        private void StartFakePyhsicalTransationIfNeeded()
        {
            if (!inFakePhysicalTranscation && InSystemTransaction)
            {
                inFakePhysicalTranscation = true;

                fieldIsTransactionInProgress.SetValue(this, true);
            }
        }

        /// <summary>
        /// Close the fake physical transaction that was started, if any.  See the doc of the field for more information.
        /// </summary>
        private void CloseFakePhysicalTransactionIfNeeded()
        {
            if (inFakePhysicalTranscation)
            {
                inFakePhysicalTranscation = false;

                fieldIsTransactionInProgress.SetValue(this, false);

                // Since LLBLgen thought we were in a "real" transaction since we set that flag, it didn't auto close the connection
                if (!KeepConnectionOpen)
                {
                    CloseConnection();
                }
            }
        }

        #endregion Connection \ Transaction

        #region InfoMessages

        /// <summary>
        /// Indicates if the adapter should log all the InfoMessage events from its connection
        /// </summary>
        public bool LogInfoMessages
        {
            get
            {
                return logInfoMessages;
            }
            set
            {
                if (logInfoMessages == value)
                {
                    return;
                }

                SqlConnection con = GetActiveConnection() as SqlConnection;

                if (con != null)
                {
                    if (value)
                    {
                        con.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
                    }
                    else
                    {
                        con.InfoMessage -= new SqlInfoMessageEventHandler(OnInfoMessage);
                    }
                }

                logInfoMessages = value;
            }
        }

        /// <summary>
        /// An info message has been sent from sql server
        /// </summary>
        private void OnInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            foreach (string message in e.Message.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                log.InfoFormat(message);
            }
        }

        #endregion InfoMessages

        #region Utility

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        public bool SaveAndRefetch(IEntity2 entity)
        {
            return SaveAndRefetch(entity, true);
        }

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        public bool SaveAndRefetch(IEntity2 entity, bool recurse)
        {
            entitySaved = false;

            base.SaveEntity(entity, true, recurse);

            return entitySaved;
        }

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        public Task<bool> SaveAndRefetchAsync(IEntity2 entity) =>
            SaveAndRefetchAsync(entity, true);

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        public async Task<bool> SaveAndRefetchAsync(IEntity2 entity, bool recurse)
        {
            entitySaved = false;

            await SaveEntityAsync(entity, true, recurse).ConfigureAwait(false);

            return entitySaved;
        }

        /// <summary>
        /// Override of the SaveEntity method to wrap exceptions.
        /// </summary>
        public override bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return base.SaveEntity(entityToSave, refetchAfterSave, updateRestriction, recurse);
            }
            catch (ORMQueryExecutionException ex)
            {
                TranslateException(ex);
                throw;
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Override of the SaveEntityAsync method to wrap exceptions.
        /// </summary>
        public override async Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse, CancellationToken cancellationToken)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return await base.SaveEntityAsync(entityToSave, refetchAfterSave, updateRestriction, recurse, cancellationToken).ConfigureAwait(false);
            }
            catch (ORMQueryExecutionException ex)
            {
                TranslateException(ex);
                throw;
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Called write before an entity is written to the database
        /// </summary>
        protected override void OnSaveEntity(IActionQuery saveQuery, IEntity2 entityToSave)
        {
            entitySaved = true;

            base.OnSaveEntity(saveQuery, entityToSave);
        }

        /// <summary>Sets the flag to signal the SqlServer DQE to generate SET ARITHABORT ON statements prior to INSERT, DELETE and UPDATE Queries.
        /// Keep this flag to false in normal usage, but set it to true if you need to write into a table which is part of an indexed view.
        /// It will not affect normal inserts/updates that much, leaving it on is not harmful. See Books online for details on SET ARITHABORT ON.
        /// After each statement the setting is turned off if it has been turned on prior to that statement.</summary>
        /// <remarks>Setting this flag is a global change.</remarks>
        public void SetArithAbortFlag(bool value)
        {
            DynamicQueryEngine.ArithAbortOn = value;
        }

        /// <summary>Sets the default compatibility level used by the DQE. Default is SqlServer2005. This is a global setting.
        /// Compatibility level influences the query generated for paging, sequence name (@@IDENTITY/SCOPE_IDENTITY()), and usage of newsequenceid() in inserts.
        /// It also influences the ado.net provider to use. This way you can switch between SqlServer server client 'SqlClient' and SqlServer CE Desktop.</summary>
        /// <remarks>Setting this property will overrule a similar setting in the .config file. Don't set this property when queries are executed as
        /// it might switch factories for ADO.NET elements which could result in undefined behavior so set this property at startup of your application</remarks>
        public void SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel compatibilityLevel)
        {
            DynamicQueryEngine.DefaultCompatibilityLevel = compatibilityLevel;
        }

        /// <summary>Creates a new Dynamic Query engine object and passes in the defined catalog/schema overwrite hashtables.</summary>
        protected override DynamicQueryEngineBase CreateDynamicQueryEngine()
        {
            return this.PostProcessNewDynamicQueryEngine(new DynamicQueryEngine());
        }

        /// <summary>Reads the value of the setting with the key ConnectionStringKeyName from the *.config file and stores that value as the active connection string to use for this object.</summary>
        /// <returns>connection string read</returns>
        private static string ReadConnectionStringFromConfig()
        {
            return ConfigFileHelper.ReadConnectionStringFromConfig(ConnectionStringKeyName);
        }

        /// <summary>Sets the per instance compatibility level on the dqe instance specified.</summary>
        /// <param name="dqe">The dqe.</param>
        protected override void SetPerInstanceCompatibilityLevel(DynamicQueryEngineBase dqe)
        {
            if (_compatibilityLevel.HasValue)
            {
                ((DynamicQueryEngine) dqe).CompatibilityLevel = _compatibilityLevel.Value;
            }
        }

        private Nullable<SqlServerCompatibilityLevel> _compatibilityLevel = null;

        /// <summary>The per-instance compatibility level used by this DQE instance. Default is the one set globally, which is by default SqlServer2005 (for 2005+).
        /// Compatibility level influences the query generated for paging, sequence name (@@IDENTITY/SCOPE_IDENTITY()), and usage of newsequenceid() in inserts.
        /// It also influences the ado.net provider to use. This way you can switch between SqlServer server client 'SqlClient' and SqlServer CE Desktop.</summary>
        public Nullable<SqlServerCompatibilityLevel> CompatibilityLevel
        {
            get { return _compatibilityLevel; }
            set { _compatibilityLevel = value; }
        }

        /// <summary>
        /// Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        /// from the persistent storage if they match the filter supplied in filterBucket.
        /// </summary>
        public override int DeleteEntitiesDirectly(string entityName, IRelationPredicateBucket filterBucket)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return base.DeleteEntitiesDirectly(entityName, filterBucket);
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Deletes the specified entity from the persistent storage. The entity is not
        /// usable after this call, the state is set to OutOfSync.  Will use the current
        /// transaction if a transaction is in progress.
        /// </summary>
        public override bool DeleteEntity(IEntity2 entityToDelete, IPredicateExpression deleteRestriction)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return base.DeleteEntity(entityToDelete, deleteRestriction);
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Deletes the specified entity from the persistent storage. The entity is not
        /// usable after this call, the state is set to OutOfSync.  Will use the current
        /// transaction if a transaction is in progress.
        /// </summary>
        public override async Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, IPredicateExpression deleteRestriction, CancellationToken cancellationToken)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return await base.DeleteEntityAsync(entityToDelete, deleteRestriction, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Deletes all dirty objects inside the collection passed from the persistent
        /// storage. It will do this inside a transaction if a transaction is not yet
        /// available. Entities which are physically deleted from the persistent storage
        /// are marked with the state 'Deleted' but are not removed from the collection.
        /// </summary>
        public override int DeleteEntityCollection(IEntityCollection2 collectionToDelete)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return base.DeleteEntityCollection(collectionToDelete);
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Updates all entities of the same type or subtype of the entity entityWithNewValues
        /// directly in the persistent storage if they match the filter supplied in filterBucket.
        /// Only the fields changed in entityWithNewValues are updated for these fields.
        /// Entities of a subtype of the type of entityWithNewValues which are affected
        /// by the filterBucket's filter will thus also be updated.
        /// </summary>
        public override int UpdateEntitiesDirectly(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return base.UpdateEntitiesDirectly(entityWithNewValues, filterBucket);
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available.
        /// </summary>
        public override int SaveEntityCollection(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse)
        {
            StartFakePyhsicalTransationIfNeeded();

            try
            {
                return base.SaveEntityCollection(collectionToSave, refetchSavedEntitiesAfterSave, recurse);
            }
            finally
            {
                CloseFakePhysicalTransactionIfNeeded();
            }
        }

        /// <summary>
        /// Override to wrap exceptions
        /// </summary>
        public override int ExecuteActionQuery(IActionQuery queryToExecute)
        {
            try
            {
                return base.ExecuteActionQuery(queryToExecute);
            }
            catch (ORMQueryExecutionException ex)
            {
                TranslateException(ex);
                throw;
            }
        }

        /// <summary>
        /// Override to wrap exceptions
        /// </summary>
        public override int CallActionStoredProcedure(string storedProcedureToCall, DbParameter[] parameters)
        {
            try
            {
                return base.CallActionStoredProcedure(storedProcedureToCall, parameters);
            }
            catch (SqlException ex)
            {
                TranslateException(ex);
                throw;
            }
        }

        /// <summary>
        /// Executes the passed in retrieval query and returns the results in the data table
        /// specified using the passed in data-adapter. It sets the connection object
        /// of the command object of query object passed in to the connection object
        /// of this class.
        /// </summary>
        public override bool ExecuteMultiRowDataTableRetrievalQuery(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, DataTable tableToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo)
        {
            // The base implementation does not close the connection in a finally, so it's possible the connection could be left open, and the next call would
            // either potentially promote to DTC, or something else bad.
            try
            {
                return base.ExecuteMultiRowDataTableRetrievalQuery(queryToExecute, dataAdapterToUse, tableToFill, fieldsPersistenceInfo);
            }
            finally
            {
                if (!KeepConnectionOpen && !IsTransactionInProgress)
                {
                    CloseConnection();
                }
            }
        }

        /// <summary>
        /// Translate the given query execution exception into a more detailed wrapper exception.
        /// </summary>
        private void TranslateException(Exception ex)
        {
            SqlException sqlEx = ex as SqlException;
            if (sqlEx == null)
            {
                ORMQueryExecutionException queryEx = ex as ORMQueryExecutionException;
                if (queryEx != null)
                {
                    sqlEx = queryEx.InnerException as SqlException;
                }
            }

            if (sqlEx != null && sqlEx.Errors.Count > 0)
            {
                SqlError error = sqlEx.Errors[0];

                if (error.Number == ForeignKeyReferentialIntegrityError)
                {
                    string message = error.Message;

                    Match match = Regex.Match(error.Message, "table \"dbo.([^\"]+)");
                    if (match.Success)
                    {
                        string table = match.Groups[1].Value;

                        // Utilize the fact that we use camel casing for table names
                        string friendly = new string(table.ToCharArray().SelectMany(c => char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray()).ToLower().Trim();

                        if (message.IndexOf("DELETE statement", StringComparison.Ordinal) != -1)
                        {
                            message = $"A parent could not be deleted because it still has {friendly} children.";
                        }
                        else
                        {
                            message = $"A child could not be saved because its parent {friendly} has been deleted.";
                        }
                    }

                    throw new SqlForeignKeyException(message, ex);
                }

                // Dead lock error
                if (UtilityFunctions.IsDeadlockException(sqlEx))
                {
                    throw new SqlDeadlockException(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Get the name of the physical table represented by the entity type
        /// </summary>
        public static string GetTableName(EntityType entityType)
        {
            IEntityFields2 fields = EntityFieldsFactory.CreateEntityFieldsObject(entityType);

            // We have to find a field whose containing object name is the one e want
            foreach (EntityField2 field in fields)
            {
                if (EntityTypeProvider.GetEntityType(field.ContainingObjectName) == entityType)
                {
                    return GetPersistenceInfo(field).SourceObjectName;
                }
            }

            throw new InvalidOperationException("Could not determine actual TableName for EntityType " + entityType);
        }

        /// <summary>
        /// Returns a new default instance of a SqlAdapter
        /// </summary>
        public static SqlAdapter Default => Create(false);

        /// <summary>
        /// Create a new SqlAdapter
        /// </summary>
        public static SqlAdapter Create(bool inTransaction) => new SqlAdapter(inTransaction);

        #endregion Utility

        #region Identity Inserts

        /// <summary>
        /// Indicates if the SET IDENTITY INSERT should be set before inserts, allowing pk values to be explicitly set
        /// </summary>
        public bool IdentityInsert
        {
            get
            {
                return identityInsert;
            }
            set
            {
                identityInsert = value;
            }
        }

        /// <summary>
        /// Create an insert query
        /// </summary>
        protected override IActionQuery CreateInsertDQ(IEntity2 entityToSave, IFieldPersistenceInfo[] persistenceInfoObjects)
        {
            IEntityFieldCore[] fieldCores = entityToSave.Fields.GetAsEntityFieldCoreArray();

            // If we are doing an identity insert, we have to remove the identity information
            if (identityInsert)
            {
                RemoveIdentityProperty(persistenceInfoObjects, fieldCores, entityToSave.Fields);
            }

            IActionQuery saveQuery = CreateDynamicQueryEngine().CreateInsertDQ(fieldCores, persistenceInfoObjects, this.GetActiveConnection());

            if (identityInsert)
            {
                BatchActionQuery query = saveQuery as BatchActionQuery;
                if (query == null)
                {
                    query = new BatchActionQuery();
                    query.AddActionQuery(saveQuery);
                }

                // Turn identity insert on in the first statement
                query[0].Command.CommandText = string.Format(@"
                    SET IDENTITY_INSERT [{0}] ON;
                    {1}",
                GetTableName((EntityType) entityToSave.LLBLGenProEntityTypeValue),
                query[0].Command.CommandText);

                // Turn it off in the last statement
                query[query.Count - 1].Command.CommandText += string.Format(@"
                    SET IDENTITY_INSERT [{0}] OFF;",
                GetTableName((EntityType) entityToSave.LLBLGenProEntityTypeValue));
            }

            return saveQuery;
        }

        /// <summary>
        /// Remove all flags for all identity fields to make them normal writable fields
        /// </summary>
        private void RemoveIdentityProperty(IFieldPersistenceInfo[] infos, IEntityFieldCore[] fields,
            IEntityFieldsCore containingEntityFields)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                IFieldPersistenceInfo info = infos[i];

                if (info.IsIdentity)
                {
                    infos[i] = CloneWithIdentityOff(info);
                    fields[i] = CloneWithReadOnlyOff(fields[i], containingEntityFields);
                }
            }
        }

        /// <summary>
        /// Clone the persistence info, setting the identity value off.
        /// </summary>
        private IFieldPersistenceInfo CloneWithIdentityOff(IFieldPersistenceInfo info)
        {
            return new FieldPersistenceInfo(
                 info.SourceCatalogName,
                 info.SourceSchemaName,
                 info.SourceObjectName,
                 info.SourceColumnName,
                 info.SourceColumnIsNullable,
                 info.SourceColumnDbType,
                 info.SourceColumnMaxLength,
                 info.SourceColumnScale,
                 info.SourceColumnPrecision,
                 false,
                 null,
                 info.TypeConverterToUse,
                 info.ActualDotNetType);
        }

        /// <summary>
        /// Clone the given field with the ReadOnly flag turned off
        /// </summary>
        /// <remarks>The LinkedSuperTypeField property is derived from the containing entity fields collection,
        /// which we now pass into the constructor because the property has been made read-only in LLBLgen 4.2</remarks>
        private IEntityFieldCore CloneWithReadOnlyOff(IEntityFieldCore field, IEntityFieldsCore containingEntityFields)
        {
            IEntityFieldCore clone = new EntityField2(
               new FieldInfo(
                   field.Name,
                   field.ContainingObjectName,
                   field.DataType,
                   field.IsPrimaryKey,
                   field.IsForeignKey,
                   false,
                   field.IsNullable,
                   field.FieldIndex,
                   field.MaxLength,
                   field.Scale,
                   field.Precision),
                containingEntityFields);

            clone.AggregateFunctionToApply = field.AggregateFunctionToApply;
            clone.Alias = field.Alias;
            clone.ForcedCurrentValueWrite(field.CurrentValue, field.DbValue);
            clone.ExpressionToApply = field.ExpressionToApply;
            clone.IsChanged = field.IsChanged;
            clone.IsNull = field.IsNull;
            clone.ObjectAlias = field.ObjectAlias;

            return clone;
        }

        #endregion Identity Inserts
    }
}