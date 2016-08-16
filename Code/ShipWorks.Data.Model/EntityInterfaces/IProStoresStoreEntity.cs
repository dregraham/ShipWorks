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
    /// Entity interface which represents the entity 'ProStoresStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IProStoresStoreEntity: IStoreEntity
    {
        
        /// <summary> The ShortName property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ShortName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ShortName { get; }
        /// <summary> The Username property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."Username"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Username { get; }
        /// <summary> The LoginMethod property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LoginMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LoginMethod { get; }
        /// <summary> The ApiEntryPoint property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiEntryPoint"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiEntryPoint { get; }
        /// <summary> The ApiToken property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiToken"<br/>
        /// Table field type characteristics (type, precision, scale, length): Text, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiToken { get; }
        /// <summary> The ApiStorefrontUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiStorefrontUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiStorefrontUrl { get; }
        /// <summary> The ApiTokenLogonUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiTokenLogonUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiTokenLogonUrl { get; }
        /// <summary> The ApiXteUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiXteUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiXteUrl { get; }
        /// <summary> The ApiRestSecureUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiRestSecureUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiRestSecureUrl { get; }
        /// <summary> The ApiRestNonSecureUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiRestNonSecureUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiRestNonSecureUrl { get; }
        /// <summary> The ApiRestScriptSuffix property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."ApiRestScriptSuffix"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ApiRestScriptSuffix { get; }
        /// <summary> The LegacyAdminUrl property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyAdminUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LegacyAdminUrl { get; }
        /// <summary> The LegacyXtePath property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyXtePath"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 75<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LegacyXtePath { get; }
        /// <summary> The LegacyPrefix property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyPrefix"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 30<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LegacyPrefix { get; }
        /// <summary> The LegacyPassword property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyPassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 150<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LegacyPassword { get; }
        /// <summary> The LegacyCanUpgrade property of the Entity ProStoresStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "ProStoresStore"."LegacyCanUpgrade"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean LegacyCanUpgrade { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IProStoresStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IProStoresStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'ProStoresStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class ProStoresStoreEntity : IProStoresStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IProStoresStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IProStoresStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IProStoresStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyProStoresStoreEntity(this, objectMap);
        }
    }
}
