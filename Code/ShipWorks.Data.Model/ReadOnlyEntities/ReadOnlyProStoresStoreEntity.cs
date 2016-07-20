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
    /// Read-only representation of the entity 'ProStoresStore'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyProStoresStoreEntity : ReadOnlyStoreEntity, IProStoresStoreEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyProStoresStoreEntity(IProStoresStoreEntity source, IDictionary<object, object> objectMap) : base(source, objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShortName = source.ShortName;
            Username = source.Username;
            LoginMethod = source.LoginMethod;
            ApiEntryPoint = source.ApiEntryPoint;
            ApiToken = source.ApiToken;
            ApiStorefrontUrl = source.ApiStorefrontUrl;
            ApiTokenLogonUrl = source.ApiTokenLogonUrl;
            ApiXteUrl = source.ApiXteUrl;
            ApiRestSecureUrl = source.ApiRestSecureUrl;
            ApiRestNonSecureUrl = source.ApiRestNonSecureUrl;
            ApiRestScriptSuffix = source.ApiRestScriptSuffix;
            LegacyAdminUrl = source.LegacyAdminUrl;
            LegacyXtePath = source.LegacyXtePath;
            LegacyPrefix = source.LegacyPrefix;
            LegacyPassword = source.LegacyPassword;
            LegacyCanUpgrade = source.LegacyCanUpgrade;
            
            
            

            CopyCustomProStoresStoreData(source);
        }

        
        /// <summary> The ShortName property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ShortName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ShortName { get; }
        /// <summary> The Username property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Username { get; }
        /// <summary> The LoginMethod property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LoginMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 LoginMethod { get; }
        /// <summary> The ApiEntryPoint property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiEntryPoint"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiEntryPoint { get; }
        /// <summary> The ApiToken property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiToken { get; }
        /// <summary> The ApiStorefrontUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiStorefrontUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiStorefrontUrl { get; }
        /// <summary> The ApiTokenLogonUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiTokenLogonUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiTokenLogonUrl { get; }
        /// <summary> The ApiXteUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiXteUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiXteUrl { get; }
        /// <summary> The ApiRestSecureUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiRestSecureUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiRestSecureUrl { get; }
        /// <summary> The ApiRestNonSecureUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiRestNonSecureUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiRestNonSecureUrl { get; }
        /// <summary> The ApiRestScriptSuffix property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiRestScriptSuffix"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ApiRestScriptSuffix { get; }
        /// <summary> The LegacyAdminUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyAdminUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LegacyAdminUrl { get; }
        /// <summary> The LegacyXtePath property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyXtePath"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LegacyXtePath { get; }
        /// <summary> The LegacyPrefix property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyPrefix"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LegacyPrefix { get; }
        /// <summary> The LegacyPassword property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String LegacyPassword { get; }
        /// <summary> The LegacyCanUpgrade property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyCanUpgrade"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean LegacyCanUpgrade { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IProStoresStoreEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IProStoresStoreEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomProStoresStoreData(IProStoresStoreEntity source);
    }
}
