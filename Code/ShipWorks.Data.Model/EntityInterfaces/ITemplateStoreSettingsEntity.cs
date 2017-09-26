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
    /// Entity interface which represents the entity 'TemplateStoreSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ITemplateStoreSettingsEntity
    {
        
        /// <summary> The TemplateStoreSettingsID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."TemplateStoreSettingsID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 TemplateStoreSettingsID { get; }
        /// <summary> The TemplateID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 TemplateID { get; }
        /// <summary> The StoreID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."StoreID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> StoreID { get; }
        /// <summary> The EmailUseDefault property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailUseDefault"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean EmailUseDefault { get; }
        /// <summary> The EmailAccountID property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 EmailAccountID { get; }
        /// <summary> The EmailTo property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailTo"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailTo { get; }
        /// <summary> The EmailCc property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailCc"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailCc { get; }
        /// <summary> The EmailBcc property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailBcc"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailBcc { get; }
        /// <summary> The EmailSubject property of the Entity TemplateStoreSettings<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "TemplateStoreSettings"."EmailSubject"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String EmailSubject { get; }
        
        
        ITemplateEntity Template { get; }
        

        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateStoreSettingsEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateStoreSettingsEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'TemplateStoreSettings'. <br/><br/>
    /// 
    /// </summary>
    public partial class TemplateStoreSettingsEntity : ITemplateStoreSettingsEntity
    {
        
        ITemplateEntity ITemplateStoreSettingsEntity.Template => Template;
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateStoreSettingsEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ITemplateStoreSettingsEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ITemplateStoreSettingsEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyTemplateStoreSettingsEntity(this, objectMap);
        }

        
    }
}
