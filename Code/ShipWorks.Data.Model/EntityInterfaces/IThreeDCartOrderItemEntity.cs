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
    /// Entity interface which represents the entity 'ThreeDCartOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IThreeDCartOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The ThreeDCartShipmentID property of the Entity ThreeDCartOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrderItem"."ThreeDCartShipmentID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ThreeDCartShipmentID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartOrderItemEntity AsReadOnlyThreeDCartOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartOrderItemEntity AsReadOnlyThreeDCartOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ThreeDCartOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class ThreeDCartOrderItemEntity : IThreeDCartOrderItemEntity
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
                return (IThreeDCartOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyThreeDCartOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IThreeDCartOrderItemEntity AsReadOnlyThreeDCartOrderItem() =>
            (IThreeDCartOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IThreeDCartOrderItemEntity AsReadOnlyThreeDCartOrderItem(IDictionary<object, object> objectMap) =>
            (IThreeDCartOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
