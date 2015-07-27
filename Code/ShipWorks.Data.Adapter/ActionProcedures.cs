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
using System.Data;
using System.Data.SqlClient;

namespace ShipWorks.Data.Adapter
{
	/// <summary>
	/// Class which contains the static logic to execute action stored procedures in the database.
	/// </summary>
	public partial class ActionProcedures
	{
		/// <summary>
		/// private CTor so no instance can be created.
		/// </summary>
		private ActionProcedures()
		{
		}

	
		/// <summary>
		/// Delegate definition for stored procedure 'CalculateInitialFilterCounts' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int CalculateInitialFilterCountsCallBack(DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'CalculateInitialFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateInitialFilterCounts()
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return CalculateInitialFilterCounts( adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'CalculateInitialFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateInitialFilterCounts(DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[0];


			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[CalculateInitialFilterCounts]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'CalculateInitialFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateInitialFilterCounts(ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return CalculateInitialFilterCounts(ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'CalculateInitialFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateInitialFilterCounts(ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[0 + 1];


			parameters[0] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[CalculateInitialFilterCounts]", parameters);

			
			returnValue = (int)parameters[0].Value;
			return toReturn;
		}
	

		/// <summary>
		/// Delegate definition for stored procedure 'CalculateUpdateFilterCounts' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int CalculateUpdateFilterCountsCallBack(DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'CalculateUpdateFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateUpdateFilterCounts()
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return CalculateUpdateFilterCounts( adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'CalculateUpdateFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateUpdateFilterCounts(DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[0];


			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[CalculateUpdateFilterCounts]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'CalculateUpdateFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateUpdateFilterCounts(ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return CalculateUpdateFilterCounts(ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'CalculateUpdateFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int CalculateUpdateFilterCounts(ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[0 + 1];


			parameters[0] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[CalculateUpdateFilterCounts]", parameters);

			
			returnValue = (int)parameters[0].Value;
			return toReturn;
		}
	

		/// <summary>
		/// Delegate definition for stored procedure 'DeleteAbandonedFilterCounts' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int DeleteAbandonedFilterCountsCallBack(DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'DeleteAbandonedFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int DeleteAbandonedFilterCounts()
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return DeleteAbandonedFilterCounts( adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'DeleteAbandonedFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int DeleteAbandonedFilterCounts(DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[0];


			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[DeleteAbandonedFilterCounts]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'DeleteAbandonedFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int DeleteAbandonedFilterCounts(ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return DeleteAbandonedFilterCounts(ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'DeleteAbandonedFilterCounts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int DeleteAbandonedFilterCounts(ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[0 + 1];


			parameters[0] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[DeleteAbandonedFilterCounts]", parameters);

			
			returnValue = (int)parameters[0].Value;
			return toReturn;
		}
	

		/// <summary>
		/// Delegate definition for stored procedure 'ResetShipSense' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int ResetShipSenseCallBack(DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'ResetShipSense'.<br/><br/>
		/// 
		/// </summary>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ResetShipSense()
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ResetShipSense( adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'ResetShipSense'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ResetShipSense(DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[0];


			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ResetShipSense]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'ResetShipSense'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ResetShipSense(ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ResetShipSense(ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'ResetShipSense'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ResetShipSense(ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[0 + 1];


			parameters[0] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ResetShipSense]", parameters);

			
			returnValue = (int)parameters[0].Value;
			return toReturn;
		}
	

		/// <summary>
		/// Delegate definition for stored procedure 'ShipmentShipSenseProcedure' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int ShipmentShipSenseProcedureCallBack(System.String hashKey, System.String excludedShipmentXml, DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'ShipmentShipSenseProcedure'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="hashKey">Input parameter of stored procedure</param>
		/// <param name="excludedShipmentXml">Input parameter of stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ShipmentShipSenseProcedure(System.String hashKey, System.String excludedShipmentXml)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ShipmentShipSenseProcedure(hashKey, excludedShipmentXml,  adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'ShipmentShipSenseProcedure'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="hashKey">Input parameter of stored procedure</param>
		/// <param name="excludedShipmentXml">Input parameter of stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ShipmentShipSenseProcedure(System.String hashKey, System.String excludedShipmentXml, DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[2];
			parameters[0] = new SqlParameter("@hashKey", SqlDbType.NVarChar, 4000, ParameterDirection.Input, true, 0, 0, "",  DataRowVersion.Current, hashKey);
			parameters[1] = new SqlParameter("@excludedShipmentXml", SqlDbType.Xml, 2147483647, ParameterDirection.Input, true, 0, 0, "",  DataRowVersion.Current, excludedShipmentXml);

			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ShipmentShipSenseProcedure]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'ShipmentShipSenseProcedure'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="hashKey">Input parameter of stored procedure</param>
		/// <param name="excludedShipmentXml">Input parameter of stored procedure</param>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ShipmentShipSenseProcedure(System.String hashKey, System.String excludedShipmentXml, ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ShipmentShipSenseProcedure(hashKey, excludedShipmentXml, ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'ShipmentShipSenseProcedure'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="hashKey">Input parameter of stored procedure</param>
		/// <param name="excludedShipmentXml">Input parameter of stored procedure</param>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ShipmentShipSenseProcedure(System.String hashKey, System.String excludedShipmentXml, ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[2 + 1];
			parameters[0] = new SqlParameter("@hashKey", SqlDbType.NVarChar, 4000, ParameterDirection.Input, true, 0, 0, "",  DataRowVersion.Current, hashKey);
			parameters[1] = new SqlParameter("@excludedShipmentXml", SqlDbType.Xml, 2147483647, ParameterDirection.Input, true, 0, 0, "",  DataRowVersion.Current, excludedShipmentXml);

			parameters[2] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ShipmentShipSenseProcedure]", parameters);

			
			returnValue = (int)parameters[2].Value;
			return toReturn;
		}
	

		/// <summary>
		/// Delegate definition for stored procedure 'ValidateFilterLayouts' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int ValidateFilterLayoutsCallBack(DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'ValidateFilterLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateFilterLayouts()
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ValidateFilterLayouts( adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'ValidateFilterLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateFilterLayouts(DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[0];


			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ValidateFilterLayouts]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'ValidateFilterLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateFilterLayouts(ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ValidateFilterLayouts(ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'ValidateFilterLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateFilterLayouts(ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[0 + 1];


			parameters[0] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ValidateFilterLayouts]", parameters);

			
			returnValue = (int)parameters[0].Value;
			return toReturn;
		}
	

		/// <summary>
		/// Delegate definition for stored procedure 'ValidateGridLayouts' to be used in combination of a UnitOfWork2 object. 
		/// </summary>
		public delegate int ValidateGridLayoutsCallBack(DataAccessAdapter adapter);

		/// <summary>
		/// Calls stored procedure 'ValidateGridLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateGridLayouts()
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ValidateGridLayouts( adapter);
			}
		}


