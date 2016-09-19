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
    /// Entity interface which represents the entity 'Template'. <br/><br/>
    /// 
    /// </summary>
    public partial interface ITemplateEntity
    {
        
        /// <summary> The TemplateID property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 TemplateID { get; }
        /// <summary> The RowVersion property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ParentFolderID property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."ParentFolderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ParentFolderID { get; }
        /// <summary> The Name property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Name { get; }
        /// <summary> The Xsl property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Xsl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Xsl { get; }
        /// <summary> The Type property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Type"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Type { get; }
        /// <summary> The Context property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Context"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Context { get; }
        /// <summary> The OutputFormat property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."OutputFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 OutputFormat { get; }
        /// <summary> The OutputEncoding property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."OutputEncoding"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String OutputEncoding { get; }
        /// <summary> The PageMarginLeft property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginLeft"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginLeft { get; }
        /// <summary> The PageMarginRight property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginRight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginRight { get; }
        /// <summary> The PageMarginBottom property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginBottom"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginBottom { get; }
        /// <summary> The PageMarginTop property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginTop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginTop { get; }
        /// <summary> The PageWidth property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageWidth { get; }
        /// <summary> The PageHeight property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageHeight { get; }
        /// <summary> The LabelSheetID property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."LabelSheetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 LabelSheetID { get; }
        /// <summary> The PrintCopies property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PrintCopies"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PrintCopies { get; }
        /// <summary> The PrintCollate property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PrintCollate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean PrintCollate { get; }
        /// <summary> The SaveFileName property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SaveFileName { get; }
        /// <summary> The SaveFileFolder property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SaveFileFolder { get; }
        /// <summary> The SaveFilePrompt property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFilePrompt"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SaveFilePrompt { get; }
        /// <summary> The SaveFileBOM property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileBOM"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SaveFileBOM { get; }
        /// <summary> The SaveFileOnlineResources property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileOnlineResources"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean SaveFileOnlineResources { get; }
        
        
        ITemplateFolderEntity ParentFolder { get; }
        
        IEnumerable<ITemplateComputerSettingsEntity> ComputerSettings { get; }
        IEnumerable<ITemplateStoreSettingsEntity> StoreSettings { get; }
        IEnumerable<ITemplateUserSettingsEntity> UserSettings { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        ITemplateEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'Template'. <br/><br/>
    /// 
    /// </summary>
    public partial class TemplateEntity : ITemplateEntity
    {
        
        ITemplateFolderEntity ITemplateEntity.ParentFolder => ParentFolder;
        
        IEnumerable<ITemplateComputerSettingsEntity> ITemplateEntity.ComputerSettings => ComputerSettings;
        IEnumerable<ITemplateStoreSettingsEntity> ITemplateEntity.StoreSettings => StoreSettings;
        IEnumerable<ITemplateUserSettingsEntity> ITemplateEntity.UserSettings => UserSettings;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual ITemplateEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (ITemplateEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyTemplateEntity(this, objectMap);
        }
    }
}
