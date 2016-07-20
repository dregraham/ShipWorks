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
    /// Read-only representation of the entity 'YahooStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyYahooStoreEntity : ReadOnlyStoreEntity, IYahooStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyYahooStoreEntity(IYahooStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            YahooEmailAccountID = source.YahooEmailAccountID;
            TrackingUpdatePassword = source.TrackingUpdatePassword;
            YahooStoreID = source.YahooStoreID;
            AccessToken = source.AccessToken;
            BackupOrderNumber = source.BackupOrderNumber;
            
            
            YahooEmailAccount = source.YahooEmailAccount?.AsReadOnly(objectMap);
            

            CopyCustomYahooStoreData(source);
        }

        
        /// <summary> The YahooEmailAccountID property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."YahooEmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 YahooEmailAccountID { get; }
        /// <summary> The TrackingUpdatePassword property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."TrackingUpdatePassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String TrackingUpdatePassword { get; }
        /// <summary> The YahooStoreID property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."YahooStoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String YahooStoreID { get; }
        /// <summary> The AccessToken property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."AccessToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String AccessToken { get; }
        /// <summary> The BackupOrderNumber property of the Entity YahooStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "YahooStore"."BackupOrderNumber"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> BackupOrderNumber { get; }
        
        
        public IEmailAccountEntity YahooEmailAccount { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IYahooStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IYahooStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomYahooStoreData(IYahooStoreEntity source);
    }
}
