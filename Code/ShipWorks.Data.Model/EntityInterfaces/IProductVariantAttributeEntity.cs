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
    /// Entity interface which represents the entity 'ProductVariantAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductVariantAttributeEntity
    {
        
        /// <summary> The ProductVariantAttributeID property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."ProductVariantAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ProductVariantAttributeID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductVariantID { get; }
        /// <summary> The AttributeName property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."AttributeName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributeName { get; }
        /// <summary> The AttributeValue property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."AttributeValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributeValue { get; }
        
        
        IProductVariantEntity ProductVariant { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantAttributeEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantAttributeEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductVariantAttribute'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductVariantAttributeEntity : IProductVariantAttributeEntity
    {
        
        IProductVariantEntity IProductVariantAttributeEntity.ProductVariant => ProductVariant;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAttributeEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductVariantAttributeEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductVariantAttributeEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductVariantAttributeEntity(this, objectMap);
        }

        
    }
}
