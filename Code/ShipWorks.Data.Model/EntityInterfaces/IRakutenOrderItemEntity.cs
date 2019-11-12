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
using System.Collections.ObjectModel;
using System.Linq;

namespace ShipWorks.Data.Model.EntityInterfaces
{
    /// <summary>
    /// Entity interface which represents the entity 'RakutenOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IRakutenOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The RakutenOrderID property of the Entity RakutenOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderItem"."RakutenOrderItemID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String RakutenOrderID { get; }
        /// <summary> The Discount property of the Entity RakutenOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderItem"."Discount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal Discount { get; }
        /// <summary> The ItemTotal property of the Entity RakutenOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "RakutenOrderItem"."ItemTotal"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal ItemTotal { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenOrderItemEntity AsReadOnlyRakutenOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IRakutenOrderItemEntity AsReadOnlyRakutenOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'RakutenOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class RakutenOrderItemEntity : IRakutenOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IRakutenOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyRakutenOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IRakutenOrderItemEntity AsReadOnlyRakutenOrderItem() =>
            (IRakutenOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IRakutenOrderItemEntity AsReadOnlyRakutenOrderItem(IDictionary<object, object> objectMap) =>
            (IRakutenOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
