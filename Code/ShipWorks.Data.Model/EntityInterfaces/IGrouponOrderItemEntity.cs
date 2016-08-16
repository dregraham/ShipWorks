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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'GrouponOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGrouponOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The Permalink property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."Permalink"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Permalink { get; }
        /// <summary> The ChannelSKUProvided property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."ChannelSKUProvided"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ChannelSKUProvided { get; }
        /// <summary> The FulfillmentLineItemID property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."FulfillmentLineItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FulfillmentLineItemID { get; }
        /// <summary> The BomSKU property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."BomSKU"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BomSKU { get; }
        /// <summary> The GrouponLineItemID property of the Entity GrouponOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GrouponOrderItem"."GrouponLineItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String GrouponLineItemID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IGrouponOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IGrouponOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GrouponOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class GrouponOrderItemEntity : IGrouponOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IGrouponOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IGrouponOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGrouponOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGrouponOrderItemEntity(this, objectMap);
        }
    }
}
