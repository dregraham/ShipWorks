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
    /// Entity interface which represents the entity 'TemplateStoreSettings'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyTemplateStoreSettingsEntity : ITemplateStoreSettingsEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyTemplateStoreSettingsEntity(ITemplateStoreSettingsEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            TemplateStoreSettingsID = source.TemplateStoreSettingsID;
            TemplateID = source.TemplateID;
            StoreID = source.StoreID;
            EmailUseDefault = source.EmailUseDefault;
            EmailAccountID = source.EmailAccountID;
            EmailTo = source.EmailTo;
            EmailCc = source.EmailCc;
            EmailBcc = source.EmailBcc;
            EmailSubject = source.EmailSubject;
            
            
            Template = source.Template?.AsReadOnly(objectMap);
            

            CopyCustomTemplateStoreSettingsData(source);
        }

        
        /// <summary> The TemplateStoreSettingsID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."TemplateStoreSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 TemplateStoreSettingsID { get; }
        /// <summary> The TemplateID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 TemplateID { get; }
        /// <summary> The StoreID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> StoreID { get; }
        /// <summary> The EmailUseDefault property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailUseDefault"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean EmailUseDefault { get; }
        /// <summary> The EmailAccountID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 EmailAccountID { get; }
        /// <summary> The EmailTo property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailTo { get; }
        /// <summary> The EmailCc property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailCc"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailCc { get; }
        /// <summary> The EmailBcc property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailBcc"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailBcc { get; }
        /// <summary> The EmailSubject property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailSubject"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String EmailSubject { get; }
        
        
        public ITemplateEntity Template { get; }
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateStoreSettingsEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateStoreSettingsEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomTemplateStoreSettingsData(ITemplateStoreSettingsEntity source);
    }
}
