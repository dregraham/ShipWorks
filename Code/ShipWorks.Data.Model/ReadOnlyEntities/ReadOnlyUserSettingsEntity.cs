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
    /// Entity interface which represents the entity 'UserSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyUserSettingsEntity : IUserSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyUserSettingsEntity(IUserSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            UserID = source.UserID;
            DisplayColorScheme = source.DisplayColorScheme;
            DisplaySystemTray = source.DisplaySystemTray;
            WindowLayout = source.WindowLayout;
            GridMenuLayout = source.GridMenuLayout;
            FilterInitialUseLastActive = source.FilterInitialUseLastActive;
            FilterInitialSpecified = source.FilterInitialSpecified;
            FilterInitialSortType = source.FilterInitialSortType;
            OrderFilterLastActive = source.OrderFilterLastActive;
            OrderFilterExpandedFolders = source.OrderFilterExpandedFolders;
            ShippingWeightFormat = source.ShippingWeightFormat;
            TemplateExpandedFolders = source.TemplateExpandedFolders;
            TemplateLastSelected = source.TemplateLastSelected;
            CustomerFilterLastActive = source.CustomerFilterLastActive;
            CustomerFilterExpandedFolders = source.CustomerFilterExpandedFolders;
            
            User = source.User?.AsReadOnly(objectMap);
            
            

            CopyCustomUserSettingsData(source);
        }

        
        /// <summary> The UserID property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The DisplayColorScheme property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."DisplayColorScheme"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 DisplayColorScheme { get; }
        /// <summary> The DisplaySystemTray property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."DisplaySystemTray"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean DisplaySystemTray { get; }
        /// <summary> The WindowLayout property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."WindowLayout"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarBinary, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] WindowLayout { get; }
        /// <summary> The GridMenuLayout property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."GridMenuLayout"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String GridMenuLayout { get; }
        /// <summary> The FilterInitialUseLastActive property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."FilterInitialUseLastActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean FilterInitialUseLastActive { get; }
        /// <summary> The FilterInitialSpecified property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."FilterInitialSpecified"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 FilterInitialSpecified { get; }
        /// <summary> The FilterInitialSortType property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."FilterInitialSortType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 FilterInitialSortType { get; }
        /// <summary> The OrderFilterLastActive property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."OrderFilterLastActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 OrderFilterLastActive { get; }
        /// <summary> The OrderFilterExpandedFolders property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."OrderFilterExpandedFolders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String OrderFilterExpandedFolders { get; }
        /// <summary> The ShippingWeightFormat property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."ShippingWeightFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 ShippingWeightFormat { get; }
        /// <summary> The TemplateExpandedFolders property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."TemplateExpandedFolders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String TemplateExpandedFolders { get; }
        /// <summary> The TemplateLastSelected property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."TemplateLastSelected"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TemplateLastSelected { get; }
        /// <summary> The CustomerFilterLastActive property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."CustomerFilterLastActive"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 CustomerFilterLastActive { get; }
        /// <summary> The CustomerFilterExpandedFolders property of the Entity UserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "UserSettings"."CustomerFilterExpandedFolders"<br/>
        /// Table field type characteristics (type, precision, scale, length): Xml, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String CustomerFilterExpandedFolders { get; }
        
        public IUserEntity User { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IUserSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomUserSettingsData(IUserSettingsEntity source);
    }
}
