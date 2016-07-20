///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Read-only representation of the entity 'Computer'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyComputerEntity : IComputerEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyComputerEntity(IComputerEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ComputerID = source.ComputerID;
            RowVersion = source.RowVersion;
            Identifier = source.Identifier;
            Name = source.Name;
            
            
            
            Audit = source.Audit?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IAuditEntity>();
            ServiceStatus = source.ServiceStatus?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IServiceStatusEntity>();

            CopyCustomComputerData(source);
        }

        
        /// <summary> The ComputerID property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The RowVersion property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The Identifier property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."Identifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid Identifier { get; }
        /// <summary> The Name property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        
        
        
        public IEnumerable<IAuditEntity> Audit { get; }
        
        public IEnumerable<IServiceStatusEntity> ServiceStatus { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IComputerEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IComputerEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomComputerData(IComputerEntity source);
    }
}
