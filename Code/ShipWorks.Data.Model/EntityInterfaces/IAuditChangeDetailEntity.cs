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
    /// Entity interface which represents the entity 'AuditChangeDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IAuditChangeDetailEntity
    {
        
        /// <summary> The AuditChangeDetailID property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."AuditChangeDetailID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 AuditChangeDetailID { get; }
        /// <summary> The AuditChangeID property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."AuditChangeID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 AuditChangeID { get; }
        /// <summary> The AuditID property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."AuditID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 AuditID { get; }
        /// <summary> The DisplayName property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."DisplayName"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String DisplayName { get; }
        /// <summary> The DisplayFormat property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."DisplayFormat"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte DisplayFormat { get; }
        /// <summary> The DataType property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."DataType"<br/>
        /// Table field type characteristics (type, precision, scale, length): TinyInt, 3, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte DataType { get; }
        /// <summary> The TextOld property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."TextOld"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String TextOld { get; }
        /// <summary> The TextNew property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."TextNew"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String TextNew { get; }
        /// <summary> The VariantOld property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."VariantOld"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Object VariantOld { get; }
        /// <summary> The VariantNew property of the Entity AuditChangeDetail<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "AuditChangeDetail"."VariantNew"<br/>
        /// Table field type characteristics (type, precision, scale, length): Variant, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.Object VariantNew { get; }
        
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAuditChangeDetailEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IAuditChangeDetailEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'AuditChangeDetail'. <br/><br/>
    /// 
    /// </summary>
    public partial class AuditChangeDetailEntity : IAuditChangeDetailEntity
    {
        
        

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IAuditChangeDetailEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IAuditChangeDetailEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IAuditChangeDetailEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyAuditChangeDetailEntity(this, objectMap);
        }
    }
}
