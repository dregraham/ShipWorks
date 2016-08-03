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
    /// Read-only representation of the entity 'Audit'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAuditEntity : IAuditEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAuditEntity(IAuditEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AuditID = source.AuditID;
            RowVersion = source.RowVersion;
            TransactionID = source.TransactionID;
            UserID = source.UserID;
            ComputerID = source.ComputerID;
            Reason = source.Reason;
            ReasonDetail = source.ReasonDetail;
            Date = source.Date;
            Action = source.Action;
            ObjectID = source.ObjectID;
            HasEvents = source.HasEvents;
            
            
            Computer = source.Computer?.AsReadOnly(objectMap);
            User = source.User?.AsReadOnly(objectMap);
            
            AuditChanges = source.AuditChanges?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IAuditChangeEntity>();

            CopyCustomAuditData(source);
        }

        
        /// <summary> The AuditID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."AuditID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 AuditID { get; }
        /// <summary> The RowVersion property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The TransactionID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TransactionID { get; }
        /// <summary> The UserID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The Reason property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."Reason"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Reason { get; }
        /// <summary> The ReasonDetail property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."ReasonDetail"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReasonDetail { get; }
        /// <summary> The Date property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."Date"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime Date { get; }
        /// <summary> The Action property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."Action"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Action { get; }
        /// <summary> The ObjectID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ObjectID { get; }
        /// <summary> The HasEvents property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."HasEvents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean HasEvents { get; }
        
        
        public IComputerEntity Computer { get; }
        
        public IUserEntity User { get; }
        
        
        public IEnumerable<IAuditChangeEntity> AuditChanges { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAuditData(IAuditEntity source);
    }
}
