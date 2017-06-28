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
    /// Entity interface which represents the entity 'EbayOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEbayOrderSearchEntity
    {
        
        /// <summary> The EbayOrderSearchID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."EbayOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EbayOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OrderNumberComplete { get; }
        /// <summary> The EbayOrderID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."EbayOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EbayOrderID { get; }
        /// <summary> The EbayBuyerID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."EbayBuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EbayBuyerID { get; }
        /// <summary> The SellingManagerRecord property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."SellingManagerRecord"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SellingManagerRecord { get; }
        
        
        IEbayOrderEntity EbayOrder { get; }
        IStoreEntity Store { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEbayOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEbayOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EbayOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class EbayOrderSearchEntity : IEbayOrderSearchEntity
    {
        
        IEbayOrderEntity IEbayOrderSearchEntity.EbayOrder => EbayOrder;
        IStoreEntity IEbayOrderSearchEntity.Store => Store;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEbayOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEbayOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEbayOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEbayOrderSearchEntity(this, objectMap);
        }
    }
}
