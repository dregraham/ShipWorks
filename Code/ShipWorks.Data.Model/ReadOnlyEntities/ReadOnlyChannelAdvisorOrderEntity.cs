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
    /// Read-only representation of the entity 'ChannelAdvisorOrder'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyChannelAdvisorOrderEntity : ReadOnlyOrderEntity, IChannelAdvisorOrderEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyChannelAdvisorOrderEntity(IChannelAdvisorOrderEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            CustomOrderIdentifier = source.CustomOrderIdentifier;
            ResellerID = source.ResellerID;
            OnlineShippingStatus = source.OnlineShippingStatus;
            OnlineCheckoutStatus = source.OnlineCheckoutStatus;
            OnlinePaymentStatus = source.OnlinePaymentStatus;
            FlagStyle = source.FlagStyle;
            FlagDescription = source.FlagDescription;
            FlagType = source.FlagType;
            MarketplaceNames = source.MarketplaceNames;
            IsPrime = source.IsPrime;
            
            
            
            ChannelAdvisorOrderSearch = source.ChannelAdvisorOrderSearch?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IChannelAdvisorOrderSearchEntity>();

            CopyCustomChannelAdvisorOrderData(source);
        }

        
        /// <summary> The CustomOrderIdentifier property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."CustomOrderIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomOrderIdentifier { get; }
        /// <summary> The ResellerID property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."ResellerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ResellerID { get; }
        /// <summary> The OnlineShippingStatus property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."OnlineShippingStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OnlineShippingStatus { get; }
        /// <summary> The OnlineCheckoutStatus property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."OnlineCheckoutStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OnlineCheckoutStatus { get; }
        /// <summary> The OnlinePaymentStatus property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."OnlinePaymentStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OnlinePaymentStatus { get; }
        /// <summary> The FlagStyle property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."FlagStyle"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 32<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FlagStyle { get; }
        /// <summary> The FlagDescription property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."FlagDescription"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FlagDescription { get; }
        /// <summary> The FlagType property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."FlagType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FlagType { get; }
        /// <summary> The MarketplaceNames property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."MarketplaceNames"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 1024<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String MarketplaceNames { get; }
        /// <summary> The IsPrime property of the Entity ChannelAdvisorOrder<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrder"."IsPrime"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 IsPrime { get; }
        
        
        
        public IEnumerable<IChannelAdvisorOrderSearchEntity> ChannelAdvisorOrderSearch { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorOrderEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorOrderEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomChannelAdvisorOrderData(IChannelAdvisorOrderEntity source);
    }
}
