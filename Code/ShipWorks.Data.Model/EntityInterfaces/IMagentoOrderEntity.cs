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
    /// Entity interface which represents the entity 'MagentoOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMagentoOrderEntity: IOrderEntity
    {
        
        /// <summary> The MagentoOrderID property of the Entity MagentoOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MagentoOrder"."MagentoOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 MagentoOrderID { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IMagentoOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IMagentoOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MagentoOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class MagentoOrderEntity : IMagentoOrderEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMagentoOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IMagentoOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMagentoOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMagentoOrderEntity(this, objectMap);
        }
    }
}
