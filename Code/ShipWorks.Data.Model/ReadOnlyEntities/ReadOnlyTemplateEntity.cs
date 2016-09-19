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
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Data.Model.ReadOnlyEntityClasses
{
    /// <summary>
    /// Read-only representation of the entity 'Template'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyTemplateEntity : ITemplateEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyTemplateEntity(ITemplateEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            TemplateID = source.TemplateID;
            RowVersion = source.RowVersion;
            ParentFolderID = source.ParentFolderID;
            Name = source.Name;
            Xsl = source.Xsl;
            Type = source.Type;
            Context = source.Context;
            OutputFormat = source.OutputFormat;
            OutputEncoding = source.OutputEncoding;
            PageMarginLeft = source.PageMarginLeft;
            PageMarginRight = source.PageMarginRight;
            PageMarginBottom = source.PageMarginBottom;
            PageMarginTop = source.PageMarginTop;
            PageWidth = source.PageWidth;
            PageHeight = source.PageHeight;
            LabelSheetID = source.LabelSheetID;
            PrintCopies = source.PrintCopies;
            PrintCollate = source.PrintCollate;
            SaveFileName = source.SaveFileName;
            SaveFileFolder = source.SaveFileFolder;
            SaveFilePrompt = source.SaveFilePrompt;
            SaveFileBOM = source.SaveFileBOM;
            SaveFileOnlineResources = source.SaveFileOnlineResources;
            
            
            ParentFolder = source.ParentFolder?.AsReadOnly(objectMap);
            
            ComputerSettings = source.ComputerSettings?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<ITemplateComputerSettingsEntity>();
            StoreSettings = source.StoreSettings?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<ITemplateStoreSettingsEntity>();
            UserSettings = source.UserSettings?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<ITemplateUserSettingsEntity>();

            CopyCustomTemplateData(source);
        }

        
        /// <summary> The TemplateID property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 TemplateID { get; }
        /// <summary> The RowVersion property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ParentFolderID property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."ParentFolderID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ParentFolderID { get; }
        /// <summary> The Name property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Name"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Name { get; }
        /// <summary> The Xsl property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Xsl"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Xsl { get; }
        /// <summary> The Type property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Type"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Type { get; }
        /// <summary> The Context property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."Context"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Context { get; }
        /// <summary> The OutputFormat property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."OutputFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 OutputFormat { get; }
        /// <summary> The OutputEncoding property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."OutputEncoding"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String OutputEncoding { get; }
        /// <summary> The PageMarginLeft property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginLeft"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginLeft { get; }
        /// <summary> The PageMarginRight property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginRight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginRight { get; }
        /// <summary> The PageMarginBottom property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginBottom"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginBottom { get; }
        /// <summary> The PageMarginTop property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageMarginTop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginTop { get; }
        /// <summary> The PageWidth property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageWidth { get; }
        /// <summary> The PageHeight property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PageHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageHeight { get; }
        /// <summary> The LabelSheetID property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."LabelSheetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 LabelSheetID { get; }
        /// <summary> The PrintCopies property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PrintCopies"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PrintCopies { get; }
        /// <summary> The PrintCollate property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."PrintCollate"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean PrintCollate { get; }
        /// <summary> The SaveFileName property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SaveFileName { get; }
        /// <summary> The SaveFileFolder property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileFolder"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 500<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SaveFileFolder { get; }
        /// <summary> The SaveFilePrompt property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFilePrompt"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SaveFilePrompt { get; }
        /// <summary> The SaveFileBOM property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileBOM"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaveFileBOM { get; }
        /// <summary> The SaveFileOnlineResources property of the Entity Template<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "Template"."SaveFileOnlineResources"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean SaveFileOnlineResources { get; }
        
        
        public ITemplateFolderEntity ParentFolder { get; }
        
        
        public IEnumerable<ITemplateComputerSettingsEntity> ComputerSettings { get; }
        
        public IEnumerable<ITemplateStoreSettingsEntity> StoreSettings { get; }
        
        public IEnumerable<ITemplateUserSettingsEntity> UserSettings { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual ITemplateEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomTemplateData(ITemplateEntity source);
    }
}
