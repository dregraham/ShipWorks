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
    /// Read-only representation of the entity 'RakutenOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyRakutenOrderItemEntity : ReadOnlyOrderItemEntity, IRakutenOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyRakutenOrderItemEntity(IRakutenOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            RakutenOrderID = source.RakutenOrderID;
            Discount = source.Discount;
            ItemTotal = source.ItemTotal;
            
            
            

            CopyCustomRakutenOrderItemData(source);
        }

        
        /// <summary> The RakutenOrderID property of the Entity RakutenOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderItem"."RakutenOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String RakutenOrderID { get; }
        /// <summary> The Discount property of the Entity RakutenOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderItem"."Discount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal Discount { get; }
        /// <summary> The ItemTotal property of the Entity RakutenOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderItem"."ItemTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal ItemTotal { get; }
        
        
        
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
        public IRakutenOrderItemEntity AsReadOnlyRakutenOrderItem() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IRakutenOrderItemEntity AsReadOnlyRakutenOrderItem(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomRakutenOrderItemData(IRakutenOrderItemEntity source);
    }
}
