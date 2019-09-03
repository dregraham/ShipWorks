﻿///////////////////////////////////////////////////////////////
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
    /// Read-only representation of the entity 'EndiciaProfile'. <br/><br/>
    /// 
    /// </summary>
    [Serializable]
    public partial class ReadOnlyEndiciaProfileEntity : IEndiciaProfileEntity
    {
        /// <summary>
        /// Constructor
        /// </summary>
        internal ReadOnlyEndiciaProfileEntity(IEndiciaProfileEntity source, IDictionary<object, object> objectMap)
        {
            MethodConditions.EnsureArgumentIsNotNull(source, nameof(source));
            MethodConditions.EnsureArgumentIsNotNull(objectMap, nameof(objectMap));

            if (objectMap.ContainsKey(source) && objectMap[source] == null)
            {
                objectMap[source] = this;
            }
            
            ShippingProfileID = source.ShippingProfileID;
            EndiciaAccountID = source.EndiciaAccountID;
            StealthPostage = source.StealthPostage;
            ReferenceID = source.ReferenceID;
            ScanBasedReturn = source.ScanBasedReturn;
            ReferenceID2 = source.ReferenceID2;
            ReferenceID3 = source.ReferenceID3;
            ReferenceID4 = source.ReferenceID4;
            GroupCode = source.GroupCode;
            
            PostalProfile = (IPostalProfileEntity) source.PostalProfile?.AsReadOnly(objectMap);
            
            

            CopyCustomEndiciaProfileData(source);
        }

        
        /// <summary> The ShippingProfileID property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ShippingProfileID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): false, true, false</remarks>
        public System.Int64 ShippingProfileID { get; }
        /// <summary> The EndiciaAccountID property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."EndiciaAccountID"<br/>
        /// Table field type characteristics (type, precision, scale, length): BigInt, 19, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Int64> EndiciaAccountID { get; }
        /// <summary> The StealthPostage property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."StealthPostage"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> StealthPostage { get; }
        /// <summary> The ReferenceID property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ReferenceID"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceID { get; }
        /// <summary> The ScanBasedReturn property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ScanBasedReturn"<br/>
        /// Table field type characteristics (type, precision, scale, length): Bit, 0, 0, 0<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public Nullable<System.Boolean> ScanBasedReturn { get; }
        /// <summary> The ReferenceID2 property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ReferenceID2"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceID2 { get; }
        /// <summary> The ReferenceID3 property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ReferenceID3"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceID3 { get; }
        /// <summary> The ReferenceID4 property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."ReferenceID4"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 300<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String ReferenceID4 { get; }
        /// <summary> The GroupCode property of the Entity EndiciaProfile<br/><br/>
        /// </summary>
        /// <remarks>Mapped on table field: "EndiciaProfile"."GroupCode"<br/>
        /// Table field type characteristics (type, precision, scale, length): NVarChar, 0, 0, 50<br/>
        /// Table field behavior characteristics (is nullable, is PK, is identity): true, false, false</remarks>
        public System.String GroupCode { get; }
        
        public IPostalProfileEntity PostalProfile { get; }
        
        
        
        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaProfileEntity AsReadOnly() => this;

        /// <summary>
        /// Get a read only version of the entity
        /// </summary>
        public virtual IEndiciaProfileEntity AsReadOnly(IDictionary<object, object> objectMap) => this;

        

        /// <summary>
        /// Copy any custom data
        /// </summary>
        partial void CopyCustomEndiciaProfileData(IEndiciaProfileEntity source);
    }
}
