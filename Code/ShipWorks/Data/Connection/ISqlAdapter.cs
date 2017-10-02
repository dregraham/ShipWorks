using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Interapptive.Shared;
using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.QuerySpec;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// A custom ShipWorks DataAccessAdapter
    /// </summary>
    [SuppressMessage("SonarLint", "S107: Methods should not have too many parameters",
        Justification = "These methods are part of LLBLgen and are only included here so that we can test code that uses them")]
    [SuppressMessage("SonarLint", "S103: Lines should not be too long",
        Justification = "The long lines are from LLBLgen")]
    public interface ISqlAdapter : IDataAccessCore, IDisposable, ITransactionController
    {
        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        bool SaveAndRefetch(IEntity2 entity);

        /// <summary>
        /// Get this SqlAdapter as an IDataAccessAdapter
        /// </summary>
        IDataAccessAdapter AsDataAccessAdapter();

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        Task<bool> SaveAndRefetchAsync(IEntity2 entity);

        #region "IDataAccess adapter methods"

        /// <summary>
        /// The flag (default true) to signal the adapter that entities participating in
        /// a transaction controlled by this adapter are tracked during the transaction and
        /// which values are rolled back after a rollback of the transaction itself. Only
        /// set this flag to false if the entities participating in a transaction are not
        /// kept in memory during or after the transaction's life time.
        /// </summary>
        bool KeepTrackOfTransactionParticipants { get; set; }

        /// <summary>
        /// Gets or sets the parameterized prefetch path threshold. This threshold is used
        /// to determine when the prefetch path logic should switch to a subquery or when
        /// it should use a WHERE field IN (value1, value2, ... valueN) construct, based
        /// on the # of elements in the parent collection. If that # of elements exceeds
        /// this threshold, a subquery is constructed, otherwise field IN (value1, value2,
        /// ...) construct is used. The default value is 50. On average, this is faster than
        /// using a subquery which returns 50 elements. Use this to tune prefetch path fetch
        /// logic for your particular needs. This threshold is also used to determine if
        /// paging is possible. A page size bigger than this threshold will disable the paging
        /// functionality when using paging + prefetch paths.
        /// </summary>
        /// <remarks>
        /// Testing showed that values larger than 300 will be slower than a subquery. Special
        /// thanks to Marcus Mac Innes (http://www.styledesign.biz) for this optimization
        /// code.
        /// </remarks>
        int ParameterisedPrefetchPathThreshold { get; set; }

        /// <summary>
        /// Gets / sets the isolation level a transaction should use. Setting this during
        /// a transaction in progress has no effect on the current running transaction.
        /// </summary>
        IsolationLevel TransactionIsolationLevel { get; set; }

        /// <summary>
        /// Gets the name of the transaction. Setting this during a transaction in progress
        /// has no effect on the current running transaction.
        /// </summary>
        string TransactionName { get; set; }

        /// <summary>
        /// Gets / sets the connection string to use for the connection with the database.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Gets / sets KeepConnectionOpen, a flag used to keep open connections after a
        /// database action has finished.
        /// </summary>
        bool KeepConnectionOpen { get; set; }

        /// <summary>
        /// Gets / sets the timeout value to use with the command object(s) created by the
        /// adapter. Default is 30 seconds Set this prior to calling a method which executes
        /// database logic.
        /// </summary>
        int CommandTimeOut { get; set; }

        /// <summary>
        /// Gets a value indicating whether a System.Transactions transaction is going on.
        /// If not, false is returned.
        /// </summary>
        bool InSystemTransaction { get; }

        /// <summary>
        /// Gets or sets the active recovery strategy to use with supported actions on this
        /// DataAccessAdapter. If null (default), no recovery strategy is used and all exceptions
        /// are fatal.
        /// </summary>
        RecoveryStrategyBase ActiveRecoveryStrategy { get; set; }

        /// <summary>
        /// Gets the function mappings for the DQE related to this object. These function
        /// mappings are static and therefore not changeable.
        /// </summary>
        FunctionMappingStore FunctionMappings { get; }

        /// <summary>
        /// Closes the active connection. If no connection is available or the connection
        /// is closed, nothing is done.
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Creates a new predicate expression which filters on the primary key fields and
        /// the set values for the given primary key fields. If no primary key fields are
        /// specified, null is returned.
        /// </summary>
        /// <param name="primaryKeyFields">
        /// ArrayList with IEntityField2 instances which form the primary key for which the
        /// filter has to be constructed
        /// </param>
        /// <returns>
        /// filled in predicate expression or null if no primary key fields are specified.
        /// </returns>
        /// <remarks>
        /// Call this method passing IEntity2.Fields.PrimaryKeyFields
        /// </remarks>
        IPredicateExpression CreatePrimaryKeyFilter(IList primaryKeyFields);

        /// <summary>
        /// Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        /// from the persistent storage if they match the filter supplied in filterBucket.
        /// </summary>
        /// <typeparam name="TypeOfEntity">
        /// The type of the entity to retrieve persistence information.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to delete
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        /// <remarks>
        /// Not supported for entities which are in a TargetPerEntity hierarchy.
        /// </remarks>
        int DeleteEntitiesDirectly(Type typeOfEntity, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        /// from the persistent storage if they match the filter supplied in filterBucket.
        /// </summary>
        /// <param name="entityName">
        /// The name of the entity to retrieve persistence information. For example "CustomerEntity".
        /// This name can be retrieved from an existing entity's Name property.
        /// </param>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to delete
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        int DeleteEntitiesDirectly(string entityName, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.Type,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        /// Deletes all entities of the type passed in from the persistent storage if they
        /// match the filter supplied in filterBucket.
        /// </summary>
        /// <typeparam name="TypeOfEntity">
        /// The type of the entity to retrieve persistence information.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to delete
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// typeOfEntity;typeOfEntity can't be null
        /// </exception>
        /// <remarks>
        /// Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        /// performs authorization. Use this overload instead of the one which accepts a
        /// name instead of a type instance if you want to have authorization support at
        /// runtime.
        /// </remarks>
        Task<int> DeleteEntitiesDirectlyAsync(Type typeOfEntity, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.String,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        /// Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        /// from the persistent storage if they match the filter supplied in filterBucket.
        /// </summary>
        /// <param name="entityName">
        /// The name of the entity to retrieve persistence information. For example "CustomerEntity".
        /// This name can be retrieved from an existing entity's LLBLGenProEntityName property.
        /// </param>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to delete
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        /// <remarks>
        /// Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        /// doesn't support Authorization or Auditing. It's recommended, if you want to use
        /// authorization and/or auditing on this method, use the overload of DeleteEntitiesDirectly
        /// which accepts a type.
        /// </remarks>
        Task<int> DeleteEntitiesDirectlyAsync(string entityName, IRelationPredicateBucket filterBucket, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.String,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        /// Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        /// from the persistent storage if they match the filter supplied in filterBucket.
        /// </summary>
        /// <param name="entityName">
        /// The name of the entity to retrieve persistence information. For example "CustomerEntity".
        /// This name can be retrieved from an existing entity's LLBLGenProEntityName property.
        /// </param>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to delete
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        /// <remarks>
        /// Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        /// doesn't support Authorization or Auditing. It's recommended, if you want to use
        /// authorization and/or auditing on this method, use the overload of DeleteEntitiesDirectly
        /// which accepts a type.
        /// </remarks>
        Task<int> DeleteEntitiesDirectlyAsync(string entityName, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.Type,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        /// Deletes all entities of the type passed in from the persistent storage if they
        /// match the filter supplied in filterBucket.
        /// </summary>
        /// <typeparam name="TypeOfEntity">
        /// The type of the entity to retrieve persistence information.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to delete
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// typeOfEntity;typeOfEntity can't be null
        /// </exception>
        /// <remarks>
        /// Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        /// performs authorization. Use this overload instead of the one which accepts a
        /// name instead of a type instance if you want to have authorization support at
        /// runtime.
        /// </remarks>
        Task<int> DeleteEntitiesDirectlyAsync(Type typeOfEntity, IRelationPredicateBucket filterBucket, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes the specified entity from the persistent storage. The entity is not usable
        /// after this call, the state is set to OutOfSync. Will use the current transaction
        /// if a transaction is in progress.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity instance to delete from the persistent storage
        /// </param>
        /// <returns>
        /// true if the delete was successful, otherwise false.
        /// </returns>
        bool DeleteEntity(IEntity2 entityToDelete);

        /// <summary>
        /// Deletes the specified entity from the persistent storage. The entity is not usable
        /// after this call, the state is set to OutOfSync. Will use the current transaction
        /// if a transaction is in progress.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity instance to delete from the persistent storage
        /// </param>
        /// <param name="deleteRestriction">
        /// Predicate expression, meant for concurrency checks in a delete query
        /// </param>
        /// <returns>
        /// true if the delete was successful, otherwise false.
        /// </returns>
        bool DeleteEntity(IEntity2 entityToDelete, IPredicateExpression deleteRestriction);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression).
        /// Deletes the specified entity from the persistent storage. The entity is not usable
        /// after this call, the state is set to OutOfSync. Will use the current transaction
        /// if a transaction is in progress.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity instance to delete from the persistent storage
        /// </param>
        /// <param name="deleteRestriction">
        /// Predicate expression, meant for concurrency checks in a delete query
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if the delete was successful, otherwise false.
        /// </returns>
        /// <exception cref="T:SD.LLBLGen.Pro.ORMSupportClasses.ORMConcurrencyException">
        /// Will throw an ORMConcurrencyException if the delete fails.
        /// </exception>
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, IPredicateExpression deleteRestriction, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression).
        /// Deletes the specified entity from the persistent storage. The entity is not usable
        /// after this call, the state is set to OutOfSync. Will use the current transaction
        /// if a transaction is in progress.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity instance to delete from the persistent storage
        /// </param>
        /// <param name="deleteRestriction">
        /// Predicate expression, meant for concurrency checks in a delete query
        /// </param>
        /// <returns>
        /// true if the delete was successful, otherwise false.
        /// </returns>
        /// <exception cref="T:SD.LLBLGen.Pro.ORMSupportClasses.ORMConcurrencyException">
        /// Will throw an ORMConcurrencyException if the delete fails.
        /// </exception>
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, IPredicateExpression deleteRestriction);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        /// Deletes the specified entity from the persistent storage. The entity is not usable
        /// after this call, the state is set to OutOfSync. Will use the current transaction
        /// if a transaction is in progress. If the passed in entity has a concurrency predicate
        /// factory object, the returned predicate expression is used to restrict the delete
        /// process.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity instance to delete from the persistent storage
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if the delete was successful, otherwise false.
        /// </returns>
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        /// Deletes the specified entity from the persistent storage. The entity is not usable
        /// after this call, the state is set to OutOfSync. Will use the current transaction
        /// if a transaction is in progress. If the passed in entity has a concurrency predicate
        /// factory object, the returned predicate expression is used to restrict the delete
        /// process.
        /// </summary>
        /// <param name="entityToDelete">
        /// The entity instance to delete from the persistent storage
        /// </param>
        /// <returns>
        /// true if the delete was successful, otherwise false.
        /// </returns>
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete);

        /// <summary>
        /// Deletes all dirty objects inside the collection passed from the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available. Entities
        /// which are physically deleted from the persistent storage are marked with the
        /// state 'Deleted' but are not removed from the collection. If the passed in entity
        /// has a concurrency predicate factory object, the returned predicate expression
        /// is used to restrict the delete process.
        /// </summary>
        /// <param name="collectionToDelete">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        int DeleteEntityCollection(IEntityCollection2 collectionToDelete);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        /// Deletes all dirty objects inside the collection passed from the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available. Entities
        /// which are physically deleted from the persistent storage are marked with the
        /// state 'Deleted' but are not removed from the collection.
        /// </summary>
        /// <param name="collectionToDelete">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        Task<int> DeleteEntityCollectionAsync(IEntityCollection2 collectionToDelete, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        /// Deletes all dirty objects inside the collection passed from the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available. Entities
        /// which are physically deleted from the persistent storage are marked with the
        /// state 'Deleted' but are not removed from the collection.
        /// </summary>
        /// <param name="collectionToDelete">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <returns>
        /// the amount of physically deleted entities
        /// </returns>
        Task<int> DeleteEntityCollectionAsync(IEntityCollection2 collectionToDelete);

        /// <summary>
        /// Executes the passed in action query and, if not null, runs it inside the passed
        /// in transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// ActionQuery to execute.
        /// </param>
        /// <returns>
        /// execution result, which is the amount of rows affected (if applicable)
        /// </returns>
        int ExecuteActionQuery(IActionQuery queryToExecute);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteActionQuery(SD.LLBLGen.Pro.ORMSupportClasses.IActionQuery).
        /// Executes the passed in action query and, if not null, runs it inside the passed
        /// in transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// ActionQuery to execute.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// execution result, which is the amount of rows affected (if applicable)
        /// </returns>
        Task<int> ExecuteActionQueryAsync(IActionQuery queryToExecute, CancellationToken cancellationToken);

        /// <summary>
        /// Executes the passed in retrieval query and returns the results in the datatable
        /// specified using the passed in data-adapter. It sets the connection object of
        /// the command object of query object passed in to the connection object of this
        /// class.
        /// </summary>
        /// <param name="queryToExecute">
        /// Retrieval query to execute
        /// </param>
        /// <typeparam name="TableToFill">
        /// DataTable to fill
        /// </typeparam>
        /// <param name="fieldsPersistenceInfo">
        /// Fields persistence info objects for the fields used for the query. Required for
        /// type conversion on values.
        /// </param>
        /// <returns>
        /// true if succeeded, false otherwise
        /// </returns>
        bool ExecuteMultiRowDataTableRetrievalQuery(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, DataTable tableToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo);

        /// <summary>
        /// Executes the passed in retrieval query and returns the results as a datatable
        /// using the passed in data-adapter. It sets the connection object of the command
        /// object of query object passed in to the connection object of this class.
        /// </summary>
        /// <param name="queryToExecute">
        /// Retrieval query to execute
        /// </param>
        /// <param name="fieldsPersistenceInfo">
        /// Fields persistence info objects for the fields used for the query. Required for
        /// type conversion on values.
        /// </param>
        /// <returns>
        /// DataTable with the rows requested
        /// </returns>
        DataTable ExecuteMultiRowDataTableRetrievalQuery(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, IFieldPersistenceInfo[] fieldsPersistenceInfo);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteMultiRowDataTableRetrievalQuery(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery,System.Data.Common.DbDataAdapter,System.Data.DataTable,SD.LLBLGen.Pro.ORMSupportClasses.IFieldPersistenceInfo[]).
        /// Executes the passed in retrieval query and returns the results in the datatable
        /// specified using the passed in data-adapter. It sets the connection object of
        /// the command object of query object passed in to the connection object of this
        /// class.
        /// </summary>
        /// <param name="queryToExecute">
        /// Retrieval query to execute
        /// </param>
        /// <typeparam name="TableToFill">
        /// DataTable to fill
        /// </typeparam>
        /// <param name="fieldsPersistenceInfo">
        /// Fields persistence info objects for the fields used for the query. Required for
        /// type conversion on values.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if succeeded, false otherwise
        /// </returns>
        Task<bool> ExecuteMultiRowDataTableRetrievalQueryAsync(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, DataTable tableToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, CancellationToken cancellationToken);

        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the passed
        /// in transaction. Used to read 1 or more rows. It sets the connection object of
        /// the command object of query object passed in to the connection object of this
        /// class.
        /// </summary>
        /// <param name="queryToExecute">
        /// Retrieval query to execute
        /// </param>
        /// <param name="entityFactory">
        /// the factory object which can produce the entities this method has to fill.
        /// </param>
        /// <param name="collectionToFill">
        /// Collection to fill with the retrieved rows.
        /// </param>
        /// <param name="fieldsPersistenceInfo">
        /// The persistence information for the fields of the entity created by entityFactory
        /// </param>
        /// <param name="allowDuplicates">
        /// Flag to signal if duplicates in the datastream should be loaded into the collection
        /// (true) or not (false)
        /// </param>
        /// <param name="fieldsUsedForQuery">
        /// Fields used for producing the query
        /// </param>
        [NDependIgnoreTooManyParams]
        void ExecuteMultiRowRetrievalQuery(IRetrievalQuery queryToExecute, IEntityFactory2 entityFactory, IEntityCollection2 collectionToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, bool allowDuplicates, IEntityFields2 fieldsUsedForQuery);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteMultiRowRetrievalQuery(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery,SD.LLBLGen.Pro.ORMSupportClasses.IEntityFactory2,SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,SD.LLBLGen.Pro.ORMSupportClasses.IFieldPersistenceInfo[],System.Boolean,SD.LLBLGen.Pro.ORMSupportClasses.IEntityFields2).
        /// Executes the passed in retrieval query and, if not null, runs it inside the passed
        /// in transaction. Used to read 1 or more rows. It sets the connection object of
        /// the command object of query object passed in to the connection object of this
        /// class.
        /// </summary>
        /// <param name="queryToExecute">
        /// Retrieval query to execute
        /// </param>
        /// <param name="entityFactory">
        /// the factory object which can produce the entities this method has to fill.
        /// </param>
        /// <param name="collectionToFill">
        /// Collection to fill with the retrieved rows.
        /// </param>
        /// <param name="fieldsPersistenceInfo">
        /// The persistence information for the fields of the entity created by entityFactory
        /// </param>
        /// <param name="allowDuplicates">
        /// Flag to signal if duplicates in the datastream should be loaded into the collection
        /// (true) or not (false)
        /// </param>
        /// <param name="fieldsUsedForQuery">
        /// Fields used for producing the query
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        [NDependIgnoreTooManyParams]
        Task ExecuteMultiRowRetrievalQueryAsync(IRetrievalQuery queryToExecute, IEntityFactory2 entityFactory, IEntityCollection2 collectionToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, bool allowDuplicates, IEntityFields2 fieldsUsedForQuery, CancellationToken cancellationToken);

        /// <summary>
        /// Executes the passed in query as a scalar query and returns the value returned
        /// from this scalar execution.
        /// </summary>
        /// <param name="queryToExecute">
        /// a scalar query, which is a SELECT query which returns a single value
        /// </param>
        /// <returns>
        /// the scalar value returned from the query.
        /// </returns>
        object ExecuteScalarQuery(IRetrievalQuery queryToExecute);

        /// <summary>
        /// Executes the passed in retrieval query and, if not null, runs it inside the passed
        /// in transaction. Used to read 1 row. It sets the connection object of the command
        /// object of query object passed in to the connection object of this class.
        /// </summary>
        /// <param name="queryToExecute">
        /// Retrieval query to execute
        /// </param>
        /// <param name="fieldsToFill">
        /// The IEntityFields2 object to store the fetched data in
        /// </param>
        /// <param name="fieldsPersistenceInfo">
        /// The IFieldPersistenceInfo objects for the fieldsToFill fields
        /// </param>
        void ExecuteSingleRowRetrievalQuery(IRetrievalQuery queryToExecute, IEntityFields2 fieldsToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo);

        /// <summary>
        /// Executes the specified plain SQL query using this adapter. Every parameter value
        /// is converted into one or more parameters which have to be pre-defined in the
        /// sqlQuery
        /// </summary>
        /// <param name="sqlQuery">
        /// The SQL query to execute. Should contain parameter names for the parameter values,
        /// or placeholders for parameter sets. See documentation for details regarding format
        /// specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <returns>
        /// The value returned by the executed DbCommand. In general this is the number of
        /// rows affected by the executed sqlQuery
        /// </returns>
        int ExecuteSQL(string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteSQL(System.String,System.Object)
        /// Executes the specified plain SQL query using this adapter. Every parameter value
        /// is converted into one or more parameters which have to be pre-defined in the
        /// sqlQuery
        /// </summary>
        /// <param name="sqlQuery">
        /// The SQL query to execute. Should contain parameter names for the parameter values,
        /// or placeholders for parameter sets. See documentation for details regarding format
        /// specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <returns>
        /// The value returned by the executed DbCommand. In general this is the number of
        /// rows affected by the executed sqlQuery
        /// </returns>
        Task<int> ExecuteSQLAsync(string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteSQL(System.String,System.Object)
        /// Executes the specified plain SQL query using this adapter. Every parameter value
        /// is converted into one or more parameters which have to be pre-defined in the
        /// sqlQuery
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query to execute. Should contain parameter names for the parameter values,
        /// or placeholders for parameter sets. See documentation for details regarding format
        /// specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <returns>
        /// The value returned by the executed DbCommand. In general this is the number of
        /// rows affected by the executed sqlQuery
        /// </returns>
        Task<int> ExecuteSQLAsync(CancellationToken cancellationToken, string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in and executes that retrieval query
        /// to return an open, ready to use IDataReader. The datareader's command behavior
        /// is set to the readerBehavior passed in. If a transaction is in progress, the
        /// command is wired to the transaction.
        /// </summary>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="sortClauses">
        /// The sort clauses.
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        [NDependIgnoreTooManyParams]
#pragma warning disable S107 // Methods should not have too many parameters
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, int pageNumber, int pageSize);
#pragma warning restore S107 // Methods should not have too many parameters

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in and executes that retrieval query
        /// to return an open, ready to use IDataReader. The datareader's command behavior
        /// is set to the readerBehavior passed in. If a transaction is in progress, the
        /// command is wired to the transaction.
        /// </summary>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        [NDependIgnoreTooManyParams]
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, bool allowDuplicates);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in and executes that retrieval query
        /// to return an open, ready to use IDataReader. The datareader's command behavior
        /// is set to the readerBehavior passed in. If a transaction is in progress, the
        /// command is wired to the transaction.
        /// </summary>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="sortClauses">
        /// The sort clauses.
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        [NDependIgnoreTooManyParams]
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in and executes that retrieval query
        /// to return an open, ready to use IDataReader. The datareader's command behavior
        /// is set to the readerBehavior passed in. If a transaction is in progress, the
        /// command is wired to the transaction.
        /// </summary>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="sortClauses">
        /// The sort clauses.
        /// </param>
        /// <param name="groupByClause">
        /// The group by clause.
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        [NDependIgnoreTooManyParams]
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IGroupByCollection groupByClause, bool allowDuplicates, int pageNumber, int pageSize);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in and executes that retrieval query
        /// to return an open, ready to use IDataReader. The datareader's command behavior
        /// is set to the readerBehavior passed in. If a transaction is in progress, the
        /// command is wired to the transaction.
        /// </summary>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        IDataReader FetchDataReader(CommandBehavior readerBehavior, QueryParameters parameters);

        /// <summary>
        /// Executes the passed in retrieval query and returns an open, ready to use IDataReader.
        /// The datareader's command behavior is set to the readerBehavior passed in. If
        /// a transaction is in progress, the command is wired to the transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        IDataReader FetchDataReader(IRetrievalQuery queryToExecute, CommandBehavior readerBehavior);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchDataReader(System.Data.CommandBehavior,SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        /// Creates a new Retrieval query from the elements passed in and executes that retrieval query
        /// to return an open, ready to use IDataReader. The datareader's command behavior
        /// is set to the readerBehavior passed in. If a transaction is in progress, the
        /// command is wired to the transaction.
        /// </summary>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        /// parameters
        /// </exception>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open
        /// </remarks>
        Task<IDataReader> FetchDataReaderAsync(CommandBehavior readerBehavior, QueryParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchDataReader(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery,System.Data.CommandBehavior).
        /// Executes the passed in retrieval query and returns an open, ready to use IDataReader.
        /// The datareader's command behavior is set to the readerBehavior passed in. If
        /// a transaction is in progress, the command is wired to the transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        /// <param name="readerBehavior">
        /// The reader behavior to set.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// Open, ready to use IDataReader
        /// </returns>
        /// <remarks>
        /// Advanced functionality: be aware that the datareader returned is open, and the
        /// dataaccessadapter's connection is also open. It can be, if the query is set to
        /// cache its resultset, that the reader returned is actually a reader over the cached
        /// resultset. If you ordered the query to be cached, be sure to pass queryToExecute
        /// to the FetchProjection method to cache the resultset.
        /// </remarks>
        Task<IDataReader> FetchDataReaderAsync(IRetrievalQuery queryToExecute, CommandBehavior readerBehavior, CancellationToken cancellationToken);

        /// <summary>
        /// Fetches an entity from the persistent storage into the passed in Entity2 object
        /// using a primary key filter. The primary key fields of the entity passed in have
        /// to have the primary key values. (Example: CustomerID has to have a value, when
        /// you want to fetch a CustomerEntity from the persistent storage into the passed
        /// in object). All fields specified in excludedFields are excluded from the fetch
        /// so the entity won't get any value set for those fields. excludedFields can be
        /// null or empty, in which case all fields are fetched (default).
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored. The primary
        /// key fields have to have a value.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress, so MVCC or other
        /// concurrency scheme used by the database can be utilized
        /// </remarks>
        bool FetchEntity(IEntity2 entityToFetch, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Fetches an entity from the persistent storage into the passed in Entity2 object
        /// using a primary key filter. The primary key fields of the entity passed in have
        /// to have the primary key values. (Example: CustomerID has to have a value, when
        /// you want to fetch a CustomerEntity from the persistent storage into the passed
        /// in object)
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored. The primary
        /// key fields have to have a value.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress, so MVCC or other
        /// concurrency scheme used by the database can be utilized
        /// </remarks>
        bool FetchEntity(IEntity2 entityToFetch, IPrefetchPath2 prefetchPath, Context contextToUse);

        /// <summary>
        /// Fetches an entity from the persistent storage into the passed in Entity2 object
        /// using a primary key filter. The primary key fields of the entity passed in have
        /// to have the primary key values. (Example: CustomerID has to have a value, when
        /// you want to fetch a CustomerEntity from the persistent storage into the passed
        /// in object)
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored. The primary
        /// key fields have to have a value.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress, so MVCC or other
        /// concurrency scheme used by the database can be utilized
        /// </remarks>
        bool FetchEntity(IEntity2 entityToFetch, Context contextToUse);

        /// <summary>
        /// Fetches an entity from the persistent storage into the passed in Entity2 object
        /// using a primary key filter. The primary key fields of the entity passed in have
        /// to have the primary key values. (Example: CustomerID has to have a value, when
        /// you want to fetch a CustomerEntity from the persistent storage into the passed
        /// in object)
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored. The primary
        /// key fields have to have a value.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress, so MVCC or other
        /// concurrency scheme used by the database can be utilized
        /// </remarks>
        bool FetchEntity(IEntity2 entityToFetch);

        /// <summary>
        /// Fetches an entity from the persistent storage into the passed in Entity2 object
        /// using a primary key filter. The primary key fields of the entity passed in have
        /// to have the primary key values. (Example: CustomerID has to have a value, when
        /// you want to fetch a CustomerEntity from the persistent storage into the passed
        /// in object)
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored. The primary
        /// key fields have to have a value.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress, so MVCC or other
        /// concurrency scheme used by the database can be utilized
        /// </remarks>
        bool FetchEntity(IEntity2 entityToFetch, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. It will apply paging and it will from there use a prefetch path fetch
        /// using the read page. It's important that pageSize is smaller than the set SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold.
        /// If pagesize is larger than the limits set for the SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold
        /// value, the query is likely to be slower than expected, though will work. If pageNumber
        /// / pageSize are set to values which disable paging, a normal prefetch path fetch
        /// will be performed.
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="prefetchPath">
        /// Prefetch path to use.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <param name="pageNumber">
        /// the page number to retrieve. First page is 1. When set to 0, no paging logic
        /// is applied
        /// </param>
        /// <param name="pageSize">
        /// the size of the page. When set to 0, no paging logic is applied
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        /// <remarks>
        /// Special thanks to Marcus Mac Innes (http://www.styledesign.biz) for the paging
        /// optimization code.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath, ExcludeIncludeFieldsList excludedIncludedFields, int pageNumber, int pageSize);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. It will apply paging and it will from there use a prefetch path fetch
        /// using the read page. It's important that pageSize is smaller than the set ParameterisedPrefetchPathThreshold.
        /// It will work, though if pagesize is larger than the limits set for the ParameterisedPrefetchPathThreshold
        /// value, the query is likely to be slower than expected.
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="prefetchPath">
        /// Prefetch path to use.
        /// </param>
        /// <param name="pageNumber">
        /// the page number to retrieve. First page is 1. When set to 0, no paging logic
        /// is applied
        /// </param>
        /// <param name="pageSize">
        /// the size of the page. When set to 0, no paging logic is applied
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        [NDependIgnoreTooManyParams]
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath, int pageNumber, int pageSize);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched.
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="pageNumber">
        /// the page number to retrieve. First page is 1. When set to 0, no paging logic
        /// is applied
        /// </param>
        /// <param name="pageSize">
        /// the size of the page. When set to 0, no paging logic is applied
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        [NDependIgnoreTooManyParams]
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, int pageNumber, int pageSize);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched.
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the parameters
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. It will apply paging and it will from there use a prefetch path fetch
        /// using the read page. It's important that pageSize is smaller than the set SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold.
        /// If pagesize is larger than the limits set for the SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold
        /// value, the query is likely to be slower than expected, though will work. If pageNumber
        /// / pageSize are set to values which disable paging, a normal prefetch path fetch
        /// will be performed.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        void FetchEntityCollection(QueryParameters parameters);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, ExcludeIncludeFieldsList excludedIncludedFields, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. This overload doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. This overload returns all found entities and doesn't apply sorting
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched.
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        [NDependIgnoreTooManyParams]
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Fetches one or more entities which match the filter information in the filterBucket
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched.
        /// </summary>
        /// <param name="collectionToFill">
        /// EntityCollection object containing an entity factory which has to be filled
        /// </param>
        /// <param name="filterBucket">
        /// filter information for retrieving the entities. If null, all entities are returned
        /// of the type created by the factory in the passed in EntityCollection instance.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of entities to return. If 0, all entities matching the filter
        /// are returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        /// Fetches one or more entities which match the filter information in the parameters
        /// into the EntityCollection passed. The entity collection object has to contain
        /// an entity factory object which will be the factory for the entity instances to
        /// be fetched. It will apply paging and it will from there use a prefetch path fetch
        /// using the read page. It's important that pageSize is smaller than the set SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold.
        /// If pagesize is larger than the limits set for the SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold
        /// value, the query is likely to be slower than expected, though will work. If pageNumber
        /// / pageSize are set to values which disable paging, a normal prefetch path fetch
        /// will be performed.
        /// </summary>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="T:System.ArgumentException">
        /// If the passed in collectionToFill doesn't contain an entity factory.
        /// </exception>
        /// <remarks>
        /// Async variant
        /// </remarks>
        Task FetchEntityCollectionAsync(QueryParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Fetches an entity from the persistent storage into the object specified using
        /// the filter specified. Use the entity's uniqueconstraint filter construction methods
        /// to construct the required uniqueConstraintFilter for the unique constraint you
        /// want to use.
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored.
        /// </param>
        /// <param name="uniqueConstraintFilter">
        /// The filter which should filter on fields with a unique constraint.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Fetches an entity from the persistent storage into the object specified using
        /// the filter specified. Use the entity's uniqueconstraint filter construction methods
        /// to construct the required uniqueConstraintFilter for the unique constraint you
        /// want to use.
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored.
        /// </param>
        /// <param name="uniqueConstraintFilter">
        /// The filter which should filter on fields with a unique constraint.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, IPrefetchPath2 prefetchPath, Context contextToUse);

        /// <summary>
        /// Fetches an entity from the persistent storage into the object specified using
        /// the filter specified. Use the entity's uniqueconstraint filter construction methods
        /// to construct the required uniqueConstraintFilter for the unique constraint you
        /// want to use.
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored.
        /// </param>
        /// <param name="uniqueConstraintFilter">
        /// The filter which should filter on fields with a unique constraint.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, Context contextToUse);

        /// <summary>
        /// Fetches an entity from the persistent storage into the object specified using
        /// the filter specified. Use the entity's uniqueconstraint filter construction methods
        /// to construct the required uniqueConstraintFilter for the unique constraint you
        /// want to use.
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored.
        /// </param>
        /// <param name="uniqueConstraintFilter">
        /// The filter which should filter on fields with a unique constraint.
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Fetches an entity from the persistent storage into the object specified using
        /// the filter specified. Use the entity's uniqueconstraint filter construction methods
        /// to construct the required uniqueConstraintFilter for the unique constraint you
        /// want to use.
        /// </summary>
        /// <param name="entityToFetch">
        /// The entity object in which the fetched entity data will be stored.
        /// </param>
        /// <param name="uniqueConstraintFilter">
        /// The filter which should filter on fields with a unique constraint.
        /// </param>
        /// <returns>
        /// true if the Fetch was successful, false otherwise
        /// </returns>
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter);

        /// <summary>
        /// Loads the data for the excluded fields specified in the list of excluded fields
        /// into all the entities in the entities collection passed in.
        /// </summary>
        /// <param name="entities">
        /// The entities to load the excluded field data into. The entities have to be either
        /// of the same type or have to be in the same inheritance hierarchy as the entity
        /// which factory is set in the collection.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The excludedIncludedFields object as it is used when fetching the passed in collection.
        /// If you used the excludedIncludedFields object to fetch only the fields in that
        /// list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        /// will fetch all other fields in the resultset for the entities in the collection
        /// excluding the fields in excludedIncludedFields.
        /// </param>
        /// <remarks>
        /// The field data is set like a normal field value set, so authorization is applied
        /// to it. This routine batches fetches to have at most 5*ParameterisedPrefetchPathThreshold
        /// of parameters per fetch. Keep in mind that most databases have a limit on the
        /// # of parameters per query.
        /// </remarks>
        void FetchExcludedFields(IEntityCollection2 entities, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Loads the data for the excluded fields specified in the list of excluded fields
        /// into the entity passed in.
        /// </summary>
        /// <param name="entity">
        /// The entity to load the excluded field data into.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The excludedIncludedFields object as it is used when fetching the passed in entity.
        /// If you used the excludedIncludedFields object to fetch only the fields in that
        /// list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        /// will fetch all other fields in the resultset for the entities in the collection
        /// excluding the fields in excludedIncludedFields.
        /// </param>
        /// <remarks>
        /// The field data is set like a normal field value set, so authorization is applied
        /// to it.
        /// </remarks>
        void FetchExcludedFields(IEntity2 entity, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        /// Loads the data for the excluded fields specified in the list of excluded fields
        /// into the entity passed in.
        /// </summary>
        /// <param name="entity">
        /// The entity to load the excluded field data into.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The excludedIncludedFields object as it is used when fetching the passed in entity.
        /// If you used the excludedIncludedFields object to fetch only the fields in that
        /// list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        /// will fetch all other fields in the resultset for the entities in the collection
        /// excluding the fields in excludedIncludedFields.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <remarks>
        /// The field data is set like a normal field value set, so authorization is applied
        /// to it.
        /// </remarks>
        Task FetchExcludedFieldsAsync(IEntity2 entity, ExcludeIncludeFieldsList excludedIncludedFields, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        /// Loads the data for the excluded fields specified in the list of excluded fields
        /// into all the entities in the entities collection passed in.
        /// </summary>
        /// <param name="entities">
        /// The entities to load the excluded field data into. The entities have to be either
        /// of the same type or have to be in the same inheritance hierarchy as the entity
        /// which factory is set in the collection.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The excludedIncludedFields object as it is used when fetching the passed in collection.
        /// If you used the excludedIncludedFields object to fetch only the fields in that
        /// list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        /// will fetch all other fields in the resultset for the entities in the collection
        /// excluding the fields in excludedIncludedFields.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="T:SD.LLBLGen.Pro.ORMSupportClasses.ORMGeneralOperationException">
        /// The entity factory of the passed in entities collection is null.
        /// </exception>
        /// <remarks>
        /// The field data is set like a normal field value set, so authorization is applied
        /// to it. This routine batches fetches to have at most 5*ParameterizedThreshold
        /// of parameters per fetch. Keep in mind that most databases have a limit on the
        /// # of parameters per query.
        /// </remarks>
        Task FetchExcludedFieldsAsync(IEntityCollection2 entities, ExcludeIncludeFieldsList excludedIncludedFields, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        /// Loads the data for the excluded fields specified in the list of excluded fields
        /// into the entity passed in.
        /// </summary>
        /// <param name="entity">
        /// The entity to load the excluded field data into.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The excludedIncludedFields object as it is used when fetching the passed in entity.
        /// If you used the excludedIncludedFields object to fetch only the fields in that
        /// list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        /// will fetch all other fields in the resultset for the entities in the collection
        /// excluding the fields in excludedIncludedFields.
        /// </param>
        /// <remarks>
        /// The field data is set like a normal field value set, so authorization is applied
        /// to it.
        /// </remarks>
        Task FetchExcludedFieldsAsync(IEntity2 entity, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        /// Loads the data for the excluded fields specified in the list of excluded fields
        /// into all the entities in the entities collection passed in.
        /// </summary>
        /// <param name="entities">
        /// The entities to load the excluded field data into. The entities have to be either
        /// of the same type or have to be in the same inheritance hierarchy as the entity
        /// which factory is set in the collection.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The excludedIncludedFields object as it is used when fetching the passed in collection.
        /// If you used the excludedIncludedFields object to fetch only the fields in that
        /// list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        /// will fetch all other fields in the resultset for the entities in the collection
        /// excluding the fields in excludedIncludedFields.
        /// </param>
        /// <exception cref="T:SD.LLBLGen.Pro.ORMSupportClasses.ORMGeneralOperationException">
        /// The entity factory of the passed in entities collection is null.
        /// </exception>
        /// <remarks>
        /// The field data is set like a normal field value set, so authorization is applied
        /// to it. This routine batches fetches to have at most 5*ParameterizedThreshold
        /// of parameters per fetch. Keep in mind that most databases have a limit on the
        /// # of parameters per query.
        /// </remarks>
        Task FetchExcludedFieldsAsync(IEntityCollection2 entities, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the passed in entity factory.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="entityFactoryToUse">
        /// The factory which will be used to create a new entity object which will be fetched
        /// </param>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <returns>
        /// The new entity fetched.
        /// </returns>
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath);

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the passed in entity factory.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="entityFactoryToUse">
        /// The factory which will be used to create a new entity object which will be fetched
        /// </param>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <returns>
        /// The new entity fetched.
        /// </returns>
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the passed in entity factory.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="entityFactoryToUse">
        /// The factory which will be used to create a new entity object which will be fetched
        /// </param>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful.
        /// </param>
        /// <returns>
        /// The new entity fetched, or a previous entity fetched if that entity was in the
        /// context specified
        /// </returns>
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, Context contextToUse);

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the specified generic type.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity to fetch
        /// </typeparam>
        /// <returns>
        /// The new entity fetched, or a previous entity fetched if that entity was in the
        /// context specified
        /// </returns>
        /// <remarks>
        /// TEntity can't be a type which is an abstract entity. If you want to fetch an
        /// instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        /// which accepts an entity factory instead
        /// </remarks>
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields) where TEntity : EntityBase2, IEntity2, new();

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the passed in entity factory.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="entityFactoryToUse">
        /// The factory which will be used to create a new entity object which will be fetched
        /// </param>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <returns>
        /// The new entity fetched, or a previous entity fetched if that entity was in the
        /// context specified
        /// </returns>
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse);

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the specified generic type.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity to fetch
        /// </typeparam>
        /// <returns>
        /// The new entity fetched, or a previous entity fetched if that entity was in the
        /// context specified
        /// </returns>
        /// <remarks>
        /// TEntity can't be a type which is an abstract entity. If you want to fetch an
        /// instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        /// which accepts an entity factory instead
        /// </remarks>
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse) where TEntity : EntityBase2, IEntity2, new();

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the specified generic type.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity to fetch
        /// </typeparam>
        /// <returns>
        /// The new entity fetched.
        /// </returns>
        /// <remarks>
        /// TEntity can't be a type which is an abstract entity. If you want to fetch an
        /// instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        /// which accepts an entity factory instead
        /// </remarks>
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath) where TEntity : EntityBase2, IEntity2, new();

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the specified generic type.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity to fetch
        /// </typeparam>
        /// <returns>
        /// The new entity fetched, or a previous entity fetched if that entity was in the
        /// context specified
        /// </returns>
        /// <remarks>
        /// TEntity can't be a type which is an abstract entity. If you want to fetch an
        /// instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        /// which accepts an entity factory instead
        /// </remarks>
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, Context contextToUse) where TEntity : EntityBase2, IEntity2, new();

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the specified generic type.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of entity to fetch
        /// </typeparam>
        /// <returns>
        /// The new entity fetched.
        /// </returns>
        /// <remarks>
        /// TEntity can't be a type which is an abstract entity. If you want to fetch an
        /// instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        /// which accepts an entity factory instead
        /// </remarks>
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket) where TEntity : EntityBase2, IEntity2, new();

        /// <summary>
        /// Fetches a new entity using the filter/relation combination filter passed in via
        /// filterBucket and the new entity is created using the passed in entity factory.
        /// Use this method when fetching a related entity using a current entity (for example,
        /// fetch the related Customer entity of an existing Order entity)
        /// </summary>
        /// <param name="entityFactoryToUse">
        /// The factory which will be used to create a new entity object which will be fetched
        /// </param>
        /// <param name="filterBucket">
        /// the completely filled in IRelationPredicateBucket object which will be used as
        /// a filter for the fetch. The fetch will only load the first entity loaded, even
        /// if the filter results into more entities being fetched
        /// </param>
        /// <param name="prefetchPath">
        /// The prefetch path to use for this fetch, which will fetch all related entities
        /// defined by the path as well.
        /// </param>
        /// <param name="contextToUse">
        /// The context to add the entity to if the fetch was successful, and before the prefetch
        /// path is fetched. This ensures that the prefetch path is fetched using the context
        /// specified and will re-use already loaded entity objects.
        /// </param>
        /// <param name="excludedIncludedFields">
        /// The list of IEntityField2 objects which have to be excluded or included for the
        /// fetch. If null or empty, all fields are fetched (default). If an instance of
        /// ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        /// is set to false, the fields contained in excludedIncludedFields are kept in the
        /// query, the rest of the fields in the query are excluded.
        /// </param>
        /// <returns>
        /// The new entity fetched, or a previous entity fetched if that entity was in the
        /// context specified
        /// </returns>
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields);

        /// <summary>
        /// Executes the passed in retrieval query and projects the resultset using the value
        /// projectors and the projector specified. IF a transaction is in progress, the
        /// command is wired to the transaction and executed inside the transaction. The
        /// projection results will be stored in the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IRetrievalQuery queryToExecute);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in, executes that retrieval query
        /// and projects the resultset of that query using the value projectors and the projector
        /// specified. If a transaction is in progress, the command is wired to the transaction
        /// and executed inside the transaction. The projection results will be stored in
        /// the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, QueryParameters parameters);

        /// <summary>
        /// Projects the current resultset of the passed in datareader using the value projectors
        /// and the projector specified. The reader will be left open
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="reader">
        /// The reader which points to the first row of a resultset
        /// </param>
        /// <param name="executedQuery">
        /// the query object executed which produced the reader. Pass the executed query
        /// object to make sure resultset caching is possible.
        /// </param>
        /// <remarks>
        /// Use this overload together with FetchDataReader if your datareader contains multiple
        /// resultsets, so you have fine-grained control over how you want to project which
        /// resultset in the datareader. Resultset caching will occur if the passed in executedQuery
        /// is setup to cache its resultset.
        /// </remarks>
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IDataReader reader, IRetrievalQuery executedQuery);

        /// <summary>
        /// Projects the current resultset of the passed in datareader using the value projectors
        /// and the projector specified. The reader will be left open
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="reader">
        /// The reader which points to the first row of a resultset
        /// </param>
        /// <remarks>
        /// Use this overload together with FetchDataReader if your datareader contains multiple
        /// resultsets, so you have fine-grained control over how you want to project which
        /// resultset in the datareader. The resultset won't be cached in the resultset cache.
        /// To cache the resultset, use the overload which accepts the IRetrievalQuery executed
        /// </remarks>
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IDataReader reader);

        /// <summary>
        /// Projects the current resultset of the passed in datareader using the value projectors
        /// and the projector specified. The reader will be left open
        /// </summary>
        /// <param name="reader">
        /// The open reader to project the active resultset of
        /// </param>
        /// <param name="queryExecuted">
        /// the query object executed which produced the reader. Pass the executed query
        /// object to make sure resultset caching is possible.
        /// </param>
        /// <typeparam name="T">
        /// Type of the return elements, one for each row
        /// </typeparam>
        /// <returns>
        /// List of instances of T, one for each row in the resultset of reader
        /// </returns>
        /// <remarks>
        /// Use this overload together with FetchDataReader if your datareader contains multiple
        /// resultsets, so you have fine-grained control over how you want to project which
        /// resultset in the datareader. Resultset caching will occur if the passed in executedQuery
        /// is setup to cache its resultset.
        /// </remarks>
        List<T> FetchProjection<T>(IDataReader reader, IRetrievalQuery queryExecuted);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in, executes that retrieval query
        /// and projects the resultset of that query using the value projectors and the projector
        /// specified. If a transaction is in progress, the command is wired to the transaction
        /// and executed inside the transaction. The projection results will be stored in
        /// the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="sortClauses">
        /// The sort clauses.
        /// </param>
        /// <param name="groupByClause">
        /// The group by clause.
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IGroupByCollection groupByClause, bool allowDuplicates, int pageNumber, int pageSize);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in, executes that retrieval query
        /// and projects the resultset of that query using the value projectors and the projector
        /// specified. If a transaction is in progress, the command is wired to the transaction
        /// and executed inside the transaction. The projection results will be stored in
        /// the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="sortClauses">
        /// The sort clauses.
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);

        /// <summary>
        /// Executes the passed in retrieval query and projects the resultset onto instances
        /// of T (each row is materialized into an instance of T). If a transaction is in
        /// progress, the command is wired to the transaction and executed inside the transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        /// <typeparam name="T">
        /// Type of the return elements, one for each row
        /// </typeparam>
        /// <returns>
        /// List of instances of T, one for each row in the resultset of queryToExecute
        /// </returns>
        List<T> FetchProjection<T>(IRetrievalQuery queryToExecute);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in, executes that retrieval query
        /// and projects the resultset of that query using the value projectors and the projector
        /// specified. If a transaction is in progress, the command is wired to the transaction
        /// and executed inside the transaction. The projection results will be stored in
        /// the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, bool allowDuplicates);

        /// <summary>
        /// Creates a new Retrieval query from the elements passed in, executes that retrieval query
        /// and projects the resultset of that query using the value projectors and the projector
        /// specified. If a transaction is in progress, the command is wired to the transaction
        /// and executed inside the transaction. The projection results will be stored in
        /// the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="fields">
        /// The fields to use to build the query.
        /// </param>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The max number of items to return. Specify 0 to return all elements
        /// </param>
        /// <param name="sortClauses">
        /// The sort clauses.
        /// </param>
        /// <param name="allowDuplicates">
        /// If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        /// into the query (if possible).
        /// </param>
        /// <param name="pageNumber">
        /// The page number.
        /// </param>
        /// <param name="pageSize">
        /// Size of the page.
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, int pageNumber, int pageSize);

        /// <summary>
        /// Projects the current resultset of the passed in datareader using the value projectors
        /// and the projector specified. The reader will be left open
        /// </summary>
        /// <param name="reader">
        /// The open reader to project the active resultset of
        /// </param>
        /// <typeparam name="T">
        /// Type of the return elements, one for each row
        /// </typeparam>
        /// <returns>
        /// List of instances of T, one for each row in the resultset of reader
        /// </returns>
        /// <remarks>
        /// Use this overload together with FetchDataReader if your datareader contains multiple
        /// resultsets, so you have fine-grained control over how you want to project which
        /// resultset in the datareader. Resultset caching will not occur. To use resultset
        /// caching, use the overload which accepts an IRetrievalQuery
        /// </remarks>
        List<T> FetchProjection<T>(IDataReader reader);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection(System.Collections.Generic.List{SD.LLBLGen.Pro.ORMSupportClasses.IDataValueProjector},SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector,SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        /// Executes the passed in retrieval query and projects the resultset using the value
        /// projectors and the projector specified. IF a transaction is in progress, the
        /// command is wired to the transaction and executed inside the transaction. The
        /// projection results will be stored in the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        Task FetchProjectionAsync(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IRetrievalQuery queryToExecute, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection(System.Collections.Generic.List{SD.LLBLGen.Pro.ORMSupportClasses.IDataValueProjector},SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector,SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        /// Creates a new Retrieval query from the elements passed in, executes that retrieval query
        /// and projects the resultset of that query using the value projectors and the projector
        /// specified. If a transaction is in progress, the command is wired to the transaction
        /// and executed inside the transaction. The projection results will be stored in
        /// the projector.
        /// </summary>
        /// <param name="valueProjectors">
        /// The value projectors.
        /// </param>
        /// <param name="projector">
        /// The projector to use for projecting a plain row onto a new object provided by
        /// the projector.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// parameters
        /// </exception>
        Task FetchProjectionAsync(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, QueryParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection``1(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        /// Executes the passed in retrieval query and projects the resultset onto instances
        /// of T (each row is materialized into an instance of T). If a transaction is in
        /// progress, the command is wired to the transaction and executed inside the transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// List of instances of T, one for each row in the resultset of queryToExecute
        /// </returns>
        Task<List<T>> FetchProjectionAsync<T>(IRetrievalQuery queryToExecute, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection``1(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        /// Executes the passed in retrieval query and projects the resultset onto instances
        /// of T (each row is materialized into an instance of T). If a transaction is in
        /// progress, the command is wired to the transaction and executed inside the transaction.
        /// </summary>
        /// <param name="queryToExecute">
        /// The query to execute.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// List of instances of T, one for each row in the resultset of queryToExecute
        /// </returns>
        Task<List<T>> FetchProjectionAsync<T>(IRetrievalQuery queryToExecute);

        /// <summary>
        /// Executes the specified plain SQL query using this adapter and projects each row
        /// in the resultset to an instance of T. Every parameter value is converted into
        /// one or more parameters which have to be pre-defined in the sqlQuery. Uses default
        /// fetch aspects.
        /// </summary>
        /// <param name="sqlQuery">
        /// The SQL query to execute, which returns a resultset. Should contain parameter
        /// names for the parameter values, or placeholders for parameter sets. See documentation
        /// for details regarding format specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <typeparam name="T">
        /// The type of the element to project each row to.
        /// </typeparam>
        /// <returns>
        /// A list with 0 or more instances of T, one for each row in the resultset obtained
        /// from executing the query constructed from sqlQuery and the specified parameters
        /// </returns>
        List<T> FetchQuery<T>(string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Executes the specified plain SQL query using this adapter and projects each row
        /// in the resultset to an instance of T. Every parameter value is converted into
        /// one or more parameters which have to be pre-defined in the sqlQuery.
        /// </summary>
        /// <param name="fetchAspects">
        /// The fetch aspects for this query. Can be null, in which case the defaults are
        /// used.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query to execute, which returns a resultset. Should contain parameter
        /// names for the parameter values, or placeholders for parameter sets. See documentation
        /// for details regarding format specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <typeparam name="T">
        /// The type of the element to project each row to.
        /// </typeparam>
        /// <returns>
        /// A list with 0 or more instances of T, one for each row in the resultset obtained
        /// from executing the query constructed from sqlQuery and the specified parameters
        /// </returns>
        List<T> FetchQuery<T>(PlainSQLFetchAspects fetchAspects, string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.PlainSQLFetchAspects,System.String,System.Object)
        /// Executes the specified plain SQL query using this adapter and projects each row
        /// in the resultset to an instance of T. Every parameter value is converted into
        /// one or more parameters which have to be pre-defined in the sqlQuery.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="fetchAspects">
        /// The fetch aspects for this query. Can be null, in which case the defaults are
        /// used.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query to execute, which returns a resultset. Should contain parameter
        /// names for the parameter values, or placeholders for parameter sets. See documentation
        /// for details regarding format specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <typeparam name="T">
        /// The type of the element to project each row to.
        /// </typeparam>
        /// <returns>
        /// A list with 0 or more instances of T, one for each row in the resultset obtained
        /// from executing the query constructed from sqlQuery and the specified parameters
        /// </returns>
        Task<List<T>> FetchQueryAsync<T>(CancellationToken cancellationToken, PlainSQLFetchAspects fetchAspects, string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.PlainSQLFetchAspects,System.String,System.Object)
        /// Executes the specified plain SQL query using this adapter and projects each row
        /// in the resultset to an instance of T. Every parameter value is converted into
        /// one or more parameters which have to be pre-defined in the sqlQuery.
        /// </summary>
        /// <param name="fetchAspects">
        /// The fetch aspects for this query. Can be null, in which case the defaults are
        /// used.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query to execute, which returns a resultset. Should contain parameter
        /// names for the parameter values, or placeholders for parameter sets. See documentation
        /// for details regarding format specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <typeparam name="T">
        /// The type of the element to project each row to.
        /// </typeparam>
        /// <returns>
        /// A list with 0 or more instances of T, one for each row in the resultset obtained
        /// from executing the query constructed from sqlQuery and the specified parameters
        /// </returns>
        Task<List<T>> FetchQueryAsync<T>(PlainSQLFetchAspects fetchAspects, string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(System.String,System.Object)
        /// Executes the specified plain SQL query using this adapter and projects each row
        /// in the resultset to an instance of T. Every parameter value is converted into
        /// one or more parameters which have to be pre-defined in the sqlQuery. Uses default
        /// fetch aspects.
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <param name="sqlQuery">
        /// The SQL query to execute, which returns a resultset. Should contain parameter
        /// names for the parameter values, or placeholders for parameter sets. See documentation
        /// for details regarding format specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <typeparam name="T">
        /// The type of the element to project each row to.
        /// </typeparam>
        /// <returns>
        /// A list with 0 or more instances of T, one for each row in the resultset obtained
        /// from executing the query constructed from sqlQuery and the specified parameters
        /// </returns>
        Task<List<T>> FetchQueryAsync<T>(CancellationToken cancellationToken, string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(System.String,System.Object)
        /// Executes the specified plain SQL query using this adapter and projects each row
        /// in the resultset to an instance of T. Every parameter value is converted into
        /// one or more parameters which have to be pre-defined in the sqlQuery. Uses default
        /// fetch aspects.
        /// </summary>
        /// <param name="sqlQuery">
        /// The SQL query to execute, which returns a resultset. Should contain parameter
        /// names for the parameter values, or placeholders for parameter sets. See documentation
        /// for details regarding format specifics.
        /// </param>
        /// <param name="parameterValues">
        /// The object containing the parameter values to use in the query. If it's an object
        /// array, parameters using ordering are assumed, otherwise for each public, non-static
        /// property, a parameter is created.
        /// </param>
        /// <typeparam name="T">
        /// The type of the element to project each row to.
        /// </typeparam>
        /// <returns>
        /// A list with 0 or more instances of T, one for each row in the resultset obtained
        /// from executing the query constructed from sqlQuery and the specified parameters
        /// </returns>
        Task<List<T>> FetchQueryAsync<T>(string sqlQuery, object parameterValues = null);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. For TypedView
        /// filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void FetchTypedList(DataTable dataTableToFill, QueryParameters parameters);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in the passed in typed list.
        /// For TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <typeparam name="TypedListToFill">
        /// Typed list to fill.
        /// </typeparam>
        /// <remarks>
        /// Grabs the fields list and relations set from the typed list passed in.
        /// </remarks>
        void FetchTypedList(ITypedListLgp2 typedListToFill);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in the passed in typed list.
        /// For TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <typeparam name="TypedListToFill">
        /// Typed list to fill.
        /// </typeparam>
        /// <param name="additionalFilter">
        /// An additional filter to use to filter the fetch of the typed list. If you need
        /// a more sophisticated filter approach, please use the overload which accepts an
        /// IRelationalPredicateBucket and add your filter to the bucket you receive when
        /// calling typedListToFill.GetRelationInfo().
        /// </param>
        /// <remarks>
        /// Grabs the fields list and relations set from the typed list passed in.
        /// </remarks>
        void FetchTypedList(ITypedListLgp2 typedListToFill, IPredicateExpression additionalFilter);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in the passed in typed list.
        /// For TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <typeparam name="TypedListToFill">
        /// Typed list to fill.
        /// </typeparam>
        /// <param name="additionalFilter">
        /// An additional filter to use to filter the fetch of the typed list. If you need
        /// a more sophisticated filter approach, please use the overload which accepts an
        /// IRelationalPredicateBucket and add your filter to the bucket you receive when
        /// calling typedListToFill.GetRelationInfo().
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <remarks>
        /// Grabs the fields list and relations set from the typed list passed in.
        /// </remarks>
        void FetchTypedList(ITypedListLgp2 typedListToFill, IPredicateExpression additionalFilter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. For TypedView
        /// filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields to fetch into the datatable.
        /// The fields inside this object are used to construct the selection resultset.
        /// Use the typed list's method GetFieldsInfo() to retrieve this IEntityField2 information
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <param name="groupByClause">
        /// GroupByCollection with fields to group by on
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. For TypedView
        /// filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields to fetch into the datatable.
        /// The fields inside this object are used to construct the selection resultset.
        /// Use the typed list's method GetFieldsInfo() to retrieve this IEntityField2 information
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. Doesn't apply
        /// any sorting. For TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields to fetch into the datatable.
        /// The fields inside this object are used to construct the selection resultset.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, bool allowDuplicates);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. Doesn't apply
        /// any sorting, doesn't limit the resultset on the amount of rows to return. For
        /// TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields to fetch into the datatable.
        /// The fields inside this object are used to construct the selection resultset.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, bool allowDuplicates);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. Doesn't apply
        /// any sorting, doesn't limit the resultset on the amount of rows to return, does
        /// allow duplicates. For TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields to fetch into the datatable.
        /// The fields inside this object are used to construct the selection resultset.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. For TypedView
        /// filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields to fetch into the datatable.
        /// The fields inside this object are used to construct the selection resultset.
        /// Use the typed list's method GetFieldsInfo() to retrieve this IEntityField2 information
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <param name="groupByClause">
        /// GroupByCollection with fields to group by on
        /// </param>
        /// <param name="pageNumber">
        /// the page number to retrieve. First page is 1. When set to 0, no paging logic
        /// is applied
        /// </param>
        /// <param name="pageSize">
        /// the size of the page. When set to 0, no paging logic is applied
        /// </param>
        [NDependIgnoreTooManyParams]
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause, int pageNumber, int pageSize);

        /// <summary>
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in the passed in typed list.
        /// For TypedView filling, use the method FetchTypedView()
        /// </summary>
        /// <typeparam name="TypedListToFill">
        /// Typed list to fill.
        /// </typeparam>
        /// <param name="additionalFilter">
        /// An additional filter to use to filter the fetch of the typed list. If you need
        /// a more sophisticated filter approach, please use the overload which accepts an
        /// IRelationalPredicateBucket and add your filter to the bucket you receive when
        /// calling typedListToFill.GetRelationInfo().
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <param name="pageNumber">
        /// the page number to retrieve. First page is 1. When set to 0, no paging logic
        /// is applied
        /// </param>
        /// <param name="pageSize">
        /// the size of the page. When set to 0, no paging logic is applied
        /// </param>
        /// <remarks>
        /// Grabs the fields list and relations set from the typed list passed in.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        void FetchTypedList(ITypedListLgp2 typedListToFill, IPredicateExpression additionalFilter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, int pageNumber, int pageSize);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchTypedList(System.Data.DataTable,SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        /// Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        /// using the relations and filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a typed list object. For TypedView
        /// filling, use the method FetchTypedView()
        /// </summary>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// parameters
        /// </exception>
        Task FetchTypedListAsync(DataTable dataTableToFill, QueryParameters parameters, CancellationToken cancellationToken);

        /// <summary>
        /// Fetches the Typed View passed in from the persistent storage Doesn't apply any
        /// sorting. Use this routine to fill a TypedView object.
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// Typed view to fill.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage into the DataTable object passed in. Doesn't apply any sorting, doesn't
        /// limit the amount of rows returned by the query, doesn't apply any filtering,
        /// allows duplicate rows. Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill);

        /// <summary>
        /// Fetches the Typed View passed in from the persistent storage Doesn't apply any
        /// sorting, doesn't limit the amount of rows returned by the query, doesn't apply
        /// any filtering. Use this routine to fill a TypedView object.
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// Typed view to fill.
        /// </typeparam>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedView(ITypedView2 typedViewToFill, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage using the filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable. Use the Typed View's method GetFieldsInfo() to get this IEntityField2
        /// field collection
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <remarks>
        /// To fill a DataTable with entity rows, use this method as well, by passing the
        /// Fields collection of an entity of the same type as the one you want to fetch
        /// as fieldCollectionToFetch.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View passed in from the persistent storage
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// Typed view to fill.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <param name="groupByClause">
        /// GroupByCollection with fields to group by on
        /// </param>
        /// <remarks>
        /// To fill a DataTable with entity rows, use this method as well, by passing the
        /// Fields collection of an entity of the same type as the one you want to fetch
        /// as fieldCollectionToFetch.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause);

        /// <summary>
        /// Fetches the Typed View passed in from the persistent storage
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// Typed view to fill.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <remarks>
        /// To fill a DataTable with entity rows, use this method as well, by passing the
        /// Fields collection of an entity of the same type as the one you want to fetch
        /// as fieldCollectionToFetch.
        /// </remarks>
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage using the filter information stored in filterBucket into the DataTable
        /// object passed in. Doesn't apply any sorting, doesn't limit the amount of rows
        /// returned by the query. Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View passed in from the persistent storage Doesn't apply any
        /// sorting, doesn't limit the amount of rows returned by the query, doesn't apply
        /// any filtering, allows duplicate rows. Use this routine to fill a TypedView object.
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// Typed view to fill.
        /// </typeparam>
        void FetchTypedView(ITypedView2 typedViewToFill);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage using the query information stored in parameters into the DataTable object
        /// passed in. Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="dataTableToFill">
        /// The data table to fill.
        /// </param>
        /// <param name="parameters">
        /// The parameters.
        /// </param>
        void FetchTypedView(DataTable dataTableToFill, QueryParameters parameters);

        /// <summary>
        /// Fetches the typed view, using the query specified.
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// The typed view to fill.
        /// </typeparam>
        /// <param name="queryToUse">
        /// The query to use.
        /// </param>
        /// <remarks>
        /// Used with stored procedure calling IRetrievalQuery instances to fill a typed
        /// view mapped onto a resultset. Be sure to call Dispose() on the passed in query,
        /// as it's not disposed in this method.
        /// </remarks>
        void FetchTypedView(ITypedView2 typedViewToFill, IRetrievalQuery queryToUse);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage into the DataTable object passed in. Doesn't apply any sorting, doesn't
        /// limit the amount of rows returned by the query, doesn't apply any filtering.
        /// Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View passed in from the persistent storage Doesn't apply any
        /// sorting, doesn't limit the amount of rows returned by the query. Use this routine
        /// to fill a TypedView object.
        /// </summary>
        /// <typeparam name="TypedViewToFill">
        /// Typed view to fill.
        /// </typeparam>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, bool allowDuplicates);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage using the filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable. Use the Typed View's method GetFieldsInfo() to get this IEntityField2
        /// field collection
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <param name="groupByClause">
        /// GroupByCollection with fields to group by on
        /// </param>
        /// <remarks>
        /// To fill a DataTable with entity rows, use this method as well, by passing the
        /// Fields collection of an entity of the same type as the one you want to fetch
        /// as fieldCollectionToFetch.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage using the filter information stored in filterBucket into the DataTable
        /// object passed in. Use this routine to fill a TypedView object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable. Use the Typed View's method GetFieldsInfo() to get this IEntityField2
        /// field collection
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="sortClauses">
        /// SortClause expression which is applied to the query executed, sorting the fetch
        /// result.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <param name="groupByClause">
        /// GroupByCollection with fields to group by on
        /// </param>
        /// <param name="pageNumber">
        /// the page number to retrieve. First page is 1. When set to 0, no paging logic
        /// is applied
        /// </param>
        /// <param name="pageSize">
        /// the size of the page. When set to 0, no paging logic is applied
        /// </param>
        /// <remarks>
        /// To fill a DataTable with entity rows, use this method as well, by passing the
        /// Fields collection of an entity of the same type as the one you want to fetch
        /// as fieldCollectionToFetch.
        /// </remarks>
        [NDependIgnoreTooManyParams]
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause, int pageNumber, int pageSize);

        /// <summary>
        /// Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        /// storage using the filter information stored in filterBucket into the DataTable
        /// object passed in. Doesn't apply any sorting Use this routine to fill a TypedView
        /// object.
        /// </summary>
        /// <param name="fieldCollectionToFetch">
        /// IEntityField2 collection which contains the fields of the typed view to fetch
        /// into the datatable.
        /// </param>
        /// <param name="dataTableToFill">
        /// The datatable object to fill with the data for the fields in fieldCollectionToFetch
        /// </param>
        /// <param name="filterBucket">
        /// filter information (relations and predicate expressions) for retrieving the data.
        /// Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        /// </param>
        /// <param name="maxNumberOfItemsToReturn">
        /// The maximum amount of rows to return. If 0, all rows matching the filter are
        /// returned
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, bool allowDuplicates);

        /// <summary>
        /// Gets the estimated number of objects returned by a query for objects to store
        /// in the entity collection passed in, using the filter and groupby clause specified.
        /// The number is estimated as duplicate objects can be present in the plain query
        /// results, but will be filtered out when the query result is transformed into objects.
        /// </summary>
        /// <param name="collection">
        /// EntityCollection instance which will be fetched by the query to get the row count
        /// for
        /// </param>
        /// <param name="filter">
        /// filter to use by the query to get the row count for
        /// </param>
        /// <param name="groupByClause">
        /// The list of fields to group by on. When not specified or an empty collection
        /// is specified, no group by clause is added to the query. A check is performed
        /// for each field in the selectList. If a field in the selectList is not present
        /// in the groupByClause collection, an exception is thrown.
        /// </param>
        /// <returns>
        /// the number of rows the query for the fields specified, using the filter, relations
        /// and groupbyClause specified.
        /// </returns>
        /// <remarks>
        /// This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        /// as a scalar query. This construct is not supported on Firebird. You can try to
        /// achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        /// those results can differ from the result returned by GetDbCount if you use a
        /// group by clause.
        /// </remarks>
        int GetDbCount(IEntityCollection2 collection, IRelationPredicateBucket filter, IGroupByCollection groupByClause);

        /// <summary>
        /// Gets the estimated number of objects returned by a query for objects to store
        /// in the entity collection passed in, using the filter and groupby clause specified.
        /// The number is estimated as duplicate objects can be present in the plain query
        /// results, but will be filtered out when the query result is transformed into objects.
        /// </summary>
        /// <param name="collection">
        /// EntityCollection instance which will be fetched by the query to get the row count
        /// for
        /// </param>
        /// <param name="filter">
        /// filter to use by the query to get the row count for
        /// </param>
        /// <returns>
        /// the number of rows the query for the fields specified, using the filter, relations
        /// and groupbyClause specified.
        /// </returns>
        /// <remarks>
        /// This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        /// as a scalar query. This construct is not supported on Firebird. You can try to
        /// achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        /// those results can differ from the result returned by GetDbCount if you use a
        /// group by clause.
        /// </remarks>
        int GetDbCount(IEntityCollection2 collection, IRelationPredicateBucket filter);

        /// <summary>
        /// Gets the number of rows returned by a query for the fields specified, using the
        /// filter and groupby clause specified.
        /// </summary>
        /// <param name="fields">
        /// IEntityFields2 instance with the fields returned by the query to get the row count
        /// for
        /// </param>
        /// <param name="filter">
        /// filter to use by the query to get the row count for
        /// </param>
        /// <param name="groupByClause">
        /// The list of fields to group by on. When not specified or an empty collection
        /// is specified, no group by clause is added to the query. A check is performed
        /// for each field in the selectList. If a field in the selectList is not present
        /// in the groupByClause collection, an exception is thrown.
        /// </param>
        /// <param name="allowDuplicates">
        /// When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        /// duplicate rows.
        /// </param>
        /// <returns>
        /// the number of rows the query for the fields specified, using the filter, relations
        /// and groupbyClause specified.
        /// </returns>
        /// <remarks>
        /// This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        /// as a scalar query. This construct is not supported on Firebird. You can try to
        /// achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        /// those results can differ from the result returned by GetDbCount if you use a
        /// group by clause.
        /// </remarks>
        int GetDbCount(IEntityFields2 fields, IRelationPredicateBucket filter, IGroupByCollection groupByClause, bool allowDuplicates);

        /// <summary>
        /// Gets the number of rows returned by a query for the fields specified, using the
        /// filter and groupby clause specified.
        /// </summary>
        /// <param name="fields">
        /// IEntityFields2 instance with the fields returned by the query to get the row count
        /// for
        /// </param>
        /// <param name="filter">
        /// filter to use by the query to get the row count for
        /// </param>
        /// <param name="groupByClause">
        /// The list of fields to group by on. When not specified or an empty collection
        /// is specified, no group by clause is added to the query. A check is performed
        /// for each field in the selectList. If a field in the selectList is not present
        /// in the groupByClause collection, an exception is thrown.
        /// </param>
        /// <returns>
        /// the number of rows the query for the fields specified, using the filter, relations
        /// and groupbyClause specified.
        /// </returns>
        /// <remarks>
        /// This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        /// as a scalar query. This construct is not supported on Firebird. You can try to
        /// achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        /// those results can differ from the result returned by GetDbCount if you use a
        /// group by clause.
        /// </remarks>
        int GetDbCount(IEntityFields2 fields, IRelationPredicateBucket filter, IGroupByCollection groupByClause);

        /// <summary>
        /// Gets the number of rows returned by a query for the fields specified, using the
        /// filter and groupby clause specified.
        /// </summary>
        /// <param name="fields">
        /// IEntityFields2 instance with the fields returned by the query to get the row count
        /// for
        /// </param>
        /// <param name="filter">
        /// filter to use by the query to get the row count for
        /// </param>
        /// <returns>
        /// the number of rows the query for the fields specified, using the filter, relations
        /// and groupbyClause specified.
        /// </returns>
        /// <remarks>
        /// This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        /// as a scalar query. This construct is not supported on Firebird. You can try to
        /// achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        /// those results can differ from the result returned by GetDbCount if you use a
        /// group by clause.
        /// </remarks>
        int GetDbCount(IEntityFields2 fields, IRelationPredicateBucket filter);

        /// <summary>
        /// Executes the expression defined with the field in the fields collection specified,
        /// using the various elements defined. The expression is executed as a scalar query
        /// and a single value is returned.
        /// </summary>
        /// <param name="fields">
        /// IEntityFields2 instance with a single field with an expression defined and eventually
        /// aggregates
        /// </param>
        /// <param name="filter">
        /// filter to use
        /// </param>
        /// <param name="groupByClause">
        /// The list of fields to group by on. When not specified or an empty collection
        /// is specified, no group by clause is added to the query. A check is performed
        /// for each field in the selectList. If a field in the selectList is not present
        /// in the groupByClause collection, an exception is thrown.
        /// </param>
        /// <param name="relations">
        /// The relations part of the filter.
        /// </param>
        /// <returns>
        /// the value which is the result of the expression defined on the specified field
        /// </returns>
        object GetScalar(IEntityFields2 fields, IPredicate filter, IGroupByCollection groupByClause, IRelationCollection relations);

        /// <summary>
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        /// <param name="field">
        /// Field to which to apply the aggregate function and expression
        /// </param>
        /// <param name="aggregateToApply">
        /// Aggregate function to apply.
        /// </param>
        /// <returns>
        /// the scalar value requested
        /// </returns>
        object GetScalar(IEntityField2 field, AggregateFunction aggregateToApply);

        /// <summary>
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        /// <param name="field">
        /// Field to which to apply the aggregate function and expression
        /// </param>
        /// <param name="expressionToExecute">
        /// The expression to execute. Can be null
        /// </param>
        /// <param name="aggregateToApply">
        /// Aggregate function to apply.
        /// </param>
        /// <param name="filter">
        /// The filter to apply to retrieve the scalar
        /// </param>
        /// <param name="groupByClause">
        /// The groupby clause to apply to retrieve the scalar
        /// </param>
        /// <param name="relations">
        /// The relations part of the filter.
        /// </param>
        /// <returns>
        /// the scalar value requested
        /// </returns>
        [NDependIgnoreTooManyParams]
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter, IGroupByCollection groupByClause, IRelationCollection relations);

        /// <summary>
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        /// <param name="field">
        /// Field to which to apply the aggregate function and expression
        /// </param>
        /// <param name="expressionToExecute">
        /// The expression to execute. Can be null
        /// </param>
        /// <param name="aggregateToApply">
        /// Aggregate function to apply.
        /// </param>
        /// <param name="filter">
        /// The filter to apply to retrieve the scalar
        /// </param>
        /// <returns>
        /// the scalar value requested
        /// </returns>
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter);

        /// <summary>
        /// Executes the expression defined with the field in the fields collection specified,
        /// using the various elements defined. The expression is executed as a scalar query
        /// and a single value is returned.
        /// </summary>
        /// <param name="fields">
        /// IEntityFields2 instance with a single field with an expression defined and eventually
        /// aggregates
        /// </param>
        /// <param name="filter">
        /// filter to use
        /// </param>
        /// <param name="groupByClause">
        /// The list of fields to group by on. When not specified or an empty collection
        /// is specified, no group by clause is added to the query. A check is performed
        /// for each field in the selectList. If a field in the selectList is not present
        /// in the groupByClause collection, an exception is thrown.
        /// </param>
        /// <returns>
        /// the value which is the result of the expression defined on the specified field
        /// </returns>
        object GetScalar(IEntityFields2 fields, IPredicate filter, IGroupByCollection groupByClause);

        /// <summary>
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        /// <param name="field">
        /// Field to which to apply the aggregate function and expression
        /// </param>
        /// <param name="expressionToExecute">
        /// The expression to execute. Can be null
        /// </param>
        /// <param name="aggregateToApply">
        /// Aggregate function to apply.
        /// </param>
        /// <returns>
        /// the scalar value requested
        /// </returns>
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply);

        /// <summary>
        /// Gets a scalar value, calculated with the aggregate and expression specified.
        /// the field specified is the field the expression and aggregate are applied on.
        /// </summary>
        /// <param name="field">
        /// Field to which to apply the aggregate function and expression
        /// </param>
        /// <param name="expressionToExecute">
        /// The expression to execute. Can be null
        /// </param>
        /// <param name="aggregateToApply">
        /// Aggregate function to apply.
        /// </param>
        /// <param name="filter">
        /// The filter to apply to retrieve the scalar
        /// </param>
        /// <param name="groupByClause">
        /// The groupby clause to apply to retrieve the scalar
        /// </param>
        /// <returns>
        /// the scalar value requested
        /// </returns>
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter, IGroupByCollection groupByClause);

        /// <summary>
        /// Opens the active connection object. If the connection is already open, nothing
        /// is done. If no connection object is present, a new one is created
        /// </summary>
        void OpenConnection();

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.OpenConnection.
        /// Opens the active connection object. If the connection is already open, nothing
        /// is done. If no connection object is present, a new one is created
        /// </summary>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="T:System.ObjectDisposedException">
        /// DataAccessAdapterBase;This DataAccessAdapter instance has already been disposed,
        /// you can't use it for further persistence activity
        /// </exception>
        Task OpenConnectionAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.OpenConnection.
        /// Opens the active connection object. If the connection is already open, nothing
        /// is done. If no connection object is present, a new one is created
        /// </summary>
        /// <exception cref="T:System.ObjectDisposedException">
        /// DataAccessAdapterBase;This DataAccessAdapter instance has already been disposed,
        /// you can't use it for further persistence activity
        /// </exception>
        Task OpenConnectionAsync();

        /// <summary>
        /// Rolls back the transaction in action to the savepoint with the name savepointName.
        /// No internal objects are being reset when this method is called, so call this
        /// Rollback overload only to roll back to a savepoint. To roll back a complete transaction,
        /// call Rollback() without specifying a savepoint name. Create a savepoint by calling
        /// SaveTransaction(savePointName)
        /// </summary>
        /// <param name="savePointName">
        /// name of the savepoint to roll back to.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// If no transaction is in progress.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// if savePointName is empty or null
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// if the .NET database provider doesn't support transaction rolling back a transaction
        /// to a named point or when COM+ is used.
        /// </exception>
        void Rollback(string savePointName);

        /// <summary>
        /// Saves the passed in entity to the persistent storage. Will not refetch the entity
        /// after this save. The entity will stay out-of-sync. If the entity is new, it will
        /// be inserted, if the entity is existent, the changed entity fields will be changed
        /// in the database. Will do a recursive save.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        bool SaveEntity(IEntity2 entityToSave);

        /// <summary>
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will do a recursive save. Will pass the concurrency
        /// predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave);

        /// <summary>
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will do a recursive save. Will pass the concurrency
        /// predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="updateRestriction">
        /// Predicate expression, meant for concurrency checks in an Update query. Will be
        /// ignored when the entity is new.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction);

        /// <summary>
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="updateRestriction">
        /// Predicate expression, meant for concurrency checks in an Update query. Will be
        /// ignored when the entity is new.
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by entityToSave also.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse);

        /// <summary>
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by entityToSave also.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, bool recurse);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean).
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will do a recursive save. Will pass the concurrency
        /// predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean,System.Boolean).
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by entityToSave also.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, bool recurse, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean,SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression,System.Boolean).
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="updateRestriction">
        /// Predicate expression, meant for concurrency checks in an Update query. Will be
        /// ignored if the entity is new. This predicate is used instead of a predicate produced
        /// by a set ConcurrencyPredicateFactory.
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by entityToSave also.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        /// Saves the passed in entity to the persistent storage. Will not refetch the entity
        /// after this save. The entity will stay out-of-sync. If the entity is new, it will
        /// be inserted, if the entity is existent, the changed entity fields will be changed
        /// in the database. Will do a recursive save. Will pass the concurrency predicate
        /// returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save) as update
        /// restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean).
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will do a recursive save. Will pass the concurrency
        /// predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        /// Saves the passed in entity to the persistent storage. Will not refetch the entity
        /// after this save. The entity will stay out-of-sync. If the entity is new, it will
        /// be inserted, if the entity is existent, the changed entity fields will be changed
        /// in the database. Will do a recursive save. Will pass the concurrency predicate
        /// returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save) as update
        /// restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean,System.Boolean).
        /// Saves the passed in entity to the persistent storage. If the entity is new, it
        /// will be inserted, if the entity is existent, the changed entity fields will be
        /// changed in the database. Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        /// as update restriction.
        /// </summary>
        /// <param name="entityToSave">
        /// The entity to save
        /// </param>
        /// <param name="refetchAfterSave">
        /// When true, it will refetch the entity from the persistent storage so it will
        /// be up-to-date after the save action.
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by entityToSave also.
        /// </param>
        /// <returns>
        /// true if the save was successful, false otherwise.
        /// </returns>
        /// <remarks>
        /// Will use a current transaction if a transaction is in progress
        /// </remarks>
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, bool recurse);

        /// <summary>
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available. Will
        /// not refetch saved entities and will not recursively save the entities.
        /// </summary>
        /// <param name="collectionToSave">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <returns>
        /// the amount of persisted entities
        /// </returns>
        int SaveEntityCollection(IEntityCollection2 collectionToSave);

        /// <summary>
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available.
        /// </summary>
        /// <param name="collectionToSave">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <param name="refetchSavedEntitiesAfterSave">
        /// Refetches a saved entity from the database, so the entity will not be 'out of
        /// sync'
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by the entities inside collectionToSave also.
        /// </param>
        /// <returns>
        /// the amount of persisted entities
        /// </returns>
        int SaveEntityCollection(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,System.Boolean,System.Boolean).
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available.
        /// </summary>
        /// <param name="collectionToSave">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <param name="refetchSavedEntitiesAfterSave">
        /// Refetches a saved entity from the database, so the entity will not be 'out of
        /// sync'
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by the entities inside collectionToSave also.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the amount of persisted entities
        /// </returns>
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available. Will
        /// not refetch saved entities and will not recursively save the entities.
        /// </summary>
        /// <param name="collectionToSave">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the amount of persisted entities
        /// </returns>
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,System.Boolean,System.Boolean).
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available.
        /// </summary>
        /// <param name="collectionToSave">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <param name="refetchSavedEntitiesAfterSave">
        /// Refetches a saved entity from the database, so the entity will not be 'out of
        /// sync'
        /// </param>
        /// <param name="recurse">
        /// When true, it will save all dirty objects referenced (directly or indirectly)
        /// by the entities inside collectionToSave also.
        /// </param>
        /// <returns>
        /// the amount of persisted entities
        /// </returns>
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        /// Saves all dirty objects inside the collection passed to the persistent storage.
        /// It will do this inside a transaction if a transaction is not yet available. Will
        /// not refetch saved entities and will not recursively save the entities.
        /// </summary>
        /// <param name="collectionToSave">
        /// EntityCollection with one or more dirty entities which have to be persisted
        /// </param>
        /// <returns>
        /// the amount of persisted entities
        /// </returns>
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave);

        /// <summary>
        /// Creates a savepoint with the name savePointName in the current transaction. You
        /// can roll back to this savepoint using SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.Rollback(System.String).
        /// </summary>
        /// <param name="savePointName">
        /// name of savepoint. Must be unique in an active transaction
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// If no transaction is in progress.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        /// if savePointName is empty or null
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        /// if the .NET database provider doesn't support transaction saving or when COM+
        /// is used.
        /// </exception>
        void SaveTransaction(string savePointName);

        /// <summary>
        /// Starts a new transaction. All database activity after this call will be ran in
        /// this transaction and all objects will participate in this transaction until its
        /// committed or rolled back. If there is a transaction in progress, an exception
        /// is thrown. Will create and open a new connection if a transaction is not open
        /// and/or available.
        /// </summary>
        /// <param name="isolationLevelToUse">
        /// The isolation level to use for this transaction
        /// </param>
        /// <param name="name">
        /// The name for this transaction
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// If a transaction is already in progress.
        /// </exception>
        void StartTransaction(IsolationLevel isolationLevelToUse, string name);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.StartTransaction(System.Data.IsolationLevel,System.String).
        /// Starts a new transaction. All database activity after this call will be ran in
        /// this transaction and all objects will participate in this transaction until its
        /// committed or rolled back. If there is a transaction in progress, an exception
        /// is thrown. Will create and open a new connection if a transaction is not open
        /// and/or available.
        /// </summary>
        /// <param name="isolationLevelToUse">
        /// The isolation level to use for this transaction
        /// </param>
        /// <param name="name">
        /// The name for this transaction
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// If a transaction is already in progress.
        /// </exception>
        /// <remarks>
        /// If this DataAccessAdapter is in a System.Transactions.Transaction, no real ado.net
        /// transaction will be started, as a transaction is already in progress. In that
        /// situation, this method will just open the connection if required.
        /// </remarks>
        Task StartTransactionAsync(IsolationLevel isolationLevelToUse, string name);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.StartTransaction(System.Data.IsolationLevel,System.String).
        /// Starts a new transaction. All database activity after this call will be ran in
        /// this transaction and all objects will participate in this transaction until its
        /// committed or rolled back. If there is a transaction in progress, an exception
        /// is thrown. Will create and open a new connection if a transaction is not open
        /// and/or available.
        /// </summary>
        /// <param name="isolationLevelToUse">
        /// The isolation level to use for this transaction
        /// </param>
        /// <param name="name">
        /// The name for this transaction
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        /// If a transaction is already in progress.
        /// </exception>
        /// <remarks>
        /// If this DataAccessAdapter is in a System.Transactions.Transaction, no real ado.net
        /// transaction will be started, as a transaction is already in progress. In that
        /// situation, this method will just open the connection if required.
        /// </remarks>
        Task StartTransactionAsync(IsolationLevel isolationLevelToUse, string name, CancellationToken cancellationToken);

        /// <summary>
        /// Updates all entities of the same type of the entity entityWithNewValues directly
        /// in the persistent storage if they match the filter supplied in filterBucket.
        /// Only the fields changed in entityWithNewValues are updated for these fields.
        /// </summary>
        /// <param name="entityWithNewValues">
        /// Entity object which contains the new values for the entities of the same type
        /// and which match the filter in filterBucket. Only fields which are changed are
        /// updated.
        /// </param>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to update.
        /// </param>
        /// <returns>
        /// the amount of physically updated entities
        /// </returns>
        int UpdateEntitiesDirectly(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.UpdateEntitiesDirectly(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        /// Updates all entities of the same type or subtype of the entity entityWithNewValues
        /// directly in the persistent storage if they match the filter supplied in filterBucket.
        /// Only the fields changed in entityWithNewValues are updated for these fields.
        /// Entities of a subtype of the type of entityWithNewValues which are affected by
        /// the filterBucket's filter will thus also be updated.
        /// </summary>
        /// <param name="entityWithNewValues">
        /// Entity object which contains the new values for the entities of the same type
        /// and which match the filter in filterBucket. Only fields which are changed are
        /// updated.
        /// </param>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to update.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the number of physically updated entities. Use this number only to test if the
        /// update succeeded (so value is > 0).
        /// </returns>
        Task<int> UpdateEntitiesDirectlyAsync(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.UpdateEntitiesDirectly(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        /// Updates all entities of the same type or subtype of the entity entityWithNewValues
        /// directly in the persistent storage if they match the filter supplied in filterBucket.
        /// Only the fields changed in entityWithNewValues are updated for these fields.
        /// Entities of a subtype of the type of entityWithNewValues which are affected by
        /// the filterBucket's filter will thus also be updated.
        /// </summary>
        /// <param name="entityWithNewValues">
        /// Entity object which contains the new values for the entities of the same type
        /// and which match the filter in filterBucket. Only fields which are changed are
        /// updated.
        /// </param>
        /// <param name="filterBucket">
        /// filter information to filter out the entities to update.
        /// </param>
        /// <returns>
        /// the number of physically updated entities. Use this number only to test if the
        /// update succeeded (so value is > 0).
        /// </returns>
        Task<int> UpdateEntitiesDirectlyAsync(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket);
        #endregion

        #region "IDataAccess adapter extension methods"

        /// <summary>
        /// Fetches the query as an open data reader.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <remarks>
        /// Ignores nested queries and projection logic embedded in a lambda specification
        /// in the query. The DataReader returned will contain the resultset of the plain
        /// SQL query.
        /// </remarks>
        IDataReader FetchAsDataReader(DynamicQuery query, CommandBehavior behavior);

        /// <summary>
        /// Fetches the specified query into a new DataTable specified and returns that datatable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// a new DataTable with the data fetched.
        /// </returns>
        DataTable FetchAsDataTable(DynamicQuery query);

        /// <summary>
        /// Fetches the specified query into the DataTable specified and returns that datatable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="destination">
        /// The destination datatable to fetch the data into.
        /// </param>
        /// <returns>
        /// the destination datatable specified.
        /// </returns>
        /// <remarks>
        /// If the DataTable specified already has columns defined, they have to have compatible
        /// types and have to be in the same order as the columns in the resultset of the
        /// query. It's recommended to have columns with the same names as the resultset
        /// of the query, to be able to convert null values to DBNull.Value.
        /// </remarks>
        DataTable FetchAsDataTable(DynamicQuery query, DataTable destination);

        /// <summary>
        /// Fetches the query as a projection, using the projector specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="projector">
        /// The projector.
        /// </param>
        /// <remarks>
        /// Will ignore nested queries. Use with queries without nested / hierarchical queries.
        /// The projector has to be setup and ready to use when calling this method
        /// </remarks>
        void FetchAsProjection(DynamicQuery query, IGeneralDataProjector projector);

        /// <summary>
        /// Fetches the first entity of the set returned by the query and returns that entity,
        /// if any, otherwise null.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// the first entity in the resultset, or null if the resultset is empty.
        /// </returns>
        TEntity FetchFirst<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;

        /// <summary>
        /// Fetches the first object of the set returned by the query and returns that object,
        /// if any, otherwise null.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// the first object in the resultset, or null if the resultset is empty.
        /// </returns>
        T FetchFirst<T>(DynamicQuery<T> query);

        /// <summary>
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// EntityCollection(Of TEntity) with the results of the query fetch
        /// </returns>
        IEntityCollection2 FetchQuery<TEntity>(EntityQuery<TEntity> query) where TEntity : IEntity2;

        /// <summary>
        /// Fetches the query specified on the adapter specified into the collectionToFill
        /// specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="collectionToFill">
        /// The collection to fill.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <typeparam name="TCollection">
        /// The type of the collection.
        /// </typeparam>
        /// <returns>
        /// collectionToFill, filled with the query fetch results.
        /// </returns>
        /// <remarks>
        /// Equal to calling adapter.FetchEntityCollection(), so entities already present
        /// in collectionToFill are left as-is. If the fetch has to take into account a Context,
        /// the passed collectionToFill has to be assigned to the context before calling
        /// this method.
        /// </remarks>
        TCollection FetchQuery<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill)
            where TEntity : IEntity2
            where TCollection : IEntityCollection2;

        /// <summary>
        /// Fetches the query specified and returns the results in plain object arrays, one
        /// object array per returned row of the query specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        List<object[]> FetchQuery(DynamicQuery query);

        /// <summary>
        /// Fetches the query specified and returns the results in a list of TElement objects,
        /// which are created using the projectorFunc of the query specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TElement">
        /// The type of the element.
        /// </typeparam>
        List<TElement> FetchQuery<TElement>(DynamicQuery<TElement> query);

        /// <summary>
        /// Fetches the query with the projection specified from the source query specified.
        /// Typically used to fetch a typed view from a stored procedure source.
        /// </summary>
        /// <param name="projectionDefinition">
        /// The projection definition.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="TElement">
        /// The type of the element.
        /// </typeparam>
        /// <returns>
        /// List of TElement instances instantiated from each row in source
        /// </returns>
        List<TElement> FetchQueryFromSource<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source);

        /// <summary>
        /// Fetches a scalar value using the query specified, and returns this value typed
        /// as TValue, using a cast. The query specified will be converted to a scalar query
        /// prior to execution.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TValue">
        /// The type of the value to return.
        /// </typeparam>
        /// <returns>
        /// the value to fetch
        /// </returns>
        /// <remarks>
        /// Use nullable(Of T) for scalars which are a value type, to avoid crashes when
        /// the scalar query returns a NULL value.
        /// </remarks>
        TValue FetchScalar<TValue>(DynamicQuery query);

        /// <summary>
        /// Fetches the single entity of the set returned by the query and returns that entity.
        /// If there are no elements or more than 1 element, a NotSupportedException will
        /// be thrown.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// the first entity in the resultset
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        /// value in the resultset.
        /// </exception>
        TEntity FetchSingle<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;

        /// <summary>
        /// Fetches the single object of the set returned by the query and returns that object.
        /// If there are no elements or more than 1 element, a NotSupportedException will
        /// be thrown.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// the first object in the resultset
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        /// value in the resultset.
        /// </exception>
        T FetchSingle<T>(DynamicQuery<T> query);
        #endregion

        #region "IDataAccess adapter async extension methods"

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataReader(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,System.Data.CommandBehavior).
        /// Fetches the query as an open data reader.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="behavior">
        /// The behavior.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <remarks>
        /// Ignores nested queries and projection logic embedded in a lambda specification
        /// in the query. The DataReader returned will contain the resultset of the plain
        /// SQL query.
        /// </remarks>
        Task<IDataReader> FetchAsDataReaderAsync(DynamicQuery query, CommandBehavior behavior, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        /// Fetches the specified query into a new DataTable specified and returns that datatable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <returns>
        /// a new DataTable with the data fetched.
        /// </returns>
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,System.Data.DataTable).
        /// Fetches the specified query into the DataTable specified and returns that datatable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="destination">
        /// The destination datatable to fetch the data into.
        /// </param>
        /// <returns>
        /// the destination datatable specified.
        /// </returns>
        /// <remarks>
        /// If the DataTable specified already has columns defined, they have to have compatible
        /// types and have to be in the same order as the columns in the resultset of the
        /// query. It's recommended to have columns with the same names as the resultset
        /// of the query, to be able to convert null values to DBNull.Value.
        /// </remarks>
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, DataTable destination);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,System.Data.DataTable).
        /// Fetches the specified query into the DataTable specified and returns that datatable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="destination">
        /// The destination datatable to fetch the data into.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// the destination datatable specified.
        /// </returns>
        /// <remarks>
        /// If the DataTable specified already has columns defined, they have to have compatible
        /// types and have to be in the same order as the columns in the resultset of the
        /// query. It's recommended to have columns with the same names as the resultset
        /// of the query, to be able to convert null values to DBNull.Value.
        /// </remarks>
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, DataTable destination, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsDataTable(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        /// Fetches the specified query into a new DataTable specified and returns that datatable.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <returns>
        /// a new DataTable with the data fetched.
        /// </returns>
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsProjection(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector).
        /// Fetches the query as a projection, using the projector specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="projector">
        /// The projector.
        /// </param>
        /// <remarks>
        /// Will ignore nested queries. Use with queries without nested / hierarchical queries.
        /// The projector has to be setup and ready to use when calling this method
        /// </remarks>
        Task FetchAsProjectionAsync(DynamicQuery query, IGeneralDataProjector projector);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchAsProjection(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery,SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector).
        /// Fetches the query as a projection, using the projector specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="projector">
        /// The projector.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <remarks>
        /// Will ignore nested queries. Use with queries without nested / hierarchical queries.
        /// The projector has to be setup and ready to use when calling this method
        /// </remarks>
        Task FetchAsProjectionAsync(DynamicQuery query, IGeneralDataProjector projector, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        /// Fetches the first object of the set returned by the query and returns that object,
        /// if any, otherwise null.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// the first object in the resultset, or null if the resultset is empty.
        /// </returns>
        Task<T> FetchFirstAsync<T>(DynamicQuery<T> query);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        /// Fetches the first object of the set returned by the query and returns that object,
        /// if any, otherwise null.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// the first object in the resultset, or null if the resultset is empty.
        /// </returns>
        Task<T> FetchFirstAsync<T>(DynamicQuery<T> query, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the first entity of the set returned by the query and returns that entity,
        /// if any, otherwise null.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// the first entity in the resultset, or null if the resultset is empty.
        /// </returns>
        Task<TEntity> FetchFirstAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchFirst``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the first entity of the set returned by the query and returns that entity,
        /// if any, otherwise null.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// the first entity in the resultset, or null if the resultset is empty.
        /// </returns>
        Task<TEntity> FetchFirstAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : EntityBase2, IEntity2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        /// Fetches the query specified and returns the results in a list of TElement objects,
        /// which are created using the projectorFunc of the query specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="TElement">
        /// The type of the element.
        /// </typeparam>
        Task<List<TElement>> FetchQueryAsync<TElement>(DynamicQuery<TElement> query, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``2(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0},``1).
        /// Fetches the query specified on the adapter specified into the collectionToFill
        /// specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="collectionToFill">
        /// The collection to fill.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <typeparam name="TCollection">
        /// The type of the collection.
        /// </typeparam>
        /// <returns>
        /// collectionToFill, filled with the query fetch results.
        /// </returns>
        /// <remarks>
        /// Equal to calling adapter.FetchEntityCollection(), so entities already present
        /// in collectionToFill are left as-is. If the fetch has to take into account a Context,
        /// the passed collectionToFill has to be assigned to the context before calling
        /// this method.
        /// </remarks>
        Task<TCollection> FetchQueryAsync<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill, CancellationToken cancellationToken)
            where TEntity : IEntity2
            where TCollection : IEntityCollection2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``2(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0},``1).
        /// Fetches the query specified on the adapter specified into the collectionToFill
        /// specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="collectionToFill">
        /// The collection to fill.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <typeparam name="TCollection">
        /// The type of the collection.
        /// </typeparam>
        /// <returns>
        /// collectionToFill, filled with the query fetch results.
        /// </returns>
        /// <remarks>
        /// Equal to calling adapter.FetchEntityCollection(), so entities already present
        /// in collectionToFill are left as-is. If the fetch has to take into account a Context,
        /// the passed collectionToFill has to be assigned to the context before calling
        /// this method.
        /// </remarks>
        Task<TCollection> FetchQueryAsync<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill)
            where TEntity : IEntity2
            where TCollection : IEntityCollection2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// EntityCollection(Of TEntity) with the results of the query fetch
        /// </returns>
        Task<IEntityCollection2> FetchQueryAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : IEntity2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        /// Fetches the query specified and returns the results in plain object arrays, one
        /// object array per returned row of the query specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        Task<List<object[]>> FetchQueryAsync(DynamicQuery query);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        /// Fetches the query specified and returns the results in plain object arrays, one
        /// object array per returned row of the query specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        Task<List<object[]>> FetchQueryAsync(DynamicQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        /// Fetches the query specified and returns the results in a list of TElement objects,
        /// which are created using the projectorFunc of the query specified.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TElement">
        /// The type of the element.
        /// </typeparam>
        Task<List<TElement>> FetchQueryAsync<TElement>(DynamicQuery<TElement> query);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the query specified on the adapter specified. Uses the TEntity type to
        /// produce an EntityCollection(Of TEntity) for the results to return
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// EntityCollection(Of TEntity) with the results of the query fetch
        /// </returns>
        Task<IEntityCollection2> FetchQueryAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : IEntity2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQueryFromSource``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0},SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        /// Fetches the query which projection specified from the source query specified.
        /// Typically used to fetch a typed view from a stored procedure source.
        /// </summary>
        /// <param name="projectionDefinition">
        /// The projection definition.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="TElement">
        /// The type of the element.
        /// </typeparam>
        /// <returns>
        /// List of TElement instances instantiated from each row in source
        /// </returns>
        Task<List<TElement>> FetchQueryFromSourceAsync<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchQueryFromSource``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0},SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        /// Fetches the query which projection specified from the source query specified.
        /// Typically used to fetch a typed view from a stored procedure source.
        /// </summary>
        /// <param name="projectionDefinition">
        /// The projection definition.
        /// </param>
        /// <param name="source">
        /// The source.
        /// </param>
        /// <typeparam name="TElement">
        /// The type of the element.
        /// </typeparam>
        /// <returns>
        /// List of TElement instances instantiated from each row in source
        /// </returns>
        Task<List<TElement>> FetchQueryFromSourceAsync<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchScalar``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        /// Fetches a scalar value using the query specified, and returns this value typed
        /// as TValue, using a cast. The query specified will be converted to a scalar query
        /// prior to execution.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TValue">
        /// The type of the value to return.
        /// </typeparam>
        /// <returns>
        /// the value to fetch
        /// </returns>
        /// <remarks>
        /// Use nullable(Of T) for scalars which are a value type, to avoid crashes when
        /// the scalar query returns a NULL value.
        /// </remarks>
        Task<TValue> FetchScalarAsync<TValue>(DynamicQuery query);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchScalar``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery).
        /// Fetches a scalar value using the query specified, and returns this value typed
        /// as TValue, using a cast. The query specified will be converted to a scalar query
        /// prior to execution.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="TValue">
        /// The type of the value to return.
        /// </typeparam>
        /// <returns>
        /// the value to fetch
        /// </returns>
        /// <remarks>
        /// Use nullable(Of T) for scalars which are a value type, to avoid crashes when
        /// the scalar query returns a NULL value.
        /// </remarks>
        Task<TValue> FetchScalarAsync<TValue>(DynamicQuery query, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        /// Fetches the single object of the set returned by the query and returns that object.
        /// If there are no elements or more than 1 element, a NotSupportedException will
        /// be thrown.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// the first object in the resultset
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        /// value in the resultset.
        /// </exception>
        Task<T> FetchSingleAsync<T>(DynamicQuery<T> query, CancellationToken cancellationToken);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.DynamicQuery{``0}).
        /// Fetches the single object of the set returned by the query and returns that object.
        /// If there are no elements or more than 1 element, a NotSupportedException will
        /// be thrown.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// the first object in the resultset
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        /// value in the resultset.
        /// </exception>
        Task<T> FetchSingleAsync<T>(DynamicQuery<T> query);

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the single entity of the set returned by the query and returns that entity.
        /// If there are no elements or more than 1 element, a NotSupportedException will
        /// be thrown.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <param name="cancellationToken">
        /// The cancellation token.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// the first entity in the resultset
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        /// value in the resultset.
        /// </exception>
        Task<TEntity> FetchSingleAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : EntityBase2, IEntity2;

        /// <summary>
        /// Async variant of SD.LLBLGen.Pro.QuerySpec.Adapter.AdapterExtensionMethods.FetchSingle``1(SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter,SD.LLBLGen.Pro.QuerySpec.EntityQuery{``0}).
        /// Fetches the single entity of the set returned by the query and returns that entity.
        /// If there are no elements or more than 1 element, a NotSupportedException will
        /// be thrown.
        /// </summary>
        /// <param name="query">
        /// The query.
        /// </param>
        /// <typeparam name="TEntity">
        /// The type of the entity.
        /// </typeparam>
        /// <returns>
        /// the first entity in the resultset
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">
        /// Thrown if the resultset has 0 or 2 or more elements, as Single requires a single
        /// value in the resultset.
        /// </exception>
        Task<TEntity> FetchSingleAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;
        #endregion
    }
}