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
    /// Read-only representation of the entity 'ProductVariantAttribute'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProductVariantAttributeEntity : IProductVariantAttributeEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProductVariantAttributeEntity(IProductVariantAttributeEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ProductVariantAttributeID = source.ProductVariantAttributeID;
            ProductVariantID = source.ProductVariantID;
            AttributeName = source.AttributeName;
            AttributeValue = source.AttributeValue;
            
            
            ProductVariant = (IProductVariantEntity) source.ProductVariant?.AsReadOnly(objectMap);
            

            CopyCustomProductVariantAttributeData(source);
        }

        
        /// <summary> The ProductVariantAttributeID property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."ProductVariantAttributeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ProductVariantAttributeID { get; }
        /// <summary> The ProductVariantID property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."ProductVariantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ProductVariantID { get; }
        /// <summary> The AttributeName property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."AttributeName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AttributeName { get; }
        /// <summary> The AttributeValue property of the Entity ProductVariantAttribute<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProductVariantAttribute"."AttributeValue"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AttributeValue { get; }
        
        
        public IProductVariantEntity ProductVariant { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAttributeEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProductVariantAttributeEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProductVariantAttributeData(IProductVariantAttributeEntity source);
    }
}
