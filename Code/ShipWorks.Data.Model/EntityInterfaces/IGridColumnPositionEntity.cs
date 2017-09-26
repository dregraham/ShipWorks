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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'GridColumnPosition'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGridColumnPositionEntity
    {
        
        /// <summary> The GridColumnPositionID property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."GridColumnPositionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 GridColumnPositionID { get; }
        /// <summary> The GridColumnLayoutID property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 GridColumnLayoutID { get; }
        /// <summary> The ColumnGuid property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."ColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid ColumnGuid { get; }
        /// <summary> The Visible property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."Visible"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Visible { get; }
        /// <summary> The Width property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."Width"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Width { get; }
        /// <summary> The Position property of the Entity GridColumnPosition<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnPosition"."Position"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Position { get; }
        
        
        IGridColumnLayoutEntity GridColumnLayout { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGridColumnPositionEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGridColumnPositionEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GridColumnPosition'. <br/><br/>
    /// 
    /// </summary>
    public partial class GridColumnPositionEntity : IGridColumnPositionEntity
    {
        
        IGridColumnLayoutEntity IGridColumnPositionEntity.GridColumnLayout => GridColumnLayout;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnPositionEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IGridColumnPositionEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGridColumnPositionEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGridColumnPositionEntity(this, objectMap);
        }

        
    }
}
