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
    /// Read-only representation of the entity 'ChannelAdvisorOrderItem'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyChannelAdvisorOrderItemEntity : ReadOnlyOrderItemEntity, IChannelAdvisorOrderItemEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyChannelAdvisorOrderItemEntity(IChannelAdvisorOrderItemEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            MarketplaceName = source.MarketplaceName;
            MarketplaceStoreName = source.MarketplaceStoreName;
            MarketplaceBuyerID = source.MarketplaceBuyerID;
            MarketplaceSalesID = source.MarketplaceSalesID;
            Classification = source.Classification;
            DistributionCenter = source.DistributionCenter;
            IsFBA = source.IsFBA;
            MPN = source.MPN;
            DistributionCenterID = source.DistributionCenterID;
            DistributionCenterName = source.DistributionCenterName;
            
            
            

            CopyCustomChannelAdvisorOrderItemData(source);
        }

        
        /// <summary> The MarketplaceName property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceName { get; }
        /// <summary> The MarketplaceStoreName property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceStoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceStoreName { get; }
        /// <summary> The MarketplaceBuyerID property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceBuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceBuyerID { get; }
        /// <summary> The MarketplaceSalesID property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceSalesID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceSalesID { get; }
        /// <summary> The Classification property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."Classification"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Classification { get; }
        /// <summary> The DistributionCenter property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."DistributionCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DistributionCenter { get; }
        /// <summary> The IsFBA property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."IsFBA"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean IsFBA { get; }
        /// <summary> The MPN property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MPN"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MPN { get; }
        /// <summary> The DistributionCenterID property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."DistributionCenterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 DistributionCenterID { get; }
        /// <summary> The DistributionCenterName property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."DistributionCenterName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DistributionCenterName { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorOrderItemEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomChannelAdvisorOrderItemData(IChannelAdvisorOrderItemEntity source);
    }
}
