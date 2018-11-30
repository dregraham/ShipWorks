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
    /// Read-only representation of the entity 'ProductBundle'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductBundleEntity : IProductBundleEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductBundleEntity(IProductBundleEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductID = source.ProductID;
            ChildProductVariantID = source.ChildProductVariantID;
            Quantity = source.Quantity;
            
            
            Product = (IProductEntity) source.Product?.AsReadOnly(objectMap);
            ChildVariant = (IProductVariantEntity) source.ChildVariant?.AsReadOnly(objectMap);
            

            CopyCustomProductBundleData(source);
        }

        
        /// <summary> The ProductID property of the Entity ProductBundle<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductBundle"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ProductID { get; }
        /// <summary> The ChildProductVariantID property of the Entity ProductBundle<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductBundle"."ChildProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ChildProductVariantID { get; }
        /// <summary> The Quantity property of the Entity ProductBundle<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductBundle"."Quantity"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Quantity { get; }
        
        
        public IProductEntity Product { get; }
        
        public IProductVariantEntity ChildVariant { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductBundleEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductBundleEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductBundleData(IProductBundleEntity source);
    }
}
