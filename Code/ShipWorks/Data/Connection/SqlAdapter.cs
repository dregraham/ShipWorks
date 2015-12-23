using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Interapptive.Shared.Data;
using log4net;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Adapter;
using ShipWorks.Data.Adapter.Custom;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.SqlServer.General;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    public sealed class SqlAdapter : DataAccessAdapter
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlAdapter));

        // Allows us to use a user-configured database.
        static CatalogNameOverwriteHashtable catalogNameOverwrites = new CatalogNameOverwriteHashtable();

        // Indicates if we should be logging all InfoMessage events from the underlying connection
        bool logInfoMessages = false;

        // Indiciates if the SET IDENTITY INSERT should be set before inserts, allowing pk values to be explicitly set
        bool identityInsert = false;

        // Lets us track if any entity is saved in a recursive save operation
        bool entitySaved = false;

        // The user passes in the connection to use
        SqlConnection overrideConnection;

        // The active TransactionScope, of the adapter was instructed to be required to be in a transaction
        System.Transactions.TransactionScope transactionScope;

        // LLBLgen starts a new "regular" transaction when recursive saving\deleting\updating.  The problem is then if there is an error, it does a direct call to "Rollback",
        // even though it should really wait and see what the wrapping transaction scope vote ends up doing.  This way LLBLgen sees that there is already
        // a regular transaction and doesn't do that.  Doing "StartTransaction" when InSystemTransaction is true is basically just setting the internal
        // isTransactionInProgress flag to true.
        bool inFakePhysicalTranscation = false;

        /// <summary>
        /// Raised when the transaction completes if the adapter participates in an ambient transaction.
        /// </summary>
        public event System.Transactions.TransactionCompletedEventHandler TransactionCompleted;

        // Needed to keep LLBLgen from trying to create its own trans for a recursive save
        private static System.Reflection.FieldInfo fieldIsTransactionInProgress;

        // Needed to allow SqlAdapter to use an existing SqlTransaction
        private static System.Reflection.FieldInfo fieldPhysicalTransaction;

        /// <summary>
        /// Static constructor
        /// </summary>
        static SqlAdapter()
        {
            catalogNameOverwrites.Add("ShipWorks", "");
            catalogNameOverwrites.Add("ShipWorksLocal", "");

            SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel.SqlServer2005);

            fieldIsTransactionInProgress = typeof(DataAccessAdapterBase).GetField("_isTransactionInProgress", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (fieldIsTransactionInProgress == null)
            {
                throw new InvalidOperationException("Could not get _isTransactionInProgress field");
            }

            fieldPhysicalTransaction = typeof(DataAccessAdapterBase).GetField("_physicalTransaction", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
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
        public SqlAdapter(SqlConnection connectionToUse)
            : base("", true, catalogNameOverwrites, null)
        {
            this.overrideConnection = connectionToUse;

            InitializeCommon();
        }

        /// <summary>
        /// Constructor that specifies the connection that the adapter should use
        /// </summary>
        public SqlAdapter(SqlConnection connectionToUse, SqlTransaction transactionToUse)
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
                    new System.Transactions.TransactionOptions { IsolationLevel = isolation, Timeout = SqlCommandProvider.DefaultTimeout });
            }

            return null;
        }

        /// <summary>
        /// Common initialization that should be called by all constructors
        /// </summary>
        private void InitializeCommon()
        {
            CommandTimeOut = (int) SqlCommandProvider.DefaultTimeout.TotalSeconds;

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
            SqlConnection existingConnection = overrideConnection;

            // If a connection was passed in, we have to prevent the base implementation from
            // closing it.
            if (overrideConnection != null && overrideConnection.State == ConnectionState.Open)
            {
                ensureOverrideOpenAfterDispose = true;

                System.Reflection.FieldInfo activeConnection = typeof(DataAccessAdapterBase).GetField(
                    "_activeConnection",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                if (activeConnection == null)
                {
                    throw new InvalidOperationException("Coult not find DataAccessAdapterBase._activeConnection.");
                }

                // Clear it, so it doesnt get disposed by base. OverrideConnection will be used when activeConnection is
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
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void OpenConnection()
        {
            if (InComPlusTransaction)
            {
                throw new NotSupportedException("Cannot be in com plus transaction due to custom OpenConnection functionality.");
            }

            IDbConnection connection = GetActiveConnection();

            if (connection.State != ConnectionState.Open)
            {
                ConnectionMonitor.OpenConnection((SqlConnection) connection);
            }
        }

        /// <summary>
        /// Intercept creation of the connection to override connection if necessary
        /// </summary>
        protected override IDbConnection CreateNewPhysicalConnection(string connectionString)
        {
            SqlConnection con;

            if (overrideConnection != null)
            {
                con = overrideConnection;
            }
            else
            {
                con = (SqlConnection) base.CreateNewPhysicalConnection(connectionString);
            }

            if (logInfoMessages)
            {
                con.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
            }

            return con;
        }

        /// <summary>
        /// Transaction is commiting
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
        void OnTransactionCompleted(object sender, System.Transactions.TransactionEventArgs e)
        {
            if (TransactionCompleted != null)
            {
                TransactionCompleted(this, e);
            }
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

                // Since LLBLgen thought we were in a "real" transaction since we set that flag, it didn't autoclose the connection
                if (!KeepConnectionOpen)
                {
                    CloseConnection();
                }
            }
        }

        #endregion

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

                SqlConnection con = (SqlConnection) GetActiveConnection();

                if (value)
                {
                    con.InfoMessage += new SqlInfoMessageEventHandler(OnInfoMessage);
                }
                else
                {
                    con.InfoMessage -= new SqlInfoMessageEventHandler(OnInfoMessage);
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

        #endregion

        #region Utility

        /// <summary>
        /// Save the given entity, and automatically refetch it back.  Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        public bool SaveAndRefetch(IEntity2 entity)
        {
            return SaveAndRefetch(entity, true);
        }

        /// <summary>
        /// Save the given entity, and automatically refetch it back.    Return strue if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        public bool SaveAndRefetch(IEntity2 entity, bool recurse)
        {
            entitySaved = false;

            base.SaveEntity(entity, true, recurse);

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
        public override int CallActionStoredProcedure(string storedProcedureToCall, SqlParameter[] parameters)
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
        /// Executes the passed in retrieval query and returns the results in thedatatable
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

                // FK RI error number
                if (error.Number == 547)
                {
                    string message = error.Message;

                    Match match = Regex.Match(error.Message, "table \"dbo.([^\"]+)");
                    if (match.Success)
                    {
                        string table = match.Groups[1].Value;

                        // Utilize the fact that we use camel casing for table names
                        string friendly = new string(table.ToCharArray().SelectMany(c => char.IsUpper(c) ? new char[] { ' ', c } : new char[] { c }).ToArray()).ToLower().Trim();

                        if (message.IndexOf("DELETE statement") != -1)
                        {
                            message = string.Format("A parent could not be deleted because it still has {0} children.", friendly);
                        }
                        else
                        {
                            message = string.Format("A child could not be saved because its parent {0} has been deleted.", friendly);
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
        public static SqlAdapter Default
        {
            get
            {
                return new SqlAdapter();
            }
        }

        /// <summary>
        /// Create a new SqlAdapter
        /// </summary>
        public static SqlAdapter Create(bool inTransaction) => new SqlAdapter(inTransaction);

        #endregion

        #region Identity Inserts

        /// <summary>
        /// Indiciates if the SET IDENTITY INSERT should be set before inserts, allowing pk values to be explicitly set
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
                RemoveIdentityProperty(persistenceInfoObjects, fieldCores);
            }

            IActionQuery saveQuery = CreateDynamicQueryEngine().CreateInsertDQ(fieldCores, persistenceInfoObjects, this.GetActiveConnection());

            if (identityInsert)
            {
                BatchActionQuery query = (BatchActionQuery) saveQuery;

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
        private void RemoveIdentityProperty(IFieldPersistenceInfo[] infos, IEntityFieldCore[] fields)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                IFieldPersistenceInfo info = infos[i];

                if (info.IsIdentity)
                {
                    infos[i] = CloneWithIdentityOff(info);
                    fields[i] = CloneWithReadOnlyOff(fields[i]);
                }
            }
        }

        /// <summary>
        /// Clone the persistance info, setting the identity value off.
        /// </summary>
        private IFieldPersistenceInfo CloneWithIdentityOff(IFieldPersistenceInfo info)
        {
            IFieldPersistenceInfo clone = new FieldPersistenceInfo(
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
                 info.IdentityValueSequenceName,
                 info.TypeConverterToUse,
                 info.ActualDotNetType);

            return clone;
        }

        /// <summary>
        /// Clone the given field with the ReadOnly flag turned off
        /// </summary>
        private IEntityFieldCore CloneWithReadOnlyOff(IEntityFieldCore field)
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
                   field.Precision));

            clone.AggregateFunctionToApply = field.AggregateFunctionToApply;
            clone.Alias = field.Alias;
            clone.ForcedCurrentValueWrite(field.CurrentValue, field.DbValue);
            clone.ExpressionToApply = field.ExpressionToApply;
            clone.IsChanged = field.IsChanged;
            clone.IsNull = field.IsNull;
            clone.LinkedSuperTypeField = field.LinkedSuperTypeField;
            clone.ObjectAlias = field.ObjectAlias;

            return clone;
        }

        #endregion
    }
}
