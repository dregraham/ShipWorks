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
    /// Entity interface which represents the entity 'YahooStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IYahooStoreEntity: IStoreEntity
    {
        
        /// <summary> The YahooEmailAccountID property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."YahooEmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 YahooEmailAccountID { get; }
        /// <summary> The TrackingUpdatePassword property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."TrackingUpdatePassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String TrackingUpdatePassword { get; }
        /// <summary> The YahooStoreID property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."YahooStoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String YahooStoreID { get; }
        /// <summary> The AccessToken property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."AccessToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String AccessToken { get; }
        /// <summary> The BackupOrderNumber property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."BackupOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> BackupOrderNumber { get; }
        
        
        IEmailAccountEntity YahooEmailAccount { get; }
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IYahooStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IYahooStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'YahooStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class YahooStoreEntity : IYahooStoreEntity
    {
        
        IEmailAccountEntity IYahooStoreEntity.YahooEmailAccount => YahooEmailAccount;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IYahooStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IYahooStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IYahooStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyYahooStoreEntity(this, objectMap);
        }
    }
}
