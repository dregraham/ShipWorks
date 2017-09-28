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
    /// Entity interface which represents the entity 'AmazonServiceType'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonServiceTypeEntity
    {
        
        /// <summary> The AmazonServiceTypeID property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."AmazonServiceTypeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int32 AmazonServiceTypeID { get; }
        /// <summary> The ApiValue property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."ApiValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiValue { get; }
        /// <summary> The Description property of the Entity AmazonServiceType<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonServiceType"."Description"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Description { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonServiceTypeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonServiceType'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonServiceTypeEntity : IAmazonServiceTypeEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonServiceTypeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonServiceTypeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonServiceTypeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonServiceTypeEntity(this, objectMap);
        }

        
    }
}
