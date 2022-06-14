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
    /// Entity interface which represents the entity 'AmazonSFPServiceType'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonSFPServiceTypeEntity
    {
        
        /// <summary> The AmazonSFPServiceTypeID property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."AmazonSFPServiceTypeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int32 AmazonSFPServiceTypeID { get; }
        /// <summary> The ApiValue property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."ApiValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiValue { get; }
        /// <summary> The Description property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        /// <summary> The PlatformApiCode property of the Entity AmazonSFPServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPServiceType"."PlatformApiCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String PlatformApiCode { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonSFPServiceTypeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonSFPServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonSFPServiceType'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonSFPServiceTypeEntity : IAmazonSFPServiceTypeEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSFPServiceTypeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonSFPServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonSFPServiceTypeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonSFPServiceTypeEntity(this, objectMap);
        }

        
    }
}
