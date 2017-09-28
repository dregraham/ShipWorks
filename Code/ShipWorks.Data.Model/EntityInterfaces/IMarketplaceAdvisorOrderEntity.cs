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
    /// Entity interface which represents the entity 'MarketplaceAdvisorOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMarketplaceAdvisorOrderEntity: IOrderEntity
    {
        
        /// <summary> The BuyerNumber property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."BuyerNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 BuyerNumber { get; }
        /// <summary> The SellerOrderNumber property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."SellerOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 SellerOrderNumber { get; }
        /// <summary> The InvoiceNumber property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."InvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String InvoiceNumber { get; }
        /// <summary> The ParcelID property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."ParcelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ParcelID { get; }
        
        
        
        IEnumerable<IMarketplaceAdvisorOrderSearchEntity> MarketplaceAdvisorOrderSearch { get; }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMarketplaceAdvisorOrderEntity AsReadOnlyMarketplaceAdvisorOrder();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IMarketplaceAdvisorOrderEntity AsReadOnlyMarketplaceAdvisorOrder(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MarketplaceAdvisorOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class MarketplaceAdvisorOrderEntity : IMarketplaceAdvisorOrderEntity
    {
        
        
        IEnumerable<IMarketplaceAdvisorOrderSearchEntity> IMarketplaceAdvisorOrderEntity.MarketplaceAdvisorOrderSearch => MarketplaceAdvisorOrderSearch;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMarketplaceAdvisorOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMarketplaceAdvisorOrderEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IMarketplaceAdvisorOrderEntity AsReadOnlyMarketplaceAdvisorOrder() =>
            (IMarketplaceAdvisorOrderEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IMarketplaceAdvisorOrderEntity AsReadOnlyMarketplaceAdvisorOrder(IDictionary<object, object> objectMap) =>
            (IMarketplaceAdvisorOrderEntity) AsReadOnly(objectMap);
        
    }
}
