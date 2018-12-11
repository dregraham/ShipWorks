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
    /// Read-only representation of the entity 'ProductAttribute'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductAttributeEntity : IProductAttributeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductAttributeEntity(IProductAttributeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductAttributeID = source.ProductAttributeID;
            ProductID = source.ProductID;
            AttributeName = source.AttributeName;
            
            
            Product = (IProductEntity) source.Product?.AsReadOnly(objectMap);
            
            ProductVariantAttribute = source.ProductVariantAttribute?.Select(x => x.AsReadOnly(objectMap)).OfType<IProductVariantAttributeEntity>().ToReadOnly() ??
                Enumerable.Empty<IProductVariantAttributeEntity>();

            CopyCustomProductAttributeData(source);
        }

        
        /// <summary> The ProductAttributeID property of the Entity ProductAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductAttribute"."ProductAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ProductAttributeID { get; }
        /// <summary> The ProductID property of the Entity ProductAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductAttribute"."ProductID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductID { get; }
        /// <summary> The AttributeName property of the Entity ProductAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductAttribute"."AttributeName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AttributeName { get; }
        
        
        public IProductEntity Product { get; }
        
        
        public IEnumerable<IProductVariantAttributeEntity> ProductVariantAttribute { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductAttributeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductAttributeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductAttributeData(IProductAttributeEntity source);
    }
}
