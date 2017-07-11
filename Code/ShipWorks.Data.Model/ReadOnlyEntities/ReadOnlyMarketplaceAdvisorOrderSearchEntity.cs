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
    /// Read-only representation of the entity 'MarketplaceAdvisorOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMarketplaceAdvisorOrderSearchEntity : IMarketplaceAdvisorOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMarketplaceAdvisorOrderSearchEntity(IMarketplaceAdvisorOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MarketplaceAdvisorOrderSearchID = source.MarketplaceAdvisorOrderSearchID;
            OrderID = source.OrderID;
            InvoiceNumber = source.InvoiceNumber;
            SellerOrderNumber = source.SellerOrderNumber;
            
            
            MarketplaceAdvisorOrder = source.MarketplaceAdvisorOrder?.AsReadOnly(objectMap);
            

            CopyCustomMarketplaceAdvisorOrderSearchData(source);
        }

        
        /// <summary> The MarketplaceAdvisorOrderSearchID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."MarketplaceAdvisorOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 MarketplaceAdvisorOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The InvoiceNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."InvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InvoiceNumber { get; }
        /// <summary> The SellerOrderNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."SellerOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 SellerOrderNumber { get; }
        
        
        public IMarketplaceAdvisorOrderEntity MarketplaceAdvisorOrder { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IMarketplaceAdvisorOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IMarketplaceAdvisorOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMarketplaceAdvisorOrderSearchData(IMarketplaceAdvisorOrderSearchEntity source);
    }
}
