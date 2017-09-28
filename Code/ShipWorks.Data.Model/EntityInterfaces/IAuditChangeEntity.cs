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
    /// Entity interface which represents the entity 'AuditChange'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAuditChangeEntity
    {
        
        /// <summary> The AuditChangeID property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."AuditChangeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 AuditChangeID { get; }
        /// <summary> The AuditID property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."AuditID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 AuditID { get; }
        /// <summary> The ChangeType property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."ChangeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ChangeType { get; }
        /// <summary> The EntityID property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EntityID { get; }
        
        
        IAuditEntity Audit { get; }
        
        IEnumerable<IAuditChangeDetailEntity> AuditChangeDetails { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAuditChangeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAuditChangeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AuditChange'. <br/><br/>
    /// 
    /// </summary>
    public partial class AuditChangeEntity : IAuditChangeEntity
    {
        
        IAuditEntity IAuditChangeEntity.Audit => Audit;
        
        IEnumerable<IAuditChangeDetailEntity> IAuditChangeEntity.AuditChangeDetails => AuditChangeDetails;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditChangeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAuditChangeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAuditChangeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAuditChangeEntity(this, objectMap);
        }

        
    }
}
