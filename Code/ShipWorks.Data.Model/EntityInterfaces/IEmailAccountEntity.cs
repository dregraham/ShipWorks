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
    /// Entity interface which represents the entity 'EmailAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEmailAccountEntity
    {
        
        /// <summary> The EmailAccountID property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."EmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EmailAccountID { get; }
        /// <summary> The RowVersion property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The AccountName property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AccountName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AccountName { get; }
        /// <summary> The DisplayName property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."DisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DisplayName { get; }
        /// <summary> The EmailAddress property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."EmailAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailAddress { get; }
        /// <summary> The IncomingServer property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingServer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String IncomingServer { get; }
        /// <summary> The IncomingServerType property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingServerType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IncomingServerType { get; }
        /// <summary> The IncomingPort property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingPort"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IncomingPort { get; }
        /// <summary> The IncomingSecurityType property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingSecurityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IncomingSecurityType { get; }
        /// <summary> The IncomingUsername property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String IncomingUsername { get; }
        /// <summary> The IncomingPassword property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String IncomingPassword { get; }
        /// <summary> The OutgoingServer property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingServer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OutgoingServer { get; }
        /// <summary> The OutgoingPort property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingPort"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OutgoingPort { get; }
        /// <summary> The OutgoingSecurityType property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingSecurityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OutgoingSecurityType { get; }
        /// <summary> The OutgoingCredentialSource property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingCredentialSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OutgoingCredentialSource { get; }
        /// <summary> The OutgoingUsername property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OutgoingUsername { get; }
        /// <summary> The OutgoingPassword property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OutgoingPassword { get; }
        /// <summary> The AutoSend property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AutoSend"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AutoSend { get; }
        /// <summary> The AutoSendMinutes property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AutoSendMinutes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AutoSendMinutes { get; }
        /// <summary> The AutoSendLastTime property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AutoSendLastTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime AutoSendLastTime { get; }
        /// <summary> The LimitMessagesPerConnection property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerConnection"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean LimitMessagesPerConnection { get; }
        /// <summary> The LimitMessagesPerConnectionQuantity property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerConnectionQuantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LimitMessagesPerConnectionQuantity { get; }
        /// <summary> The LimitMessagesPerHour property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerHour"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean LimitMessagesPerHour { get; }
        /// <summary> The LimitMessagesPerHourQuantity property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerHourQuantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LimitMessagesPerHourQuantity { get; }
        /// <summary> The LimitMessageInterval property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessageInterval"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean LimitMessageInterval { get; }
        /// <summary> The LimitMessageIntervalSeconds property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessageIntervalSeconds"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LimitMessageIntervalSeconds { get; }
        /// <summary> The InternalOwnerID property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."InternalOwnerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> InternalOwnerID { get; }
        
        
        
        IEnumerable<IOrderMotionStoreEntity> OrderMotionStore { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEmailAccountEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEmailAccountEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EmailAccount'. <br/><br/>
    /// 
    /// </summary>
    public partial class EmailAccountEntity : IEmailAccountEntity
    {
        
        
        IEnumerable<IOrderMotionStoreEntity> IEmailAccountEntity.OrderMotionStore => OrderMotionStore;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailAccountEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEmailAccountEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEmailAccountEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEmailAccountEntity(this, objectMap);
        }
    }
}
