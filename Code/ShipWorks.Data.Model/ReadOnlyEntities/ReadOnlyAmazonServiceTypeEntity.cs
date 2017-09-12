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
    /// Read-only representation of the entity 'AmazonServiceType'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonServiceTypeEntity : IAmazonServiceTypeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonServiceTypeEntity(IAmazonServiceTypeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonServiceTypeID = source.AmazonServiceTypeID;
            RowVersion = source.RowVersion;
            ApiValue = source.ApiValue;
            Description = source.Description;
            
            
            

            CopyCustomAmazonServiceTypeData(source);
        }

        
        /// <summary> The AmazonServiceTypeID property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."AmazonServiceTypeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int32 AmazonServiceTypeID { get; }
        /// <summary> The RowVersion property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ApiValue property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."ApiValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiValue { get; }
        /// <summary> The Description property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonServiceTypeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonServiceTypeData(IAmazonServiceTypeEntity source);
    }
}
