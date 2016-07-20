///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'GrouponOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyGrouponOrderItemEntity : ReadOnlyOrderItemEntity, IGrouponOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyGrouponOrderItemEntity(IGrouponOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Permalink = source.Permalink;
            ChannelSKUProvided = source.ChannelSKUProvided;
            FulfillmentLineItemID = source.FulfillmentLineItemID;
            BomSKU = source.BomSKU;
            GrouponLineItemID = source.GrouponLineItemID;
            
            
            

            CopyCustomGrouponOrderItemData(source);
        }

        
        /// <summary> The Permalink property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."Permalink"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Permalink { get; }
        /// <summary> The ChannelSKUProvided property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."ChannelSKUProvided"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ChannelSKUProvided { get; }
        /// <summary> The FulfillmentLineItemID property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."FulfillmentLineItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FulfillmentLineItemID { get; }
        /// <summary> The BomSKU property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."BomSKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BomSKU { get; }
        /// <summary> The GrouponLineItemID property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."GrouponLineItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String GrouponLineItemID { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IGrouponOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IGrouponOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomGrouponOrderItemData(IGrouponOrderItemEntity source);
    }
}
