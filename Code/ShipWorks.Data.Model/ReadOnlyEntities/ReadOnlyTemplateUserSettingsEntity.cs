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
    /// Entity interface which represents the entity 'TemplateUserSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyTemplateUserSettingsEntity : ITemplateUserSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyTemplateUserSettingsEntity(ITemplateUserSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            TemplateUserSettingsID = source.TemplateUserSettingsID;
            TemplateID = source.TemplateID;
            UserID = source.UserID;
            PreviewSource = source.PreviewSource;
            PreviewCount = source.PreviewCount;
            PreviewFilterNodeID = source.PreviewFilterNodeID;
            PreviewZoom = source.PreviewZoom;
            
            
            Template = source.Template?.AsReadOnly(objectMap);
            

            CopyCustomTemplateUserSettingsData(source);
        }

        
        /// <summary> The TemplateUserSettingsID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."TemplateUserSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 TemplateUserSettingsID { get; }
        /// <summary> The TemplateID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TemplateID { get; }
        /// <summary> The UserID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."UserID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 UserID { get; }
        /// <summary> The PreviewSource property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PreviewSource { get; }
        /// <summary> The PreviewCount property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PreviewCount { get; }
        /// <summary> The PreviewFilterNodeID property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewFilterNodeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> PreviewFilterNodeID { get; }
        /// <summary> The PreviewZoom property of the Entity TemplateUserSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateUserSettings"."PreviewZoom"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 10<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PreviewZoom { get; }
        
        
        public ITemplateEntity Template { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateUserSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateUserSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomTemplateUserSettingsData(ITemplateUserSettingsEntity source);
    }
}
