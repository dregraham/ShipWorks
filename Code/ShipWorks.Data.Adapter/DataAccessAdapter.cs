///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SqlServerSpecific.NET20
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Data;
using System.Data.Common;

#if CF
using System.Data.SqlServerCe;
#else
using System.Data.SqlClient;
#endif

#if !CF
using System.Configuration;
using System.EnterpriseServices;
#endif

using SD.LLBLGen.Pro.ORMSupportClasses;
using SD.LLBLGen.Pro.DQE.SqlServer;

namespace ShipWorks.Data.Adapter
{
	
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END

	/// <summary>
	/// Data access adapter class, which controls the complete database interaction with the database for all objects.
	/// SqlServer specific.
	/// </summary>
	/// <remarks>
	/// Use a DataAccessAdapter object solely per thread, and per connection. A DataAccessAdapter object contains 1 active connection 
	/// and no thread-access scheduling code. This means that you need to create a new DataAccessAdapter object if you want to utilize
	/// in another thread a new connection and a new transaction or want to open a new connection.
	/// </remarks>
	public partial class DataAccessAdapter : DataAccessAdapterBase
	{
#if !CF	
		#region Public static members
		/// <summary>
		/// The name of the key in the *.config file of the executing application which contains the connection string.
		/// </summary>
		/// <remarks>Default: the value set in the LLBLGen Pro project properties</remarks>
		public static string ConnectionStringKeyName="Main.ConnectionString";
		#endregion
#endif		
		#region Class Member Declarations
		// SqlServer specific information about catalog name usage
		private CatalogNameOverwriteHashtable _catalogNameOverwrites;
		private SchemaNameOverwriteHashtable _schemaNameOverwrites;
		#endregion

		#region Constructors
		/// <summary>
		/// CTor
		/// </summary>
		public DataAccessAdapter() : base (PersistenceInfoProviderSingleton.GetInstance())
		{
#if CF
			InitClass(string.Empty, false, CatalogNameUsage.Default, String.Empty, null, null);
#else
			InitClass(ReadConnectionStringFromConfig(), false, CatalogNameUsage.Default, String.Empty, null, null);
#endif			
		}

#if !CF
		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="comPlusContextHost">the COM+ context host for this adapter instance.</param>
		/// <remarks>do not call this from your code</remarks>
		public DataAccessAdapter(IComPlusAdapterContext comPlusContextHost) : base(comPlusContextHost, PersistenceInfoProviderSingleton.GetInstance())
		{
			InitClass(ReadConnectionStringFromConfig(), false, CatalogNameUsage.Default, String.Empty, null, null);
		}

		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="comPlusContextHost">the COM+ context host for this adapter instance.</param>
		/// <param name="connectionString">The connection string to use when connecting to the database.</param>
		/// <remarks>do not call this from your code</remarks>
		public DataAccessAdapter(IComPlusAdapterContext comPlusContextHost, string connectionString) : base(comPlusContextHost, PersistenceInfoProviderSingleton.GetInstance())
		{
			InitClass(connectionString, false, CatalogNameUsage.Default, String.Empty, null, null);
		}
#endif

		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="keepConnectionOpen">when true, the DataAccessAdapter will not close an opened connection. Use this for multi action usage.</param>
		public DataAccessAdapter(bool keepConnectionOpen) : base(PersistenceInfoProviderSingleton.GetInstance())
		{
#if CF
			InitClass(string.Empty, keepConnectionOpen, CatalogNameUsage.Default, String.Empty, null, null);
#else
			InitClass(ReadConnectionStringFromConfig(), keepConnectionOpen, CatalogNameUsage.Default, String.Empty, null, null);
#endif
		}


		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="connectionString">The connection string to use when connecting to the database.</param>
		public DataAccessAdapter(string connectionString) : base(PersistenceInfoProviderSingleton.GetInstance())
		{
			InitClass(connectionString, false, CatalogNameUsage.Default, String.Empty, null, null);
		}


