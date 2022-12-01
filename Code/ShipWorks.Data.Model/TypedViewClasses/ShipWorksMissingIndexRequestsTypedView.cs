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
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;


namespace ShipWorks.Data.Model.TypedViewClasses
{
	// __LLBLGENPRO_USER_CODE_REGION_START AdditionalNamespaces
	// __LLBLGENPRO_USER_CODE_REGION_END
	
	/// <summary>Typed datatable for the view 'ShipWorksMissingIndexRequests'.<br/><br/></summary>
	[Serializable, System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	public partial class ShipWorksMissingIndexRequestsTypedView : TypedViewBase<ShipWorksMissingIndexRequestsRow>, ITypedView2
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfacesView
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private DataColumn _columnIndexHandle;
		private DataColumn _columnTableName;
		private DataColumn _columnIndexAdvantage;
		private DataColumn _columnGroupHandle;
		private DataColumn _columnColumnID;
		private DataColumn _columnColumnName;
		private DataColumn _columnColumnUsage;
		private IEntityFields2	_fields;
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalMembers
		// __LLBLGENPRO_USER_CODE_REGION_END
		private static Hashtable	_customProperties;
		private static Hashtable	_fieldsCustomProperties;
		#endregion

		#region Class Constants
		/// <summary>
		/// The amount of fields in the resultset.
		/// </summary>
		private const int AmountOfFields = 7;
		#endregion

		/// <summary>Static CTor for setting up custom property hashtables.</summary>
		static ShipWorksMissingIndexRequestsTypedView()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary>CTor</summary>
		public ShipWorksMissingIndexRequestsTypedView():base("ShipWorksMissingIndexRequests")
		{
			InitClass();
		}
#if !CF	
		/// <summary>Protected constructor for deserialization.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ShipWorksMissingIndexRequestsTypedView(SerializationInfo info, StreamingContext context):base(info, context)
		{
			if (SerializationHelper.Optimization == SerializationOptimization.None)
			{
				InitMembers();
			}
		}
#endif
		/// <summary>Gets the IEntityFields2 collection of fields of this typed view. </summary>
		/// <returns>Ready to use IEntityFields2 collection object.</returns>
		public virtual IEntityFields2 GetFieldsInfo()
		{
			return _fields;
		}

		/// <summary>Creates a new typed row during the build of the datatable during a Fill session by a dataadapter.</summary>
		/// <param name="rowBuilder">supplied row builder to pass to the typed row</param>
		/// <returns>the new typed datarow</returns>
		protected override DataRow NewRowFromBuilder(DataRowBuilder rowBuilder) 
		{
			return new ShipWorksMissingIndexRequestsRow(rowBuilder);
		}

		/// <summary>Initializes the hashtables for the typed view type and typed view field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Hashtable();
			_fieldsCustomProperties = new Hashtable();
			Hashtable fieldHashtable;
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("IndexHandle", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("TableName", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("IndexAdvantage", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("GroupHandle", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("ColumnID", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("ColumnName", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("ColumnUsage", fieldHashtable);
		}

		/// <summary>
		/// Initialize the datastructures.
		/// </summary>
		protected override void InitClass()
		{
			TableName = "ShipWorksMissingIndexRequests";		
			_columnIndexHandle = GeneralUtils.CreateTypedDataTableColumn("IndexHandle", @"IndexHandle", typeof(System.Int32), this.Columns);
			_columnTableName = GeneralUtils.CreateTypedDataTableColumn("TableName", @"TableName", typeof(System.String), this.Columns);
			_columnIndexAdvantage = GeneralUtils.CreateTypedDataTableColumn("IndexAdvantage", @"IndexAdvantage", typeof(System.Double), this.Columns);
			_columnGroupHandle = GeneralUtils.CreateTypedDataTableColumn("GroupHandle", @"GroupHandle", typeof(System.Int32), this.Columns);
			_columnColumnID = GeneralUtils.CreateTypedDataTableColumn("ColumnID", @"ColumnID", typeof(System.Int32), this.Columns);
			_columnColumnName = GeneralUtils.CreateTypedDataTableColumn("ColumnName", @"ColumnName", typeof(System.String), this.Columns);
			_columnColumnUsage = GeneralUtils.CreateTypedDataTableColumn("ColumnUsage", @"ColumnUsage", typeof(System.String), this.Columns);
			_fields = EntityFieldsFactory.CreateTypedViewEntityFieldsObject(TypedViewType.ShipWorksMissingIndexRequestsTypedView);
			
			// __LLBLGENPRO_USER_CODE_REGION_START AdditionalFields
			// be sure to call _fields.Expand(number of new fields) first. 
			// __LLBLGENPRO_USER_CODE_REGION_END
			OnInitialized();
		}

		/// <summary>Initializes the members, after a clone action.</summary>
		private void InitMembers()
		{
			_columnIndexHandle = this.Columns["IndexHandle"];
			_columnTableName = this.Columns["TableName"];
			_columnIndexAdvantage = this.Columns["IndexAdvantage"];
			_columnGroupHandle = this.Columns["GroupHandle"];
			_columnColumnID = this.Columns["ColumnID"];
			_columnColumnName = this.Columns["ColumnName"];
			_columnColumnUsage = this.Columns["ColumnUsage"];
			_fields = EntityFieldsFactory.CreateTypedViewEntityFieldsObject(TypedViewType.ShipWorksMissingIndexRequestsTypedView);
			// __LLBLGENPRO_USER_CODE_REGION_START InitMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
		}

		/// <summary>Clones this instance.</summary>
		/// <returns>A clone of this instance</returns>
		public override DataTable Clone() 
		{
			ShipWorksMissingIndexRequestsTypedView cloneToReturn = ((ShipWorksMissingIndexRequestsTypedView)(base.Clone()));
			cloneToReturn.InitMembers();
			return cloneToReturn;
		}
#if !CF			
		/// <summary>Creates a new instance of the DataTable class.</summary>
		/// <returns>a new instance of a datatable with this schema.</returns>
		protected override DataTable CreateInstance() 
		{
			return new ShipWorksMissingIndexRequestsTypedView();
		}
#endif

		#region Class Property Declarations
		/// <summary>The custom properties for this TypedView type.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public static Hashtable CustomProperties
		{
			get { return _customProperties;}
		}

		/// <summary>The custom properties for the type of this TypedView instance.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[System.ComponentModel.Browsable(false)]
		public virtual Hashtable CustomPropertiesOfType
		{
			get { return ShipWorksMissingIndexRequestsTypedView.CustomProperties;}
		}

		/// <summary>The custom properties for the fields of this TypedView type. The returned Hashtable contains per fieldname a hashtable of name-value pairs. </summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		public static Hashtable FieldsCustomProperties
		{
			get { return _fieldsCustomProperties;}
		}

		/// <summary>The custom properties for the fields of the type of this TypedView instance. The returned Hashtable contains per fieldname a hashtable of name-value pairs.</summary>
		/// <remarks>The data returned from this property should be considered read-only: it is not thread safe to alter this data at runtime.</remarks>
		[System.ComponentModel.Browsable(false)]
		public virtual Hashtable FieldsCustomPropertiesOfType
		{
			get { return ShipWorksMissingIndexRequestsTypedView.FieldsCustomProperties;}
		}

		/// <summary>Returns the column object belonging to the TypedView field IndexHandle</summary>
		internal DataColumn IndexHandleColumn 
		{
			get { return _columnIndexHandle; }
		}

		/// <summary>Returns the column object belonging to the TypedView field TableName</summary>
		internal DataColumn TableNameColumn 
		{
			get { return _columnTableName; }
		}

		/// <summary>Returns the column object belonging to the TypedView field IndexAdvantage</summary>
		internal DataColumn IndexAdvantageColumn 
		{
			get { return _columnIndexAdvantage; }
		}

		/// <summary>Returns the column object belonging to the TypedView field GroupHandle</summary>
		internal DataColumn GroupHandleColumn 
		{
			get { return _columnGroupHandle; }
		}

		/// <summary>Returns the column object belonging to the TypedView field ColumnID</summary>
		internal DataColumn ColumnIDColumn 
		{
			get { return _columnColumnID; }
		}

		/// <summary>Returns the column object belonging to the TypedView field ColumnName</summary>
		internal DataColumn ColumnNameColumn 
		{
			get { return _columnColumnName; }
		}

		/// <summary>Returns the column object belonging to the TypedView field ColumnUsage</summary>
		internal DataColumn ColumnUsageColumn 
		{
			get { return _columnColumnUsage; }
		}

		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalColumnProperties
		// __LLBLGENPRO_USER_CODE_REGION_END
 		#endregion

		#region Custom TypedView code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomTypedViewCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion

		#region Included Code

		#endregion
	}

	/// <summary>Typed datarow for the typed datatable ShipWorksMissingIndexRequests</summary>
	public partial class ShipWorksMissingIndexRequestsRow : DataRow
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfacesRow
		// __LLBLGENPRO_USER_CODE_REGION_END	
	{
		#region Class Member Declarations
		private ShipWorksMissingIndexRequestsTypedView	_parent;
		#endregion

		/// <summary>CTor</summary>
		/// <param name="rowBuilder">Row builder object to use when building this row</param>
		protected internal ShipWorksMissingIndexRequestsRow(DataRowBuilder rowBuilder) : base(rowBuilder) 
		{
			_parent = ((ShipWorksMissingIndexRequestsTypedView)(this.Table));
		}

		#region Class Property Declarations

		/// <summary>Gets / sets the value of the TypedView field IndexHandle<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."IndexHandle"<br/>
		/// View field characteristics (type, precision, scale, length): Int, 10, 0, 0</remarks>
		public System.Int32 IndexHandle 
		{
			get { return IsIndexHandleNull() ? (System.Int32)TypeDefaultValue.GetDefaultValue(typeof(System.Int32)) : (System.Int32)this[_parent.IndexHandleColumn]; }
			set { this[_parent.IndexHandleColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field IndexHandle is NULL, false otherwise.</summary>
		public bool IsIndexHandleNull() 
		{
			return IsNull(_parent.IndexHandleColumn);
		}

		/// <summary>Sets the TypedView field IndexHandle to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetIndexHandleNull() 
		{
			this[_parent.IndexHandleColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field TableName<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."TableName"<br/>
		/// View field characteristics (type, precision, scale, length): NVarChar, 0, 0, 128</remarks>
		public System.String TableName 
		{
			get { return IsTableNameNull() ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : (System.String)this[_parent.TableNameColumn]; }
			set { this[_parent.TableNameColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field TableName is NULL, false otherwise.</summary>
		public bool IsTableNameNull() 
		{
			return IsNull(_parent.TableNameColumn);
		}

		/// <summary>Sets the TypedView field TableName to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetTableNameNull() 
		{
			this[_parent.TableNameColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field IndexAdvantage<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."IndexAdvantage"<br/>
		/// View field characteristics (type, precision, scale, length): Float, 38, 0, 0</remarks>
		public System.Double IndexAdvantage 
		{
			get { return IsIndexAdvantageNull() ? (System.Double)TypeDefaultValue.GetDefaultValue(typeof(System.Double)) : (System.Double)this[_parent.IndexAdvantageColumn]; }
			set { this[_parent.IndexAdvantageColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field IndexAdvantage is NULL, false otherwise.</summary>
		public bool IsIndexAdvantageNull() 
		{
			return IsNull(_parent.IndexAdvantageColumn);
		}

		/// <summary>Sets the TypedView field IndexAdvantage to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetIndexAdvantageNull() 
		{
			this[_parent.IndexAdvantageColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field GroupHandle<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."GroupHandle"<br/>
		/// View field characteristics (type, precision, scale, length): Int, 10, 0, 0</remarks>
		public System.Int32 GroupHandle 
		{
			get { return IsGroupHandleNull() ? (System.Int32)TypeDefaultValue.GetDefaultValue(typeof(System.Int32)) : (System.Int32)this[_parent.GroupHandleColumn]; }
			set { this[_parent.GroupHandleColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field GroupHandle is NULL, false otherwise.</summary>
		public bool IsGroupHandleNull() 
		{
			return IsNull(_parent.GroupHandleColumn);
		}

		/// <summary>Sets the TypedView field GroupHandle to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetGroupHandleNull() 
		{
			this[_parent.GroupHandleColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field ColumnID<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."ColumnID"<br/>
		/// View field characteristics (type, precision, scale, length): Int, 10, 0, 0</remarks>
		public System.Int32 ColumnID 
		{
			get { return IsColumnIDNull() ? (System.Int32)TypeDefaultValue.GetDefaultValue(typeof(System.Int32)) : (System.Int32)this[_parent.ColumnIDColumn]; }
			set { this[_parent.ColumnIDColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field ColumnID is NULL, false otherwise.</summary>
		public bool IsColumnIDNull() 
		{
			return IsNull(_parent.ColumnIDColumn);
		}

		/// <summary>Sets the TypedView field ColumnID to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetColumnIDNull() 
		{
			this[_parent.ColumnIDColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field ColumnName<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."ColumnName"<br/>
		/// View field characteristics (type, precision, scale, length): NVarChar, 0, 0, 4000</remarks>
		public System.String ColumnName 
		{
			get { return IsColumnNameNull() ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : (System.String)this[_parent.ColumnNameColumn]; }
			set { this[_parent.ColumnNameColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field ColumnName is NULL, false otherwise.</summary>
		public bool IsColumnNameNull() 
		{
			return IsNull(_parent.ColumnNameColumn);
		}

		/// <summary>Sets the TypedView field ColumnName to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetColumnNameNull() 
		{
			this[_parent.ColumnNameColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field ColumnUsage<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksMissingIndexRequests"."ColumnUsage"<br/>
		/// View field characteristics (type, precision, scale, length): NVarChar, 0, 0, 4000</remarks>
		public System.String ColumnUsage 
		{
			get { return IsColumnUsageNull() ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : (System.String)this[_parent.ColumnUsageColumn]; }
			set { this[_parent.ColumnUsageColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field ColumnUsage is NULL, false otherwise.</summary>
		public bool IsColumnUsageNull() 
		{
			return IsNull(_parent.ColumnUsageColumn);
		}

		/// <summary>Sets the TypedView field ColumnUsage to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetColumnUsageNull() 
		{
			this[_parent.ColumnUsageColumn] = System.Convert.DBNull;
		}
		#endregion
		
		#region Custom Typed View Row Code
		
		// __LLBLGENPRO_USER_CODE_REGION_START CustomTypedViewRowCode
		// __LLBLGENPRO_USER_CODE_REGION_END
		#endregion
		
		#region Included Row Code

		#endregion	
	}
}
