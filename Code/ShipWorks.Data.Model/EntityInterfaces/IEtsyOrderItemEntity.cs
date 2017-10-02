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
    /// Entity interface which represents the entity 'EtsyOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEtsyOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The ListingID property of the Entity EtsyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderItem"."ListingID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ListingID { get; }
        /// <summary> The TransactionID property of the Entity EtsyOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderItem"."TransactionID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 TransactionID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEtsyOrderItemEntity AsReadOnlyEtsyOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEtsyOrderItemEntity AsReadOnlyEtsyOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EtsyOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class EtsyOrderItemEntity : IEtsyOrderItemEntity
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
                return (IEtsyOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEtsyOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IEtsyOrderItemEntity AsReadOnlyEtsyOrderItem() =>
            (IEtsyOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IEtsyOrderItemEntity AsReadOnlyEtsyOrderItem(IDictionary<object, object> objectMap) =>
            (IEtsyOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
