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
    /// Entity interface which represents the entity 'ProductVariantAttributeValue'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductVariantAttributeValueEntity
    {
        
        /// <summary> The ProductVariantAttributeValueID property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."ProductVariantAttributeValueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ProductVariantAttributeValueID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductVariantID { get; }
        /// <summary> The AttributeValue property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."AttributeValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributeValue { get; }
        /// <summary> The ProductAttributeID property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."ProductAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductAttributeID { get; }
        
        
        IProductAttributeEntity ProductAttribute { get; }
        IProductVariantEntity ProductVariant { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantAttributeValueEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantAttributeValueEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductVariantAttributeValue'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductVariantAttributeValueEntity : IProductVariantAttributeValueEntity
    {
        
        IProductAttributeEntity IProductVariantAttributeValueEntity.ProductAttribute => ProductAttribute;
        IProductVariantEntity IProductVariantAttributeValueEntity.ProductVariant => ProductVariant;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAttributeValueEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductVariantAttributeValueEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductVariantAttributeValueEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductVariantAttributeValueEntity(this, objectMap);
        }

        
    }
}
