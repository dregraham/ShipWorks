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
    /// Entity interface which represents the entity 'InfopiaOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IInfopiaOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The Marketplace property of the Entity InfopiaOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaOrderItem"."Marketplace"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Marketplace { get; }
        /// <summary> The MarketplaceItemID property of the Entity InfopiaOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaOrderItem"."MarketplaceItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceItemID { get; }
        /// <summary> The BuyerID property of the Entity InfopiaOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaOrderItem"."BuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BuyerID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IInfopiaOrderItemEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IInfopiaOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'InfopiaOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class InfopiaOrderItemEntity : IInfopiaOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IInfopiaOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IInfopiaOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IInfopiaOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyInfopiaOrderItemEntity(this, objectMap);
        }
    }
}
