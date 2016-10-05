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
    /// Entity interface which represents the entity 'MarketplaceAdvisorStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IMarketplaceAdvisorStoreEntity: IStoreEntity
    {
        
        /// <summary> The Username property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The Password property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."Password"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Password { get; }
        /// <summary> The AccountType property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."AccountType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AccountType { get; }
        /// <summary> The DownloadFlags property of the Entity MarketplaceAdvisorStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "MarketplaceAdvisorStore"."DownloadFlags"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DownloadFlags { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IMarketplaceAdvisorStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IMarketplaceAdvisorStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'MarketplaceAdvisorStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class MarketplaceAdvisorStoreEntity : IMarketplaceAdvisorStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IMarketplaceAdvisorStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IMarketplaceAdvisorStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IMarketplaceAdvisorStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyMarketplaceAdvisorStoreEntity(this, objectMap);
        }
    }
}