		/// <summary>
		/// CTor
		/// </summary>
		/// <param name="connectionString">The connection string to use when connecting to the database.</param>
		/// <param name="keepConnectionOpen">when true, the DataAccessAdapter will not close an opened connection. Use this for multi action usage.</param>
		public DataAccessAdapter(string connectionString, bool keepConnectionOpen) : base(PersistenceInfoProviderSingleton.GetInstance())
		{
			InitClass(connectionString, keepConnectionOpen, CatalogNameUsage.Default, String.Empty, null, null);
		}
		
		
		/// <summary>
		/// CTor. SqlServer specific
		/// </summary>
		/// <param name="connectionString">The connection string to use when connecting to the database.</param>
		/// <param name="keepConnectionOpen">when true, the DataAccessAdapter will not close an opened connection. Use this for multi action usage.</param>
		/// <param name="catalogNameUsageSetting">SqlServer specific. Configures this data access adapter object how to threat catalog names in 
		/// persistence information.</param>
		/// <param name="catalogNameToUse">SqlServer specific. The name to use if catalogNameUsageSetting is set to ForceName. Ignored otherwise.</param>
		/// <remarks>For backwards compatibility.</remarks>
		public DataAccessAdapter(string connectionString, bool keepConnectionOpen, CatalogNameUsage catalogNameUsageSetting, string catalogNameToUse) : base(PersistenceInfoProviderSingleton.GetInstance())
		{
			InitClass(connectionString, keepConnectionOpen, catalogNameUsageSetting, catalogNameToUse, null, null);
		}


		/// <summary>
		/// CTor. SqlServer specific
		/// </summary>
		/// <param name="connectionString">The connection string to use when connecting to the database.</param>
		/// <param name="keepConnectionOpen">when true, the DataAccessAdapter will not close an opened connection. Use this for multi action usage.</param>
		/// <param name="catalogNameOverwrites">SqlServer specific. The from-to name value pairs and setting for the overwriting of catalog names.
		/// Can be null.</param>
		/// <param name="schemaNameOverwrites">SqlServer specific. The from-to name value pairs and setting for the overwriting of schema names.
		/// Can be null.</param>
		public DataAccessAdapter(string connectionString, bool keepConnectionOpen, CatalogNameOverwriteHashtable catalogNameOverwrites, 
			SchemaNameOverwriteHashtable schemaNameOverwrites) : base(PersistenceInfoProviderSingleton.GetInstance())
		{
			InitClass(connectionString, keepConnectionOpen, CatalogNameUsage.Default, String.Empty, catalogNameOverwrites, schemaNameOverwrites);
		}

		#endregion

#if !CF
		#region Stored Procedure related routines
		/// <summary>
		/// Calls the specified action stored procedure in the SqlServer database. If a transaction is in progress, it will participate in
		/// that transaction.
		/// </summary>
		/// <param name="storedProcedureToCall">Stored procedure to call</param>
		/// <param name="parameters">array of parameters to specify</param>
		/// <returns>the amount of rows affected. This value will be -1 if the stored procedure sets ROWCOUNT to OFF or this has
		/// been disabled in the catalog by other settings.</returns>
		public virtual int CallActionStoredProcedure(string storedProcedureToCall, SqlParameter[] parameters)
		{
			SqlCommand command = new SqlCommand(CreateCorrectStoredProcedureName(storedProcedureToCall));
			command.Connection = (SqlConnection)base.GetActiveConnection();
			if(base.IsTransactionInProgress)
			{
				command.Transaction = (SqlTransaction)base.PhysicalTransaction;
			}
			command.CommandType = CommandType.StoredProcedure;
			command.CommandTimeout = base.CommandTimeOut;

			int toReturn = -1;
			try
			{
				for(int i=0;i<parameters.Length;i++)
				{
					command.Parameters.Add(parameters[i]);
				}

				base.OpenConnection();
				toReturn = command.ExecuteNonQuery();
			}
			finally
			{
				command.Dispose();
				// clean up a dangling automaticly opened connection if needed.
				if(!(base.KeepConnectionOpen || base.IsTransactionInProgress))
				{
					base.CloseConnection();
				}
			}
			return toReturn;
		}


