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
    /// Read-only representation of the entity 'EbayOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEbayOrderSearchEntity : IEbayOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEbayOrderSearchEntity(IEbayOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EbayOrderSearchID = source.EbayOrderSearchID;
            OrderID = source.OrderID;
            EbayOrderID = source.EbayOrderID;
            EbayBuyerID = source.EbayBuyerID;
            SellingManagerRecord = source.SellingManagerRecord;
            OriginalOrderID = source.OriginalOrderID;
            
            
            EbayOrder = (IEbayOrderEntity) source.EbayOrder?.AsReadOnly(objectMap);
            

            CopyCustomEbayOrderSearchData(source);
        }

        
        /// <summary> The EbayOrderSearchID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."EbayOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EbayOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The EbayOrderID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."EbayOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EbayOrderID { get; }
        /// <summary> The EbayBuyerID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."EbayBuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EbayBuyerID { get; }
        /// <summary> The SellingManagerRecord property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."SellingManagerRecord"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> SellingManagerRecord { get; }
        /// <summary> The OriginalOrderID property of the Entity EbayOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EbayOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OriginalOrderID { get; }
        
        
        public IEbayOrderEntity EbayOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEbayOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEbayOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEbayOrderSearchData(IEbayOrderSearchEntity source);
    }
}
