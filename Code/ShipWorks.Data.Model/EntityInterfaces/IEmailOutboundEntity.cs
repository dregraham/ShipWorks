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
    /// Entity interface which represents the entity 'EmailOutbound'. <br/><br/>
    /// 
    /// </summary>
    public partial interface IEmailOutboundEntity
    {
        
        /// <summary> The EmailOutboundID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."EmailOutboundID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        System.Int64 EmailOutboundID { get; }
        /// <summary> The RowVersion property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Byte[] RowVersion { get; }
        /// <summary> The ContextID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ContextID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> ContextID { get; }
        /// <summary> The ContextType property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ContextType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int32> ContextType { get; }
        /// <summary> The TemplateID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> TemplateID { get; }
        /// <summary> The AccountID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."AccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 AccountID { get; }
        /// <summary> The Visibility property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."Visibility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 Visibility { get; }
        /// <summary> The FromAddress property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."FromAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String FromAddress { get; }
        /// <summary> The ToList property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ToList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String ToList { get; }
        /// <summary> The CcList property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."CcList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String CcList { get; }
        /// <summary> The BccList property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."BccList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String BccList { get; }
        /// <summary> The Subject property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."Subject"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String Subject { get; }
        /// <summary> The HtmlPartResourceID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."HtmlPartResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.Int64> HtmlPartResourceID { get; }
        /// <summary> The PlainPartResourceID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."PlainPartResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int64 PlainPartResourceID { get; }
        /// <summary> The Encoding property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."Encoding"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        System.String Encoding { get; }
        /// <summary> The ComposedDate property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ComposedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime ComposedDate { get; }
        /// <summary> The SentDate property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SentDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.DateTime SentDate { get; }
        /// <summary> The DontSendBefore property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."DontSendBefore"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        Nullable<System.DateTime> DontSendBefore { get; }
        /// <summary> The SendStatus property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SendStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SendStatus { get; }
        /// <summary> The SendAttemptCount property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SendAttemptCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.Int32 SendAttemptCount { get; }
        /// <summary> The SendAttemptLastError property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SendAttemptLastError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        System.String SendAttemptLastError { get; }
        
        
        
        IEnumerable<IEmailOutboundRelationEntity> RelatedObjects { get; }

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEmailOutboundEntity AsReadOnly();

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        IEmailOutboundEntity AsReadOnly(IDictionary<object, object> objectMap);
    }
}

namespace ShipWorks.Data.Model.EntityClasses
{
    using ShipWorks.Data.Model.EntityInterfaces;
    using ShipWorks.Data.Model.ReadOnlyEntityClasses;

    /// <summary>
    /// Entity interface which represents the entity 'EmailOutbound'. <br/><br/>
    /// 
    /// </summary>
    public partial class EmailOutboundEntity : IEmailOutboundEntity
    {
        
        
        IEnumerable<IEmailOutboundRelationEntity> IEmailOutboundEntity.RelatedObjects => RelatedObjects;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailOutboundEntity AsReadOnly() =>
            AsReadOnly(new Dictionary<object, object>());

        /// <summary>
        /// Get a read only version of the entity that handles cyclic references
        /// </summary>
        public virtual IEmailOutboundEntity AsReadOnly(IDictionary<object, object> objectMap)
        {
            if (objectMap.ContainsKey(this))
            {
                return (IEmailOutboundEntity) objectMap[this];
            }

            objectMap.Add(this, null);

            return new ReadOnlyEmailOutboundEntity(this, objectMap);
        }
    }
}