		/// <summary>
		/// Calls the specified retrieval stored procedure in the SqlServer database. Fills the
		/// specified datatable. Will participate in the transaction if a transaction is in progress.
		/// </summary>
		/// <param name="storedProcedureToCall">Stored procedure to call</param>
		/// <param name="parameters">array of parameters to specify</param>
		/// <param name="tableToFill">Datatable to fill by the stored procedure</param>
		/// <returns>true if succeeded, false otherwise</returns>
		public virtual bool CallRetrievalStoredProcedure(string storedProcedureToCall, SqlParameter[] parameters, DataTable tableToFill)
		{
			SqlCommand command = new SqlCommand(CreateCorrectStoredProcedureName(storedProcedureToCall));
			command.Connection = (SqlConnection)base.GetActiveConnection();
			if(base.IsTransactionInProgress)
			{
				command.Transaction = (SqlTransaction)base.PhysicalTransaction;
			}
			command.CommandType = CommandType.StoredProcedure;
			command.CommandTimeout = base.CommandTimeOut;

			using(SqlDataAdapter adapter = (SqlDataAdapter)CreateNewPhysicalDataAdapter())
			{
				adapter.SelectCommand = command;

				for(int i=0;i<parameters.Length;i++)
				{
					command.Parameters.Add(parameters[i]);
				}
				adapter.Fill(tableToFill);
			}
			command.Dispose();
			return true;
		}


		/// <summary>
		/// Calls the specified retrieval stored procedure in the SqlServer database. Fills the
		/// specified DataSet. Will participate in the transaction if a transaction is in progress.
		/// </summary>
		/// <param name="storedProcedureToCall">Stored procedure to call</param>
		/// <param name="parameters">array of parameters to specify</param>
		/// <param name="dataSetToFill">DataSet to fill by the stored procedure</param>
		/// <returns>true if succeeded, false otherwise</returns>
		public virtual bool CallRetrievalStoredProcedure(string storedProcedureToCall, SqlParameter[] parameters, DataSet dataSetToFill)
		{
			SqlCommand command = new SqlCommand(CreateCorrectStoredProcedureName(storedProcedureToCall));
			command.Connection = (SqlConnection)base.GetActiveConnection();
			if(base.IsTransactionInProgress)
			{
				command.Transaction = (SqlTransaction)base.PhysicalTransaction;
			}
			command.CommandType = CommandType.StoredProcedure;
			command.CommandTimeout = base.CommandTimeOut;

			using(SqlDataAdapter adapter = (SqlDataAdapter)CreateNewPhysicalDataAdapter())
			{
				adapter.SelectCommand = command;

				for(int i=0;i<parameters.Length;i++)
				{
					command.Parameters.Add(parameters[i]);
				}
				adapter.Fill(dataSetToFill);
			}
			command.Dispose();
			return true;
		}
		#endregion


		/// <summary>
		/// Executes the passed in retrievalquery and returns an open, ready to use IDataReader. The datareader's command behavior is set to the
		/// readerBehavior passed in. If a transaction is in progress, the command is wired to the transaction.
		/// </summary>
		/// <param name="queryToExecute">The query to execute.</param>
		/// <param name="readerBehavior">The reader behavior to set.</param>
		/// <returns>Open, ready to use IDataReader</returns>
		/// <remarks>Advanced functionality: be aware that the datareader returned is open, and the dataaccessadapter's connection is also open</remarks>
		public override IDataReader FetchDataReader(IRetrievalQuery queryToExecute, CommandBehavior readerBehavior)
		{
			if(queryToExecute.Command.CommandType == CommandType.StoredProcedure)
			{
				queryToExecute.Command.CommandText = CreateCorrectStoredProcedureName(queryToExecute.Command.CommandText);
			}
			return base.FetchDataReader(queryToExecute, readerBehavior);
		}

