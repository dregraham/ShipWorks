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
    /// Entity interface which represents the entity 'AmazonSFPProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonSFPProfileEntity
    {
        
        /// <summary> The ShippingProfileID property of the Entity AmazonSFPProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ShippingProfileID { get; }
        /// <summary> The DeliveryExperience property of the Entity AmazonSFPProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPProfile"."DeliveryExperience"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> DeliveryExperience { get; }
        /// <summary> The ShippingServiceID property of the Entity AmazonSFPProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPProfile"."ShippingServiceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String ShippingServiceID { get; }
        /// <summary> The Reference1 property of the Entity AmazonSFPProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonSFPProfile"."Reference1"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Reference1 { get; }
        
        IShippingProfileEntity ShippingProfile { get; }
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonSFPProfileEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonSFPProfileEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonSFPProfile'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonSFPProfileEntity : IAmazonSFPProfileEntity
    {
        IShippingProfileEntity IAmazonSFPProfileEntity.ShippingProfile => ShippingProfile;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonSFPProfileEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonSFPProfileEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonSFPProfileEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonSFPProfileEntity(this, objectMap);
        }

        
    }
}
