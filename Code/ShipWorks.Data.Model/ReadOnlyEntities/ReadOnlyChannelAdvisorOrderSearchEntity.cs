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
    /// Read-only representation of the entity 'ChannelAdvisorOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyChannelAdvisorOrderSearchEntity : IChannelAdvisorOrderSearchEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyChannelAdvisorOrderSearchEntity(IChannelAdvisorOrderSearchEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ChannelAdvisorOrderSearchID = source.ChannelAdvisorOrderSearchID;
            OrderID = source.OrderID;
            StoreID = source.StoreID;
            OrderNumber = source.OrderNumber;
            OrderNumberComplete = source.OrderNumberComplete;
            CustomOrderIdentifier = source.CustomOrderIdentifier;
            
            
            ChannelAdvisorOrder = source.ChannelAdvisorOrder?.AsReadOnly(objectMap);
            Store = source.Store?.AsReadOnly(objectMap);
            

            CopyCustomChannelAdvisorOrderSearchData(source);
        }

        
        /// <summary> The ChannelAdvisorOrderSearchID property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."ChannelAdvisorOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 ChannelAdvisorOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderID { get; }
        /// <summary> The StoreID property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 StoreID { get; }
        /// <summary> The OrderNumber property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."OrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderNumber { get; }
        /// <summary> The OrderNumberComplete property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."OrderNumberComplete"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OrderNumberComplete { get; }
        /// <summary> The CustomOrderIdentifier property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."CustomOrderIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CustomOrderIdentifier { get; }
        
        
        public IChannelAdvisorOrderEntity ChannelAdvisorOrder { get; }
        
        public IStoreEntity Store { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IChannelAdvisorOrderSearchEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IChannelAdvisorOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomChannelAdvisorOrderSearchData(IChannelAdvisorOrderSearchEntity source);
    }
}
