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
    /// Entity interface which represents the entity 'Audit'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAuditEntity
    {
        
        /// <summary> The AuditID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."AuditID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 AuditID { get; }
        /// <summary> The RowVersion property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The TransactionID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TransactionID { get; }
        /// <summary> The UserID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The ComputerID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The Reason property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."Reason"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Reason { get; }
        /// <summary> The ReasonDetail property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."ReasonDetail"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ReasonDetail { get; }
        /// <summary> The Date property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."Date"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime Date { get; }
        /// <summary> The Action property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."Action"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Action { get; }
        /// <summary> The EntityID property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> EntityID { get; }
        /// <summary> The HasEvents property of the Entity Audit<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Audit"."HasEvents"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean HasEvents { get; }
        
        
        IComputerEntity Computer { get; }
        IUserEntity User { get; }
        
        IEnumerable<IAuditChangeEntity> AuditChanges { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAuditEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAuditEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Audit'. <br/><br/>
    /// 
    /// </summary>
    public partial class AuditEntity : IAuditEntity
    {
        
        IComputerEntity IAuditEntity.Computer => Computer;
        IUserEntity IAuditEntity.User => User;
        
        IEnumerable<IAuditChangeEntity> IAuditEntity.AuditChanges => AuditChanges;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAuditEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAuditEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAuditEntity(this, objectMap);
        }

        
    }
}