		/// <summary>
		/// Sets the flag to signal the SqlServer DQE to generate SET ARITHABORT ON statements prior to INSERT, DELETE and UPDATE Queries.
		/// Keep this flag to false in normal usage, but set it to true if you need to write into a table which is part of an indexed view.
		/// It will not affect normal inserts/updates that much, leaving it on is not harmful. See Books online for details on SET ARITHABORT ON.
		/// After each statement the setting is turned off if it has been turned on prior to that statement.
		/// </summary>
		/// <remarks>Setting this flag is a global change.</remarks>
		public static void SetArithAbortFlag(bool value)
		{
			SD.LLBLGen.Pro.DQE.SqlServer.DynamicQueryEngine.ArithAbortOn = value;
		}

		/// <summary>
		/// Compatibility level used by the DQE. Default is SqlServer2000. To utilize SqlServer 2005 specific features, set this parameter 
		/// to SqlServer2005, either through a setting in the .config file of your application or by setting this parameter once in your application.
		/// Compatibility level influences the query generated for paging, sequence name (@@IDENTITY/SCOPE_IDENTITY()), and usage of newsequenceid() in inserts. 
		/// It also influences the provider to use. This way you can switch between SqlServer server client 'SqlClient' and SqlServer CE Desktop. 
		/// </summary>
		/// <remarks>Setting this property will overrule a similar setting in the .config file. Don't set this property when queries are executed as
		/// it might switch factories for ADO.NET elements which could result in undefined behavior. Set this property at startup of your application as it's
		/// a global setting (affects all queries in your application using this DQE)</remarks>
		public static void SetSqlServerCompatibilityLevel(SqlServerCompatibilityLevel compatibilityLevel)
		{
			SD.LLBLGen.Pro.DQE.SqlServer.DynamicQueryEngine.CompatibilityLevel = compatibilityLevel;
		}
#endif

