///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'GridColumnPosition'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGridColumnPositionEntity : IGridColumnPositionEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGridColumnPositionEntity(IGridColumnPositionEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            GridColumnPositionID = source.GridColumnPositionID;
            GridColumnLayoutID = source.GridColumnLayoutID;
            ColumnGuid = source.ColumnGuid;
            Visible = source.Visible;
            Width = source.Width;
            Position = source.Position;
            
            
            GridColumnLayout = source.GridColumnLayout?.AsReadOnly(objectMap);
            

            CopyCustomGridColumnPositionData(source);
        }

        
        /// <summary> The GridColumnPositionID property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."GridColumnPositionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 GridColumnPositionID { get; }
        /// <summary> The GridColumnLayoutID property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 GridColumnLayoutID { get; }
        /// <summary> The ColumnGuid property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."ColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid ColumnGuid { get; }
        /// <summary> The Visible property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."Visible"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Visible { get; }
        /// <summary> The Width property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Width { get; }
        /// <summary> The Position property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."Position"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Position { get; }
        
        
        public IGridColumnLayoutEntity GridColumnLayout { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnPositionEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnPositionEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGridColumnPositionData(IGridColumnPositionEntity source);
    }
}
