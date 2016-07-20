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
    /// Entity interface which represents the entity 'Resource'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyResourceEntity : IResourceEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyResourceEntity(IResourceEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ResourceID = source.ResourceID;
            Data = source.Data;
            Checksum = source.Checksum;
            Compressed = source.Compressed;
            Filename = source.Filename;
            
            
            

            CopyCustomResourceData(source);
        }

        
        /// <summary> The ResourceID property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."ResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ResourceID { get; }
        /// <summary> The Data property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Data"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] Data { get; }
        /// <summary> The Checksum property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Checksum"<br/>
        /// Table field type characteristics (type, precision, scale, length): Binary, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] Checksum { get; }
        /// <summary> The Compressed property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Compressed"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Compressed { get; }
        /// <summary> The Filename property of the Entity Resource<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Resource"."Filename"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Filename { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IResourceEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IResourceEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomResourceData(IResourceEntity source);
    }
}
