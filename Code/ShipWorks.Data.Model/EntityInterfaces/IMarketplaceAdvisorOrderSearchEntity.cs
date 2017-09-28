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
    /// Entity interface which represents the entity 'MarketplaceAdvisorOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMarketplaceAdvisorOrderSearchEntity
    {
        
        /// <summary> The MarketplaceAdvisorOrderSearchID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."MarketplaceAdvisorOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 MarketplaceAdvisorOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The InvoiceNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."InvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InvoiceNumber { get; }
        /// <summary> The SellerOrderNumber property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."SellerOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 SellerOrderNumber { get; }
        /// <summary> The OriginalOrderID property of the Entity MarketplaceAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IMarketplaceAdvisorOrderEntity MarketplaceAdvisorOrder { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMarketplaceAdvisorOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMarketplaceAdvisorOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MarketplaceAdvisorOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class MarketplaceAdvisorOrderSearchEntity : IMarketplaceAdvisorOrderSearchEntity
    {
        
        IMarketplaceAdvisorOrderEntity IMarketplaceAdvisorOrderSearchEntity.MarketplaceAdvisorOrder => MarketplaceAdvisorOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IMarketplaceAdvisorOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IMarketplaceAdvisorOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMarketplaceAdvisorOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMarketplaceAdvisorOrderSearchEntity(this, objectMap);
        }

        
    }
}
