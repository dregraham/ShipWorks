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
    /// Entity interface which represents the entity 'GenericModuleStore'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IGenericModuleStoreEntity: IStoreEntity
    {
        
        /// <summary> The ModuleUsername property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleUsername"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModuleUsername { get; }
        /// <summary> The ModulePassword property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModulePassword"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 80<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModulePassword { get; }
        /// <summary> The ModuleUrl property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleUrl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModuleUrl { get; }
        /// <summary> The ModuleVersion property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModuleVersion { get; }
        /// <summary> The ModulePlatform property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModulePlatform"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModulePlatform { get; }
        /// <summary> The ModuleDeveloper property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleDeveloper"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModuleDeveloper { get; }
        /// <summary> The ModuleOnlineStoreCode property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineStoreCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModuleOnlineStoreCode { get; }
        /// <summary> The ModuleStatusCodes property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleStatusCodes"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ModuleStatusCodes { get; }
        /// <summary> The ModuleDownloadPageSize property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleDownloadPageSize"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleDownloadPageSize { get; }
        /// <summary> The ModuleRequestTimeout property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleRequestTimeout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleRequestTimeout { get; }
        /// <summary> The ModuleDownloadStrategy property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleDownloadStrategy"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleDownloadStrategy { get; }
        /// <summary> The ModuleOnlineStatusSupport property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineStatusSupport"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleOnlineStatusSupport { get; }
        /// <summary> The ModuleOnlineStatusDataType property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineStatusDataType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleOnlineStatusDataType { get; }
        /// <summary> The ModuleOnlineCustomerSupport property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineCustomerSupport"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ModuleOnlineCustomerSupport { get; }
        /// <summary> The ModuleOnlineCustomerDataType property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineCustomerDataType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleOnlineCustomerDataType { get; }
        /// <summary> The ModuleOnlineShipmentDetails property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleOnlineShipmentDetails"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ModuleOnlineShipmentDetails { get; }
        /// <summary> The ModuleHttpExpect100Continue property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleHttpExpect100Continue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean ModuleHttpExpect100Continue { get; }
        /// <summary> The ModuleResponseEncoding property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."ModuleResponseEncoding"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ModuleResponseEncoding { get; }
        /// <summary> The SchemaVersion property of the Entity GenericModuleStore<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "GenericModuleStore"."SchemaVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SchemaVersion { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IGenericModuleStoreEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        new IGenericModuleStoreEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'GenericModuleStore'. <br/><br/>
    /// 
    /// </summary>
    public partial class GenericModuleStoreEntity : IGenericModuleStoreEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public new IGenericModuleStoreEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public new IGenericModuleStoreEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IGenericModuleStoreEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyGenericModuleStoreEntity(this, objectMap);
        }
    }
}
