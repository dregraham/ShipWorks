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
    /// Entity interface which represents the entity 'SearsOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ISearsOrderSearchEntity
    {
        
        /// <summary> The SearsOrderSearchID property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."SearsOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 SearsOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        /// <summary> The PoNumber property of the Entity SearsOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "SearsOrderSearch"."PoNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PoNumber { get; }
        
        
        ISearsOrderEntity SearsOrder { get; }
        IStoreEntity Store { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearsOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ISearsOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'SearsOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class SearsOrderSearchEntity : ISearsOrderSearchEntity
    {
        
        ISearsOrderEntity ISearsOrderSearchEntity.SearsOrder => SearsOrder;
        IStoreEntity ISearsOrderSearchEntity.Store => Store;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ISearsOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ISearsOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ISearsOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlySearsOrderSearchEntity(this, objectMap);
        }
    }
}
