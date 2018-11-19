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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'ProductVariantAlias'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductVariantAliasEntity : IProductVariantAliasEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductVariantAliasEntity(IProductVariantAliasEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductVariantAliasID = source.ProductVariantAliasID;
            ProductVariantID = source.ProductVariantID;
            AliasName = source.AliasName;
            Sku = source.Sku;
            
            
            ProductVariant = (IProductVariantEntity) source.ProductVariant?.AsReadOnly(objectMap);
            

            CopyCustomProductVariantAliasData(source);
        }

        
        /// <summary> The ProductVariantAliasID property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."ProductVariantAliasID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ProductVariantAliasID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductVariantID { get; }
        /// <summary> The AliasName property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."AliasName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AliasName { get; }
        /// <summary> The Sku property of the Entity ProductVariantAlias<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAlias"."Sku"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Sku { get; }
        
        
        public IProductVariantEntity ProductVariant { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAliasEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAliasEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductVariantAliasData(IProductVariantAliasEntity source);
    }
}
