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
    /// Read-only representation of the entity 'VolusionStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyVolusionStoreEntity : ReadOnlyStoreEntity, IVolusionStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyVolusionStoreEntity(IVolusionStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            StoreUrl = source.StoreUrl;
            WebUserName = source.WebUserName;
            WebPassword = source.WebPassword;
            ApiPassword = source.ApiPassword;
            PaymentMethods = source.PaymentMethods;
            ShipmentMethods = source.ShipmentMethods;
            DownloadOrderStatuses = source.DownloadOrderStatuses;
            ServerTimeZone = source.ServerTimeZone;
            ServerTimeZoneDST = source.ServerTimeZoneDST;
            
            
            

            CopyCustomVolusionStoreData(source);
        }

        
        /// <summary> The StoreUrl property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."StoreUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String StoreUrl { get; }
        /// <summary> The WebUserName property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."WebUserName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String WebUserName { get; }
        /// <summary> The WebPassword property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."WebPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 70<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String WebPassword { get; }
        /// <summary> The ApiPassword property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ApiPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiPassword { get; }
        /// <summary> The PaymentMethods property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."PaymentMethods"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PaymentMethods { get; }
        /// <summary> The ShipmentMethods property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ShipmentMethods"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShipmentMethods { get; }
        /// <summary> The DownloadOrderStatuses property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."DownloadOrderStatuses"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 255<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DownloadOrderStatuses { get; }
        /// <summary> The ServerTimeZone property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ServerTimeZone"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ServerTimeZone { get; }
        /// <summary> The ServerTimeZoneDST property of the Entity VolusionStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "VolusionStore"."ServerTimeZoneDST"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean ServerTimeZoneDST { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IVolusionStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IVolusionStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomVolusionStoreData(IVolusionStoreEntity source);
    }
}
