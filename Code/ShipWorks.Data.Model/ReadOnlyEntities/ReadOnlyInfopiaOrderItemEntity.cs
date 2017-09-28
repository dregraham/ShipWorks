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
    /// Read-only representation of the entity 'InfopiaOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyInfopiaOrderItemEntity : ReadOnlyOrderItemEntity, IInfopiaOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyInfopiaOrderItemEntity(IInfopiaOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Marketplace = source.Marketplace;
            MarketplaceItemID = source.MarketplaceItemID;
            BuyerID = source.BuyerID;
            
            
            

            CopyCustomInfopiaOrderItemData(source);
        }

        
        /// <summary> The Marketplace property of the Entity InfopiaOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaOrderItem"."Marketplace"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Marketplace { get; }
        /// <summary> The MarketplaceItemID property of the Entity InfopiaOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaOrderItem"."MarketplaceItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceItemID { get; }
        /// <summary> The BuyerID property of the Entity InfopiaOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "InfopiaOrderItem"."BuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BuyerID { get; }
        
        
        
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
        public IInfopiaOrderItemEntity AsReadOnlyInfopiaOrderItem() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IInfopiaOrderItemEntity AsReadOnlyInfopiaOrderItem(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomInfopiaOrderItemData(IInfopiaOrderItemEntity source);
    }
}