		/// <summary>
		/// Calls stored procedure 'ValidateGridLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateGridLayouts(DataAccessAdapter adapter)
		{
			SqlParameter[] parameters = new SqlParameter[0];


			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ValidateGridLayouts]", parameters);

			return toReturn;
		}
		

		/// <summary>
		/// Calls stored procedure 'ValidateGridLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateGridLayouts(ref System.Int32 returnValue)
		{
			using(DataAccessAdapter adapter = new DataAccessAdapter())
			{
				return ValidateGridLayouts(ref returnValue, adapter);
			}
		}

		
		/// <summary>
		/// Calls stored procedure 'ValidateGridLayouts'.<br/><br/>
		/// 
		/// </summary>
		/// <param name="returnValue">Return value of the stored procedure</param>
		/// <param name="adapter">The DataAccessAdapter object to use for the call</param>
		/// <returns>Amount of rows affected, if the database / routine doesn't surpress rowcounting.</returns>
		public static int ValidateGridLayouts(ref System.Int32 returnValue, DataAccessAdapter adapter)
		{
			// create parameters. Add 1 to make room for the return value parameter.
			SqlParameter[] parameters = new SqlParameter[0 + 1];


			parameters[0] = new SqlParameter("RETURNVALUE", SqlDbType.Int, 0, ParameterDirection.ReturnValue, true, 10, 0, "",  DataRowVersion.Current, returnValue);
			int toReturn = adapter.CallActionStoredProcedure("[ShipWorkslocal].[dbo].[ValidateGridLayouts]", parameters);

			
			returnValue = (int)parameters[0].Value;
			return toReturn;
		}
	

		#region Included Code

		#endregion
	}
}
