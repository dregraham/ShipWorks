using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
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
    [NDependIgnoreTooManyParamsAttribute]
    public interface ISqlAdapter : IDataAccessCore, IDisposable, ITransactionController
    {
        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        bool SaveAndRefetch(IEntity2 entity);

        /// <summary>
        /// Save the given entity, and automatically refetch it back. Returns true if there were any entities in the graph that were dirty and saved.  Returns
        /// false if nothing was dirty and thus nothing written to the database.
        /// </summary>
        Task<bool> SaveAndRefetchAsync(IEntity2 entity);

        #region "IDataAccess adapter methods"
        //
        // Summary:
        //     The flag (default true) to signal the adapter that entities participating in
        //     a transaction controlled by this adapter are tracked during the transaction and
        //     which values are rolled back after a rollback of the transaction itself. Only
        //     set this flag to false if the entities participating in a transaction are not
        //     kept in memory during or after the transaction's life time.
        bool KeepTrackOfTransactionParticipants { get; set; }
        //
        // Summary:
        //     Gets or sets the parameterised prefetch path threshold. This threshold is used
        //     to determine when the prefetch path logic should switch to a subquery or when
        //     it should use a WHERE field IN (value1, value2, ... valueN) construct, based
        //     on the # of elements in the parent collection. If that # of elements exceeds
        //     this threshold, a subquery is constructed, otherwise field IN (value1, value2,
        //     ...) construct is used. The default value is 50. On average, this is faster than
        //     using a subquery which returns 50 elements. Use this to tune prefetch path fetch
        //     logic for your particular needs. This threshold is also used to determine if
        //     paging is possible. A page size bigger than this threshold will disable the paging
        //     functionality when using paging + prefetch paths.
        //
        // Remarks:
        //     Testing showed that values larger than 300 will be slower than a subquery. Special
        //     thanks to Marcus Mac Innes (http://www.styledesign.biz) for this optimization
        //     code.
        int ParameterisedPrefetchPathThreshold { get; set; }
        //
        // Summary:
        //     Gets / sets the isolation level a transaction should use. Setting this during
        //     a transaction in progress has no effect on the current running transaction.
        IsolationLevel TransactionIsolationLevel { get; set; }
        //
        // Summary:
        //     Gets the name of the transaction. Setting this during a transaction in progress
        //     has no effect on the current running transaction.
        string TransactionName { get; set; }
        //
        // Summary:
        //     Gets / sets the connection string to use for the connection with the database.
        string ConnectionString { get; set; }
        //
        // Summary:
        //     Gets / sets KeepConnectionOpen, a flag used to keep open connections after a
        //     database action has finished.
        bool KeepConnectionOpen { get; set; }
        //
        // Summary:
        //     Gets / sets the timeout value to use with the command object(s) created by the
        //     adapter. Default is 30 seconds Set this prior to calling a method which executes
        //     database logic.
        int CommandTimeOut { get; set; }
        //
        // Summary:
        //     Gets a value indicating whether a System.Transactions transaction is going on.
        //     If not, false is returned.
        bool InSystemTransaction { get; }
        //
        // Summary:
        //     Gets or sets the active recovery strategy to use with supported actions on this
        //     DataAccessAdapter. If null (default), no recovery strategy is used and all exceptions
        //     are fatal.
        RecoveryStrategyBase ActiveRecoveryStrategy { get; set; }
        //
        // Summary:
        //     Gets the function mappings for the DQE related to this object. These function
        //     mappings are static and therefore not changeable.
        FunctionMappingStore FunctionMappings { get; }

        //
        // Summary:
        //     Closes the active connection. If no connection is available or the connection
        //     is closed, nothing is done.
        void CloseConnection();
        //
        // Summary:
        //     Creates a new predicate expression which filters on the primary key fields and
        //     the set values for the given primary key fields. If no primary key fields are
        //     specified, null is returned.
        //
        // Parameters:
        //   primaryKeyFields:
        //     ArrayList with IEntityField2 instances which form the primary key for which the
        //     filter has to be constructed
        //
        // Returns:
        //     filled in predicate expression or null if no primary key fields are specified.
        //
        // Remarks:
        //     Call this method passing IEntity2.Fields.PrimaryKeyFields
        IPredicateExpression CreatePrimaryKeyFilter(IList primaryKeyFields);
        //
        // Summary:
        //     Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        //     from the persistent storage if they match the filter supplied in filterBucket.
        //
        // Parameters:
        //   typeOfEntity:
        //     The type of the entity to retrieve persistence information.
        //
        //   filterBucket:
        //     filter information to filter out the entities to delete
        //
        // Returns:
        //     the amount of physically deleted entities
        //
        // Remarks:
        //     Not supported for entities which are in a TargetPerEntity hierarchy.
        int DeleteEntitiesDirectly(Type typeOfEntity, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        //     from the persistent storage if they match the filter supplied in filterBucket.
        //
        // Parameters:
        //   entityName:
        //     The name of the entity to retrieve persistence information. For example "CustomerEntity".
        //     This name can be retrieved from an existing entity's Name property.
        //
        //   filterBucket:
        //     filter information to filter out the entities to delete
        //
        // Returns:
        //     the amount of physically deleted entities
        int DeleteEntitiesDirectly(string entityName, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.Type,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        //     Deletes all entities of the type passed in from the persistent storage if they
        //     match the filter supplied in filterBucket.
        //
        // Parameters:
        //   typeOfEntity:
        //     The type of the entity to retrieve persistence information.
        //
        //   filterBucket:
        //     filter information to filter out the entities to delete
        //
        // Returns:
        //     the amount of physically deleted entities
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     typeOfEntity;typeOfEntity can't be null
        //
        // Remarks:
        //     Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        //     performs authorization. Use this overload instead of the one which accepts a
        //     name instead of a type instance if you want to have authorization support at
        //     runtime.
        Task<int> DeleteEntitiesDirectlyAsync(Type typeOfEntity, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.String,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        //     Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        //     from the persistent storage if they match the filter supplied in filterBucket.
        //
        // Parameters:
        //   entityName:
        //     The name of the entity to retrieve persistence information. For example "CustomerEntity".
        //     This name can be retrieved from an existing entity's LLBLGenProEntityName property.
        //
        //   filterBucket:
        //     filter information to filter out the entities to delete
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the amount of physically deleted entities
        //
        // Remarks:
        //     Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        //     doesn't support Authorization or Auditing. It's recommended, if you want to use
        //     authorization and/or auditing on this method, use the overload of DeleteEntitiesDirectly
        //     which accepts a type.
        Task<int> DeleteEntitiesDirectlyAsync(string entityName, IRelationPredicateBucket filterBucket, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.String,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        //     Deletes all entities of the name passed in as entityName (e.g. "CustomerEntity")
        //     from the persistent storage if they match the filter supplied in filterBucket.
        //
        // Parameters:
        //   entityName:
        //     The name of the entity to retrieve persistence information. For example "CustomerEntity".
        //     This name can be retrieved from an existing entity's LLBLGenProEntityName property.
        //
        //   filterBucket:
        //     filter information to filter out the entities to delete
        //
        // Returns:
        //     the amount of physically deleted entities
        //
        // Remarks:
        //     Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        //     doesn't support Authorization or Auditing. It's recommended, if you want to use
        //     authorization and/or auditing on this method, use the overload of DeleteEntitiesDirectly
        //     which accepts a type.
        Task<int> DeleteEntitiesDirectlyAsync(string entityName, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntitiesDirectly(System.Type,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        //     Deletes all entities of the type passed in from the persistent storage if they
        //     match the filter supplied in filterBucket.
        //
        // Parameters:
        //   typeOfEntity:
        //     The type of the entity to retrieve persistence information.
        //
        //   filterBucket:
        //     filter information to filter out the entities to delete
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the amount of physically deleted entities
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     typeOfEntity;typeOfEntity can't be null
        //
        // Remarks:
        //     Not supported for entities which are in a TargetPerEntity hierarchy. This overload
        //     performs authorization. Use this overload instead of the one which accepts a
        //     name instead of a type instance if you want to have authorization support at
        //     runtime.
        Task<int> DeleteEntitiesDirectlyAsync(Type typeOfEntity, IRelationPredicateBucket filterBucket, CancellationToken cancellationToken);
        //
        // Summary:
        //     Deletes the specified entity from the persistent storage. The entity is not usable
        //     after this call, the state is set to OutOfSync. Will use the current transaction
        //     if a transaction is in progress.
        //
        // Parameters:
        //   entityToDelete:
        //     The entity instance to delete from the persistent storage
        //
        // Returns:
        //     true if the delete was successful, otherwise false.
        bool DeleteEntity(IEntity2 entityToDelete);
        //
        // Summary:
        //     Deletes the specified entity from the persistent storage. The entity is not usable
        //     after this call, the state is set to OutOfSync. Will use the current transaction
        //     if a transaction is in progress.
        //
        // Parameters:
        //   entityToDelete:
        //     The entity instance to delete from the persistent storage
        //
        //   deleteRestriction:
        //     Predicate expression, meant for concurrency checks in a delete query
        //
        // Returns:
        //     true if the delete was successful, otherwise false.
        bool DeleteEntity(IEntity2 entityToDelete, IPredicateExpression deleteRestriction);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression).
        //     Deletes the specified entity from the persistent storage. The entity is not usable
        //     after this call, the state is set to OutOfSync. Will use the current transaction
        //     if a transaction is in progress.
        //
        // Parameters:
        //   entityToDelete:
        //     The entity instance to delete from the persistent storage
        //
        //   deleteRestriction:
        //     Predicate expression, meant for concurrency checks in a delete query
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if the delete was successful, otherwise false.
        //
        // Exceptions:
        //   T:SD.LLBLGen.Pro.ORMSupportClasses.ORMConcurrencyException:
        //     Will throw an ORMConcurrencyException if the delete fails.
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, IPredicateExpression deleteRestriction, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression).
        //     Deletes the specified entity from the persistent storage. The entity is not usable
        //     after this call, the state is set to OutOfSync. Will use the current transaction
        //     if a transaction is in progress.
        //
        // Parameters:
        //   entityToDelete:
        //     The entity instance to delete from the persistent storage
        //
        //   deleteRestriction:
        //     Predicate expression, meant for concurrency checks in a delete query
        //
        // Returns:
        //     true if the delete was successful, otherwise false.
        //
        // Exceptions:
        //   T:SD.LLBLGen.Pro.ORMSupportClasses.ORMConcurrencyException:
        //     Will throw an ORMConcurrencyException if the delete fails.
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, IPredicateExpression deleteRestriction);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        //     Deletes the specified entity from the persistent storage. The entity is not usable
        //     after this call, the state is set to OutOfSync. Will use the current transaction
        //     if a transaction is in progress. If the passed in entity has a concurrency predicate
        //     factory object, the returned predicate expression is used to restrict the delete
        //     process.
        //
        // Parameters:
        //   entityToDelete:
        //     The entity instance to delete from the persistent storage
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if the delete was successful, otherwise false.
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        //     Deletes the specified entity from the persistent storage. The entity is not usable
        //     after this call, the state is set to OutOfSync. Will use the current transaction
        //     if a transaction is in progress. If the passed in entity has a concurrency predicate
        //     factory object, the returned predicate expression is used to restrict the delete
        //     process.
        //
        // Parameters:
        //   entityToDelete:
        //     The entity instance to delete from the persistent storage
        //
        // Returns:
        //     true if the delete was successful, otherwise false.
        Task<bool> DeleteEntityAsync(IEntity2 entityToDelete);
        //
        // Summary:
        //     Deletes all dirty objects inside the collection passed from the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available. Entities
        //     which are physically deleted from the persistent storage are marked with the
        //     state 'Deleted' but are not removed from the collection. If the passed in entity
        //     has a concurrency predicate factory object, the returned predicate expression
        //     is used to restrict the delete process.
        //
        // Parameters:
        //   collectionToDelete:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        // Returns:
        //     the amount of physically deleted entities
        int DeleteEntityCollection(IEntityCollection2 collectionToDelete);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        //     Deletes all dirty objects inside the collection passed from the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available. Entities
        //     which are physically deleted from the persistent storage are marked with the
        //     state 'Deleted' but are not removed from the collection.
        //
        // Parameters:
        //   collectionToDelete:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the amount of physically deleted entities
        Task<int> DeleteEntityCollectionAsync(IEntityCollection2 collectionToDelete, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.DeleteEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        //     Deletes all dirty objects inside the collection passed from the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available. Entities
        //     which are physically deleted from the persistent storage are marked with the
        //     state 'Deleted' but are not removed from the collection.
        //
        // Parameters:
        //   collectionToDelete:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        // Returns:
        //     the amount of physically deleted entities
        Task<int> DeleteEntityCollectionAsync(IEntityCollection2 collectionToDelete);
        //
        // Summary:
        //     Executes the passed in action query and, if not null, runs it inside the passed
        //     in transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     ActionQuery to execute.
        //
        // Returns:
        //     execution result, which is the amount of rows affected (if applicable)
        int ExecuteActionQuery(IActionQuery queryToExecute);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteActionQuery(SD.LLBLGen.Pro.ORMSupportClasses.IActionQuery).
        //     Executes the passed in action query and, if not null, runs it inside the passed
        //     in transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     ActionQuery to execute.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     execution result, which is the amount of rows affected (if applicable)
        Task<int> ExecuteActionQueryAsync(IActionQuery queryToExecute, CancellationToken cancellationToken);
        //
        // Summary:
        //     Executes the passed in retrieval query and returns the results in the datatable
        //     specified using the passed in data-adapter. It sets the connection object of
        //     the command object of query object passed in to the connection object of this
        //     class.
        //
        // Parameters:
        //   queryToExecute:
        //     Retrieval query to execute
        //
        //   dataAdapterToUse:
        //     The dataadapter to use to fill the datatable.
        //
        //   tableToFill:
        //     DataTable to fill
        //
        //   fieldsPersistenceInfo:
        //     Fields persistence info objects for the fields used for the query. Required for
        //     type conversion on values.
        //
        // Returns:
        //     true if succeeded, false otherwise
        bool ExecuteMultiRowDataTableRetrievalQuery(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, DataTable tableToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo);
        //
        // Summary:
        //     Executes the passed in retrieval query and returns the results as a datatable
        //     using the passed in data-adapter. It sets the connection object of the command
        //     object of query object passed in to the connection object of this class.
        //
        // Parameters:
        //   queryToExecute:
        //     Retrieval query to execute
        //
        //   dataAdapterToUse:
        //     The dataadapter to use to fill the datatable.
        //
        //   fieldsPersistenceInfo:
        //     Fields persistence info objects for the fields used for the query. Required for
        //     type conversion on values.
        //
        // Returns:
        //     DataTable with the rows requested
        DataTable ExecuteMultiRowDataTableRetrievalQuery(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, IFieldPersistenceInfo[] fieldsPersistenceInfo);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteMultiRowDataTableRetrievalQuery(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery,System.Data.Common.DbDataAdapter,System.Data.DataTable,SD.LLBLGen.Pro.ORMSupportClasses.IFieldPersistenceInfo[]).
        //     Executes the passed in retrieval query and returns the results in the datatable
        //     specified using the passed in data-adapter. It sets the connection object of
        //     the command object of query object passed in to the connection object of this
        //     class.
        //
        // Parameters:
        //   queryToExecute:
        //     Retrieval query to execute
        //
        //   dataAdapterToUse:
        //     The dataadapter to use to fill the datatable.
        //
        //   tableToFill:
        //     DataTable to fill
        //
        //   fieldsPersistenceInfo:
        //     Fields persistence info objects for the fields used for the query. Required for
        //     type conversion on values.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if succeeded, false otherwise
        Task<bool> ExecuteMultiRowDataTableRetrievalQueryAsync(IRetrievalQuery queryToExecute, DbDataAdapter dataAdapterToUse, DataTable tableToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, CancellationToken cancellationToken);
        //
        // Summary:
        //     Executes the passed in retrieval query and, if not null, runs it inside the passed
        //     in transaction. Used to read 1 or more rows. It sets the connection object of
        //     the command object of query object passed in to the connection object of this
        //     class.
        //
        // Parameters:
        //   queryToExecute:
        //     Retrieval query to execute
        //
        //   entityFactory:
        //     the factory object which can produce the entities this method has to fill.
        //
        //   collectionToFill:
        //     Collection to fill with the retrieved rows.
        //
        //   fieldsPersistenceInfo:
        //     The persistence information for the fields of the entity created by entityFactory
        //
        //   allowDuplicates:
        //     Flag to signal if duplicates in the datastream should be loaded into the collection
        //     (true) or not (false)
        //
        //   fieldsUsedForQuery:
        //     Fields used for producing the query
        void ExecuteMultiRowRetrievalQuery(IRetrievalQuery queryToExecute, IEntityFactory2 entityFactory, IEntityCollection2 collectionToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, bool allowDuplicates, IEntityFields2 fieldsUsedForQuery);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteMultiRowRetrievalQuery(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery,SD.LLBLGen.Pro.ORMSupportClasses.IEntityFactory2,SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,SD.LLBLGen.Pro.ORMSupportClasses.IFieldPersistenceInfo[],System.Boolean,SD.LLBLGen.Pro.ORMSupportClasses.IEntityFields2).
        //     Executes the passed in retrieval query and, if not null, runs it inside the passed
        //     in transaction. Used to read 1 or more rows. It sets the connection object of
        //     the command object of query object passed in to the connection object of this
        //     class.
        //
        // Parameters:
        //   queryToExecute:
        //     Retrieval query to execute
        //
        //   entityFactory:
        //     the factory object which can produce the entities this method has to fill.
        //
        //   collectionToFill:
        //     Collection to fill with the retrieved rows.
        //
        //   fieldsPersistenceInfo:
        //     The persistence information for the fields of the entity created by entityFactory
        //
        //   allowDuplicates:
        //     Flag to signal if duplicates in the datastream should be loaded into the collection
        //     (true) or not (false)
        //
        //   fieldsUsedForQuery:
        //     Fields used for producing the query
        //
        //   cancellationToken:
        //     The cancellation token.
        Task ExecuteMultiRowRetrievalQueryAsync(IRetrievalQuery queryToExecute, IEntityFactory2 entityFactory, IEntityCollection2 collectionToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo, bool allowDuplicates, IEntityFields2 fieldsUsedForQuery, CancellationToken cancellationToken);
        //
        // Summary:
        //     Executes the passed in query as a scalar query and returns the value returned
        //     from this scalar execution.
        //
        // Parameters:
        //   queryToExecute:
        //     a scalar query, which is a SELECT query which returns a single value
        //
        // Returns:
        //     the scalar value returned from the query.
        object ExecuteScalarQuery(IRetrievalQuery queryToExecute);
        //
        // Summary:
        //     Executes the passed in retrieval query and, if not null, runs it inside the passed
        //     in transaction. Used to read 1 row. It sets the connection object of the command
        //     object of query object passed in to the connection object of this class.
        //
        // Parameters:
        //   queryToExecute:
        //     Retrieval query to execute
        //
        //   fieldsToFill:
        //     The IEntityFields2 object to store the fetched data in
        //
        //   fieldsPersistenceInfo:
        //     The IFieldPersistenceInfo objects for the fieldsToFill fields
        void ExecuteSingleRowRetrievalQuery(IRetrievalQuery queryToExecute, IEntityFields2 fieldsToFill, IFieldPersistenceInfo[] fieldsPersistenceInfo);
        //
        // Summary:
        //     Executes the specified plain SQL query using this adapter. Every parameter value
        //     is converted into one or more parameters which have to be pre-defined in the
        //     sqlQuery
        //
        // Parameters:
        //   sqlQuery:
        //     The SQL query to execute. Should contain parameter names for the parameter values,
        //     or placeholders for parameter sets. See documentation for details regarding format
        //     specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Returns:
        //     The value returned by the executed DbCommand. In general this is the number of
        //     rows affected by the executed sqlQuery
        int ExecuteSQL(string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteSQL(System.String,System.Object)
        //     Executes the specified plain SQL query using this adapter. Every parameter value
        //     is converted into one or more parameters which have to be pre-defined in the
        //     sqlQuery
        //
        // Parameters:
        //   sqlQuery:
        //     The SQL query to execute. Should contain parameter names for the parameter values,
        //     or placeholders for parameter sets. See documentation for details regarding format
        //     specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Returns:
        //     The value returned by the executed DbCommand. In general this is the number of
        //     rows affected by the executed sqlQuery
        Task<int> ExecuteSQLAsync(string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ExecuteSQL(System.String,System.Object)
        //     Executes the specified plain SQL query using this adapter. Every parameter value
        //     is converted into one or more parameters which have to be pre-defined in the
        //     sqlQuery
        //
        // Parameters:
        //   cancellationToken:
        //     The cancellation token.
        //
        //   sqlQuery:
        //     The SQL query to execute. Should contain parameter names for the parameter values,
        //     or placeholders for parameter sets. See documentation for details regarding format
        //     specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Returns:
        //     The value returned by the executed DbCommand. In general this is the number of
        //     rows affected by the executed sqlQuery
        Task<int> ExecuteSQLAsync(CancellationToken cancellationToken, string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in and executes that retrieval query
        //     to return an open, ready to use IDataReader. The datareader's command behavior
        //     is set to the readerBehavior passed in. If a transaction is in progress, the
        //     command is wired to the transaction.
        //
        // Parameters:
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   sortClauses:
        //     The sort clauses.
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        //
        //   pageNumber:
        //     The page number.
        //
        //   pageSize:
        //     Size of the page.
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, int pageNumber, int pageSize);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in and executes that retrieval query
        //     to return an open, ready to use IDataReader. The datareader's command behavior
        //     is set to the readerBehavior passed in. If a transaction is in progress, the
        //     command is wired to the transaction.
        //
        // Parameters:
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, bool allowDuplicates);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in and executes that retrieval query
        //     to return an open, ready to use IDataReader. The datareader's command behavior
        //     is set to the readerBehavior passed in. If a transaction is in progress, the
        //     command is wired to the transaction.
        //
        // Parameters:
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   sortClauses:
        //     The sort clauses.
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in and executes that retrieval query
        //     to return an open, ready to use IDataReader. The datareader's command behavior
        //     is set to the readerBehavior passed in. If a transaction is in progress, the
        //     command is wired to the transaction.
        //
        // Parameters:
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   sortClauses:
        //     The sort clauses.
        //
        //   groupByClause:
        //     The group by clause.
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        //
        //   pageNumber:
        //     The page number.
        //
        //   pageSize:
        //     Size of the page.
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        IDataReader FetchDataReader(IEntityFields2 fields, IRelationPredicateBucket filter, CommandBehavior readerBehavior, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IGroupByCollection groupByClause, bool allowDuplicates, int pageNumber, int pageSize);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in and executes that retrieval query
        //     to return an open, ready to use IDataReader. The datareader's command behavior
        //     is set to the readerBehavior passed in. If a transaction is in progress, the
        //     command is wired to the transaction.
        //
        // Parameters:
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   parameters:
        //     The parameters.
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        IDataReader FetchDataReader(CommandBehavior readerBehavior, QueryParameters parameters);
        //
        // Summary:
        //     Executes the passed in retrieval query and returns an open, ready to use IDataReader.
        //     The datareader's command behavior is set to the readerBehavior passed in. If
        //     a transaction is in progress, the command is wired to the transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     The query to execute.
        //
        //   readerBehavior:
        //     The reader behavior to set.
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        IDataReader FetchDataReader(IRetrievalQuery queryToExecute, CommandBehavior readerBehavior);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchDataReader(System.Data.CommandBehavior,SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        //     Creates a new Retrieval query from the elements passed in and executes that retrieval query
        //     to return an open, ready to use IDataReader. The datareader's command behavior
        //     is set to the readerBehavior passed in. If a transaction is in progress, the
        //     command is wired to the transaction.
        //
        // Parameters:
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   parameters:
        //     The parameters.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     parameters
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open
        Task<IDataReader> FetchDataReaderAsync(CommandBehavior readerBehavior, QueryParameters parameters, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchDataReader(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery,System.Data.CommandBehavior).
        //     Executes the passed in retrieval query and returns an open, ready to use IDataReader.
        //     The datareader's command behavior is set to the readerBehavior passed in. If
        //     a transaction is in progress, the command is wired to the transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     The query to execute.
        //
        //   readerBehavior:
        //     The reader behavior to set.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     Open, ready to use IDataReader
        //
        // Remarks:
        //     Advanced functionality: be aware that the datareader returned is open, and the
        //     dataaccessadapter's connection is also open. It can be, if the query is set to
        //     cache its resultset, that the reader returned is actually a reader over the cached
        //     resultset. If you ordered the query to be cached, be sure to pass queryToExecute
        //     to the FetchProjection method to cache the resultset.
        Task<IDataReader> FetchDataReaderAsync(IRetrievalQuery queryToExecute, CommandBehavior readerBehavior, CancellationToken cancellationToken);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the passed in Entity2 object
        //     using a primary key filter. The primary key fields of the entity passed in have
        //     to have the primary key values. (Example: CustomerID has to have a value, when
        //     you want to fetch a CustomerEntity from the persistent storage into the passed
        //     in object). All fields specified in excludedFields are excluded from the fetch
        //     so the entity won't get any value set for those fields. excludedFields can be
        //     null or empty, in which case all fields are fetched (default).
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored. The primary
        //     key fields have to have a value.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress, so MVCC or other
        //     concurrency scheme used by the database can be utilized
        bool FetchEntity(IEntity2 entityToFetch, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the passed in Entity2 object
        //     using a primary key filter. The primary key fields of the entity passed in have
        //     to have the primary key values. (Example: CustomerID has to have a value, when
        //     you want to fetch a CustomerEntity from the persistent storage into the passed
        //     in object)
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored. The primary
        //     key fields have to have a value.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress, so MVCC or other
        //     concurrency scheme used by the database can be utilized
        bool FetchEntity(IEntity2 entityToFetch, IPrefetchPath2 prefetchPath, Context contextToUse);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the passed in Entity2 object
        //     using a primary key filter. The primary key fields of the entity passed in have
        //     to have the primary key values. (Example: CustomerID has to have a value, when
        //     you want to fetch a CustomerEntity from the persistent storage into the passed
        //     in object)
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored. The primary
        //     key fields have to have a value.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress, so MVCC or other
        //     concurrency scheme used by the database can be utilized
        bool FetchEntity(IEntity2 entityToFetch, Context contextToUse);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the passed in Entity2 object
        //     using a primary key filter. The primary key fields of the entity passed in have
        //     to have the primary key values. (Example: CustomerID has to have a value, when
        //     you want to fetch a CustomerEntity from the persistent storage into the passed
        //     in object)
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored. The primary
        //     key fields have to have a value.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress, so MVCC or other
        //     concurrency scheme used by the database can be utilized
        bool FetchEntity(IEntity2 entityToFetch);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the passed in Entity2 object
        //     using a primary key filter. The primary key fields of the entity passed in have
        //     to have the primary key values. (Example: CustomerID has to have a value, when
        //     you want to fetch a CustomerEntity from the persistent storage into the passed
        //     in object)
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored. The primary
        //     key fields have to have a value.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress, so MVCC or other
        //     concurrency scheme used by the database can be utilized
        bool FetchEntity(IEntity2 entityToFetch, IPrefetchPath2 prefetchPath);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. It will apply paging and it will from there use a prefetch path fetch
        //     using the read page. It's important that pageSize is smaller than the set SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold.
        //     If pagesize is larger than the limits set for the SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold
        //     value, the query is likely to be slower than expected, though will work. If pageNumber
        //     / pageSize are set to values which disable paging, a normal prefetch path fetch
        //     will be performed.
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   prefetchPath:
        //     Prefetch path to use.
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        //   pageNumber:
        //     the page number to retrieve. First page is 1. When set to 0, no paging logic
        //     is applied
        //
        //   pageSize:
        //     the size of the page. When set to 0, no paging logic is applied
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        //
        // Remarks:
        //     Special thanks to Marcus Mac Innes (http://www.styledesign.biz) for the paging
        //     optimization code.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath, ExcludeIncludeFieldsList excludedIncludedFields, int pageNumber, int pageSize);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. It will apply paging and it will from there use a prefetch path fetch
        //     using the read page. It's important that pageSize is smaller than the set ParameterisedPrefetchPathThreshold.
        //     It will work, though if pagesize is larger than the limits set for the ParameterisedPrefetchPathThreshold
        //     value, the query is likely to be slower than expected.
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   prefetchPath:
        //     Prefetch path to use.
        //
        //   pageNumber:
        //     the page number to retrieve. First page is 1. When set to 0, no paging logic
        //     is applied
        //
        //   pageSize:
        //     the size of the page. When set to 0, no paging logic is applied
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath, int pageNumber, int pageSize);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched.
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   pageNumber:
        //     the page number to retrieve. First page is 1. When set to 0, no paging logic
        //     is applied
        //
        //   pageSize:
        //     the size of the page. When set to 0, no paging logic is applied
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, int pageNumber, int pageSize);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched.
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the parameters
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. It will apply paging and it will from there use a prefetch path fetch
        //     using the read page. It's important that pageSize is smaller than the set SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold.
        //     If pagesize is larger than the limits set for the SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold
        //     value, the query is likely to be slower than expected, though will work. If pageNumber
        //     / pageSize are set to values which disable paging, a normal prefetch path fetch
        //     will be performed.
        //
        // Parameters:
        //   parameters:
        //     The parameters.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        void FetchEntityCollection(QueryParameters parameters);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. This overload returns all found entities and doesn't apply sorting
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. This overload returns all found entities and doesn't apply sorting
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, ExcludeIncludeFieldsList excludedIncludedFields, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. This overload doesn't apply sorting
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. This overload returns all found entities and doesn't apply sorting
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched.
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Fetches one or more entities which match the filter information in the filterBucket
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched.
        //
        // Parameters:
        //   collectionToFill:
        //     EntityCollection object containing an entity factory which has to be filled
        //
        //   filterBucket:
        //     filter information for retrieving the entities. If null, all entities are returned
        //     of the type created by the factory in the passed in EntityCollection instance.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of entities to return. If 0, all entities matching the filter
        //     are returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        void FetchEntityCollection(IEntityCollection2 collectionToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IPrefetchPath2 prefetchPath);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        //     Fetches one or more entities which match the filter information in the parameters
        //     into the EntityCollection passed. The entity collection object has to contain
        //     an entity factory object which will be the factory for the entity instances to
        //     be fetched. It will apply paging and it will from there use a prefetch path fetch
        //     using the read page. It's important that pageSize is smaller than the set SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold.
        //     If pagesize is larger than the limits set for the SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.ParameterisedPrefetchPathThreshold
        //     value, the query is likely to be slower than expected, though will work. If pageNumber
        //     / pageSize are set to values which disable paging, a normal prefetch path fetch
        //     will be performed.
        //
        // Parameters:
        //   parameters:
        //     The parameters.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     If the passed in collectionToFill doesn't contain an entity factory.
        //
        // Remarks:
        //     Async variant
        Task FetchEntityCollectionAsync(QueryParameters parameters, CancellationToken cancellationToken);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the object specified using
        //     the filter specified. Use the entity's uniqueconstraint filter construction methods
        //     to construct the required uniqueConstraintFilter for the unique constraint you
        //     want to use.
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored.
        //
        //   uniqueConstraintFilter:
        //     The filter which should filter on fields with a unique constraint.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the object specified using
        //     the filter specified. Use the entity's uniqueconstraint filter construction methods
        //     to construct the required uniqueConstraintFilter for the unique constraint you
        //     want to use.
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored.
        //
        //   uniqueConstraintFilter:
        //     The filter which should filter on fields with a unique constraint.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, IPrefetchPath2 prefetchPath, Context contextToUse);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the object specified using
        //     the filter specified. Use the entity's uniqueconstraint filter construction methods
        //     to construct the required uniqueConstraintFilter for the unique constraint you
        //     want to use.
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored.
        //
        //   uniqueConstraintFilter:
        //     The filter which should filter on fields with a unique constraint.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, Context contextToUse);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the object specified using
        //     the filter specified. Use the entity's uniqueconstraint filter construction methods
        //     to construct the required uniqueConstraintFilter for the unique constraint you
        //     want to use.
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored.
        //
        //   uniqueConstraintFilter:
        //     The filter which should filter on fields with a unique constraint.
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter, IPrefetchPath2 prefetchPath);
        //
        // Summary:
        //     Fetches an entity from the persistent storage into the object specified using
        //     the filter specified. Use the entity's uniqueconstraint filter construction methods
        //     to construct the required uniqueConstraintFilter for the unique constraint you
        //     want to use.
        //
        // Parameters:
        //   entityToFetch:
        //     The entity object in which the fetched entity data will be stored.
        //
        //   uniqueConstraintFilter:
        //     The filter which should filter on fields with a unique constraint.
        //
        // Returns:
        //     true if the Fetch was successful, false otherwise
        bool FetchEntityUsingUniqueConstraint(IEntity2 entityToFetch, IPredicateExpression uniqueConstraintFilter);
        //
        // Summary:
        //     Loads the data for the excluded fields specified in the list of excluded fields
        //     into all the entities in the entities collection passed in.
        //
        // Parameters:
        //   entities:
        //     The entities to load the excluded field data into. The entities have to be either
        //     of the same type or have to be in the same inheritance hierarchy as the entity
        //     which factory is set in the collection.
        //
        //   excludedIncludedFields:
        //     The excludedIncludedFields object as it is used when fetching the passed in collection.
        //     If you used the excludedIncludedFields object to fetch only the fields in that
        //     list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        //     will fetch all other fields in the resultset for the entities in the collection
        //     excluding the fields in excludedIncludedFields.
        //
        // Remarks:
        //     The field data is set like a normal field value set, so authorization is applied
        //     to it. This routine batches fetches to have at most 5*ParameterisedPrefetchPathThreshold
        //     of parameters per fetch. Keep in mind that most databases have a limit on the
        //     # of parameters per query.
        void FetchExcludedFields(IEntityCollection2 entities, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Loads the data for the excluded fields specified in the list of excluded fields
        //     into the entity passed in.
        //
        // Parameters:
        //   entity:
        //     The entity to load the excluded field data into.
        //
        //   excludedIncludedFields:
        //     The excludedIncludedFields object as it is used when fetching the passed in entity.
        //     If you used the excludedIncludedFields object to fetch only the fields in that
        //     list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        //     will fetch all other fields in the resultset for the entities in the collection
        //     excluding the fields in excludedIncludedFields.
        //
        // Remarks:
        //     The field data is set like a normal field value set, so authorization is applied
        //     to it.
        void FetchExcludedFields(IEntity2 entity, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        //     Loads the data for the excluded fields specified in the list of excluded fields
        //     into the entity passed in.
        //
        // Parameters:
        //   entity:
        //     The entity to load the excluded field data into.
        //
        //   excludedIncludedFields:
        //     The excludedIncludedFields object as it is used when fetching the passed in entity.
        //     If you used the excludedIncludedFields object to fetch only the fields in that
        //     list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        //     will fetch all other fields in the resultset for the entities in the collection
        //     excluding the fields in excludedIncludedFields.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Remarks:
        //     The field data is set like a normal field value set, so authorization is applied
        //     to it.
        Task FetchExcludedFieldsAsync(IEntity2 entity, ExcludeIncludeFieldsList excludedIncludedFields, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        //     Loads the data for the excluded fields specified in the list of excluded fields
        //     into all the entities in the entities collection passed in.
        //
        // Parameters:
        //   entities:
        //     The entities to load the excluded field data into. The entities have to be either
        //     of the same type or have to be in the same inheritance hierarchy as the entity
        //     which factory is set in the collection.
        //
        //   excludedIncludedFields:
        //     The excludedIncludedFields object as it is used when fetching the passed in collection.
        //     If you used the excludedIncludedFields object to fetch only the fields in that
        //     list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        //     will fetch all other fields in the resultset for the entities in the collection
        //     excluding the fields in excludedIncludedFields.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Exceptions:
        //   T:SD.LLBLGen.Pro.ORMSupportClasses.ORMGeneralOperationException:
        //     The entity factory of the passed in entities collection is null.
        //
        // Remarks:
        //     The field data is set like a normal field value set, so authorization is applied
        //     to it. This routine batches fetches to have at most 5*ParameterizedThreshold
        //     of parameters per fetch. Keep in mind that most databases have a limit on the
        //     # of parameters per query.
        Task FetchExcludedFieldsAsync(IEntityCollection2 entities, ExcludeIncludeFieldsList excludedIncludedFields, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        //     Loads the data for the excluded fields specified in the list of excluded fields
        //     into the entity passed in.
        //
        // Parameters:
        //   entity:
        //     The entity to load the excluded field data into.
        //
        //   excludedIncludedFields:
        //     The excludedIncludedFields object as it is used when fetching the passed in entity.
        //     If you used the excludedIncludedFields object to fetch only the fields in that
        //     list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        //     will fetch all other fields in the resultset for the entities in the collection
        //     excluding the fields in excludedIncludedFields.
        //
        // Remarks:
        //     The field data is set like a normal field value set, so authorization is applied
        //     to it.
        Task FetchExcludedFieldsAsync(IEntity2 entity, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchExcludedFields(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,SD.LLBLGen.Pro.ORMSupportClasses.ExcludeIncludeFieldsList).
        //     Loads the data for the excluded fields specified in the list of excluded fields
        //     into all the entities in the entities collection passed in.
        //
        // Parameters:
        //   entities:
        //     The entities to load the excluded field data into. The entities have to be either
        //     of the same type or have to be in the same inheritance hierarchy as the entity
        //     which factory is set in the collection.
        //
        //   excludedIncludedFields:
        //     The excludedIncludedFields object as it is used when fetching the passed in collection.
        //     If you used the excludedIncludedFields object to fetch only the fields in that
        //     list (i.e. excludedIncludedFields.ExcludeContainedFields==false), the routine
        //     will fetch all other fields in the resultset for the entities in the collection
        //     excluding the fields in excludedIncludedFields.
        //
        // Exceptions:
        //   T:SD.LLBLGen.Pro.ORMSupportClasses.ORMGeneralOperationException:
        //     The entity factory of the passed in entities collection is null.
        //
        // Remarks:
        //     The field data is set like a normal field value set, so authorization is applied
        //     to it. This routine batches fetches to have at most 5*ParameterizedThreshold
        //     of parameters per fetch. Keep in mind that most databases have a limit on the
        //     # of parameters per query.
        Task FetchExcludedFieldsAsync(IEntityCollection2 entities, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the passed in entity factory.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   entityFactoryToUse:
        //     The factory which will be used to create a new entity object which will be fetched
        //
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        // Returns:
        //     The new entity fetched.
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath);
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the passed in entity factory.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   entityFactoryToUse:
        //     The factory which will be used to create a new entity object which will be fetched
        //
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        // Returns:
        //     The new entity fetched.
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the passed in entity factory.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   entityFactoryToUse:
        //     The factory which will be used to create a new entity object which will be fetched
        //
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful.
        //
        // Returns:
        //     The new entity fetched, or a previous entity fetched if that entity was in the
        //     context specified
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, Context contextToUse);
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the specified generic type.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity to fetch
        //
        // Returns:
        //     The new entity fetched, or a previous entity fetched if that entity was in the
        //     context specified
        //
        // Remarks:
        //     TEntity can't be a type which is an abstract entity. If you want to fetch an
        //     instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        //     which accepts an entity factory instead
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields) where TEntity : EntityBase2, IEntity2, new();
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the passed in entity factory.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   entityFactoryToUse:
        //     The factory which will be used to create a new entity object which will be fetched
        //
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        // Returns:
        //     The new entity fetched, or a previous entity fetched if that entity was in the
        //     context specified
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse);
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the specified generic type.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity to fetch
        //
        // Returns:
        //     The new entity fetched, or a previous entity fetched if that entity was in the
        //     context specified
        //
        // Remarks:
        //     TEntity can't be a type which is an abstract entity. If you want to fetch an
        //     instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        //     which accepts an entity factory instead
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse) where TEntity : EntityBase2, IEntity2, new();
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the specified generic type.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity to fetch
        //
        // Returns:
        //     The new entity fetched.
        //
        // Remarks:
        //     TEntity can't be a type which is an abstract entity. If you want to fetch an
        //     instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        //     which accepts an entity factory instead
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath) where TEntity : EntityBase2, IEntity2, new();
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the specified generic type.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful.
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity to fetch
        //
        // Returns:
        //     The new entity fetched, or a previous entity fetched if that entity was in the
        //     context specified
        //
        // Remarks:
        //     TEntity can't be a type which is an abstract entity. If you want to fetch an
        //     instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        //     which accepts an entity factory instead
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket, Context contextToUse) where TEntity : EntityBase2, IEntity2, new();
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the specified generic type.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        // Type parameters:
        //   TEntity:
        //     The type of entity to fetch
        //
        // Returns:
        //     The new entity fetched.
        //
        // Remarks:
        //     TEntity can't be a type which is an abstract entity. If you want to fetch an
        //     instance of an abstract entity (e.g. polymorphic fetch) please use the overload
        //     which accepts an entity factory instead
        TEntity FetchNewEntity<TEntity>(IRelationPredicateBucket filterBucket) where TEntity : EntityBase2, IEntity2, new();
        //
        // Summary:
        //     Fetches a new entity using the filter/relation combination filter passed in via
        //     filterBucket and the new entity is created using the passed in entity factory.
        //     Use this method when fetching a related entity using a current entity (for example,
        //     fetch the related Customer entity of an existing Order entity)
        //
        // Parameters:
        //   entityFactoryToUse:
        //     The factory which will be used to create a new entity object which will be fetched
        //
        //   filterBucket:
        //     the completely filled in IRelationPredicateBucket object which will be used as
        //     a filter for the fetch. The fetch will only load the first entity loaded, even
        //     if the filter results into more entities being fetched
        //
        //   prefetchPath:
        //     The prefetch path to use for this fetch, which will fetch all related entities
        //     defined by the path as well.
        //
        //   contextToUse:
        //     The context to add the entity to if the fetch was successful, and before the prefetch
        //     path is fetched. This ensures that the prefetch path is fetched using the context
        //     specified and will re-use already loaded entity objects.
        //
        //   excludedIncludedFields:
        //     The list of IEntityField2 objects which have to be excluded or included for the
        //     fetch. If null or empty, all fields are fetched (default). If an instance of
        //     ExcludeIncludeFieldsList is passed in and its ExcludeContainedFields property
        //     is set to false, the fields contained in excludedIncludedFields are kept in the
        //     query, the rest of the fields in the query are excluded.
        //
        // Returns:
        //     The new entity fetched, or a previous entity fetched if that entity was in the
        //     context specified
        IEntity2 FetchNewEntity(IEntityFactory2 entityFactoryToUse, IRelationPredicateBucket filterBucket, IPrefetchPath2 prefetchPath, Context contextToUse, ExcludeIncludeFieldsList excludedIncludedFields);
        //
        // Summary:
        //     Executes the passed in retrieval query and projects the resultset using the value
        //     projectors and the projector specified. IF a transaction is in progress, the
        //     command is wired to the transaction and executed inside the transaction. The
        //     projection results will be stored in the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   queryToExecute:
        //     The query to execute.
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IRetrievalQuery queryToExecute);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in, executes that retrieval query
        //     and projects the resultset of that query using the value projectors and the projector
        //     specified. If a transaction is in progress, the command is wired to the transaction
        //     and executed inside the transaction. The projection results will be stored in
        //     the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   parameters:
        //     The parameters.
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, QueryParameters parameters);
        //
        // Summary:
        //     Projects the current resultset of the passed in datareader using the value projectors
        //     and the projector specified. The reader will be left open
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   reader:
        //     The reader which points to the first row of a resultset
        //
        //   executedQuery:
        //     the query object executed which produced the reader. Pass the executed query
        //     object to make sure resultset caching is possible.
        //
        // Remarks:
        //     Use this overload together with FetchDataReader if your datareader contains multiple
        //     resultsets, so you have fine-grained control over how you want to project which
        //     resultset in the datareader. Resultset caching will occur if the passed in executedQuery
        //     is setup to cache its resultset.
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IDataReader reader, IRetrievalQuery executedQuery);
        //
        // Summary:
        //     Projects the current resultset of the passed in datareader using the value projectors
        //     and the projector specified. The reader will be left open
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   reader:
        //     The reader which points to the first row of a resultset
        //
        // Remarks:
        //     Use this overload together with FetchDataReader if your datareader contains multiple
        //     resultsets, so you have fine-grained control over how you want to project which
        //     resultset in the datareader. The resultset won't be cached in the resultset cache.
        //     To cache the resultset, use the overload which accepts the IRetrievalQuery executed
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IDataReader reader);
        //
        // Summary:
        //     Projects the current resultset of the passed in datareader using the value projectors
        //     and the projector specified. The reader will be left open
        //
        // Parameters:
        //   reader:
        //     The open reader to project the active resultset of
        //
        //   queryExecuted:
        //     the query object executed which produced the reader. Pass the executed query
        //     object to make sure resultset caching is possible.
        //
        // Type parameters:
        //   T:
        //     Type of the return elements, one for each row
        //
        // Returns:
        //     List of instances of T, one for each row in the resultset of reader
        //
        // Remarks:
        //     Use this overload together with FetchDataReader if your datareader contains multiple
        //     resultsets, so you have fine-grained control over how you want to project which
        //     resultset in the datareader. Resultset caching will occur if the passed in executedQuery
        //     is setup to cache its resultset.
        List<T> FetchProjection<T>(IDataReader reader, IRetrievalQuery queryExecuted);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in, executes that retrieval query
        //     and projects the resultset of that query using the value projectors and the projector
        //     specified. If a transaction is in progress, the command is wired to the transaction
        //     and executed inside the transaction. The projection results will be stored in
        //     the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   sortClauses:
        //     The sort clauses.
        //
        //   groupByClause:
        //     The group by clause.
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        //
        //   pageNumber:
        //     The page number.
        //
        //   pageSize:
        //     Size of the page.
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, IGroupByCollection groupByClause, bool allowDuplicates, int pageNumber, int pageSize);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in, executes that retrieval query
        //     and projects the resultset of that query using the value projectors and the projector
        //     specified. If a transaction is in progress, the command is wired to the transaction
        //     and executed inside the transaction. The projection results will be stored in
        //     the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   sortClauses:
        //     The sort clauses.
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);
        //
        // Summary:
        //     Executes the passed in retrieval query and projects the resultset onto instances
        //     of T (each row is materialized into an instance of T). If a transaction is in
        //     progress, the command is wired to the transaction and executed inside the transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     The query to execute.
        //
        // Type parameters:
        //   T:
        //     Type of the return elements, one for each row
        //
        // Returns:
        //     List of instances of T, one for each row in the resultset of queryToExecute
        List<T> FetchProjection<T>(IRetrievalQuery queryToExecute);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in, executes that retrieval query
        //     and projects the resultset of that query using the value projectors and the projector
        //     specified. If a transaction is in progress, the command is wired to the transaction
        //     and executed inside the transaction. The projection results will be stored in
        //     the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, bool allowDuplicates);
        //
        // Summary:
        //     Creates a new Retrieval query from the elements passed in, executes that retrieval query
        //     and projects the resultset of that query using the value projectors and the projector
        //     specified. If a transaction is in progress, the command is wired to the transaction
        //     and executed inside the transaction. The projection results will be stored in
        //     the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   fields:
        //     The fields to use to build the query.
        //
        //   filter:
        //     The filter.
        //
        //   maxNumberOfItemsToReturn:
        //     The max number of items to return. Specify 0 to return all elements
        //
        //   sortClauses:
        //     The sort clauses.
        //
        //   allowDuplicates:
        //     If set to true, allow duplicates in the resultset, otherwise it will emit DISTINCT
        //     into the query (if possible).
        //
        //   pageNumber:
        //     The page number.
        //
        //   pageSize:
        //     Size of the page.
        void FetchProjection(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IEntityFields2 fields, IRelationPredicateBucket filter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, int pageNumber, int pageSize);
        //
        // Summary:
        //     Projects the current resultset of the passed in datareader using the value projectors
        //     and the projector specified. The reader will be left open
        //
        // Parameters:
        //   reader:
        //     The open reader to project the active resultset of
        //
        // Type parameters:
        //   T:
        //     Type of the return elements, one for each row
        //
        // Returns:
        //     List of instances of T, one for each row in the resultset of reader
        //
        // Remarks:
        //     Use this overload together with FetchDataReader if your datareader contains multiple
        //     resultsets, so you have fine-grained control over how you want to project which
        //     resultset in the datareader. Resultset caching will not occur. To use resultset
        //     caching, use the overload which accepts an IRetrievalQuery
        List<T> FetchProjection<T>(IDataReader reader);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection(System.Collections.Generic.List{SD.LLBLGen.Pro.ORMSupportClasses.IDataValueProjector},SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector,SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        //     Executes the passed in retrieval query and projects the resultset using the value
        //     projectors and the projector specified. IF a transaction is in progress, the
        //     command is wired to the transaction and executed inside the transaction. The
        //     projection results will be stored in the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   queryToExecute:
        //     The query to execute.
        //
        //   cancellationToken:
        //     The cancellation token.
        Task FetchProjectionAsync(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, IRetrievalQuery queryToExecute, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection(System.Collections.Generic.List{SD.LLBLGen.Pro.ORMSupportClasses.IDataValueProjector},SD.LLBLGen.Pro.ORMSupportClasses.IGeneralDataProjector,SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        //     Creates a new Retrieval query from the elements passed in, executes that retrieval query
        //     and projects the resultset of that query using the value projectors and the projector
        //     specified. If a transaction is in progress, the command is wired to the transaction
        //     and executed inside the transaction. The projection results will be stored in
        //     the projector.
        //
        // Parameters:
        //   valueProjectors:
        //     The value projectors.
        //
        //   projector:
        //     The projector to use for projecting a plain row onto a new object provided by
        //     the projector.
        //
        //   parameters:
        //     The parameters.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     parameters
        Task FetchProjectionAsync(List<IDataValueProjector> valueProjectors, IGeneralDataProjector projector, QueryParameters parameters, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection``1(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        //     Executes the passed in retrieval query and projects the resultset onto instances
        //     of T (each row is materialized into an instance of T). If a transaction is in
        //     progress, the command is wired to the transaction and executed inside the transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     The query to execute.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     List of instances of T, one for each row in the resultset of queryToExecute
        Task<List<T>> FetchProjectionAsync<T>(IRetrievalQuery queryToExecute, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchProjection``1(SD.LLBLGen.Pro.ORMSupportClasses.IRetrievalQuery)
        //     Executes the passed in retrieval query and projects the resultset onto instances
        //     of T (each row is materialized into an instance of T). If a transaction is in
        //     progress, the command is wired to the transaction and executed inside the transaction.
        //
        // Parameters:
        //   queryToExecute:
        //     The query to execute.
        //
        // Type parameters:
        //   T:
        //
        // Returns:
        //     List of instances of T, one for each row in the resultset of queryToExecute
        Task<List<T>> FetchProjectionAsync<T>(IRetrievalQuery queryToExecute);
        //
        // Summary:
        //     Executes the specified plain SQL query using this adapter and projects each row
        //     in the resultset to an instance of T. Every parameter value is converted into
        //     one or more parameters which have to be pre-defined in the sqlQuery. Uses default
        //     fetch aspects.
        //
        // Parameters:
        //   sqlQuery:
        //     The SQL query to execute, which returns a resultset. Should contain parameter
        //     names for the parameter values, or placeholders for parameter sets. See documentation
        //     for details regarding format specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Type parameters:
        //   T:
        //     The type of the element to project each row to.
        //
        // Returns:
        //     A list with 0 or more instances of T, one for each row in the resultset obtained
        //     from executing the query constructed from sqlQuery and the specified parameters
        List<T> FetchQuery<T>(string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Executes the specified plain SQL query using this adapter and projects each row
        //     in the resultset to an instance of T. Every parameter value is converted into
        //     one or more parameters which have to be pre-defined in the sqlQuery.
        //
        // Parameters:
        //   fetchAspects:
        //     The fetch aspects for this query. Can be null, in which case the defaults are
        //     used.
        //
        //   sqlQuery:
        //     The SQL query to execute, which returns a resultset. Should contain parameter
        //     names for the parameter values, or placeholders for parameter sets. See documentation
        //     for details regarding format specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Type parameters:
        //   T:
        //     The type of the element to project each row to.
        //
        // Returns:
        //     A list with 0 or more instances of T, one for each row in the resultset obtained
        //     from executing the query constructed from sqlQuery and the specified parameters
        List<T> FetchQuery<T>(PlainSQLFetchAspects fetchAspects, string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.PlainSQLFetchAspects,System.String,System.Object)
        //     Executes the specified plain SQL query using this adapter and projects each row
        //     in the resultset to an instance of T. Every parameter value is converted into
        //     one or more parameters which have to be pre-defined in the sqlQuery.
        //
        // Parameters:
        //   cancellationToken:
        //     The cancellation token.
        //
        //   fetchAspects:
        //     The fetch aspects for this query. Can be null, in which case the defaults are
        //     used.
        //
        //   sqlQuery:
        //     The SQL query to execute, which returns a resultset. Should contain parameter
        //     names for the parameter values, or placeholders for parameter sets. See documentation
        //     for details regarding format specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Type parameters:
        //   T:
        //     The type of the element to project each row to.
        //
        // Returns:
        //     A list with 0 or more instances of T, one for each row in the resultset obtained
        //     from executing the query constructed from sqlQuery and the specified parameters
        Task<List<T>> FetchQueryAsync<T>(CancellationToken cancellationToken, PlainSQLFetchAspects fetchAspects, string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(SD.LLBLGen.Pro.ORMSupportClasses.PlainSQLFetchAspects,System.String,System.Object)
        //     Executes the specified plain SQL query using this adapter and projects each row
        //     in the resultset to an instance of T. Every parameter value is converted into
        //     one or more parameters which have to be pre-defined in the sqlQuery.
        //
        // Parameters:
        //   fetchAspects:
        //     The fetch aspects for this query. Can be null, in which case the defaults are
        //     used.
        //
        //   sqlQuery:
        //     The SQL query to execute, which returns a resultset. Should contain parameter
        //     names for the parameter values, or placeholders for parameter sets. See documentation
        //     for details regarding format specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Type parameters:
        //   T:
        //     The type of the element to project each row to.
        //
        // Returns:
        //     A list with 0 or more instances of T, one for each row in the resultset obtained
        //     from executing the query constructed from sqlQuery and the specified parameters
        Task<List<T>> FetchQueryAsync<T>(PlainSQLFetchAspects fetchAspects, string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(System.String,System.Object)
        //     Executes the specified plain SQL query using this adapter and projects each row
        //     in the resultset to an instance of T. Every parameter value is converted into
        //     one or more parameters which have to be pre-defined in the sqlQuery. Uses default
        //     fetch aspects.
        //
        // Parameters:
        //   cancellationToken:
        //     The cancellation token.
        //
        //   sqlQuery:
        //     The SQL query to execute, which returns a resultset. Should contain parameter
        //     names for the parameter values, or placeholders for parameter sets. See documentation
        //     for details regarding format specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Type parameters:
        //   T:
        //     The type of the element to project each row to.
        //
        // Returns:
        //     A list with 0 or more instances of T, one for each row in the resultset obtained
        //     from executing the query constructed from sqlQuery and the specified parameters
        Task<List<T>> FetchQueryAsync<T>(CancellationToken cancellationToken, string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchQuery``1(System.String,System.Object)
        //     Executes the specified plain SQL query using this adapter and projects each row
        //     in the resultset to an instance of T. Every parameter value is converted into
        //     one or more parameters which have to be pre-defined in the sqlQuery. Uses default
        //     fetch aspects.
        //
        // Parameters:
        //   sqlQuery:
        //     The SQL query to execute, which returns a resultset. Should contain parameter
        //     names for the parameter values, or placeholders for parameter sets. See documentation
        //     for details regarding format specifics.
        //
        //   parameterValues:
        //     The object containing the parameter values to use in the query. If it's an object
        //     array, parameters using ordering are assumed, otherwise for each public, non-static
        //     property, a parameter is created.
        //
        // Type parameters:
        //   T:
        //     The type of the element to project each row to.
        //
        // Returns:
        //     A list with 0 or more instances of T, one for each row in the resultset obtained
        //     from executing the query constructed from sqlQuery and the specified parameters
        Task<List<T>> FetchQueryAsync<T>(string sqlQuery, object parameterValues = null);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. For TypedView
        //     filling, use the method FetchTypedView()
        //
        // Parameters:
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   parameters:
        //     The parameters.
        void FetchTypedList(DataTable dataTableToFill, QueryParameters parameters);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in the passed in typed list.
        //     For TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   typedListToFill:
        //     Typed list to fill.
        //
        // Remarks:
        //     Grabs the fields list and relations set from the typed list passed in.
        void FetchTypedList(ITypedListLgp2 typedListToFill);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in the passed in typed list.
        //     For TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   typedListToFill:
        //     Typed list to fill.
        //
        //   additionalFilter:
        //     An additional filter to use to filter the fetch of the typed list. If you need
        //     a more sophisticated filter approach, please use the overload which accepts an
        //     IRelationalPredicateBucket and add your filter to the bucket you receive when
        //     calling typedListToFill.GetRelationInfo().
        //
        // Remarks:
        //     Grabs the fields list and relations set from the typed list passed in.
        void FetchTypedList(ITypedListLgp2 typedListToFill, IPredicateExpression additionalFilter);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in the passed in typed list.
        //     For TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   typedListToFill:
        //     Typed list to fill.
        //
        //   additionalFilter:
        //     An additional filter to use to filter the fetch of the typed list. If you need
        //     a more sophisticated filter approach, please use the overload which accepts an
        //     IRelationalPredicateBucket and add your filter to the bucket you receive when
        //     calling typedListToFill.GetRelationInfo().
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        // Remarks:
        //     Grabs the fields list and relations set from the typed list passed in.
        void FetchTypedList(ITypedListLgp2 typedListToFill, IPredicateExpression additionalFilter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. For TypedView
        //     filling, use the method FetchTypedView()
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields to fetch into the datatable.
        //     The fields inside this object are used to construct the selection resultset.
        //     Use the typed list's method GetFieldsInfo() to retrieve this IEntityField2 information
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        //   groupByClause:
        //     GroupByCollection with fields to group by on
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. For TypedView
        //     filling, use the method FetchTypedView()
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields to fetch into the datatable.
        //     The fields inside this object are used to construct the selection resultset.
        //     Use the typed list's method GetFieldsInfo() to retrieve this IEntityField2 information
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. Doesn't apply
        //     any sorting. For TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields to fetch into the datatable.
        //     The fields inside this object are used to construct the selection resultset.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. Doesn't apply
        //     any sorting, doesn't limit the resultset on the amount of rows to return. For
        //     TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields to fetch into the datatable.
        //     The fields inside this object are used to construct the selection resultset.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. Doesn't apply
        //     any sorting, doesn't limit the resultset on the amount of rows to return, does
        //     allow duplicates. For TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields to fetch into the datatable.
        //     The fields inside this object are used to construct the selection resultset.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. For TypedView
        //     filling, use the method FetchTypedView()
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields to fetch into the datatable.
        //     The fields inside this object are used to construct the selection resultset.
        //     Use the typed list's method GetFieldsInfo() to retrieve this IEntityField2 information
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        //   groupByClause:
        //     GroupByCollection with fields to group by on
        //
        //   pageNumber:
        //     the page number to retrieve. First page is 1. When set to 0, no paging logic
        //     is applied
        //
        //   pageSize:
        //     the size of the page. When set to 0, no paging logic is applied
        void FetchTypedList(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause, int pageNumber, int pageSize);
        //
        // Summary:
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in the passed in typed list.
        //     For TypedView filling, use the method FetchTypedView()
        //
        // Parameters:
        //   typedListToFill:
        //     Typed list to fill.
        //
        //   additionalFilter:
        //     An additional filter to use to filter the fetch of the typed list. If you need
        //     a more sophisticated filter approach, please use the overload which accepts an
        //     IRelationalPredicateBucket and add your filter to the bucket you receive when
        //     calling typedListToFill.GetRelationInfo().
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        //   pageNumber:
        //     the page number to retrieve. First page is 1. When set to 0, no paging logic
        //     is applied
        //
        //   pageSize:
        //     the size of the page. When set to 0, no paging logic is applied
        //
        // Remarks:
        //     Grabs the fields list and relations set from the typed list passed in.
        void FetchTypedList(ITypedListLgp2 typedListToFill, IPredicateExpression additionalFilter, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, int pageNumber, int pageSize);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.FetchTypedList(System.Data.DataTable,SD.LLBLGen.Pro.ORMSupportClasses.QueryParameters).
        //     Fetches the fields passed in fieldCollectionToFetch from the persistent storage
        //     using the relations and filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a typed list object. For TypedView
        //     filling, use the method FetchTypedView()
        //
        // Parameters:
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   parameters:
        //     The parameters.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     parameters
        Task FetchTypedListAsync(DataTable dataTableToFill, QueryParameters parameters, CancellationToken cancellationToken);
        //
        // Summary:
        //     Fetches the Typed View passed in from the persistent storage Doesn't apply any
        //     sorting. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   typedViewToFill:
        //     Typed view to fill.
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage into the DataTable object passed in. Doesn't apply any sorting, doesn't
        //     limit the amount of rows returned by the query, doesn't apply any filtering,
        //     allows duplicate rows. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill);
        //
        // Summary:
        //     Fetches the Typed View passed in from the persistent storage Doesn't apply any
        //     sorting, doesn't limit the amount of rows returned by the query, doesn't apply
        //     any filtering. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   typedViewToFill:
        //     Typed view to fill.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedView(ITypedView2 typedViewToFill, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage using the filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable. Use the Typed View's method GetFieldsInfo() to get this IEntityField2
        //     field collection
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        // Remarks:
        //     To fill a DataTable with entity rows, use this method as well, by passing the
        //     Fields collection of an entity of the same type as the one you want to fetch
        //     as fieldCollectionToFetch.
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View passed in from the persistent storage
        //
        // Parameters:
        //   typedViewToFill:
        //     Typed view to fill.
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        //   groupByClause:
        //     GroupByCollection with fields to group by on
        //
        // Remarks:
        //     To fill a DataTable with entity rows, use this method as well, by passing the
        //     Fields collection of an entity of the same type as the one you want to fetch
        //     as fieldCollectionToFetch.
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Fetches the Typed View passed in from the persistent storage
        //
        // Parameters:
        //   typedViewToFill:
        //     Typed view to fill.
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        // Remarks:
        //     To fill a DataTable with entity rows, use this method as well, by passing the
        //     Fields collection of an entity of the same type as the one you want to fetch
        //     as fieldCollectionToFetch.
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage using the filter information stored in filterBucket into the DataTable
        //     object passed in. Doesn't apply any sorting, doesn't limit the amount of rows
        //     returned by the query. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View passed in from the persistent storage Doesn't apply any
        //     sorting, doesn't limit the amount of rows returned by the query, doesn't apply
        //     any filtering, allows duplicate rows. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   typedViewToFill:
        //     Typed view to fill.
        void FetchTypedView(ITypedView2 typedViewToFill);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage using the query information stored in parameters into the DataTable object
        //     passed in. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   dataTableToFill:
        //     The data table to fill.
        //
        //   parameters:
        //     The parameters.
        void FetchTypedView(DataTable dataTableToFill, QueryParameters parameters);
        //
        // Summary:
        //     Fetches the typed view, using the query specified.
        //
        // Parameters:
        //   typedViewToFill:
        //     The typed view to fill.
        //
        //   queryToUse:
        //     The query to use.
        //
        // Remarks:
        //     Used with stored procedure calling IRetrievalQuery instances to fill a typed
        //     view mapped onto a resultset. Be sure to call Dispose() on the passed in query,
        //     as it's not disposed in this method.
        void FetchTypedView(ITypedView2 typedViewToFill, IRetrievalQuery queryToUse);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage into the DataTable object passed in. Doesn't apply any sorting, doesn't
        //     limit the amount of rows returned by the query, doesn't apply any filtering.
        //     Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View passed in from the persistent storage Doesn't apply any
        //     sorting, doesn't limit the amount of rows returned by the query. Use this routine
        //     to fill a TypedView object.
        //
        // Parameters:
        //   typedViewToFill:
        //     Typed view to fill.
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedView(ITypedView2 typedViewToFill, IRelationPredicateBucket filterBucket, bool allowDuplicates);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage using the filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable. Use the Typed View's method GetFieldsInfo() to get this IEntityField2
        //     field collection
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        //   groupByClause:
        //     GroupByCollection with fields to group by on
        //
        // Remarks:
        //     To fill a DataTable with entity rows, use this method as well, by passing the
        //     Fields collection of an entity of the same type as the one you want to fetch
        //     as fieldCollectionToFetch.
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage using the filter information stored in filterBucket into the DataTable
        //     object passed in. Use this routine to fill a TypedView object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable. Use the Typed View's method GetFieldsInfo() to get this IEntityField2
        //     field collection
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   sortClauses:
        //     SortClause expression which is applied to the query executed, sorting the fetch
        //     result.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        //   groupByClause:
        //     GroupByCollection with fields to group by on
        //
        //   pageNumber:
        //     the page number to retrieve. First page is 1. When set to 0, no paging logic
        //     is applied
        //
        //   pageSize:
        //     the size of the page. When set to 0, no paging logic is applied
        //
        // Remarks:
        //     To fill a DataTable with entity rows, use this method as well, by passing the
        //     Fields collection of an entity of the same type as the one you want to fetch
        //     as fieldCollectionToFetch.
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, ISortExpression sortClauses, bool allowDuplicates, IGroupByCollection groupByClause, int pageNumber, int pageSize);
        //
        // Summary:
        //     Fetches the Typed View fields passed in fieldCollectionToFetch from the persistent
        //     storage using the filter information stored in filterBucket into the DataTable
        //     object passed in. Doesn't apply any sorting Use this routine to fill a TypedView
        //     object.
        //
        // Parameters:
        //   fieldCollectionToFetch:
        //     IEntityField2 collection which contains the fields of the typed view to fetch
        //     into the datatable.
        //
        //   dataTableToFill:
        //     The datatable object to fill with the data for the fields in fieldCollectionToFetch
        //
        //   filterBucket:
        //     filter information (relations and predicate expressions) for retrieving the data.
        //     Use the TypedList's method GetRelationInfo() to retrieve this bucket.
        //
        //   maxNumberOfItemsToReturn:
        //     The maximum amount of rows to return. If 0, all rows matching the filter are
        //     returned
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        void FetchTypedView(IEntityFields2 fieldCollectionToFetch, DataTable dataTableToFill, IRelationPredicateBucket filterBucket, int maxNumberOfItemsToReturn, bool allowDuplicates);
        //
        // Summary:
        //     Gets the estimated number of objects returned by a query for objects to store
        //     in the entity collection passed in, using the filter and groupby clause specified.
        //     The number is estimated as duplicate objects can be present in the plain query
        //     results, but will be filtered out when the query result is transformed into objects.
        //
        // Parameters:
        //   collection:
        //     EntityCollection instance which will be fetched by the query to get the rowcount
        //     for
        //
        //   filter:
        //     filter to use by the query to get the rowcount for
        //
        //   groupByClause:
        //     The list of fields to group by on. When not specified or an empty collection
        //     is specified, no group by clause is added to the query. A check is performed
        //     for each field in the selectList. If a field in the selectList is not present
        //     in the groupByClause collection, an exception is thrown.
        //
        // Returns:
        //     the number of rows the query for the fields specified, using the filter, relations
        //     and groupbyClause specified.
        //
        // Remarks:
        //     This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        //     as a scalar query. This construct is not supported on Firebird. You can try to
        //     achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        //     those results can differ from the result returned by GetDbCount if you use a
        //     group by clause.
        int GetDbCount(IEntityCollection2 collection, IRelationPredicateBucket filter, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Gets the estimated number of objects returned by a query for objects to store
        //     in the entity collection passed in, using the filter and groupby clause specified.
        //     The number is estimated as duplicate objects can be present in the plain query
        //     results, but will be filtered out when the query result is transformed into objects.
        //
        // Parameters:
        //   collection:
        //     EntityCollection instance which will be fetched by the query to get the rowcount
        //     for
        //
        //   filter:
        //     filter to use by the query to get the rowcount for
        //
        // Returns:
        //     the number of rows the query for the fields specified, using the filter, relations
        //     and groupbyClause specified.
        //
        // Remarks:
        //     This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        //     as a scalar query. This construct is not supported on Firebird. You can try to
        //     achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        //     those results can differ from the result returned by GetDbCount if you use a
        //     group by clause.
        int GetDbCount(IEntityCollection2 collection, IRelationPredicateBucket filter);
        //
        // Summary:
        //     Gets the number of rows returned by a query for the fields specified, using the
        //     filter and groupby clause specified.
        //
        // Parameters:
        //   fields:
        //     IEntityFields2 instance with the fields returned by the query to get the rowcount
        //     for
        //
        //   filter:
        //     filter to use by the query to get the rowcount for
        //
        //   groupByClause:
        //     The list of fields to group by on. When not specified or an empty collection
        //     is specified, no group by clause is added to the query. A check is performed
        //     for each field in the selectList. If a field in the selectList is not present
        //     in the groupByClause collection, an exception is thrown.
        //
        //   allowDuplicates:
        //     When true, it will not filter out duplicate rows, otherwise it will DISTINCT
        //     duplicate rows.
        //
        // Returns:
        //     the number of rows the query for the fields specified, using the filter, relations
        //     and groupbyClause specified.
        //
        // Remarks:
        //     This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        //     as a scalar query. This construct is not supported on Firebird. You can try to
        //     achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        //     those results can differ from the result returned by GetDbCount if you use a
        //     group by clause.
        int GetDbCount(IEntityFields2 fields, IRelationPredicateBucket filter, IGroupByCollection groupByClause, bool allowDuplicates);
        //
        // Summary:
        //     Gets the number of rows returned by a query for the fields specified, using the
        //     filter and groupby clause specified.
        //
        // Parameters:
        //   fields:
        //     IEntityFields2 instance with the fields returned by the query to get the rowcount
        //     for
        //
        //   filter:
        //     filter to use by the query to get the rowcount for
        //
        //   groupByClause:
        //     The list of fields to group by on. When not specified or an empty collection
        //     is specified, no group by clause is added to the query. A check is performed
        //     for each field in the selectList. If a field in the selectList is not present
        //     in the groupByClause collection, an exception is thrown.
        //
        // Returns:
        //     the number of rows the query for the fields specified, using the filter, relations
        //     and groupbyClause specified.
        //
        // Remarks:
        //     This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        //     as a scalar query. This construct is not supported on Firebird. You can try to
        //     achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        //     those results can differ from the result returned by GetDbCount if you use a
        //     group by clause.
        int GetDbCount(IEntityFields2 fields, IRelationPredicateBucket filter, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Gets the number of rows returned by a query for the fields specified, using the
        //     filter and groupby clause specified.
        //
        // Parameters:
        //   fields:
        //     IEntityFields2 instance with the fields returned by the query to get the rowcount
        //     for
        //
        //   filter:
        //     filter to use by the query to get the rowcount for
        //
        // Returns:
        //     the number of rows the query for the fields specified, using the filter, relations
        //     and groupbyClause specified.
        //
        // Remarks:
        //     This method performs a SELECT COUNT(*) FROM (actual query) and executes that
        //     as a scalar query. This construct is not supported on Firebird. You can try to
        //     achieve the same results by using GetScalar and AggregateFunction.CountRow, though
        //     those results can differ from the result returned by GetDbCount if you use a
        //     group by clause.
        int GetDbCount(IEntityFields2 fields, IRelationPredicateBucket filter);
        //
        // Summary:
        //     Executes the expression defined with the field in the fields collection specified,
        //     using the various elements defined. The expression is executed as a scalar query
        //     and a single value is returned.
        //
        // Parameters:
        //   fields:
        //     IEntityFields2 instance with a single field with an expression defined and eventually
        //     aggregates
        //
        //   filter:
        //     filter to use
        //
        //   groupByClause:
        //     The list of fields to group by on. When not specified or an empty collection
        //     is specified, no group by clause is added to the query. A check is performed
        //     for each field in the selectList. If a field in the selectList is not present
        //     in the groupByClause collection, an exception is thrown.
        //
        //   relations:
        //     The relations part of the filter.
        //
        // Returns:
        //     the value which is the result of the expression defined on the specified field
        object GetScalar(IEntityFields2 fields, IPredicate filter, IGroupByCollection groupByClause, IRelationCollection relations);
        //
        // Summary:
        //     Gets a scalar value, calculated with the aggregate and expression specified.
        //     the field specified is the field the expression and aggregate are applied on.
        //
        // Parameters:
        //   field:
        //     Field to which to apply the aggregate function and expression
        //
        //   aggregateToApply:
        //     Aggregate function to apply.
        //
        // Returns:
        //     the scalar value requested
        object GetScalar(IEntityField2 field, AggregateFunction aggregateToApply);
        //
        // Summary:
        //     Gets a scalar value, calculated with the aggregate and expression specified.
        //     the field specified is the field the expression and aggregate are applied on.
        //
        // Parameters:
        //   field:
        //     Field to which to apply the aggregate function and expression
        //
        //   expressionToExecute:
        //     The expression to execute. Can be null
        //
        //   aggregateToApply:
        //     Aggregate function to apply.
        //
        //   filter:
        //     The filter to apply to retrieve the scalar
        //
        //   groupByClause:
        //     The groupby clause to apply to retrieve the scalar
        //
        //   relations:
        //     The relations part of the filter.
        //
        // Returns:
        //     the scalar value requested
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter, IGroupByCollection groupByClause, IRelationCollection relations);
        //
        // Summary:
        //     Gets a scalar value, calculated with the aggregate and expression specified.
        //     the field specified is the field the expression and aggregate are applied on.
        //
        // Parameters:
        //   field:
        //     Field to which to apply the aggregate function and expression
        //
        //   expressionToExecute:
        //     The expression to execute. Can be null
        //
        //   aggregateToApply:
        //     Aggregate function to apply.
        //
        //   filter:
        //     The filter to apply to retrieve the scalar
        //
        // Returns:
        //     the scalar value requested
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter);
        //
        // Summary:
        //     Executes the expression defined with the field in the fields collection specified,
        //     using the various elements defined. The expression is executed as a scalar query
        //     and a single value is returned.
        //
        // Parameters:
        //   fields:
        //     IEntityFields2 instance with a single field with an expression defined and eventually
        //     aggregates
        //
        //   filter:
        //     filter to use
        //
        //   groupByClause:
        //     The list of fields to group by on. When not specified or an empty collection
        //     is specified, no group by clause is added to the query. A check is performed
        //     for each field in the selectList. If a field in the selectList is not present
        //     in the groupByClause collection, an exception is thrown.
        //
        // Returns:
        //     the value which is the result of the expression defined on the specified field
        object GetScalar(IEntityFields2 fields, IPredicate filter, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Gets a scalar value, calculated with the aggregate and expression specified.
        //     the field specified is the field the expression and aggregate are applied on.
        //
        // Parameters:
        //   field:
        //     Field to which to apply the aggregate function and expression
        //
        //   expressionToExecute:
        //     The expression to execute. Can be null
        //
        //   aggregateToApply:
        //     Aggregate function to apply.
        //
        // Returns:
        //     the scalar value requested
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply);
        //
        // Summary:
        //     Gets a scalar value, calculated with the aggregate and expression specified.
        //     the field specified is the field the expression and aggregate are applied on.
        //
        // Parameters:
        //   field:
        //     Field to which to apply the aggregate function and expression
        //
        //   expressionToExecute:
        //     The expression to execute. Can be null
        //
        //   aggregateToApply:
        //     Aggregate function to apply.
        //
        //   filter:
        //     The filter to apply to retrieve the scalar
        //
        //   groupByClause:
        //     The groupby clause to apply to retrieve the scalar
        //
        // Returns:
        //     the scalar value requested
        object GetScalar(IEntityField2 field, IExpression expressionToExecute, AggregateFunction aggregateToApply, IPredicate filter, IGroupByCollection groupByClause);
        //
        // Summary:
        //     Opens the active connection object. If the connection is already open, nothing
        //     is done. If no connection object is present, a new one is created
        void OpenConnection();
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.OpenConnection.
        //     Opens the active connection object. If the connection is already open, nothing
        //     is done. If no connection object is present, a new one is created
        //
        // Parameters:
        //   cancellationToken:
        //     The cancellation token.
        //
        // Exceptions:
        //   T:System.ObjectDisposedException:
        //     DataAccessAdapterBase;This DataAccessAdapter instance has already been disposed,
        //     you can't use it for further persistence activity
        Task OpenConnectionAsync(CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.OpenConnection.
        //     Opens the active connection object. If the connection is already open, nothing
        //     is done. If no connection object is present, a new one is created
        //
        // Exceptions:
        //   T:System.ObjectDisposedException:
        //     DataAccessAdapterBase;This DataAccessAdapter instance has already been disposed,
        //     you can't use it for further persistence activity
        Task OpenConnectionAsync();
        //
        // Summary:
        //     Rolls back the transaction in action to the savepoint with the name savepointName.
        //     No internal objects are being reset when this method is called, so call this
        //     Rollback overload only to roll back to a savepoint. To roll back a complete transaction,
        //     call Rollback() without specifying a savepoint name. Create a savepoint by calling
        //     SaveTransaction(savePointName)
        //
        // Parameters:
        //   savePointName:
        //     name of the savepoint to roll back to.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     If no transaction is in progress.
        //
        //   T:System.ArgumentException:
        //     if savePointName is empty or null
        //
        //   T:System.NotSupportedException:
        //     if the .NET database provider doesn't support transaction rolling back a transaction
        //     to a named point or when COM+ is used.
        void Rollback(string savePointName);
        //
        // Summary:
        //     Saves the passed in entity to the persistent storage. Will not refetch the entity
        //     after this save. The entity will stay out-of-sync. If the entity is new, it will
        //     be inserted, if the entity is existent, the changed entity fields will be changed
        //     in the database. Will do a recursive save.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        bool SaveEntity(IEntity2 entityToSave);
        //
        // Summary:
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will do a recursive save. Will pass the concurrency
        //     predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave);
        //
        // Summary:
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will do a recursive save. Will pass the concurrency
        //     predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   updateRestriction:
        //     Predicate expression, meant for concurrency checks in an Update query. Will be
        //     ignored when the entity is new.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction);
        //
        // Summary:
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   updateRestriction:
        //     Predicate expression, meant for concurrency checks in an Update query. Will be
        //     ignored when the entity is new.
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by entityToSave also.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse);
        //
        // Summary:
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by entityToSave also.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        bool SaveEntity(IEntity2 entityToSave, bool refetchAfterSave, bool recurse);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean).
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will do a recursive save. Will pass the concurrency
        //     predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean,System.Boolean).
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by entityToSave also.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, bool recurse, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean,SD.LLBLGen.Pro.ORMSupportClasses.IPredicateExpression,System.Boolean).
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   updateRestriction:
        //     Predicate expression, meant for concurrency checks in an Update query. Will be
        //     ignored if the entity is new. This predicate is used instead of a predicate produced
        //     by a set ConcurrencyPredicateFactory.
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by entityToSave also.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, IPredicateExpression updateRestriction, bool recurse, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        //     Saves the passed in entity to the persistent storage. Will not refetch the entity
        //     after this save. The entity will stay out-of-sync. If the entity is new, it will
        //     be inserted, if the entity is existent, the changed entity fields will be changed
        //     in the database. Will do a recursive save. Will pass the concurrency predicate
        //     returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save) as update
        //     restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean).
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will do a recursive save. Will pass the concurrency
        //     predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2).
        //     Saves the passed in entity to the persistent storage. Will not refetch the entity
        //     after this save. The entity will stay out-of-sync. If the entity is new, it will
        //     be inserted, if the entity is existent, the changed entity fields will be changed
        //     in the database. Will do a recursive save. Will pass the concurrency predicate
        //     returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save) as update
        //     restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntity(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,System.Boolean,System.Boolean).
        //     Saves the passed in entity to the persistent storage. If the entity is new, it
        //     will be inserted, if the entity is existent, the changed entity fields will be
        //     changed in the database. Will pass the concurrency predicate returned by GetConcurrencyPredicate(ConcurrencyPredicateType.Save)
        //     as update restriction.
        //
        // Parameters:
        //   entityToSave:
        //     The entity to save
        //
        //   refetchAfterSave:
        //     When true, it will refetch the entity from the persistent storage so it will
        //     be up-to-date after the save action.
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by entityToSave also.
        //
        // Returns:
        //     true if the save was successful, false otherwise.
        //
        // Remarks:
        //     Will use a current transaction if a transaction is in progress
        Task<bool> SaveEntityAsync(IEntity2 entityToSave, bool refetchAfterSave, bool recurse);
        //
        // Summary:
        //     Saves all dirty objects inside the collection passed to the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available. Will
        //     not refetch saved entities and will not recursively save the entities.
        //
        // Parameters:
        //   collectionToSave:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        // Returns:
        //     the amount of persisted entities
        int SaveEntityCollection(IEntityCollection2 collectionToSave);
        //
        // Summary:
        //     Saves all dirty objects inside the collection passed to the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available.
        //
        // Parameters:
        //   collectionToSave:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        //   refetchSavedEntitiesAfterSave:
        //     Refetches a saved entity from the database, so the entity will not be 'out of
        //     sync'
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by the entities inside collectionToSave also.
        //
        // Returns:
        //     the amount of persisted entities
        int SaveEntityCollection(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,System.Boolean,System.Boolean).
        //     Saves all dirty objects inside the collection passed to the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available.
        //
        // Parameters:
        //   collectionToSave:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        //   refetchSavedEntitiesAfterSave:
        //     Refetches a saved entity from the database, so the entity will not be 'out of
        //     sync'
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by the entities inside collectionToSave also.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the amount of persisted entities
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        //     Saves all dirty objects inside the collection passed to the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available. Will
        //     not refetch saved entities and will not recursively save the entities.
        //
        // Parameters:
        //   collectionToSave:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the amount of persisted entities
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2,System.Boolean,System.Boolean).
        //     Saves all dirty objects inside the collection passed to the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available.
        //
        // Parameters:
        //   collectionToSave:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        //   refetchSavedEntitiesAfterSave:
        //     Refetches a saved entity from the database, so the entity will not be 'out of
        //     sync'
        //
        //   recurse:
        //     When true, it will save all dirty objects referenced (directly or indirectly)
        //     by the entities inside collectionToSave also.
        //
        // Returns:
        //     the amount of persisted entities
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave, bool refetchSavedEntitiesAfterSave, bool recurse);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.SaveEntityCollection(SD.LLBLGen.Pro.ORMSupportClasses.IEntityCollection2).
        //     Saves all dirty objects inside the collection passed to the persistent storage.
        //     It will do this inside a transaction if a transaction is not yet available. Will
        //     not refetch saved entities and will not recursively save the entities.
        //
        // Parameters:
        //   collectionToSave:
        //     EntityCollection with one or more dirty entities which have to be persisted
        //
        // Returns:
        //     the amount of persisted entities
        Task<int> SaveEntityCollectionAsync(IEntityCollection2 collectionToSave);
        //
        // Summary:
        //     Creates a savepoint with the name savePointName in the current transaction. You
        //     can roll back to this savepoint using SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.Rollback(System.String).
        //
        // Parameters:
        //   savePointName:
        //     name of savepoint. Must be unique in an active transaction
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     If no transaction is in progress.
        //
        //   T:System.ArgumentException:
        //     if savePointName is empty or null
        //
        //   T:System.NotSupportedException:
        //     if the .NET database provider doesn't support transaction saving or when COM+
        //     is used.
        void SaveTransaction(string savePointName);
        //
        // Summary:
        //     Starts a new transaction. All database activity after this call will be ran in
        //     this transaction and all objects will participate in this transaction until its
        //     committed or rolled back. If there is a transaction in progress, an exception
        //     is thrown. Will create and open a new connection if a transaction is not open
        //     and/or available.
        //
        // Parameters:
        //   isolationLevelToUse:
        //     The isolation level to use for this transaction
        //
        //   name:
        //     The name for this transaction
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     If a transaction is already in progress.
        void StartTransaction(IsolationLevel isolationLevelToUse, string name);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.StartTransaction(System.Data.IsolationLevel,System.String).
        //     Starts a new transaction. All database activity after this call will be ran in
        //     this transaction and all objects will participate in this transaction until its
        //     committed or rolled back. If there is a transaction in progress, an exception
        //     is thrown. Will create and open a new connection if a transaction is not open
        //     and/or available.
        //
        // Parameters:
        //   isolationLevelToUse:
        //     The isolation level to use for this transaction
        //
        //   name:
        //     The name for this transaction
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     If a transaction is already in progress.
        //
        // Remarks:
        //     If this DataAccessAdapter is in a System.Transactions.Transaction, no real ado.net
        //     transaction will be started, as a transaction is already in progress. In that
        //     situation, this method will just open the connection if required.
        Task StartTransactionAsync(IsolationLevel isolationLevelToUse, string name);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.StartTransaction(System.Data.IsolationLevel,System.String).
        //     Starts a new transaction. All database activity after this call will be ran in
        //     this transaction and all objects will participate in this transaction until its
        //     committed or rolled back. If there is a transaction in progress, an exception
        //     is thrown. Will create and open a new connection if a transaction is not open
        //     and/or available.
        //
        // Parameters:
        //   isolationLevelToUse:
        //     The isolation level to use for this transaction
        //
        //   name:
        //     The name for this transaction
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     If a transaction is already in progress.
        //
        // Remarks:
        //     If this DataAccessAdapter is in a System.Transactions.Transaction, no real ado.net
        //     transaction will be started, as a transaction is already in progress. In that
        //     situation, this method will just open the connection if required.
        Task StartTransactionAsync(IsolationLevel isolationLevelToUse, string name, CancellationToken cancellationToken);
        //
        // Summary:
        //     Updates all entities of the same type of the entity entityWithNewValues directly
        //     in the persistent storage if they match the filter supplied in filterBucket.
        //     Only the fields changed in entityWithNewValues are updated for these fields.
        //
        // Parameters:
        //   entityWithNewValues:
        //     Entity object which contains the new values for the entities of the same type
        //     and which match the filter in filterBucket. Only fields which are changed are
        //     updated.
        //
        //   filterBucket:
        //     filter information to filter out the entities to update.
        //
        // Returns:
        //     the amount of physically updated entities
        int UpdateEntitiesDirectly(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.UpdateEntitiesDirectly(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        //     Updates all entities of the same type or subtype of the entity entityWithNewValues
        //     directly in the persistent storage if they match the filter supplied in filterBucket.
        //     Only the fields changed in entityWithNewValues are updated for these fields.
        //     Entities of a subtype of the type of entityWithNewValues which are affected by
        //     the filterBucket's filter will thus also be updated.
        //
        // Parameters:
        //   entityWithNewValues:
        //     Entity object which contains the new values for the entities of the same type
        //     and which match the filter in filterBucket. Only fields which are changed are
        //     updated.
        //
        //   filterBucket:
        //     filter information to filter out the entities to update.
        //
        //   cancellationToken:
        //     The cancellation token.
        //
        // Returns:
        //     the number of physically updated entities. Use this number only to test if the
        //     update succeeded (so value is > 0).
        Task<int> UpdateEntitiesDirectlyAsync(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket, CancellationToken cancellationToken);
        //
        // Summary:
        //     Async variant of SD.LLBLGen.Pro.ORMSupportClasses.IDataAccessAdapter.UpdateEntitiesDirectly(SD.LLBLGen.Pro.ORMSupportClasses.IEntity2,SD.LLBLGen.Pro.ORMSupportClasses.IRelationPredicateBucket).
        //     Updates all entities of the same type or subtype of the entity entityWithNewValues
        //     directly in the persistent storage if they match the filter supplied in filterBucket.
        //     Only the fields changed in entityWithNewValues are updated for these fields.
        //     Entities of a subtype of the type of entityWithNewValues which are affected by
        //     the filterBucket's filter will thus also be updated.
        //
        // Parameters:
        //   entityWithNewValues:
        //     Entity object which contains the new values for the entities of the same type
        //     and which match the filter in filterBucket. Only fields which are changed are
        //     updated.
        //
        //   filterBucket:
        //     filter information to filter out the entities to update.
        //
        // Returns:
        //     the number of physically updated entities. Use this number only to test if the
        //     update succeeded (so value is > 0).
        Task<int> UpdateEntitiesDirectlyAsync(IEntity2 entityWithNewValues, IRelationPredicateBucket filterBucket);
        #endregion

        #region "IDataAccess adapter extension methods"
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
        IDataReader FetchAsDataReader(DynamicQuery query, CommandBehavior behavior);
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
        DataTable FetchAsDataTable(DynamicQuery query);
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
        DataTable FetchAsDataTable(DynamicQuery query, DataTable destination);
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
        void FetchAsProjection(DynamicQuery query, IGeneralDataProjector projector);
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
        TEntity FetchFirst<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;
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
        T FetchFirst<T>(DynamicQuery<T> query);
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
        IEntityCollection2 FetchQuery<TEntity>(EntityQuery<TEntity> query) where TEntity : IEntity2;
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
        TCollection FetchQuery<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill)
            where TEntity : IEntity2
            where TCollection : IEntityCollection2;
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
        List<object[]> FetchQuery(DynamicQuery query);
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
        List<TElement> FetchQuery<TElement>(DynamicQuery<TElement> query);
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
        List<TElement> FetchQueryFromSource<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source);
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
        TValue FetchScalar<TValue>(DynamicQuery query);
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
        TEntity FetchSingle<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;
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
        T FetchSingle<T>(DynamicQuery<T> query);
        #endregion

        #region "IDataAccess adapter async extension methods"
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
        Task<IDataReader> FetchAsDataReaderAsync(DynamicQuery query, CommandBehavior behavior, CancellationToken cancellationToken);
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
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query);
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
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, DataTable destination);
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
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, DataTable destination, CancellationToken cancellationToken);
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
        Task<DataTable> FetchAsDataTableAsync(DynamicQuery query, CancellationToken cancellationToken);
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
        Task FetchAsProjectionAsync(DynamicQuery query, IGeneralDataProjector projector);
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
        Task FetchAsProjectionAsync(DynamicQuery query, IGeneralDataProjector projector, CancellationToken cancellationToken);
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
        Task<T> FetchFirstAsync<T>(DynamicQuery<T> query);
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
        Task<T> FetchFirstAsync<T>(DynamicQuery<T> query, CancellationToken cancellationToken);
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
        Task<TEntity> FetchFirstAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;
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
        Task<TEntity> FetchFirstAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : EntityBase2, IEntity2;
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
        Task<List<TElement>> FetchQueryAsync<TElement>(DynamicQuery<TElement> query, CancellationToken cancellationToken);
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
        Task<TCollection> FetchQueryAsync<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill, CancellationToken cancellationToken)
            where TEntity : IEntity2
            where TCollection : IEntityCollection2;
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
        Task<TCollection> FetchQueryAsync<TEntity, TCollection>(EntityQuery<TEntity> query, TCollection collectionToFill)
            where TEntity : IEntity2
            where TCollection : IEntityCollection2;
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
        Task<IEntityCollection2> FetchQueryAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : IEntity2;
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
        Task<List<object[]>> FetchQueryAsync(DynamicQuery query);
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
        Task<List<object[]>> FetchQueryAsync(DynamicQuery query, CancellationToken cancellationToken);
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
        Task<List<TElement>> FetchQueryAsync<TElement>(DynamicQuery<TElement> query);
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
        Task<IEntityCollection2> FetchQueryAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : IEntity2;
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
        Task<List<TElement>> FetchQueryFromSourceAsync<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source, CancellationToken cancellationToken);
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
        Task<List<TElement>> FetchQueryFromSourceAsync<TElement>(DynamicQuery<TElement> projectionDefinition, IRetrievalQuery source);
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
        Task<TValue> FetchScalarAsync<TValue>(DynamicQuery query);
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
        Task<TValue> FetchScalarAsync<TValue>(DynamicQuery query, CancellationToken cancellationToken);
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
        Task<T> FetchSingleAsync<T>(DynamicQuery<T> query, CancellationToken cancellationToken);
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
        Task<T> FetchSingleAsync<T>(DynamicQuery<T> query);
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
        Task<TEntity> FetchSingleAsync<TEntity>(EntityQuery<TEntity> query, CancellationToken cancellationToken) where TEntity : EntityBase2, IEntity2;
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
        Task<TEntity> FetchSingleAsync<TEntity>(EntityQuery<TEntity> query) where TEntity : EntityBase2, IEntity2;
        #endregion
    }
}