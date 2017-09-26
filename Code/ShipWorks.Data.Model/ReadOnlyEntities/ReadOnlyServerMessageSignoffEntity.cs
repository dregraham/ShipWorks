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
    /// Read-only representation of the entity 'ServerMessageSignoff'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyServerMessageSignoffEntity : IServerMessageSignoffEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyServerMessageSignoffEntity(IServerMessageSignoffEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ServerMessageSignoffID = source.ServerMessageSignoffID;
            RowVersion = source.RowVersion;
            ServerMessageID = source.ServerMessageID;
            UserID = source.UserID;
            ComputerID = source.ComputerID;
            Dismissed = source.Dismissed;
            
            
            ServerMessage = (IServerMessageEntity) source.ServerMessage?.AsReadOnly(objectMap);
            

            CopyCustomServerMessageSignoffData(source);
        }

        
        /// <summary> The ServerMessageSignoffID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."ServerMessageSignoffID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ServerMessageSignoffID { get; }
        /// <summary> The RowVersion property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ServerMessageID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."ServerMessageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ServerMessageID { get; }
        /// <summary> The UserID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The Dismissed property of the Entity ServerMessageSignoff<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessageSignoff"."Dismissed"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Dismissed { get; }
        
        
        public IServerMessageEntity ServerMessage { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServerMessageSignoffEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServerMessageSignoffEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomServerMessageSignoffData(IServerMessageSignoffEntity source);
    }
}
