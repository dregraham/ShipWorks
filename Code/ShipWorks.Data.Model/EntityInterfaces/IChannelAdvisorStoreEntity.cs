///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'ChannelAdvisorStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IChannelAdvisorStoreEntity: IStoreEntity
    {
        
        /// <summary> The AccountKey property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AccountKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AccountKey { get; }
        /// <summary> The ProfileID property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."ProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ProfileID { get; }
        /// <summary> The AttributesToDownload property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AttributesToDownload"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AttributesToDownload { get; }
        /// <summary> The ConsolidatorAsUsps property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."ConsolidatorAsUsps"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ConsolidatorAsUsps { get; }
        /// <summary> The AmazonMerchantID property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonMerchantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonMerchantID { get; }
        /// <summary> The AmazonAuthToken property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonAuthToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonAuthToken { get; }
        /// <summary> The AmazonApiRegion property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonApiRegion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonApiRegion { get; }
        /// <summary> The AmazonShippingToken property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonShippingToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AmazonShippingToken { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IChannelAdvisorStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IChannelAdvisorStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ChannelAdvisorStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class ChannelAdvisorStoreEntity : IChannelAdvisorStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IChannelAdvisorStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IChannelAdvisorStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyChannelAdvisorStoreEntity(this, objectMap);
        }
    }
}
