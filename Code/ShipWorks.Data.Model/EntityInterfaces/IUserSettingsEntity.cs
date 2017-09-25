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
    /// Entity interface which represents the entity 'UserSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IUserSettingsEntity
    {
        
        /// <summary> The UserID property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The DisplayColorScheme property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."DisplayColorScheme"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 DisplayColorScheme { get; }
        /// <summary> The DisplaySystemTray property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."DisplaySystemTray"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean DisplaySystemTray { get; }
        /// <summary> The WindowLayout property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."WindowLayout"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] WindowLayout { get; }
        /// <summary> The GridMenuLayout property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."GridMenuLayout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String GridMenuLayout { get; }
        /// <summary> The FilterInitialUseLastActive property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."FilterInitialUseLastActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean FilterInitialUseLastActive { get; }
        /// <summary> The FilterInitialSpecified property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."FilterInitialSpecified"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 FilterInitialSpecified { get; }
        /// <summary> The FilterInitialSortType property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."FilterInitialSortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 FilterInitialSortType { get; }
        /// <summary> The OrderFilterLastActive property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."OrderFilterLastActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 OrderFilterLastActive { get; }
        /// <summary> The OrderFilterExpandedFolders property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."OrderFilterExpandedFolders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String OrderFilterExpandedFolders { get; }
        /// <summary> The ShippingWeightFormat property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."ShippingWeightFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 ShippingWeightFormat { get; }
        /// <summary> The TemplateExpandedFolders property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."TemplateExpandedFolders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String TemplateExpandedFolders { get; }
        /// <summary> The TemplateLastSelected property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."TemplateLastSelected"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TemplateLastSelected { get; }
        /// <summary> The CustomerFilterLastActive property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."CustomerFilterLastActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 CustomerFilterLastActive { get; }
        /// <summary> The CustomerFilterExpandedFolders property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."CustomerFilterExpandedFolders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String CustomerFilterExpandedFolders { get; }
        /// <summary> The NextGlobalPostNotificationDate property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."NextGlobalPostNotificationDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime NextGlobalPostNotificationDate { get; }
        /// <summary> The SingleScanSettings property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."SingleScanSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SingleScanSettings { get; }
        /// <summary> The AutoWeigh property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."AutoWeigh"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean AutoWeigh { get; }
        /// <summary> The DialogSettings property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."DialogSettings"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String DialogSettings { get; }
        
        IUserEntity User { get; }
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserSettingsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IUserSettingsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'UserSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial class UserSettingsEntity : IUserSettingsEntity
    {
        IUserEntity IUserSettingsEntity.User => User;
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserSettingsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IUserSettingsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IUserSettingsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyUserSettingsEntity(this, objectMap);
        }
    }
}
