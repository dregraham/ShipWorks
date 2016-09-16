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
    /// Entity interface which represents the entity 'PrintResult'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IPrintResultEntity
    {
        
        /// <summary> The PrintResultID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PrintResultID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 PrintResultID { get; }
        /// <summary> The RowVersion property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The JobIdentifier property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."JobIdentifier"<br/>
        /// Table field type characteristics (type, precision, scale, length): UniqueIdentifier, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Guid JobIdentifier { get; }
        /// <summary> The RelatedObjectID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."RelatedObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 RelatedObjectID { get; }
        /// <summary> The ContextObjectID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."ContextObjectID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ContextObjectID { get; }
        /// <summary> The TemplateID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> TemplateID { get; }
        /// <summary> The TemplateType property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."TemplateType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> TemplateType { get; }
        /// <summary> The OutputFormat property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."OutputFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> OutputFormat { get; }
        /// <summary> The LabelSheetID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."LabelSheetID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> LabelSheetID { get; }
        /// <summary> The ComputerID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."ComputerID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ComputerID { get; }
        /// <summary> The ContentResourceID property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."ContentResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 ContentResourceID { get; }
        /// <summary> The PrintDate property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PrintDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime PrintDate { get; }
        /// <summary> The PrinterName property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PrinterName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 350<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PrinterName { get; }
        /// <summary> The PaperSource property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PaperSource"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 PaperSource { get; }
        /// <summary> The PaperSourceName property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PaperSourceName"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 100<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String PaperSourceName { get; }
        /// <summary> The Copies property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."Copies"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Copies { get; }
        /// <summary> The Collated property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."Collated"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Boolean Collated { get; }
        /// <summary> The PageMarginLeft property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginLeft"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginLeft { get; }
        /// <summary> The PageMarginRight property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginRight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginRight { get; }
        /// <summary> The PageMarginBottom property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginBottom"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginBottom { get; }
        /// <summary> The PageMarginTop property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageMarginTop"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageMarginTop { get; }
        /// <summary> The PageWidth property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageWidth"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageWidth { get; }
        /// <summary> The PageHeight property of the Entity PrintResult<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "PrintResult"."PageHeight"<br/>
        /// Table field type characteristics (type, precision, scale, length): Float, 38, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Double PageHeight { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPrintResultEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IPrintResultEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'PrintResult'. <br/><br/>
    /// 
    /// </summary>
    public partial class PrintResultEntity : IPrintResultEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IPrintResultEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IPrintResultEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IPrintResultEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyPrintResultEntity(this, objectMap);
        }
    }
}
