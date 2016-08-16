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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'GridColumnFormat'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGridColumnFormatEntity
    {
        
        /// <summary> The GridColumnFormatID property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."GridColumnFormatID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 GridColumnFormatID { get; }
        /// <summary> The UserID property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The ColumnGuid property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."ColumnGuid"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid ColumnGuid { get; }
        /// <summary> The Settings property of the Entity GridColumnFormat<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GridColumnFormat"."Settings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Settings { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGridColumnFormatEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IGridColumnFormatEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GridColumnFormat'. <br/><br/>
    /// 
    /// </summary>
    public partial class GridColumnFormatEntity : IGridColumnFormatEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IGridColumnFormatEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IGridColumnFormatEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGridColumnFormatEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGridColumnFormatEntity(this, objectMap);
        }
    }
}
