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
    /// Read-only representation of the entity 'ChannelAdvisorStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyChannelAdvisorStoreEntity : ReadOnlyStoreEntity, IChannelAdvisorStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyChannelAdvisorStoreEntity(IChannelAdvisorStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AccountKey = source.AccountKey;
            ProfileID = source.ProfileID;
            AttributesToDownload = source.AttributesToDownload;
            ConsolidatorAsUsps = source.ConsolidatorAsUsps;
            AmazonMerchantID = source.AmazonMerchantID;
            AmazonAuthToken = source.AmazonAuthToken;
            AmazonApiRegion = source.AmazonApiRegion;
            
            
            

            CopyCustomChannelAdvisorStoreData(source);
        }

        
        /// <summary> The AccountKey property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AccountKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AccountKey { get; }
        /// <summary> The ProfileID property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."ProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ProfileID { get; }
        /// <summary> The AttributesToDownload property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AttributesToDownload"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AttributesToDownload { get; }
        /// <summary> The ConsolidatorAsUsps property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."ConsolidatorAsUsps"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ConsolidatorAsUsps { get; }
        /// <summary> The AmazonMerchantID property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonMerchantID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonMerchantID { get; }
        /// <summary> The AmazonAuthToken property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonAuthToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonAuthToken { get; }
        /// <summary> The AmazonApiRegion property of the Entity ChannelAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ChannelAdvisorStore"."AmazonApiRegion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Char, 0, 0, 2<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AmazonApiRegion { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IChannelAdvisorStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomChannelAdvisorStoreData(IChannelAdvisorStoreEntity source);
    }
}
