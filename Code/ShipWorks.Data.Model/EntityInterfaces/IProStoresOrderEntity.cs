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
    /// Entity interface which represents the entity 'ProStoresOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProStoresOrderEntity: IOrderEntity
    {
        
        /// <summary> The ConfirmationNumber property of the Entity ProStoresOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrder"."ConfirmationNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 12<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ConfirmationNumber { get; }
        /// <summary> The AuthorizedDate property of the Entity ProStoresOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrder"."AuthorizedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> AuthorizedDate { get; }
        /// <summary> The AuthorizedBy property of the Entity ProStoresOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresOrder"."AuthorizedBy"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AuthorizedBy { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IProStoresOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IProStoresOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProStoresOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProStoresOrderEntity : IProStoresOrderEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IProStoresOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IProStoresOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProStoresOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProStoresOrderEntity(this, objectMap);
        }
    }
}
