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
    /// Entity interface which represents the entity 'GridColumnLayout'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGridColumnLayoutEntity
    {
        
        /// <summary> The GridColumnLayoutID property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."GridColumnLayoutID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 GridColumnLayoutID { get; }
        /// <summary> The DefinitionSet property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DefinitionSet"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DefinitionSet { get; }
        /// <summary> The DefaultSortColumnGuid property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DefaultSortColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid DefaultSortColumnGuid { get; }
        /// <summary> The DefaultSortOrder property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DefaultSortOrder"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DefaultSortOrder { get; }
        /// <summary> The LastSortColumnGuid property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."LastSortColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid LastSortColumnGuid { get; }
        /// <summary> The LastSortOrder property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."LastSortOrder"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LastSortOrder { get; }
        /// <summary> The DetailViewSettings property of the Entity GridColumnLayout<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnLayout"."DetailViewSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DetailViewSettings { get; }
        
        
        
        IEnumerable<IGridColumnPositionEntity> GridColumnPositions { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGridColumnLayoutEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGridColumnLayoutEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GridColumnLayout'. <br/><br/>
    /// 
    /// </summary>
    public partial class GridColumnLayoutEntity : IGridColumnLayoutEntity
    {
        
        
        IEnumerable<IGridColumnPositionEntity> IGridColumnLayoutEntity.GridColumnPositions => GridColumnPositions;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnLayoutEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IGridColumnLayoutEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGridColumnLayoutEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGridColumnLayoutEntity(this, objectMap);
        }

        
    }
}
