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
    /// Entity interface which represents the entity 'OrderMotionStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IOrderMotionStoreEntity: IStoreEntity
    {
        
        /// <summary> The OrderMotionEmailAccountID property of the Entity OrderMotionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionStore"."OrderMotionEmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderMotionEmailAccountID { get; }
        /// <summary> The OrderMotionBizID property of the Entity OrderMotionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionStore"."OrderMotionBizID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderMotionBizID { get; }
        
        
        IEmailAccountEntity OrderMotionEmailAccount { get; }
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderMotionStoreEntity AsReadOnlyOrderMotionStore();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IOrderMotionStoreEntity AsReadOnlyOrderMotionStore(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'OrderMotionStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class OrderMotionStoreEntity : IOrderMotionStoreEntity
    {
        
        IEmailAccountEntity IOrderMotionStoreEntity.OrderMotionEmailAccount => OrderMotionEmailAccount;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IOrderMotionStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyOrderMotionStoreEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IOrderMotionStoreEntity AsReadOnlyOrderMotionStore() =>
            (IOrderMotionStoreEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IOrderMotionStoreEntity AsReadOnlyOrderMotionStore(IDictionary<object, object> objectMap) =>
            (IOrderMotionStoreEntity) AsReadOnly(objectMap);
        
    }
}
