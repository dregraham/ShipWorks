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
    /// Entity interface which represents the entity 'PrintResult'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyPrintResultEntity : IPrintResultEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyPrintResultEntity(IPrintResultEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            PrintResultID = source.PrintResultID;
            RowVersion = source.RowVersion;
            JobIdentifier = source.JobIdentifier;
            RelatedObjectID = source.RelatedObjectID;
            ContextObjectID = source.ContextObjectID;
            TemplateID = source.TemplateID;
            TemplateType = source.TemplateType;
            OutputFormat = source.OutputFormat;
            LabelSheetID = source.LabelSheetID;
            ComputerID = source.ComputerID;
            ContentResourceID = source.ContentResourceID;
            PrintDate = source.PrintDate;
            PrinterName = source.PrinterName;
            PaperSource = source.PaperSource;
            PaperSourceName = source.PaperSourceName;
            Copies = source.Copies;
            Collated = source.Collated;
            PageMarginLeft = source.PageMarginLeft;
            PageMarginRight = source.PageMarginRight;
            PageMarginBottom = source.PageMarginBottom;
            PageMarginTop = source.PageMarginTop;
            PageWidth = source.PageWidth;
            PageHeight = source.PageHeight;
            
            
            

            CopyCustomPrintResultData(source);
        }

        
        /// <summary> The PrintResultID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PrintResultID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 PrintResultID { get; }
        /// <summary> The RowVersion property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The JobIdentifier property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."JobIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Guid JobIdentifier { get; }
        /// <summary> The RelatedObjectID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."RelatedObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 RelatedObjectID { get; }
        /// <summary> The ContextObjectID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."ContextObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ContextObjectID { get; }
        /// <summary> The TemplateID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> TemplateID { get; }
        /// <summary> The TemplateType property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."TemplateType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> TemplateType { get; }
        /// <summary> The OutputFormat property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."OutputFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> OutputFormat { get; }
        /// <summary> The LabelSheetID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."LabelSheetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> LabelSheetID { get; }
        /// <summary> The ComputerID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ComputerID { get; }
        /// <summary> The ContentResourceID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."ContentResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 ContentResourceID { get; }
        /// <summary> The PrintDate property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PrintDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime PrintDate { get; }
        /// <summary> The PrinterName property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PrinterName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PrinterName { get; }
        /// <summary> The PaperSource property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PaperSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 PaperSource { get; }
        /// <summary> The PaperSourceName property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PaperSourceName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String PaperSourceName { get; }
        /// <summary> The Copies property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."Copies"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Copies { get; }
        /// <summary> The Collated property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."Collated"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Boolean Collated { get; }
        /// <summary> The PageMarginLeft property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginLeft"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginLeft { get; }
        /// <summary> The PageMarginRight property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginRight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginRight { get; }
        /// <summary> The PageMarginBottom property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginBottom"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginBottom { get; }
        /// <summary> The PageMarginTop property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginTop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageMarginTop { get; }
        /// <summary> The PageWidth property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageWidth { get; }
        /// <summary> The PageHeight property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Double PageHeight { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPrintResultEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPrintResultEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomPrintResultData(IPrintResultEntity source);
    }
}
