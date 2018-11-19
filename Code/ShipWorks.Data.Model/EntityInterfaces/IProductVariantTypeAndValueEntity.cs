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
    /// Entity interface which represents the entity 'ProductVariantTypeAndValue'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductVariantTypeAndValueEntity
    {
        
        /// <summary> The ID property of the Entity ProductVariantTypeAndValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantTypeAndValue"."ProductVariantTypeAndValueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantTypeAndValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantTypeAndValue"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductVariantID { get; }
        /// <summary> The AttributeName property of the Entity ProductVariantTypeAndValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantTypeAndValue"."AttributeName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributeName { get; }
        /// <summary> The AttributeValue property of the Entity ProductVariantTypeAndValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantTypeAndValue"."AttributeValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributeValue { get; }
        
        
        IProductVariantEntity ProductVariant { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantTypeAndValueEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantTypeAndValueEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductVariantTypeAndValue'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductVariantTypeAndValueEntity : IProductVariantTypeAndValueEntity
    {
        
        IProductVariantEntity IProductVariantTypeAndValueEntity.ProductVariant => ProductVariant;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantTypeAndValueEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductVariantTypeAndValueEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductVariantTypeAndValueEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductVariantTypeAndValueEntity(this, objectMap);
        }

        
    }
}
