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
    /// Entity interface which represents the entity 'ServerMessageSignoff'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IServerMessageSignoffEntity
    {
        
        /// <summary> The ServerMessageSignoffID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."ServerMessageSignoffID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ServerMessageSignoffID { get; }
        /// <summary> The RowVersion property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ServerMessageID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."ServerMessageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ServerMessageID { get; }
        /// <summary> The UserID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The Dismissed property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."Dismissed"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Dismissed { get; }
        
        
        IServerMessageEntity ServerMessage { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IServerMessageSignoffEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IServerMessageSignoffEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ServerMessageSignoff'. <br/><br/>
    /// 
    /// </summary>
    public partial class ServerMessageSignoffEntity : IServerMessageSignoffEntity
    {
        
        IServerMessageEntity IServerMessageSignoffEntity.ServerMessage => ServerMessage;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServerMessageSignoffEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IServerMessageSignoffEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IServerMessageSignoffEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyServerMessageSignoffEntity(this, objectMap);
        }

        
    }
}
