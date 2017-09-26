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
    /// Entity interface which represents the entity 'ThreeDCartOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IThreeDCartOrderEntity: IOrderEntity
    {
        
        /// <summary> The ThreeDCartOrderID property of the Entity ThreeDCartOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ThreeDCartOrder"."ThreeDCartOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ThreeDCartOrderID { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartOrderEntity AsReadOnlyThreeDCartOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IThreeDCartOrderEntity AsReadOnlyThreeDCartOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ThreeDCartOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class ThreeDCartOrderEntity : IThreeDCartOrderEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IThreeDCartOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyThreeDCartOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IThreeDCartOrderEntity AsReadOnlyThreeDCartOrder() =>
            (IThreeDCartOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IThreeDCartOrderEntity AsReadOnlyThreeDCartOrder(IDictionary<object, object> objectMap) =>
            (IThreeDCartOrderEntity) AsReadOnly(objectMap);
        
    }
}
