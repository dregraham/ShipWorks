﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ChannelAdvisorOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IChannelAdvisorOrderItemEntity: IOrderItemEntity
    {
        
        /// <summary> The MarketplaceName property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceName { get; }
        /// <summary> The MarketplaceStoreName property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceStoreName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceStoreName { get; }
        /// <summary> The MarketplaceBuyerID property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceBuyerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceBuyerID { get; }
        /// <summary> The MarketplaceSalesID property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."MarketplaceSalesID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceSalesID { get; }
        /// <summary> The Classification property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."Classification"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Classification { get; }
        /// <summary> The DistributionCenter property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."DistributionCenter"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DistributionCenter { get; }
        /// <summary> The IsFBA property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."IsFBA"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean IsFBA { get; }
        /// <summary> The DistributionCenterID property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."DistributionCenterID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 DistributionCenterID { get; }
        /// <summary> The DistributionCenterName property of the Entity ChannelAdvisorOrderItem<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderItem"."DistributionCenterName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DistributionCenterName { get; }
        
        
        

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IChannelAdvisorOrderItemEntity AsReadOnlyChannelAdvisorOrderItem();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IChannelAdvisorOrderItemEntity AsReadOnlyChannelAdvisorOrderItem(IDictionary<object, object> objectMap);
        
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ChannelAdvisorOrderItem'. <br/><br/>
    /// 
    /// </summary>
    public partial class ChannelAdvisorOrderItemEntity : IChannelAdvisorOrderItemEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public override IOrderItemEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public override IOrderItemEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IChannelAdvisorOrderItemEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyChannelAdvisorOrderItemEntity(this, objectMap);
        }

        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public IChannelAdvisorOrderItemEntity AsReadOnlyChannelAdvisorOrderItem() =>
            (IChannelAdvisorOrderItemEntity) AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public IChannelAdvisorOrderItemEntity AsReadOnlyChannelAdvisorOrderItem(IDictionary<object, object> objectMap) =>
            (IChannelAdvisorOrderItemEntity) AsReadOnly(objectMap);
        
    }
}
