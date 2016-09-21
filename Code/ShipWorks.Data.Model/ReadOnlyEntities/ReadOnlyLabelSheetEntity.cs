///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 5.0
// Code is generated on: 
// Code is generated using templates: ShipWorks
// Templates vendor: Solutions Design.
// Templates version: 
//////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'LabelSheet'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyLabelSheetEntity : ILabelSheetEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyLabelSheetEntity(ILabelSheetEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            LabelSheetID = source.LabelSheetID;
            RowVersion = source.RowVersion;
            Name = source.Name;
            PaperSizeHeight = source.PaperSizeHeight;
            PaperSizeWidth = source.PaperSizeWidth;
            MarginTop = source.MarginTop;
            MarginLeft = source.MarginLeft;
            LabelHeight = source.LabelHeight;
            LabelWidth = source.LabelWidth;
            VerticalSpacing = source.VerticalSpacing;
            HorizontalSpacing = source.HorizontalSpacing;
            Rows = source.Rows;
            Columns = source.Columns;
            
            
            

            CopyCustomLabelSheetData(source);
        }

        
        /// <summary> The LabelSheetID property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."LabelSheetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 LabelSheetID { get; }
        /// <summary> The RowVersion property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Name property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The PaperSizeHeight property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."PaperSizeHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PaperSizeHeight { get; }
        /// <summary> The PaperSizeWidth property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."PaperSizeWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PaperSizeWidth { get; }
        /// <summary> The MarginTop property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."MarginTop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double MarginTop { get; }
        /// <summary> The MarginLeft property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."MarginLeft"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double MarginLeft { get; }
        /// <summary> The LabelHeight property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."LabelHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double LabelHeight { get; }
        /// <summary> The LabelWidth property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."LabelWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double LabelWidth { get; }
        /// <summary> The VerticalSpacing property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."VerticalSpacing"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double VerticalSpacing { get; }
        /// <summary> The HorizontalSpacing property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."HorizontalSpacing"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double HorizontalSpacing { get; }
        /// <summary> The Rows property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."Rows"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Rows { get; }
        /// <summary> The Columns property of the Entity LabelSheet<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "LabelSheet"."Columns"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Columns { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ILabelSheetEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ILabelSheetEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomLabelSheetData(ILabelSheetEntity source);
    }
}
