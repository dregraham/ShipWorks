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
    /// Entity interface which represents the entity 'TemplateUserSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ITemplateUserSettingsEntity
    {
        
        /// <summary> The TemplateUserSettingsID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."TemplateUserSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 TemplateUserSettingsID { get; }
        /// <summary> The TemplateID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TemplateID { get; }
        /// <summary> The UserID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 UserID { get; }
        /// <summary> The PreviewSource property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PreviewSource { get; }
        /// <summary> The PreviewCount property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PreviewCount { get; }
        /// <summary> The PreviewFilterNodeID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> PreviewFilterNodeID { get; }
        /// <summary> The PreviewZoom property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewZoom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PreviewZoom { get; }
        
        
        ITemplateEntity Template { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateUserSettingsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateUserSettingsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'TemplateUserSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial class TemplateUserSettingsEntity : ITemplateUserSettingsEntity
    {
        
        ITemplateEntity ITemplateUserSettingsEntity.Template => Template;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateUserSettingsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ITemplateUserSettingsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ITemplateUserSettingsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyTemplateUserSettingsEntity(this, objectMap);
        }

        
    }
}
