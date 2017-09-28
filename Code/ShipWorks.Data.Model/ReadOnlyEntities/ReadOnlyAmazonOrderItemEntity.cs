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
    /// Read-only representation of the entity 'AmazonOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonOrderItemEntity : ReadOnlyOrderItemEntity, IAmazonOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonOrderItemEntity(IAmazonOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonOrderItemCode = source.AmazonOrderItemCode;
            ASIN = source.ASIN;
            ConditionNote = source.ConditionNote;
            
            
            

            CopyCustomAmazonOrderItemData(source);
        }

        
        /// <summary> The AmazonOrderItemCode property of the Entity AmazonOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderItem"."AmazonOrderItemCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 64<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonOrderItemCode { get; }
        /// <summary> The ASIN property of the Entity AmazonOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderItem"."ASIN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ASIN { get; }
        /// <summary> The ConditionNote property of the Entity AmazonOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrderItem"."ConditionNote"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ConditionNote { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IAmazonOrderItemEntity AsReadOnlyAmazonOrderItem() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IAmazonOrderItemEntity AsReadOnlyAmazonOrderItem(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonOrderItemData(IAmazonOrderItemEntity source);
    }
}
