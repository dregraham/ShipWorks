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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Entity interface which represents the entity 'MarketplaceAdvisorStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyMarketplaceAdvisorStoreEntity : ReadOnlyStoreEntity, IMarketplaceAdvisorStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyMarketplaceAdvisorStoreEntity(IMarketplaceAdvisorStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            Username = source.Username;
            Password = source.Password;
            AccountType = source.AccountType;
            DownloadFlags = source.DownloadFlags;
            
            
            

            CopyCustomMarketplaceAdvisorStoreData(source);
        }

        
        /// <summary> The Username property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The Password property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Password { get; }
        /// <summary> The AccountType property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."AccountType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 AccountType { get; }
        /// <summary> The DownloadFlags property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."DownloadFlags"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DownloadFlags { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMarketplaceAdvisorStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMarketplaceAdvisorStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomMarketplaceAdvisorStoreData(IMarketplaceAdvisorStoreEntity source);
    }
}
