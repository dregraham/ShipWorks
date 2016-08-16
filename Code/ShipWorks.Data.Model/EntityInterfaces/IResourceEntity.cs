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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'Resource'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IResourceEntity
    {
        
        /// <summary> The ResourceID property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."ResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ResourceID { get; }
        /// <summary> The Data property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Data"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] Data { get; }
        /// <summary> The Checksum property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Checksum"<br/>
        /// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] Checksum { get; }
        /// <summary> The Compressed property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Compressed"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Compressed { get; }
        /// <summary> The Filename property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Filename"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Filename { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IResourceEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IResourceEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Resource'. <br/><br/>
    /// 
    /// </summary>
    public partial class ResourceEntity : IResourceEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IResourceEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IResourceEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IResourceEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyResourceEntity(this, objectMap);
        }
    }
}
