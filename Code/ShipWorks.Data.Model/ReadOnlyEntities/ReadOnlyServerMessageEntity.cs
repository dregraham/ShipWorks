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
    /// Read-only representation of the entity 'ServerMessage'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyServerMessageEntity : IServerMessageEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyServerMessageEntity(IServerMessageEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ServerMessageID = source.ServerMessageID;
            RowVersion = source.RowVersion;
            Number = source.Number;
            Published = source.Published;
            Active = source.Active;
            Dismissable = source.Dismissable;
            Expires = source.Expires;
            ResponseTo = source.ResponseTo;
            ResponseAction = source.ResponseAction;
            EditTo = source.EditTo;
            Image = source.Image;
            PrimaryText = source.PrimaryText;
            SecondaryText = source.SecondaryText;
            Actions = source.Actions;
            Stores = source.Stores;
            Shippers = source.Shippers;
            
            
            
            Signoffs = source.Signoffs?.Select(x => x.AsReadOnly(objectMap)).OfType<IServerMessageSignoffEntity>().ToReadOnly() ??
                Enumerable.Empty<IServerMessageSignoffEntity>();

            CopyCustomServerMessageData(source);
        }

        
        /// <summary> The ServerMessageID property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."ServerMessageID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ServerMessageID { get; }
        /// <summary> The RowVersion property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Number property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Number"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Number { get; }
        /// <summary> The Published property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Published"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Published { get; }
        /// <summary> The Active property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Active"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Active { get; }
        /// <summary> The Dismissable property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Dismissable"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Dismissable { get; }
        /// <summary> The Expires property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Expires"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> Expires { get; }
        /// <summary> The ResponseTo property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."ResponseTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ResponseTo { get; }
        /// <summary> The ResponseAction property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."ResponseAction"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ResponseAction { get; }
        /// <summary> The EditTo property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."EditTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> EditTo { get; }
        /// <summary> The Image property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Image"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Image { get; }
        /// <summary> The PrimaryText property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."PrimaryText"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PrimaryText { get; }
        /// <summary> The SecondaryText property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."SecondaryText"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 60<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SecondaryText { get; }
        /// <summary> The Actions property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Actions"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Actions { get; }
        /// <summary> The Stores property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Stores"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Stores { get; }
        /// <summary> The Shippers property of the Entity ServerMessage<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ServerMessage"."Shippers"<br/>
        /// Table field type characteristics (type, precision, scale, length): NText, 0, 0, 1073741823<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Shippers { get; }
        
        
        
        public IEnumerable<IServerMessageSignoffEntity> Signoffs { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServerMessageEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IServerMessageEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomServerMessageData(IServerMessageEntity source);
    }
}
