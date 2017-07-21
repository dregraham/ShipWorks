using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;
using SD.LLBLGen.Pro.QuerySpec.Adapter;
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

            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);

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
        }

        /// <summary>
        /// Constructor that specifies the connection that the adapter should use
        /// </summary>
        public SqlAdapter(DbConnection connectionToUse)
            : base("", true, catalogNameOverwrites, null)
        {
            this.overrideConnection = connectionToUse;

            InitializeCommon();
        }

        /// <summary>
        /// Constructor that specifies the connection that the adapter should use
        /// </summary>
        public SqlAdapter(DbConnection connectionToUse, DbTransaction transactionToUse)
            : base("", true, catalogNameOverwrites, null)
        {
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
        }

        /// <summary>
        /// If transacted is true, all calls to the adapter will be executed on the same connection within a transaction.
        /// </summary>
        public SqlAdapter(bool ensureTransacted, System.Transactions.IsolationLevel isolation) :
            this(StartTransactionScopeIfNeeded(ensureTransacted, isolation))
        {
        }

        /// <summary>
        /// If transacted is true, all calls to the adapter will be executed on the same connection within a transaction.
        /// </summary>
        private SqlAdapter(System.Transactions.TransactionScope transactionScope) :
            base(SqlSession.Current.Configuration.GetConnectionString(), false, catalogNameOverwrites, null)
        {
            this.transactionScope = transactionScope;

            System.Transactions.Transaction ambient = System.Transactions.Transaction.Current;
            if (ambient != null)
            {
                ambient.TransactionCompleted += OnTransactionCompleted;
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
        private IDisposable StartFakePyhsicalTransationIfNeeded()
        {
            if (!inFakePhysicalTranscation && InSystemTransaction)
            {
                inFakePhysicalTranscation = true;

                fieldIsTransactionInProgress.SetValue(this, true);

                return Disposable.Create(CloseFakePhysicalTransactionIfNeeded);
            }

            return Disposable.Empty;
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
            using (StartFakePyhsicalTransationIfNeeded())
            {
                try
                {
                    return base.SaveEntity(entityToSave, refetchAfterSave, updateRestriction, recurse);
                }
                catch (ORMQueryExecutionException ex)
                {
                    TranslateException(ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Override of the SaveEntityAsync method to wrap exceptions.
        /// </summary>
        public override async Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse, CancellationToken cancellationToken)
        {
            using (StartFakePyhsicalTransationIfNeeded())
            {
                try
                {
                    return await base.SaveEntityAsync(entityToSave, refetchAfterSave, updateRestriction, recurse, cancellationToken).ConfigureAwait(false);
                }
                catch (ORMQueryExecutionException ex)
                {
                    TranslateException(ex);
                    throw;
                }
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

        /// <summary>
        /// Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        /// from the persistent storage if they match the filter supplied in filterBucket.
        /// </summary>
        public override int DeleteEntitiesDirectly(string entityName, IRelationPredicateBucket filterBucket)
        {
            using (StartFakePyhsicalTransationIfNeeded())
            {
                return base.DeleteEntitiesDirectly(entityName, filterBucket);
            }
        }

        /// <summary>
        /// Deletes the specified entity from the persistent storage. The entity is not
        /// usable after this call, the state is set to OutOfSync.  Will use the current
        /// transaction if a transaction is in progress.
        /// </summary>
        public override bool DeleteEntity(IEntity2 entityToDelete, IPredicateExpression deleteRestriction)
        {
            using (StartFakePyhsicalTransationIfNeeded())
            {
                return base.DeleteEntity(entityToDelete, deleteRestriction);
            }
        }

        /// <summary>
        /// Deletes the specified entity from the persistent storage. The entity is not
        /// usable after this call, the state is set to OutOfSync.  Will use the current
        /// transaction if a transaction is in progress.
        /// </summary>
        public override async Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, IPredicateExpression deleteRestriction, CancellationToken cancellationToken)
        {
            using (StartFakePyhsicalTransationIfNeeded())
            {
                return await base.DeleteEntityAsync(entityToDelete, deleteRestriction, cancellationToken).ConfigureAwait(false);
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
            using (StartFakePyhsicalTransationIfNeeded())
            {
                return base.DeleteEntityCollection(collectionToDelete);
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
            using (StartFakePyhsicalTransationIfNeeded())
            {
                return base.UpdateEntitiesDirectly(entityWithNewValues, filterBucket);
            }
        }

        /// <summary>
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available.
        /// </summary>
        public override int SaveEntityCollection(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse)
        {
            using (StartFakePyhsicalTransationIfNeeded())
            {
                return base.SaveEntityCollection(collectionToSave, refetchSavedEntitiesAfterSave, recurse);
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

        #region "IDataAccessAdapter extension method explicit implementation"
        //
        // Summary:
        //     Fetches the query as an open data reader.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   behavior:
        //     The behavior.
        //
        // Remarks:
        //     Ignores nested queries and projection logic embedded in a lambda specification
        //     in the query. The DataReader returned will contain the resultset of the plain
        //     SQL query.
        public IDataReader FetchAsDataReader(DynamicQuery query, CommandBehavior behavior) =>
            ((IDataAccessAdapter) this).FetchAsDataReader(query, behavior);
        //
        // Summary:
        //     Fetches the specified query into a new DataTable specified and returns that datatable.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Returns:
        //     a new DataTable with the data fetched.
        public DataTable FetchAsDataTable(DynamicQuery query) =>
            ((IDataAccessAdapter) this).FetchAsDataTable(query);
        //
        // Summary:
        //     Fetches the specified query into the DataTable specified and returns that datatable.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   destination:
        //     The destination datatable to fetch the data into.
        //
        // Returns:
        //     the destination datatable specified.
        //
        // Remarks:
        //     If the DataTable specified already has columns defined, they have to have compatible
        //     types and have to be in the same order as the columns in the resultset of the
        //     query. It's recommended to have columns with the same names as the resultset
        //     of the query, to be able to convert null values to DBNull.Value.
        public DataTable FetchAsDataTable(DynamicQuery query, DataTable destination) =>
            ((IDataAccessAdapter) this).FetchAsDataTable(query, destination);
        //
        // Summary:
        //     Fetches the query as a projection, using the projector specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   projector:
        //     The projector.
        //
        // Remarks:
        //     Will ignore nested queries. Use with queries without nested / hierarchical queries.
        //     The projector has to be setup and ready to use when calling this method
        public void FetchAsProjection(DynamicQuery query, IGeneralDataProjector projector) =>
            ((IDataAccessAdapter) this).FetchAsProjection(query, projector);
        //
        // Summary:
        //     Fetches the first entity of the set returned by the query and returns that entity,
        //     if any, otherwise null.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     the first entity in the resultset, or null if the resultset is empty.
        public TEntity FetchFirst<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2 =>
            ((IDataAccessAdapter) this).FetchFirst<TEntity>(query);
        //
        // Summary:
        //     Fetches the first object of the set returned by the query and returns that object,
        //     if any, otherwise null.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     the first object in the resultset, or null if the resultset is empty.
        public T FetchFirst<T>(DynamicQuery<T> query) =>
            ((IDataAccessAdapter) this).FetchFirst<T>(query);
        //
        // Summary:
        //     Fetches the query specified on the adapter specified. Uses the TEntity type to
        //     produce an EntityCollection(Of TEntity) for the results to return
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     EntityCollection(Of TEntity) with the results of the query fetch
        public IEntityCollection2 FetchQuery<TEntity>(EntityQuery<TEntity> query) where TEntity : IEntity2 =>
            ((IDataAccessAdapter) this).FetchQuery<TEntity>(query);
        //
        // Summary:
        //     Fetches the query specified on the adapter specified into the collectionToFill
        //     specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   collectionToFill:
        //     The collection to fill.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        //   TCollection:
        //     The type of the collection.
        //
        // Returns:
        //     collectionToFill, filled with the query fetch results.
        //
        // Remarks:
        //     Equal to calling adapter.FetchEntityCollection(), so entities already present
        //     in collectionToFill are left as-is. If the fetch has to take into account a Context,
        //     the passed collectionToFill has to be assigned to the context before calling
        //     this method.
        public TCollection FetchQuery<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill)
        where TEntity : IEntity2
        where TCollection : IEntityCollection2 =>
            ((IDataAccessAdapter) this).FetchQuery(query, collectionToFill);
        //
        // Summary:
        //     Fetches the query specified and returns the results in plain object arrays, one
        //     object array per returned row of the query specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        public List<object[]> FetchQuery(DynamicQuery query) =>
            ((IDataAccessAdapter) this).FetchQuery(query);
        //
        // Summary:
        //     Fetches the query specified and returns the results in a list of TElement objects,
        //     which are created using the projectorFunc of the query specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TElement:
        //     The type of the element.
        public List<TElement> FetchQuery<TElement>(DynamicQuery<TElement> query) =>
            ((IDataAccessAdapter) this).FetchQuery<TElement>(query);
        //
        // Summary:
        //     Fetches the query with the projection specified from the source query specified.
        //     Typically used to fetch a typed view from a stored procedure source.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   projectionDefinition:
        //     The projection definition.
        //
        //   source:
        //     The source.
        //
        // Type parameters:
        //   TElement:
        //     The type of the element.
        //
        // Returns:
        //     List of TElement instances instantiated from each row in source
        public List<TElement> FetchQueryFromSource<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source) =>
            ((IDataAccessAdapter) this).FetchQueryFromSource<TElement>(projectionDefinition, source);
        //
        // Summary:
        //     Fetches a scalar value using the query specified, and returns this value typed
        //     as TValue, using a cast. The query specified will be converted to a scalar query
        //     prior to execution.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TValue:
        //     The type of the value to return.
        //
        // Returns:
        //     the value to fetch
        //
        // Remarks:
        //     Use nullable(Of T) for scalars which are a value type, to avoid crashes when
        //     the scalar query returns a NULL value.
        public TValue FetchScalar<TValue>(DynamicQuery query) =>
            ((IDataAccessAdapter) this).FetchScalar<TValue>(query);
        //
        // Summary:
        //     Fetches the single entity of the set returned by the query and returns that entity.
        //     If there are no elements or more than 1 element, a NotSupportedException will
        //     be thrown.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     the first entity in the resultset
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        //     value in the resultset.
        public TEntity FetchSingle<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2 =>
            ((IDataAccessAdapter) this).FetchSingle<TEntity>(query);
        //
        // Summary:
        //     Fetches the single object of the set returned by the query and returns that object.
        //     If there are no elements or more than 1 element, a NotSupportedException will
        //     be thrown.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     the first object in the resultset
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //     Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        //     value in the resultset.
        public T FetchSingle<T>(DynamicQuery<T> query) =>
            ((IDataAccessAdapter) this).FetchSingle<T>(query);
        #endregion

        #region "IDataAccessAdapter async extension method explicit implementation"
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataReader(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,System.Data.CommandBehavior).
        //     Fetches the query as an open data reader.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   behavior:
        //     The behavior.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Remarks:
        //     Ignores nested queries and projection logic embedded in a lambda specification
        //     in the query. The DataReader returned will contain the resultset of the plain
        //     SQL query.
        public Task<IDataReader> FetchAsDataReaderAsync(DynamicQuery query, CommandBehavior behavior, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchAsDataReaderAsync(query, behavior, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        //     Fetches the specified query into a new DataTable specified and returns that datatable.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Returns:
        //     a new DataTable with the data fetched.
        public Task<DataTable> FetchAsDataTableAsync(DynamicQuery query) =>
            ((IDataAccessAdapter) this).FetchAsDataTableAsync(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,System.Data.DataTable).
        //     Fetches the specified query into the DataTable specified and returns that datatable.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   destination:
        //     The destination datatable to fetch the data into.
        //
        // Returns:
        //     the destination datatable specified.
        //
        // Remarks:
        //     If the DataTable specified already has columns defined, they have to have compatible
        //     types and have to be in the same order as the columns in the resultset of the
        //     query. It's recommended to have columns with the same names as the resultset
        //     of the query, to be able to convert null values to DBNull.Value.
        public Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, DataTable destination) =>
            ((IDataAccessAdapter) this).FetchAsDataTableAsync(query, destination);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,System.Data.DataTable).
        //     Fetches the specified query into the DataTable specified and returns that datatable.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   destination:
        //     The destination datatable to fetch the data into.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the destination datatable specified.
        //
        // Remarks:
        //     If the DataTable specified already has columns defined, they have to have compatible
        //     types and have to be in the same order as the columns in the resultset of the
        //     query. It's recommended to have columns with the same names as the resultset
        //     of the query, to be able to convert null values to DBNull.Value.

        [DebuggerStepThrough]
        public Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, DataTable destination, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchAsDataTableAsync(query, destination, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        //     Fetches the specified query into a new DataTable specified and returns that datatable.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     a new DataTable with the data fetched.
        public Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchAsDataTableAsync(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsProjection(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector).
        //     Fetches the query as a projection, using the projector specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   projector:
        //     The projector.
        //
        // Remarks:
        //     Will ignore nested queries. Use with queries without nested / hierarchical queries.
        //     The projector has to be setup and ready to use when calling this method
        public Task FetchAsProjectionAsync(DynamicQuery query, IGeneralDataProjector projector) =>
            ((IDataAccessAdapter) this).FetchAsProjectionAsync(query, projector);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsProjection(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector).
        //     Fetches the query as a projection, using the projector specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   projector:
        //     The projector.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Remarks:
        //     Will ignore nested queries. Use with queries without nested / hierarchical queries.
        //     The projector has to be setup and ready to use when calling this method
        public Task FetchAsProjectionAsync(DynamicQuery query, IGeneralDataProjector projector, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchAsProjectionAsync(query, projector, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        //     Fetches the first object of the set returned by the query and returns that object,
        //     if any, otherwise null.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     the first object in the resultset, or null if the resultset is empty.
        public Task<T> FetchFirstAsync<T>(DynamicQuery<T> query) =>
            ((IDataAccessAdapter) this).FetchFirstAsync<T>(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        //     Fetches the first object of the set returned by the query and returns that object,
        //     if any, otherwise null.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     the first object in the resultset, or null if the resultset is empty.

        [DebuggerStepThrough]
        public Task<T> FetchFirstAsync<T>(DynamicQuery<T> query, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchFirstAsync<T>(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        //     Fetches the first entity of the set returned by the query and returns that entity,
        //     if any, otherwise null.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     the first entity in the resultset, or null if the resultset is empty.
        public Task<TEntity> FetchFirstAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2 =>
            ((IDataAccessAdapter) this).FetchFirstAsync(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        //     Fetches the first entity of the set returned by the query and returns that entity,
        //     if any, otherwise null.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     the first entity in the resultset, or null if the resultset is empty.

        [DebuggerStepThrough]
        public Task<TEntity> FetchFirstAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : EntityBase2, IEntity2 =>
            ((IDataAccessAdapter) this).FetchFirstAsync(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        //     Fetches the query specified and returns the results in a list of TElement objects,
        //     which are created using the projectorFunc of the query specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   TElement:
        //     The type of the element.

        [DebuggerStepThrough]
        public Task<List<TElement>> FetchQueryAsync<TElement>(DynamicQuery<TElement> query, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchQueryAsync(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``2(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0},``1).
        //     Fetches the query specified on the adapter specified into the collectionToFill
        //     specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   collectionToFill:
        //     The collection to fill.
        //
        //   cancellationToken:
        //     The cancellation token
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        //   TCollection:
        //     The type of the collection.
        //
        // Returns:
        //     collectionToFill, filled with the query fetch results.
        //
        // Remarks:
        //     Equal to calling adapter.FetchEntityCollection(), so entities already present
        //     in collectionToFill are left as-is. If the fetch has to take into account a Context,
        //     the passed collectionToFill has to be assigned to the context before calling
        //     this method.
        public Task<TCollection> FetchQueryAsync<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill, CancellationToken cancellationToken)
        where TEntity : IEntity2
        where TCollection : IEntityCollection2 =>
                ((IDataAccessAdapter) this).FetchQueryAsync(query, collectionToFill, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``2(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0},``1).
        //     Fetches the query specified on the adapter specified into the collectionToFill
        //     specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   collectionToFill:
        //     The collection to fill.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        //   TCollection:
        //     The type of the collection.
        //
        // Returns:
        //     collectionToFill, filled with the query fetch results.
        //
        // Remarks:
        //     Equal to calling adapter.FetchEntityCollection(), so entities already present
        //     in collectionToFill are left as-is. If the fetch has to take into account a Context,
        //     the passed collectionToFill has to be assigned to the context before calling
        //     this method.
        public Task<TCollection> FetchQueryAsync<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill)
        where TEntity : IEntity2
        where TCollection : IEntityCollection2 =>
                ((IDataAccessAdapter) this).FetchQueryAsync(query, collectionToFill);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        //     Fetches the query specified on the adapter specified. Uses the TEntity type to
        //     produce an EntityCollection(Of TEntity) for the results to return
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     EntityCollection(Of TEntity) with the results of the query fetch
        public Task<IEntityCollection2> FetchQueryAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : IEntity2 =>
            ((IDataAccessAdapter) this).FetchQueryAsync<TEntity>(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        //     Fetches the query specified and returns the results in plain object arrays, one
        //     object array per returned row of the query specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        public Task<List<object[]>> FetchQueryAsync(DynamicQuery query) =>
            ((IDataAccessAdapter) this).FetchQueryAsync(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        //     Fetches the query specified and returns the results in plain object arrays, one
        //     object array per returned row of the query specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.

        [DebuggerStepThrough]
        public Task<List<object[]>> FetchQueryAsync(DynamicQuery query, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchQueryAsync(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        //     Fetches the query specified and returns the results in a list of TElement objects,
        //     which are created using the projectorFunc of the query specified.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TElement:
        //     The type of the element.
        public Task<List<TElement>> FetchQueryAsync<TElement>(DynamicQuery<TElement> query) =>
            ((IDataAccessAdapter) this).FetchQueryAsync(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        //     Fetches the query specified on the adapter specified. Uses the TEntity type to
        //     produce an EntityCollection(Of TEntity) for the results to return
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     EntityCollection(Of TEntity) with the results of the query fetch
        public Task<IEntityCollection2> FetchQueryAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : IEntity2 =>
            ((IDataAccessAdapter) this).FetchQueryAsync(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQueryFromSource``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0},SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        //     Fetches the query which projection specified from the source query specified.
        //     Typically used to fetch a typed view from a stored procedure source.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   projectionDefinition:
        //     The projection definition.
        //
        //   source:
        //     The source.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   TElement:
        //     The type of the element.
        //
        // Returns:
        //     List of TElement instances instantiated from each row in source

        [DebuggerStepThrough]
        public Task<List<TElement>> FetchQueryFromSourceAsync<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchQueryFromSourceAsync(projectionDefinition, source, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQueryFromSource``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0},SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        //     Fetches the query which projection specified from the source query specified.
        //     Typically used to fetch a typed view from a stored procedure source.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   projectionDefinition:
        //     The projection definition.
        //
        //   source:
        //     The source.
        //
        // Type parameters:
        //   TElement:
        //     The type of the element.
        //
        // Returns:
        //     List of TElement instances instantiated from each row in source
        public Task<List<TElement>> FetchQueryFromSourceAsync<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source) =>
            ((IDataAccessAdapter) this).FetchQueryFromSourceAsync(projectionDefinition, source);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchScalar``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        //     Fetches a scalar value using the query specified, and returns this value typed
        //     as TValue, using a cast. The query specified will be converted to a scalar query
        //     prior to execution.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TValue:
        //     The type of the value to return.
        //
        // Returns:
        //     the value to fetch
        //
        // Remarks:
        //     Use nullable(Of T) for scalars which are a value type, to avoid crashes when
        //     the scalar query returns a NULL value.
        public Task<TValue> FetchScalarAsync<TValue>(DynamicQuery query) =>
            ((IDataAccessAdapter) this).FetchScalarAsync<TValue>(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchScalar``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        //     Fetches a scalar value using the query specified, and returns this value typed
        //     as TValue, using a cast. The query specified will be converted to a scalar query
        //     prior to execution.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   TValue:
        //     The type of the value to return.
        //
        // Returns:
        //     the value to fetch
        //
        // Remarks:
        //     Use nullable(Of T) for scalars which are a value type, to avoid crashes when
        //     the scalar query returns a NULL value.

        [DebuggerStepThrough]
        public Task<TValue> FetchScalarAsync<TValue>(DynamicQuery query, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchScalarAsync<TValue>(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        //     Fetches the single object of the set returned by the query and returns that object.
        //     If there are no elements or more than 1 element, a NotSupportedException will
        //     be thrown.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     the first object in the resultset
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //
        //   T:System.NotSupportedException:
        //     Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        //     value in the resultset.

        [DebuggerStepThrough]
        public Task<T> FetchSingleAsync<T>(DynamicQuery<T> query, CancellationToken cancellationToken) =>
            ((IDataAccessAdapter) this).FetchSingleAsync<T>(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        //     Fetches the single object of the set returned by the query and returns that object.
        //     If there are no elements or more than 1 element, a NotSupportedException will
        //     be thrown.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     the first object in the resultset
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //
        //   T:System.NotSupportedException:
        //     Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        //     value in the resultset.
        public Task<T> FetchSingleAsync<T>(DynamicQuery<T> query) =>
            ((IDataAccessAdapter) this).FetchSingleAsync(query);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        //     Fetches the single entity of the set returned by the query and returns that entity.
        //     If there are no elements or more than 1 element, a NotSupportedException will
        //     be thrown.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     the first entity in the resultset
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //
        //   T:System.NotSupportedException:
        //     Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        //     value in the resultset.

        [DebuggerStepThrough]
        public Task<TEntity> FetchSingleAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : EntityBase2, IEntity2 =>
            ((IDataAccessAdapter) this).FetchSingleAsync(query, cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        //     Fetches the single entity of the set returned by the query and returns that entity.
        //     If there are no elements or more than 1 element, a NotSupportedException will
        //     be thrown.
        //
        // Parameters:
        //   adapter:
        //     The adapter.
        //
        //   query:
        //     The query.
        //
        // Type parameters:
        //   TEntity:
        //     The type of the entity.
        //
        // Returns:
        //     the first entity in the resultset
        //
        // Exceptions:
        //   T:System.NotSupportedException:
        //
        //   T:System.NotSupportedException:
        //     Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        //     value in the resultset.
        public Task<TEntity> FetchSingleAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2 =>
            ((IDataAccessAdapter) this).FetchSingleAsync(query);
        #endregion
    }
}