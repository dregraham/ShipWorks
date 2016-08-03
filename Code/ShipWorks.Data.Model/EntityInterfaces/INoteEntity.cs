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
    /// Entity interface which represents the entity 'Note'. <br/><br/>
    /// 
    /// </summary>
    public partial interface INoteEntity
    {
        
        /// <summary> The NoteID property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."NoteID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 NoteID { get; }
        /// <summary> The RowVersion property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ObjectID property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ObjectID { get; }
        /// <summary> The UserID property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> UserID { get; }
        /// <summary> The Edited property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Edited"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Edited { get; }
        /// <summary> The Text property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Text"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Text { get; }
        /// <summary> The Source property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Source"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Source { get; }
        /// <summary> The Visibility property of the Entity Note<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Note"."Visibility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Visibility { get; }
        
        
        IOrderEntity Order { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        INoteEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        INoteEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Note'. <br/><br/>
    /// 
    /// </summary>
    public partial class NoteEntity : INoteEntity
    {
        
        IOrderEntity INoteEntity.Order => Order;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual INoteEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual INoteEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (INoteEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyNoteEntity(this, objectMap);
        }
    }
}
