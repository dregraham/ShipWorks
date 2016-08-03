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
    /// Read-only representation of the entity 'AuditChangeDetail'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyAuditChangeDetailEntity : IAuditChangeDetailEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyAuditChangeDetailEntity(IAuditChangeDetailEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            AuditChangeDetailID = source.AuditChangeDetailID;
            AuditChangeID = source.AuditChangeID;
            AuditID = source.AuditID;
            DisplayName = source.DisplayName;
            DisplayFormat = source.DisplayFormat;
            DataType = source.DataType;
            TextOld = source.TextOld;
            TextNew = source.TextNew;
            VariantOld = source.VariantOld;
            VariantNew = source.VariantNew;
            
            
            

            CopyCustomAuditChangeDetailData(source);
        }

        
        /// <summary> The AuditChangeDetailID property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."AuditChangeDetailID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 AuditChangeDetailID { get; }
        /// <summary> The AuditChangeID property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."AuditChangeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AuditChangeID { get; }
        /// <summary> The AuditID property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."AuditID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AuditID { get; }
        /// <summary> The DisplayName property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."DisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String DisplayName { get; }
        /// <summary> The DisplayFormat property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."DisplayFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte DisplayFormat { get; }
        /// <summary> The DataType property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."DataType"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte DataType { get; }
        /// <summary> The TextOld property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."TextOld"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String TextOld { get; }
        /// <summary> The TextNew property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."TextNew"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String TextNew { get; }
        /// <summary> The VariantOld property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."VariantOld"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.Object VariantOld { get; }
        /// <summary> The VariantNew property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."VariantNew"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.Object VariantNew { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditChangeDetailEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditChangeDetailEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomAuditChangeDetailData(IAuditChangeDetailEntity source);
    }
}
