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
    /// Read-only representation of the entity 'OrderMotionStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyOrderMotionStoreEntity : ReadOnlyStoreEntity, IOrderMotionStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyOrderMotionStoreEntity(IOrderMotionStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            OrderMotionEmailAccountID = source.OrderMotionEmailAccountID;
            OrderMotionBizID = source.OrderMotionBizID;
            
            
            OrderMotionEmailAccount = (IEmailAccountEntity) source.OrderMotionEmailAccount?.AsReadOnly(objectMap);
            

            CopyCustomOrderMotionStoreData(source);
        }

        
        /// <summary> The OrderMotionEmailAccountID property of the Entity OrderMotionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionStore"."OrderMotionEmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderMotionEmailAccountID { get; }
        /// <summary> The OrderMotionBizID property of the Entity OrderMotionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "OrderMotionStore"."OrderMotionBizID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderMotionBizID { get; }
        
        
        public IEmailAccountEntity OrderMotionEmailAccount { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IOrderMotionStoreEntity AsReadOnlyOrderMotionStore() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IOrderMotionStoreEntity AsReadOnlyOrderMotionStore(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomOrderMotionStoreData(IOrderMotionStoreEntity source);
    }
}
