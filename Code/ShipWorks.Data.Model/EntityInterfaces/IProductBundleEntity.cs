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
    /// Entity interface which represents the entity 'ProductBundle'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductBundleEntity
    {
        
        /// <summary> The ProductID property of the Entity ProductBundle<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductBundle"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ProductID { get; }
        /// <summary> The ChildProductVariantID property of the Entity ProductBundle<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductBundle"."ChildProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ChildProductVariantID { get; }
        /// <summary> The Quantity property of the Entity ProductBundle<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductBundle"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Quantity { get; }
        
        
        IProductEntity Product { get; }
        IProductVariantEntity ProductVariant { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductBundleEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductBundleEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductBundle'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductBundleEntity : IProductBundleEntity
    {
        
        IProductEntity IProductBundleEntity.Product => Product;
        IProductVariantEntity IProductBundleEntity.ProductVariant => ProductVariant;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductBundleEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductBundleEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductBundleEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductBundleEntity(this, objectMap);
        }

        
    }
}
