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
    /// Entity interface which represents the entity 'AmazonASIN'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonASINEntity
    {
        
        /// <summary> The StoreID property of the Entity AmazonASIN<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonASIN"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The SKU property of the Entity AmazonASIN<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonASIN"."SKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.String SKU { get; }
        /// <summary> The AmazonASIN property of the Entity AmazonASIN<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonASIN"."AmazonASIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonASIN { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonASINEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAmazonASINEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonASIN'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonASINEntity : IAmazonASINEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAmazonASINEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAmazonASINEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonASINEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonASINEntity(this, objectMap);
        }

        
    }
}
