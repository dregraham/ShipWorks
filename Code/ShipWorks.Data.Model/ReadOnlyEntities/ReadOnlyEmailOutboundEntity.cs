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
    /// Entity interface which represents the entity 'EmailOutbound'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEmailOutboundEntity : IEmailOutboundEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEmailOutboundEntity(IEmailOutboundEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            EmailOutboundID = source.EmailOutboundID;
            RowVersion = source.RowVersion;
            ContextID = source.ContextID;
            ContextType = source.ContextType;
            TemplateID = source.TemplateID;
            AccountID = source.AccountID;
            Visibility = source.Visibility;
            FromAddress = source.FromAddress;
            ToList = source.ToList;
            CcList = source.CcList;
            BccList = source.BccList;
            Subject = source.Subject;
            HtmlPartResourceID = source.HtmlPartResourceID;
            PlainPartResourceID = source.PlainPartResourceID;
            Encoding = source.Encoding;
            ComposedDate = source.ComposedDate;
            SentDate = source.SentDate;
            DontSendBefore = source.DontSendBefore;
            SendStatus = source.SendStatus;
            SendAttemptCount = source.SendAttemptCount;
            SendAttemptLastError = source.SendAttemptLastError;
            
            
            
            RelatedObjects = source.RelatedObjects?.Select(x => x.AsReadOnly(objectMap)).ToReadOnly() ??
                Enumerable.Empty<IEmailOutboundRelationEntity>();

            CopyCustomEmailOutboundData(source);
        }

        
        /// <summary> The EmailOutboundID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."EmailOutboundID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, true</remarks>
        public System.Int64 EmailOutboundID { get; }
        /// <summary> The RowVersion property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."RowVersion"<br/>
        /// Table field type characteristics (type, precision, scale, length): Timestamp, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Byte[] RowVersion { get; }
        /// <summary> The ContextID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ContextID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> ContextID { get; }
        /// <summary> The ContextType property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ContextType"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int32> ContextType { get; }
        /// <summary> The TemplateID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."TemplateID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> TemplateID { get; }
        /// <summary> The AccountID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."AccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 AccountID { get; }
        /// <summary> The Visibility property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."Visibility"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 Visibility { get; }
        /// <summary> The FromAddress property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."FromAddress"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 200<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String FromAddress { get; }
        /// <summary> The ToList property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ToList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String ToList { get; }
        /// <summary> The CcList property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."CcList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String CcList { get; }
        /// <summary> The BccList property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."BccList"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 2147483647<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String BccList { get; }
        /// <summary> The Subject property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."Subject"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String Subject { get; }
        /// <summary> The HtmlPartResourceID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."HtmlPartResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> HtmlPartResourceID { get; }
        /// <summary> The PlainPartResourceID property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."PlainPartResourceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int64 PlainPartResourceID { get; }
        /// <summary> The Encoding property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."Encoding"<br/>
        /// Table field type characteristics (type, precision, scale, length): VarChar, 0, 0, 20<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String Encoding { get; }
        /// <summary> The ComposedDate property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."ComposedDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime ComposedDate { get; }
        /// <summary> The SentDate property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SentDate"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.DateTime SentDate { get; }
        /// <summary> The DontSendBefore property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."DontSendBefore"<br/>
        /// Table field type characteristics (type, precision, scale, length): DateTime, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.DateTime> DontSendBefore { get; }
        /// <summary> The SendStatus property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SendStatus"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SendStatus { get; }
        /// <summary> The SendAttemptCount property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SendAttemptCount"<br/>
        /// Table field type characteristics (type, precision, scale, length): Int, 10, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.Int32 SendAttemptCount { get; }
        /// <summary> The SendAttemptLastError property of the Entity EmailOutbound<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EmailOutbound"."SendAttemptLastError"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, false, false</remarks>
        public System.String SendAttemptLastError { get; }
        
        
        
        public IEnumerable<IEmailOutboundRelationEntity> RelatedObjects { get; }
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailOutboundEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEmailOutboundEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEmailOutboundData(IEmailOutboundEntity source);
    }
}
