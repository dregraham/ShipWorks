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
    /// Read-only representation of the entity 'EmailAccount'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEmailAccountEntity : IEmailAccountEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEmailAccountEntity(IEmailAccountEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EmailAccountID = source.EmailAccountID;
            RowVersion = source.RowVersion;
            AccountName = source.AccountName;
            DisplayName = source.DisplayName;
            EmailAddress = source.EmailAddress;
            IncomingServer = source.IncomingServer;
            IncomingServerType = source.IncomingServerType;
            IncomingPort = source.IncomingPort;
            IncomingSecurityType = source.IncomingSecurityType;
            IncomingUsername = source.IncomingUsername;
            IncomingPassword = source.IncomingPassword;
            OutgoingServer = source.OutgoingServer;
            OutgoingPort = source.OutgoingPort;
            OutgoingSecurityType = source.OutgoingSecurityType;
            OutgoingCredentialSource = source.OutgoingCredentialSource;
            OutgoingUsername = source.OutgoingUsername;
            OutgoingPassword = source.OutgoingPassword;
            AutoSend = source.AutoSend;
            AutoSendMinutes = source.AutoSendMinutes;
            AutoSendLastTime = source.AutoSendLastTime;
            LimitMessagesPerConnection = source.LimitMessagesPerConnection;
            LimitMessagesPerConnectionQuantity = source.LimitMessagesPerConnectionQuantity;
            LimitMessagesPerHour = source.LimitMessagesPerHour;
            LimitMessagesPerHourQuantity = source.LimitMessagesPerHourQuantity;
            LimitMessageInterval = source.LimitMessageInterval;
            LimitMessageIntervalSeconds = source.LimitMessageIntervalSeconds;
            InternalOwnerID = source.InternalOwnerID;
            
            
            
            OrderMotionStore = source.OrderMotionStore?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IOrderMotionStoreEntity>();

            CopyCustomEmailAccountData(source);
        }

        
        /// <summary> The EmailAccountID property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."EmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EmailAccountID { get; }
        /// <summary> The RowVersion property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The AccountName property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AccountName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AccountName { get; }
        /// <summary> The DisplayName property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."DisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DisplayName { get; }
        /// <summary> The EmailAddress property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."EmailAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailAddress { get; }
        /// <summary> The IncomingServer property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingServer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IncomingServer { get; }
        /// <summary> The IncomingServerType property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingServerType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IncomingServerType { get; }
        /// <summary> The IncomingPort property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingPort"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IncomingPort { get; }
        /// <summary> The IncomingSecurityType property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingSecurityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IncomingSecurityType { get; }
        /// <summary> The IncomingUsername property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IncomingUsername { get; }
        /// <summary> The IncomingPassword property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."IncomingPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String IncomingPassword { get; }
        /// <summary> The OutgoingServer property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingServer"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OutgoingServer { get; }
        /// <summary> The OutgoingPort property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingPort"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OutgoingPort { get; }
        /// <summary> The OutgoingSecurityType property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingSecurityType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OutgoingSecurityType { get; }
        /// <summary> The OutgoingCredentialSource property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingCredentialSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OutgoingCredentialSource { get; }
        /// <summary> The OutgoingUsername property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OutgoingUsername { get; }
        /// <summary> The OutgoingPassword property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."OutgoingPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OutgoingPassword { get; }
        /// <summary> The AutoSend property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AutoSend"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean AutoSend { get; }
        /// <summary> The AutoSendMinutes property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AutoSendMinutes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AutoSendMinutes { get; }
        /// <summary> The AutoSendLastTime property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."AutoSendLastTime"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime AutoSendLastTime { get; }
        /// <summary> The LimitMessagesPerConnection property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerConnection"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean LimitMessagesPerConnection { get; }
        /// <summary> The LimitMessagesPerConnectionQuantity property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerConnectionQuantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LimitMessagesPerConnectionQuantity { get; }
        /// <summary> The LimitMessagesPerHour property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerHour"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean LimitMessagesPerHour { get; }
        /// <summary> The LimitMessagesPerHourQuantity property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessagesPerHourQuantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LimitMessagesPerHourQuantity { get; }
        /// <summary> The LimitMessageInterval property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessageInterval"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean LimitMessageInterval { get; }
        /// <summary> The LimitMessageIntervalSeconds property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."LimitMessageIntervalSeconds"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LimitMessageIntervalSeconds { get; }
        /// <summary> The InternalOwnerID property of the Entity EmailAccount<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailAccount"."InternalOwnerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> InternalOwnerID { get; }
        
        
        
        public IEnumerable<IOrderMotionStoreEntity> OrderMotionStore { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailAccountEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailAccountEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEmailAccountData(IEmailAccountEntity source);
    }
}