		/// <summary>
		/// Inserts in each predicate expression element the persistence info object for the field used. If there is already a fieldpersistenceinfo 
		/// element for a given field, it is skipped. 
		/// </summary>
		/// <param name="expression">IPredicateExpression object which has predicate elements whose persistence info objects have to be
		/// set to a value.</param>
		protected override void InsertPersistenceInfoObjects(IPredicateExpression expression)
		{
			if(expression == null)
			{
				return;
			}

			for (int i = 0; i < expression.Count; i++)
			{
				if(expression[i].Type!=PredicateExpressionElementType.Predicate)
				{
					continue;
				}

				IPredicate currentPredicate = (IPredicate)expression[i].Contents;
				switch((PredicateType)currentPredicate.InstanceType)
				{
					case PredicateType.Undefined:
						continue;
					case PredicateType.PredicateExpression:
						// recurse
						InsertPersistenceInfoObjects((IPredicateExpression)expression[i].Contents);
						break;
					case PredicateType.FieldBetweenPredicate:
						FieldBetweenPredicate betweenPredicate = (FieldBetweenPredicate)currentPredicate;
						if(betweenPredicate.PersistenceInfo==null)
						{
							betweenPredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)betweenPredicate.FieldCore);
						}
						if(betweenPredicate.BeginIsField && (betweenPredicate.PersistenceInfoBegin==null))
						{
							betweenPredicate.PersistenceInfoBegin = GetFieldPersistenceInfo((IEntityField2)betweenPredicate.FieldBeginCore);
						}
						if(betweenPredicate.EndIsField && (betweenPredicate.PersistenceInfoEnd==null))
						{
							betweenPredicate.PersistenceInfoEnd = GetFieldPersistenceInfo((IEntityField2)betweenPredicate.FieldEndCore);
						}
						break;
					case PredicateType.FieldCompareNullPredicate:
						FieldCompareNullPredicate compareNullPredicate = (FieldCompareNullPredicate)currentPredicate;
						if(compareNullPredicate.PersistenceInfo==null)
						{
							compareNullPredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)compareNullPredicate.FieldCore);
						}
						break;
					case PredicateType.FieldCompareValuePredicate:
						FieldCompareValuePredicate compareValuePredicate = (FieldCompareValuePredicate)currentPredicate;
						if(compareValuePredicate.PersistenceInfo==null)
						{
							compareValuePredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)compareValuePredicate.FieldCore);
						}
						break;
					case PredicateType.FieldLikePredicate:
						FieldLikePredicate likePredicate = (FieldLikePredicate)currentPredicate;
						if(likePredicate.PersistenceInfo==null)
						{
							likePredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)likePredicate.FieldCore);
						}
						break;
					case PredicateType.FieldCompareRangePredicate:
						FieldCompareRangePredicate compareRangePredicate = (FieldCompareRangePredicate)currentPredicate;
						if(compareRangePredicate.PersistenceInfo==null)
						{
							compareRangePredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)compareRangePredicate.FieldCore);
						}
						break;
					case PredicateType.FieldCompareExpressionPredicate:
						FieldCompareExpressionPredicate expressionPredicate = (FieldCompareExpressionPredicate)currentPredicate;
						if(expressionPredicate.PersistenceInfo==null)
						{
							expressionPredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)expressionPredicate.FieldCore);
						}
						if(expressionPredicate.ExpressionToCompareWith!=null)
						{
							InsertPersistenceInfoObjects(expressionPredicate.ExpressionToCompareWith);
						}
						break;
					case PredicateType.FieldFullTextSearchPredicate:
						FieldFullTextSearchPredicate fullTextSearchPredicate = (FieldFullTextSearchPredicate)currentPredicate;
						if(fullTextSearchPredicate.TargetIsFieldList)
						{
							if(fullTextSearchPredicate.PersistenceInfosFieldsList==null)
							{
								ArrayList persistenceInfos = new ArrayList();
								foreach(IEntityField2 field in fullTextSearchPredicate.FieldsList)
								{
									persistenceInfos.Add(GetFieldPersistenceInfo(field));
								}
								fullTextSearchPredicate.PersistenceInfosFieldsList = persistenceInfos;
							}
						}
						else
						{
							if(fullTextSearchPredicate.PersistenceInfo==null)
							{
								fullTextSearchPredicate.PersistenceInfo = GetFieldPersistenceInfo((IEntityField2)fullTextSearchPredicate.FieldCore);
							}
						}
						break;
					case PredicateType.FieldCompareSetPredicate:
						FieldCompareSetPredicate compareSetPredicate = (FieldCompareSetPredicate)currentPredicate;
						if((compareSetPredicate.PersistenceInfoField==null)&&(compareSetPredicate.FieldCore!=null))
						{
							compareSetPredicate.PersistenceInfoField = GetFieldPersistenceInfo((IEntityField2)compareSetPredicate.FieldCore);
						}
						if(compareSetPredicate.PersistenceInfoSetField==null)
						{
							compareSetPredicate.PersistenceInfoSetField = GetFieldPersistenceInfo((IEntityField2)compareSetPredicate.SetFieldCore);
						}
						InsertPersistenceInfoObjects(compareSetPredicate.SetFilterAsPredicateExpression);
						InsertPersistenceInfoObjects(compareSetPredicate.SetRelations);
						InsertPersistenceInfoObjects(compareSetPredicate.SetSorter);
						InsertPersistenceInfoObjects(compareSetPredicate.GroupByClause);
						break;
					default:
						
						// __LLBLGENPRO_USER_CODE_REGION_START InsertPersistenceInfoObjectsPredicate
						// __LLBLGENPRO_USER_CODE_REGION_END
						OnInsertPersistenceObjects(currentPredicate);
						break;
				}
			}
		}


		/// <summary>
		/// Creates a new Dynamic Query engine object and passes in the defined catalog/schema overwrite hashtables.
		/// </summary>
		protected override DynamicQueryEngineBase CreateDynamicQueryEngine()
		{
			DynamicQueryEngine toReturn = new DynamicQueryEngine();
			toReturn.PerCallCatalogNameOverwrites = _catalogNameOverwrites;
			toReturn.PerCallSchemaNameOverwrites = _schemaNameOverwrites;
			return (DynamicQueryEngineBase)toReturn;
		}

		
		/// <summary>
		/// Creates a new physical connection object.
		/// </summary>
		/// <param name="connectionString">Connectionstring to use for the new connection object</param>
		/// <returns>IDbConnection implementing connection object.</returns>
		protected override IDbConnection CreateNewPhysicalConnection(string connectionString)
		{
#if CF
			return new SqlCeConnection(connectionString);
#else
			DbConnection toReturn = SD.LLBLGen.Pro.DQE.SqlServer.DynamicQueryEngine.FactoryToUse.CreateConnection();
			toReturn.ConnectionString = connectionString;
			return toReturn;
#endif
		}


		/// <summary>
		/// Creates a new physical transaction object over the created connection. The connection is assumed to be open.
		/// </summary>
		/// <returns>a physical transaction object, like an instance of SqlTransaction.</returns>
		protected override IDbTransaction CreateNewPhysicalTransaction()
		{
#if CF
			return ((SqlCeConnection)base.GetActiveConnection()).BeginTransaction(base.TransactionIsolationLevel);
#else
			return base.GetActiveConnection().BeginTransaction(base.TransactionIsolationLevel);
#endif
		}


		/// <summary>
		/// Creates a new .NET DataAdapter for the database system this DataAccessAdapter object is targeting. 
		/// </summary>
		/// <returns>New .NET DataAdapter object</returns>
		protected override DbDataAdapter CreateNewPhysicalDataAdapter()
		{
#if CF
			return new SqlCeDataAdapter();
#else
			return SD.LLBLGen.Pro.DQE.SqlServer.DynamicQueryEngine.FactoryToUse.CreateDataAdapter();
#endif			
		}


		/// <summary>
		/// Initializes the class.
		/// </summary>
		/// <param name="connectionString"></param>
		/// <param name="keepConnectionOpen"></param>
		/// <param name="catalogNameUsageSetting"></param>
		/// <param name="catalogNameToUse"></param>
		/// <param name="catalogNameOverwrites"></param>
		/// <param name="schemaNameOverwrites"></param>
		private void InitClass(string connectionString, bool keepConnectionOpen, CatalogNameUsage catalogNameUsageSetting, string catalogNameToUse,
			CatalogNameOverwriteHashtable catalogNameOverwrites, SchemaNameOverwriteHashtable schemaNameOverwrites)
		{
			base.ConnectionString = connectionString;
			base.KeepConnectionOpen = keepConnectionOpen;
			if(catalogNameOverwrites == null)
			{
				_catalogNameOverwrites = new CatalogNameOverwriteHashtable(catalogNameUsageSetting);
				if(catalogNameUsageSetting!=CatalogNameUsage.Default)
				{
					_catalogNameOverwrites.Add("*", catalogNameToUse);
				}
				else
				{
					base.ReadHandleCatalogNameSettingFromConfig(_catalogNameOverwrites);
				}
			}
			else
			{
				_catalogNameOverwrites = catalogNameOverwrites;
			}
			_schemaNameOverwrites = schemaNameOverwrites;
		}


