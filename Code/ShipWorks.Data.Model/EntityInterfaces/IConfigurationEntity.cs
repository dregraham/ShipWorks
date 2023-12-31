﻿///////////////////////////////////////////////////////////////
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
    /// Entity interface which represents the entity 'Configuration'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IConfigurationEntity
    {
        
        /// <summary> The ConfigurationID property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."ConfigurationID"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Boolean ConfigurationID { get; }
        /// <summary> The RowVersion property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The LogOnMethod property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."LogOnMethod"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 LogOnMethod { get; }
        /// <summary> The AddressCasing property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AddressCasing"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AddressCasing { get; }
        /// <summary> The CustomerCompareEmail property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerCompareEmail"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomerCompareEmail { get; }
        /// <summary> The CustomerCompareAddress property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerCompareAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomerCompareAddress { get; }
        /// <summary> The CustomerUpdateBilling property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateBilling"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomerUpdateBilling { get; }
        /// <summary> The CustomerUpdateShipping property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateShipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean CustomerUpdateShipping { get; }
        /// <summary> The CustomerUpdateModifiedBilling property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateModifiedBilling"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CustomerUpdateModifiedBilling { get; }
        /// <summary> The CustomerUpdateModifiedShipping property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerUpdateModifiedShipping"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 CustomerUpdateModifiedShipping { get; }
        /// <summary> The AuditNewOrders property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AuditNewOrders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AuditNewOrders { get; }
        /// <summary> The AuditDeletedOrders property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AuditDeletedOrders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AuditDeletedOrders { get; }
        /// <summary> The CustomerKey property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."CustomerKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CustomerKey { get; }
        /// <summary> The UseParallelActionQueue property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."UseParallelActionQueue"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean UseParallelActionQueue { get; }
        /// <summary> The AllowEbayCombineLocally property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AllowEbayCombineLocally"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AllowEbayCombineLocally { get; }
        /// <summary> The ArchivalSettingsXml property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."ArchivalSettingsXml"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ArchivalSettingsXml { get; }
        /// <summary> The AuditEnabled property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AuditEnabled"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AuditEnabled { get; }
        /// <summary> The DefaultPickListTemplateID property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."DefaultPickListTemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> DefaultPickListTemplateID { get; }
        /// <summary> The AutoUpdateDayOfWeek property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AutoUpdateDayOfWeek"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DayOfWeek AutoUpdateDayOfWeek { get; }
        /// <summary> The AutoUpdateHourOfDay property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AutoUpdateHourOfDay"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 AutoUpdateHourOfDay { get; }
        /// <summary> The AutoUpdateStartDate property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."AutoUpdateStartDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime2, 7, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime AutoUpdateStartDate { get; }
        /// <summary> The WarehouseID property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."WarehouseID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String WarehouseID { get; }
        /// <summary> The WarehouseName property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."WarehouseName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String WarehouseName { get; }
        /// <summary> The LegacyCustomerKey property of the Entity Configuration<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Configuration"."LegacyCustomerKey"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String LegacyCustomerKey { get; }
        
        
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IConfigurationEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IConfigurationEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Configuration'. <br/><br/>
    /// 
    /// </summary>
    public partial class ConfigurationEntity : IConfigurationEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IConfigurationEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IConfigurationEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IConfigurationEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyConfigurationEntity(this, objectMap);
        }

        
    }
}
