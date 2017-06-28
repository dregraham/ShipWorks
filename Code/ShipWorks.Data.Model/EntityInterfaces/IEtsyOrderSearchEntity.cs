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
    /// Entity interface which represents the entity 'EtsyOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEtsyOrderSearchEntity
    {
        
        /// <summary> The EtsyOrderSearchID property of the Entity EtsyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderSearch"."EtsyOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EtsyOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity EtsyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity EtsyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity EtsyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity EtsyOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EtsyOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        
        
        IEtsyOrderEntity EtsyOrder { get; }
        IStoreEntity Store { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEtsyOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEtsyOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EtsyOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class EtsyOrderSearchEntity : IEtsyOrderSearchEntity
    {
        
        IEtsyOrderEntity IEtsyOrderSearchEntity.EtsyOrder => EtsyOrder;
        IStoreEntity IEtsyOrderSearchEntity.Store => Store;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEtsyOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEtsyOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEtsyOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEtsyOrderSearchEntity(this, objectMap);
        }
    }
}
