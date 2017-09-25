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
    /// Entity interface which represents the entity 'AmazonOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAmazonOrderEntity: IOrderEntity
    {
        
        /// <summary> The AmazonOrderID property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."AmazonOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonOrderID { get; }
        /// <summary> The AmazonCommission property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."AmazonCommission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Decimal AmazonCommission { get; }
        /// <summary> The FulfillmentChannel property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."FulfillmentChannel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FulfillmentChannel { get; }
        /// <summary> The IsPrime property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."IsPrime"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IsPrime { get; }
        /// <summary> The EarliestExpectedDeliveryDate property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."EarliestExpectedDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> EarliestExpectedDeliveryDate { get; }
        /// <summary> The LatestExpectedDeliveryDate property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."LatestExpectedDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> LatestExpectedDeliveryDate { get; }
        /// <summary> The PurchaseOrderNumber property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."PurchaseOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PurchaseOrderNumber { get; }
        
        
        
        IEnumerable<IAmazonOrderSearchEntity> AmazonOrderSearch { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmazonOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IAmazonOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AmazonOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class AmazonOrderEntity : IAmazonOrderEntity
    {
        
        
        IEnumerable<IAmazonOrderSearchEntity> IAmazonOrderEntity.AmazonOrderSearch => AmazonOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IAmazonOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAmazonOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAmazonOrderEntity(this, objectMap);
        }
    }
}
