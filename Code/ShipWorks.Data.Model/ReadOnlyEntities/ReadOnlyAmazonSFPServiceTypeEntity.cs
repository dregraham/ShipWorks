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
    /// Read-only representation of the entity 'AmazonSFPServiceType'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonSFPServiceTypeEntity : IAmazonSFPServiceTypeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonSFPServiceTypeEntity(IAmazonSFPServiceTypeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonSFPServiceTypeID = source.AmazonSFPServiceTypeID;
            ApiValue = source.ApiValue;
            Description = source.Description;
            PlatformApiCode = source.PlatformApiCode;
            
            
            

            CopyCustomAmazonSFPServiceTypeData(source);
        }

        
        /// <summary> The AmazonSFPServiceTypeID property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."AmazonSFPServiceTypeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int32 AmazonSFPServiceTypeID { get; }
        /// <summary> The ApiValue property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."ApiValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiValue { get; }
        /// <summary> The Description property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Description { get; }
        /// <summary> The PlatformApiCode property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."PlatformApiCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String PlatformApiCode { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSFPServiceTypeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSFPServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonSFPServiceTypeData(IAmazonSFPServiceTypeEntity source);
    }
}
