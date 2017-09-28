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
    /// Read-only representation of the entity 'MarketplaceAdvisorOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMarketplaceAdvisorOrderEntity : ReadOnlyOrderEntity, IMarketplaceAdvisorOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMarketplaceAdvisorOrderEntity(IMarketplaceAdvisorOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            BuyerNumber = source.BuyerNumber;
            SellerOrderNumber = source.SellerOrderNumber;
            InvoiceNumber = source.InvoiceNumber;
            ParcelID = source.ParcelID;
            
            
            
            MarketplaceAdvisorOrderSearch = source.MarketplaceAdvisorOrderSearch?.Select(x => x.AsReadOnly(objectMap)).OfType<IMarketplaceAdvisorOrderSearchEntity>().ToReadOnly() ??
                Enumerable.Empty<IMarketplaceAdvisorOrderSearchEntity>();

            CopyCustomMarketplaceAdvisorOrderData(source);
        }

        
        /// <summary> The BuyerNumber property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."BuyerNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 BuyerNumber { get; }
        /// <summary> The SellerOrderNumber property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."SellerOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 SellerOrderNumber { get; }
        /// <summary> The InvoiceNumber property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."InvoiceNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String InvoiceNumber { get; }
        /// <summary> The ParcelID property of the Entity MarketplaceAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorOrder"."ParcelID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ParcelID { get; }
        
        
        
        public IEnumerable<IMarketplaceAdvisorOrderSearchEntity> MarketplaceAdvisorOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IMarketplaceAdvisorOrderEntity AsReadOnlyMarketplaceAdvisorOrder() => this;

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IMarketplaceAdvisorOrderEntity AsReadOnlyMarketplaceAdvisorOrder(IDictionary<object, object> objectMap) => this;
        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMarketplaceAdvisorOrderData(IMarketplaceAdvisorOrderEntity source);
    }
}
