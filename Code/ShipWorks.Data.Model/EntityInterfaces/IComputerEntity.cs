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
    /// Entity interface which represents the entity 'Computer'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IComputerEntity
    {
        
        /// <summary> The ComputerID property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The RowVersion property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The Identifier property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."Identifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid Identifier { get; }
        /// <summary> The Name property of the Entity Computer<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Computer"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        
        
        
        IEnumerable<IAuditEntity> Audit { get; }
        IEnumerable<IServiceStatusEntity> ServiceStatus { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IComputerEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IComputerEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Computer'. <br/><br/>
    /// 
    /// </summary>
    public partial class ComputerEntity : IComputerEntity
    {
        
        
        IEnumerable<IAuditEntity> IComputerEntity.Audit => Audit;
        IEnumerable<IServiceStatusEntity> IComputerEntity.ServiceStatus => ServiceStatus;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IComputerEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IComputerEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IComputerEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyComputerEntity(this, objectMap);
        }

        
    }
}
