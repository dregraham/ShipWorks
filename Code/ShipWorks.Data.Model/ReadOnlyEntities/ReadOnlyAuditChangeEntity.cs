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
    /// Read-only representation of the entity 'AuditChange'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAuditChangeEntity : IAuditChangeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAuditChangeEntity(IAuditChangeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AuditChangeID = source.AuditChangeID;
            AuditID = source.AuditID;
            ChangeType = source.ChangeType;
            ObjectID = source.ObjectID;
            
            
            Audit = source.Audit?.AsReadOnly(objectMap);
            
            AuditChangeDetails = source.AuditChangeDetails?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IAuditChangeDetailEntity>();

            CopyCustomAuditChangeData(source);
        }

        
        /// <summary> The AuditChangeID property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."AuditChangeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 AuditChangeID { get; }
        /// <summary> The AuditID property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."AuditID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AuditID { get; }
        /// <summary> The ChangeType property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."ChangeType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ChangeType { get; }
        /// <summary> The ObjectID property of the Entity AuditChange<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChange"."ObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ObjectID { get; }
        
        
        public IAuditEntity Audit { get; }
        
        
        public IEnumerable<IAuditChangeDetailEntity> AuditChangeDetails { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditChangeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditChangeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAuditChangeData(IAuditChangeEntity source);
    }
}