#if !CF
		/// <summary>
		/// Reads the value of the setting with the key ConnectionStringKeyName from the *.config file and stores that value as the
		/// active connection string to use for this object.
		/// </summary>
		/// <returns>connection string read</returns>
		private string ReadConnectionStringFromConfig()
		{
			return ConfigFileHelper.ReadConnectionStringFromConfig( DataAccessAdapter.ConnectionStringKeyName);
		}

		/// <summary>
		/// Creates the correct name of the stored procedure to call, thus with schema/catalog name overwrites. 
		/// </summary>
		/// <param name="storedProcedureName">Name of the stored procedure.</param>
		/// <returns>stored procedure name with the catalog/schema name overwritten, if applicable</returns>
		private string CreateCorrectStoredProcedureName(string storedProcedureName)
		{
			DynamicQueryEngine dqe = (DynamicQueryEngine)CreateDynamicQueryEngine();
			string procName = dqe.GetNewPerCallStoredProcedureName(storedProcedureName);
			return DynamicQueryEngine.GetNewStoredProcedureName(procName);
		}
#endif
	
		#region Class Property Declarations
		/// <summary>
		/// SqlServer specific. Configurates this data access adapter object how to threat catalog names in persistence information.
		/// </summary>
		/// <remarks>For backwards compatibility.</remarks>
		public CatalogNameUsage CatalogNameUsageSetting
		{
			get
			{
				if(_catalogNameOverwrites!=null)
				{
					return _catalogNameOverwrites.CatalogNameUsageSetting;
				}
				else
				{
					return CatalogNameUsage.Default;
				}
			}
			set
			{
				if(_catalogNameOverwrites!=null)
				{
					_catalogNameOverwrites.CatalogNameUsageSetting = value;
				}
			}
		}
		
		/// <summary>
		/// SqlServer specific. The name to use if catalogNameUsageSetting is set to ForceName. Ignored otherwise.
		/// </summary>
		/// <remarks>For backwards compatibility.</remarks>
		public string CatalogNameToUse
		{
			get
			{
				if((_catalogNameOverwrites!=null)&&(_catalogNameOverwrites.ContainsKey("*")))
				{
					return (string)_catalogNameOverwrites["*"];
				}
				else
				{
					return string.Empty;
				}
			}
			set
			{
				if(_catalogNameOverwrites!=null)
				{
					if(_catalogNameOverwrites.ContainsKey("*"))
					{
						_catalogNameOverwrites["*"] = value;
					}
					else
					{
						_catalogNameOverwrites.Add("*", value);
					}
				}
			}
		}
		
		
		/// <summary>
		/// SqlServer specific. The from-to name value pairs and setting for the overwriting of catalog names.
		/// </summary>
		public CatalogNameOverwriteHashtable CatalogNameOverwrites
		{
			get { return _catalogNameOverwrites; }
			set { _catalogNameOverwrites = value; }
		}

		
		/// <summary>
		/// SqlServer specific. The from-to name value pairs and setting for the overwriting of schema names.
		/// </summary>
		public SchemaNameOverwriteHashtable SchemaNameOverwrites
		{
			get { return _schemaNameOverwrites; }
			set { _schemaNameOverwrites = value; }
		}
		
		
		#endregion

		#region Custom DataAccessAdapter code.
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomDataAccessAdapterCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion
		
		#region Included Code

		#endregion
	}
	

#if !CF
	/// <summary>
	/// ComPlusAdapterContext class which is used in Adapter for hosting a COM+ aware DataAccessAdapter class. 
	/// Use the hosted DataAccessAdapter class to start a new COM+ transaction.
	/// </summary>
	public partial class ComPlusAdapterContext : ComPlusAdapterContextBase
	{
		/// <summary>
		/// CTor
		/// </summary>
		public ComPlusAdapterContext()
		{
			base.SetAdapter(new DataAccessAdapter(this));
		}

		/// <summary>
		/// Creates the physical connection object
		/// </summary>
		/// <param name="connectionString">connection string to use</param>
		/// <returns>Usable connection object (closed)</returns>
		protected override IDbConnection CreateDatabaseConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}
	}
#endif
}
