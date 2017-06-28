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
    /// Read-only representation of the entity 'AmazonOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAmazonOrderEntity : ReadOnlyOrderEntity, IAmazonOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAmazonOrderEntity(IAmazonOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AmazonOrderID = source.AmazonOrderID;
            AmazonCommission = source.AmazonCommission;
            FulfillmentChannel = source.FulfillmentChannel;
            IsPrime = source.IsPrime;
            EarliestExpectedDeliveryDate = source.EarliestExpectedDeliveryDate;
            LatestExpectedDeliveryDate = source.LatestExpectedDeliveryDate;
            PurchaseOrderNumber = source.PurchaseOrderNumber;
            
            
            
            AmazonOrderSearch = source.AmazonOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IAmazonOrderSearchEntity>();

            CopyCustomAmazonOrderData(source);
        }

        
        /// <summary> The AmazonOrderID property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."AmazonOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonOrderID { get; }
        /// <summary> The AmazonCommission property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."AmazonCommission"<br/>
        /// Table field type characteristics (type, precision, scale, length): Money, 19, 4, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Decimal AmazonCommission { get; }
        /// <summary> The FulfillmentChannel property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."FulfillmentChannel"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FulfillmentChannel { get; }
        /// <summary> The IsPrime property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."IsPrime"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IsPrime { get; }
        /// <summary> The EarliestExpectedDeliveryDate property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."EarliestExpectedDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> EarliestExpectedDeliveryDate { get; }
        /// <summary> The LatestExpectedDeliveryDate property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."LatestExpectedDeliveryDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> LatestExpectedDeliveryDate { get; }
        /// <summary> The PurchaseOrderNumber property of the Entity AmazonOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AmazonOrder"."PurchaseOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PurchaseOrderNumber { get; }
        
        
        
        public IEnumerable<IAmazonOrderSearchEntity> AmazonOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IAmazonOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAmazonOrderData(IAmazonOrderEntity source);
    }
}
