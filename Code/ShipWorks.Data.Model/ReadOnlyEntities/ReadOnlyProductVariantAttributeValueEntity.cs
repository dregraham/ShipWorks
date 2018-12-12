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
    /// Read-only representation of the entity 'ProductVariantAttributeValue'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductVariantAttributeValueEntity : IProductVariantAttributeValueEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductVariantAttributeValueEntity(IProductVariantAttributeValueEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductVariantAttributeValueID = source.ProductVariantAttributeValueID;
            ProductVariantID = source.ProductVariantID;
            AttributeValue = source.AttributeValue;
            ProductAttributeID = source.ProductAttributeID;
            
            
            ProductAttribute = (IProductAttributeEntity) source.ProductAttribute?.AsReadOnly(objectMap);
            ProductVariant = (IProductVariantEntity) source.ProductVariant?.AsReadOnly(objectMap);
            

            CopyCustomProductVariantAttributeValueData(source);
        }

        
        /// <summary> The ProductVariantAttributeValueID property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."ProductVariantAttributeValueID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ProductVariantAttributeValueID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductVariantID { get; }
        /// <summary> The AttributeValue property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."AttributeValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AttributeValue { get; }
        /// <summary> The ProductAttributeID property of the Entity ProductVariantAttributeValue<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttributeValue"."ProductAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductAttributeID { get; }
        
        
        public IProductAttributeEntity ProductAttribute { get; }
        
        public IProductVariantEntity ProductVariant { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAttributeValueEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAttributeValueEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductVariantAttributeValueData(IProductVariantAttributeValueEntity source);
    }
}
