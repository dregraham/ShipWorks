///////////////////////////////////////////////////////////////
// This is generated code. 
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: SD.TemplateBindings.SharedTemplates
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model
{
	/// <summary>Class which contains the static logic to execute action stored procedures in the database.</summary>
	public static partial class ActionProcedures
	{

		/// <summary>Delegate definition for stored procedure 'CalculateInitialFilterCounts' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int CalculateInitialFilterCountsCallBack(ref System.Int32 nodesUpdated, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'CalculateUpdateFilterCounts' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int CalculateUpdateFilterCountsCallBack(IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'CalculateUpdateQuickFilterCounts' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int CalculateUpdateQuickFilterCountsCallBack(IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'DeleteAbandonedFilterCounts' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int DeleteAbandonedFilterCountsCallBack(IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'ResetShipSense' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int ResetShipSenseCallBack(IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'ShipmentShipSenseProcedure' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int ShipmentShipSenseProcedureCallBack(System.String hashKey, System.String excludedShipmentXml, IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'ValidateFilterLayouts' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int ValidateFilterLayoutsCallBack(IDataAccessCore dataAccessProvider);
		/// <summary>Delegate definition for stored procedure 'ValidateGridLayouts' to be used in combination of a UnitOfWork2 object.</summary>
		public delegate int ValidateGridLayoutsCallBack(IDataAccessCore dataAccessProvider);


		/// <summary>Calls stored procedure 'CalculateInitialFilterCounts'.<br/><br/></summary>
		/// <param name="nodesUpdated">InputOutput parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int CalculateInitialFilterCounts(ref System.Int32 nodesUpdated)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return CalculateInitialFilterCounts(ref nodesUpdated, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'CalculateInitialFilterCounts'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="nodesUpdated">InputOutput parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int CalculateInitialFilterCounts(ref System.Int32 nodesUpdated, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateCalculateInitialFilterCountsCall(dataAccessProvider, nodesUpdated))
			{
				int toReturn = call.Call();
				nodesUpdated = call.GetParameterValue<System.Int32>(0);
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'CalculateUpdateFilterCounts'.<br/><br/></summary>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int CalculateUpdateFilterCounts()
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return CalculateUpdateFilterCounts(dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'CalculateUpdateFilterCounts'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int CalculateUpdateFilterCounts(IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateCalculateUpdateFilterCountsCall(dataAccessProvider))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'CalculateUpdateQuickFilterCounts'.<br/><br/></summary>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int CalculateUpdateQuickFilterCounts()
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return CalculateUpdateQuickFilterCounts(dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'CalculateUpdateQuickFilterCounts'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int CalculateUpdateQuickFilterCounts(IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateCalculateUpdateQuickFilterCountsCall(dataAccessProvider))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'DeleteAbandonedFilterCounts'.<br/><br/></summary>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int DeleteAbandonedFilterCounts()
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return DeleteAbandonedFilterCounts(dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'DeleteAbandonedFilterCounts'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int DeleteAbandonedFilterCounts(IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateDeleteAbandonedFilterCountsCall(dataAccessProvider))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'ResetShipSense'.<br/><br/></summary>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ResetShipSense()
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return ResetShipSense(dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'ResetShipSense'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ResetShipSense(IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateResetShipSenseCall(dataAccessProvider))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'ShipmentShipSenseProcedure'.<br/><br/></summary>
		/// <param name="hashKey">Input parameter. </param>
		/// <param name="excludedShipmentXml">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ShipmentShipSenseProcedure(System.String hashKey, System.String excludedShipmentXml)
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return ShipmentShipSenseProcedure(hashKey, excludedShipmentXml, dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'ShipmentShipSenseProcedure'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="hashKey">Input parameter. </param>
		/// <param name="excludedShipmentXml">Input parameter. </param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ShipmentShipSenseProcedure(System.String hashKey, System.String excludedShipmentXml, IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateShipmentShipSenseProcedureCall(dataAccessProvider, hashKey, excludedShipmentXml))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'ValidateFilterLayouts'.<br/><br/></summary>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ValidateFilterLayouts()
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return ValidateFilterLayouts(dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'ValidateFilterLayouts'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ValidateFilterLayouts(IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateValidateFilterLayoutsCall(dataAccessProvider))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}

		/// <summary>Calls stored procedure 'ValidateGridLayouts'.<br/><br/></summary>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ValidateGridLayouts()
		{
			using(DataAccessAdapter dataAccessProvider = new DataAccessAdapter())
			{
				return ValidateGridLayouts(dataAccessProvider);
			}
		}

		/// <summary>Calls stored procedure 'ValidateGridLayouts'.<br/><br/></summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Number of rows affected, if the database / routine doesn't suppress rowcounting.</returns>
		public static int ValidateGridLayouts(IDataAccessCore dataAccessProvider)
		{
			using(StoredProcedureCall call = CreateValidateGridLayoutsCall(dataAccessProvider))
			{
				int toReturn = call.Call();
				return toReturn;
			}
		}
		
		/// <summary>Creates the call object for the call 'CalculateInitialFilterCounts' to stored procedure 'CalculateInitialFilterCounts'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="nodesUpdated">InputOutput parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateCalculateInitialFilterCountsCall(IDataAccessCore dataAccessProvider, System.Int32 nodesUpdated)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[CalculateInitialFilterCounts]", "CalculateInitialFilterCounts")
							.AddParameter("@nodesUpdated", "Int", 0, ParameterDirection.InputOutput, true, 10, 0, nodesUpdated);
		}

		/// <summary>Creates the call object for the call 'CalculateUpdateFilterCounts' to stored procedure 'CalculateUpdateFilterCounts'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateCalculateUpdateFilterCountsCall(IDataAccessCore dataAccessProvider)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[CalculateUpdateFilterCounts]", "CalculateUpdateFilterCounts");
		}

		/// <summary>Creates the call object for the call 'CalculateUpdateQuickFilterCounts' to stored procedure 'CalculateUpdateQuickFilterCounts'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateCalculateUpdateQuickFilterCountsCall(IDataAccessCore dataAccessProvider)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[CalculateUpdateQuickFilterCounts]", "CalculateUpdateQuickFilterCounts");
		}

		/// <summary>Creates the call object for the call 'DeleteAbandonedFilterCounts' to stored procedure 'DeleteAbandonedFilterCounts'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateDeleteAbandonedFilterCountsCall(IDataAccessCore dataAccessProvider)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[DeleteAbandonedFilterCounts]", "DeleteAbandonedFilterCounts");
		}

		/// <summary>Creates the call object for the call 'ResetShipSense' to stored procedure 'ResetShipSense'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateResetShipSenseCall(IDataAccessCore dataAccessProvider)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[ResetShipSense]", "ResetShipSense");
		}

		/// <summary>Creates the call object for the call 'ShipmentShipSenseProcedure' to stored procedure 'ShipmentShipSenseProcedure'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <param name="hashKey">Input parameter</param>
		/// <param name="excludedShipmentXml">Input parameter</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateShipmentShipSenseProcedureCall(IDataAccessCore dataAccessProvider, System.String hashKey, System.String excludedShipmentXml)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[ShipmentShipSenseProcedure]", "ShipmentShipSenseProcedure")
							.AddParameter("@hashKey", "NVarChar", 4000, ParameterDirection.Input, true, 0, 0, hashKey)
							.AddParameter("@excludedShipmentXml", "Xml", 2147483647, ParameterDirection.Input, true, 0, 0, excludedShipmentXml);
		}

		/// <summary>Creates the call object for the call 'ValidateFilterLayouts' to stored procedure 'ValidateFilterLayouts'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateValidateFilterLayoutsCall(IDataAccessCore dataAccessProvider)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[ValidateFilterLayouts]", "ValidateFilterLayouts");
		}

		/// <summary>Creates the call object for the call 'ValidateGridLayouts' to stored procedure 'ValidateGridLayouts'.</summary>
		/// <param name="dataAccessProvider">The data access provider.</param>
		/// <returns>Ready to use StoredProcedureCall object</returns>
		private static StoredProcedureCall CreateValidateGridLayoutsCall(IDataAccessCore dataAccessProvider)
		{
			return new StoredProcedureCall(dataAccessProvider, @"[ShipWorksLocal].[dbo].[ValidateGridLayouts]", "ValidateGridLayouts");
		}


		#region Included Code

		#endregion
	}
}
