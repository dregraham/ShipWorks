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
    /// Entity interface which represents the entity 'ServerMessage'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IServerMessageEntity
    {
        
        /// <summary> The ServerMessageID property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."ServerMessageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ServerMessageID { get; }
        /// <summary> The RowVersion property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Number property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Number"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Number { get; }
        /// <summary> The Published property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Published"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Published { get; }
        /// <summary> The Active property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Active"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Active { get; }
        /// <summary> The Dismissable property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Dismissable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Dismissable { get; }
        /// <summary> The Expires property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Expires"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> Expires { get; }
        /// <summary> The ResponseTo property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."ResponseTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ResponseTo { get; }
        /// <summary> The ResponseAction property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."ResponseAction"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ResponseAction { get; }
        /// <summary> The EditTo property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."EditTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> EditTo { get; }
        /// <summary> The Image property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Image"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Image { get; }
        /// <summary> The PrimaryText property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."PrimaryText"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PrimaryText { get; }
        /// <summary> The SecondaryText property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."SecondaryText"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SecondaryText { get; }
        /// <summary> The Actions property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Actions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Actions { get; }
        /// <summary> The Stores property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Stores"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Stores { get; }
        /// <summary> The Shippers property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Shippers"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Shippers { get; }
        
        
        
        IEnumerable<IServerMessageSignoffEntity> Signoffs { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IServerMessageEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IServerMessageEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ServerMessage'. <br/><br/>
    /// 
    /// </summary>
    public partial class ServerMessageEntity : IServerMessageEntity
    {
        
        
        IEnumerable<IServerMessageSignoffEntity> IServerMessageEntity.Signoffs => Signoffs;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServerMessageEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IServerMessageEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IServerMessageEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyServerMessageEntity(this, objectMap);
        }
    }
}
