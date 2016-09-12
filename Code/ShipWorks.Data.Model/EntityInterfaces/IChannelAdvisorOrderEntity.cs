﻿///////////////////////////////////////////////////////////////
// This is generated code.
//////////////////////////////////////////////////////////////
// Code is generated using LLBLGen Pro version: 2.6
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
    /// Entity interface which represents the entity 'ChannelAdvisorOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IChannelAdvisorOrderEntity: IOrderEntity
    {
        
        /// <summary> The CustomOrderIdentifier property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."CustomOrderIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CustomOrderIdentifier { get; }
        /// <summary> The ResellerID property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."ResellerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ResellerID { get; }
        /// <summary> The OnlineShippingStatus property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."OnlineShippingStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OnlineShippingStatus { get; }
        /// <summary> The OnlineCheckoutStatus property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."OnlineCheckoutStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OnlineCheckoutStatus { get; }
        /// <summary> The OnlinePaymentStatus property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."OnlinePaymentStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OnlinePaymentStatus { get; }
        /// <summary> The FlagStyle property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."FlagStyle"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FlagStyle { get; }
        /// <summary> The FlagDescription property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."FlagDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FlagDescription { get; }
        /// <summary> The FlagType property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."FlagType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FlagType { get; }
        /// <summary> The MarketplaceNames property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."MarketplaceNames"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String MarketplaceNames { get; }
        /// <summary> The IsPrime property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."IsPrime"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 IsPrime { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IChannelAdvisorOrderEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IChannelAdvisorOrderEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ChannelAdvisorOrder'. <br/><br/>
    /// 
    /// </summary>
    public partial class ChannelAdvisorOrderEntity : IChannelAdvisorOrderEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorOrderEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IChannelAdvisorOrderEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IChannelAdvisorOrderEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyChannelAdvisorOrderEntity(this, objectMap);
        }
    }
}