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
    /// Entity interface which represents the entity 'ProductVariantAlias'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProductVariantAliasEntity
    {
        
        /// <summary> The ProductVariantAliasID property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."ProductVariantAliasID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 ProductVariantAliasID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ProductVariantID { get; }
        /// <summary> The AliasName property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."AliasName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AliasName { get; }
        /// <summary> The Sku property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."Sku"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 Sku { get; }
        
        
        IProductVariantEntity ProductVariant { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantAliasEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProductVariantAliasEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProductVariantAlias'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProductVariantAliasEntity : IProductVariantAliasEntity
    {
        
        IProductVariantEntity IProductVariantAliasEntity.ProductVariant => ProductVariant;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAliasEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProductVariantAliasEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProductVariantAliasEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProductVariantAliasEntity(this, objectMap);
        }

        
    }
}
