﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ProductAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductAttributeEntity
    {
        
        /// <summary> The ProductAttributeID property of the Entity ProductAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductAttribute"."ProductAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ProductAttributeID { get; }
        /// <summary> The ProductID property of the Entity ProductAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductAttribute"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductID { get; }
        /// <summary> The AttributeName property of the Entity ProductAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductAttribute"."AttributeName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributeName { get; }
        
        
        IProductEntity Product { get; }
        
        IEnumerable<IProductVariantAttributeValueEntity> ProductVariantAttributeValue { get; }

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductAttributeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductAttributeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductAttributeEntity : IProductAttributeEntity
    {
        
        IProductEntity IProductAttributeEntity.Product => Product;
        
        IEnumerable<IProductVariantAttributeValueEntity> IProductAttributeEntity.ProductVariantAttributeValue => ProductVariantAttributeValue;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductAttributeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductAttributeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductAttributeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductAttributeEntity(this, objectMap);
        }

        
    }
}
