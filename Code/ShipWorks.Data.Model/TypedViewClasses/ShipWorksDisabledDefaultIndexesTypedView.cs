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
	
	
	/// <summary>Typed datatable for the view 'ShipWorksDisabledDefaultIndexes'.<br/><br/></summary>
	[Serializable, System.ComponentModel.DesignerCategory("Code")]
	[ToolboxItem(true)]
	[DesignTimeVisible(true)]
	public partial class ShipWorksDisabledDefaultIndexesTypedView : TypedViewBase<ShipWorksDisabledDefaultIndexesRow>, ITypedView2
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfacesView
		// __LLBLGENPRO_USER_CODE_REGION_END
			
	{
		#region Class Member Declarations
		private DataColumn _columnTableName;
		private DataColumn _columnIndexName;
		private DataColumn _columnColumnName;
		private DataColumn _columnEnableIndex;
		private DataColumn _columnIndexID;
		private DataColumn _columnIndexColumnId;
		private DataColumn _columnIsIncluded;
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
		static ShipWorksDisabledDefaultIndexesTypedView()
		{
			SetupCustomPropertyHashtables();
		}

		/// <summary>CTor</summary>
		public ShipWorksDisabledDefaultIndexesTypedView():base("ShipWorksDisabledDefaultIndexes")
		{
			InitClass();
		}
#if !CF	
		/// <summary>Protected constructor for deserialization.</summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
		protected ShipWorksDisabledDefaultIndexesTypedView(SerializationInfo info, StreamingContext context):base(info, context)
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
			return new ShipWorksDisabledDefaultIndexesRow(rowBuilder);
		}

		/// <summary>Initializes the hashtables for the typed view type and typed view field custom properties. </summary>
		private static void SetupCustomPropertyHashtables()
		{
			_customProperties = new Hashtable();
			_fieldsCustomProperties = new Hashtable();
			Hashtable fieldHashtable;
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("TableName", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("IndexName", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("ColumnName", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("EnableIndex", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("IndexID", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("IndexColumnId", fieldHashtable);
			fieldHashtable = new Hashtable();
			_fieldsCustomProperties.Add("IsIncluded", fieldHashtable);
		}

		/// <summary>
		/// Initialize the datastructures.
		/// </summary>
		protected override void InitClass()
		{
			TableName = "ShipWorksDisabledDefaultIndexes";		
			_columnTableName = GeneralUtils.CreateTypedDataTableColumn("TableName", @"TableName", typeof(System.String), this.Columns);
			_columnIndexName = GeneralUtils.CreateTypedDataTableColumn("IndexName", @"IndexName", typeof(System.String), this.Columns);
			_columnColumnName = GeneralUtils.CreateTypedDataTableColumn("ColumnName", @"ColumnName", typeof(System.String), this.Columns);
			_columnEnableIndex = GeneralUtils.CreateTypedDataTableColumn("EnableIndex", @"EnableIndex", typeof(System.String), this.Columns);
			_columnIndexID = GeneralUtils.CreateTypedDataTableColumn("IndexID", @"IndexID", typeof(System.Int32), this.Columns);
			_columnIndexColumnId = GeneralUtils.CreateTypedDataTableColumn("IndexColumnId", @"IndexColumnId", typeof(System.Int32), this.Columns);
			_columnIsIncluded = GeneralUtils.CreateTypedDataTableColumn("IsIncluded", @"IsIncluded", typeof(System.Boolean), this.Columns);
			_fields = EntityFieldsFactory.CreateTypedViewEntityFieldsObject(TypedViewType.ShipWorksDisabledDefaultIndexesTypedView);
			
			// __LLBLGENPRO_USER_CODE_REGION_START AdditionalFields
			// be sure to call _fields.Expand(number of new fields) first. 
			// __LLBLGENPRO_USER_CODE_REGION_END
			
			OnInitialized();
		}

		/// <summary>Initializes the members, after a clone action.</summary>
		private void InitMembers()
		{
			_columnTableName = this.Columns["TableName"];
			_columnIndexName = this.Columns["IndexName"];
			_columnColumnName = this.Columns["ColumnName"];
			_columnEnableIndex = this.Columns["EnableIndex"];
			_columnIndexID = this.Columns["IndexID"];
			_columnIndexColumnId = this.Columns["IndexColumnId"];
			_columnIsIncluded = this.Columns["IsIncluded"];
			_fields = EntityFieldsFactory.CreateTypedViewEntityFieldsObject(TypedViewType.ShipWorksDisabledDefaultIndexesTypedView);
			// __LLBLGENPRO_USER_CODE_REGION_START InitMembers
			// __LLBLGENPRO_USER_CODE_REGION_END
			
		}

		/// <summary>Clones this instance.</summary>
		/// <returns>A clone of this instance</returns>
		public override DataTable Clone() 
		{
			ShipWorksDisabledDefaultIndexesTypedView cloneToReturn = ((ShipWorksDisabledDefaultIndexesTypedView)(base.Clone()));
			cloneToReturn.InitMembers();
			return cloneToReturn;
		}
#if !CF			
		/// <summary>Creates a new instance of the DataTable class.</summary>
		/// <returns>a new instance of a datatable with this schema.</returns>
		protected override DataTable CreateInstance() 
		{
			return new ShipWorksDisabledDefaultIndexesTypedView();
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
			get { return ShipWorksDisabledDefaultIndexesTypedView.CustomProperties;}
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
			get { return ShipWorksDisabledDefaultIndexesTypedView.FieldsCustomProperties;}
		}

		/// <summary>Returns the column object belonging to the TypedView field TableName</summary>
		internal DataColumn TableNameColumn 
		{
			get { return _columnTableName; }
		}

		/// <summary>Returns the column object belonging to the TypedView field IndexName</summary>
		internal DataColumn IndexNameColumn 
		{
			get { return _columnIndexName; }
		}

		/// <summary>Returns the column object belonging to the TypedView field ColumnName</summary>
		internal DataColumn ColumnNameColumn 
		{
			get { return _columnColumnName; }
		}

		/// <summary>Returns the column object belonging to the TypedView field EnableIndex</summary>
		internal DataColumn EnableIndexColumn 
		{
			get { return _columnEnableIndex; }
		}

		/// <summary>Returns the column object belonging to the TypedView field IndexID</summary>
		internal DataColumn IndexIDColumn 
		{
			get { return _columnIndexID; }
		}

		/// <summary>Returns the column object belonging to the TypedView field IndexColumnId</summary>
		internal DataColumn IndexColumnIdColumn 
		{
			get { return _columnIndexColumnId; }
		}

		/// <summary>Returns the column object belonging to the TypedView field IsIncluded</summary>
		internal DataColumn IsIncludedColumn 
		{
			get { return _columnIsIncluded; }
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

	/// <summary>Typed datarow for the typed datatable ShipWorksDisabledDefaultIndexes</summary>
	public partial class ShipWorksDisabledDefaultIndexesRow : DataRow
		// __LLBLGENPRO_USER_CODE_REGION_START AdditionalInterfacesRow
		// __LLBLGENPRO_USER_CODE_REGION_END
			
	{
		#region Class Member Declarations
		private ShipWorksDisabledDefaultIndexesTypedView	_parent;
		#endregion

		/// <summary>CTor</summary>
		/// <param name="rowBuilder">Row builder object to use when building this row</param>
		protected internal ShipWorksDisabledDefaultIndexesRow(DataRowBuilder rowBuilder) : base(rowBuilder) 
		{
			_parent = ((ShipWorksDisabledDefaultIndexesTypedView)(this.Table));
		}

		#region Class Property Declarations

		/// <summary>Gets / sets the value of the TypedView field TableName<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."TableName"<br/>
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

		/// <summary>Gets / sets the value of the TypedView field IndexName<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."IndexName"<br/>
		/// View field characteristics (type, precision, scale, length): NVarChar, 0, 0, 128</remarks>
		public System.String IndexName 
		{
			get { return IsIndexNameNull() ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : (System.String)this[_parent.IndexNameColumn]; }
			set { this[_parent.IndexNameColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field IndexName is NULL, false otherwise.</summary>
		public bool IsIndexNameNull() 
		{
			return IsNull(_parent.IndexNameColumn);
		}

		/// <summary>Sets the TypedView field IndexName to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetIndexNameNull() 
		{
			this[_parent.IndexNameColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field ColumnName<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."ColumnName"<br/>
		/// View field characteristics (type, precision, scale, length): NVarChar, 0, 0, 128</remarks>
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

		/// <summary>Gets / sets the value of the TypedView field EnableIndex<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."EnableIndex"<br/>
		/// View field characteristics (type, precision, scale, length): NVarChar, 0, 0, 825</remarks>
		public System.String EnableIndex 
		{
			get { return IsEnableIndexNull() ? (System.String)TypeDefaultValue.GetDefaultValue(typeof(System.String)) : (System.String)this[_parent.EnableIndexColumn]; }
			set { this[_parent.EnableIndexColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field EnableIndex is NULL, false otherwise.</summary>
		public bool IsEnableIndexNull() 
		{
			return IsNull(_parent.EnableIndexColumn);
		}

		/// <summary>Sets the TypedView field EnableIndex to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetEnableIndexNull() 
		{
			this[_parent.EnableIndexColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field IndexID<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."IndexID"<br/>
		/// View field characteristics (type, precision, scale, length): Int, 10, 0, 0</remarks>
		public System.Int32 IndexID 
		{
			get { return IsIndexIDNull() ? (System.Int32)TypeDefaultValue.GetDefaultValue(typeof(System.Int32)) : (System.Int32)this[_parent.IndexIDColumn]; }
			set { this[_parent.IndexIDColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field IndexID is NULL, false otherwise.</summary>
		public bool IsIndexIDNull() 
		{
			return IsNull(_parent.IndexIDColumn);
		}

		/// <summary>Sets the TypedView field IndexID to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetIndexIDNull() 
		{
			this[_parent.IndexIDColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field IndexColumnId<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."IndexColumnId"<br/>
		/// View field characteristics (type, precision, scale, length): Int, 10, 0, 0</remarks>
		public System.Int32 IndexColumnId 
		{
			get { return IsIndexColumnIdNull() ? (System.Int32)TypeDefaultValue.GetDefaultValue(typeof(System.Int32)) : (System.Int32)this[_parent.IndexColumnIdColumn]; }
			set { this[_parent.IndexColumnIdColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field IndexColumnId is NULL, false otherwise.</summary>
		public bool IsIndexColumnIdNull() 
		{
			return IsNull(_parent.IndexColumnIdColumn);
		}

		/// <summary>Sets the TypedView field IndexColumnId to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetIndexColumnIdNull() 
		{
			this[_parent.IndexColumnIdColumn] = System.Convert.DBNull;
		}

		/// <summary>Gets / sets the value of the TypedView field IsIncluded<br/><br/></summary>
		/// <remarks>Mapped on view field: "ShipWorksDisabledDefaultIndexes"."IsIncluded"<br/>
		/// View field characteristics (type, precision, scale, length): Bit, 0, 0, 0</remarks>
		public System.Boolean IsIncluded 
		{
			get { return IsIsIncludedNull() ? (System.Boolean)TypeDefaultValue.GetDefaultValue(typeof(System.Boolean)) : (System.Boolean)this[_parent.IsIncludedColumn]; }
			set { this[_parent.IsIncludedColumn] = value; }
		}

		/// <summary>Returns true if the TypedView field IsIncluded is NULL, false otherwise.</summary>
		public bool IsIsIncludedNull() 
		{
			return IsNull(_parent.IsIncludedColumn);
		}

		/// <summary>Sets the TypedView field IsIncluded to NULL. Not recommended; a typed list should be used as a readonly object.</summary>
    	public void SetIsIncludedNull() 
		{
			this[_parent.IsIncludedColumn] = System.Convert.DBNull;
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
