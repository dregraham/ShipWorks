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
    /// Read-only representation of the entity 'Product'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductEntity : IProductEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductEntity(IProductEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductID = source.ProductID;
            CreatedDate = source.CreatedDate;
            IsActive = source.IsActive;
            IsBundle = source.IsBundle;
            
            
            
            Attributes = source.Attributes?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductAttributeEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductAttributeEntity>();
            Bundles = source.Bundles?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductBundleEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductBundleEntity>();
            Variants = source.Variants?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductVariantEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductVariantEntity>();

            CopyCustomProductData(source);
        }

        
        /// <summary> The ProductID property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ProductID { get; }
        /// <summary> The CreatedDate property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."CreatedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime CreatedDate { get; }
        /// <summary> The IsActive property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."IsActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsActive { get; }
        /// <summary> The IsBundle property of the Entity Product<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Product"."IsBundle"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsBundle { get; }
        
        
        
        public IEnumerable<IProductAttributeEntity> Attributes { get; }
        
        public IEnumerable<IProductBundleEntity> Bundles { get; }
        
        public IEnumerable<IProductVariantEntity> Variants { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductData(IProductEntity source);
    }
}
