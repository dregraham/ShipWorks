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
    /// Entity interface which represents the entity 'ProStoresOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProStoresOrderSearchEntity
    {
        
        /// <summary> The ProStoresOrderSearchID property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."ProStoresOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ProStoresOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The ConfirmationNumber property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."ConfirmationNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ConfirmationNumber { get; }
        /// <summary> The OriginalOrderID property of the Entity ProStoresOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IProStoresOrderEntity ProStoresOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProStoresOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IProStoresOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProStoresOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProStoresOrderSearchEntity : IProStoresOrderSearchEntity
    {
        
        IProStoresOrderEntity IProStoresOrderSearchEntity.ProStoresOrder => ProStoresOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IProStoresOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IProStoresOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProStoresOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProStoresOrderSearchEntity(this, objectMap);
        }

        
    }
}
