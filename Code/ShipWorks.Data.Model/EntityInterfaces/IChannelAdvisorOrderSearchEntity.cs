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
    /// Entity interface which represents the entity 'ChannelAdvisorOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IChannelAdvisorOrderSearchEntity
    {
        
        /// <summary> The ChannelAdvisorOrderSearchID property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."ChannelAdvisorOrderSearchID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 ChannelAdvisorOrderSearchID { get; }
        /// <summary> The OrderID property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."OrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderID { get; }
        /// <summary> The CustomOrderIdentifier property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."CustomOrderIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CustomOrderIdentifier { get; }
        /// <summary> The OriginalOrderID property of the Entity ChannelAdvisorOrderSearch<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorOrderSearch"."OriginalOrderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OriginalOrderID { get; }
        
        
        IChannelAdvisorOrderEntity ChannelAdvisorOrder { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IChannelAdvisorOrderSearchEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IChannelAdvisorOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ChannelAdvisorOrderSearch'. <br/><br/>
    /// 
    /// </summary>
    public partial class ChannelAdvisorOrderSearchEntity : IChannelAdvisorOrderSearchEntity
    {
        
        IChannelAdvisorOrderEntity IChannelAdvisorOrderSearchEntity.ChannelAdvisorOrder => ChannelAdvisorOrder;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IChannelAdvisorOrderSearchEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IChannelAdvisorOrderSearchEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IChannelAdvisorOrderSearchEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyChannelAdvisorOrderSearchEntity(this, objectMap);
        }
    }
}
